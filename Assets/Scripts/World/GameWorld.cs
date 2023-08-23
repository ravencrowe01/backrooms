using System.Collections.Generic;
using System.Collections;
using Syste.Exceptions;
using Unity;

namespace Raven.backrooms.World {
    public class GameWorld {
        public long Seed { get; private set;}

        public IDictionary<Coordinate, Chunk> GeneratedChunks => (IReadonlyDictionary<Coordinate, Chunk>) _genedChunks;
        private Dictionary<Coordinate, Chunk> _genedChunks = new Dictionary<Coordinate, Chunk>();

        public Player Player { get; private set; }
        public Vector3 PlayerPos => Player.GetPos();
        
        public GameWorld(string name, long seed, Player player) {
            Name = name;
            Seed = seed;
            Player = player;
        }

        public void Update() {
            throw new NotImplementedException();
        }

        private bool ShouldGenNewChunk(){
            throw new NotImplementedException();
        }

        private Vector2Int WorldCordsToChunkCords(Vector3 cords) {
            throw new NotImplementedException();
        }

        private Chunk GenerateChunk (int x, int y) {
            throw new NotImplementedException();
        }
    }
}