using Backrooms.Assets.Scripts.World.Config;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class ChunkRoot : MonoBehaviour {
        private List<Chunk> _chunks = new List<Chunk> ();

        public Chunk FindChunk (int x, int z) => _chunks.Where (c => c.Coordinates.x == x && c.Coordinates.y == z).FirstOrDefault ();

        public void AddChunk (IChunkConfig chunk, Chunk chunkBase, int seed) {
            var c = Instantiate (chunkBase, transform, false);

            _chunks.Add (c);

            c.Init (chunk, seed);

            c.InstantiateRooms ();

            // IDK why this needs to go after the rooms are instantiated.
            // I guess it's something to do with how unity works that I haven't dug into.
            // I can't wait until it doesn't work on other computers :)
            c.transform.position = new Vector3 (chunk.Coordinates.x * 24, 0, chunk.Coordinates.y * 24);
        }
    }
}
