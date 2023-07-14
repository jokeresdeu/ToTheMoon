using System.Collections.Generic;
using System.Linq;
using Game;
using UI.MainMenu.Data;
using UI.Texts.Provider;
using UnityEngine;

namespace UI.MainMenu
{
    public sealed class MainMenuScreenModel
    {
        private readonly List<GameModeSelectionSettings> _gameModesSelectionSettings;
        private readonly GameLauncher _gameLauncher;

        public int SelectedModeIndex { get; private set; }
        public List<string> GameModesTexts { get; }

        public MainMenuScreenModel(GameLauncher gameLauncher, ITextProvider textProvider)
        {
            var storage = Resources.Load<GameModeSelectionStorage>(nameof(GameModeSelectionStorage));
            _gameLauncher = gameLauncher;
            _gameModesSelectionSettings = storage.GameModesSelectionSettings;
            GameModesTexts =
                _gameModesSelectionSettings.Select(element => textProvider.GetText(element.TextKey)).ToList();
        }

        public void SelectGameMode(int gameModeIndex) =>
            SelectedModeIndex = gameModeIndex;

        public void StartGame() => _gameLauncher.LaunchGame(_gameModesSelectionSettings[SelectedModeIndex].GameMode);
    }
}