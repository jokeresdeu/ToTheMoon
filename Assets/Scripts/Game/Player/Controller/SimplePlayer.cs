using Game.TouchController;
using UnityEngine;

namespace Game.Player.Controller
{
    public sealed class SimplePlayer : IPlayer
    {
        private readonly IInput _input;

        public SimplePlayer(IInput input)
        {
            _input = input;
            _input.Clicked += OnClicked;
        } 

        private bool _active;

        private void OnClicked(Vector2 position)
        {
            if(!_active)
                return;
                
            var collider = Physics2D.OverlapPoint(position);
            if (collider != null && collider.TryGetComponent(out IClickable clickable))
                clickable.Click();
        }

        public void SetActive(bool active) => _active = active;

        public void Dispose() => _input.Clicked -= OnClicked;
    }
}