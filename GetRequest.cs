using System.Net;

namespace WeatherApi
{
    public class GetRequest
    {
        HttpWebRequest request;
        string address;

        public string Response { get; set; }

        public GetRequest(HttpWebRequest request, string _address)
        {
            this.request = request;
            this.address = _address;
        }

        public void Run()
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null) Response = new StreamReader(stream).ReadToEnd();
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка подключения");
            }
        }
    }
}
