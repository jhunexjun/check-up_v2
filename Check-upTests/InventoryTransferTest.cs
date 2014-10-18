using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Check_upTests
{
    [TestClass]
    public class InventoryTransferTest
    {
        [TestInitialize]
        public void setUp()
        {
            userTests user = new userTests();
            user.setUp();
            user.addUser();
        }

        [TestMethod]
        public void addInventoryTransfer()
        {
            Assert.IsTrue(functions.createDefaultRecordsForTheTests());
        }
    }
}
