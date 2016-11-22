using System;
using System.Collections.Generic;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using NUnit.Framework;
using SensorTagMvvm.ViewModels;
using UnitTesting;
using Moq;
using SensorTagMvvm.DAL;
using SensorTagMvvm.Domain;

namespace UnitTesting
{
    /*
     *TODO: lists like temperature list need to be lists of models not double.
     *TODO: Unit test for these lists need to written after this is done. Only one unit test done this far.
     */
    [TestFixture]
    public class ConnectedViewModelTest : MvxTest
    {
        private ConnectedViewModel _connectedViewModel;
        [Test]
        public void TestConnectedViewModelDeviceName()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var repository = new Mock<ISensorTagRepository>();
            var viewModel = new ConnectedViewModel(repository.Object) { DeviceName = "Test" };

            Assert.AreEqual("Test", viewModel.DeviceName);
        }

        [Test]
        public void TestConnectedViewModelDeviceId()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var repository = new Mock<ISensorTagRepository>();
            var viewModel = new ConnectedViewModel(repository.Object) { DeviceId = Guid.Parse("41697849-d8b1-47cc-b236-f674fb721b35") };


            Assert.AreEqual(Guid.Parse("41697849-d8b1-47cc-b236-f674fb721b35"), viewModel.DeviceId);
        }

        [Test]
        public void TestConnectedViewModelTemperatureList()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var repository = new Mock<ISensorTagRepository>();

            var viewModel = new ConnectedViewModel(repository.Object)
            {
                TemperaturesList = new List<Temperature> {
                    new Temperature() {ID = "Test_1", Measured = DateTime.Today, Value = 10},
                    new Temperature() {ID = "Test_2", Measured = DateTime.Today, Value = 20},
                    new Temperature() {ID = "Test_3", Measured = DateTime.Today, Value = 30},
                    new Temperature() {ID = "Test_4", Measured = DateTime.Today, Value = 40},
                    new Temperature() {ID = "Test_5", Measured = DateTime.Today, Value = 50}
                }
            };


            Assert.AreEqual(5, viewModel.TemperaturesList.Count);
            Assert.AreEqual(10, viewModel.TemperaturesList[0].Value);
            Assert.AreEqual("Test_3", viewModel.TemperaturesList[2].ID);
        }

        [Test]
        public void TestConnectedViewModelHumidityList()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var repository = new Mock<ISensorTagRepository>();

            var viewModel = new ConnectedViewModel(repository.Object)
            {
                HumidityList = new List<Humidity>
                {
                    new Humidity {ID = "Test_1", Measured = DateTime.Today, Percentage = 10},
                    new Humidity {ID = "Test_2", Measured = DateTime.Today, Percentage = 20},
                    new Humidity {ID = "Test_3", Measured = DateTime.Today, Percentage = 30},
                    new Humidity {ID = "Test_4", Measured = DateTime.Today, Percentage = 40},
                    new Humidity {ID = "Test_5", Measured = DateTime.Today, Percentage = 50}
                }
            };

            Assert.AreEqual(5, viewModel.HumidityList.Count);
            Assert.AreEqual(10, viewModel.HumidityList[0].Percentage);
            Assert.AreEqual("Test_3", viewModel.HumidityList[2].ID);
        }

        [Test]
        public void TestConnectedViewModelBarometerList()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var repository = new Mock<ISensorTagRepository>();

            var viewModel = new ConnectedViewModel(repository.Object)
            {
                BarometerList = new List<AirPressure>
                {
                    new AirPressure() {ID = "Test_1", Measured = DateTime.Today, Value = 10},
                    new AirPressure() {ID = "Test_2", Measured = DateTime.Today, Value = 20},
                    new AirPressure() {ID = "Test_3", Measured = DateTime.Today, Value = 30},
                    new AirPressure() {ID = "Test_4", Measured = DateTime.Today, Value = 40},
                    new AirPressure() {ID = "Test_5", Measured = DateTime.Today, Value = 50}
                }
            };

            Assert.AreEqual(5, viewModel.BarometerList.Count);
            Assert.AreEqual(10, viewModel.BarometerList[0].Value);
            Assert.AreEqual("Test_3", viewModel.BarometerList[2].ID);
        }

        [Test]
        public void TestConnectedViewModelOpticalList()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var repository = new Mock<ISensorTagRepository>();

            var viewModel = new ConnectedViewModel(repository.Object)
            {
                OpticalList = new List<Brightness>
                {
                    new Brightness() {ID = "Test_1", Measured = DateTime.Today, Value = 10},
                    new Brightness() {ID = "Test_2", Measured = DateTime.Today, Value = 20},
                    new Brightness() {ID = "Test_3", Measured = DateTime.Today, Value = 30},
                    new Brightness() {ID = "Test_4", Measured = DateTime.Today, Value = 40},
                    new Brightness() {ID = "Test_5", Measured = DateTime.Today, Value = 50}
                }
            };

            Assert.AreEqual(5, viewModel.OpticalList.Count);
            Assert.AreEqual(10, viewModel.OpticalList[0].Value);
            Assert.AreEqual("Test_3", viewModel.OpticalList[2].ID);
        }
    }
}