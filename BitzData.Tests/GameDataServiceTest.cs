using BitzData.Models.GameData;
using BitzData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitzData.Tests
{
    [TestClass]
    class GameDataServiceTest
    {
        private static GameDataService testTarget;

        [ClassInitialize]
        public static void InitializeGameDataService(TestContext ctx)
        {
            testTarget = GameDataService.GetInstance();
        }



        [TestMethod]
        public async Task TestUpsertPlayerInfo()
        {
            var playerID = Guid.NewGuid();
            var player = new PlayerInfo
            {
                PlayerId = playerID,
                UserName = "Test",
                Experience = 1000,
                ProfilePictureId = null
            };

            await testTarget.UpdatePlayerInfo(player);

            var fetched = await testTarget.GetPlayerInfoAsync(playerID);
            Assert.IsNotNull(fetched);
            Assert.AreEqual("TestUser", fetched.UserName);
            Assert.AreEqual(1000, fetched.Experience);
        }

    }
}
