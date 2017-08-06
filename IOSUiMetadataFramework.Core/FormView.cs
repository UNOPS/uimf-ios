namespace IOSUiMetadataFramework.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using CoreGraphics;
	using IOSUiMetadataFramework.Core.Managers;
	using MediatR;
	using Newtonsoft.Json;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.MediatR;
	using UIKit;

	public class FormView
	{
		public int ResultYAxis = 30;
		public int YAxis = 310;

		public FormView(UIView viewController,
			IMediator mediator,
			FormRegister formRegister,
			InputManagerCollection inputManager,
			OutputManagerCollection outputManager,
			List<Layout> appLayouts)
		{
			this.ViewController = viewController;
			this.Mediator = mediator;
			this.InputsManager = new List<FormInputManager>();
			this.InputManagerCollection = inputManager;
			this.OutputManagerCollection = outputManager;
			this.FormRegister = formRegister;
			this.AppLayouts = appLayouts;
		}

		public List<Layout> AppLayouts { get; set; }
		public IDictionary<string, object> InputFieldValues { get; set; }
		private FormMetadata FormMetadata { get; set; }
		private FormRegister FormRegister { get; }
		private InputManagerCollection InputManagerCollection { get; }
		private List<FormInputManager> InputsManager { get; }
		private IMediator Mediator { get; }
		private OutputManagerCollection OutputManagerCollection { get; }
		private UIView ViewController { get; }

		public async Task<Layout> StartIForm(Type form, IDictionary<string, object> inputFieldValues = null)
		{
			this.FormMetadata = this.FormRegister.GetFormInfo(form.FullName)?.Metadata;
			this.InputFieldValues = inputFieldValues;
			var layout = await this.StartIForm();
			if (layout != null)
			{
				this.ViewController.RemoveAllViews();
				this.ViewController.AddSubview(layout.View);
				this.ViewController.AddConstraints(layout.Constraints);
				this.AppLayouts.Add(layout);
			}
			return layout;
		}

		public async Task<Layout> GetIForm(string form, IDictionary<string, object> inputFieldValues = null)
		{
			this.FormMetadata = this.FormRegister.GetFormInfo(form)?.Metadata;
			this.InputFieldValues = inputFieldValues;
			var layout = await this.StartIForm();
			return layout;
		}

		public async Task<Layout> StartIForm(string form, IDictionary<string, object> inputFieldValues = null)
		{
			this.FormMetadata = this.FormRegister.GetFormInfo(form)?.Metadata;
			this.InputFieldValues = inputFieldValues;
			var layout = await this.StartIForm();
			if (layout != null)
			{
				this.ViewController.RemoveAllViews();
				this.ViewController.AddSubview(layout.View);
				this.ViewController.AddConstraints(layout.Constraints);
				this.AppLayouts.Add(layout);
			}
			return layout;
		}

		public async Task<Layout> StartIForm()
		{
			try
			{
				var layout = await this.DrawIFormAsync();
				return layout;
			}
			catch (Exception ex)
			{
				this.ShowToast(ex.Message);
				return null;
			}
		}

		private async Task<Layout> DrawIFormAsync()
		{
			this.YAxis = 10;
			UIScrollView view = new UIScrollView();

			var constraints = view.FillParent(this.ViewController);
			
			view.BackgroundColor = UIColor.White;
			if (this.FormMetadata != null)
			{
				InvokeForm.Response result = null;
				var resultLayout = new UIView();

				if (this.FormMetadata.InputFields.Count > 0)
				{
					this.DrawInputs(view);
					if (this.FormMetadata.InputFields.Count(a => !a.Hidden) > 0)
					{
						var btn = new UIButton();
						btn.SetTitle("Submit", UIControlState.Normal);
						var btnSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 40);
						btn.Frame = new CGRect(new CGPoint(20, this.YAxis), btnSize);
						btn.BackgroundColor = UIColor.Gray;
						this.YAxis += 45;
						btn.TouchUpInside += async (sender, args) =>
						{
							var valid = this.ValidateForm();
							if (valid)
							{
								this.ResultYAxis = 0;
								resultLayout.RemoveAllViews();
								result = await this.HandelForm();
								this.DrawOutput(resultLayout, result, view);
							}
						};
						view.AddSubview(btn);
					}
				}
				if (this.FormMetadata.PostOnLoad)
				{
					var valid = this.ValidateForm();
					if (valid)
					{
						this.ResultYAxis = 0;
						resultLayout.RemoveAllViews();
						result = await this.HandelForm();
						this.DrawOutput(resultLayout, result, view);
					}
				}
			}
			return new Layout(view, constraints);
		}

		public void DrawInputs(UIScrollView layout)
		{
			var orderedInputs = this.FormMetadata.InputFields.OrderBy(a => a.OrderIndex).ToList();

			this.InputsManager.Clear();

			var size = new CGSize(this.ViewController.Frame.Width - 40, 45);
			var frame = layout.Frame;
			foreach (var input in orderedInputs)
			{
				if (!input.Hidden)
				{
					var label = new UITextView { Text = input.Label };
					var labelSize = new CGSize(this.ViewController.Frame.Width - 40, 30);
					label.Frame = new CGRect(new CGPoint(20, this.YAxis), labelSize);
					layout.AddSubview(label);
					this.YAxis += 35;
				}

				var manager = this.InputManagerCollection.GetManager(input.Type);

				var view = manager.GetView(input.CustomProperties);
				var value = this.InputFieldValues?.SingleOrDefault(a => a.Key.Equals(input.Id)).Value;
				if (value != null)
				{
					manager.SetValue(value);
				}
				this.InputsManager.Add(new FormInputManager(input, manager, view));
				if (input.DefaultValue != null)
				{
					manager.SetValue(input.DefaultValue.Id);
				}

				view.Hidden = input.Hidden;
				if (!input.Hidden)
				{
					frame.Height += 35 + view.Frame.Height;
					layout.ContentSize = frame.Size;
				}
				view.Frame = new CGRect(new CGPoint(20, this.YAxis), size);
				layout.AddSubview(view);
				this.YAxis += (int)view.Frame.Height + 10;
			}
		}

		private void DrawOutput(UIView layout, InvokeForm.Response result, UIScrollView scrollView)
		{
			if (result != null)
			{
				var orderedOutputs = this.FormMetadata.OutputFields.OrderBy(a => a.OrderIndex).ToList();

				foreach (var output in orderedOutputs)
				{
					var propertyInfo = result.Data.GetType().GetProperty(output.Id);
					if (propertyInfo != null)
					{
						if (!output.Hidden)
						{
							var value = propertyInfo.GetValue(result.Data, null);
							var manager = this.OutputManagerCollection.GetManager(output.Type);
							var view = manager.GetView(this.ViewController, output.Label, value, this, this.ResultYAxis);
							layout.AddSubview(view);
							this.ResultYAxis += (int)view.Frame.Height + 5;
						}
					}
				}
				var resSize = new CGSize(scrollView.Bounds.Width, this.ResultYAxis + 10);
				layout.Frame = new CGRect(new CGPoint(0, this.YAxis), resSize);
				scrollView.AddSubview(layout);

				var frame = scrollView.Frame;
				frame.Height += layout.Frame.Height;
				scrollView.ContentSize = frame.Size;
			}
		}

		private object GetFormValues()
		{
			var list = new Dictionary<string, object>();
			foreach (var inputManager in this.InputsManager)
			{
				var value = inputManager.Manager.GetValue();
				if (value != null)
				{
					list.Add(inputManager.Input.Id, value);
				}
			}
			return JsonConvert.SerializeObject(list);
		}

		private async Task<InvokeForm.Response> HandelForm()
		{
			try
			{
				var obj = this.GetFormValues();
				var request = new InvokeForm.Request
				{
					Form = this.FormMetadata.Id,
					InputFieldValues = obj
				};

				var response = await this.Mediator.Send(request);

				return new InvokeForm.Response
				{
					RequestId = request.RequestId,
					Data = response.Data
				};
			}
			catch (Exception ex)
			{
				this.ShowToast(ex.Message);
			}

			return new InvokeForm.Response();
		}

		private bool ValidateForm()
		{
			var valid = true;
			foreach (var inputManager in this.InputsManager)
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
			var toastLabel = new UILabel(frame: new CGRect(x: 20, y: this.ViewController.Frame.Size.Height - 100,
				width: this.ViewController.Frame.Size.Width - 40,
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

			this.ViewController.AddSubview(toastLabel);
			UIView.Animate(4, 0.1, UIViewAnimationOptions.CurveEaseOut, () => { toastLabel.Alpha = 0; }, () => { toastLabel.RemoveFromSuperview(); });
		}
	}
}