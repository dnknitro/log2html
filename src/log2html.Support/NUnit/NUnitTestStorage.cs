using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace dnk.log2html.Support.NUnit;

public class NUnitTestStorage : ITestStorage
{
    private IPropertyBag PropertyBag => TestExecutionContext.CurrentContext.CurrentTest.Properties;

    public T Get<T>(string key)
    {
        return PropertyBag.ContainsKey(key)
            ? (T)PropertyBag.Get(key)
            : default;
    }

    public void Set(string key, object value) => PropertyBag.Set(key, value);
}