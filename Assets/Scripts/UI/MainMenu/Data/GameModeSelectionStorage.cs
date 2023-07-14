using System.Collections.Generic;
using UnityEngine;

namespace UI.MainMenu.Data
{
    [CreateAssetMenu(fileName = "GameModeSelectionStorage", menuName = "UI/GameModeSelectionStorage")]
    public sealed class GameModeSelectionStorage : ScriptableObject
    {
        [field: SerializeField] public List<GameModeSelectionSettings> GameModesSelectionSettings { get; private set; }
    }
}