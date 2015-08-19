using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2
{
	public static partial class RulesExtensions
	{

		public static void AddCombinedKeys<TCommand>(this IRMCombinedRules<TCommand> rules, 
            KeyboardController<TCommand> pool, string controllerId)
			where TCommand : ICombinedCommand, new()
		{
			if (controllerId == TwoPlayersId.Left)
			{
                pool.Add(Keys.D1, () => new TCommand { CombinedCommand = new[] { "LeftDeployer", "RightDeployer" } });
                pool.Add(Keys.D2, () => new TCommand { CombinedCommand = new[] { "LadderTaker" } });
			}
			if (controllerId == TwoPlayersId.Right)
			{
                pool.Add(Keys.NumPad1, () => new TCommand { CombinedCommand = new[] { "LeftDeployer", "RightDeployer" } });
                pool.Add(Keys.NumPad2, () => new TCommand { CombinedCommand = new[] { "LadderTaker" } });
			}
		}

        public static TCommand UseLeftDeployer<TCommand>(this IRMCombinedRules<TCommand> factory)
            where TCommand : ICombinedCommand, new()
        {
            return new TCommand { CombinedCommand = new[] { "LeftDeployer" } };
        }
        public static TCommand UseRightDeployer<TCommand>(this IRMCombinedRules<TCommand> factory)
            where TCommand : ICombinedCommand, new()
        {
            return new TCommand { CombinedCommand = new[] { "RightDeployer" } };
        }
        public static TCommand UpLadder<TCommand>(this IRMCombinedRules<TCommand> factory)
            where TCommand : ICombinedCommand, new()
        {
            return new TCommand { CombinedCommand = new[] { "LadderTaker" } };
        }
        public static TCommand GetPopCorn<TCommand>(this IRMCombinedRules<TCommand> factory)
           where TCommand : ICombinedCommand, new()
        {
            return new TCommand { CombinedCommand = new[] { "PopcornMachineUser" } };
        }
	}
}
