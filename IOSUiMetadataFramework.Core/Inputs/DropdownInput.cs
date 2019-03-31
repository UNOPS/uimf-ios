namespace IOSUiMetadataFramework.Core.Inputs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CoreGraphics;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using IOSUiMetadataFramework.Core.Model;
	using UiMetadataFramework.Basic.Input;
	using UIKit;

	[Input(Type = "dropdown")]
	public class DropdownInput : IInputManager
	{
		private UIPickerView PickerView { get; set; }
		private DropdownValue<string> SelectedValue { get; set; }
		private IList<DropdownItem> Items { get; set; }
	    private UITextField TextField { get; set; }

	    public UIView GetView(IDictionary<string, object> inputCustomProperties, MyFormHandler myFormHandler)
		{
			this.PickerView = new UIPickerView();
            this.Items = inputCustomProperties.GetCustomProperty<IList<DropdownItem>>("items");
            this.Items.Insert(0, new DropdownItem
		    {
		        Label = "",
		        Value = ""
		    });
			PickerModel model = new PickerModel(this.Items.Select(a => a.Label).ToArray());
			model.PickerChanged += (sender, e) =>
			{
				var selected = this.Items.SingleOrDefault(a => a.Label.Equals(e.SelectedValue));
				var value = new DropdownValue<string>(selected?.Value);
				this.SelectedValue = value;
			};
			this.PickerView.ShowSelectionIndicator = true;
			this.PickerView.Model = model;

			// Setup the toolbar
		    UIToolbar toolbar = new UIToolbar
		    {
		        BarStyle = UIBarStyle.Black,
		        Translucent = true
		    };
		    toolbar.SizeToFit();

			this.TextField = new UITextField();
			this.TextField.Layer.CornerRadius = 8;
			this.TextField.Layer.BorderColor = UIColor.LightGray.CGColor;
			this.TextField.Layer.BorderWidth = 1;
			UIView paddingView = new UIView(new CGRect(0, 0, 10, 20));
			this.TextField.LeftView = paddingView;
			this.TextField.LeftViewMode = UITextFieldViewMode.Always;

			// Create a 'done' button for the toolbar and add it to the toolbar
			UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done,
				(s, e) => {
					this.TextField.Text = this.SelectedValue.Value;
					this.TextField.ResignFirstResponder();
				});
			toolbar.SetItems(new [] { doneButton }, true);

			// Tell the textbox to use the picker for input
			this.TextField.InputView = this.PickerView;

			// Display the toolbar over the pickers
			this.TextField.InputAccessoryView = toolbar;

			return this.TextField;
		}

	    public object GetValue()
	    {
	        return this.SelectedValue;
	    }

	    public void SetValue(object value)
	    {
	        var dropdownValue = value.CastTObject<DropdownValue<object>>();
	        DropdownItem selectedItem = this.Items.FirstOrDefault(a => a.Value.Equals(dropdownValue.Value));
	        if (selectedItem != null)
	        {
	            var selectedPosition = this.Items.IndexOf(selectedItem);
	            this.PickerView.Select(selectedPosition, 0, false);
	        }

	    }
    }
	
}