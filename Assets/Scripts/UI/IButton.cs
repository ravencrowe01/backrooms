using System;

namespace Raven.backrooms.UI {
    public interface IButton {
        Guid ID { get; }

        event Action<object, ButtonPressedEventArgs> Pressed;

        bool SetID (Guid ID);

        void ButtonPressed ();
    }
}
