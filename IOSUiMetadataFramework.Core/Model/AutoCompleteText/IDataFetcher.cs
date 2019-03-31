namespace IOSUiMetadataFramework.Core.Model.AutoCompleteText
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDataFetcher
	{
		Task PerformFetch(AutoCompleteTextField textField, Action<ICollection<string>> completionHandler);
	}
}

