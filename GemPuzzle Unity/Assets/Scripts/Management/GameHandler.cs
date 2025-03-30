using Gem.Basic.FieldInteraction;
using Gem.Basic.Logic;
using Gem.Systems;
using UnityEngine;

namespace Gem.Management
{
	public sealed class GameHandler : MonoBehaviour
	{
		public static GameHandler Instance { get; private set; }

		[field: SerializeField] public InputSystem InputSystem { get; private set; }

		[field: SerializeField] public ResourcesSystem ResourcesSystem { get; private set; }

		[field: SerializeField] public UIManager UIManager { get; private set; }

		public GameProcess GameProcess { get; private set; } = new();

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(this);
			}

			Instance = this;
		}

		private void Start()
		{
			UIManager.SetupUI();
		}
	}
}
