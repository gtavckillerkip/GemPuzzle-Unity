using Gem.Basic.FieldInteraction;
using Gem.Basic.Logic;

namespace Gem.Commands
{
	public sealed class HandleSquareClickCommand : ICommand
	{
		private GameProcess _gameProcess;

        public HandleSquareClickCommand(GameProcess gameProcess)
        {
            _gameProcess = gameProcess;
        }

        public void Execute(params object[] parameters)
		{
			var square = parameters[0] as SquareVisual;

			_gameProcess.HandleSquareClicked((square.Coordinates.Row, square.Coordinates.Col));
		}
	}
}
