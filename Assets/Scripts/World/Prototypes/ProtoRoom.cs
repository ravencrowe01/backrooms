using Backrooms.Assets.Scripts.World.Config;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Prototypes {
    public class ProtoRoom {
        private Vector2 _cords;
        private Dictionary<Direction, ProtoSideState> _states;

        public ProtoRoom (Vector2 cords, int size) {
            _cords = cords;

            _states = new Dictionary<Direction, ProtoSideState> {
                    {Direction.North, new ProtoSideState(size) },
                    {Direction.South, new ProtoSideState(size) },
                    {Direction.East, new ProtoSideState(size) },
                    {Direction.West, new ProtoSideState(size) }
                };
        }

        public Dictionary<Direction, ProtoSideState> GetOpenSides () {
            var open = new Dictionary<Direction, ProtoSideState> ();

            foreach (var dir in (Direction[]) Enum.GetValues (typeof (Direction))) {
                if (_states[dir].Open) {
                    open.Add (dir, _states[dir]);
                }
            }

            return open;
        }

        public void SetSideState (Direction dir, int i, bool state) => _states[dir].SetState (i, state);

        public void SetSideTotalState (Direction dir, bool state) {
            for (int i = 0; i < _states[dir]._states.Length; i++) {
                SetSideState (dir, i, state);
            }
        }

        public bool GetSideState (Direction dir, int i) => _states[dir].GetState (i);

        public ProtoSideState GetSideState (Direction dir) => _states[dir];

        public IRoomConfig ToRoomConfig () {
            var states = new Dictionary<Direction, ISideStateConfig> ();

            foreach (var state in _states.Keys) {
                states.Add (state, _states[state].ToSideStateConfig ());
            }

            return new RoomConfig (_cords, states);
        }
    }
}
