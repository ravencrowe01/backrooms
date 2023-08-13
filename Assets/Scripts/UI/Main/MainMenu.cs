using Raven.backrooms.Managers;
using System;
using UnityEngine;

namespace Raven.backrooms.UI.Main {
    public class MainMenu : MonoBehaviour {
        public Action<object, ButtonPressedEventArgs> ButtonClicked;

        [SerializeField]
        private TitleMenu _titleMenu;
        [SerializeField]
        private NewWorldMenu _newWorldMenu;
        [SerializeField]
        private LoadWorldMenu _loadWorldMenu;
        [SerializeField]
        private SettingsMenu _settingsMenu;

        // I may or may not need to keep these refs, but it makes sense to
        private TitleMenuHandler _titleHandler;
        private NewWorldMenuHandler _newWorldHandler;
        private LoadWorldMenuHandler _loadWorldHandler;
        private SettingsMenuHandler _settingsHandler;

        private MenuHandler _currentMenu;

        private void Start () {
            _titleHandler = new TitleMenuHandler (this, _titleMenu);
            //_newWorldHandler = new NewWorldMenuHandler (this, _newWorldMenu);
            //_loadWorldHandler = new LoadWorldMenuHandler (this, _loadWorldMenu);
            //_settingsHandler = new SettingsMenuHandler (this, _settingsMenu);
        }

        protected void ChangeMenuTo(MainMenuType to) {
            _currentMenu.SetActiveState (false);

            _currentMenu = to switch {
                MainMenuType.Title => _titleHandler,
                MainMenuType.NewWorld => _newWorldHandler,
                MainMenuType.LoadWorld => _loadWorldHandler,
                MainMenuType.Settings => _settingsHandler,
                _ => throw new Exception ($"Unknown MenuType {to}")
            };
        }

        protected void RequestShutdown () {
            GameManager.Instance.RequestShutdown (this);
        }

        #region Handlers

        private class TitleMenuHandler : MenuHandler {
            private TitleMenu _title;

            public TitleMenuHandler (MainMenu main, TitleMenu title) : base(main) {
                _title = title;

                _title.NewWorldButtonPressedEvent += NewWorldButtonPressed;
                _title.LoadWorldButtonPressedEvent += LoadWorldButtonPressed;
                _title.SettingsButtonPressedEvent += SettingsButtonPressed;
                _title.ExitButtonPressedEvent += ExitButtonPressed;
            }

            public override void SetActiveState (bool state) => _title.gameObject.SetActive (state);

            private void NewWorldButtonPressed (object invoker, ButtonPressedEventArgs args) {

            }

            private void LoadWorldButtonPressed (object invoker, ButtonPressedEventArgs args) {

            }

            private void SettingsButtonPressed (object invoker, ButtonPressedEventArgs args) {

            }

            private void ExitButtonPressed (object invoker, ButtonPressedEventArgs args) {
                _main.RequestShutdown ();
            }
        }

        private class NewWorldMenuHandler : MenuHandler {
            private NewWorldMenu _newWorld;

            public NewWorldMenuHandler (MainMenu main, NewWorldMenu nwm) : base(main) {
                _newWorld = nwm;
            }

            public override void SetActiveState (bool state) => _newWorld.gameObject.SetActive (state);
        }

        private class LoadWorldMenuHandler : MenuHandler {
            private LoadWorldMenu _loadWorld;

            public LoadWorldMenuHandler (MainMenu main, LoadWorldMenu lwm) : base (main) {
                _loadWorld = lwm;
            }

            public override void SetActiveState (bool state) => _loadWorld.gameObject.SetActive (state);
        }

        private class SettingsMenuHandler : MenuHandler {
            private SettingsMenu _settings;

            public SettingsMenuHandler (MainMenu main, SettingsMenu settings) : base (main) {
                _settings = settings;
            }

            public override void SetActiveState (bool state) => _settings.gameObject.SetActive (state);
        }
        #endregion
    }
}

namespace Raven.backrooms.UI {
}