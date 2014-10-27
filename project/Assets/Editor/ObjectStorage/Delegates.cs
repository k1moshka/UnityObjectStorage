using System;

namespace UnityStaticData
{
    public delegate T RenderFieldMethod<T>();
    public delegate T RenderFieldMethodParametized<T>(T instance);
    public delegate object RenderCustomField(string label, object value);
}