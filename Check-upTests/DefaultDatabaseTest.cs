using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using Check_up;

namespace Check_upTests
{
    [TestClass]
    public class DefaultDatabaseTest
    {
        [TestMethod]
        public void testDefaultDatabase()
        {
            Hashtable ht = new Hashtable();
            ht = functions.readDbConfigFile();
            Assert.IsNotNull(ht);
            Assert.IsTrue(functions.dropAndCreateDatabase(ht));

            // we have to select db again because it was dropped above.
            vars.MySqlConnection.ChangeDatabase(ht["database"].ToString());

            //we want to make sure we don't drop live database.
            Assert.AreEqual("check-up_pmorcilladev", vars.MySqlConnection.Database.ToString());
            Assert.IsTrue(vars.MySqlConnection.Ping());
            Assert.IsTrue(functions.reloadDatabase());

            // before we can create default records first thing is the user because of the constraint.
            userTests user = new userTests();
            user.setUp();
            user.testAddUser();
            Assert.IsTrue(functions.createDefaultRecordsForTheTestsForBranch());

            //continue with the asserts here...

        }
    }
}
