using bigbus.checkout.data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Infrastructure;

namespace bigbus.checkout.Tests.UnitTests.ServiceTests
{
    [TestClass]
    public class CheckoutServiceTest
    {
        [TestMethod]
        public void CanSendBookingToEcr()
        {
            var moq = new Mock<ICheckoutService>();

            moq.Setup(x => x.GetFullOrder(It.IsAny<string>())).Returns(new Order());
        }

       
        
    }
}
