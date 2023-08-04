using System;
using UnityEngine;

namespace Raven.backrooms.UI.Main.Title {
    public class OptionsButton : MonoBehaviour {
        public Action<OptionsButton> OnButtonClicked;

        public void ButtonClicked () {
            OnButtonClicked.Invoke (this);
        }
    }
}