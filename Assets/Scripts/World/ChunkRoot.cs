using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World.Config;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class ChunkRoot : MonoBehaviour {
        private List<Chunk> _chunks = new List<Chunk> ();

        public Chunk FindChunk (int x, int z) => _chunks.Where (c => c.Coordinates.x == x && c.Coordinates.y == z).FirstOrDefault ();

        public void AddChunk (IChunkConfig chunk, Chunk chunkBase, IRNG rng) {
            var c = Instantiate (chunkBase, transform);
            c.Init (chunk, rng);
            c.transform.position = new Vector3 (chunk.Coordinates.x, 0, chunk.Coordinates.y);
            c.InstantiateRooms ();
        }
    }
}
