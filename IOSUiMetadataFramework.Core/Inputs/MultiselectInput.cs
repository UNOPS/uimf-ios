namespace IOSUiMetadataFramework.Core.Inputs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CoreGraphics;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using IOSUiMetadataFramework.Core.Model.AutoCompleteText;
    using UiMetadataFramework.Basic.Input.Typeahead;
    using UiMetadataFramework.MediatR;
    using UIKit;

    [Input(Type = "multiselect")]
	public class MultiselectInput : IInputManager
	{
	    private AutoCompleteTextField InputText { get; set; }
	    private List<TypeaheadItem<object>> ItemsList { get; set; }
        private object CustomeSource { get; set; }
        public UIView GetView(IDictionary<string, object> inputCustomProperties, MyFormHandler myFormHandler)
	    {
	        this.InputText = new AutoCompleteTextField
	        {
	            SelectionMode = AutoCompleteTextMode.MultiChoice
	        };
            this.InputText.SetTextBorders();
	        UIView paddingView = new UIView(new CGRect(0, 0, 10, 20));
	        this.InputText.LeftView = paddingView;
	        this.InputText.LeftViewMode = UITextFieldViewMode.Always;

	        this.ItemsList = new List<TypeaheadItem<object>>();
            this.CustomeSource = inputCustomProperties.GetCustomProperty<object>("source");
            var source = this.CustomeSource.GetTypeaheadSource(myFormHandler, new TypeaheadRequest<object> { Query = "" });
            foreach (var item in source)
	        {
	            this.ItemsList.Add(item.CastTObject<TypeaheadItem<object>>());
	        }
	        this.InputText.Setup(this.ItemsList.Select(a => a.Label).ToList());

            return this.InputText;
	    }

	    public object GetValue()
	    {
	        var items = this.InputText.Text.Split(',');
	        var selectedItems = this.ItemsList.Where(a => items.Contains(a.Label)).Select(a => a.Value);

	        return new MultiSelect<object>
	        {
	            Items = selectedItems.ToList()
	        };
        }

	    public void SetValue(object value)
	    {
	        var typeahead = value.CastTObject<MultiSelect<object>>();
	        if (typeahead != null)
	            this.InputText.Text = string.Join(",", typeahead.Items);
        }

    }
}