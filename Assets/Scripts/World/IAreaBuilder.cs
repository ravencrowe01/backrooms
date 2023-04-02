using Backrooms.Assets.Scripts.World.Config;

namespace Backrooms.Assets.Scripts.World {
    public interface IAreaBuilder {
        IAreaConfig BuildArea (int rng);
        IAreaBuilder WithChunkSize (int size);
        IAreaBuilder WithSize (int size);
        IAreaBuilder WithRoomSize (int size);
    }
}
