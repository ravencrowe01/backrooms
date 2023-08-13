using System;
using UnityEngine;

namespace Raven.backrooms.UI {
    public class Button : MonoBehaviour, IButton {
        public Guid ID { get; protected set; }
        private bool _idLocked = false;

        public event Action<object, ButtonPressedEventArgs> Pressed;

        public bool SetID (Guid id) {
            if (!_idLocked) {
                ID = id;
                _idLocked = true;
                return true;
            }

            return false;
        }

        public void ButtonPressed () {
            Pressed (this, new ButtonPressedEventArgs {
                ID =ID
            });
        }
    }
}
