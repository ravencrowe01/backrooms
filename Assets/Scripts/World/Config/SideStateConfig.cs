using System.Linq;

namespace Backrooms.Assets.Scripts.World.Config {
    public class SideStateConfig : ISideStateConfig {
        public bool[] SideStates => (bool[]) _states.Clone ();
        public bool[] _states;

        public bool Open => _states.Any (s => s);

        public SideStateConfig (int size) {
            _states = new bool[size];
        }

        public SideStateConfig (bool[] states) {
            _states = states;
        }

        public void SetState (int i, bool state) => _states[i] = state;

        public bool Compare (ISideStateConfig other) {
            if (SideStates.Length == other.SideStates.Length) {
                for (int i = 0; i < SideStates.Length; i++) {
                    if (SideStates[i] != other.SideStates[i]) {
                        return false;
                    }
                }
            }
            else {
                return false;
            }

            return true;
        }
    }
}
