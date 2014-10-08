using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2
{
    public interface IKeyboard
    {
        IEnumerable<string> PressedKeys { get; }
    }
}
