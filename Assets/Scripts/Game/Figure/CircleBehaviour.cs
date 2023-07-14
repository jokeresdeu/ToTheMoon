using System.Collections;
using Game.Enum;
using Shapes;
using UnityEngine;

namespace Game.Figure
{
    [RequireComponent(typeof(Disc))]
    public sealed class CircleBehaviour : FigureBehaviour
    {
        [SerializeField] private Disc _disc;
        [SerializeField] private float _showTime;
        [SerializeField] private float _timeStep;

        private float _startRadian;
        private float _endRadian;
        private float _angleStep;

        private FigureAnimationState _currentAnimationState;

        private void Awake()
        {
            _disc = GetComponent<Disc>();
            _startRadian = _disc.AngRadiansStart;
            _endRadian = _disc.AngRadiansEnd;
            _angleStep = (_endRadian - _startRadian) / _showTime * _timeStep;
        }

        public override void SetColor(Color color) => _disc.Color = color;

        public override IEnumerator PlayAnimation(FigureAnimationState animationState)
        {
            if (_currentAnimationState != FigureAnimationState.Idle)
                yield break;

            _currentAnimationState = animationState;
            switch (_currentAnimationState)
            {
                case FigureAnimationState.Show:
                    yield return PlayDraw();
                    break;
                case FigureAnimationState.Hide:
                    yield return PlayHide();
                    break;
                case FigureAnimationState.Wrong:
                    yield return PlayWrong();
                    break;
            }

            _currentAnimationState = FigureAnimationState.Idle;
            if (animationState == FigureAnimationState.Hide)
                Hide();
        }

        private IEnumerator PlayDraw()
        {
            _disc.AngRadiansEnd = _startRadian;
            yield return PlayMove(_endRadian, 1);
        }

        private IEnumerator PlayHide()
        {
            _disc.AngRadiansEnd = _endRadian;
            yield return PlayMove(_startRadian, -1);
        }

        public override void Hide()
        {
            base.Hide();
            _disc.AngRadiansStart = _startRadian;
            _disc.AngRadiansEnd = _endRadian;
        }

        private IEnumerator PlayMove(float endValue, int direction)
        {
            while (_disc.AngRadiansEnd != endValue)
            {
                var newAngle = _disc.AngRadiansEnd + direction * _angleStep;
                var angleDif = Mathf.Abs(Mathf.Abs(newAngle) - Mathf.Abs(_endRadian));
                var angleOffset = Mathf.Abs(_angleStep) * 0.51f;
                if (angleDif < angleOffset)
                    newAngle = endValue;
                _disc.AngRadiansEnd = newAngle;
                yield return new WaitForSeconds(_timeStep);
            }
        }
    }
}