namespace IOSUiMetadataFramework.Core.Inputs
{
    using System.Collections.Generic;
    using System.Linq;
    using CoreGraphics;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model.AutoCompleteText;
    using UiMetadataFramework.Basic.Input.Typeahead;
    using UIKit;

    [Input(Type = "typeahead")]
    public class TypeaheadInput : IInputManager
    {
        private AutoCompleteTextField InputText { get; set; }
        private IList<TypeaheadItem<object>> Items { get; set; }

        public UIView GetView(object inputCustomProperties, MyFormHandler myFormHandler)
        {
            this.InputText = new AutoCompleteTextField();
            this.InputText.SetTextBorders();
            var paddingView = new UIView(new CGRect(0, 0, 10, 20));
            this.InputText.LeftView = paddingView;
            this.InputText.LeftViewMode = UITextFieldViewMode.Always;
            this.Items = new List<TypeaheadItem<object>>();
            var properties = inputCustomProperties.CastTObject<TypeaheadCustomProperties>();
            var source = properties.GetTypeaheadSource(myFormHandler);
            foreach (var item in source)
            {
                this.Items.Add(item.CastTObject<TypeaheadItem<object>>());
            }
            this.InputText.Setup(this.Items.Select(a => a.Label).ToList());

            return this.InputText;
        }

        public object GetValue()
        {
            if (!string.IsNullOrEmpty(this.InputText.Text))
            {
                return new TypeaheadItem<object>
                {
                    Label = this.InputText.Text,
                    Value = this.Items.SingleOrDefault(a => a.Label.Equals(this.InputText.Text))?.Value
                };
            }
            return null;
        }

        public void SetValue(object value)
        {
            TypeaheadItem<object> typeaheadValue = value.CastTObject<TypeaheadItem<object>>();
            var label = this.Items.SingleOrDefault(a => a.Value.Equals(typeaheadValue.Value))?.Label;
            this.InputText.Text = label;
        }
    }
}