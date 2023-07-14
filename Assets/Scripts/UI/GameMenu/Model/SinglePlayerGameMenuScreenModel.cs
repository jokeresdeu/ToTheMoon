using Game.Core;
using Game.Enum;
using UI.Texts.Provider;

namespace UI.GameMenu.Model
{
    public sealed class SinglePlayerGameMenuScreenModel : BaseGameMenuScreenModel
    {
        private readonly SinglePlayerGameController _gameController;
        private FigureType _userFigureType;

        public SinglePlayerGameMenuScreenModel(SinglePlayerGameController gameController, ITextProvider textProvider) : base(gameController,
            textProvider) =>
            _gameController = gameController;

        public void ShowHint() => _gameController.ShowHint();
        public void Undo() => _gameController.UndoMove();
    }
}