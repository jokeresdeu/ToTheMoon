using System.Collections.Generic;
using Game.Enum;
using UnityEngine;

namespace Game.Solver
{
    public interface IFiguresGridController
    {
        Dictionary<Vector2Int, FigureType> FiguresOnGrid { get; }
    }
}