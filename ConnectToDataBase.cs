namespace WeatherApi
{
    public class ConnectDB
    {
        public string connect = "Host=db;Username=postgres;Password=1234;Database=citydb";

        public string GetConnect()
        {
            return connect;
        }
    }
}
