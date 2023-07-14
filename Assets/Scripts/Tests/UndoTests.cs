using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Enum;
using Game.Grid;
using Game.Player;
using Game.Player.Controller;
using NSubstitute;
using NUnit.Framework;
using Tools;
using UI.GameMenu.Model;
using UI.Texts.Provider;
using UnityEngine;

namespace Tests
{
    public sealed class UndoTests
    {
        private static List<FigureType> _figures = new List<FigureType> { FigureType.Circle, FigureType.Cross };
        private IGridView _gridView;

        [SetUp]
        public void SetUp()
        {
            _gridView = Substitute.For<IGridView>();
            _gridView.PlaceFigureToHolder(Arg.Any<int>(), Arg.Any<FigureType>()).Returns(enumerator => YieldReturnNull());
        }

        [TearDown]
        public void TearDown() => _gridView = null;

        [TestCaseSource(nameof(_figures))]
        [Test, Order(0)]
        public void
            SinglePlayerGameController_ShowHint_does_not_calls_PlayHint_from_IGridView_if_it_is_not_user_turn_and_at_least_two_moves_were_made(
                FigureType figureType)
        {
            var gameController = GetGameController(figureType, figureType == FigureType.Circle ? FigureType.Cross : FigureType.Circle,
                new List<Vector2Int>());
            gameController.UndoMove();
            _gridView.DidNotReceive().PlaceFigureToHolder(Arg.Any<int>(), FigureType.None);
        }

        [TestCaseSource(nameof(_figures))]
        [Test, Order(1)]
        public void SinglePlayerGameMenuScreenModel_Undo_calls_PlayHint_from_IGridView_if_it_is_not_user_turn_and_at_least_two_moves_were_made(
            FigureType figureType)
        {
            var gameController = GetGameController(figureType, figureType == FigureType.Circle ? FigureType.Cross : FigureType.Circle,
                new List<Vector2Int>());
            var model = new SinglePlayerGameMenuScreenModel(gameController, Substitute.For<ITextProvider>());
            model.Undo();
            _gridView.DidNotReceive().PlaceFigureToHolder(Arg.Any<int>(), FigureType.None);
        }

        [TestCaseSource(nameof(_figures))]
        [Test, Order(2)]
        public void
            SinglePlayerGameController_ShowHint_does_not_calls_PlayHint_from_IGridView_if_it_is_user_turn_but_at_least_two_moves_were_not_made(
                FigureType figureType)
        {
            var gameController = GetGameController(figureType, figureType,
                new List<Vector2Int>());
            gameController.UndoMove();
            _gridView.DidNotReceive().PlaceFigureToHolder(Arg.Any<int>(), FigureType.None);
        }

        [TestCaseSource(nameof(_figures))]
        [Test, Order(3)]
        public void SinglePlayerGameMenuScreenModel_Undo_calls_PlayHint_from_IGridView_if_it_is_user_turn_but_at_least_two_moves_were_not_made(
            FigureType figureType)
        {
            var gameController = GetGameController(figureType, figureType,
                new List<Vector2Int>());
            var model = new SinglePlayerGameMenuScreenModel(gameController, Substitute.For<ITextProvider>());
            model.Undo();
            _gridView.DidNotReceive().PlaceFigureToHolder(Arg.Any<int>(), FigureType.None);
        }

        [TestCaseSource(nameof(_figures))]
        [Test, Order(4)]
        public void SinglePlayerGameController_ShowHint_calls_PlayHint_from_IGridView_if_it_is_user_turn_and_at_least_two_moves_were_made(
            FigureType figureType)
        {
            var gameController = GetGameController(figureType, figureType,
                new List<Vector2Int> { Vector2Int.zero, Vector2Int.one });
            gameController.UndoMove();
            _gridView.Received(1).PlaceFigureToHolder(Arg.Any<int>(), FigureType.None);
        }

        [TestCaseSource(nameof(_figures))]
        [Test, Order(5)]
        public void SinglePlayerGameMenuScreenModel_Undo_calls_PlayHint_from_IGridView_if_it_is_user_turn_and_at_least_two_moves_were_made(
            FigureType figureType)
        {
            var gameController = GetGameController(figureType, figureType,
                new List<Vector2Int> { Vector2Int.zero, Vector2Int.one });
            var model = new SinglePlayerGameMenuScreenModel(gameController, Substitute.For<ITextProvider>());
            model.Undo();
            _gridView.Received(1).PlaceFigureToHolder(Arg.Any<int>(), FigureType.None);
        }

        [TestCaseSource(nameof(_figures))]
        [Test, Order(6)]
        public void SinglePlayerGameMenuScreenModel_PlaceFigure_saves_placements_position(FigureType figureType)
        {
            var gameController = GetGameController(figureType, figureType,
                new List<Vector2Int> { Vector2Int.zero, Vector2Int.one });
            gameController.InvokePrivateMethod("PlaceFigure", new object[] { Vector2Int.zero });
            var placements = gameController.GetPrivateField<List<Vector2Int>>("_placements");
            Assert.True(placements.Contains(Vector2Int.zero));
        }

        private SinglePlayerGameController GetGameController(FigureType activePlayerFigure, FigureType userFigure, List<Vector2Int> placements)
        {
            var gameController = new SinglePlayerGameController(_gridView, null);
            gameController.SetPrivateField("ActivePlayerFigureType", activePlayerFigure);
            gameController.SetUser(userFigure);
            gameController.SetPrivateField("_placements", placements);
            var players = new Dictionary<FigureType, PlayerConfig>
            {
                { FigureType.Cross, new PlayerConfig(Substitute.For<IPlayer>(), Color.black) },
                { FigureType.Circle, new PlayerConfig(Substitute.For<IPlayer>(), Color.black) }
            };
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                figures[vectorPos] = FigureType.Cross;
            }

            gameController.SetPrivateField("FiguresOnGrid", figures);
            gameController.SetPrivateField("Players", players);
            return gameController;
        }

        private IEnumerator YieldReturnNull()
        {
            yield return null;
        }
    }
}