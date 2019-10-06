using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvvmValidation;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;
using MvxAutomationApp.Core.Common;
using MvxAutomationApp.Core.Extensions;
using MvxAutomationApp.Core.Resources;

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
        private bool _isLoading;
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

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
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
                _popupService.Show(MessageType.Error, AppResources.ValidationFailed);
                return;
            }
            try
            {
                IsLoading = true;
                var package = GetPackage();
                var saved = await _deliveryService.PickupPackage(package);
                if (saved)
                {
                    _popupService.Show(MessageType.Success, string.Format(AppResources.PakcageSaved, $"{package} {package.Barcode}"));
                }
                else
                {
                    _popupService.Show(MessageType.Error, string.Format(AppResources.PackageAlreadySaved, package.Barcode));
                }
            }
            catch (Exception e)
            {
                _popupService.Show(MessageType.Error, e.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected Package GetPackage()
        {
            return new Package
            {
                Barcode = Barcode,
                Depth = Depth ?? default,
                Height = Height ?? default,
                Width = Width ?? default,
                PickupTime = DateTimeOffset.Now
            };
        }

        protected bool Validate()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => Barcode, string.Format(AppResources.FieldRequired, nameof(Barcode)));
            validator.AddRequiredRule(() => Width, string.Format(AppResources.FieldRequired, nameof(Width)));
            validator.AddRequiredRule(() => Height,string.Format(AppResources.FieldRequired, nameof(Height)));
            validator.AddRequiredRule(() => Depth, string.Format(AppResources.FieldRequired, nameof(Depth)));

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
            _popupService.Show(MessageType.Success, AppResources.DataCleared);
        }
    }
}
