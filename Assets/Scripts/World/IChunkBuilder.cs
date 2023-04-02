using Backrooms.Assets.Scripts.World.Config;
using Backrooms.Assets.Scripts.World.Prototypes;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public interface IChunkBuilder {
        IChunkConfig BuildChunk (int seed);
        ProtoChunk BuildChunkAsPrototype (int seed);
        IChunkBuilder WithConnection (Vector2 connectedRoom, Direction dir);
        IChunkBuilder WithCoordinates (Vector2 cords);
        IChunkBuilder WithDiminsions (int size);
        IChunkBuilder WithHallway (Vector2 start, float chance, Direction dir);
        IChunkBuilder WithRoomSize (int amt = 1);
    }
}