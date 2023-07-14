using System;
using System.Collections;
using DG.Tweening;
using Game.Enum;
using UnityEngine;

namespace Game.Figure
{
    public abstract class FigureBehaviour : MonoBehaviour
    {
        private Tweener _tweener;
        [field: SerializeField] public FigureType FigureType { get; private set; }
        
        public event Action<FigureBehaviour> Hided;
        public abstract void SetColor(Color color);
        public virtual void Hide() => Hided?.Invoke(this);
        public abstract IEnumerator PlayAnimation(FigureAnimationState animationState);

        protected IEnumerator PlayWrong()
        {
           _tweener?.Kill();
           transform.localPosition = Vector3.zero;
           _tweener = transform.DOPunchPosition(Vector3.right * 0.3f, 0.5f);
           yield return new WaitForSeconds(_tweener.Duration());
        }
    }
}