namespace Gem.Commands
{
	public interface ICommand
	{
		void Execute(params object[] parameters);
	}
}
