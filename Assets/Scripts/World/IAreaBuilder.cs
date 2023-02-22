using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World.Config;

namespace Backrooms.Assets.Scripts.World {
    public interface IAreaBuilder {
        AreaConfig BuildArea (IRNG rng);
        IAreaBuilder WithChunkDiminsions (int width, int height);
        IAreaBuilder WithDiminsions (int width, int height);
        IAreaBuilder WithRoomSize (int size);
    }
}
