using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using MvxAutomationApp.Core.Extensions;
using MvxAutomationApp.Core.Models;

namespace MvxAutomationApp.Core.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IBlobCache _blobCache;

        public DeliveryService(IBlobCache blobCache)
        {
            _blobCache = blobCache;
        }
        public async Task<bool> PickupPackage(Package package)
        {
            if (await _blobCache.ContainsKey(package.Barcode))
                return false;

            await _blobCache.InsertObject(package.Barcode, package);
            return true;
        }

        public async Task<Package[]> TrackPackages(DateTimeOffset pickupTime) =>
            (await _blobCache.GetAllObjects<Package>()
                .Select(x => x.Where(p => p?.PickupTime != null &&
                                          p.PickupTime.Date == pickupTime.Date)))
            .ToArray();
    }
}