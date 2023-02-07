using System.Numerics;

namespace Backrooms.Assets.Scripts.World.Config {
    public class HallwayConfig : IHallwayConfig {
        public Vector2 Origin => new Vector2 (_origin.X, _origin.Y);
        private Vector2 _origin;

        public Direction Direction { get; private set; }

        public float Chance { get; private set; }

        public HallwayConfig (Vector2 origin, Direction direction, float chance) {
            _origin = origin;

            Direction = direction;

            Chance = chance;
        }
    }
}
