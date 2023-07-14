using UI.Texts.Enum;

namespace UI.Texts.Provider
{
    public interface ITextProvider
    {
        string GetText(TextKey textKey);
    }
}