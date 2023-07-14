using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UI.Core.MVC;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameMenu.View
{
    public class BaseGameMenuScreenView : ScreenView
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private TMP_Text _timer;

        [Header("EndGamePopup")] [SerializeField]
        private GameObject _endGameBlocker;

        [SerializeField] private RectTransform _endGamePopup;
        [SerializeField] private TMP_Text _endGamePopupText;
        [SerializeField] private float _endGameAnimTime;
        [SerializeField] private Vector2 _offScreenPosition;
        [SerializeField] private Vector2 _onScreenPosition;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        public event Action HomeButtonClicked;
        public event Action RestartGameClicked;

        private Tweener _endGameMoveTweener;

        protected override void SubscribeEvents()
        {
            _homeButton.onClick.AddListener(() => HomeButtonClicked?.Invoke());
            _restartGameButton.onClick.AddListener(() => RestartGameClicked?.Invoke());
            _yesButton.onClick.AddListener(() => RestartGameClicked?.Invoke());
            _noButton.onClick.AddListener(() => HomeButtonClicked?.Invoke());
        }

        protected override void UnsubscribeEvents()
        {
            _homeButton.onClick.RemoveAllListeners();
            _restartGameButton.onClick.RemoveAllListeners();
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }

        public void SetTimerText(string timerText) => _timer.text = timerText;
        public void SetActivePlayer(Color color) => _timer.color = color;

        public IEnumerator PlayEndGame(Color color, string text)
        {
            _endGameBlocker.gameObject.SetActive(true);
            _endGamePopup.anchoredPosition = _offScreenPosition;
            _endGamePopup.gameObject.SetActive(true);
            _endGamePopupText.color = color;
            _endGamePopupText.text = text;
            _endGameMoveTweener = _endGamePopup.DOAnchorPos(_onScreenPosition, _endGameAnimTime);
            yield return new WaitForSeconds(_endGameAnimTime);
        }

        public void ResetView()
        {
            _endGameBlocker.gameObject.SetActive(false);
            _endGamePopup.gameObject.SetActive(false);
            _endGamePopup.anchoredPosition = _offScreenPosition;
            _endGameMoveTweener?.Kill();
        }
    }
}