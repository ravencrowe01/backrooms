using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World.Config;
using System.Numerics;

namespace Backrooms.Assets.Scripts.World {
    public interface IChunkBuilder {
        IChunkConfig BuildChunk (IRNG rng);
        IChunkBuilder WithConnection (Vector2 connectedRoom, Direction dir, int index);
        IChunkBuilder WithCoordinates (Vector2 cords);
        IChunkBuilder WithDiminsions (int width, int height);
        IChunkBuilder WithHallway (Vector2 start, float chance, Direction dir);
        IChunkBuilder WithRoomSize (int amt = 1);
    }
}