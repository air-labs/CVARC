using System;
using CVARC.Basic;
using CVARC.Basic.Core.Participants;

namespace CVARC.Network
{
    internal static class SettingsProvider
    {
        public static CompetitionsSettings Get(string[] args)
        {
            var multiplayer = bool.Parse(args[0]);
            var package = ParseHelloPackage(multiplayer, args);
            var server = new ParticipantsServer("Fall2013.0.dll");
            var participants = multiplayer ? GetParticipants(server, package) : GetMultiplayerParticipants(server, package);
            return new CompetitionsSettings
            {
                Participants = participants,
                Competitions = server.Competitions
            };
        }

        private static HelloPackage ParseHelloPackage(bool multiplayer, string[] args)
        {
            return new HelloPackage
            {
                LevelName = args[1],
                MapSeed = int.Parse(args[2]),
                Opponent = multiplayer ? null : args[3],
                Side = multiplayer ? Side.Random : (Side) Enum.Parse(typeof (Side), args[4])
            };
        }

        private static Participant[] GetParticipants(ParticipantsServer server, HelloPackage helloPackage)
        {
            var participant = server.GetParticipant(helloPackage);
            var participants = new Participant[2];
            participants[participant.ControlledRobot] = participant;
            var botNumber = participant.ControlledRobot == 0 ? 1 : 0;
            server.Competitions.Initialize(new CVARCEngine(server.Rules), new[]
            {
                new RobotSettings(participant.ControlledRobot, false), 
                new RobotSettings(botNumber, true)
            });
            var botName = server.Competitions.HelloPackage.Opponent ?? "None";
            participants[botNumber] = server.Competitions.CreateBot(botName, botNumber);
            return participants;
        }

        private static Participant[] GetMultiplayerParticipants(ParticipantsServer server, HelloPackage helloPackage)
        {
            var participants = server.GetParticipants(helloPackage);
            server.Competitions.Initialize(new CVARCEngine(server.Rules), new[]
            {
                new RobotSettings(0, false), 
                new RobotSettings(1, false)
            });
            return participants;
        }
    }
}
