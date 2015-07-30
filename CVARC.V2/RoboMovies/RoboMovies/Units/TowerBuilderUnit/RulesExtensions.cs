using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2
{
	public static partial class RulesExtensions
	{
		public static TCommand Collect<TCommand>(this ITowerBuilderRules<TCommand> factory)
			where TCommand : ITowerBuilderCommand, new()
		{
			return new TCommand { TowerBuilderCommand = TowerBuilderAction.Collect };
		}
		
        public static TCommand BuildTower<TCommand>(this ITowerBuilderRules<TCommand> factory)
			where TCommand : ITowerBuilderCommand, new()
		{
			return new TCommand { TowerBuilderCommand = TowerBuilderAction.BuildTower };
		}

		public static void AddBuilderKeys<TCommand>(this ITowerBuilderRules<TCommand> rules, 
            KeyboardController<TCommand> pool, string controllerId)
			where TCommand : ITowerBuilderCommand, new()
		{
			if (controllerId == TwoPlayersId.Left)
			{
				pool.Add(Keys.Q, () => new TCommand { TowerBuilderCommand = TowerBuilderAction.Collect });
				pool.Add(Keys.E, () => new TCommand { TowerBuilderCommand = TowerBuilderAction.BuildTower });
			}
			if (controllerId == TwoPlayersId.Right)
			{
				pool.Add(Keys.U, () => new TCommand { TowerBuilderCommand = TowerBuilderAction.Collect });
				pool.Add(Keys.O, () => new TCommand { TowerBuilderCommand = TowerBuilderAction.BuildTower });
			}
		}

	}
}
