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
            Instantiate (Player);
        }

        private void BuildChunks () {
            for (int chunkX = 0; chunkX < 1; chunkX++) {
                for (int chunkZ = 0; chunkZ < 1; chunkZ++) {
                    var chunk = Instantiate (ChunkBase, ChunkRoot.transform);

                    //if (chunkX == 0 && chunkZ == 0) {
                    //    chunk.AddRoom (0, 0, RoomDatabase.Instance.GetRoomByID (12));
                    //    chunk.AddRoom (1, 0, RoomDatabase.Instance.GetRoomByID (15));
                    //    chunk.AddRoom (2, 0, RoomDatabase.Instance.GetRoomByID (13));
                    //    chunk.AddRoom (0, 1, RoomDatabase.Instance.GetRoomByID (15));
                    //    chunk.AddRoom (1, 1, RoomDatabase.Instance.GetRoomByID (1));
                    //    chunk.AddRoom (2, 1, RoomDatabase.Instance.GetRoomByID (15));
                    //    chunk.AddRoom (0, 2, RoomDatabase.Instance.GetRoomByID (11));
                    //    chunk.AddRoom (1, 2, RoomDatabase.Instance.GetRoomByID (15));
                    //    chunk.AddRoom (2, 2, RoomDatabase.Instance.GetRoomByID (14));

                    //    chunk.transform.position = new Vector3 ();
                    //    chunk.InstantiateRooms ();

                    //    _chunks.Add (new Vector3 (), chunk);
                    //    continue;
                    //}

                    var position = new Vector3 (chunkX, 0, chunkZ);

                    _chunks.Add (position, chunk);

                    if (_chunks.ContainsKey (position + Vector3.forward)) {
                        chunk.AddConnections (_chunks[position + Vector3.forward].GetConnections (Direction.North));
                    }

                    if (_chunks.ContainsKey (position + Vector3.back)) {
                        chunk.AddConnections (_chunks[position + Vector3.back].GetConnections (Direction.South));
                    }

                    if (_chunks.ContainsKey (position + Vector3.right)) {
                        chunk.AddConnections (_chunks[position + Vector3.right].GetConnections (Direction.East));
                    }

                    if (_chunks.ContainsKey (position + Vector3.left)) {
                        chunk.AddConnections (_chunks[position + Vector3.left].GetConnections (Direction.West));
                    }

                    chunk.transform.position = new Vector3 (position.x * 48, 0, position.z * 48);
                    chunk.BuildChunk ();
                    chunk.InstantiateRooms ();
                }
            }
        }
    }
}
