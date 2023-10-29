using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Globalization;
using System.Net;
using System.Xml.Linq;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public string GetWeather(string cityName) // получение погоды по введенному населенному пункту
        {

            //парсинг координат с яндекс геокодер
            string apiYandexCoord = $"https://geocode-maps.yandex.ru/1.x/?apikey=5bca6481-2ce4-4ddb-8e1e-7987ecc2776e&geocode={cityName}&format=json";
            HttpWebRequest settingCoordRequest;

            settingCoordRequest = (HttpWebRequest)WebRequest.Create(apiYandexCoord);
            settingCoordRequest.Method = "GET";

            var coordRequest = new GetRequest(settingCoordRequest, apiYandexCoord);

            coordRequest.Run();

            var coordRecponse = coordRequest.Response;
            var coordJson = JObject.Parse(coordRecponse.ToString());

            var lowerCorner = coordJson["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["Point"]["pos"];

            string[] Coords = lowerCorner.ToString().Split(' ');


            Cities cities = new Cities();
            if (cities.SearchCity(cityName) == false)
            {
                cities.SaveCity(cityName, Coords); //сохранение города и его координат в бд
            }


            //парсинг данных с яндекс погоды
            string apiKeyYandexWeather = "X-Yandex-API-Key: 426015e6-eaa3-4b5a-8962-2b3f9ce93bc2";
            string apiYandexWeather = $"https://api.weather.yandex.ru/v2/forecast?lat={Coords[1]}&lon={Coords[0]}&extra=true";
            HttpWebRequest settingWeatherRequest;

            settingWeatherRequest = (HttpWebRequest)WebRequest.Create(apiYandexWeather);
            settingWeatherRequest.Method = "GET";
            settingWeatherRequest.Headers.Add(apiKeyYandexWeather);

            var weatherRequest = new GetRequest(settingWeatherRequest, apiYandexWeather);

            weatherRequest.Run();

            var weatherRecponse = weatherRequest.Response;
            var weatherJson = JObject.Parse(weatherRecponse.ToString());

            string region = weatherJson["info"]["tzinfo"]["name"].ToString();
            string temperature = weatherJson["fact"]["temp"].ToString();

            DateTime localDate = DateTime.Now;
            string data = localDate.ToString(new CultureInfo("ru-RU"));
            string[] dataAndTime = data.ToString().Split(' ');

            string result = $"Дата(ru): {dataAndTime[0]}\nВремя: {dataAndTime[1]}\nРегион: {region}\nТемпература: {temperature}";

            History history = new History();
            history.SaveHistory(dataAndTime[0], dataAndTime[1], cityName, temperature); // сохранение истории в БД

            return result;
        }

        [HttpGet("{id:int}")]
        public string GetHistoryByValue(string data, string cityName) // получение истории поиска по дате и городу
        {
            History history = new History();

            string result = history.GetHistory(data, cityName);

            return result;
        }


        [HttpPost]
        public void SetCityValue(string oldCityName, string newCityName) // изменение названия города 
        {
            Cities cities = new Cities();

            try
            {
                cities.SetCity(oldCityName, newCityName);
            }
            catch(Exception e)
            {

            }
        }

        [HttpDelete]
        public void DelCity(string  cityName) // удаление города по названию
        {
            Cities cities = new Cities();
            cities.DeleteCity(cityName);
        }
    }
}
