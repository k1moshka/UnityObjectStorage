using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityStaticData;

namespace UnityObjectStorage.Tests.SourceGeneration.Resoures
{
    [TestClass]
    public class EntitySourceGeneratorTest
    {
        [TestMethod]
        public void Test1()
        {
            foreach (var s in TestHelper.schemes)
            {
                EntitySourceGenerator.GenerateEntity(s);
            }
        }
    }
}
