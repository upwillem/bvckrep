﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using Dal;

namespace AccountTest
{
    /// <summary>
    /// OWNER: Roel Larik 1236830 & Ralph Lazarus 1227319
    /// Containts manipulations of accounts which are being tested.
    /// The method names indicate what the method actually does.
    /// </summary>
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
            string username = "johnny";

            List<string> accountData = Cc.AccountHandler.GetAccountData(username);

            string expected = "displayName;Johnny";
            string actual = accountData[4];

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

            Mysql.Query(String.Format("DELETE FROM accounts WHERE username='{0}'", username));

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void AddContact1()
        {
            string sender = "Hansje";
            string recipient = "Jeffke";

            bool expected = true;
            bool actual = Cc.AccountHandler.AddContact(sender, recipient);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AcceptContact1()
        {
            string sender = "Jeffke";
            string recipient = "Hansje";

            bool expected = true;
            bool actual = Cc.AccountHandler.AcceptContact(sender, recipient);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeleteContact()
        {
            string sender = "Pietje";
            string recipient = "Hansje";

            bool expected = true;
            bool actual = Cc.AccountHandler.DeleteContact(sender, recipient);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AccountExists1()
        {
            string table = "accounts";
            string field = "username";
            string value = "123123123";

            bool got = Mysql.Exists(table, field, value);

            Mysql.Query(String.Format("DELETE FROM accounts WHERE username={0}", value));

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

            bool got = Mysql.Exists(table, field, value);

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

            Mysql.Query(String.Format("DELETE FROM accounts WHERE username={0}", username));
        }

        [TestMethod]
        public void ToggleBlock()
        {
            string sender = "Hansje";
            int senderId = 3;
            string receiver = "Jeffke";
            int receiverId = 4;

            List<string[]> currentData = Mysql.Select(String.Format("SELECT * FROM contacts WHERE account_id={0} AND contact_id={1}", senderId, receiverId));

            bool expected = !Convert.ToBoolean(currentData[0][3]);

            Cc.AccountHandler.ToggleBlock(sender, receiver);

            List<string[]> actualData = Mysql.Select(String.Format("SELECT * FROM contacts WHERE account_id={0} AND contact_id={1}", senderId, receiverId));
            bool actual = Convert.ToBoolean(actualData[0][3]);

            Assert.AreEqual(expected, actual);
        }
    }
}
