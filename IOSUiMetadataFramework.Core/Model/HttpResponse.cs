namespace IOSUiMetadataFramework.Core
{
    using System.Collections.Generic;
    using UiMetadataFramework.MediatR;

    public class InvokeFormResponse : HttpResponse
	{
		public List<InvokeForm.Response> Response { get; set; }
	}

	public class HttpResponse
	{
		public string Cookies { get; set; }
	}
}