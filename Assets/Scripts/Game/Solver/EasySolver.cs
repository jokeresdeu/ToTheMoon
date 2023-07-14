using System.Linq;
using Game.Enum;
using Tools;
using UnityEngine;

namespace Game.Solver
{
    public sealed class EasySolver : ISolver
    {
        private readonly IFiguresGridController _figuresGridController;

        public EasySolver(IFiguresGridController figuresGridController) => _figuresGridController = figuresGridController;

        public int GetPositionToPlace()
        {
            var freeCells = _figuresGridController.FiguresOnGrid.
                Where(element => element.Value == FigureType.None).
                Select(element => element.Key).ToList();
            return freeCells[Random.Range(0, freeCells.Count)].ToIntPosition();
        } 
    }
}