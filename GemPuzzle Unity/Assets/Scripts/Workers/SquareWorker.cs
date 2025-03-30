using Gem.Management;
using System;
using System.Linq;

namespace Gem.Workers
{
	public static class SquareWorker
	{
		#region Definitions
		/// <summary>
		/// Направление сдвига фишки.
		/// </summary>
		private enum Direction
		{
			/// <summary>
			/// Влево.
			/// </summary>
			Left,

			/// <summary>
			/// Вправо.
			/// </summary>
			Right,

			/// <summary>
			/// Вверх.
			/// </summary>
			Up,

			/// <summary>
			/// Вниз.
			/// </summary>
			Down
		}
		#endregion

		/// <summary>
		/// Расположить фишки в случайном порядке.
		/// </summary>
		/// <param name="squares"> Матрица фишек. </param>
		/// <param name="freePosition"> Позиция свободной клетки. </param>
		public static void PositionSquaresRandomly(ref int[,] squares, ref (int Row, int Col) freePosition)
		{
			var rand = new Random();

			for (var i = 0; i < 400; i++)
			{
				// Вычисление направления перемещения !свободной клетки!.
				var direction = Enum.GetValues(typeof(Direction)).OfType<Direction>().ElementAt(rand.Next() % 4);

				switch (direction)
				{
					case Direction.Up:
						if (freePosition.Row > 0)
						{
							freePosition = (freePosition.Row - 1, freePosition.Col);
							(squares[freePosition.Row + 1, freePosition.Col], squares[freePosition.Row, freePosition.Col]) =
								(squares[freePosition.Row, freePosition.Col], squares[freePosition.Row + 1, freePosition.Col]);
						}
						break;

					case Direction.Down:
						if (freePosition.Row < 3)
						{
							freePosition = (freePosition.Row + 1, freePosition.Col);
							(squares[freePosition.Row - 1, freePosition.Col], squares[freePosition.Row, freePosition.Col]) =
								(squares[freePosition.Row, freePosition.Col], squares[freePosition.Row - 1, freePosition.Col]);
						}
						break;

					case Direction.Left:
						if (freePosition.Col > 0)
						{
							freePosition = (freePosition.Row, freePosition.Col - 1);
							(squares[freePosition.Row, freePosition.Col + 1], squares[freePosition.Row, freePosition.Col]) =
								(squares[freePosition.Row, freePosition.Col], squares[freePosition.Row, freePosition.Col + 1]);
						}
						break;

					case Direction.Right:
						if (freePosition.Col < 3)
						{
							freePosition = (freePosition.Row, freePosition.Col + 1);
							(squares[freePosition.Row, freePosition.Col - 1], squares[freePosition.Row, freePosition.Col]) =
								(squares[freePosition.Row, freePosition.Col], squares[freePosition.Row, freePosition.Col - 1]);
						}
						break;
				}
			}
		}

		/// <summary>
		/// Попытаться поменять местами фишку и свободную клетку.
		/// </summary>
		/// <param name="squarePosition"> Позиция фишки. </param>
		/// <param name="freePosition"> Позиция свободной клетки. </param>
		public static bool TrySwapSquarePositionAndFreePosition(ref (int Row, int Col) squarePosition, ref (int Row, int Col) freePosition)
		{
			switch ((freePosition.Row * 4 + freePosition.Col) - (squarePosition.Row * 4 + squarePosition.Col))
			{
				case -4:
					freePosition = (freePosition.Row + 1, freePosition.Col);
					squarePosition = (squarePosition.Row - 1, squarePosition.Col);
					return true;

				case 4:
					freePosition = (freePosition.Row - 1, freePosition.Col);
					squarePosition = (squarePosition.Row + 1, squarePosition.Col);
					return true;

				case -1:
					freePosition = (freePosition.Row, freePosition.Col + 1);
					squarePosition = (squarePosition.Row, squarePosition.Col - 1);
					return true;

				case 1:
					freePosition = (freePosition.Row, freePosition.Col - 1);
					squarePosition = (squarePosition.Row, squarePosition.Col + 1);
					return true;
			}

			return false;
		}
	}
}
