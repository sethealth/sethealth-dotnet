using System;
using System.Threading.Tasks;
using Sethealth;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace SethealthTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public async Task TestValid()
        {
            var client = new Client();
            GetTokenResponse response = await client.GetToken();
            Assert.IsTrue(response.token.Length > 10);
        }

        [TestMethod]
        public async Task TestValidOptions()
        {
            var client = new Client();
            GetTokenOptions opts = new GetTokenOptions("user", 1000, false);
            GetTokenResponse response = await client.GetToken(opts);
            Assert.IsTrue(response.token.Length > 10);
        }

        [TestMethod]
        public async Task TestUnvalid()
        {
            var client = new Client("key", "secret");
            await Assert.ThrowsExceptionAsync<AuthException>(async () => await client.GetToken());
        }

        [TestMethod]
        public void TestMissingKey()
        {
            Assert.ThrowsException<InputException>(() => new Client("", "secret"));
        }

        [TestMethod]
        public void TestMissingSecret()
        {
            Assert.ThrowsException<InputException>(() => new Client("key", null));

        }
    }
}
