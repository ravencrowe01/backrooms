using Backrooms.Assets.Scripts.World.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Backrooms.Assets.Scripts.World.Prototypes {
    public class ProtoArea {
        public readonly int Size;

        private ProtoChunk[,] _chunks;

        public ProtoArea (int size) {
            Size = size;
            _chunks = new ProtoChunk[size, size];
        }

        public ProtoChunk GetChunk (int x, int y) => _chunks[x, y];

        public void AddChunk (ProtoChunk chunk, int x, int y) => _chunks[x, y] = chunk;

        public void SetRoomSideState (Vector2 chunk, Vector2 room, Direction dir, bool state, int index) => _chunks[(int) chunk.x, (int) chunk.y].SetRoomSideState (room, dir, index, state);

        public void SetRoomSideTotalState (Vector2 chunk, Vector2 room, Direction dir, bool state) => _chunks[(int) chunk.x, (int) chunk.y].SetRoomSideTotalState (room, dir, state);

        public IAreaConfig ToAreaConfig () {
            var config = new AreaConfig (Size);

            for(int x = 0; x < Size; x++) {
                for(int y = 0; y < Size; y++) {
                    config.AddChunk (_chunks[x, y].ToChunkConfig(new Vector2(x, y)), x, y);
                }
            }

            return config;
        }
    }
}
