namespace Backrooms.Assets.Scripts.World.Config {
    public interface ISideStateConfig {
        bool[] SideStates { get; }
        bool Open { get; }

        bool Compare (ISideStateConfig other);
        void SetState (int i, bool state);
    }
}