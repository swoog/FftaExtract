namespace FftaExtract.Providers
{
    public class Job
    {
        public void Push(string api, params  object[] parameters)
        {
            var url = string.Format(api, parameters);
        }
    }
}