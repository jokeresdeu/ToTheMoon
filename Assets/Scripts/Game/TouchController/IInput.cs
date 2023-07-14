using System;
using UnityEngine;

namespace Game.TouchController
{
    public interface IInput
    {
        event Action<Vector2> Clicked;
        void Update();
    }
}