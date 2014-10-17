using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jango
{
    public class JangoKeyboardControllerPool : KeyboardControllerPool<JangoCommand>
    {
        public JangoKeyboardControllerPool()
        {
            Add(Keys.W, "Robot", ()=>new JangoCommand { 
                Duration=1, 
                RequestedAngleDeltas=new Angle[] { Angle.Zero, Angle.FromGrad(10) }});

            Add(Keys.E, "Robot", ()=>new JangoCommand { 
                Duration=1, 
                RequestedAngleDeltas=new Angle[] { Angle.Zero, Angle.FromGrad(-10) }});
Add(Keys.S, "Robot", ()=>new JangoCommand { 
                Duration=1, 
                RequestedAngleDeltas=new Angle[] { Angle.FromGrad(10), Angle.Zero }});
Add(Keys.D, "Robot", ()=>new JangoCommand { 
                Duration=1, 
                RequestedAngleDeltas=new Angle[] { Angle.FromGrad(-10), Angle.Zero }});
        }
    }
}
