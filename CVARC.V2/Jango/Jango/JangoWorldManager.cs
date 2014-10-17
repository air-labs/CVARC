using CVARC.Core;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jango
{
    public class JangoWorldManager : KroRWorldManager<JangoWorld>
    {
        public override void CreateWorld(IdGenerator generator)
        {
            Root.Add(new Box
            {
                XSize = 100,
                YSize = 3,
                ZSize = 100,
                DefaultColor = Color.White,
                Top = new SolidColorBrush { Color = Color.Yellow },
                IsStatic = true,
                Type = "floor",
            });
        }
    }
}
