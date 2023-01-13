using Backrooms.Assets.Scripts.Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts {
    public class Debugger : MonoBehaviour {
        private void Update () {
            if (Input.GetKeyDown (KeyCode.P)) {
                TestPathfinding ();
            }
        }

        private void TestPathfinding () {
            var nodes = new Node[3, 3];

            nodes[0, 0] = new Node () {
                Cost = 1,
                Position = new Vector2 (0, 0),
                OpenEntrances = new List<Direction> {
                    Direction.East
                }
            };

            nodes[1, 0] = new Node () {
                Cost = 1,
                Position = new Vector2 (1, 0),
                OpenEntrances = new List<Direction> {
                    Direction.East,
                    Direction.West
                }
            };

            nodes[2, 0] = new Node () {
                Cost = 1,
                Position = new Vector2 (2, 0),
                OpenEntrances = new List<Direction> {
                    Direction.South,
                    Direction.West
                }
            };

            nodes[0, 1] = new Node () {
                Cost = 1,
                Position = new Vector2 (0, 1),
                OpenEntrances = new List<Direction> {
                    Direction.East,
                    Direction.South
                }
            };

            nodes[1, 1] = new Node () {
                Cost = 1,
                Position = new Vector2 (1, 1),
                OpenEntrances = new List<Direction> {
                    Direction.East,
                    Direction.West
                }
            };

            nodes[2, 1] = new Node () {
                Cost = 1,
                Position = new Vector2 (2, 1),
                OpenEntrances = new List<Direction> {
                    Direction.North,
                    Direction.West
                }
            };

            nodes[0, 2] = new Node () {
                Cost = 1,
                Position = new Vector2 (0, 2),
                OpenEntrances = new List<Direction> {
                    Direction.North,
                    Direction.East
                }
            };

            nodes[1, 2] = new Node () {
                Cost = 1,
                Position = new Vector2 (1, 2),
                OpenEntrances = new List<Direction> {
                    Direction.East,
                    Direction.West
                }
            };

            nodes[2, 2] = new Node () {
                Cost = 1,
                Position = new Vector2 (2, 2),
                OpenEntrances = new List<Direction> {
                    Direction.West
                }
            };

            var end = nodes[0, 0];

            for(int x = 0; x < 3; x++) {
                for(int y = 0; y < 3; y++) {
                    var start = nodes[x, y];

                    var path = new AStar (start, end, nodes);
                    path.CheckEntrances = true;

                    do {
                        path.Step ();
                    }
                    while (path.Status == PathfindingStatus.Finding);

                    if (path.Status == PathfindingStatus.Completed) {
                        var str = "";

                        foreach (var node in path.Path) {
                            str += $"[{node.Position.x}, {node.Position.y}]";

                            if (node != path.Path.Last ()) {
                                str += " -> ";
                            }
                        }

                        Debug.Log ($"[{x}, {y}] is valid: {str}");
                    }
                    else {
                        Debug.Log ($"[{x}, {y}] is invalid");
                    }
                }
            }
        }
    }
}
