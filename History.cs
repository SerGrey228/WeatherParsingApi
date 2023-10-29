using Npgsql;
using System;
using System.Runtime.CompilerServices;

namespace WeatherApi
{
    public class History
    {
        public async Task SaveHistory(string data, string time, string cityName, string weather)
        {
            var connString = "Host=localhost;Username=postgres;Password=1234;Database=citydb";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            var dataSource = dataSourceBuilder.Build();

            var conn = await dataSource.OpenConnectionAsync();

            
            await using (var cmd = new NpgsqlCommand($"INSERT INTO history (datevalue, timevalue, cityname, weather) VALUES ('{data}', '{time}', '{cityName}', {weather})", conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }

        }

        public string GetHistory(string data, string cityName)
        {
            var connectionDataBase = "Host=localhost;Username=postgres;Password=1234;Database=citydb";

            using var connection = new NpgsqlConnection(connectionDataBase);

            connection.Open();

            string sql = $"SELECT * FROM history WHERE datevalue = '{data}' AND cityname = '{cityName}'";

            string line = "";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    line += $"Дата: {reader[1].ToString().Remove(10)} | Время: {reader[2].ToString()}" +
                        $" | Город: {reader[3].ToString()} | Температура: {reader[4].ToString()}\n";
                }
            }
            return line;
        }
    }
}