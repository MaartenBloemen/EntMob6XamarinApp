using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using NUnit.Framework;
using SensorTagMvvm.ViewModels;
using UnitTesting;
using Moq;
using Plugin.Connectivity.Abstractions;

namespace UnitTesting
{
    [TestFixture]
    public class NoInternetViewModelTest : MvxTest
    {
        private NoInternetViewModel _noInternetViewModel;
        [Test]
        public void TestNoInternetViewModelIsConnectedFalse()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var connectivity = new Mock<IConnectivity>();
            connectivity.Setup(c => c.IsConnected).Returns(false);
            _noInternetViewModel = new NoInternetViewModel(connectivity.Object);

            _noInternetViewModel.Start();
            /*
             * 2130837511 this number is the number linked to the red square image.
             * This number can be found in Recources/Recource.Designer.cs
             */
            Assert.AreEqual(2130837511, _noInternetViewModel.IsConnected);
            Assert.IsFalse(connectivity.Object.IsConnected);
        }

        [Test]
        public void TestNoInternetViewModelIsConnectedTrue()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var connectivity = new Mock<IConnectivity>();
            connectivity.Setup(c => c.IsConnected).Returns(true);
            _noInternetViewModel = new NoInternetViewModel(connectivity.Object);

            _noInternetViewModel.Start();
            /*
             * 2130837509 this number is the number linked to the green square image.
             * This number can be found in Recources/Recource.Designer.cs
             */
            Assert.AreEqual(2130837509, _noInternetViewModel.IsConnected);
            Assert.IsTrue(connectivity.Object.IsConnected);
        }

        [Test]
        public void TestNoInternetViewModelHasInternetFalse()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var connectivity = new Mock<IConnectivity>();
            connectivity.Setup(c => c.IsConnected).Returns(true);
            connectivity.Setup(c => c.IsRemoteReachable("google.com", 80, 5000)).Returns(Task.FromResult(false));
            _noInternetViewModel = new NoInternetViewModel(connectivity.Object);

            _noInternetViewModel.Start();
            /*
             * 2130837511 this number is the number linked to the red square image.
             * This number can be found in Recources/Recource.Designer.cs
             */
            Assert.AreEqual(2130837511, _noInternetViewModel.InternetAccess);
            Assert.IsFalse(_noInternetViewModel.HasInternetAccess);
        }

        [Test]
        public void TestNoInternetViewModelHasInternetTrue()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var connectivity = new Mock<IConnectivity>();
            connectivity.Setup(c => c.IsConnected).Returns(true);
            connectivity.Setup(c => c.IsRemoteReachable("google.com", 80, 5000)).Returns(Task.FromResult(true));
            _noInternetViewModel = new NoInternetViewModel(connectivity.Object);

            _noInternetViewModel.Start();
            /*
             * 2130837509 this number is the number linked to the green square image.
             * This number can be found in Recources/Recource.Designer.cs
             */
            Assert.AreEqual(2130837509, _noInternetViewModel.InternetAccess);
            Assert.IsTrue(_noInternetViewModel.HasInternetAccess);
        }
    }
}
