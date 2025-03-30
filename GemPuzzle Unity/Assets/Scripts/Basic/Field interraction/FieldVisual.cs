using Gem.Management;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Gem.Basic.FieldInteraction
{
	[RequireComponent(typeof(SquareClickDetection), typeof(SpriteRenderer))]
	public sealed class FieldVisual : MonoBehaviour
	{
		#region Fields declarations
		[SerializeField] private List<Transform> _squarePlacementsTransforms = new(16);

		private SquareClickDetection _squareClickDetection;
		#endregion

		private void Awake()
		{
			_squareClickDetection = gameObject.GetComponent<SquareClickDetection>();
		}

		private void Start()
		{
			GameHandler.Instance.GameProcess.NewGameInitComplete += OnNewGameInitComplete;
			GameHandler.Instance.GameProcess.GameInterrupted += OnGameInterrupted;
			GameHandler.Instance.GameProcess.GameFinished += OnGameFinished;
			_squareClickDetection.SquareClicked += OnSquareClicked;
			gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			GameHandler.Instance.GameProcess.NewGameInitComplete -= OnNewGameInitComplete;
			GameHandler.Instance.GameProcess.GameInterrupted -= OnGameInterrupted;
			GameHandler.Instance.GameProcess.GameFinished -= OnGameFinished;
			_squareClickDetection.SquareClicked -= OnSquareClicked;
		}

		private void OnNewGameInitComplete()
		{
			gameObject.SetActive(true);

			foreach (var element in _squarePlacementsTransforms)
			{
				if (element.childCount != 0)
				{
					GameObject.DestroyImmediate(element.GetChild(0).gameObject);
				}
			}

			var field = GameHandler.Instance.GameProcess.Field;

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (field[i,j] != 16)
					{
						InstantiateNewSquare(ref field, i, j);
					}
				}
			}

			GameHandler.Instance.GameProcess.SquareDataPositionChanged += OnSquareDataPositionChanged;
		}

		private void OnGameInterrupted()
		{
			GameHandler.Instance.GameProcess.SquareDataPositionChanged -= OnSquareDataPositionChanged;
		}

		private void OnGameFinished()
		{
			GameHandler.Instance.GameProcess.SquareDataPositionChanged -= OnSquareDataPositionChanged;
		}

		private void InstantiateNewSquare(ref int[,] field, int row, int col)
		{
			var random = new System.Random();

			var square = GameObject.Instantiate(
				GameHandler.Instance.ResourcesSystem.SquaresPrefabs.ElementAt(field[row, col] - 1),
				_squarePlacementsTransforms[row * 4 + col]);

			var squareVisual = square.GetComponent<SquareVisual>();
			squareVisual.Coordinates = new() { Row = row, Col = col };
			var baseColor = (random.Next() % 75 + 180, random.Next() % 75 + 180, random.Next() % 75 + 180);
			squareVisual.BackgroundSpriteRenderer.color = new(baseColor.Item1 / 255f, baseColor.Item2 / 255f, baseColor.Item3 / 255f);
		}

		private void OnSquareClicked(SquareVisual square)
		{
			GameHandler.Instance.GameProcess.HandleSquareClickCommand.Execute(square);
		}

		private void OnSquareDataPositionChanged((int Row, int Col) oldCoordinates, (int Row, int Col) newCoordinates)
		{
			var squareTransform = _squarePlacementsTransforms[oldCoordinates.Row * 4 + oldCoordinates.Col].GetChild(0);

			squareTransform.parent = _squarePlacementsTransforms[newCoordinates.Row * 4 + newCoordinates.Col];
			squareTransform.position = _squarePlacementsTransforms[newCoordinates.Row * 4 + newCoordinates.Col].position;
			squareTransform.gameObject.GetComponent<SquareVisual>().Coordinates = new() { Row = newCoordinates.Row, Col = newCoordinates.Col };
		}
	}
}
