namespace Gem.Basic.Definitions
{
	public sealed class Square
	{
        public Square(int number)
        {
            Number = number;
        }

        public int Number { get; private set; }
	}
}
