﻿namespace IOSUiMetadataFramework.Core.Inputs
{
    using System.Collections.Generic;
    using CoreGraphics;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.Basic.Input;
    using UIKit;

    [Input(Type = "password")]
	public class PasswordInput : IInputManager
	{
	    private UITextField InputText { get; set; }

	    public UIView GetView(IDictionary<string, object> inputCustomProperties, MyFormHandler myFormHandler)
	    {
	        this.InputText = new UITextField { SecureTextEntry = true };
	        this.InputText.SetTextBorders();
            UIView paddingView = new UIView(new CGRect(0, 0, 10, 20));
	        this.InputText.LeftView = paddingView;
	        this.InputText.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            this.InputText.LeftViewMode = UITextFieldViewMode.Always;
	        return this.InputText;
	    }

	    public object GetValue()
	    {
            return new Password
            {
                Value = this.InputText.Text
            };
        }

	    public void SetValue(object value)
	    {
	        this.InputText.Text = value?.ToString();
	    }

    }
}