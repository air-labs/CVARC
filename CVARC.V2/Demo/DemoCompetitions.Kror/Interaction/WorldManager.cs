using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.Core;
using CVARC.V2;

namespace Demo
{
    public class InteractionWorldManager : KroRWorldManager<MovementWorld>
    {
        public override void CreateWorld(IdGenerator generator)
        {
            Root.Add(new Box
            {
                XSize = 10,
                YSize = 100,
                ZSize = 50,
                DefaultColor = Color.Gray,
                IsStatic = true,
                NewId = "floor",
            });
        }
    }
}
