using System;
using UnityEngine;

namespace Raven.backrooms.UI.Main {
    public class TitleMenu : MonoBehaviour {
        [SerializeField]
        private Button _newWorldButton;
        [SerializeField]
        private Button _loadWorldButton;
        [SerializeField]
        private Button _settingsButton;
        [SerializeField]
        private Button _exitButton;

        public event Action<object, ButtonPressedEventArgs> NewWorldButtonPressedEvent;
        public event Action<object, ButtonPressedEventArgs> LoadWorldButtonPressedEvent;
        public event Action<object, ButtonPressedEventArgs> SettingsButtonPressedEvent;
        public event Action<object, ButtonPressedEventArgs> ExitButtonPressedEvent;

        private void Start () {
            _newWorldButton.Pressed += NewWorldButtonpressed;
            _loadWorldButton.Pressed += LoadWorldButtonPressed;
            _loadWorldButton.Pressed += OptionsButtonPressed;
            _exitButton.Pressed += ExitButtonPressed;
        }

        private void NewWorldButtonpressed (object invoker, ButtonPressedEventArgs args) {
            // TODO Reset the button here
            NewWorldButtonPressedEvent (invoker, args); 
        }

        private void LoadWorldButtonPressed (object invoker, ButtonPressedEventArgs args) {
            // TODO Reset the button here
            LoadWorldButtonPressedEvent (invoker, args);
        }

        private void OptionsButtonPressed (object invoker, ButtonPressedEventArgs args) {
            // TODO Reset the button here
            SettingsButtonPressedEvent (invoker, args);
        }

        private void ExitButtonPressed (object invoker, ButtonPressedEventArgs args) {
            ExitButtonPressedEvent (invoker, args);
        }
    }
}
