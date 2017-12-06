namespace IOSUiMetadataFramework.Core.Outputs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CoreGraphics;
	using Foundation;
	using IOSUiMetadataFramework.Core;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using IOSUiMetadataFramework.Core.Model;
	using UiMetadataFramework.Core;
	using UIKit;

	[Output(Type = "table")]
	public class TableOutput : IOutputManager
	{
		private UIView OutputList { get; set; }
		public UIView GetView(OutputFieldMetadata outputField,
		    object value,
		    MyFormHandler myFormHandler,
		    FormMetadata formMetadata,
		    List<MyFormHandler.FormInputManager> inputsManager,
		    int yAxis)
		{
			var list = ((IEnumerable<object>)value).ToList();
			var properties = list[0].GetType().GetProperties();
			var height = properties.Length * 25 * list.Count;
			this.OutputList =new UIView();
			var size = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, height + 30);
			this.OutputList.Frame = new CGRect(new CGPoint(20, yAxis), size);
			var label = new UITextView { Text = outputField.Label };
			var labelSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 30);
			label.Frame = new CGRect(new CGPoint(0, 0), labelSize);
			this.OutputList.AddSubview(label);
			var listViewSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, height);
			var listView = new UITableView(new CGRect(new CGPoint(10, 30), listViewSize))
			{
				//Source = new CustomTableViewController(list)
			};

			this.OutputList.AddSubview(listView);

			return this.OutputList;
		}
	}

}