using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CVARC.Basic.Controllers
{
    public abstract class KeyboardController : Controller
    {
        /*
        protected KeyboardController(Control control)
        {
            var form = (control.TopLevelControl as Form);
            if (form == null)
                throw new Exception("Control must be added to a top-level form.");
            form.KeyPreview = true;
            form.KeyDown += ProcessKey;
        }

        private void ProcessKey(object sender, KeyEventArgs keyEventArgs)
        {
            RaiseProcess(GetCommand(keyEventArgs.KeyData));
        }*/

        public abstract IEnumerable<Command> GetCommand(Keys keyData);
    }
}
