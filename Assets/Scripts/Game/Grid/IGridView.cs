using System;
using System.Collections;
using System.Collections.Generic;
using Game.Enum;
using UnityEngine;

namespace Game.Grid
{
    public interface IGridView
    {
        GameObject GameObject { get; }
        event Action<int> FigurePlacingRequested;
        void SetColors(Color crossColor, Color circleColor);
        IEnumerator PlayShow();
        void RequestFigurePlacing(int index);
        IEnumerator PlaceFigureToHolder(int position, FigureType figureType);
        IEnumerator PlayWin(List<int> line);
        void PlayHint(int position);
        IEnumerator PlayTie();
        void Clear();
    }
}