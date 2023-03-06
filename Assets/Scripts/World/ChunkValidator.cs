using Backrooms.Assets.Scripts.Pathfinding;
using Backrooms.Assets.Scripts.World.Config;
using Backrooms.Assets.Scripts.World.Prototypes;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class ChunkValidator {
        public static bool ValidateChunk (ProtoChunk chunk) {
            Node[,] nodeMap = BuildNodeMap (chunk);

            for (int x = 0; x < chunk.Size; x++) {
                for (int y = 0; y < chunk.Size; y++) {
                    if (!(x == 1 && y == 1)) {
                        var pathfinder = new AStar (nodeMap[x, y], nodeMap[1, 1], nodeMap);
                        var rPathfinder = new AStar (nodeMap[1, 1], nodeMap[x, y], nodeMap);

                        PathfindingStatus pathfinding;
                        PathfindingStatus rPathfinding;

                        do {
                            pathfinding = pathfinder.Step ();
                            rPathfinding = rPathfinder.Step ();
                        }
                        while (pathfinding == PathfindingStatus.Finding && rPathfinding == PathfindingStatus.Finding);

                        if (pathfinding == PathfindingStatus.Invalid || rPathfinding == PathfindingStatus.Invalid) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static Node[,] BuildNodeMap (ProtoChunk chunk) {
            var nodeMap = new Node[chunk.Size, chunk.Size];

            for (int x = 0; x < chunk.Size; x++) {
                for (int y = 0; y < chunk.Size; y++) {
                    var cords = new Vector2 (x, y);
                    nodeMap[x, y] = new Node {
                        Cost = 1,
                        Position = cords,
                        Config = chunk.GetRoom(new Vector2(x, y)).ToRoomConfig(),
                        Blocking = false
                    };
                }
            }

            return nodeMap;
        }
    }
}
