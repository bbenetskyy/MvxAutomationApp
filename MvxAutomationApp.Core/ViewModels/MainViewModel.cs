using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;

namespace MvxAutomationApp.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        public IMvxCommand PackageDimmsCommand { get; }
        public IMvxCommand ShowPackagesCommand { get; }

        public MainViewModel(IMvxNavigationService navigationService, IDeliveryService deliveryService)
        {
            _navigationService = navigationService;

            PackageDimmsCommand = new MvxAsyncCommand(NavigateToPackageDimms);
            ShowPackagesCommand = new MvxAsyncCommand(NavigateToShowPackages);
        }

        private Task NavigateToPackageDimms()
        {
            return _navigationService.Navigate<PackageDimmsViewModel>();
        }

        private Task NavigateToShowPackages()
        {
            return _navigationService.Navigate<ShowPackagesViewModel>();
        }
    }
}
