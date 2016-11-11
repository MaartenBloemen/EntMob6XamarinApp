using MvvmCross.Platform;
using MvvmCross.Test.Core;
using NUnit.Framework;
using SensorTagMvvm.ViewModels;
using UnitTesting;
using Moq;

namespace UnitTesting
{
    [TestFixture]
    public class ConnectedViewModelTest : MvxTest
    {
        private ConnectedViewModel _connectedViewModel;
        [Test]
        public void TestConnectedViewModel()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var viewModel = new ConnectedViewModel();
            Assert.AreEqual(0, mockNavigation.NavigateRequests.Count);
        }
    }
}