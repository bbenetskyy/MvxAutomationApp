using System;
using System.Threading.Tasks;
using MvxAutomationApp.Core.Models;

namespace MvxAutomationApp.Core.Services
{
    public interface IDeliveryService
    {
        Task CleanAllDeliveries();
        Task<bool> PickupPackage(Package package);
        Task<Package[]> TrackPackages(DateTimeOffset pickupTime);
    }
}
