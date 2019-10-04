using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvxAutomationApp.Core.ViewModels;
using Fragment = Android.Support.V4.App.Fragment;

namespace MvxAutomationApp.Droid.Views
{
    [Activity(
        Theme = "@style/AppTheme",
        WindowSoftInputMode = SoftInput.AdjustResize | SoftInput.StateHidden,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.FontScale | ConfigChanges.ScreenLayout | ConfigChanges.Density | ConfigChanges.UiMode)]
    public class MainContainerActivity : MvxAppCompatActivity<MainContainerViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main_container);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
            }
        }

        public override void OnAttachFragment(Fragment fragment)
        {
            base.OnAttachFragment(fragment);
            UpdateBackButtonVisibility();
        }

        private void UpdateBackButtonVisibility()
        {
            var backButtonEnabled = SupportFragmentManager.BackStackEntryCount > 0;

            SupportActionBar.SetDisplayHomeAsUpEnabled(backButtonEnabled);
            SupportActionBar.SetHomeButtonEnabled(backButtonEnabled);
        }

        protected override void AttachBaseContext(Context @base)
        {
            var configuration = new Configuration(@base.Resources.Configuration)
            {
                FontScale = 1f
            };
            var config = @base.CreateConfigurationContext(configuration);
            base.AttachBaseContext(config);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            UpdateBackButtonVisibility();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId != Resource.Id.home)
            {
                SupportFragmentManager.PopBackStackImmediate();
                UpdateBackButtonVisibility();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
