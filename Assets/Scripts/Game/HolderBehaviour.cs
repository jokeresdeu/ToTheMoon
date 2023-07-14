using System;
using System.Collections;
using DG.Tweening;
using Game.Enum;
using Game.Figure;
using Game.TouchController;
using UnityEngine;

namespace Game
{
    public sealed class HolderBehaviour : MonoBehaviour, IClickable
    {
        private const float HintShowTime = 0.6f;
        [SerializeField] private SpriteRenderer _placeHolder;

        private FigureBehaviour _figureBehaviour;
        private Color _color;

        public event Action<HolderBehaviour> HolderClicked;

        private void Start() => _color = _placeHolder.color;

        public void SetFigure(FigureBehaviour figureBehaviour)
        {
            _figureBehaviour = figureBehaviour;
            Transform transform1;
            (transform1 = _figureBehaviour.transform).SetParent(transform);
            transform1.localPosition = Vector3.zero;
            _figureBehaviour.gameObject.SetActive(true);
        }

        public IEnumerator PlayShow()
        {
            if (_figureBehaviour == null)
                yield break;

            yield return _figureBehaviour.PlayAnimation(FigureAnimationState.Show);
        }

        public void Click()
        {
            if (_figureBehaviour != null)
            {
                PlayWrongChoice();
                return;
            }

            HolderClicked?.Invoke(this);
        }


        public IEnumerator PlayRemove()
        {
            if (_figureBehaviour == null)
                yield break;

            yield return _figureBehaviour.PlayAnimation(FigureAnimationState.Hide);
        }

        public Sequence PlayHint()
        {
            var hintSequence = DOTween.Sequence();
            _placeHolder.gameObject.SetActive(true);
            _placeHolder.color = new Color(_color.r, _color.g, _color.b, 0);
            hintSequence.Join(_placeHolder.DOFade(0.8f, HintShowTime / 2));
            hintSequence.Append(_placeHolder.DOFade(0, HintShowTime / 2));
            hintSequence.SetLoops(2);
            hintSequence.onComplete += ResetHint;
            hintSequence.onKill += ResetHint;
            return hintSequence;
        }

        public void RemoveFigure()
        {
            if (_figureBehaviour == null)
                return;

            _figureBehaviour.Hide();
            _figureBehaviour = null;
        }

        private void PlayWrongChoice()
        {
            if (_figureBehaviour == null)
                return;

            StartCoroutine(_figureBehaviour.PlayAnimation(FigureAnimationState.Wrong));
        }

        private void ResetHint()
        {
            _placeHolder.gameObject.SetActive(false);
            _placeHolder.color = _color;
        }
    }
}