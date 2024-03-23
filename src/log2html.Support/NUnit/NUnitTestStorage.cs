using NUnit.Framework;
using NUnit.Framework.Internal;

namespace dnk.log2html.Support.NUnit;

public class NUnitTestStorage : ITestStorage
{
    public T Get<T>(string key)
    {
        return TestContext.CurrentContext.Test.Properties.ContainsKey(key)
            ? (T)TestContext.CurrentContext.Test.Properties.Get(key)
            : default;
    }

    public void Set(string key, object value) => TestExecutionContext.CurrentContext.CurrentTest.Properties.Set(key, value);
}