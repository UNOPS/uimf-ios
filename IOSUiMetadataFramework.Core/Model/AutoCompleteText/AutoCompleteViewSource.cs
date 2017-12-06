namespace IOSUiMetadataFramework.Core.Model.AutoCompleteText
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Foundation;
    using UIKit;

    public abstract class AutoCompleteViewSource : UITableViewSource
	{
		public event EventHandler RowSelectedEvent;

		private ICollection<string> suggestionsList = new List<string>();
		public ICollection<string> Suggestions
		{
			get => this.suggestionsList;
		    set
			{
				this.suggestionsList = value;
				this.NewSuggestions(this.suggestionsList);
			}
		}

		public abstract void NewSuggestions(ICollection<string> suggestions);

		public AutoCompleteTextField AutoCompleteTextField
		{
			get;
			set;
		}

		public abstract override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath);

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return this.Suggestions.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
		    if (this.AutoCompleteTextField.SelectionMode == AutoCompleteTextMode.SingleChoice)
		    {
		        this.AutoCompleteTextField.Text = this.Suggestions.ElementAt(indexPath.Row);
		    }
		    else
		    {
		        var choices = this.AutoCompleteTextField.Text.Split(',');
		        var result = "";
		        if (choices.Length > 1)
		        {
		            for (var i = 0; i < choices.Length -1; i++)
		            {
		                result += choices[i] + ",";
		            }
		        }
		        this.AutoCompleteTextField.Text = result + this.Suggestions.ElementAt(indexPath.Row) +",";

            }
			
			this.AutoCompleteTextField.AutoCompleteTableView.Hidden = true;

			this.RowSelectedEvent?.Invoke(this, EventArgs.Empty);
		}
	}
}

