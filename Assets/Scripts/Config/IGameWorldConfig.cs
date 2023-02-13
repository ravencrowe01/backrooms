using Backrooms.Assets.Scripts.World;
using UnityEngine;

namespace Backrooms.Assets.Scripts.Config {
    public interface IGameWorldConfig {
        int StartingAreaWidth { get; }
        int StartingAreaHeight { get; }

        int ChunkWidth { get; }
        int ChunkHeight { get; }

        int RoomSize { get; }

        int Seed { get; }

        Chunk ChunkBase { get; }

        ChunkRoot ChunkRoot { get; }
    }
}
