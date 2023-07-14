using System.Collections;
using System.Collections.Generic;
using Game.Enum;
using Game.Grid;
using Game.Solver;
using Services;
using UnityEngine;

namespace Game.Player.Controller
{
    public sealed class AiPlayer : IPlayer
    {
        private readonly Dictionary<Vector2Int, FigureType> _figuresTypes;
        private readonly GridView _gridView;
        private Coroutine _turnCoroutine;
        private readonly ISolver _solver;

        public AiPlayer(GridView gridView, ISolver solver)
        {
            _gridView = gridView;
            _solver = solver;
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                _turnCoroutine = TakeTurn().StartCoroutine();
                return;
            }
            _turnCoroutine?.StopCoroutine();
            _turnCoroutine = null;
        }
     
        private IEnumerator TakeTurn()
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            var position = _solver.GetPositionToPlace();
            _gridView.RequestFigurePlacing(position);
        }

        public void Dispose()
        {
            _turnCoroutine?.StopCoroutine();
            _turnCoroutine = null;
        }
    }
}