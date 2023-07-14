using System;
using System.Collections.Generic;
using UI.Enum;

namespace UI.Core.MVC
{
    public interface IScreenController : IScreenOpener
    {
        event Action CloseRequested;
        void Initialize(List<object> data);
        void Complete();
    }
}