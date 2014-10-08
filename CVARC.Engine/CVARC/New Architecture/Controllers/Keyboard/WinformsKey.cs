using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2
{
    public class WinformsKeyboard : IKeyboard, IMessageFilter
    {
        HashSet<Keys> pressedKeys = new HashSet<Keys>();

      

        private const int WM_KEYDOWN = 0x0100;

        private const int WM_KEYUP = 0x0101;

        private bool m_keyPressed = false;

        public WinformsKeyboard()
        {
            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
                pressedKeys.Add((Keys)m.WParam);

            if (m.Msg == WM_KEYUP)
            {
                var key = (Keys)m.WParam;
                if (pressedKeys.Contains(key)) pressedKeys.Remove(key);
            }

            return false;
        }

        public IEnumerable<string> PressedKeys
        {
            get { return pressedKeys.Select(z => z.ToString()); }
        }
    }
}
