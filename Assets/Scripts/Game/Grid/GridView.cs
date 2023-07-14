using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Enum;
using Game.Figure;
using UnityEngine;

namespace Game.Grid
{
    public sealed class GridView : MonoBehaviour, IGridView
    {
        [SerializeField] private Transform _figuresHolder;

        [Header("Animation")] [SerializeField] private AnimationClip _showAnimation;
        [SerializeField] private float _endLevelAnimationTime = 0.5f;

        private List<HolderBehaviour> _holders;
        private List<Vector2> _holdersOriginalPositions;
        private List<FigureBehaviour> _figures;

        public GameObject GameObject => gameObject;

        public event Action<int> FigurePlacingRequested;

        private Sequence _endSequence;
        private Sequence _hintSequence;

        public void Initialize(CrossBehaviour crossBehaviour, CircleBehaviour circleBehaviour)
        {
            _holders = GetComponentsInChildren<HolderBehaviour>(true).ToList();
            _holdersOriginalPositions = new List<Vector2>();
            foreach (var holder in _holders)
            {
                holder.HolderClicked += OnHolderClicked;
                _holdersOriginalPositions.Add(holder.transform.position);
            }

            _figures = new List<FigureBehaviour>();
            CreateFigures(5, crossBehaviour);
            CreateFigures(4, circleBehaviour);
        }

        private void OnDestroy()
        {
            foreach (var holder in _holders)
                holder.HolderClicked -= OnHolderClicked;

            foreach (var figure in _figures)
                figure.Hided -= OnFigureHidden;
            
            StopAllCoroutines();
        }

        public void SetColors(Color crossColor, Color circleColor)
        {
            foreach (var figure in _figures)
                figure.SetColor(figure.FigureType == FigureType.Cross ? crossColor : circleColor);
        }

        public IEnumerator PlayShow()
        {
            yield return new WaitForSeconds(_showAnimation.length);
        }

        public void RequestFigurePlacing(int index) => FigurePlacingRequested?.Invoke(index);

        public IEnumerator PlaceFigureToHolder(int position, FigureType figureType)
        {
            _hintSequence?.Kill();
            var holder = _holders[position];
            if (figureType == FigureType.None)
            {
                yield return holder.PlayRemove();
                holder.RemoveFigure();
                yield break;
            }

            holder.SetFigure(GetFigure(figureType));
            yield return holder.PlayShow();
        }

        public IEnumerator PlayWin(List<int> line)
        {
            _endSequence = DOTween.Sequence();
            foreach (var position in line)
                _endSequence.Append(_holders[position].transform.DOPunchPosition(Vector3.right * 0.3f, 0.5f));
            _endSequence.Play();
            yield return new WaitForSeconds(_endSequence.Duration());
        }

        public void PlayHint(int position)
        {
            _hintSequence?.Kill();
            _hintSequence = _holders[position].PlayHint();
        }

        public IEnumerator PlayTie()
        {
            _endSequence = DOTween.Sequence();
            foreach (var holder in _holders)
            {
                var holderSequence = DOTween.Sequence();
                holderSequence.Join(holder.transform.DOScale(1.1f, _endLevelAnimationTime / 2));
                holderSequence.Append(holder.transform.DOScale(1, _endLevelAnimationTime / 2));
                _endSequence.Join(holderSequence);
            }

            _endSequence.Play();
            yield return new WaitForSeconds(_endLevelAnimationTime);
        }

        public void Clear()
        {
            for (var i = 0; i < _holders.Count; i++)
            {
                _holders[i].RemoveFigure();
                _holders[i].transform.position = _holdersOriginalPositions[i];
            }
        }

        private void CreateFigures(int count, FigureBehaviour figureBehaviour)
        {
            for (var i = 0; i < count; i++)
            {
                var figure = Instantiate(figureBehaviour, _figuresHolder);
                figure.gameObject.SetActive(false);
                _figures.Add(figure);
            }
        }

        private void OnHolderClicked(HolderBehaviour holderBehaviour) =>
            FigurePlacingRequested?.Invoke(_holders.IndexOf(holderBehaviour));

        private FigureBehaviour GetFigure(FigureType figureType)
        {
            var figure = _figures.Find(element => element.FigureType == figureType);
            _figures.Remove(figure);
            figure.Hided += OnFigureHidden;
            return figure;
        }

        private void OnFigureHidden(FigureBehaviour figureBehaviour)
        {
            figureBehaviour.transform.SetParent(_figuresHolder);
            figureBehaviour.gameObject.SetActive(false);
            figureBehaviour.Hided -= OnFigureHidden;
            _figures.Add(figureBehaviour);
        }
    }
}