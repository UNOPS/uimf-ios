namespace IOSUiMetadataFramework.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CoreGraphics;
	using IOSUiMetadataFramework.Core.Managers;
	using IOSUiMetadataFramework.Core.Model;
	using MediatR;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UiMetadataFramework.Basic.Response;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.MediatR;
	using UIKit;

	public class MyFormHandler
	{
		public int ResultYAxis = 30;
		public int YAxis = 310;

		public MyFormHandler(
			IMediator mediator,
			FormRegister formRegister,
			InputManagerCollection inputManager,
			OutputManagerCollection outputManager)
		{
			this.Mediator = mediator;
			this.InputManagerCollection = inputManager;
			this.OutputManagerCollection = outputManager;
			this.FormRegister = formRegister;
		}

	    public MyFormHandler(
            IFormWrapper formWrapper,
	        UiMetadataWebApi uiMetadataWebApi,
	        InputManagerCollection inputManager,
	        OutputManagerCollection outputManager,
	        EventHandlerManagerCollection eventManager,
	        Dictionary<string, FormMetadata> allForms)
	    {
	        this.AllFormsMetadata = allForms;
	        this.EventHandlerManager = eventManager;
	        this.InputManagerCollection = inputManager;
	        this.OutputManagerCollection = outputManager;
	        this.UiMetadataWebApi = uiMetadataWebApi;
	        this.FormWrapper = formWrapper;
	    }

	    public IFormWrapper FormWrapper { get; set; }

	    public UiMetadataWebApi UiMetadataWebApi { get; set; }

	    public Dictionary<string, FormMetadata> AllFormsMetadata { get; set; }

	    public EventHandlerManagerCollection EventHandlerManager { get; set; }

		private FormRegister FormRegister { get; }
		private InputManagerCollection InputManagerCollection { get; }
		private IMediator Mediator { get; }
		public OutputManagerCollection OutputManagerCollection { get; }

		public async Task<Layout> StartIForm(Type form, IDictionary<string, object> inputFieldValues = null)
		{
			var formMetadata = this.FormRegister.GetFormInfo(form.FullName)?.Metadata;
            var layout = await this.GetIFormAsync(formMetadata, inputFieldValues);
		    try
		    {
		        await this.FormWrapper.UpdateViewAsync(this, formMetadata, inputFieldValues);
		    }
		    catch (NotImplementedException)
		    {
		        this.FormWrapper.UpdateView(this, formMetadata, inputFieldValues);
		    }
            return layout;
		}

		public async Task<Layout> GetIFormAsync(string form, IDictionary<string, object> inputFieldValues = null)
		{
		    FormMetadata formMetadata = await this.GetFormMetadataAsync(form);
            var layout = await this.GetIFormAsync(formMetadata, inputFieldValues);
			return layout;
		}

	    public async Task<FormMetadata> GetFormMetadataAsync(string form)
	    {
	        FormMetadata formMetadata;
	        if (this.AllFormsMetadata != null)
	        {
	            formMetadata = this.AllFormsMetadata[form];

	        }
	        else if (this.UiMetadataWebApi != null)
	        {
	            formMetadata = await UiMetadataHttpRequestHelper.GetFormMetadata(form, this.UiMetadataWebApi.FormMetadataUrl, "");
	        }
	        else
	        {
	            formMetadata = this.FormRegister.GetFormInfo(form)?.Metadata;
	        }

	        if (formMetadata == null)
	        {
	            this.ShowToast("Error fetching data. Server returned status code: {0}");
	            return null;
	        }
	        return formMetadata;
	    }

        public async Task<Layout> GetIFormAsync(FormMetadata formMetadata, IDictionary<string, object> inputFieldValues = null)
	    {
	        if (formMetadata == null)
	        {
	            this.ShowToast("You don't have access to this form");
	            return null;
	        }
	        try
	        {
	            var formParameters = new FormParameters(formMetadata, inputFieldValues);
	            var layout = await this.DrawFormAsync(formParameters);
	            return layout;
            }
	        catch (Exception ex)
	        {
	            this.ShowToast(ex.Message);
	            return null;
            }	       
        }

        public async void StartIFormAsyn(string form, IDictionary<string, object> inputFieldValues = null)
		{
		    FormMetadata  formMetadata= await this.GetFormMetadataAsync(form);
            try
		    {
		        await this.FormWrapper.UpdateViewAsync(this, formMetadata, inputFieldValues);
		    }
		    catch (NotImplementedException)
		    {
		        this.FormWrapper.UpdateView(this, formMetadata, inputFieldValues);
		    }
		}

		private async Task<Layout> DrawFormAsync(FormParameters formParameters)
		{
			this.YAxis = 10;
		    UIScrollView scrollView = new UIScrollView { BackgroundColor = UIColor.White };
		    var frame = scrollView.Frame;
            if (formParameters.Form != null)
			{
				InvokeForm.Response result = null;
				var resultLayout = new UIView();
			    List<FormInputManager> inputsManager = new List<FormInputManager>();
                if (formParameters.Form.InputFields.Count > 0)
				{
				   
                    this.DrawInputs(scrollView, formParameters, inputsManager);				  
				    frame.Height += this.YAxis + 20;
                    scrollView.ContentSize = frame.Size;

                    if (formParameters.Form.InputFields.Count(a => !a.Hidden) > 0)
					{
					    var submitLabel = "Submit";
					    if (formParameters.Form.CustomProperties != null)
					    {
					        var customeProperties = (JObject)formParameters.Form.CustomProperties;
					        var submitbuttonlabel = customeProperties.GetValue("submitButtonLabel")?.ToString();

					        if (!string.IsNullOrEmpty(submitbuttonlabel))
					        {
					            submitLabel = submitbuttonlabel;
					        }
					    }
                        var btn = new UIButton();
						btn.SetTitle(submitLabel, UIControlState.Normal);
						var btnSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 40);
						btn.Frame = new CGRect(new CGPoint(20, this.YAxis), btnSize);
						btn.BackgroundColor = UIColor.Gray;
						this.YAxis += 45;
						btn.TouchUpInside += async (sender, args) =>
						{
		
							result = await this.SubmitFormAsync(resultLayout,formParameters.Form, inputsManager);
							this.DrawOutput(resultLayout, result, formParameters.Form, inputsManager);

						};
						scrollView.AddSubview(btn);
					}
				}

			    // run on response handled events
			    EventsManager.OnFormLoadedEvent(formParameters);

                if (formParameters.Form.PostOnLoad)
				{

					result = await this.SubmitFormAsync(resultLayout,formParameters.Form, inputsManager, formParameters.Form.PostOnLoadValidation);
					this.DrawOutput(resultLayout, result, formParameters.Form, inputsManager);
				}

			    resultLayout.UserInteractionEnabled = true;
                var resSize = new CGSize(UIScreen.MainScreen.Bounds.Width, this.ResultYAxis + 10);
			    resultLayout.Frame = new CGRect(new CGPoint(0, this.YAxis), resSize);
			    scrollView.AddSubview(resultLayout);

			    frame.Height += resultLayout.Frame.Height;
			    scrollView.ContentSize = frame.Size;
            }
			return new Layout(scrollView, null);
		}

	    private async Task<InvokeForm.Response> SubmitFormAsync(UIView resultLayout, FormMetadata formMetadata, List<FormInputManager> inputsManager, bool validate = true)
	    {
	        var valid = !validate || this.ValidateForm(inputsManager);
	        if (valid)
	        {
	            this.ResultYAxis = 0;
                resultLayout.RemoveAllViews();
	            return await this.HandleFormAsync(formMetadata, inputsManager);
	        }
	        return null;
	    }

        public void DrawInputs(UIScrollView layout, FormParameters formParameters, List<FormInputManager> inputsManager)
		{
			var orderedInputs = formParameters.Form.InputFields.OrderBy(a => a.OrderIndex).ToList();

		    inputsManager.Clear();

			var size = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 45);
			var frame = layout.Frame;
			foreach (var input in orderedInputs)
			{
				if (!input.Hidden)
				{
					var label = new UITextView { Text = input.Label };

					var labelSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 30);
					label.Frame = new CGRect(new CGPoint(20, this.YAxis), labelSize);
                    
				    //layout.InsertSubviewBelow(label, _parentViewController.View);

                    layout.AddSubview(label);
					this.YAxis += 35;
				}

				var manager = this.InputManagerCollection.GetManager(input.Type);

				var view = manager.GetView(input.CustomProperties, this);
			    var value = formParameters.Parameters?.SingleOrDefault(a => a.Key.Equals(input.Id)).Value;
			    if (value != null)
			    {
			        manager.SetValue(value);
			    }
                inputsManager.Add(new FormInputManager(input, manager, view));

				view.Hidden = input.Hidden;
				if (!input.Hidden)
				{
					frame.Height += 35 + view.Frame.Height;
					layout.ContentSize = frame.Size;
				    view.Frame = new CGRect(new CGPoint(20, this.YAxis), size);
                }				
				layout.AddSubview(view);
				this.YAxis += (int)view.Frame.Height + 10;
			}
		}

		private void DrawOutput(UIView layout, InvokeForm.Response result, FormMetadata formMetadata, List<FormInputManager> inputsManager)
		{
			if (result?.Data != null)
			{
			    var reloadResponse = result.Data.CastTObject<ReloadResponse>();
			    if (reloadResponse?.Form != null)
			    {
			        if (this.AllFormsMetadata != null)
			        {
                       
			            var metadata = this.AllFormsMetadata[reloadResponse.Form];
			            Task.Run(
			                () => this.FormWrapper.UpdateViewAsync(this, metadata, reloadResponse.InputFieldValues));
                        
                    }
			        else
			        {
			            this.StartIFormAsyn(reloadResponse.Form, reloadResponse.InputFieldValues);
			        }
			        return;
			    }

                var orderedOutputs = formMetadata.OutputFields.OrderBy(a => a.OrderIndex).ToList();

				foreach (var output in orderedOutputs)
				{
					if (!output.Hidden)
					{
						    object value;
						    if (result.Data.GetType() == typeof(JObject))
						    {
						        var jsonObj = (JObject)result.Data;
						        value = jsonObj.GetValue(output.Id, StringComparison.OrdinalIgnoreCase);
						    }
						    else
						    {
						        var propertyInfo = result.Data.GetType().GetProperty(output.Id);
						        value = propertyInfo?.GetValue(result.Data, null);
						    }
							var manager = this.OutputManagerCollection.GetManager(output.Type);
							var view = manager.GetView(output,value, this,formMetadata, inputsManager, this.ResultYAxis);
							layout.AddSubview(view);
					        this.ResultYAxis += (int)view.Frame.Height + 20;

					}
				}
			    // run on response handled events
			    EventsManager.OnResponseHandledEvent(this, formMetadata, inputsManager, result);
            }
		}

	   

	    private object GetFormValues(List<FormInputManager> inputsManager)
		{
			var list = new Dictionary<string, object>();
			foreach (var inputManager in inputsManager)
			{
				var value = inputManager.Manager.GetValue();
				if (value != null)
				{
					list.Add(inputManager.Input.Id, value);
				}
			}
			return JsonConvert.SerializeObject(list);
		}

		private async Task<InvokeForm.Response> HandleFormAsync(FormMetadata formMetadata, List<FormInputManager> inputsManager)
		{

		    try
		    {
		        var obj = this.GetFormValues(inputsManager);
		        var request = new InvokeForm.Request
		        {
		            Form = formMetadata.Id,
		            InputFieldValues = obj
		        };
		        // run on form posting events
		        EventsManager.OnFormPostingEvent(formMetadata, inputsManager);

		        object resultData = null;
		        if (this.UiMetadataWebApi != null)
		        {
		            var result = await this.InvokeFormAsync(new[] { request });
		            if (result != null)
		            {
		                resultData = result[0].Data;
		            }
		        }
		        else
		        {
		            InvokeForm.Response response = await this.Mediator.Send(request);
		            resultData = response.Data;
		        }

		        // run on response received events
		        EventsManager.OnResponseReceivedEvent(formMetadata, inputsManager, resultData);

		        return new InvokeForm.Response
		        {
		            Data = resultData
		        };
		    }
		    catch (Exception ex)
		    {
		        this.ShowToast(ex.Message);
                return null;
		    }

		}

	    public async Task<List<InvokeForm.Response>> InvokeFormAsync(object param, bool setCookies = true)
	    {
	        var response = await UiMetadataHttpRequestHelper.InvokeForm(this.UiMetadataWebApi.RunFormUrl, "",
	            param);

	        //if (setCookies)
	        //    this.AppPreference.SetSharedKey("Cookies", response.Cookies);

	        if (response.Response == null)
	        {
	            this.ShowToast("Error fetching data. Server returned status code: {0}");
	            return null;
	        }
	        return response.Response;
	    }
        private bool ValidateForm(List<FormInputManager> inputsManager)
		{
			var valid = true;
			foreach (var inputManager in inputsManager)
			{
				var value = inputManager.Manager.GetValue();
				if (inputManager.Input.Required)
				{
					if (string.IsNullOrEmpty(value?.ToString()))
					{
						valid = false;
						inputManager.View.Layer.BorderColor = UIColor.Red.CGColor;
					}
				}
			}

			return valid;
		}

		public class FormInputManager
		{
			public FormInputManager(InputFieldMetadata input, IInputManager manager, UIView view)
			{
				this.Input = input;
				this.Manager = manager;
				this.View = view;
			}

			public InputFieldMetadata Input { get; set; }
			public IInputManager Manager { get; set; }
			public UIView View { get; set; }
		}

		public void ShowToast(string message)
		{
			var toastLabel = new UILabel(frame: new CGRect(x: 20, y: UIScreen.MainScreen.Bounds.Height - 100,
				width: UIScreen.MainScreen.Bounds.Width - 40,
				height: 35))
			{
				BackgroundColor = UIColor.Black.ColorWithAlpha(0.6f),
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center,
				Text = message,
				Alpha = 1.0f
			};

			toastLabel.Layer.CornerRadius = 10;
			toastLabel.ClipsToBounds = true;
		    var window = UIApplication.SharedApplication.KeyWindow;
		    var vc = window.RootViewController;
		    while (vc.PresentedViewController != null)
		    {
		        vc = vc.PresentedViewController;
		    }
		    vc.View.AddSubview(toastLabel);
			UIView.Animate(4, 0.1, UIViewAnimationOptions.CurveEaseOut, () => { toastLabel.Alpha = 0; }, () => { toastLabel.RemoveFromSuperview(); });
		}
	}
}