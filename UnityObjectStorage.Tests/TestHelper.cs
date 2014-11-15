using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityStaticData;

namespace UnityObjectStorage.Tests
{
    public static class TestHelper
    {
        public static Settings Settings = new Settings(true);
        public static DataScheme[] schemes;

        static TestHelper()
        {
            initSettings();
            initSchemes();
            initInstances();
        }

        public static void Init()
        {

        }

        private static void initSettings()
        {
            var dir = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
            dir = dir.Parent.Parent.Parent;
            var path = Path.Combine(dir.FullName, "testData");

            Settings.PathToSaveData = path;
            Settings.PathToSaveSources = path;

            typeof(Settings)
                .GetProperty("Instance")
                .SetValue(null, Settings);
        }

        private static void initSchemes()
        {
            schemes = new DataScheme[]
            {
                new DataScheme() 
                { 
                    TypeName = "Account",
                    DataType = DataType.Class, 
                    StorageType = StorageType.Compile, 
                    Fields = new Dictionary<string,Field>() 
                    {
                        { KeyGenerator.GenerateStringKey(), new Field(Settings.GetDescriptor("int")) { Name = "Id" } },
                        { KeyGenerator.GenerateStringKey(), new Field(Settings.GetDescriptor("String")) { Name = "FirstName" } }
                    }
                },
                new DataScheme() 
                { 
                    TypeName = "Hero",
                    DataType = DataType.Class, 
                    StorageType = StorageType.Compile, 
                    Fields = new Dictionary<string,Field>() 
                    {
                        { KeyGenerator.GenerateStringKey(), new Field(Settings.GetDescriptor("int")) { Name = "Id" } },
                        { KeyGenerator.GenerateStringKey(), new Field(Settings.GetDescriptor("int")) { Name = "Health" } },
                        { KeyGenerator.GenerateStringKey(), new Field(Settings.GetDescriptor("int")) { Name = "Damage" } },
                        { KeyGenerator.GenerateStringKey(), new Field(Settings.GetDescriptor("String")) { Name = "NickName" } },
                    }
                },
            };

            foreach (var s in schemes)
            {
                SchemeStorage.AddScheme(s);
            }    
        }

        private static void initInstances()
        {
            var accountInstances = new Instance[1]
            {
                new Instance(schemes[0]),
            };
            var heroInstances = new Instance[0];

            DataRegister.SaveInstances("Account", accountInstances);
            DataRegister.SaveInstances("Hero", heroInstances);
        }
    }
}