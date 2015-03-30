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
            string phoneNumber = "123123123";

            Cc.AccountHandler.CreateMainAccount(username, password1, password2, email, name, phoneNumber);
        }

        [TestMethod]
        public void GetAccountData()
        {
            string username = "123123123";

            List<string[]> accountData = Cc.AccountHandler.GetAccountData(username);

            string expected = "Johnny";
            string actual = accountData[0][5];

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateSubAccount()
        {
            int parentId = 1;
            string username = "test1"; 
            string password1 = "asdasdasd";
            string password2 = "asdasdasd";
            
            string name = "Johnnysubaccount";

            byte[] profileImage = new byte[1];

            List<string> actual = Cc.AccountHandler.CreateSubAccount(parentId, username, password1, password2, name, profileImage);

            List<string> expected = new List<string>();
            expected.Add("success;Account registered.");

            Dal.Mysql mysql = new Dal.Mysql();
            mysql.Query(String.Format("DELETE FROM accounts WHERE username='{0}'", username));

            CollectionAssert.AreEquivalent(expected, actual);
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
            string phoneNumber = "123123123";


            List<string> actual = Cc.AccountHandler.CreateMainAccount(username, password1, password2, email, name, phoneNumber);

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
            string phoneNumber = "123123123";

            List<string> actual = Cc.AccountHandler.CreateMainAccount(username, password1, password2, email, name, phoneNumber);

            List<string> expected = new List<string>();
            expected.Add("error;Account must be between 4 and 25 characters.");

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CreateAccountNotMachingPasswords()
        {
            string username = "123123120";
            string password1 = "asdasdasd";
            string password2 = "asdasdasd2";
            string email = "johnny@harry.com";
            string name = "Johnny";
            string phoneNumber = "123123123";

            List<string> actual = Cc.AccountHandler.CreateMainAccount(username, password1, password2, email, name, phoneNumber);

            List<string> expected = new List<string>();
            expected.Add("error;Passwords do not match.");

            CollectionAssert.AreEquivalent(expected, actual);

            Dal.Mysql mysql = new Dal.Mysql();
            mysql.Query(String.Format("DELETE FROM accounts WHERE username={0}", username));
        }
    }
}
