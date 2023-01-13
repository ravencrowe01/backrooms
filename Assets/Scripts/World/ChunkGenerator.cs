using Backrooms.Assets.Scripts;
using Backrooms.Assets.Scripts.Databases;
using Backrooms.Assets.Scripts.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Backrooms.Assets.Scripts.World {
    public class ChunkGenerator : MonoBehaviour {
        public Chunk ChunkBase;
        public GameObject ChunkRoot;
        public GameObject Player;
        public Chunk StartChunk;

        private Dictionary<Vector3, Chunk> _chunks = new Dictionary<Vector3, Chunk> ();

        private void Start () {
            BuildChunks ();
            //Instantiate (Player);
        }

        private void Update () {
            if(Input.GetKeyUp(KeyCode.R)) {
                Destroy(ChunkRoot.transform.GetChild (0).gameObject);
                _chunks.Clear ();
                BuildChunks ();
            }

        }

        private void BuildChunks () {
            var start = ChunkDatabase.GetChunk (1);
            _chunks.Add (new Vector3 (0, 0, 0), start);
            Instantiate (start, ChunkRoot.transform);

            for (int chunkX = -1; chunkX < 2; chunkX++) {
                for (int chunkZ = -1; chunkZ < 2; chunkZ++) {
                    if (chunkX == 0 && chunkZ == 0) {
                        
                        continue;
                    }

                    var chunk = Instantiate (ChunkBase, ChunkRoot.transform);

                    var position = new Vector3 (chunkX, 0, chunkZ);

                    for(int x = 1; x != 1 << 3; x = x << 1) {

                    }

                    // Pregened chunks need to have open sides initilized
                    if (_chunks.ContainsKey (position + Vector3.right)) {
                        chunk.AddConnections (_chunks[position + Vector3.right].GetConnections (Direction.North));
                    }

                    if (_chunks.ContainsKey (position + Vector3.left)) {
                        chunk.AddConnections (_chunks[position + Vector3.left].GetConnections (Direction.South));
                    }

                    if (_chunks.ContainsKey (position + Vector3.forward)) {
                        chunk.AddConnections (_chunks[position + Vector3.forward].GetConnections (Direction.East));
                    }

                    if (_chunks.ContainsKey (position + Vector3.back)) {
                        chunk.AddConnections (_chunks[position + Vector3.back].GetConnections (Direction.West));
                    }

                    _chunks.Add (position, chunk);

                    chunk.transform.position = new Vector3 (position.x * 48, 0, position.z * 48);
                    chunk.BuildChunk ();
                    chunk.InstantiateRooms ();
                }
            }
        }
    }
}
