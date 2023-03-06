using Backrooms.Assets.Scripts.World.Config;
using System.Linq;

namespace Backrooms.Assets.Scripts.World.Prototypes {
    public class ProtoSideState {
        public bool[] _states;

        public bool Open => _states.Any (s => s);

        public ProtoSideState (int size) {
            _states = new bool[size];
        }

        public ProtoSideState (bool[] states) {
            _states = states;
        }

        public ISideStateConfig ToSideStateConfig () => new SideStateConfig (_states);

        public void SetState (int i, bool state) => _states[i] = state;

        public void SetTotalState (bool state) {
            for (int i = 0; i < _states.Length; i++) {
                SetState (i, state);
            }
        }

        public bool GetState (int i) => _states[i];
    }
}
