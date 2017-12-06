﻿namespace IOSUiMetadataFramework.Core.Outputs
{
	using System;
	using System.Collections.Generic;
	using CoreGraphics;
	using IOSUiMetadataFramework.Core;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UiMetadataFramework.Core;
	using UIKit;

	[Output(Type = "datetime")]
	public class DateOutput : IOutputManager
	{
		private UITextView OutputText { get; set; }

		public UIView GetView(OutputFieldMetadata outputField,
		    object value,
		    MyFormHandler myFormHandler,
		    FormMetadata formMetadata,
		    List<MyFormHandler.FormInputManager> inputsManager,
		    int yAxis)
		{
			this.OutputText = new UITextView();
			if (value != null)
			{
				DateTime datetime = value.CastTObject<DateTime>();
				this.OutputText.Text = outputField.Label + ": " + datetime.ToShortDateString();

				var labelSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 30);
				this.OutputText.Frame = new CGRect(new CGPoint(20, yAxis), labelSize);
			}
			return this.OutputText;
		}
	}
}