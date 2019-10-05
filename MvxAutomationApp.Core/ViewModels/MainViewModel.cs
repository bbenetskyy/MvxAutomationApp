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
        private readonly IDeliveryService _deliveryService;

        private bool _isTestDbChecked;
        private bool _isLoading;

        public bool IsTestDbChecked
        {
            get => _isTestDbChecked;
            set => SetProperty(ref _isTestDbChecked, value);
        }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public IMvxCommand PackageDimmsCommand { get; }
        public IMvxCommand ShowPackagesCommand { get; }
        public IMvxCommand LoadTestDbCommand { get; }

        public MainViewModel(IMvxNavigationService navigationService, IDeliveryService deliveryService)
        {
            _navigationService = navigationService;
            _deliveryService = deliveryService;

            PackageDimmsCommand = new MvxAsyncCommand(NavigateToPackageDimms);
            ShowPackagesCommand = new MvxAsyncCommand(NavigateToShowPackages);
            LoadTestDbCommand = new MvxAsyncCommand(LoadTestDb);
        }

        private async Task LoadTestDb()
        {
            IsLoading = true;
            await _deliveryService.CleanAllDeliveries();

            if (IsTestDbChecked)
            {
                var rnd = new Random();
                for (int i = 0; i < 2000; i++)
                {
                    await _deliveryService.PickupPackage(new Package
                    {
                        Barcode = rnd.Next(0, 1000000).ToString(),
                        Depth = rnd.NextDouble(),
                        Width = rnd.NextDouble(),
                        Height = rnd.NextDouble(),
                        PickupTime = DateTimeOffset.Now
                    });
                }
            }
            IsLoading = false;
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
