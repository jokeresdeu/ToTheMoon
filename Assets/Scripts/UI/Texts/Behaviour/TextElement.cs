using TMPro;
using UI.Texts.Enum;
using UnityEngine;

namespace UI.Texts.Behaviour
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class TextElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [field: SerializeField] public TextKey TextKey { get; private set; }
        public void SetText(string text) => _text.text = text;
    }
}