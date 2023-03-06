namespace Backrooms.Assets.Scripts.World.Config {
    public class AreaConfig : IAreaConfig {
        public readonly int Size;

        public IChunkConfig[,] Chunks => (IChunkConfig[,]) _chunks.Clone ();
        private IChunkConfig[,] _chunks;

        public AreaConfig (int size) {
            Size = size;
            _chunks = new ChunkConfig[size, size];
        }

        public IChunkConfig GetChunk (int x, int y) => _chunks[x, y];

        public void AddChunk (IChunkConfig chunk, int x, int y) => _chunks[x, y] = chunk;
    }
}
