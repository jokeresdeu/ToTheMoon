using System.Collections.Generic;
using Game.Core;
using Game.Enum;
using Game.Grid;
using Game.Solver;
using NSubstitute;
using NUnit.Framework;
using Tools;
using UI.GameMenu.Model;
using UI.Texts.Provider;
using UnityEngine;

namespace Tests
{
    public sealed class HintTest : MonoBehaviour
    {
        private static List<int> _positions = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        [TestCaseSource(nameof(_positions))]
        [Test, Order(0)]
        public void EasySolver_returns_freeSlot(int position)
        {
            var figuresGreedController = Substitute.For<IFiguresGridController>();
            var solver = new EasySolver(figuresGreedController);
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = i == position ? FigureType.None : FigureType.Cross;
                figures[vectorPos] = figure;
            }

            figuresGreedController.FiguresOnGrid.Returns(figures);
            Assert.IsTrue(solver.GetPositionToPlace() == position);
        }

        [Test, Order(0)]
        public void SinglePlayerGameController_ShowHint_calls_play_hit_from_IGridView()
        {
            var gridView = Substitute.For<IGridView>();
            var gameController = new SinglePlayerGameController(gridView, null);
            gameController.ShowHint();
            gridView.Received(1).PlayHint(Arg.Any<int>());
        }

        [TestCaseSource(nameof(_positions))]
        [Test, Order(0)]
        public void SinglePlayerGameMenuScreenModel_ShowHint_calls_play_hit_from_IGridView()
        {
            var gridView = Substitute.For<IGridView>();
            var gameController = new SinglePlayerGameController(gridView, null);
            var model = new SinglePlayerGameMenuScreenModel(gameController, Substitute.For<ITextProvider>());
            model.ShowHint();
            gridView.Received(1).PlayHint(Arg.Any<int>());
        }
    }
}