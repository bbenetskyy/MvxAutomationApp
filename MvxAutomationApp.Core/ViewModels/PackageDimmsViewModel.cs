using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;

namespace MvxAutomationApp.Core.ViewModels
{
    public class PackageDimmsViewModel : MvxViewModel
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IPopupService _popupService;

        private string _barcode;
        private double? _width;
        private double? _height;
        private double? _depth;

        public string Barcode
        {
            get => _barcode;
            set => SetProperty(ref _barcode, value);
        }

        public double? Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public double? Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public double? Depth
        {
            get => _depth;
            set => SetProperty(ref _depth, value);
        }

        public IMvxCommand SaveCommand { get; }

        public PackageDimmsViewModel(IDeliveryService deliveryService,
            IPopupService popupService)
        {
            _deliveryService = deliveryService;
            _popupService = popupService;

            SaveCommand = new MvxAsyncCommand(Save);
        }

        private async Task Save()
        {
            var package = new Package
            {
                Barcode = Barcode,
                Depth = Depth ?? default,
                Height = Height ?? default,
                Width = Width ?? default,
                PickupTime = DateTimeOffset.Now
            };
            try
            {
                var r = await _deliveryService.PickupPackage(package);
                if (r)
                {
                    _popupService.Show(MessageType.Success, package.ToString());
                }
                else
                {
                    _popupService.Show(MessageType.Error);
                }
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
        }
    }
}
