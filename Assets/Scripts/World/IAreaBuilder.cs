namespace Backrooms.Assets.Scripts.World {
    public interface IAreaBuilder {
        IAreaBuilder WithDiminsions (int width, int height);
        IAreaBuilder WithRoomSize (int size);
    }
}
