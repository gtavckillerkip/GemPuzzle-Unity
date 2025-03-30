using Gem.Workers;
using UnityEngine;

namespace Gem.Management
{
	public sealed class UIManager : MonoBehaviour
	{
		[field: SerializeField] public GameObject MainMenuUI { get; private set; }
		[field: SerializeField] public GameObject LevelUI { get; private set; }
		[field: SerializeField] public GameObject GameFinishedUI { get; private set; }

		private void Start()
		{
			GameHandler.Instance.GameProcess.NewGameInitComplete += OnNewGameInitComplete;
			GameHandler.Instance.GameProcess.GameFinished += OnGameFinished;
		}

		private void OnDestroy()
		{
			GameHandler.Instance.GameProcess.NewGameInitComplete -= OnNewGameInitComplete;
			GameHandler.Instance.GameProcess.GameFinished -= OnGameFinished;
		}

		public void SetupUI()
		{
			MainMenuUI.SetActive(true);
			UIWorker.SetupMainMenuUI();
		}

		private void OnNewGameInitComplete()
		{
			MainMenuUI.SetActive(false);
			GameFinishedUI.SetActive(false);
			LevelUI.SetActive(true);
			UIWorker.SetupLevelUI();
		}

		private void OnGameFinished()
		{
			LevelUI.SetActive(false);
			GameFinishedUI.SetActive(true);
			UIWorker.SetupGameFinishedUI();
		}
	}
}
