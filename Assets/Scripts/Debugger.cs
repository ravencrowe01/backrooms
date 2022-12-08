using Backrooms.Assets.Scripts.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Backrooms.Assets.Scripts {
    public class Debugger : MonoBehaviour {
        private bool _waiting = false;
        private float _waitCounter = 0;

        private void Update () {
            if(_waiting) {
                _waitCounter += Time.deltaTime;

                if(_waitCounter >= 5) {
                    _waiting = false;
                }

                if(Input.GetKeyDown(KeyCode.P)) {
                    TestPathfinding ();
                }
            }

            if(Input.GetKeyDown(KeyCode.Tilde)) {
                _waiting = true;
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
                    Direction.East
                }
            };

            nodes[2, 2] = new Node () {
                Cost = 1,
                Position = new Vector2 (2, 2),
                OpenEntrances = new List<Direction> {
                    Direction.West
                }
            };

            var start = nodes[2, 2];
            var end = nodes[0, 0];

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

                Debug.Log (str);
            }
            else {
                Debug.Log ($"Pathfinder returned status {path.Status}");
            }
        }
    }
}
