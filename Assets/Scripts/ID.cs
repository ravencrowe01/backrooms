namespace Backrooms.Assets.Scripts {
    public class ID {
        public int M { get; private set; }
        public int V { get; private set; }

        public ID (int m, int v = 0) {
            M = m;
            V = v;
        }
    }
}
