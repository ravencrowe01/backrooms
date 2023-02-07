using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World;
using UnityEngine;

namespace Backrooms.Assets.Scripts {
    public class DebugChunkGenerator : MonoBehaviour {
        public GameObject Chunk;
        public GameObject ChunkRoot;
        public int Test;

        private void Update () {
            if (Input.GetKeyDown (KeyCode.R)) {
                var builder = new ChunkBuilder ();

                builder.WithDiminsions (3, 3)
                    .WithCoordinates (new System.Numerics.Vector2 (0, 0))
                    .WithRoomSize (1);

                var rng = new RNGProvider ();
                var chunk = new Chunk (builder.BuildChunk (rng), rng);

                Instantiate (chunk);
            }
        }
    }
}
