using Backrooms.Assets.Scripts.World.Config;

namespace Backrooms.Assets.Scripts.World {
    public interface IAreaBuilder {
        IAreaConfig BuildArea (int rng);
        IAreaBuilder WithChunkDiminsions (int width, int height);
        IAreaBuilder WithDiminsions (int width, int height);
        IAreaBuilder WithRoomSize (int size);
    }
}
