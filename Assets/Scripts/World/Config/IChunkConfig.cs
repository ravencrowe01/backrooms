using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Config {
    public interface IChunkConfig {
        Vector2 Coordinates { get; }
        IReadOnlyList<IHallwayConfig> Hallways { get; }
        int Height { get; }
        IRoomConfig[,] Rooms { get; }
        int Width { get; }

        void AddHallway (Vector2 origin, Direction dir, float chance);
        Dictionary<Direction, List<IRoomConfig>> GetOpenSides ();
        void SetRoom (int x, int y, IRoomConfig room);
    }
}