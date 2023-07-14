using System;
using System.Collections;
using System.Collections.Generic;
using Game.Enum;
using Services;
using UI.Core.MVC;
using UI.Enum;
using UI.GameMenu.Model;
using UI.GameMenu.View;
using UI.Texts.Provider;
using UnityEngine;

namespace UI.GameMenu.Controller
{
    public class BaseGameMenuScreenAdapter : ScreenController<BaseGameMenuScreenView>
    {
        private readonly BaseGameMenuScreenModel _model;
        private float _startTime;
        private Coroutine _timerCoroutine;

        public BaseGameMenuScreenAdapter(BaseGameMenuScreenView view, ITextProvider textProvider, BaseGameMenuScreenModel model) :
            base(view, textProvider) => _model = model;

        public override void Initialize(List<object> data)
        {
            if (!(data.Find(element => element is Dictionary<FigureType, Color>) is Dictionary<FigureType, Color> figuresColors))
                throw new NullReferenceException($"{nameof(BaseGameMenuScreenAdapter)} needs figures color settings to work properly");

            _model.SetPlayerColorSetting(figuresColors);
            _model.LevelStarted += OnLevelStarted;
            _model.LevelEnded += OnLevelEnded;
            _model.ActivePlayerChanged += OnActivePlayerChanged;
            View.RestartGameClicked += OnRestartClicked;
            View.HomeButtonClicked += OnHomeButtonClicked;
            base.Initialize(data);
        }

        private void OnActivePlayerChanged(Color color) => View.SetActivePlayer(color);

        public override void Complete()
        {
            _model.LevelStarted -= OnLevelStarted;
            _model.LevelEnded -= OnLevelEnded;
            _model.ActivePlayerChanged -= OnActivePlayerChanged;
            View.RestartGameClicked -= OnRestartClicked;
            View.HomeButtonClicked -= OnHomeButtonClicked;
            _timerCoroutine?.StopCoroutine();
            base.Complete();
        }

        private void OnHomeButtonClicked()
        {
            _model.CloseGame();
            View.ResetView();
            RequestScreen(ScreenType.MainMenu);
        }

        private void OnRestartClicked()
        {
            _model.RestartGame();
            View.ResetView();
        }

        private void OnLevelStarted()
        {
            _startTime = Time.time;
            _timerCoroutine = TimerCoroutine().StartCoroutine();
        }

        private void OnLevelEnded(string text, Color textColor)
        {
            _timerCoroutine?.StopCoroutine();
            View.PlayEndGame(textColor, text).StartCoroutine();
        }

        private IEnumerator TimerCoroutine()
        {
            while (true)
            {
                var time = Time.time - _startTime;
                var timeSpan = TimeSpan.FromSeconds(time);
                var timeText = timeSpan.Hours == 0
                    ? $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}"
                    : $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                View.SetTimerText(timeText);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}