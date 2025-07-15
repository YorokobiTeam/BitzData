using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitzData;
namespace BitzData.Tests
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void MD5ShouldCompute()
        {
            Assert.AreEqual("162504d58f00cfba77fe75f3e4ec15d0", BitzData.Utilities.GetFileMD5("C:\\Users\\ASUS\\Pictures\\TestData\\test-2.png"));
        }

        [TestMethod]
        public void GetFileExtension()
        {
            Assert.AreEqual("png", Utilities.GetExtension("test.png"));
            Assert.AreEqual("", Utilities.GetExtension("nbcxozvcxv"));
        }
    }
}
