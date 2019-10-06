using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;
using MvxAutomationApp.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxAutomationApp.Core.Test.Stubs
{
    public class PackageDimmsViewModelStub : PackageDimmsViewModel
    {
        public PackageDimmsViewModelStub(IDeliveryService deliveryService, IPopupService popupService) : base(deliveryService, popupService)
        {
        }

        public Package GetPackage() => base.GetPackage();
        public bool Validate() => base.Validate();
    }
}
