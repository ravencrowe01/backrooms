using Random = UnityEngine.Random;

namespace Backrooms.Assets.Scripts.RNG {
    internal class RNGProvider : IRNG {
        public int Next (int max) => Next (0, max);

        public int Next (int min, int max) => Random.Range (min, max);

        public void SetSeed (int seed) => Random.InitState (seed);
    }
}
