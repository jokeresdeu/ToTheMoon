using System;

namespace Game.Player.Controller
{
    public interface IPlayer : IDisposable
    {
        void SetActive(bool active);
    }
}