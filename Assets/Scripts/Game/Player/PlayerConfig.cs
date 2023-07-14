using Game.Player.Controller;
using UnityEngine;

namespace Game.Player
{
    public sealed class PlayerConfig
    {
        public IPlayer Player { get; }
        public Color Color { get; }
        public PlayerConfig(IPlayer player, Color color)
        {
            Player = player;
            Color = color;
        }
    }
}