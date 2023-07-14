using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Enum;
using Game.Grid;
using Game.Solver;
using Services;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public sealed class SinglePlayerGameController : BaseGameController
    {
        private readonly ISolver _solver;
        private readonly List<Vector2Int> _placements;
        private FigureType _userFigureType;

        private Coroutine _undoCoroutine;

        public SinglePlayerGameController(IGridView gridView, Image fader) : base(gridView, fader)
        {
            _solver = new EasySolver(this);
            _placements = new List<Vector2Int>();
        }

        public void SetUser(FigureType figureType) => _userFigureType = figureType;

        public void ShowHint()
        {
            if (ActivePlayerFigureType != _userFigureType)
                return;

            var place = _solver.GetPositionToPlace();
            GridView.PlayHint(place);
        }

        protected override IEnumerator PlaceFigure(Vector2Int position)
        {
            if (_placements.Count > 0 || ActivePlayerFigureType == _userFigureType)
                _placements.Add(position);

            return base.PlaceFigure(position);
        }

        public void UndoMove()
        {
            if (ActivePlayerFigureType != _userFigureType || _placements.Count < 2)
                return;

            Players[ActivePlayerFigureType].Player.SetActive(false);
            ActivePlayerFigureType = FigureType.None;
            _undoCoroutine = Undo().StartCoroutine();
        }

        private IEnumerator Undo()
        {
            yield return RemoveLastPlacedFigureFigure();
            yield return RemoveLastPlacedFigureFigure();
            ActivePlayerFigureType = _userFigureType;
            Players[ActivePlayerFigureType].Player.SetActive(true);
        }

        private IEnumerator RemoveLastPlacedFigureFigure()
        {
            var position = _placements.Last();
            _placements.Remove(position);
            FiguresOnGrid[position] = FigureType.None;
            yield return GridView.PlaceFigureToHolder(position.ToIntPosition(), FigureType.None);
        }

        protected override void Clear()
        {
            _undoCoroutine?.StopCoroutine();
            _undoCoroutine = null;
            _placements.Clear();
            base.Clear();
        }
    }
}