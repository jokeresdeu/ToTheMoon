using System;
using System.Collections.Generic;
using UI.Core.MVC;
using UI.Enum;
using UnityEngine;

namespace UI.ViewProvider.Data
{
    [CreateAssetMenu(menuName = "UI/ScreenViewStorage", fileName = "ScreenViewsStorage")]
    public sealed class ScreenViewsStorage : ScriptableObject
    {
        [SerializeField] private List<ScreenTypeView> _views;

        public ScreenView GetScreenView(ScreenType screenType) =>
            _views.Find(element => element.ScreenType == screenType)?.ScreenView;

        [Serializable]
        private class ScreenTypeView
        {
            [field: SerializeField] public ScreenType ScreenType { get; private set; }
            [field: SerializeField] public ScreenView ScreenView { get; private set; }
        }
    }
}