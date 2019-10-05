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

namespace MvxAutomationApp.Core.Test.Services
{
    [TestFixture]
    public class DeliveryServiceTests : MvxIoCSupportingTest
    {
        private const string _newPackageName = "barcode";
        private const string _existedPackageName = "default";
        private Mock<IBlobCache> _blobMock;

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

            _blobMock = new Mock<IBlobCache>();
            _blobMock.Setup(b => b.InvalidateAll())
                .Returns(unitObservable);
            _blobMock.Setup(b => b.Vacuum())
                .Returns(unitObservable);
            _blobMock.Setup(b => b.Get(It.Is<string>(s => s == _existedPackageName)))
                .Returns(byteObservable);
            _blobMock.Setup(b => b.Get(It.Is<string>(s => s != _existedPackageName)))
                .Returns(exceptionObservable);
            _blobMock.Setup(InsertAnyObject)
                .Returns(unitObservable);
            Ioc.RegisterSingleton(_blobMock.Object);
        }

        private int IObservable<T>(IObservable<T> byteObservable)
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task CleanAllDeliveries_InvalidateAllAndVacuum()
        {
            //Arrange
            var deliveryService = Ioc.IoCConstruct<DeliveryService>();

            //Act
            await deliveryService.CleanAllDeliveries();

            //Assert    
            _blobMock.Verify(b => b.InvalidateAll(), Times.Once);
            _blobMock.Verify(b => b.Vacuum(), Times.Once);
        }

        [Test]
        public async Task PickupPackage_ForNewPackage_TrueReturned()
        {
            //Arrange
            var package = new Package { Barcode = _newPackageName };
            var deliveryService = Ioc.IoCConstruct<DeliveryService>();

            //Act
            var result = await deliveryService.PickupPackage(package);

            //Assert    
            result.ShouldBeTrue();
            _blobMock.Verify(b => b.Get(It.IsAny<string>()), Times.Once);
            _blobMock.Verify(InsertAnyObject, Times.Once);
        }

        [Test]
        public async Task PickupPackage_ForExistedPackage_FalseReturned()
        {
            //Arrange
            var package = new Package { Barcode = _existedPackageName };
            var deliveryService = Ioc.IoCConstruct<DeliveryService>();

            //Act
            var result = await deliveryService.PickupPackage(package);

            //Assert    
            result.ShouldBeFalse();
            _blobMock.Verify(b => b.Get(It.IsAny<string>()), Times.Once);
            _blobMock.Verify(InsertAnyObject, Times.Never);
        }

        private Expression<Func<IBlobCache, IObservable<Unit>>> InsertAnyObject =
            b => b.Insert(It.IsAny<string>(), It.IsAny<byte[]>(), default);
    }
}
