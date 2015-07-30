using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public enum RecordType
    {
        Permanent,
        Temporary,
    }

    public class ScoreRecord
    {
        public readonly double Time; 
        public readonly string Reason; 
        public readonly int Count;
        public readonly RecordType Type;
        
        public ScoreRecord(int count, string reason, double time, 
            RecordType type=RecordType.Permanent)
        {
            Count = count;
            Reason = reason;
            Time = time;
            Type = type;
        }
    }
}
