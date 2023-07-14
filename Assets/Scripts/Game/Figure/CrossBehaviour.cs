using System.Collections;
using System.Collections.Generic;
using Game.Enum;
using UnityEngine;

namespace Game.Figure
{
    public sealed class CrossBehaviour : FigureBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private List<SpriteRenderer> _sprites;

        private FigureAnimationState _currentAnimationState;

        public override void SetColor(Color color)
        {
            foreach (var sprite in _sprites)
                sprite.color = color;
        }

        public override IEnumerator PlayAnimation(FigureAnimationState animationState)
        {
            if(_currentAnimationState != FigureAnimationState.Idle)
                yield break;

            if (animationState == FigureAnimationState.Wrong)
            {
                yield return PlayWrong();
                _currentAnimationState = FigureAnimationState.Idle;
                yield break;
            }
            
            _currentAnimationState = animationState;
            _animator.SetInteger(nameof(FigureAnimationState), (int)_currentAnimationState);
            yield return new WaitUntil(()=> _currentAnimationState == FigureAnimationState.Idle);
            
            if(animationState == FigureAnimationState.Hide)
                Hide();
        }

        public override void Hide()
        {
            OnAnimationEnded();
            base.Hide();
        }

        private void OnAnimationEnded()
        {
            _currentAnimationState = FigureAnimationState.Idle;
            _animator.SetInteger(nameof(FigureAnimationState), (int)_currentAnimationState);
        }
    }
}