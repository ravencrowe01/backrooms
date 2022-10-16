using Backrooms.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class Room : MonoBehaviour {
        public int ID;

        public bool NorthOpen;
        public bool SouthOpen;
        public bool EastOpen;
        public bool WestOpen;

        public IEnumerable<Direction> OpenSides => (IEnumerable<Direction>) GetOpenSides ();

        private IEnumerable GetOpenSides () {
            if (NorthOpen) {
                yield return Direction.North;
            }

            if (SouthOpen) {
                yield return Direction.South;
            }

            if (EastOpen) {
                yield return Direction.East;
            }

            if (WestOpen) {
                yield return Direction.West;
            }
        }

        public bool IsSideOpen (Direction side) => side switch {
            Direction.North => NorthOpen,
            Direction.South => SouthOpen,
            Direction.East => EastOpen,
            Direction.West => WestOpen,
            _ => throw new ArgumentException ($"Cardinal direction {side} is outside of the scope of Room.IsSideOpen, ensure only North, South, East, or West is being passed in."),
        };
    }
}