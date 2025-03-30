using System;
using UnityEngine;

namespace Gem.Systems
{
	public sealed class InputSystem : MonoBehaviour
	{
		public event Action MouseDown;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				MouseDown?.Invoke();
			}
		}
	}
}
