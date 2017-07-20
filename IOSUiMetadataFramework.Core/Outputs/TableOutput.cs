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
	using UIKit;

	[Output(Type = "table")]
	public class TableOutput : IOutputManager
	{
		private UIView OutputList { get; set; }
		public UIView GetView(UIView viewController, string name, object value, FormView formView, int yAxis)
		{
			var list = ((IEnumerable<object>)value).ToList();
			var properties = list[0].GetType().GetProperties();
			var height = properties.Length * 25 * list.Count;
			this.OutputList =new UIView();
			var size = new CGSize(viewController.Frame.Width - 40, height + 30);
			this.OutputList.Frame = new CGRect(new CGPoint(20, yAxis), size);
			var label = new UITextView { Text = name };
			var labelSize = new CGSize(viewController.Frame.Width - 40, 30);
			label.Frame = new CGRect(new CGPoint(0, 0), labelSize);
			this.OutputList.AddSubview(label);
			var listViewSize = new CGSize(viewController.Frame.Width - 40, height);
			var listView = new UITableView(new CGRect(new CGPoint(10, 30), listViewSize))
			{
				Source = new CustomTableViewController(list)
			};

			this.OutputList.AddSubview(listView);

			return this.OutputList;
		}
	}

	public class CustomTableViewController : UITableViewSource
	{

		public CustomTableViewController(List<object> list)
		{
			this.List = list;
		}

		public List<object> List { get; set; }

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return this.List.Count;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			var properties = this.List[indexPath.Row].GetType().GetProperties();
			return properties.Length * 25;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			string CellIdentifier = "TableCell";
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);

			var properties = this.List[indexPath.Row].GetType().GetProperties();
			
			
			if (cell == null)
			{ cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier); }
			var y = 0;
			//cell.TextLabel.Text = item;
			foreach (var property in properties)
			{
				var item = property.Name + ": " + property.GetValue(this.List[indexPath.Row], null);
				var view = new UITextView { Text = item };
				var labelSize = new CGSize(cell.Frame.Width, 25);
				view.Frame = new CGRect(new CGPoint(0, y), labelSize);
				cell.AddSubview(view);
				y += 25;
			}
			
			return cell;
		}

	}
}