using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour {
    public bool NorthOpen;
    public bool SouthOpen;
    public bool EastOpen;
    public bool WestOpen;

    public IEnumerable<CardinalDirection> OpenSides => GetOpenSides ();

    public IList<CardinalDirection> GetOpenSides () {
        var open = new List<CardinalDirection> ();

        if(NorthOpen) {
            open.Add (CardinalDirection.North);
        }

        if(SouthOpen) {
            open.Add (CardinalDirection.South);
        }

        if(EastOpen) {
            open.Add (CardinalDirection.East);
        }

        if(WestOpen) {
            open.Add (CardinalDirection.West);
        }

        return open;
    }

    public CardinalDirection GetRandomOpenSide () {
        var open = GetOpenSides();

        return open[Random.Range (0, open.Count ())];
    }
}
