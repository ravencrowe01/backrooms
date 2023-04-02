using Backrooms.Assets.Scripts.World;
using UnityEngine;

namespace Backrooms.Assets.Scripts {
    public class DebugAreaGenerator : MonoBehaviour {
        public Chunk ChunkBase;
        public ChunkRoot ChunkRoot;

        int seed = 4206969;

        private void Update () {
            if ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
                && Input.GetKeyDown (KeyCode.A)) {
                var builder = new AreaBuilder ()
                    .WithSize (3)
                    .WithChunkSize (3)
                    .WithRoomSize (1);

                var area = builder.BuildArea (seed);

                var root = Instantiate (ChunkRoot, transform);

                for (int x = 0; x < area.Chunks.GetLength (0); x++) {
                    for (int z = 0; z < area.Chunks.GetLength (1); z++) {
                        var chunk = area.GetChunk (x, z);
                        root.AddChunk (chunk, ChunkBase, seed);
                    }
                }
            }
        }
    }
}
