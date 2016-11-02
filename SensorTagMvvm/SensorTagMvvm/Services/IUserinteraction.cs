using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SensorTagMvvm.Services
{
    public interface IUserInteraction
    {
        void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel");
        void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel");
        Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel");

        void Alert(string message, Action done = null, string title = "", string okButton = "OK");
        Task AlertAsync(string message, string title = "", string okButton = "OK");

        void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null);
        void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null);
        Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null);

        void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe");
        Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe");
    }
}