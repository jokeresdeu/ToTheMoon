using System;
using System.Collections.Generic;
using System.Linq;
using UI.Texts.Behaviour;
using UnityEngine;

namespace UI.Core.MVC
{
    public abstract class ScreenView : MonoBehaviour
    {
        [SerializeField] private Canvas _root;
        [field: SerializeField] public List<TextElement> TextElements { get; private set; }

        private void Awake() => SubscribeEvents();
        private void OnValidate() => TextElements = GetComponentsInChildren<TextElement>().ToList();

        private void OnDestroy() => UnsubscribeEvents();

        public virtual void Show() => _root.enabled = true;
        public virtual void Hide() => _root.enabled = false;

        protected abstract void SubscribeEvents();
        protected abstract void UnsubscribeEvents();
    }
}