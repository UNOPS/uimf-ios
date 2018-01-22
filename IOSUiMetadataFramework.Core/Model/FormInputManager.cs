using System;
using System.Collections.Generic;
using System.Text;

namespace IOSUiMetadataFramework.Core.Model
{
    using IOSUiMetadataFramework.Core.Managers;
    using UiMetadataFramework.Core;
    using UIKit;

    public class FormInputManager
    {
        public FormInputManager(InputFieldMetadata input, IInputManager manager, UIView view)
        {
            this.Input = input;
            this.Manager = manager;
            this.View = view;
        }

        public InputFieldMetadata Input { get; set; }
        public IInputManager Manager { get; set; }
        public UIView View { get; set; }
    }
}
