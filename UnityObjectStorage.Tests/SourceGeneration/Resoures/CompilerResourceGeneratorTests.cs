using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityStaticData;
using System.Collections.Generic;

namespace UnityObjectStorage.Tests.SourceGeneration.Resoures
{
    [TestClass]
    public class CompilerResourceGeneratorTests
    {
        /// <summary>
        ///Initialize() is called once during test execution before
        ///test methods in this test class are executed.
        ///</summary>
        [TestInitialize()]
        public void Initialize()
        {
            TestHelper.Init();
            EntitySourceGenerator.GenerateEntityBase();
            foreach (var s in TestHelper.schemes)
            {
                EntitySourceGenerator.GenerateEntity(s);
            }
        }

        [TestMethod]
        public void GenerateResourceRepositoryTest()
        {
            try
            {
                var compiler = new CompilerResourceGenerator();

                compiler.GenerateResourceRepository(Settings.GetPathToSaveSources("schemes.bin"), TestHelper.schemes);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod]
        public void DeserializeResourcesToDictTest()
        {
            var dict = Serializator.LoadFrom<Dictionary<string, object[]>>(Settings.GetPathToSaveSources("schemes.bin"));
        }
    }
}
