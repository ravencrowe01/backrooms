using Random = UnityEngine.Random;

namespace Backrooms.Assets.Scripts.RNG {
    internal class RNGProvider : IRNG {
        // TODO Since I'm no longer using seperate libs, this is redundant and just causes problems.
        public int Next (int max) => Next (0, max);

        public int Next (int min, int max) => Random.Range (min, max);

        public void SetSeed (int seed) => Random.InitState (seed);
    }
}
