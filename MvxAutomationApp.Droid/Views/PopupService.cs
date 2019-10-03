using System;
using Android.Widget;
using MvvmCross.Platforms.Android;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;

namespace MvxAutomationApp.Droid.Views
{
    public class PopupService : IPopupService
    {
        private readonly IMvxAndroidCurrentTopActivity _activity;

        public PopupService(IMvxAndroidCurrentTopActivity activity)
        {
            _activity = activity;
        }

        public void Show(MessageType messageType, string message = "")
        {
            //todo custom toast
            //todo different toasts
            //todo predefined messages when empty
            Toast.MakeText(_activity.Activity, message, ToastLength.Short).Show();
        }
    }
}