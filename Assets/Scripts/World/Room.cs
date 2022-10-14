using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Scripts.World {
    public class Room : MonoBehaviour {
        public int ID;

        public bool NorthOpen;
        public bool SouthOpen;
        public bool EastOpen;
        public bool WestOpen;

        public IEnumerable<CardinalDirection> OpenSides => (IEnumerable<CardinalDirection>) GetOpenSides ();

        private IEnumerable GetOpenSides () {
            if (NorthOpen) {
                yield return CardinalDirection.North;
            }

            if (SouthOpen) {
                yield return CardinalDirection.South;
            }

            if (EastOpen) {
                yield return CardinalDirection.East;
            }

            if (WestOpen) {
                yield return CardinalDirection.West;
            }
        }
    }
}