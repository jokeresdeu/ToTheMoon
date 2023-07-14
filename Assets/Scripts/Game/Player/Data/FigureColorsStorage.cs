using System.Collections.Generic;
using Game.Enum;
using UnityEngine;

namespace Game.Player.Data
{
    [CreateAssetMenu(fileName = nameof(FigureColorsStorage), menuName = "PlayerSettings/FigureColorsStorage")]
    public sealed class FigureColorsStorage : ScriptableObject
    {
        [SerializeField] private Color _crossColor;
        [SerializeField] private Color _circleColor;
        [SerializeField] private Color _defaultColor;

        public Dictionary<FigureType, Color> FiguresColors => new Dictionary<FigureType, Color>
        {
            { FigureType.Cross, _crossColor },
            { FigureType.Circle, _circleColor },
            { FigureType.None, _defaultColor }
        };
    }
}