using System;
using UnityEngine;

namespace Game.TouchController
{
    public sealed class EditorInput : IInput
    {
        private readonly Camera _camera;
        public event Action<Vector2> Clicked;

        public EditorInput()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            if (Input.GetButtonDown("Fire1"))
                Clicked?.Invoke(_camera.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}