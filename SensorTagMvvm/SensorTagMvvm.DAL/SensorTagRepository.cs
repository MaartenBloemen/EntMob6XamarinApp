using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SensorTagMvvm.Domain;

namespace SensorTagMvvm.DAL
{
    public class SensorTagRepository : ISensorTagRepository
    {
        //TODO: rember to change IP address to correct one during the presentation (use cmd - ipconfig on laptop to find IP address.).
        const string API_BASE_URL = "http://192.168.43.60:8080";
        public async void PostTemperatureData(List<Temperature> temperatures)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = (new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", val("boyen", "root")));
                    var uri = API_BASE_URL + "/temperaturelist";
                    List<PostObject<float>> objects = new List<PostObject<float>>();
                    foreach (var temp in temperatures.ToList())
                    {
                        TimeSpan t = temp.Measured - new DateTime(1970, 1, 1);
                        long secondsSinceEpoch = (long)t.TotalSeconds;
                        PostObject<float> postObject = new PostObject<float>(temp.Value, secondsSinceEpoch);
                        objects.Add(postObject);
                    }
                    var json = JsonConvert.SerializeObject(objects);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(uri, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async void PostHumidityData(List<Humidity> humidities)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = (new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", val("boyen", "root")));
                    var uri = API_BASE_URL + "/humiditylist";
                    List<PostObject<float>> objects = new List<PostObject<float>>();
                    foreach (var humidity in humidities.ToList())
                    {
                        TimeSpan t = humidity.Measured - new DateTime(1970, 1, 1);
                        long secondsSinceEpoch = (long)t.TotalSeconds;
                        PostObject<float> postObject = new PostObject<float>(humidity.Percentage, secondsSinceEpoch);
                        objects.Add(postObject);
                    }
                    var json = JsonConvert.SerializeObject(objects);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(uri, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async void PostBarometerData(List<AirPressure> airPressures)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = (new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", val("boyen", "root")));
                    var uri = API_BASE_URL + "/airpressurelist";
                    List<PostObject<float>> objects = new List<PostObject<float>>();
                    foreach (var airpressure in airPressures.ToList())
                    {
                        TimeSpan t = airpressure.Measured - new DateTime(1970, 1, 1);
                        long secondsSinceEpoch = (long)t.TotalSeconds;
                        PostObject<float> postObject = new PostObject<float>(airpressure.Value, secondsSinceEpoch);
                        objects.Add(postObject);
                    }
                    var json = JsonConvert.SerializeObject(objects);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(uri, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async void PostOpticalData(List<Brightness> brightnesses)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = (new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", val("boyen", "root")));
                    var uri = API_BASE_URL + "/brightnesslist";
                    List<PostObject<float>> objects = new List<PostObject<float>>();
                    foreach (var brightness in brightnesses.ToList())
                    {
                        TimeSpan t = brightness.Measured - new DateTime(1970, 1, 1);
                        long secondsSinceEpoch = (long)t.TotalSeconds;
                        PostObject<float> postObject = new PostObject<float>(brightness.Value, secondsSinceEpoch);
                        objects.Add(postObject);
                    }
                    var json = JsonConvert.SerializeObject(objects);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(uri, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public string val(String userName, String userPassword)
        {
            string authInfo = userName + ":" + userPassword;
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            return authInfo;
        }
    }
}
