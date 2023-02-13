using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World;
using UnityEngine;

namespace Backrooms.Assets.Scripts {
    public class DebugChunkGenerator : MonoBehaviour {
        public Chunk ChunkBase;
        public ChunkRoot ChunkRoot;
        public int Test;

        private Chunk _chunk;

        private void Update () {
            if (Input.GetKeyDown (KeyCode.R) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))) {
                if(_chunk != null) {
                    Destroy (_chunk.gameObject);
                    _chunk = null;
                }

                BuildChunk (0, 0, 4206969);
            }
        }

        private void BuildChunk(int x, int z, int seed) {
            var builder = new ChunkBuilder ();

            builder.WithDiminsions (11, 11)
                .WithCoordinates (new Vector2 (0, 0))
                .WithRoomSize (1);

            var rng = new RNGProvider ();
            rng.SetSeed ((seed / x) * z);

            _chunk = Instantiate (ChunkBase, ChunkRoot.transform);
            _chunk.Init (builder.BuildChunk (rng), rng);
            _chunk.transform.position = new Vector3 (x, 0, z);
            _chunk.InstantiateRooms ();
        }
    }
}
