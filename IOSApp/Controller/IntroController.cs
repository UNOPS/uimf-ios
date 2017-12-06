namespace IOSApp.Controller
{
    using System;
    using CoreGraphics;
    using Foundation;
    using UIKit;

    partial class IntroController : BaseController
    {
        public IntroController() : base("IntroController", null)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //UIScrollView view = new UIScrollView();
            //var resultLayout = new UIView();
            //var table = new UITableView(View.Bounds); // defaults to Plain style
            //string[] tableItems = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            //table.Source = new TableSource(tableItems);

            //resultLayout.Add(table);
            //var resSize = new CGSize(table.Frame.Width, (int)table.Frame.Height + 20);
            //resultLayout.Frame = new CGRect(new CGPoint(0, 0), resSize);
            //view.AddSubview(resultLayout);
            ////view.AddSubview(resultLayout);

            //var frame = view.Frame;
            //frame.Height += resultLayout.Frame.Height;
            //view.ContentSize = frame.Size;
            //this.View = view;
        }

        //public class TableSource : UITableViewSource
        //{

        //    string[] TableItems;
        //    string CellIdentifier = "TableCell";

        //    public TableSource(string[] items)
        //    {
        //        TableItems = items;
        //    }

        //    public override nint RowsInSection(UITableView tableview, nint section)
        //    {
        //        return TableItems.Length;
        //    }

        //    public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        //    {
        //        UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
              

        //        //---- if there are no cells to reuse, create a new one
        //        if (cell == null)
        //        { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

        //        //  cell.TextLabel.Text = item;

        //        string item = TableItems[indexPath.Row];
        //        UIButton btn = new UIButton();
        //        btn.SetTitle(item, UIControlState.Normal);
        //        btn.SetTitleColor(UIColor.Black, UIControlState.Normal);
        //        btn.BackgroundColor = UIColor.Cyan;
        //        btn.UserInteractionEnabled = true;
        //        var btnSize = new CGSize(100, 20);
        //        btn.Frame = new CGRect(new CGPoint(20, 0), btnSize);
        //        btn.TouchUpInside += (sender, args) =>
        //        {
        //            var x = 100;
        //            var y = x + 2;
        //        };
        //        cell.AddSubview(btn);

        //        return cell;
        //    }
        //}

    }
}