using Akavache;
using Moq;
using MvvmCross.Tests;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Services;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using MvxAutomationApp.Core.Test.TestData;

using static Moq.Times;
using static MvxAutomationApp.Core.Test.TestData.PackagesTestData;

namespace MvxAutomationApp.Core.Test.Services
{
    [TestFixture]
    public class DeliveryServiceTests : MvxIoCSupportingTest
    {
        private Mock<IObjectBlobCache> _blobMock;

        [SetUp]
        public void Init()
        {
            Setup();
        }

        protected override void AdditionalSetup()
        {
            var byteObservable = Observable.Return(new byte[0]);
            var exceptionObservable = Observable.Throw<byte[]>(new Exception());
            var unitObservable = Observable.Return(Unit.Default);
            var allObjectsObservable = Observable.Return(Packages);

            _blobMock = new Mock<IObjectBlobCache>();
            _blobMock.Setup(b => b.InvalidateAll())
                .Returns(unitObservable);
            _blobMock.Setup(b => b.Vacuum())
                .Returns(unitObservable);
            _blobMock.Setup(b => b.Get(It.Is<string>(s => s == ExistedPackageName)))
                .Returns(byteObservable);
            _blobMock.Setup(b => b.Get(It.Is<string>(s => s != ExistedPackageName)))
                .Returns(exceptionObservable);
            _blobMock.Setup(InsertAnyObject)
                .Returns(unitObservable);
            _blobMock.Setup(b => b.GetAllObjects<Package>())
                .Returns(allObjectsObservable);
            Ioc.RegisterSingleton<IBlobCache>(_blobMock.Object);
        }

        [Test]
        public async Task CleanAllDeliveries_InvalidateAllAndVacuum()
        {
            //Arrange
            var deliveryService = Ioc.IoCConstruct<DeliveryService>();

            //Act
            await deliveryService.CleanAllDeliveries();

            //Assert    
            _blobMock.Verify(b => b.InvalidateAll(), Once);
            _blobMock.Verify(b => b.Vacuum(), Once);
        }

        [Test]
        public async Task PickupPackage_ForNewPackage_TrueReturned()
        {
            //Arrange
            var package = new Package { Barcode = NewPackageName };
            var deliveryService = Ioc.IoCConstruct<DeliveryService>();

            //Act
            var result = await deliveryService.PickupPackage(package);

            //Assert    
            result.ShouldBeTrue();
            _blobMock.Verify(b => b.Get(It.IsAny<string>()), Once);
            _blobMock.Verify(InsertAnyObject, Once);
        }

        [Test]
        public async Task PickupPackage_ForExistedPackage_FalseReturned()
        {
            //Arrange
            var package = new Package { Barcode = ExistedPackageName };
            var deliveryService = Ioc.IoCConstruct<DeliveryService>();

            //Act
            var result = await deliveryService.PickupPackage(package);

            //Assert    
            result.ShouldBeFalse();
            _blobMock.Verify(b => b.Get(It.IsAny<string>()), Once);
            _blobMock.Verify(InsertAnyObject, Never);
        }

        [Theory]
        [TestCaseSource(typeof(PackagesTestData), nameof(PickupTimeForTrackPackages))]
        public async Task TrackPackages_SearchByDate_ExpectedPackagesReturned(DateTimeOffset pickupTime)
        {
            //Arracnge
            var deliveryService = Ioc.IoCConstruct<DeliveryService>();
            var expectedPackages = Packages.Where(p => p.PickupTime == pickupTime);

            //Act
            var packages = await deliveryService.TrackPackages(pickupTime);

            //Assert
            packages.SequenceEqual(expectedPackages);
            _blobMock.Verify(b => b.GetAllObjects<Package>(), Times.Once);
        }

        private Expression<Func<IObjectBlobCache, IObservable<Unit>>> InsertAnyObject =
            b => b.InsertObject(It.IsAny<string>(), It.IsAny<Package>(), default);
    }
}
