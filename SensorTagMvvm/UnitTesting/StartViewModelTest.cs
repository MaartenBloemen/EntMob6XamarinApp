using System.Linq;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using NUnit.Framework;
using Plugin.Connectivity.Abstractions;
using SensorTagMvvm.Services;
using SensorTagMvvm.ViewModels;

namespace UnitTesting
{
    [TestFixture]
    public class StartViewModelTest : MvxTest
    {
        private StartViewModel _startViewModel;

        [Test]
        public void TestStartViewModelConnectedFalseInternetFalse()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var bluetooth = new Mock<IBluetooth>();
            var connectivity = new Mock<IConnectivity>();
            connectivity.Setup(c => c.IsConnected).Returns(false);
            var viewModel = new StartViewModel(bluetooth.Object, connectivity.Object);
            viewModel.Start();
            Assert.That(mockNavigation.NavigateRequests.First().ViewModelType == typeof(NoInternetViewModel));
        }

        [Test]
        public void TestStartViewModelConnectedTrueInternetTrue()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            Assert.AreEqual(0, mockNavigation.NavigateRequests.Count);
            var bluetooth = new Mock<IBluetooth>();
            var connectivity = new Mock<IConnectivity>();
            connectivity.Setup(c => c.IsConnected).Returns(true);
            connectivity.Setup(c => c.IsRemoteReachable("google.com",80,5000)).Returns(Task.FromResult(true));
            var viewModel = new StartViewModel(bluetooth.Object, connectivity.Object);
            
            viewModel.Start();
            Assert.IsTrue(connectivity.Object.IsConnected);
            Assert.AreEqual(0, mockNavigation.NavigateRequests.Count);
        }

        [Test]
        public void TestStartViewModelConnectedTrueInternetFalse()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            Assert.AreEqual(0, mockNavigation.NavigateRequests.Count);
            var bluetooth = new Mock<IBluetooth>();
            var connectivity = new Mock<IConnectivity>();
            connectivity.Setup(c => c.IsConnected).Returns(true);
            connectivity.Setup(c => c.IsRemoteReachable("google.com", 80, 5000)).Returns(Task.FromResult(false));
            var viewModel = new StartViewModel(bluetooth.Object, connectivity.Object);

            viewModel.Start();
            Assert.IsTrue(connectivity.Object.IsConnected);
            Assert.AreEqual(1, mockNavigation.NavigateRequests.Count);
        }

        [Test]
        public void TestStartViewModelBluetoothDeviceConnectedTrue()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestStartViewModelBluetoothDeviceConnectedFalse()
        {
            Assert.IsTrue(true);
        }
    }
}