using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Enum;
using Game.Grid;
using Game.Player;
using Game.Solver;
using Services;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class BaseGameController : IFiguresGridController
    {
        private const float FadeTime = 1f;
        protected readonly IGridView GridView;
        private readonly Image _fader;

        protected Dictionary<FigureType, PlayerConfig> Players;
        protected FigureType ActivePlayerFigureType;

        private Coroutine _startLevelCoroutine;
        private Coroutine _placeFigureCoroutine;

        public Dictionary<Vector2Int, FigureType> FiguresOnGrid { get; }

        public event Action<FigureType> ActivePlayerChanged;
        public event Action<FigureType> LevelEnded;
        public event Action LevelStarted;

        public BaseGameController(IGridView gridView, Image fader)
        {
            FiguresOnGrid = new Dictionary<Vector2Int, FigureType>();
            for (var y = 0; y < GridMath.GridSideSize; y++)
            {
                for (var x = 0; x < GridMath.GridSideSize; x++)
                {
                    FiguresOnGrid[new Vector2Int(x, y)] = FigureType.None;
                }
            }

            GridView = gridView;
            _fader = fader;
        }

        public void StartGame(Dictionary<FigureType, PlayerConfig> players)
        {
            Time.timeScale = 1f;
            Players = players;
            GridView.FigurePlacingRequested += PlaceFigure;
            GridView.SetColors(players[FigureType.Cross].Color, players[FigureType.Circle].Color);
            _startLevelCoroutine = StartLevel().StartCoroutine();
        }

        public void RestartGame()
        {
            Clear();
            _startLevelCoroutine = StartLevel().StartCoroutine();
        }

        public void CloseGame()
        {
            Clear();
            GridView.Clear();
            GridView.FigurePlacingRequested -= PlaceFigure;
            _fader.gameObject.SetActive(false);
            foreach (var player in Players)
                player.Value.Player.Dispose();
        }

        private IEnumerator StartLevel()
        {
            yield return PlayFade();
            GridView.GameObject.SetActive(true);
            yield return GridView.PlayShow();
            LevelStarted?.Invoke();
            ChangePlayer();
        }

        private IEnumerator PlayFade()
        {
            _fader.gameObject.SetActive(true);
            var color = _fader.color;
            _fader.color = new Color(color.r, color.g, color.b, 0);
            var sequence = DOTween.Sequence();
            sequence.Join(_fader.DOFade(1f, FadeTime / 2));
            sequence.Append(_fader.DOFade(0f, FadeTime / 2));
            sequence.Play();
            yield return new WaitForSeconds(FadeTime);
            _fader.gameObject.SetActive(false);
        }

        private void ChangePlayer()
        {
            ActivePlayerFigureType = ActivePlayerFigureType == FigureType.Cross ? FigureType.Circle : FigureType.Cross;
            Players[ActivePlayerFigureType].Player.SetActive(true);
            ActivePlayerChanged?.Invoke(ActivePlayerFigureType);
        }

        private void PlaceFigure(int gridPosition) =>
            _placeFigureCoroutine = PlaceFigure(gridPosition.ToMatrixPosition()).StartCoroutine();

        protected virtual IEnumerator PlaceFigure(Vector2Int position)
        {
            FiguresOnGrid[position] = ActivePlayerFigureType;
            Players[ActivePlayerFigureType].Player.SetActive(false);
            yield return GridView.PlaceFigureToHolder(position.ToIntPosition(), ActivePlayerFigureType);

            if (FiguresOnGrid.TryGetFinishedLine(position, out var finishedLine))
            {
                yield return PlayWinLevel(finishedLine).StartCoroutine();
                yield break;
            }

            if (FiguresOnGrid.All(element => element.Value != FigureType.None))
            {
                yield return PlayTie();
                yield break;
            }

            ChangePlayer();
        }

        private IEnumerator PlayWinLevel(List<Vector2Int> finishedLine)
        {
            var line = finishedLine.Select(element => element.ToIntPosition()).ToList();
            yield return GridView.PlayWin(line);
            LevelEnded?.Invoke(ActivePlayerFigureType);
        }

        private IEnumerator PlayTie()
        {
            yield return GridView.PlayTie();
            LevelEnded?.Invoke(FigureType.None);
        }

        protected virtual void Clear()
        {
            for (var i = 0; i < FiguresOnGrid.Count; i++)
            {
                var element = FiguresOnGrid.ElementAt(i);
                FiguresOnGrid[element.Key] = FigureType.None;
            }

            _startLevelCoroutine?.StopCoroutine();
            _startLevelCoroutine = null;
            _placeFigureCoroutine?.StopCoroutine();
            _placeFigureCoroutine = null;
            GridView.Clear();
            GridView.GameObject.SetActive(false);
            ActivePlayerFigureType = FigureType.None;
            foreach (var player in Players)
                player.Value.Player.SetActive(false);
        }
    }
}