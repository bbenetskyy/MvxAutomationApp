using Akavache;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MvxAutomationApp.Core.ViewModels;

namespace MvxAutomationApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IBlobCache>(BlobCache.LocalMachine);

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            BlobCache.ApplicationName = GetType().Assembly.FullName;

            RegisterAppStart<MainViewModel>();

        }
    }
}
