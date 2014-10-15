using System;
using System.Collections.Generic;

[Serializable]
public class DataScheme
{
    public Dictionary<string, Type> Fields { get; set; }
    public DataType DataType { get; set; }
    public StorageType StorageType { get; set; }
}
