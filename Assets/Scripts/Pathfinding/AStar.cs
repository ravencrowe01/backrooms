using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.Pathfinding {
    public class AStar : IAStar {
        private readonly List<Vector2> _diagonals = new List<Vector2> {
            new Vector2(-1, -1),
            new Vector2(1, 1),
            new Vector2(1, -1),
            new Vector2(-1, 1)
        };

        private readonly Dictionary<Vector2, Direction> _directionMap = new Dictionary<Vector2, Direction> {
            // For whatever reason, I decided that this world is in a reality where North and South are flipped.
            // I'm far too deep in the sauce to back out now.
            { Vector2.down, Direction.North },
            { Vector2.up, Direction.South },
            { Vector2.right, Direction.East },
            { Vector2.left, Direction.West },
            { new Vector2(1, 1), Direction.NorthEast },
            { new Vector2(1, -1), Direction.SouthEast },
            { new Vector2(-1, -1), Direction.SouthWest },
            { new Vector2(-1, 1), Direction.NorthWest }
        };
            
        public bool CheckDiagonals;
        public bool CheckEntrances;

        private readonly PathNode _start;
        private readonly PathNode _end;
        private readonly Node[,] _map;

        public Vector2 Current => _current.MapNode.Position;
        private PathNode _current;

        private readonly List<PathNode> _closedNodes;
        private readonly List<PathNode> _openNodes;

        public PathfindingStatus Status { get; private set; } = PathfindingStatus.Waiting;

        public List<Node> Path { get; private set; }

        public AStar (Node start, Node end, Node[,] map) {
            _start = new PathNode { MapNode = start};
            _end = new PathNode { MapNode = end };
            _map = map;


            _closedNodes = new List<PathNode> ();

            _openNodes = new List<PathNode> {
                _start
            };
        }

        public PathfindingStatus Step () {
            // Ttere aren't any valid paths
            if (_openNodes.Count () == 0) {
                Status = PathfindingStatus.Invalid;
                return Status;
            }

            SetCurrentToLowestFNode ();

            // close current node
            _openNodes.Remove (_current);
            _closedNodes.Add (_current);

            if (_current.MapNode == _end.MapNode) {
                BuildPath ();

                Status = PathfindingStatus.Completed;
                return Status;
            }

            AddAdjacentNodesToOpenList ();

            Status = PathfindingStatus.Finding;
            return Status;
        }

        private void BuildPath () {
            var current = _current;
            Path = new List<Node> ();

            while (current != null) {
                Path.Add (current.MapNode);

                current = current.Parent;
            }
        }

        private void AddAdjacentNodesToOpenList () {
            foreach (var child in GetAdjacentNodes (_current)) {
                child.g = _current.g + 1;
                child.h = Vector2.Distance (child.MapNode.Position, _end.Position);

                _openNodes.Add (child);
            }
        }

        private void SetCurrentToLowestFNode () {
            _current = _openNodes[0];

            foreach (var node in _openNodes) {
                if (_current.f > node.f) {
                    _current = node;
                }
            }
        }

        private List<PathNode> GetAdjacentNodes (PathNode node) {
            var adjacent = new List<PathNode> ();

            for (int x = -1; x <= 1; x++) {
                var xRel = x + node.Position.x;

                if(xRel >= 0 && xRel < _map.GetLength(0)) {
                    for (int y = -1; y <= 1; y++) {
                        var yRel = y + node.Position.y;

                        if(yRel >= 0 && yRel < _map.GetLength (1)) {
                            if (!CheckDiagonals && _diagonals.Contains(new Vector2(x, y))) {
                                continue;
                            }

                            if (x != 0 || y != 0) {
                                var adjCords = node.Position + new Vector2 (x, y);

                                var adjNode = _map[(int) adjCords.x, (int) adjCords.y];

                                var direction = _directionMap[new Vector2 (x, y)];

                                if (IsDirectionTraversable (node, adjNode, direction)) {
                                    adjacent.Add (new PathNode {
                                        Parent = node,
                                        MapNode = adjNode,
                                        g = node.g + 1,
                                        h = Vector2.Distance (adjNode.Position, _end.Position)
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return adjacent;
        }

        private bool IsDirectionTraversable (PathNode node, Node mapNode, Direction direction) {
            if (IsClosed (mapNode)) return false;
            if (IsOpen (mapNode)) return false;
            if (mapNode.Blocking) return false;
            if (CheckEntrances && (!node.MapNode.OpenEntrances.Contains (direction))) return false;

            return true;
        }

        private bool IsClosed (Node node) => _closedNodes.Any (p => p.MapNode == node);

        private bool IsOpen (Node node) => _openNodes.Any (p => p.MapNode == node);

        private class PathNode {
            public PathNode Parent;
            public Node MapNode;

            public Vector2 Position => MapNode.Position;

            public float g;
            public float h;
            public float f => g + h;
        }
    }

    public enum PathfindingStatus {
        Waiting,
        Finding,
        Completed,
        Invalid
    }
}
