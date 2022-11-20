﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Backrooms.Assets.Scripts.Pathfinding {
    public class AStar : IAStar {
        public bool CheckDiagonals;

        private readonly PathNode _start;
        private readonly PathNode _end;
        private readonly Node[,] _map;

        private PathNode _current;

        private List<PathNode> _closedNodes;
        private List<PathNode> _openNodes;

        public AStar (Node start, Node end, Node[,] map) {
            _start = new PathNode (start);
            _end = new PathNode (end);
            _map = map;


            _closedNodes = new List<PathNode> ();

            _openNodes = new List<PathNode> {
                _start
            };
        }

        public List<Node> Step () {
            _current = _openNodes[0];

            foreach (var node in _openNodes) {
                if (_current.f > node.f) {
                    _current = node;
                }
            }

            _openNodes.Remove (_current);
            _closedNodes.Add (_current);

            if (_current.Node == _end.Node) {
                var path = new List<Node> ();

                while (_current != null) {
                    path.Add (_current.Node);

                    _current = _current.Parent;
                }

                return path;
            }

            // TODO Check if there are no valid paths to the target.

            foreach (var child in GetAdjacentNodes (_current)) {
                if (IsClosed (child.Node) || IsOpen (child.Node)) {
                    continue;
                }

                child.g = _current.g + 1;
                child.h = Vector2.Distance (child.Node.Position, _end.Position);

                _openNodes.Add (child);
            }

            return null;
        }

        private List<PathNode> GetAdjacentNodes (PathNode node) {
            var adjacent = new List<PathNode> {
                new PathNode (_map[(int) (node.Position.x + 1), (int) node.Position.y]),
                new PathNode (_map[(int) (node.Position.x - 1), (int) node.Position.y]),
                new PathNode (_map[(int) (node.Position.x), (int) node.Position.y + 1]),
                new PathNode (_map[(int) (node.Position.x), (int) node.Position.y - 1])
            };

            if (CheckDiagonals) {
                adjacent.Add (new PathNode (_map[(int) (node.Position.x + 1), (int) node.Position.y + 1]));
                adjacent.Add (new PathNode (_map[(int) (node.Position.x - 1), (int) node.Position.y + 1]));
                adjacent.Add (new PathNode (_map[(int) (node.Position.x + 1), (int) node.Position.y - 1]));
                adjacent.Add (new PathNode (_map[(int) (node.Position.x - 1), (int) node.Position.y - 1]));
            }

            return adjacent;
        }

        private bool IsClosed (Node node) => _closedNodes.Any (p => p.Node == node);

        private bool IsOpen (Node node) => _openNodes.Any (p => p.Node == node);

        private class PathNode {
            public Node Node;

            public PathNode Parent { get; set; } = null;
            public Vector2 Position => Node.Position;

            public float g;
            public float h;
            public float f => g + h;

            public PathNode (Node node) {
                Node = node;
            }
        }
    }
}
