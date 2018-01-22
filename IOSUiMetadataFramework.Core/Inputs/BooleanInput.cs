namespace IOSUiMetadataFramework.Core.Inputs
{
    using System.Collections.Generic;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using UIKit;

    [Input(Type = "boolean")]
    public class BooleanInput : IInputManager
    {
        private UISwitch ButtonSwitch { get; set; }

        public UIView GetView(IDictionary<string, object> inputCustomProperties, MyFormHandler myFormHandler)
        {
            this.ButtonSwitch = new UISwitch();
            return this.ButtonSwitch;
        }

        public object GetValue()
        {
            return this.ButtonSwitch.On;
        }

        public void SetValue(object value)
        {
            this.ButtonSwitch.On = value.CastTObject<bool>();
        }
    }
}