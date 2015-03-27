using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AccountTest
{
    [TestClass]
    public class AccountManipulationTest
    {
        [TestMethod]
        public void CreateAccount()
        {
            string username = "123123123";
            string password1 = "asdasdasd";
            string password2 = "asdasdasd";
            string email = "johnny@harry.com"; 
            string name = "Johnny";

            Cc.AccountHandler.CreateMainAccount(username, password1, password2, email, name);
        }

        [TestMethod]
        public void AccountExists1()
        {
            string table = "accounts";
            string field = "username";
            string value = "123123122";

            Dal.Mysql mysql = new Dal.Mysql();
            bool got = mysql.Exists(table, field, value);

            Assert.AreEqual(false, got);
        }

        [TestMethod]
        public void AccountExists2()
        {
            string table = "accounts";
            string field = "username";
            string value = "123123123";

            Dal.Mysql mysql = new Dal.Mysql();
            bool got = mysql.Exists(table, field, value);

            Assert.AreEqual(true, got);
        }
    }
}
