using UI.Core.MVC;
using UI.Enum;

namespace UI.ViewProvider
{
    public interface IViewProvider
    {
        T GetScreenView<T>(ScreenType screenType) where T : ScreenView;
    }
}