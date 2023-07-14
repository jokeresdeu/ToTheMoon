using System;
using UI.Texts.Enum;
using UnityEngine;

namespace UI.Texts.Data
{
    [Serializable]
    public sealed class TextKeyPair
    {
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public TextKey Key { get; private set; }
    }
}