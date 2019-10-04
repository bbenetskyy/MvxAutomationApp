using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;

namespace MvxAutomationApp.Core.ViewModels
{
    public class ShowPackagesViewModel : MvxViewModel
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IPopupService _popupService;

        private MvxObservableCollection<Package> _packages;
        private DateTimeOffset _selectedDate;
        private bool _isLoading;

        public MvxObservableCollection<Package> Packages
        {
            get => _packages;
            set => SetProperty(ref _packages, value);
        }

        public DateTimeOffset SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public IMvxCommand PickDateCommand { get; }

        public ShowPackagesViewModel(IDeliveryService deliveryService,
            IPopupService popupService)
        {
            _deliveryService = deliveryService;
            _popupService = popupService;
            Packages = new MvxObservableCollection<Package>();
            SelectedDate = DateTimeOffset.Now;
            PickDateCommand = new MvxAsyncCommand(PickDate);
        }

        private async Task LoadPackages()
        {
            IsLoading = true;
            Packages.SwitchTo(await _deliveryService.TrackPackages(SelectedDate));
            IsLoading = false;
        }

        private async Task PickDate()
        {
            SelectedDate = await _popupService.PickDate();
            await LoadPackages();
        }

        public override Task Initialize()
        {
            return LoadPackages();
        }
    }
}
