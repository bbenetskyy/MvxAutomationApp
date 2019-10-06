using Moq;
using MvvmCross.Commands;
using MvvmCross.Tests;
using MvxAutomationApp.Core.Models;
using MvxAutomationApp.Core.Resources;
using MvxAutomationApp.Core.Services;
using MvxAutomationApp.Core.ViewModels;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Moq.Times;
using static MvxAutomationApp.Core.Models.MessageType;
using static MvxAutomationApp.Core.Test.TestData.PackagesTestData;

namespace MvxAutomationApp.Core.Test.ViewModels
{
    [TestFixture]
    public class PackageDimmsViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IDeliveryService> _deliveryMock;
        private Mock<IPopupService> _popupMock;

        [SetUp]
        public void Init()
        {
            Setup();
        }

        protected override void AdditionalSetup()
        {
            _popupMock = new Mock<IPopupService>();
            Ioc.RegisterSingleton(_popupMock.Object);

            _deliveryMock = new Mock<IDeliveryService>();
            Ioc.RegisterSingleton(_deliveryMock.Object);
        }

        [Test]
        public void ResetCommand_ResetAllFieldsToNull()
        {
            //Arrange 
            var viewModel = Ioc.IoCConstruct<PackageDimmsViewModel>();
            viewModel.Barcode = "Barcode";
            viewModel.Depth = 1.0;
            viewModel.Width = 0.1;
            viewModel.Height = 0;

            //Act
            viewModel.ResetCommand.Execute();

            //Assert
            viewModel.Barcode.ShouldBeNull();
            viewModel.Depth.ShouldBeNull();
            viewModel.Width.ShouldBeNull();
            viewModel.Height.ShouldBeNull();
            _popupMock.Verify(p => p.Show(Success, AppResources.DataCleared), Once);
        }

        [Test]
        public async Task SaveCommand_EmptyFields_ValidationFailedPopupWithMessageShown()
        {
            //Arrange
            var viewModel = Ioc.IoCConstruct<PackageDimmsViewModel>();
            var errorKeys = new[] { "Barcode", "Depth", "Width", "Height" };

            //Act
            await ((MvxAsyncCommand)viewModel.SaveCommand).ExecuteAsync();

            //Assert
            _popupMock.Verify(p => p.Show(Error, AppResources.ValidationFailed), Once);
            viewModel.Errors.ShouldSatisfyAllConditions(
                 () => viewModel.Errors.ContainsKey("Barcode").ShouldBeTrue(),
                 () => viewModel.Errors.ContainsKey("Depth").ShouldBeTrue(),
                 () => viewModel.Errors.ContainsKey("Width").ShouldBeTrue(),
                 () => viewModel.Errors.ContainsKey("Height").ShouldBeTrue()
                 );
            foreach (var key in errorKeys)
            {
                viewModel.Errors[key].ShouldBe(string.Format(AppResources.FieldRequired, key));
            }
        }

        [Test]
        public async Task SaveCommand_ValidationPassed_PackageAddedAndPopupShown()
        {
            //Arrange
            _deliveryMock.Setup(d => d.PickupPackage(It.IsAny<Package>()))
                .ReturnsAsync(true);
            var viewModel = Ioc.IoCConstruct<PackageDimmsViewModel>();

            viewModel.Barcode = NewPackageName;
            viewModel.Depth = 1.0;
            viewModel.Width = 1.0;
            viewModel.Height = 1.0;

            //Act
            await ((MvxAsyncCommand)viewModel.SaveCommand).ExecuteAsync();

            //Assert
            _popupMock.Verify(p => p.Show(Success, string.Format(AppResources.PakcageSaved, $"Dimm (1 x 1 x 1) {NewPackageName}")), Once);
            _deliveryMock.Verify(d => d.PickupPackage(It.IsAny<Package>()), Once);
            viewModel.Errors.Keys.Count.ShouldBe(0);
        }

