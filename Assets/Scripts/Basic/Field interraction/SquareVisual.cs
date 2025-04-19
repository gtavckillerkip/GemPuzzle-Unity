using System;
using UnityEngine;

namespace Gem.Basic.FieldInteraction
{
	[RequireComponent(typeof(BoxCollider2D))]
	public sealed class SquareVisual : MonoBehaviour
	{
		[Serializable]
		public struct Coords
		{
			public int Row;
			public int Col;
		}

		[field: SerializeField, Tooltip("Координаты на поле: значения от 0 до 3.")]
		public Coords Coordinates { get; set; }

		[field: SerializeField]
		public SpriteRenderer BackgroundSpriteRenderer { get; private set; }
	}
}
