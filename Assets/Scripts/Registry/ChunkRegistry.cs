using Backrooms.Assets.Scripts.World;
using System.Collections.Generic;

namespace Backrooms.Assets.Scripts.Database {
    public class ChunkRegistry {
        public static ChunkRegistry Instance {
            get {
                if (_instance == null) {
                    lock (_instanceLock) {
                        _instance ??= new ChunkRegistry ();
                    }
                }

                return _instance;
            }
        }

        private static ChunkRegistry _instance;
        private static object _instanceLock = new object ();

        public IReadOnlyDictionary<ID, Chunk> Chunks => _chunks;
        private Dictionary<ID, Chunk> _chunks;

        private ChunkRegistry () {
            _chunks = new Dictionary<ID, Chunk> ();
        }

        public void AddChunk (ID id, Chunk chunk) => _chunks.Add (id, chunk);

        public Chunk GetChunk (ID id) => _chunks[id];
    }
}
