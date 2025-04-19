using Gem.Commands;
using Gem.Workers;
using System;

namespace Gem.Basic.Logic
{
	/// <summary>
	/// Класс игрового процесса.
	/// </summary>
	public sealed class GameProcess
	{
		#region Fields declarations
		/// <summary>
		/// Матрица игрового поля.
		/// </summary>
		private int[,] _field;

		/// <summary>
		/// Контрольное значение для определения окончания игры.
		/// </summary>
		private const int CONTROL_VALUE = 32767;

		/// <summary>
		/// Текущее контрольное значение для игровой сессии.
		/// </summary>
		private int _currentControlValue;

		/// <summary>
		/// Координаты свободной позиции.
		/// </summary>
		private (int Row, int Col) _freePosition;

		private bool _isGameInProcess;
		#endregion

		#region Events
		/// <summary>
		/// Инициализация новой игры окончена.
		/// </summary>
		public event Action NewGameInitComplete;

		/// <summary>
		/// Позиция числа, связанного с фишкой, в модели матрицы изменилась.
		/// </summary>
		public event Action<(int OldRow, int OldCol), (int NewRow, int NewCol)> SquareDataPositionChanged;

		/// <summary>
		/// Игра окончена.
		/// </summary>
		public event Action GameFinished;

		public event Action GameInterrupted;
		#endregion

		#region Properties declarations
		/// <summary>
		/// Матрица игрового поля.
		/// </summary>
		public int[,] Field => _field;

		/// <summary>
		/// Команда обработки нажатия на фишку.
		/// </summary>
		public HandleSquareClickCommand HandleSquareClickCommand { get; private set; }
		#endregion

		/// <summary>
		/// Инициализировать сущность новой игры.
		/// </summary>
		public void InitializeNewGame()
		{
			if (_isGameInProcess)
			{
				GameInterrupted?.Invoke();
			}

			HandleSquareClickCommand ??= new HandleSquareClickCommand(this);

			_freePosition = (3, 3);

			_field = new int[4, 4];
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					_field[i, j] = i * 4 + j + 1;
				}
			}

			SquareWorker.PositionSquaresRandomly(ref _field, ref _freePosition);

			_currentControlValue = InitializeCurrentControlValue();

			NewGameInitComplete?.Invoke();

			_isGameInProcess = true;
		}

		/// <summary>
		/// Обработать нажатие на фишку.
		/// </summary>
		/// <param name="coordinates"> Координаты фишки. </param>
		public void HandleSquareClicked((int Row, int Col) coordinates)
		{
			var freePosition = _freePosition;
			var swapped = SquareWorker.TrySwapSquarePositionAndFreePosition(ref coordinates, ref freePosition);

			if (!swapped)
			{
				return;
			}

			_field[coordinates.Row, coordinates.Col] += _field[freePosition.Row, freePosition.Col];
			_field[freePosition.Row, freePosition.Col] = _field[coordinates.Row, coordinates.Col] - _field[freePosition.Row, freePosition.Col];
			_field[coordinates.Row, coordinates.Col] -= _field[freePosition.Row, freePosition.Col];

			_freePosition = freePosition;

			SquareDataPositionChanged?.Invoke(freePosition, coordinates);

			if (_field[coordinates.Row, coordinates.Col] == coordinates.Row * 4 + coordinates.Col + 1)
			{
				_currentControlValue |= (1 << (_field[coordinates.Row, coordinates.Col] - 1));
			}
			else
			{
				_currentControlValue &= ~(1 << (_field[coordinates.Row, coordinates.Col] - 1));
			}

			if (_currentControlValue == CONTROL_VALUE)
			{
				_isGameInProcess = false;
				GameFinished?.Invoke();
			}
		}

		/// <summary>
		/// Инициализировать текущее контрольное значение.
		/// </summary>
		/// <returns> Значение. </returns>
		private int InitializeCurrentControlValue()
		{
			int value = 0;

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (_field[i, j] == i * 4 + j + 1 && _field[i, j] != 16)
					{
						value |= (1 << (_field[i,j] - 1));
					}
				}
			}

			return value;
		}
	}
}
