namespace WeatherApi
{
    public class ConnectDB
    {
        public string connect = "Host=localhost;Username=postgres;Password=1234;Database=citydb";

        public string GetConnect()
        {
            return connect;
        }
    }
}
