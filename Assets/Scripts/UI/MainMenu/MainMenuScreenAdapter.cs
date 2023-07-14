using System.Collections.Generic;
using UI.Core.MVC;
using UI.Texts.Provider;

namespace UI.MainMenu
{
    public sealed class MainMenuScreenAdapter : ScreenController<MainMenuScreenView>
    {
        private readonly MainMenuScreenModel _model;

        public MainMenuScreenAdapter(MainMenuScreenView view, ITextProvider textProvider, MainMenuScreenModel model) : base(view, textProvider) =>
            _model = model;

        public override void Initialize(List<object> data)
        {
            View.InitializeSelection(_model.GameModesTexts);
            View.SelectGameMode(_model.SelectedModeIndex);
            View.GameModeSelected += _model.SelectGameMode;
            View.StartGameClicked += _model.StartGame;
            base.Initialize(data);
        }

        public override void Complete()
        {
            View.GameModeSelected -= _model.SelectGameMode;
            View.StartGameClicked -= _model.StartGame;
            base.Complete();
        }
    }
}