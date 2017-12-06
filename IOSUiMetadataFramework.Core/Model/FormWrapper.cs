using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace IOSUiMetadataFramework.Core.Model
{
    using System.Threading.Tasks;
    using UiMetadataFramework.Core;

    public interface IFormWrapper
    {
        Task UpdateViewAsync(MyFormHandler myFormHandler, FormMetadata metadata, IDictionary<string, object> inputFieldValues = null);
         void UpdateView(MyFormHandler myFormHandler, FormMetadata metadata, IDictionary<string, object> inputFieldValues = null);
    }
}