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
            double grad = 0.5;
            double duration = 1;
            Add(Keys.W, "Robot", ()=>new JangoCommand { 
                Duration=duration, 
                RequestedAngleDeltas=new Angle[] { Angle.Zero, Angle.FromGrad(grad) }});

            Add(Keys.E, "Robot", ()=>new JangoCommand { 
                Duration=duration, 
                RequestedAngleDeltas=new Angle[] { Angle.Zero, Angle.FromGrad(-grad) }});
Add(Keys.S, "Robot", ()=>new JangoCommand { 
                Duration=duration, 
                RequestedAngleDeltas=new Angle[] { Angle.FromGrad(grad), Angle.Zero }});
Add(Keys.D, "Robot", ()=>new JangoCommand { 
                Duration=duration, 
                RequestedAngleDeltas=new Angle[] { Angle.FromGrad(-grad), Angle.Zero }});
        
            StopCommandFactory=()=>new JangoCommand { Duration=1, RequestedAngleDeltas=new Angle[] { Angle.Zero,Angle.Zero}};
        }
    }
}
