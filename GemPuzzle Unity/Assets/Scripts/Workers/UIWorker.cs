using Gem.Commands;
using Gem.Management;
using UnityEngine.UIElements;

namespace Gem.Workers
{
	public static class UIWorker
	{
		public static void SetupMainMenuUI()
		{
			var document = GameHandler.Instance.UIManager.MainMenuUI.GetComponent<UIDocument>();

			var newGameButton = document.rootVisualElement.Q<Button>("NewGameButton");

			newGameButton.RegisterCallback<MouseUpEvent>(evt => new StartNewGameCommand().Execute());
		}

		public static void SetupLevelUI()
		{
			var document = GameHandler.Instance.UIManager.LevelUI.GetComponent<UIDocument>();

			var newGameButton = document.rootVisualElement.Q<Button>("NewGameButton");

			newGameButton.RegisterCallback<MouseUpEvent>(evt => new StartNewGameCommand().Execute());
		}

		public static void SetupGameFinishedUI()
		{
			var document = GameHandler.Instance.UIManager.GameFinishedUI.GetComponent<UIDocument>();

			var newGameButton = document.rootVisualElement.Q<Button>("NewGameButton");

			newGameButton.RegisterCallback<MouseUpEvent>(evt => new StartNewGameCommand().Execute());
		}
	}
}
