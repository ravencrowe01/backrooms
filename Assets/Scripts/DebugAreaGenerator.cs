using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Backrooms.Assets.Scripts {
    public class DebugAreaGenerator : MonoBehaviour {
        public Chunk ChunkBase;
        public ChunkRoot ChunkRoot;

        private void Update () {
            if((Input.GetKey(KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) 
                && Input.GetKeyDown (KeyCode.A)) {
                var builder = new AreaBuilder ()
                    .WithDiminsions (10, 10)
                    .WithChunkDiminsions (3, 3)
                    .WithRoomSize (1);

                var rng = new RNGProvider ();
                rng.SetSeed (4206969);

                var area = builder.BuildArea (rng);

                var root = Instantiate (ChunkRoot, transform);

                for(int x = 0; x < area.Chunks.GetLength(0); x++) {
                    for(int z = 0; z < area.Chunks.GetLength(1); z++) {
                        var chunk = area.GetChunk (x, z);
                        chunk.Coordinates *= 24;
                        root.AddChunk (chunk, ChunkBase, rng);
                    }
                }
            }
        }
    }
}
