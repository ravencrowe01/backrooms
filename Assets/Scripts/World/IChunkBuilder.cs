using System.Collections.Generic;

namespace Backrooms.Assets.Scripts.World {
    public interface IChunkBuilder {
        Dictionary<Direction, float> HallwayBuildChances { get; }
        RoomConfig[,] Rooms { get; }

        void AddConnections (IEnumerable<Direction> connections);
        void AddHallwayConnections (IDictionary<Direction, float> connections);
        void BuildRooms ();
    }
}