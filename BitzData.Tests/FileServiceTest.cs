using BitzData.Services;


namespace BitzData.Tests
{

    [TestClass]
    public sealed class FileServiceTest
    {
        private static BitzFileService testTarget;

        [ClassInitialize]
        public static void InitializeDataService(TestContext ctx)
        {
            testTarget = BitzFileService.GetInstance();
        }


        [TestMethod("Files should be download and be cached correctly with correct metadata.")]
        public async Task GetObjectShouldHaveCache()
        {
            Assert.IsNotNull(await testTarget.GetStorageObject(Constants.TEST_FILE_OBJECT_ID));
            Assert.IsTrue(File.Exists(
                Path.Join(BitzData.Constants.CACHE_METADATA, BitzData.Tests.Constants.TEST_FILE_OBJECT_ID + ".bmeta")
                ));
        }

        [TestMethod]
        public async Task TestDataShouldDownloadToCorrectDirectory()
        {
            await testTarget.GetStorageObject(Constants.TEST_FILE_OBJECT_ID);
            Assert.IsTrue(
                File.Exists(Path.Combine(BitzData.Constants.APPLICATION_DATA, BitzData.Tests.Constants.TEST_FILE_OBJECT_ID + BitzData.Tests.Constants.TEST_FILE_EXTENSION))
                );
        }

        [TestMethod]
        public async Task CacheShouldBeInvalidated()
        {

            var cacheRes = new StringWriter();
            var stdOut = Console.Out;
            Console.SetOut(cacheRes);
            var storageObject = await BitzFileService.GetInstance().GetStorageObject(Constants.TEST_FILE_OBJECT_ID); ;



            Assert.IsNotNull(storageObject);

            await BitzFileService.GetInstance().OverrideFile(storageObject, Constants.TEST_FILE_1_PATH);
            await Task.Delay(5000);
            await BitzFileService.GetInstance().GetStorageObject(storageObject.ObjectId);
            Console.SetOut(stdOut);
            Console.WriteLine(cacheRes);

            Assert.IsTrue(cacheRes.ToString().Contains("Miss"));
        
        }



    }
}
