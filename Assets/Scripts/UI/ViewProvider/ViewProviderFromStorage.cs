using UI.Core.MVC;
using UI.Enum;
using UI.ViewProvider.Data;
using UnityEngine;

namespace UI.ViewProvider
{
    public sealed class ViewProviderFromStorage : IViewProvider
    {
        private readonly ScreenViewsStorage _screenViewsStorage;
        private readonly GameObject _uiContainer;
        
        public ViewProviderFromStorage(ScreenViewsStorage screenViewsStorage)
        {
            _uiContainer = new GameObject { name = "UI" };
            _screenViewsStorage = screenViewsStorage;
        }
        public T GetScreenView<T>(ScreenType screenType) where T : ScreenView
        {
            var viewPrefab = _screenViewsStorage.GetScreenView(screenType);
            var view = Object.Instantiate(viewPrefab, _uiContainer.transform, true);
            return view as T;
        }
    }
}