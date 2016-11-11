using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using NUnit.Framework;
using Plugin.BLE.Abstractions.Contracts;
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
        public void TestStartViewModelBluetoothDeviceConnectedFalse()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var bluetooth = new Mock<IBluetooth>();
            var connectivity = new Mock<IConnectivity>();
            connectivity.Setup(c => c.IsConnected).Returns(true);
            connectivity.Setup(c => c.IsRemoteReachable("google.com", 80, 5000)).Returns(Task.FromResult(true));
            var device = new Mock<IDevice>();
            var adapter = new Mock<IAdapter>();
            adapter.Setup(a => a.ConnectToDeviceAsync(device.Object, false, CancellationToken.None)).Returns(Task.FromResult(false));
            var viewModel = new StartViewModel(bluetooth.Object, connectivity.Object);

            viewModel.Start();
            Assert.AreEqual(0, mockNavigation.NavigateRequests.Count);
        }
    }
}