using Akavache;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MvxAutomationApp.Core.ViewModels;

namespace MvxAutomationApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<MainViewModel>();

            BlobCache.ApplicationName = GetType().Assembly.FullName;
        }
    }
}
