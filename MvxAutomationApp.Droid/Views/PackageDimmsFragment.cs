using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvxAutomationApp.Core.ViewModels;

namespace MvxAutomationApp.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel), Resource.Id.content_frame, true)]
    public class PackageDimmsFragment : MvxFragment<PackageDimmsViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.fragment_package_dimms, container, false);
        }

    }
}