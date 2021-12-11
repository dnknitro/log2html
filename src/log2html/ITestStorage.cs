namespace dnk.log2html
{
	public interface ITestStorage
	{
		T Get<T>(string key);

		void Set(string key, object value);
	}
}