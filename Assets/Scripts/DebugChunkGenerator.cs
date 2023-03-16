using Backrooms.Assets.Scripts.World;
using UnityEngine;

namespace Backrooms.Assets.Scripts {
    public class DebugChunkGenerator : MonoBehaviour {
        public Chunk ChunkBase;
        public ChunkRoot ChunkRoot;
        private ChunkRoot _root;

        private Chunk _chunk;

        private void Awake () {
            _root = Instantiate (ChunkRoot, transform);
        }

        private void Update () {
            if (Input.GetKeyDown (KeyCode.R) && (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))) {
                if (_chunk != null) {
                    Destroy (_chunk.gameObject);
                    _chunk = null;
                }

                BuildChunk (0, 0, 4206969);
            }
        }

        private void BuildChunk (int x, int z, int seed) {
            var builder = new ChunkBuilder ();

            builder.WithDiminsions (3, 3)
                .WithCoordinates (new Vector2 (0, 0))
                .WithRoomSize (1);

            _chunk = Instantiate (ChunkBase, _root.transform);
            _chunk.Init (builder.BuildChunk (seed), seed);
            _chunk.transform.position = new Vector3 (x, 0, z);
            _chunk.InstantiateRooms ();
        }
    }
}
