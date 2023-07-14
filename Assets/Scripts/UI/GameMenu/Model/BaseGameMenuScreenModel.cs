using System;
using System.Collections.Generic;
using Game.Core;
using Game.Enum;
using UI.Texts.Enum;
using UI.Texts.Provider;
using UnityEngine;

namespace UI.GameMenu.Model
{
    public class BaseGameMenuScreenModel
    {
        private readonly Dictionary<FigureType, TextKey> _endGameTexts = new Dictionary<FigureType, TextKey>
        {
            { FigureType.Cross, TextKey.PlayerXWon },
            { FigureType.Circle, TextKey.PlayerOWon },
            { FigureType.None, TextKey.Tie }
        };

        private readonly BaseGameController _gameController;
        private readonly ITextProvider _textProvider;

        private Dictionary<FigureType, Color> _figuresColors;

        public event Action<Color> ActivePlayerChanged;
        public event Action<string, Color> LevelEnded;
        public event Action LevelStarted;

        public BaseGameMenuScreenModel(BaseGameController gameController, ITextProvider textProvider)
        {
            _gameController = gameController;
            _textProvider = textProvider;
            _gameController.LevelEnded += OnLevelEnded;
            _gameController.LevelStarted += OnLevelStarted;
            _gameController.ActivePlayerChanged += OnActivePlayerChanged;
        }

        public void SetPlayerColorSetting(Dictionary<FigureType, Color> figuresColors) => _figuresColors = figuresColors;
        public void RestartGame() => _gameController.RestartGame();
        public void CloseGame() => _gameController.CloseGame();
        private void OnLevelStarted() => LevelStarted?.Invoke();
        private void OnActivePlayerChanged(FigureType currentFigure) => ActivePlayerChanged?.Invoke(_figuresColors[currentFigure]);
        private void OnLevelEnded(FigureType endGameFigure) =>
            LevelEnded?.Invoke(_textProvider.GetText(_endGameTexts[endGameFigure]), _figuresColors[endGameFigure]);
    }
}