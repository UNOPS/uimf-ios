namespace IOSUiMetadataFramework.Core.Inputs
{
    using System;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using UIKit;

    [Input(Type = "datetime")]
    public class DateInput : IInputManager
    {
        private UIDatePicker dateInput { get; set; }

        public UIView GetView(object inputCustomProperties, MyFormHandler myFormHandler)
        {
            this.dateInput = new UIDatePicker();
            return this.dateInput;
        }

        public object GetValue()
        {
            return this.dateInput.Date.NSDateToDateTime();
        }

        public void SetValue(object value)
        {
            var dateTime = value.CastTObject<DateTime>();
            this.dateInput.Date = dateTime.DateTimeToNSDate();
        }
    }
}