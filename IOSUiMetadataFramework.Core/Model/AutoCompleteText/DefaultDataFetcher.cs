namespace IOSUiMetadataFramework.Core.Model.AutoCompleteText
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DefaultDataFetcher : IDataFetcher
	{
		private ICollection<string> UnsortedData { get;}

		public DefaultDataFetcher(ICollection<string> unsortedData)
		{
			this.UnsortedData = unsortedData;
		}

		public async Task PerformFetch(AutoCompleteTextField textfield, Action<ICollection<string>> completionHandler)
		{
			completionHandler(this.UnsortedData);
		}
	}
}

