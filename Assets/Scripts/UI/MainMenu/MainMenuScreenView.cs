using System;
using System.Collections.Generic;
using TMPro;
using UI.Core.MVC;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public sealed class MainMenuScreenView : ScreenView
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private TMP_Dropdown _modeDropDown;

        public event Action StartGameClicked;
        public event Action<int> GameModeSelected;

        public void InitializeSelection(List<string> selectionVariants)
        {
            _modeDropDown.ClearOptions();
            _modeDropDown.AddOptions(selectionVariants);
        }

        public void SelectGameMode(int index) => _modeDropDown.SetValueWithoutNotify(index);

        protected override void SubscribeEvents()
        {
            _startGameButton.onClick.AddListener(() => StartGameClicked?.Invoke());
            _modeDropDown.onValueChanged.AddListener((gameMode) => GameModeSelected?.Invoke(gameMode));
        }

        protected override void UnsubscribeEvents()
        {
            _startGameButton.onClick.RemoveAllListeners();
            _modeDropDown.onValueChanged.RemoveAllListeners();
        }
    }
}