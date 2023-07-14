using System;
using System.Collections.Generic;
using Game.Core;
using Game.Enum;
using Game.Grid;
using Game.Player;
using Game.Player.Controller;
using Game.Player.Data;
using Game.Solver;
using Game.TouchController;
using UI.Core;
using UI.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class GameLauncher : IScreenOpener
    {
        private readonly GridView _gridView;
        private readonly IInput _input;
        private readonly Dictionary<FigureType, Color> _figuresColors;

        public BaseGameController BaseGameController { get; }
        public SinglePlayerGameController SinglePlayerGameController { get; }

        private BaseGameController _currentGameController;

        public event Action<ScreenType, List<object>> OpenScreenRequested;

        public GameLauncher(GridView gridView, Image fader, IInput input)
        {
            _gridView = gridView;
            _input = input;
            BaseGameController = new BaseGameController(_gridView, fader);
            SinglePlayerGameController = new SinglePlayerGameController(_gridView, fader);
            var figureColorsStorage = Resources.Load<FigureColorsStorage>(nameof(FigureColorsStorage));
            _figuresColors = figureColorsStorage.FiguresColors;
        }

        public void LaunchGame(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.PlayerVsPlayer:
                    StartPlayerVsPlayer();
                    return;
                case GameMode.PlayerVsComputer:
                    StartPlayerVsComputer();
                    return;
                case GameMode.ComputerVsComputer:
                    StartComputerVsComputer();
                    return;
            }
        }

        private void StartPlayerVsPlayer()
        {
            var players = new Dictionary<FigureType, PlayerConfig>
            {
                { FigureType.Cross, new PlayerConfig(new SimplePlayer(_input), _figuresColors[FigureType.Cross]) },
                { FigureType.Circle, new PlayerConfig(new SimplePlayer(_input), _figuresColors[FigureType.Circle]) }
            };
            OpenScreenRequested?.Invoke(ScreenType.TwoPlayersGameMenu, new List<object> { _figuresColors });
            BaseGameController.StartGame(players);
        }

        private void StartPlayerVsComputer()
        {
            var solver = new EasySolver(SinglePlayerGameController);
            var players = new Dictionary<FigureType, PlayerConfig>
            {
                { FigureType.Cross, new PlayerConfig(new SimplePlayer(_input), _figuresColors[FigureType.Cross]) },
                { FigureType.Circle, new PlayerConfig(new AiPlayer(_gridView, solver), _figuresColors[FigureType.Circle]) }
            };
            OpenScreenRequested?.Invoke(ScreenType.SinglePlayerGameMenu, new List<object> { _figuresColors });
            SinglePlayerGameController.StartGame(players);
            SinglePlayerGameController.SetUser(FigureType.Cross);
        }

        private void StartComputerVsComputer()
        {
            var solver = new EasySolver(BaseGameController);
            var players = new Dictionary<FigureType, PlayerConfig>
            {
                { FigureType.Cross, new PlayerConfig(new AiPlayer(_gridView, solver), _figuresColors[FigureType.Cross]) },
                { FigureType.Circle, new PlayerConfig(new AiPlayer(_gridView, solver), _figuresColors[FigureType.Circle]) }
            };
            OpenScreenRequested?.Invoke(ScreenType.TwoPlayersGameMenu, new List<object> { _figuresColors });
            BaseGameController.StartGame(players);
        }
    }
}