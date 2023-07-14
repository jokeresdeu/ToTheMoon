using System.Collections.Generic;
using UI.Texts.Enum;
using UnityEngine;

namespace UI.Texts.Data
{
    [CreateAssetMenu(fileName = nameof(TextsKeysStorage), menuName = "Text/TextsKeysStorage", order = 2)]
    public sealed class TextsKeysStorage : ScriptableObject
    {
        [SerializeField] private List<TextKeyPair> _textKeyPairs;

        public string GetText(TextKey textKey)
        {
            var keyPair = _textKeyPairs.Find(element => element.Key == textKey);
            if (keyPair != null)
                return keyPair.Text;

            Debug.LogError($"Text for {textKey} is missing");
            return textKey.ToString();
        }
    }
}