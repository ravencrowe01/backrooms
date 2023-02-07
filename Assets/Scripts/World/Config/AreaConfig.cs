namespace Backrooms.Assets.Scripts.World.Config {
    public class AreaConfig : IAreaConfig {
        public IChunkConfig[,] Chunks => (IChunkConfig[,]) _chunks.Clone ();
        private IChunkConfig[,] _chunks;

        public AreaConfig (int width, int height) {
            _chunks = new ChunkConfig[width, height];
        }

        public IChunkConfig GetChunk (int x, int y) => _chunks[x, y];

        public void AddChunk (IChunkConfig chunk, int x, int y) => _chunks[x, y] = chunk;
    }
}
