using Backrooms.Assets.Scripts.World;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Config {
    public interface IGameWorldConfig {
        string Name { get; }

        int StartingAreaWidth { get; }
        int StartingAreaHeight { get; }

        int ChunkWidth { get; }
        int ChunkHeight { get; }

        int RoomSize { get; }

        int Seed { get; }

        Chunk ChunkBase { get; }

        ChunkRoot ChunkRoot { get; }

        PlayerControl Player { get; }
    }
}
