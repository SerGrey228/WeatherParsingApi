using Npgsql;
using System.Collections.Generic;

namespace WeatherApi
{
    public class Cities
    {
        public async Task SaveCity(string cityName, string[] coord)
        {
            var connString = "Host=localhost;Username=postgres;Password=1234;Database=citydb";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            var dataSource = dataSourceBuilder.Build();

            var conn = await dataSource.OpenConnectionAsync();


            await using (var cmd = new NpgsqlCommand($"INSERT INTO cities (cityname, longitude, latitude) VALUES ('{cityName}', {coord[0]}, {coord[1]})", conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }

        }

        public bool SearchCity(string cityName)
        {
            var connectionDataBase = "Host=localhost;Username=postgres;Password=1234;Database=citydb";

            using var connection = new NpgsqlConnection(connectionDataBase);

            connection.Open();

            string sql = $"SELECT * FROM cities WHERE cityname = '{cityName}'";

            string line = "";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    line += reader[0].ToString();
                }
            }
            if (line == cityName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task SetCity(string cityName, string newCityName)
        {
            var connString = "Host=localhost;Username=postgres;Password=1234;Database=citydb";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            var dataSource = dataSourceBuilder.Build();

            var conn = await dataSource.OpenConnectionAsync();


            await using (var cmd = new NpgsqlCommand($"UPDATE cities SET cityname = '{newCityName}' WHERE cityname = '{cityName}'", conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteCity(string cityName)
        {
            var connString = "Host=localhost;Username=postgres;Password=1234;Database=citydb";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            var dataSource = dataSourceBuilder.Build();

            var conn = await dataSource.OpenConnectionAsync();


            await using (var cmd = new NpgsqlCommand($"DELETE FROM cities WHERE cityname = '{cityName}'", conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
