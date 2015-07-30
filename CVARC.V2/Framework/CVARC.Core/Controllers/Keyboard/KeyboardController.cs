using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2
{
    public interface IKeyboardController : IController
    {
    }

    public class KeyboardController<TCommand> : Controller<TCommand>, IKeyboardController
        where TCommand : ICommand
    {
        Dictionary<Keys, Func<TCommand>> keys = new Dictionary<Keys, Func<TCommand>>();
        public Func<TCommand> StopCommand { get; set; }
        IKeyboard keyboard;

        public void Add(Keys key, Func<TCommand> action)
        {
            keys[key] = action;
        }

        public override void Initialize(IActor controllableActor)
        {
            base.Initialize(controllableActor);
            keyboard = controllableActor.World.Keyboard;
        }

        public override TCommand GetCommand()
        {
            var pressedKeys = keyboard.PressedKeys.ToList();
            foreach (var key in keys)
                if (pressedKeys.Contains(key.Key.ToString()))
                {
                    var a = key.Value();
                    return a;
                }
            return StopCommand();
        }
    }
}
