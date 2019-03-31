namespace IOSUiMetadataFramework.Core.Outputs
{
    using System.Collections.Generic;
    using CoreGraphics;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
    using UIKit;

    [Output(Type = "formlink")]
    public class FormLinkOutput : IOutputManager
    {
        private UITextView OutputView { get; set; }

        public UIView GetView(OutputFieldMetadata outputField,
            object value,
            MyFormHandler myFormHandler,
            FormMetadata formMetadata,
            List<FormInputManager> inputsManager,
            int yAxis)
        {
            var formLink = value.CastTObject<FormLink>();
            var size = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 30);
            this.OutputView = new UITextView
            {
                Text = outputField.Label + ": " + formLink.Label,
                Frame = new CGRect(new CGPoint(20, yAxis), size),
                TextColor = UIColor.Blue
            };

            var gesture = new UITapGestureRecognizer();
            gesture.AddTarget(() =>
            {
                var metadata = myFormHandler.GetFormMetadataAsync(formLink.Form);
                myFormHandler.FormWrapper.UpdateView(myFormHandler, new FormParameter(metadata, formLink.InputFieldValues));
            });
            this.OutputView.AddGestureRecognizer(gesture);
            return this.OutputView;
        }
    }
}