using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;

namespace MvxAutomationApp.Core.ViewModels
{
    public class ShowPackagesViewModel : MvxViewModel
    {
        private readonly IDeliveryService _deliveryService;
        private MvxObservableCollection<Package> _packages;

        public MvxObservableCollection<Package> Packages
        {
            get => _packages;
            set => SetProperty(ref _packages, value);
        }

        public ShowPackagesViewModel(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
            Packages = new MvxObservableCollection<Package>();
        }

        public override async Task Initialize()
        {
            Packages.SwitchTo(await _deliveryService.TrackPackages(DateTimeOffset.Now));
        }
    }
}
