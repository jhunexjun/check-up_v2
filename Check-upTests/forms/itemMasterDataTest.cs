using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Check_upTests;

namespace Check_upTests.forms
{
    [TestClass]
    public class ItemMasterDataTest
    {
        [TestInitialize]
        public void setUp()
        {
            userTests user = new userTests();
            user.setUp();
            user.addUser();
        }

        [TestMethod]
        public void addItem()
        {
            
        }

        [TestMethod]
        public void updateItem()
        {

        }
    }
}
