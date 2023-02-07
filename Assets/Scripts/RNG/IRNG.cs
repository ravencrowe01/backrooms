namespace Backrooms.Assets.Scripts.RNG {
    public interface IRNG {
        int Next (int max);

        int Next (int min, int max);

        void SetSeed (int seed);
    }
}
