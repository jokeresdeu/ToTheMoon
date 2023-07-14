using System;
using System.Collections.Generic;
using UI.Enum;

namespace UI.Core
{
    public interface IScreenOpener
    {
        event Action<ScreenType, List<object>> OpenScreenRequested;
    }
}