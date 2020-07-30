using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//do not remove these namespaces they are used for the JSON and Parsing
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WeatherForecast.Models;

namespace WeatherForecast.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Weather()
        {

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public  String WeatherDetail(string City)
        {

            //This is my api key I created for you call "TestAndrew" however you can get your own API KEY or have the client get thiers from openweathermap.org or keep using this one. 
            string appId = "8113fcc5a7494b0518bd91ef3acc074f";

            //API path with CITY parameter and other parameters.This is very important, it is the current format used by the api. If the format changes you would have to change this as well
            //however it rarely changes. the {0} grabs information from the "City" and the {1} grabs information from the appid so if it every changes the , City, appId wouldn't change
            //just the url and then plug in {0} to where the city should be and {1} where the appid should be. For now and probably years though, this is fine. 
            //If you look in the url it says "imperial" that is for farienheit. If you / client wants metric just change it to metric. 
            //I am parsing the data as JSON however it can be parsed as html by adding mode=XML but doing that WILL break this webapp. So don't unless you plan on 
            //changing the application
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?zip={0}&units=imperial&cnt=1&APPID={1}", City, appId);

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                
                
                //This Converts JSON String to an OBJECT to let us read it better.
                RootObject weatherInfo = (new JavaScriptSerializer()).Deserialize<RootObject>(json);

                //This is a CUSTOM VIEWMODEL I made to  to send ONLY the fields we need not all fields will be received from 
                //www.openweathermap.org api
                ResultViewModel rslt = new ResultViewModel();

                rslt.Country = weatherInfo.sys.country;
                rslt.City = weatherInfo.name;
                rslt.Description = weatherInfo.weather[0].description;
                rslt.Humidity = Convert.ToString(weatherInfo.main.humidity);
                rslt.Temp = Convert.ToString(weatherInfo.main.temp);
                rslt.TempFeelsLike = Convert.ToString(weatherInfo.main.feels_like);
                rslt.TempMax = Convert.ToString(weatherInfo.main.temp_max);
                rslt.TempMin = Convert.ToString(weatherInfo.main.temp_min);
                rslt.WeatherIcon = weatherInfo.weather[0].icon;

                //Converting OBJECT to JSON String 
                var jsonstring = new JavaScriptSerializer().Serialize(rslt);

                //Return JSON string.
                return jsonstring;
            }
        }

    }
}