namespace IOSUiMetadataFramework.Core.Model.AutoCompleteText
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CoreGraphics;
    using UIKit;

    public class AutoCompleteTextField : UITextField, IUITextFieldDelegate
    {
        private AutoCompleteViewSource autoCompleteViewSource;

        public AutoCompleteTextField(IntPtr ptr) : base(ptr)
        {
        }

        public AutoCompleteTextField()
        {
        }

        public UITableView AutoCompleteTableView { get; private set; }

        public int AutocompleteTableViewHeight { get; set; } = 150;
        public AutoCompleteTextMode SelectionMode { get; set; } = AutoCompleteTextMode.SingleChoice;

        public AutoCompleteViewSource AutoCompleteViewSource
        {
            get => this.autoCompleteViewSource;
            set
            {
                this.autoCompleteViewSource = value;
                this.autoCompleteViewSource.AutoCompleteTextField = this;
                if (this.AutoCompleteTableView != null)
                {
                    this.AutoCompleteTableView.Source = this.AutoCompleteViewSource;
                }
            }
        }

        public IDataFetcher DataFetcher { get; set; }

        public ISortingAlghorithm SortingAlghorithm { get; set; } = new NoSortingAlghorithm();

        public int StartAutoCompleteAfterTicks { get; set; } = 0;

        public void Setup(IList<string> suggestions)
        {
            this.DataFetcher = new DefaultDataFetcher(suggestions);
            this.AutoCompleteViewSource = new DefaultDataSource();
            this.InitializeTableView();
        }

        public void Setup(IDataFetcher fetcher)
        {
            this.DataFetcher = fetcher;
            this.AutoCompleteViewSource = new DefaultDataSource();
            this.InitializeTableView();
        }

        public void Setup(IList<string> suggestions, UITableViewController tableViewController)
        {
            this.Setup(suggestions);
        }

        public async Task UpdateTableViewData()
        {
            await this.DataFetcher.PerformFetch(this, delegate(ICollection<string> unsortedData)
                {
                    var text = "";
                    if (this.SelectionMode == AutoCompleteTextMode.SingleChoice)
                    {
                        text = this.Text;
                    }
                    else
                    {
                        var choices = this.Text.Split(',');
                        text = choices[choices.Length -1 ];

                    }
                    var sorted = this.SortingAlghorithm.DoSort(text, unsortedData);
                    this.AutoCompleteViewSource.Suggestions = sorted;

                    this.AutoCompleteTableView.ReloadData();
                }
            );
        }

        private void HideAutoCompleteView()
        {
            this.AutoCompleteTableView.Hidden = true;
        }

        private void InitializeTableView()
        {
            //Some textfield settings
            this.Delegate = this;
            this.AutocorrectionType = UITextAutocorrectionType.No;
            this.ClearButtonMode = UITextFieldViewMode.WhileEditing;

            //listen to edit events
            this.EditingChanged += async (sender, eventargs) =>
            {
                if (this.Text.Length > this.StartAutoCompleteAfterTicks)
                {
                    this.ShowAutoCompleteView();
                    await this.UpdateTableViewData();
                }
                else
                {
                    this.AutoCompleteTableView.Hidden = true;
                }
            };

            this.EditingDidEnd += (sender, eventargs) => { this.HideAutoCompleteView(); };
        }

        private void ShowAutoCompleteView()
        {
            if (this.AutoCompleteTableView == null)
            {
                var listViewSize = new CGSize(this.Frame.Width, this.AutocompleteTableViewHeight);
                this.AutoCompleteTableView = new UITableView(new CGRect(new CGPoint(this.Frame.X, this.Frame.Y + this.Frame.Height), listViewSize));
                this.AutoCompleteTableView.Layer.CornerRadius = 5; //rounded corners
                this.AutoCompleteTableView.ContentInset = UIEdgeInsets.Zero;
                this.AutoCompleteTableView.AutoresizingMask =
                    UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight; //for resizing (switching from table to portait for example)
                this.AutoCompleteTableView.Bounces = false;
                this.AutoCompleteTableView.BackgroundColor = UIColor.Clear;
                this.AutoCompleteTableView.Source = this.AutoCompleteViewSource;
                this.AutoCompleteTableView.Hidden = true;
                this.Superview.AddSubview(this.AutoCompleteTableView);
            }
            this.AutoCompleteTableView.SetContentOffset(CGPoint.Empty, false);
            this.AutoCompleteTableView.Hidden = false;
        }
    }
}