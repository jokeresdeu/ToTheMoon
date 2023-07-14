using System;
using Game.Enum;
using UI.Texts.Enum;
using UnityEngine;

namespace UI.MainMenu.Data
{
    [Serializable]
    public sealed class GameModeSelectionSettings
    {
        [field: SerializeField] public GameMode GameMode { get; private set; }
        [field: SerializeField] public TextKey TextKey { get; private set; }
    }
}