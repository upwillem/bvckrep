using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
            string value = "123123123";

            Dal.Mysql mysql = new Dal.Mysql();
            bool got = mysql.Exists(table, field, value);

            mysql.Query(String.Format("DELETE FROM accounts WHERE username={0}", value));

            Assert.AreEqual(true, got);
        }

        [TestMethod]
        public void CreateAccountShortUsername()
        {
            string username = "1";
            string password1 = "asdasdasd";
            string password2 = "asdasdasd";
            string email = "johnny@harry.com";
            string name = "Johnny";

            List<string> actual = Cc.AccountHandler.CreateMainAccount(username, password1, password2, email, name);

            List<string> expected = new List<string>();
            expected.Add("error;Account must be between 4 and 25 characters.");

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void AccountExists2()
        {
            string table = "accounts";
            string field = "username";
            string value = "1";

            Dal.Mysql mysql = new Dal.Mysql();
            bool got = mysql.Exists(table, field, value);

            Assert.AreEqual(false, got);
        }

        [TestMethod]
        public void CreateAccountLongUsername()
        {
            string username = "ASNFoiwebsgniuoewbtgiuweo1";
            string password1 = "asdasdasd";
            string password2 = "asdasdasd";
            string email = "johnny@harry.com";
            string name = "Johnny";

            List<string> actual = Cc.AccountHandler.CreateMainAccount(username, password1, password2, email, name);

            List<string> expected = new List<string>();
            expected.Add("error;Account must be between 4 and 25 characters.");

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CreateAccountNotMachingPasswords()
        {
            string username = "123123123";
            string password1 = "asdasdasd";
            string password2 = "asdasdasd2";
            string email = "johnny@harry.com";
            string name = "Johnny";

            List<string> actual = Cc.AccountHandler.CreateMainAccount(username, password1, password2, email, name);

            List<string> expected = new List<string>();
            expected.Add("error;Passwords do not match.");

            CollectionAssert.AreEquivalent(expected, actual);

            Dal.Mysql mysql = new Dal.Mysql();
            mysql.Query(String.Format("DELETE FROM accounts WHERE username={0}", username));
        }
    }
}
