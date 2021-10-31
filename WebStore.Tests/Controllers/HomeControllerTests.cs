using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Controllers;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Return_View()
        {
            var controller = new HomeController();

            var actual_result = controller.Index();

            Assert.IsType<ViewResult>(actual_result);
        }
    }
}
