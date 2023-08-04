using System;
using UnityEngine;

namespace Raven.backrooms.UI.Main.Title {
    public class ExitButton : MonoBehaviour {
        public Action<ExitButton> OnButtonClicked;

        public void ButtonClicked () {
            OnButtonClicked.Invoke (this);
        }
    }
}