namespace IOSUiMetadataFramework.Core.Inputs
{
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using UIKit;

    [Input(Type = "boolean")]
    public class BooleanInput : IInputManager
    {
        private UISwitch ButtonSwitch { get; set; }

        public UIView GetView(object inputCustomProperties, MyFormHandler myFormHandler)
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