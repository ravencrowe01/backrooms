namespace Backrooms.Assets.Scripts.World.Config {
    public interface IAreaConfig {
        IChunkConfig[,] Chunks { get; }

        void AddChunk (IChunkConfig chunk, int x, int y);

        IChunkConfig GetChunk (int x, int y);
    }
}