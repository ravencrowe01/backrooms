using Raven.backrooms.UI.Main;

namespace Raven.backrooms.UI {
    public abstract class MenuHandler {
        protected MainMenu _main;

        protected MenuHandler (MainMenu main) {
            _main = main;
        }

        public abstract void SetActiveState (bool state);
    }
}