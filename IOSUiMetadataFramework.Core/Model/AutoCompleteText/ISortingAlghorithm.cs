namespace IOSUiMetadataFramework.Core.Model.AutoCompleteText
{
    using System.Collections.Generic;

    public interface ISortingAlghorithm
    {
        ICollection<string> DoSort(string userInput, ICollection<string> inputStrings);
    }
}