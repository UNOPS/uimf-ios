namespace IOSUiMetadataFramework.Core.Inputs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CoreGraphics;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UiMetadataFramework.Basic.Input;
	using UIKit;

	[Input(Type = "dropdown")]
	public class DropdownInput : IInputManager
	{
		private UIPickerView PickerView { get; set; }
		private DropdownValue<string> SelectedValue { get; set; }
		private IList<DropdownItem> Items { get; set; }
		private UITextField TextField { get; set; }

		public object GetValue()
		{
			return this.SelectedValue;
		}

		public void SetValue(object value)
		{
			var selectedItem = this.Items.FirstOrDefault(a => a.Value.Equals(value));
			if (selectedItem != null)
			{
				var selectedPosition = this.Items.IndexOf(selectedItem);
				this.PickerView.Select(selectedPosition, 0, false);
			}
			
		}

		public UIView GetView(object inputCustomProperties)
		{
			this.PickerView = new UIPickerView();
			var list = (DropdownProperties)inputCustomProperties;
			this.Items = list.Items;
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
			UIToolbar toolbar = new UIToolbar();
			toolbar.BarStyle = UIBarStyle.Black;
			toolbar.Translucent = true;
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
			toolbar.SetItems(new UIBarButtonItem[] { doneButton }, true);

			// Tell the textbox to use the picker for input
			this.TextField.InputView = this.PickerView;

			// Display the toolbar over the pickers
			this.TextField.InputAccessoryView = toolbar;

			return this.TextField;
		}
	}

		public class PickerModel : UIPickerViewModel
		{
			private readonly IList<string> values;

			public event EventHandler<PickerChangedEventArgs> PickerChanged;

			public PickerModel(IList<string> values)
			{
				this.values = values;
			}

			public override nint GetComponentCount(UIPickerView picker)
			{
				return 1;
			}

			public override nint GetRowsInComponent(UIPickerView picker, nint component)
			{
				return this.values.Count;
			}

			public override string GetTitle(UIPickerView picker, nint row, nint component)
			{
				return this.values[Convert.ToInt32(row)];
			}

			public override nfloat GetRowHeight(UIPickerView picker, nint component)
			{
				return 40f;
			}

			public override void Selected(UIPickerView picker, nint row, nint component)
			{
				this.PickerChanged?.Invoke(this, new PickerChangedEventArgs { SelectedValue = this.values[Convert.ToInt32(row)] });
			}
		}
	public class PickerChangedEventArgs : EventArgs
	{
		public string SelectedValue { get; set; }
	}
}