using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;
using NUnit.Framework;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Connectivity.Abstractions;

namespace UnitTesting
{
    public class MvxTest : MvxIoCSupportingTest
    {
        protected MockMvxViewDispatcher CreateMockNavigation()
        {
            var dispatcher = new MockMvxViewDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
            Ioc.RegisterSingleton<IMvxViewDispatcher>(dispatcher);
            return dispatcher;
        }

        protected override void AdditionalSetup()
        {
            var adapterservice = new Mock<IAdapter>();
            Ioc.RegisterSingleton<IAdapter>(adapterservice.Object);
            var connectivity = new Mock<IConnectivity>();
            connectivity.Setup(e => e.IsConnected).Returns(false);
            Ioc.RegisterSingleton<IConnectivity>(connectivity.Object);
        }
    }
}
