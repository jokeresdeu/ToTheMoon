using System.Collections.Generic;
using System.Linq;
using Game.Enum;
using NUnit.Framework;
using Tools;
using UnityEngine;

namespace Tests
{
    public sealed class GridMathTests
    {
        private static List<VectorIntPair> _vectorIntPairs = new List<VectorIntPair>
        {
            new VectorIntPair(new Vector2Int(0, 0), 0),
            new VectorIntPair(new Vector2Int(1, 0), 1),
            new VectorIntPair(new Vector2Int(2, 0), 2),
            new VectorIntPair(new Vector2Int(0, 1), 3),
            new VectorIntPair(new Vector2Int(1, 1), 4),
            new VectorIntPair(new Vector2Int(2, 1), 5),
            new VectorIntPair(new Vector2Int(0, 2), 6),
            new VectorIntPair(new Vector2Int(1, 2), 7),
            new VectorIntPair(new Vector2Int(2, 2), 8)
        };

        private static List<int> _positions = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        private static List<int> _sideElements = new List<int> { 0, 1, 2 };

        [TestCaseSource(nameof(_vectorIntPairs))]
        [Test, Order(0)]
        public void ToIntPosition_returns_expected_int_value_for_Vector2Int_position(VectorIntPair vectorIntPair)
            => Assert.AreEqual(vectorIntPair.VectorPosition.ToIntPosition(), vectorIntPair.IntPosition);

        [TestCaseSource(nameof(_vectorIntPairs))]
        [Test, Order(1)]
        public void ToMatrixPosition_returns_expected_Vector2Int_value_for_int_position(VectorIntPair vectorIntPair)
            => Assert.AreEqual(vectorIntPair.IntPosition.ToMatrixPosition(), vectorIntPair.VectorPosition);

        [TestCaseSource(nameof(_positions))]
        [Test, Order(2)]
        public void TryGetFinishedLine_returns_false_for_position_if_rest_greed_is_empty(int position)
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                figures[vectorPos] = FigureType.None;
            }

            figures[position.ToMatrixPosition()] = FigureType.Cross;
            Assert.False(figures.TryGetFinishedLine(position.ToMatrixPosition(), out var finishedLine));
        }

        [TestCaseSource(nameof(_positions))]
        [Test, Order(3)]
        public void TryGetFinishedLine_returns_false_for_position_if_grid_is_not_finished(int position)
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = vectorPos.y == 2 || vectorPos.x == 2 ? FigureType.None : FigureType.Cross;
                figures[vectorPos] = figure;
            }

            Assert.False(figures.TryGetFinishedLine(position.ToMatrixPosition(), out var finishedLine));
        }

        [TestCaseSource(nameof(_sideElements))]
        [Test, Order(4)]
        public void TryGetFinishedLine_returns_true_if_any_row_is_finished(int row)
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = vectorPos.y == row ? FigureType.Cross : FigureType.None;
                figures[vectorPos] = figure;
            }

            Assert.True(figures.TryGetFinishedLine(new Vector2Int(0, row), out var finishedLine));
        }

        [TestCaseSource(nameof(_sideElements))]
        [Test, Order(5)]
        public void TryGetFinishedLine_returns_finished_row_if_any_row_is_finished(int row)
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = vectorPos.y == row ? FigureType.Cross : FigureType.None;
                figures[vectorPos] = figure;
            }

            figures.TryGetFinishedLine(new Vector2Int(0, row), out var finishedLine);
            var expected = new List<Vector2Int>();
            for (var i = 0; i < 3; i++)
                expected.Add(new Vector2Int(i, row));
            Assert.True(LinesEqual(finishedLine, expected));
        }

        [TestCaseSource(nameof(_sideElements))]
        [Test, Order(6)]
        public void TryGetFinishedLine_returns_true_if_any_column_is_finished(int column)
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = vectorPos.x == column ? FigureType.Cross : FigureType.None;
                figures[vectorPos] = figure;
            }

            Assert.True(figures.TryGetFinishedLine(new Vector2Int(column, 0), out var finishedLine));
        }

        [TestCaseSource(nameof(_sideElements))]
        [Test, Order(7)]
        public void TryGetFinishedLine_returns_finished_column_if_any_column_is_finished(int column)
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = vectorPos.x == column ? FigureType.Cross : FigureType.None;
                figures[vectorPos] = figure;
            }

            figures.TryGetFinishedLine(new Vector2Int(column, 0), out var finishedLine);
            var expected = new List<Vector2Int>();
            for (var i = 0; i < 3; i++)
                expected.Add(new Vector2Int(column, i));
            Assert.True(LinesEqual(finishedLine, expected));
        }

        [Test, Order(8)]
        public void TryGetFinishedLine_returns_true_if_main_diagonal_is_finished()
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = vectorPos.x == vectorPos.y ? FigureType.Cross : FigureType.None;
                figures[vectorPos] = figure;
            }

            Assert.True(figures.TryGetFinishedLine(new Vector2Int(0, 0), out var finishedLine));
        }

        [Test, Order(9)]
        public void TryGetFinishedLine_returns_main_diagonal_if_main_diagonal_is_finished()
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = vectorPos.x == vectorPos.y ? FigureType.Cross : FigureType.None;
                figures[vectorPos] = figure;
            }

            figures.TryGetFinishedLine(new Vector2Int(0, 0), out var finishedLine);
            var expected = new List<Vector2Int>();
            for (var i = 0; i < 3; i++)
                expected.Add(new Vector2Int(i, i));
            Assert.True(LinesEqual(finishedLine, expected));
        }

        [Test, Order(10)]
        public void TryGetFinishedLine_returns_true_if_side_diagonal_is_finished()
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = i == 2 || i == 4 || i == 6 ? FigureType.Cross : FigureType.None;
                figures[vectorPos] = figure;
            }

            Assert.True(figures.TryGetFinishedLine(new Vector2Int(0, 2), out var finishedLine));
        }

        [Test, Order(11)]
        public void TryGetFinishedLine_returns_side_diagonal_if_side_diagonal_is_finished()
        {
            var figures = new Dictionary<Vector2Int, FigureType>();
            for (var i = 0; i < 9; i++)
            {
                var vectorPos = i.ToMatrixPosition();
                var figure = i == 2 || i == 4 || i == 6 ? FigureType.Cross : FigureType.None;
                figures[vectorPos] = figure;
            }

            figures.TryGetFinishedLine(new Vector2Int(2, 0), out var finishedLine);
            var expected = new List<Vector2Int>();
            for (var i = 0; i < 3; i++)
                expected.Add(new Vector2Int(i, 2 - i));
            Assert.True(LinesEqual(finishedLine, expected));
        }

        private bool LinesEqual(List<Vector2Int> lineOne, List<Vector2Int> lineTwo)
            => !lineOne.Where((t, i) => t != lineTwo[i]).Any();

        public class VectorIntPair
        {
            public Vector2Int VectorPosition { get; }
            public int IntPosition { get; }

            public VectorIntPair(Vector2Int vectorPosition, int intPosition)
            {
                VectorPosition = vectorPosition;
                IntPosition = intPosition;
            }
        }
    }
}