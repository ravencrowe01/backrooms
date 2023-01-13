using Backrooms.Assets.Scripts.World;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms {
    public class ChunkDatabase : MonoBehaviour {
        [SerializeField]
        private Chunk[] Chunks;

        private static Dictionary<int, Chunk> _chunks = new Dictionary<int, Chunk> ();

        // Start is called before the first frame update
        void Start () {
            foreach(var chunk in Chunks) {
                _chunks.Add (chunk.ID, chunk);
            }
        }

        public static Chunk GetChunk (int id) => _chunks[id];
    }
}
