using System;
using UnityEngine;

namespace Raven.backrooms.UI.Main.Title {
    public class NewWorldButton : MonoBehaviour {
        public Action<NewWorldButton> OnButtonClicked;

        public void ButtonClicked () {
            OnButtonClicked.Invoke (this);
        }
    }
}
