using Moq;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Tests;
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

namespace MvxAutomationApp.Core.Test.ViewModels
{
    [TestFixture]
    public class MainViewModelTests: MvxIoCSupportingTest
    {
        private Mock<IMvxNavigationService> _navigationMock;
        private Mock<IDeliveryService> _deliveryMock;

        [SetUp]
        public void Init()
        {
            Setup();
        }

        protected override void AdditionalSetup()
        {
            _navigationMock = new Mock<IMvxNavigationService>();

            Ioc.RegisterSingleton(_navigationMock.Object);

            _deliveryMock = new Mock<IDeliveryService>();

            Ioc.RegisterSingleton(_deliveryMock.Object);
        }

        [Test]
        public void PackageDimmsCommand_NavigateToPackageDimmsViewModel()
        {
            //Arrange
            var viewModel = Ioc.IoCConstruct<MainViewModel>();

            //Act
            viewModel.PackageDimmsCommand.Execute();

            //Assert
            _navigationMock.Verify(n => n.Navigate<PackageDimmsViewModel>(null,default), Once);
        }

        [Test]
        public void ShowPackagesCommand_NavigateToShowPackagesViewModel()
        {
            //Arrange
            var viewModel = Ioc.IoCConstruct<MainViewModel>();

            //Act
            viewModel.ShowPackagesCommand.Execute();

            //Assert
            _navigationMock.Verify(n => n.Navigate<ShowPackagesViewModel>(null, default), Once);
        }

        [Test]
        public async Task LoadTestDbCommand_SwithChecked_DatabeseClearAndFillWithTestRecords()
        {
            //Arrange
            var testRecordsCount = 20;
            var viewModel = Ioc.IoCConstruct<MainViewModel>();
            viewModel.IsTestDbChecked = true;

            //Set custom count of Test Records for Test purpose
            viewModel.GetType().GetField("_testRecordsCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(viewModel, testRecordsCount);

            //Act
            await ((MvxAsyncCommand)viewModel.LoadTestDbCommand).ExecuteAsync();

            //Assert
            _deliveryMock.Verify(d => d.CleanAllDeliveries(), Once);
            _deliveryMock.Verify(d => d.PickupPackage(It.IsAny<Package>()), Exactly(testRecordsCount));
        }

        [Test]
        public async Task LoadTestDbCommand_SwithUnChecked_DatabeseClearAndNoTestRecords()
        {
            //Arrange
            var viewModel = Ioc.IoCConstruct<MainViewModel>();
            viewModel.IsTestDbChecked = false;

            //Act
            await ((MvxAsyncCommand)viewModel.LoadTestDbCommand).ExecuteAsync();

            //Assert
            _deliveryMock.Verify(d => d.CleanAllDeliveries(), Once);
            _deliveryMock.Verify(d => d.PickupPackage(It.IsAny<Package>()), Never);
        }
    }
}
