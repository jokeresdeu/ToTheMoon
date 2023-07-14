using System;
using System.Collections.Generic;
using UI.Enum;
using UI.Texts.Provider;

namespace UI.Core.MVC
{
    public abstract class ScreenController<TView> : IScreenController where TView : ScreenView
    {
        protected readonly TView View;
        private readonly ITextProvider _textProvider;

        protected ScreenController(TView view, ITextProvider textProvider)
        {
            View = view;
            _textProvider = textProvider;
        }

        public event Action CloseRequested;
        public event Action<ScreenType, List<object>> OpenScreenRequested;

        public virtual void Initialize(List<object> data)
        {
            View.Show();
            foreach (var textElement in View.TextElements)
                textElement.SetText(_textProvider.GetText(textElement.TextKey));
        }


        public virtual void Complete() =>
            View.Hide();

        protected void RequestClose() => CloseRequested?.Invoke();

        protected void RequestScreen(ScreenType characterScreenType, List<object> data = null)
            => OpenScreenRequested?.Invoke(characterScreenType, data);
    }
}