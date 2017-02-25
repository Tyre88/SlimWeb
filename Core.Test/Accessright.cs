using Core.BLL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Test
{
    [TestFixture]
    public class Accessright
    {
        [Test]
        public void TestRead()
        {
            bool hasAccess = AccessrightBLL.CheckAccessTypeRight(Enums.AccessType.Core, Enums.AccessTypeRight.Read, 1);
            Assert.AreEqual(hasAccess, true);
            hasAccess = AccessrightBLL.CheckAccessTypeRight(Enums.AccessType.Core, Enums.AccessTypeRight.Admin, 1);
            Assert.AreEqual(hasAccess, true);
        }
    }
}
