namespace IOSUiMetadataFramework.Core.Outputs
{
    using System.Collections.Generic;
    using CoreGraphics;
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
    using UIKit;

    [Output(Type = "action-list")]
    public class ActionListOutput : IOutputManager
    {
        private UIView OutputView { get; set; }

        public UIView GetView(OutputFieldMetadata outputField,
            object value,
            MyFormHandler myFormHandler,
            FormMetadata formMetadata,
            List<FormInputManager> inputsManager,
            int yAxis)
        {
            this.OutputView = new UIView();
            var actions = value?.CastTObject<ActionList>();

            if (actions != null)
            {
                int y = 0;
                foreach (var btn in actions.Actions)
                {
                    var button = this.InitializeActionButton(btn, myFormHandler, y);
                    y += 40;
                    this.OutputView.AddSubview(button);
                }
                var size = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, actions.Actions.Count * 40);
                this.OutputView.Frame = new CGRect(new CGPoint(20, yAxis), size);
            }
           
            return this.OutputView;
        }

        public UIButton InitializeActionButton(FormLink formLink, MyFormHandler myFormHandler,int yAxis)
        {
            var button = new UIButton();
            button.SetTitle(formLink.Label, UIControlState.Normal);
            button.BackgroundColor = UIColor.LightGray;
            var btnSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 60, 40);
            button.Frame = new CGRect(new CGPoint(0, yAxis), btnSize);

            button.TouchUpInside += (sender, args) =>
            {
                var formMetadata = myFormHandler.GetFormMetadataAsync(formLink.Form);
                var action = formLink.Action ?? FormLinkActions.OpenModal;
                myFormHandler.FormWrapper.UpdateView(myFormHandler, new FormParameter(formMetadata, formLink.InputFieldValues), action);
            };
            return button;
        }
    }
}