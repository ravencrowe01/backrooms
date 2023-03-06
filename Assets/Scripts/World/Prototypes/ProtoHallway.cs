namespace Backrooms.Assets.Scripts.World.Prototypes {
    public struct ProtoHallway {
        public float BuildChance { get; private set; }
        public Direction Direction { get; private set; }

        public ProtoHallway (float chance, Direction direction) {
            BuildChance = chance;
            Direction = direction;
        }
    }
}
