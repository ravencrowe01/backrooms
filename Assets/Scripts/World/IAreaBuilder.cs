namespace Backrooms.Assets.Scripts.World {
    public interface IAreaBuilder {
        IAreaBuilder WithChunkDiminsions (int width, int height);
        IAreaBuilder WithDiminsions (int width, int height);
        IAreaBuilder WithRoomSize (int size);
    }
}
