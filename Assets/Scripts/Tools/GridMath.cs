using System.Collections.Generic;
using Game.Enum;
using UnityEngine;

namespace Tools
{
    public static class GridMath
    {
        public const int GridSideSize = 3;

        private static readonly List<Vector2Int> MainDiagonal = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(1, 1),
            new Vector2Int(2, 2)
        };

        private static readonly List<Vector2Int> SideDiagonal = new List<Vector2Int>
        {
            new Vector2Int(0, 2),
            new Vector2Int(1, 1),
            new Vector2Int(2, 0)
        };

        public static bool TryGetFinishedLine(this Dictionary<Vector2Int, FigureType> figuresOnGrid, Vector2Int lastPlacementPosition,
            out List<Vector2Int> finishedLine)
        {
            finishedLine = null;
            if (figuresOnGrid[lastPlacementPosition] == FigureType.None)
                return false;
            
            if (figuresOnGrid.TryGetFinishedRow(lastPlacementPosition, out finishedLine) ||
                figuresOnGrid.TryGetFinishedColumn(lastPlacementPosition, out finishedLine))
                return true;

            if (figuresOnGrid.IfSideDiagonalCompleted())
            {
                finishedLine = SideDiagonal;
                return true;
            }

            if (!figuresOnGrid.IfMainDiagonalCompleted()) 
                return false;
            
            finishedLine = MainDiagonal;
            return true;
        }

        public static int ToIntPosition(this Vector2Int position) => position.y * GridSideSize + position.x;
        public static Vector2Int ToMatrixPosition(this int position) => new Vector2Int(position % GridSideSize, position / GridSideSize);
        
        private static bool TryGetFinishedRow(this Dictionary<Vector2Int, FigureType> figuresOnGrid,
            Vector2Int lastPlacementPosition, out List<Vector2Int> finishedLine)
        {
            var figureInLine = 0;
            finishedLine = new List<Vector2Int>();
            var figureType = figuresOnGrid[lastPlacementPosition];
            for (var x = 0; x < GridSideSize; x++)
            {
                var position = new Vector2Int(x, lastPlacementPosition.y);
                if (figureType != figuresOnGrid[position])
                    break;
                finishedLine.Add(position);
                figureInLine++;
            }

            return figureInLine == GridSideSize;
        }

        private static bool TryGetFinishedColumn(this Dictionary<Vector2Int, FigureType> figuresOnGrid,
            Vector2Int lastPlacementPosition, out List<Vector2Int> finishedLine)
        {
            var figureInLine = 0;
            finishedLine = new List<Vector2Int>();
            var figureType = figuresOnGrid[lastPlacementPosition];
            for (var y = 0; y < GridSideSize; y++)
            {
                var position = new Vector2Int(lastPlacementPosition.x, y);
                if (figureType != figuresOnGrid[position])
                    break;
                finishedLine.Add(position);
                figureInLine++;
            }

            return figureInLine == GridSideSize;
        }

        private static bool IfMainDiagonalCompleted(this Dictionary<Vector2Int, FigureType> figuresOnGrid)
        {
            var figureType = figuresOnGrid[new Vector2Int(0, 0)];
            if (figureType == FigureType.None)
                return false;
            
            for (var i = 1; i < GridSideSize; i++)
            {
                if (figureType != figuresOnGrid[new Vector2Int(i, i)])
                    return false;
            }

            return true;
        }
        
        private static bool IfSideDiagonalCompleted(this Dictionary<Vector2Int, FigureType> figuresOnGrid)
        {
            var figureType = figuresOnGrid[new Vector2Int(0, GridSideSize- 1)];
            if (figureType == FigureType.None)
                return false;
            
            for (var i = 1; i < GridSideSize; i++)
            {
                if (figureType != figuresOnGrid[new Vector2Int(i, GridSideSize - i - 1)])
                    return false;
            }

            return true;
        }
    }
}