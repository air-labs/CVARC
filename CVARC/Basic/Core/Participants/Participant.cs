using System;
using CVARC.Basic.Controllers;

namespace CVARC.Basic.Core.Participants
{
    public enum ExitReason
    {
        No,
        FormatException,
        OperationalTimeLimit
    }

    public abstract class Participant
    {
        public abstract Command MakeTurn();
        public bool Active { get; set; }
        public int OperationalMilliseconds { get; set; }
        public ExitReason ExitReason { get; set; }
        public double ExitTime { get; set; }
        public Exception Exception { get; set; }
        public double WaitForNextCommandTime { get; set; }
        public int ControlledRobot { get; set; }
        public void Exit(ExitReason reason, double exitTime, Exception exception)
        {
            Active = false;
            ExitTime = exitTime;
            ExitReason = reason;
            Exception = exception;
            WaitForNextCommandTime = double.PositiveInfinity;
        }
    }
}
