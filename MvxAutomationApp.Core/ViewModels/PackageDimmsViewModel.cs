using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvvmValidation;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;
using System.Windows.Input;
using MvxAutomationApp.Core.Common;
using MvxAutomationApp.Core.Extensions;

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
        private ObservableDictionary<string, string> _errors;

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

        public ObservableDictionary<string, string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        public IMvxCommand SaveCommand { get; }
        public IMvxCommand ResetCommand { get; }

        public PackageDimmsViewModel(IDeliveryService deliveryService,
            IPopupService popupService)
        {
            _deliveryService = deliveryService;
            _popupService = popupService;

            SaveCommand = new MvxAsyncCommand(Save);
            ResetCommand = new MvxCommand(Reset);
        }

        private async Task Save()
        {
            if (!Validate())
            {
                _popupService.Show(MessageType.Error, "Validation didn't pass!");
                return;
            }

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
                var saved = await _deliveryService.PickupPackage(package);
                if (saved)
                {
                    _popupService.Show(MessageType.Success, $"{package} saved");
                }
                else
                {
                    _popupService.Show(MessageType.Error, $"{package.Barcode} already saved");
                }
            }
            catch (Exception e)
            {
                _popupService.Show(MessageType.Error, e.Message);
            }
        }

        private bool Validate()
        {
            var messagePostfix = "is required.";
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => Barcode, $"{nameof(Barcode)} {messagePostfix}");
            validator.AddRequiredRule(() => Width, $"{nameof(Width)} {messagePostfix}");
            validator.AddRequiredRule(() => Height, $"{nameof(Height)} {messagePostfix}");
            validator.AddRequiredRule(() => Depth, $"{nameof(Depth)} {messagePostfix}");

            var result = validator.ValidateAll();
            Errors = result.AsObservableDictionary();
            return result.IsValid;
        }

        private void Reset()
        {
            Barcode = null;
            Depth = null;
            Height = null;
            Width = null;
            _popupService.Show(MessageType.Success, "All data is cleared");
        }
    }
}
