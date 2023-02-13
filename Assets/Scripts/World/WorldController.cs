using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class WorldController : MonoBehaviour {
        public Chunk ChunkBase;

        public ChunkRoot ChunkRoot;
        private ChunkRoot _chunkRootInstance;

        public int StartingAreaWidth = 3;
        public int StartingAreaHeight = 3;

        public int ChunkWidth = 3;
        public int ChunkHeight = 3;

        public int RoomSize = 1;

        private void Awake () {
            _chunkRootInstance = Instantiate (ChunkRoot, transform);
        }
    }
}
