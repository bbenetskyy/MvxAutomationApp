using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MvxAutomationApp.Core.Models;
using ReactiveUI;

namespace MvxAutomationApp.Core.Services
{
    public interface IDeliveryService
    {
        Task<bool> PickupPackage(Package package);
        Task<Package[]> TrackPackages(DateTimeOffset pickupTime);
    }
}
