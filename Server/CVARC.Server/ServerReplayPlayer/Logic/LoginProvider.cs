using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic.FileIO;
using ServerReplayPlayer.Contracts;
using ServerReplayPlayer.Logic.Cryptographer;

namespace ServerReplayPlayer.Logic
{
    public class LoginProvider
    {
        private const string KeyName = "rtsToken";
        private static readonly string LoginsFile = SettingsProvider.GetSettingsFilePath("logins.csv");
        private static readonly TokenSerializer TokenSerializer = new TokenSerializer();
        private static ConcurrentDictionary<string, CommandEntity> commands;
        private static ConcurrentDictionary<string, CommandEntity> Commands
        {
            get
            {
                return commands ?? (commands = new ConcurrentDictionary<string, CommandEntity>(
                    ReadCommands().Select(x => new KeyValuePair<string, CommandEntity>(x.Email, x))));
            }
        }

        private static IEnumerable<CommandEntity> ReadCommands()
        {
            using (var parser = new TextFieldParser(LoginsFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();
                    if (fields == null)
                        yield break;
                    yield return new CommandEntity
                        {
                            CommandName = fields[1],
                            Captain = fields[3],
                            Email = fields[6],
                            Participants = new[] {fields[4], fields[5], fields[7]}.Where(y => !string.IsNullOrEmpty(y)).ToArray(),
                            Password = fields[8],
                            IsAdmin = fields.Length > 9 && fields[9] == "true"
                        };
                }
            }
        }

        public CommandEntity Auth(HttpRequestBase request)
        {
            var login = request.Cookies[KeyName] == null ? null : TokenSerializer.Deserialize(request.Cookies[KeyName].Value);
            CommandEntity command;
            if (!string.IsNullOrEmpty(login) && Commands.TryGetValue(login, out command))
                return command;
            return null;
        }

        public bool TryLogin(HttpResponseBase response, string login, string password)
        {
            if (Commands.Values.Any(x => x.Password == password && (x.Email == login || x.CommandName == login)))
            {
                response.Cookies.Add(new HttpCookie(KeyName, TokenSerializer.Serialize(login)));
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

        public void AddLogins(HttpPostedFileBase file)
        {
            file.SaveAs(LoginsFile);
            commands = null;
        }
    }
}
