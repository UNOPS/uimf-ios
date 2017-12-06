namespace IOSUiMetadataFramework.Core.Outputs
{
    using System.Collections.Generic;
    using System.Linq;
    using CoreGraphics;
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;
    using UIKit;

    [Output(Type = "paginated-data")]
	public class PaginationOutput : IOutputManager
	{
	    private UIView OutputList { get; set; }
	    private List<object> ItemList { get; set; }
	    private int PageIndex { get; set; } = 1;
	    private int TotalCount { get; set; }

        public UIView GetView(OutputFieldMetadata outputField,
            object value,
            MyFormHandler myFormHandler,
            FormMetadata formMetadata,
            List<MyFormHandler.FormInputManager> inputsManager,
            int yAxis)
	    {

	        var paginatedData = value.CastTObject<PaginatedData<object>>();
	        this.ItemList = paginatedData.Results.ToList();
	        this.TotalCount = paginatedData.TotalCount;

	        EnumerableOutputFieldProperties outputFieldProperty = outputField.CustomProperties.CastTObject<EnumerableOutputFieldProperties>();

	        this.OutputList = new UIView();
	        this.OutputList.UserInteractionEnabled = true;
	        var label = new UITextView { Text = outputField.Label };
	        var labelSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 30);
	        label.Frame = new CGRect(new CGPoint(0, 0), labelSize);
	        this.OutputList.AddSubview(label);

	      
	        UITableView tableView = new UITableView();
	        tableView.Frame = UIScreen.MainScreen.Bounds;
	        tableView.AllowsSelection = false;
           // tableView.IndexPathForCell()
            var listViewSize = new CGSize(tableView.Frame.Width, tableView.Frame.Height);

            tableView.Source = new CustomTableViewController<object>(this.ItemList, outputFieldProperty, myFormHandler, this.OutputList);

            var listView = new UITableView(new CGRect(new CGPoint(10, 30), listViewSize))
	        {
	            Source = new CustomTableViewController<object>(this.ItemList, outputFieldProperty, myFormHandler, this.OutputList)
	        };


	        if (this.TotalCount > 10)
	        {
	            if (formMetadata != null)
	            {
	                //var btnLoadMore = this.CreateLoadMoreButton(myFormHandler, formMetadata, outputField, listView, inputsManager, myFormHandler.AllFormsMetadata);
	                //listView.TableFooterView = btnLoadMore;
	            }
	        }

            listView.ContentInset = UIEdgeInsets.Zero;
	        listView.AutoresizingMask =
	            UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight; //for resizing (switching from table to portait for example)
	        listView.Bounces = false;
	        listView.SetContentOffset(CGPoint.Empty, false);
            this.OutputList.AddSubview(tableView);
	        this.OutputList.Frame = new CGRect(new CGPoint(20, yAxis), listViewSize);
            return this.OutputList;
        }
    }
}