        [Test]
        public async Task SaveCommand_ExistedPackage_ErrorPopupShown()
        {
            //Arrange
            _deliveryMock.Setup(d => d.PickupPackage(It.IsAny<Package>()))
                .ReturnsAsync(false);
            var viewModel = Ioc.IoCConstruct<PackageDimmsViewModel>();

            viewModel.Barcode = ExistedPackageName;
            viewModel.Depth = 1.0;
            viewModel.Width = 1.0;
            viewModel.Height = 1.0;

            //Act
            await ((MvxAsyncCommand)viewModel.SaveCommand).ExecuteAsync();

            //Assert
            _popupMock.Verify(p => p.Show(Error, string.Format(AppResources.PackageAlreadySaved, ExistedPackageName)), Once);
            _deliveryMock.Verify(d => d.PickupPackage(It.IsAny<Package>()), Once);
            viewModel.Errors.Keys.Count.ShouldBe(0);
        }

        [Test]
        public async Task SaveCommand_DeliveryServiceThrowException_ErrorPopupShown()
        {
            //Arrange
            _deliveryMock.Setup(d => d.PickupPackage(It.IsAny<Package>()))
                .Throws(new Exception("message"));
            var viewModel = Ioc.IoCConstruct<PackageDimmsViewModel>();

            viewModel.Barcode = ExistedPackageName;
            viewModel.Depth = 1.0;
            viewModel.Width = 1.0;
            viewModel.Height = 1.0;

            //Act
            await ((MvxAsyncCommand)viewModel.SaveCommand).ExecuteAsync();

            //Assert
            _popupMock.Verify(p => p.Show(Error, "message"), Once);
            _deliveryMock.Verify(d => d.PickupPackage(It.IsAny<Package>()), Once);
            viewModel.Errors.Keys.Count.ShouldBe(0);
        }

        [TestCase("Barcode", 12, 0.2, 3.4)]
        [TestCase(null, 12, 0.2, 3.4)]
        [TestCase("Barcode", null, 0.2, 3.4)]
        [TestCase("Barcode", 12, null, 3.4)]
        [TestCase("Barcode", 12, 0.2, null)]
        public void GetPackage_NewPackageWithDataAndPickupTimeReturned(string barcode, double? depth, double? width, double? height)
        {
            //Arrange
            var viewModel = Ioc.IoCConstruct<PackageDimmsViewModel>();

            viewModel.Barcode = barcode;
            viewModel.Depth = depth;
            viewModel.Width = width;
            viewModel.Height = height;

            //Act
            var package = viewModel.GetPackage();

            //Assert
            package.ShouldNotBeNull();
            package.ShouldSatisfyAllConditions(
                () => package.Barcode.ShouldBe(barcode),
                () => package.Depth.ShouldBe(depth ?? default),
                () => package.Width.ShouldBe(width ?? default),
                () => package.Height.ShouldBe(height ?? default),
                () => package.PickupTime.Date.ShouldBe(DateTimeOffset.Now.Date)
            );
        }

        [TestCase("Barcode", 12, 0.2, 3.4, true)]
        [TestCase(null, 12, 0.2, 3.4,false)]
        [TestCase("Barcode", null, 0.2, 3.4, false)]
        [TestCase("Barcode", 12, null, 3.4, false)]
        [TestCase("Barcode", 12, 0.2, null, false)]
        [TestCase(null, null, null, null, false)]
        public void Validate_ReturnExpectedValidationResult(string barcode, double? depth, double? width, double? height, bool expectedValidationResult)
        {
            //Arrange
            var viewModel = Ioc.IoCConstruct<PackageDimmsViewModel>();

            viewModel.Barcode = barcode;
            viewModel.Depth = depth;
            viewModel.Width = width;
            viewModel.Height = height;

            //Act
            var result = viewModel.Validate();

            //Assert
            result.ShouldBe(expectedValidationResult);
        }
    }
}
