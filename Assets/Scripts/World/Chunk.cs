using Raven.Backrooms.Framework.Word.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class Chunk : MonoBehaviour {
        private Room[,] _rooms;

        protected Chunk () { }

        public Chunk (IChunkConfig config) {
            Init (config);
        }

        private void Init (IChunkConfig config) {
            _rooms = new Room[config.Width, config.Height];

            for(int x = 0; x < config.Width; x++) {
                for(int y = 0; y < config.Height; y++) {
                    var roomConfig = config.Rooms[x, y];
                    // TODO room database time
                }
            }
        }
    }

    public class Room : MonoBehaviour {
        public IReadOnlyDictionary<Direction, ISideStateConfig> SideStates => _sideStates;
        private Dictionary<Direction, ISideStateConfig> _sideStates;

        public Room (ISideStateConfig north, ISideStateConfig south, ISideStateConfig east, ISideStateConfig west) {
            _sideStates = new Dictionary<Direction, ISideStateConfig> {
                {Direction.North, north },
                {Direction.South, south },
                {Direction.East, east },
                {Direction.West, west },
            };
        }

        public Room (IRoomConfig config) {

        }

        private void Init(IRoomConfig config) {

        }
    }
}
