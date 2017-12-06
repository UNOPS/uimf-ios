﻿namespace IOSApp
{
    using System.Collections.Generic;
    using CoreGraphics;
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using UiMetadataFramework.Core;
    using UIKit;

    [Output(Type = "instalment-list")]
    public class InstallmentList : IOutputManager
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
                this.OutputText.Text = outputField.Label + ": " + value;
                var labelSize = new CGSize(500, 30);
                this.OutputText.Frame = new CGRect(new CGPoint(20, yAxis), labelSize);
            }
            return this.OutputText;
        }
    }
}