using Backrooms.Assets.Scripts.World;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Config {
    public interface IGameWorldConfig {
        string Name { get; }

        int StartingAreaSize { get; }

        int ChunkSize { get; }
        int ChunkHeight { get; }

        int RoomSize { get; }

        int Seed { get; }

        Chunk ChunkBase { get; }

        ChunkRoot ChunkRoot { get; }

        PlayerControl Player { get; }
    }
}
