using UI.Texts.Data;
using UI.Texts.Enum;
using UnityEngine;

namespace UI.Texts.Provider
{
    public sealed class LocalTextProvider : ITextProvider
    {
        private readonly TextsKeysStorage _textsKeysStorage;

        public LocalTextProvider() => _textsKeysStorage = Resources.Load<TextsKeysStorage>(nameof(TextsKeysStorage));

        public string GetText(TextKey textKey) => _textsKeysStorage.GetText(textKey);
    }
}