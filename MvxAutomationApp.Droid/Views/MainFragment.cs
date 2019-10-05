using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvxAutomationApp.Core.ViewModels;

namespace MvxAutomationApp.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel), Resource.Id.content_frame)]
    public class MainFragment : MvxFragment<MainViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_main, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
#if !DEBUG
            var progressBar = View.FindViewById<ProgressBar>(Resource.Id.main_progressBar);
            progressBar.Visibility = ViewStates.Invisible;

            var @switch = View.FindViewById<Switch>(Resource.Id.main_switch);
            @switch.Visibility = ViewStates.Invisible;
#endif
        }
    }
}
