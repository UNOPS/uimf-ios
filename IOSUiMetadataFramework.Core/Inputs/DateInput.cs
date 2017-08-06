namespace IOSUiMetadataFramework.Core.Inputs
{
	using System;
	using Foundation;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UIKit;

	[Input(Type = "datetime")]
	public class DateInput : IInputManager
	{
		private UIDatePicker dateInput { get; set; }

		public UIView GetView(object inputCustomProperties)
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
			DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(
				new DateTime(2001, 1, 1, 0, 0, 0));
			var dateTime = (DateTime)value;
			//this.dateInput.Date = NSDate.FromTimeIntervalSinceReferenceDate((dateTime - newDate).TotalSeconds);
			this.dateInput.Date = dateTime.DateTimeToNSDate();
		}

	}
}