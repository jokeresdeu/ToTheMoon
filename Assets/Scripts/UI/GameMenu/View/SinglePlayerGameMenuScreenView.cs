using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameMenu.View
{
    public sealed class SinglePlayerGameMenuScreenView : BaseGameMenuScreenView
    {
        [SerializeField] private Button _hintButton;
        [SerializeField] private Button _undoButton;

        public event Action HintRequested;
        public event Action UndoRequested;
        
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            _hintButton.onClick.AddListener(()=> HintRequested?.Invoke());
            _undoButton.onClick.AddListener(()=> UndoRequested?.Invoke());
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            _hintButton.onClick.RemoveAllListeners();
            _undoButton.onClick.RemoveAllListeners();
        }
    }
}