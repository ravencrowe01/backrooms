using Backrooms.Assets.Scripts.World;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.Database {
    public class ChunkRegistry : MonoBehaviour {
        public static ChunkRegistry Instance { get; private set; }

        [SerializeField]
        private Chunk[] _chunkArray;

        public IReadOnlyDictionary<ID, Chunk> Chunks => _chunks;
        private Dictionary<ID, Chunk> _chunks;

        private ChunkRegistry () {
            _chunks = new Dictionary<ID, Chunk> ();
        }

        public void AddChunk (ID id, Chunk chunk) => _chunks.Add (id, chunk);

        public Chunk GetChunk (ID id) => _chunks[id];

        private void Awake () {
            if (Instance != null && Instance != this) {
                Destroy (this);
            }
            else {
                Instance = this;

                if(_chunkArray != null && _chunkArray.Length > 0) {
                    InitRegistry ();
                }
            }
        }

        private void InitRegistry () {
            foreach(var chunk in _chunkArray) {
                _chunks.Add (chunk.ID, chunk);
            }

            _chunkArray = null;
        }
    }
}
