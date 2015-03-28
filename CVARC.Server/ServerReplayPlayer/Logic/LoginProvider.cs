using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ServerReplayPlayer.Contracts;
using ServerReplayPlayer.Logic.Cryptographer;

namespace ServerReplayPlayer.Logic
{
    public class LoginProvider
    {
        private const string KeyName = "rtsToken";
        private static readonly string LoginsFile = Helpers.GetServerPath("settings\\logins.txt");
        private static readonly TokenSerializer TokenSerializer = new TokenSerializer();
        private static ConcurrentDictionary<string, CommandEntity> commands;
        private static ConcurrentDictionary<string, CommandEntity> Commands
        {
            get
            {
                return commands ?? (commands = new ConcurrentDictionary<string, CommandEntity>(File.ReadAllLines(LoginsFile).Select(x =>
                           {
                               var splits = x.Split(';');
                               return new KeyValuePair<string, CommandEntity>(splits[0], new CommandEntity
                               {
                                   CommandName = splits[0],
                                   Password = splits[1],
                                   Email = splits[2],
                                   Participants = splits[3].Split(','),
                                   IsAdmin = splits[4] == "true"
                               });
                           })));
            }
        }

        public CommandEntity Auth(HttpRequestBase request)
        {
            var commandName = request.Cookies[KeyName] == null ? null : TokenSerializer.Deserialize(request.Cookies[KeyName].Value);
            CommandEntity command;
            if (!string.IsNullOrEmpty(commandName) && Commands.TryGetValue(commandName, out command))
                return command;
            return null;
        }

        public bool TryLogin(HttpResponseBase response, string commandName, string password)
        {
            if (Commands.Values.Any(x => x.Password == password && x.CommandName == commandName))
            {
                response.Cookies.Add(new HttpCookie(KeyName, TokenSerializer.Serialize(commandName)));
                return true;
            }
            return false;
        }

        public void Logout(HttpResponseBase response)
        {
            response.Cookies.Add(new HttpCookie(KeyName)
            {
                Expires = DateTime.Now.AddDays(-1)
            });
        }
    }
}
