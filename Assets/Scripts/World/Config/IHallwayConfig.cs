using System.Numerics;

namespace Backrooms.Assets.Scripts.World.Config {
    public interface IHallwayConfig {
        float Chance { get; }
        Direction Direction { get; }
        Vector2 Origin { get; }
    }
}