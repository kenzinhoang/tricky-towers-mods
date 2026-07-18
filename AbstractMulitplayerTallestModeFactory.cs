using System;

namespace TrickyMultiplayerPlus
{
	public abstract class AbstractMulitplayerTallestModeFactory
	{
		public virtual TallestGameModeFactory Create()
		{
			throw new NotImplementedException();
		}
	}

	public abstract class AbstractMulitplayerTimeAttackModeFactory
	{
		public virtual TimeAttackGameModeFactory Create() { throw new NotImplementedException(); }
	}
}
