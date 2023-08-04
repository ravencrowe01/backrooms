using System;
using UnityEngine;

namespace Raven.backrooms.UI.Main.Title {
    public class LoadWorldButton : MonoBehaviour {
        public Action<LoadWorldButton> OnButtonClicked;

        public void ButtonClicked () {
            OnButtonClicked.Invoke (this);
        }
    }
}