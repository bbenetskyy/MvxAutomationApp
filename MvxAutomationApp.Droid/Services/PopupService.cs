using System;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;

namespace MvxAutomationApp.Droid.Services
{
    public class PopupService : IPopupService
    {
        private readonly Activity _activity;

        public PopupService(IMvxAndroidCurrentTopActivity activity)
        {
            _activity = activity.Activity;
        }

        public void Show(MessageType messageType, string message = null)
        {
            var backgroundColor = GetBackgroundColor(messageType);
            var toast = Toast.MakeText(_activity, message, ToastLength.Short);
            toast.View.SetBackgroundColor(backgroundColor);
            toast.Show();
        }

        public Task<DateTimeOffset> PickDate()
        {
            var dateNow = DateTimeOffset.Now;
            var taskSource = new TaskCompletionSource<DateTimeOffset>();
            var picker = new DatePickerDialog(_activity, (sender, args) =>
             {
                 taskSource.SetResult(args.Date);
             }, dateNow.Year, dateNow.Month - 1, dateNow.Day);
            picker.Show();
            return taskSource.Task;
        }

        private Color GetBackgroundColor(MessageType messageType)
        {
            int colorId;
            switch (messageType)
            {
                case MessageType.Error:
                    colorId = Resource.Color.errorColor;
                    break;
                case MessageType.Success:
                    colorId = Resource.Color.successColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }

            var backgroundColor = new Color(ContextCompat.GetColor(_activity, colorId));
            return backgroundColor;
        }
    }
}