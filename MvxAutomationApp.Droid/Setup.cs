using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.IoC;
using MvxAutomationApp.Core;
using MvxAutomationApp.Core.Services;
using MvxAutomationApp.Droid.Services;
using MvxAutomationApp.Droid.Views;

namespace MvxAutomationApp.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IPopupService, PopupService>();
        }
    }
}
