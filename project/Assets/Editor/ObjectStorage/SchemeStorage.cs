using System;
using System.Collections.Generic;

[Serializable]
public class SchemeStorage
{
    public Dictionary<string, DataScheme> schemes;

    private SchemeStorage()
    {
        schemes = new Dictionary<string, DataScheme>();
    }

    private readonly static SchemeStorage _instance;

    static SchemeStorage()
    {
        _instance = new SchemeStorage();
    }

    public static DataScheme GetScheme(string schemeName)
    {
        if (!_instance.schemes.ContainsKey(schemeName) || _instance.schemes[schemeName] == null)
            _instance.schemes[schemeName] = new DataScheme();
        return _instance.schemes[schemeName];
    }

    public static void SaveScheme(string schemeName, DataScheme scheme)
    {
        _instance.schemes[schemeName] = scheme;
    }
}