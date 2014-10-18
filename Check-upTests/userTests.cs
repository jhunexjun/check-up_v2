/*
 http://msdn.microsoft.com/en-us/library/ms364064(v=vs.80).aspx
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using Check_up;
using Check_up.classes;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace Check_upTests
{
    [TestClass()]
    public class userTests
    {
        //Methods with this attribute are called before the execution of each TestMethod().
        [TestInitialize]
        public void setUp()
        {
            Hashtable ht = new Hashtable();
            ht = functions.readDbConfigFile();
            Assert.IsNotNull(ht);
            Assert.IsTrue(functions.dropAndCreateDatabase(ht));
            
            // we have to select db againt because it was dropped above.
            vars.MySqlConnection.ChangeDatabase(ht["database"].ToString());

            //we want to make sure we don't drop live database.
            Assert.AreEqual("check-up_pmorcilladev", vars.MySqlConnection.Database.ToString());
            Assert.IsTrue(vars.MySqlConnection.Ping());
            Assert.IsTrue(functions.reloadDatabase());
        }

        [TestMethod()]
        public void addUser()
        {
            string pw = "password" + vars.staticSalt;
            string salt = BCrypt.GenerateSalt();
            string hash = BCrypt.HashPassword(pw, salt);
            string c;

            Hashtable ht = new Hashtable();
            ht.Add("username", "admin1");
            ht.Add("password", hash);
            ht.Add("fName", "John");
            ht.Add("midName", "Dee");
            ht.Add("lName", "Doe");
            ht.Add("email", "john_dee_joe@gmail.com");
            ht.Add("address", "Philippines");
            ht.Add("gender", "M");

            c = (true) ? "Y" : "N";
            ht.Add("deactivated", c);
            ht.Add("picLocation", @"c:\piclocation\pic.img");
            ht.Add("role", convertRole.role("Superuser"));
            ht.Add("createdBy", 0);

            Users user = new Users();
            Assert.IsTrue(user.addUser(ht));

            DataTable dt = new DataTable();
            string sql = "select * from users";
            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual(dt.Rows[0]["username"], "admin1");
            Assert.AreEqual(dt.Rows[0]["password"], hash);
            Assert.AreEqual(dt.Rows[0]["fName"], "John");
            Assert.AreEqual(dt.Rows[0]["midName"], "Dee");
            Assert.AreEqual(dt.Rows[0]["lName"], "Doe");
            Assert.AreEqual(dt.Rows[0]["email"], "john_dee_joe@gmail.com");
            Assert.AreEqual(dt.Rows[0]["address"], "Philippines");
            Assert.AreEqual(dt.Rows[0]["gender"], "M");
            Assert.AreEqual(dt.Rows[0]["deactivated"], c);
            Assert.AreEqual(dt.Rows[0]["picLocation"], @"c:\piclocation\pic.img");
            Assert.AreEqual(dt.Rows[0]["role"], 0);
            Assert.AreEqual(dt.Rows[0]["createdBy"], 0);
        }

        [TestMethod()]
        public void updateUser()
        {
            addUser();

            string pw = "password1" + vars.staticSalt;
            string salt = BCrypt.GenerateSalt();
            string hash = BCrypt.HashPassword(pw, salt);

            Hashtable ht = new Hashtable();
            ht.Add("username", "admin1");
            ht.Add("password", hash);
            ht.Add("fName", "John2");
            ht.Add("midName", "Dee2");
            ht.Add("lName", "Doe2");
            ht.Add("email", "john_dee_joe2@gmail.com");
            ht.Add("address", "Philippines2");
            ht.Add("gender", "F");

            string c = (false) ? "Y" : "N";
            ht.Add("deactivated", c);
            ht.Add("picLocation", @"c:\piclocation\pic2.img");
            ht.Add("role", convertRole.role("User"));
            ht.Add("updatedBy", 0);

            Check_up.classes.Users user = new Users();
            Assert.IsTrue(user.updateUser(ht));

            string sql = "select * from users";
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);

            Assert.AreEqual(dt.Rows[0]["username"], "admin1");
            Assert.AreEqual(dt.Rows[0]["fName"], "John2");
        }
    }
}
