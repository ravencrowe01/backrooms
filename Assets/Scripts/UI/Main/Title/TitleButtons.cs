using System;
using UnityEngine;

namespace Raven.backrooms.UI.Main.Title {
    public class TitleButtons : MonoBehaviour {
        public Action<object, int> ButtonClicked;

        public NewWorldButton NewWorldButton;
        public LoadWorldButton LoadWorldButton;
        public OptionsButton OptionsButton;
        public ExitButton ExitButton;

        void Start () {
            NewWorldButton.OnButtonClicked += OnNewWorldButtonClicked;
            LoadWorldButton.OnButtonClicked += OnLoadWorldButtonClicked;
            OptionsButton.OnButtonClicked += OnOptionsButtonClicked;
            ExitButton.OnButtonClicked += OnExitButtonClicked;
        }

        private void OnNewWorldButtonClicked (NewWorldButton invoker) {
            ButtonClicked?.Invoke (invoker, 1);
        }

        private void OnLoadWorldButtonClicked (LoadWorldButton invoker) {
            ButtonClicked?.Invoke (invoker, 2);

        }

        private void OnOptionsButtonClicked (OptionsButton invoker) {
            ButtonClicked?.Invoke (invoker, 3);

        }

        private void OnExitButtonClicked (ExitButton invoker) {
            ButtonClicked?.Invoke (invoker, 4);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit (0);
        }
    }
}
