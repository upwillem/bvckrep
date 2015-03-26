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
    }
}
