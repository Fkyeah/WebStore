using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WebStore.Controllers;
using WebStore.Interfaces.TestAPI;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTests
    {
        public ValuesControllerTests()
        {
            valuesServiceMock = new Mock<IValueClient>();
            controller = new ValuesController(valuesServiceMock.Object);
        }

        ValuesController controller;

        Mock<IValueClient> valuesServiceMock;

        [TestMethod]
        public void Index_Return_View()
        {
            var actualResult = controller.Index();

            Assert.IsType<ViewResult>(actualResult);
        }

        [TestMethod]    
        public void Index_Return_Array()
        {
            string[] expectedData = Enumerable
                .Range(1, 10)
                .Select(i => $"Value - {i}")
                .ToArray();

            valuesServiceMock
            .Setup(service => service.GetAll())
                .Returns(expectedData);

            var actualResult = controller.Index();
            var actualViewResult = Assert.IsType<ViewResult>(actualResult);
            var actualModelData = Assert.IsAssignableFrom<string[]>(actualViewResult.Model);

            for(int i = 0; i < expectedData.Length; i++)
            {
                Assert.Equal(expectedData[i], actualModelData[i]);
            }

            valuesServiceMock.Verify(service => service.GetAll());
            valuesServiceMock.VerifyNoOtherCalls();
        }
    }
}
