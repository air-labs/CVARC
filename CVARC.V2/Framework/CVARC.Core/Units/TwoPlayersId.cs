using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class TwoPlayersId
    {
        public static readonly string Left = "Left";
        public static readonly string Right = "Right";
        public static IEnumerable<string> Ids
        {
            get
            {
                yield return Left;
                yield return Right;
            }
        }


    }
}
