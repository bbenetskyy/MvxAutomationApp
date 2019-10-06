using Moq;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Tests;
using MvvmCross.Views;
using MvxAutomationApp.Core.Models;
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
using static MvxAutomationApp.Core.Test.TestData.PackagesTestData;

namespace MvxAutomationApp.Core.Test.ViewModels
{
    [TestFixture]
    public class ShowPackagesViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IMvxMainThreadAsyncDispatcher> _dispatcherMock;
        private Mock<IDeliveryService> _deliveryMock;
        private Mock<IPopupService> _popupMock;

        [SetUp]
        public void Init()
        {
            Setup();
        }

        protected override void AdditionalSetup()
        {
            _dispatcherMock = new Mock<IMvxMainThreadAsyncDispatcher>();
            Ioc.RegisterSingleton(_dispatcherMock.Object);

            _popupMock = new Mock<IPopupService>();
            Ioc.RegisterSingleton(_popupMock.Object);

            _deliveryMock = new Mock<IDeliveryService>();
            Ioc.RegisterSingleton(_deliveryMock.Object);
        }

        [Test]
        public void PickDateCommand_PickDateAndLoadPackages()
        {
            //Arrange
            _deliveryMock.Setup(d => d.TrackPackages(It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new Package[0]);
            var viewModel = Ioc.IoCConstruct<ShowPackagesViewModel>();

            //Act
            viewModel.PickDateCommand.Execute();

            //Assert
            _popupMock.Verify(p => p.PickDate(), Once);
            _deliveryMock.Verify(d => d.TrackPackages(It.IsAny<DateTimeOffset>()), Once);
        }

        [Test]
        public void PickDateCommand_SelectedDateGetValueFromPopupService()
        {
            //Arrange
            _popupMock.Setup(p => p.PickDate())
                .ReturnsAsync(DateTimeOffset.Now.AddDays(2));
            var viewModel = Ioc.IoCConstruct<ShowPackagesViewModel>();

            //Act
            viewModel.SelectedDate.Date.ShouldBe(DateTimeOffset.Now.Date);
            viewModel.PickDateCommand.Execute();

            //Assert
            viewModel.SelectedDate.Date.ShouldBe(DateTimeOffset.Now.AddDays(2).Date);
            _popupMock.Verify(p => p.PickDate(), Once);
        }

        [Test]
        public async Task LoadPackages_TrackPackagesToObservablePackagesCollection()
        {
            //Arrange
            _deliveryMock.Setup(d => d.TrackPackages(It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(Packages);
            var viewModel = Ioc.IoCConstruct<ShowPackagesViewModel>();

            //Act
            await viewModel.LoadPackages();

            //Assert
            _deliveryMock.Verify(p => p.TrackPackages(It.IsAny<DateTimeOffset>()), Once);
            viewModel.Packages.ShouldBe(Packages);
        }
    }
}
