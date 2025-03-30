using Gem.Management;

namespace Gem.Commands
{
	public sealed class StartNewGameCommand : ICommand
	{
		public void Execute(params object[] parameters)
		{
			GameHandler.Instance.GameProcess.InitializeNewGame();
		}
	}
}
