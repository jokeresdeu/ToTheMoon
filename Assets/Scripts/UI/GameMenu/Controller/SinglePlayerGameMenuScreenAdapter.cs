using System.Collections.Generic;
using UI.GameMenu.Model;
using UI.GameMenu.View;
using UI.Texts.Provider;

namespace UI.GameMenu.Controller
{
    public sealed class SinglePlayerGameMenuScreenAdapter : BaseGameMenuScreenAdapter
    {
        private readonly SinglePlayerGameMenuScreenView _view;
        private readonly SinglePlayerGameMenuScreenModel _model;

        public SinglePlayerGameMenuScreenAdapter(SinglePlayerGameMenuScreenView view, ITextProvider textProvider,
            SinglePlayerGameMenuScreenModel model)
            : base(view, textProvider, model)
        {
            _view = view;
            _model = model;
        }

        public override void Initialize(List<object> data)
        {
            base.Initialize(data);
            _view.UndoRequested += _model.Undo;
            _view.HintRequested += _model.ShowHint;
        }

        public override void Complete()
        {
            base.Complete();
            _view.UndoRequested -= _model.Undo;
            _view.HintRequested -= _model.ShowHint;
        }
    }
}