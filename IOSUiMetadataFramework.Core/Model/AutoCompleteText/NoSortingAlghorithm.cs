namespace IOSUiMetadataFramework.Core.Model.AutoCompleteText
{
    using System.Collections.Generic;
    using System.Linq;

    public class NoSortingAlghorithm : ISortingAlghorithm
	{
		public ICollection<string> DoSort(string userInput, ICollection<string> inputStrings)
		{
			return inputStrings.Where(a => a.ToLower().Contains(userInput.ToLower())).ToList();
		}
	}
}

