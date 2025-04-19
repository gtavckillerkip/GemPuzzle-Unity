using Gem.Management;
using System;
using UnityEngine;

namespace Gem.Basic.FieldInteraction
{
	public sealed class SquareClickDetection : MonoBehaviour
	{
		public event Action<SquareVisual> SquareClicked;

		private void Start()
		{
			GameHandler.Instance.InputSystem.MouseDown += OnMouseDown;
		}

		private void OnDestroy()
		{
			GameHandler.Instance.InputSystem.MouseDown -= OnMouseDown;
		}

		private void OnMouseDown()
		{
			var hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

			if (hit.collider != null && hit.collider.gameObject.TryGetComponent<SquareVisual>(out var square))
			{
				SquareClicked?.Invoke(square);
			}
		}
	}
}
