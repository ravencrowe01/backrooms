using Backrooms.Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.Pathfinding {
    public class AStar : IAStar {
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
            _start = new PathNode { MapNode = start };
            _end = new PathNode { MapNode = end };
            _map = map;


            _closedNodes = new List<PathNode> ();

            _openNodes = new List<PathNode> {
                _start
            };
        }

        public PathfindingStatus Step () {
            // There aren't any valid paths
            if (_openNodes.Count == 0) {
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
            var adjVec = Utility.GetAdjacentVectors (_current.Position, _map.GetLength (0), _map.GetLength (1));
            var adjacent = new List<PathNode> ();

            foreach (var adj in adjVec) {
                var n = _map[(int) adj.x, (int) adj.y];

                if (IsDirectionTraversable (n, Utility.GetDirectionFromVector (node.Position - adj))) {
                    adjacent.Add (new PathNode {
                        Parent = node,
                        MapNode = n,
                        g = node.g + 1,
                        h = Vector2.Distance (n.Position, _end.Position)
                    });
                }
            }

            return adjacent;
        }

        private bool IsDirectionTraversable (Node mapNode, Direction direction) {
            if (IsClosed (mapNode)) return false;
            if (IsOpen (mapNode)) return false;
            if (mapNode.Blocking) return false;
            if (!mapNode.Config.SideStates[direction].SideStates.Any (s => s)) return false;

            return true;
        }

        private bool IsClosed (Node node) => _closedNodes.Any (p => p.MapNode == node);

        private bool IsOpen (Node node) => _openNodes.Any (p => p.MapNode == node);

        private class PathNode {
#pragma warning disable CS8618
            public PathNode Parent;
            public Node MapNode;
#pragma warning restore CS8618

            public Vector2 Position => MapNode.Position;

            public float g;
            public float h;
            public float f => g + h;
        }
    }
}
