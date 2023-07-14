using System;
using System.Collections.Generic;
using Game;
using UI.Core.MVC;
using UI.Enum;
using UI.GameMenu.Controller;
using UI.GameMenu.Model;
using UI.GameMenu.View;
using UI.MainMenu;
using UI.Texts.Provider;
using UI.ViewProvider;

namespace UI.Core.Context
{
    public sealed class UIContext : IDisposable
    {
        private readonly Dictionary<ScreenType, IScreenController> _controllers;
        private readonly IViewProvider _viewProvider;
        private readonly List<IScreenOpener> _screenOpeners;
        private readonly GameLauncher _gameLauncher;
        private readonly ITextProvider _textProvider;
       
        private ScreenType _currentScreenType;
        
        public UIContext(IViewProvider viewProvider, GameLauncher gameLauncher)
        {
            _screenOpeners = new List<IScreenOpener>();
            _controllers = new Dictionary<ScreenType, IScreenController>();
            _viewProvider = viewProvider;
            _textProvider = new LocalTextProvider();
            _gameLauncher = gameLauncher;
        }

        public void RegisterScreenOpener(IScreenOpener screenOpener)
        {
            screenOpener.OpenScreenRequested += OpenOpenScreen;
            _screenOpeners.Add(screenOpener);
        }
        
        public void OpenOpenScreen(ScreenType screenType, List<object> data = null)
        {
            var previousScreenType = _currentScreenType;
            if (previousScreenType != ScreenType.None)
                CloseCurrentScreen();
            
            if(previousScreenType == screenType)
                return;
            
            if (!_controllers.TryGetValue(screenType, out IScreenController screenController))
            {
                screenController = GetController(screenType);
                screenController.CloseRequested += CloseCurrentScreen;
                screenController.OpenScreenRequested += OpenOpenScreen;
                _controllers.Add(screenType, screenController);
            }

            _currentScreenType = screenType;
            screenController.Initialize(data);
        }
        
        private void CloseCurrentScreen()
        {
            if (_currentScreenType == ScreenType.None)
                return;
            
            var controller = _controllers[_currentScreenType];
            controller.Complete();
            _currentScreenType = ScreenType.None;
        }

        private IScreenController GetController(ScreenType screenType)
        {
            switch (screenType)
            {
                case ScreenType.MainMenu:
                    return new MainMenuScreenAdapter(_viewProvider.GetScreenView<MainMenuScreenView>(ScreenType.MainMenu), _textProvider,
                        new MainMenuScreenModel(_gameLauncher, _textProvider));
                case ScreenType.TwoPlayersGameMenu:
                    return new BaseGameMenuScreenAdapter(
                        _viewProvider.GetScreenView<BaseGameMenuScreenView>(ScreenType.TwoPlayersGameMenu), _textProvider, 
                        new BaseGameMenuScreenModel(_gameLauncher.BaseGameController, _textProvider));
                case ScreenType.SinglePlayerGameMenu:
                    return new SinglePlayerGameMenuScreenAdapter(
                        _viewProvider.GetScreenView<SinglePlayerGameMenuScreenView>(ScreenType.SinglePlayerGameMenu),
                        _textProvider, new SinglePlayerGameMenuScreenModel(_gameLauncher.SinglePlayerGameController, _textProvider));
            }

            throw new NullReferenceException($"Screen of type {screenType} is not implemented and can't be opened");
        }
        
        public void Dispose()
        {
            foreach(var screenOpener in _screenOpeners)
                screenOpener.OpenScreenRequested -= OpenOpenScreen;

            foreach (var controller in _controllers.Values)
            {
                controller.CloseRequested -= CloseCurrentScreen;
                controller.OpenScreenRequested -= OpenOpenScreen;
            }
        }
    }
}