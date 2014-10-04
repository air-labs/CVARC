using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIRLab;

namespace CVARC.V2
{
    public class KeyboardControllerPool<TCommand> : IKeyboardControllerPool
        where TCommand : ICommand
    {

        public KeyboardControllerPool(IKeyboard keyboard)
        {
            this.keyboard = keyboard;
        }

        IKeyboard keyboard;
        Dictionary<Keys, Tuple<string, Func<TCommand>>> keys = new Dictionary<Keys, Tuple<string, Func<TCommand>>>();
        
        public Func<TCommand> StopCommandFactory { get; set; }
        
        public void Add(Keys key, string controllerId, Func<TCommand> commandFactory)
        {
            keys[key] = new Tuple<string, Func<TCommand>>(controllerId, commandFactory);
        }

        public TCommand GetCommand(string controllerId)
        {
            foreach (var e in keyboard.PressedKeys)
            {
                if (!keys.ContainsKey(e)) continue;
                if (keys[e].Item1 == controllerId)
                    return keys[e].Item2();
            }
            return StopCommandFactory();
        }

        public KeyboardController<TCommand> CreateController(string controllerId)
        {
            return new KeyboardController<TCommand>(this, controllerId);
        }
        IController IKeyboardControllerPool.CreateController(string controllerId)
        {
            return CreateController(controllerId);
        }
    }
}
