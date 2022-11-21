using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.Pathfinding {
    public class AStar : IAStar {
        private bool[,] DiagonalMap {
            get {
                var map = new bool[3, 3];

                map[-1, -1] = true;
                map[1, 1] = true;
                map[1, -1] = true;
                map[-1, 1] = true;

                return map;
            }
        }

        public bool CheckDiagonals;
        public bool CheckPathways;

        private readonly PathNode _start;
        private readonly PathNode _end;
        private readonly Node[,] _map;

        private PathNode _current;

        private List<PathNode> _closedNodes;
        private List<PathNode> _openNodes;

        public PathfindingStatus Status { get; private set; } = PathfindingStatus.Waiting;

        public IEnumerable<Node> Path => _path;
        private List<Node> _path;

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
            if (_openNodes.Count () == 0) {
                return PathfindingStatus.Invalid;
            }

            _current = _openNodes[0];

            foreach (var node in _openNodes) {
                if (_current.f > node.f) {
                    _current = node;
                }
            }

            _openNodes.Remove (_current);
            _closedNodes.Add (_current);

            if (_current.MapNode == _end.MapNode) {
                _path = new List<Node> ();

                while (_current != null) {
                    _path.Add (_current.MapNode);

                    _current = _current.Parent;
                }

                Status = PathfindingStatus.Completed;
                return PathfindingStatus.Completed;
            }

            // TODO Check if there are no valid paths to the target.

            foreach (var child in GetAdjacentNodes (_current)) {
                if (IsClosed (child.MapNode) || IsOpen (child.MapNode) || child.MapNode.Blocking) {
                    continue;
                }

                child.g = _current.g + 1;
                child.h = Vector2.Distance (child.MapNode.Position, _end.Position);

                _openNodes.Add (child);
            }

            Status = PathfindingStatus.Finding;
            return PathfindingStatus.Finding;
        }

        private List<PathNode> GetAdjacentNodes (PathNode node) {
            var adjacent = new List<PathNode> ();

            for (int x = -1; x <= 1; x++) {
                if(x == -1 && node.Position.x == 0) {
                    continue;
                }

                if(x == 1 && node.Position.x == _map.GetLength(0) + 1) {
                    continue;
                }

                for(int y = -1; y <= 1; y++) {
                    if (y == -1 && node.Position.y == 0) {
                        continue;
                    }

                    if (y == 1 && node.Position.y == _map.GetLength (1) + 1) {
                        continue;
                    }

                    if (!CheckDiagonals && DiagonalMap[x, y]) {
                        continue;
                    }

                    var adjCords = node.Position + new Vector2 (x, y);

                    var mapNode = _map[(int) adjCords.x, (int) adjCords.y];

                    if(CheckPathways && mapNode.Pathway) {
                        adjCords += new Vector2 (x, y);
                    }

                    adjacent.Add (new PathNode {
                        Parent = node,
                        MapNode = mapNode,
                        g = node.g + 1,
                        h = Vector2.Distance(mapNode.Position, _end.Position)
                    });
                }
            }

            return adjacent;
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
