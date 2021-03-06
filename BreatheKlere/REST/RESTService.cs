﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BreatheKlere.REST
{
    public class RESTService : IRestService
    {
        string baseURL = "https://maps.googleapis.com/maps/api/";
        string mqBaseURL = "http://www.mapquestapi.com/directions/v2/";
        string wilinskyURL = "http://ec2-52-56-59-47.eu-west-2.compute.amazonaws.com?meth=";

        HttpClient client;
        public RESTService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            client.MaxResponseContentBufferSize = 2560000;
        }

        public async Task<Direction> GetDirection(string origin, string destination, string mode)
        {
            string url = baseURL + "directions/json?alternative=true&key=" + Config.google_maps_ios_api_key + "&origin=" + origin + "&destination=" + destination + "&mode=" + mode;
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                var resultResponse = JsonConvert.DeserializeObject<Direction>(content);
                return resultResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             GetDirection ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<DistanceMatrix> GetDistance(string origin, string destination, string mode="driving")
        {
            string url = baseURL + "distancematrix/json?key=" + Config.google_maps_ios_api_key + "&origins=" + origin + "&destinations=" + destination + "&mode=" + mode;
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                var resultResponse = JsonConvert.DeserializeObject<DistanceMatrix>(content);
                return resultResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             GetDistance ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<GeoResult> GetGeoResult(string locationName)
        {
            string url = baseURL + "geocode/json?key=" + Config.google_maps_ios_api_key + "&address=" + locationName;
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                var resultResponse = JsonConvert.DeserializeObject<GeoResult>(content);
                return resultResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             GeoResult ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<Place> GetPlaces(string locationName, string location = null)
        {
            string url = baseURL + "place/autocomplete/json?key=" + Config.google_maps_ios_api_key + "&input=" + locationName;
            if(!string.IsNullOrEmpty(location))
                url += "&location=" + location;
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                var resultResponse = JsonConvert.DeserializeObject<Place>(content);
                return resultResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             GetPlace ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<MQDirection> GetMQDirection(string from, string to, string mode)
        {
            string url = mqBaseURL + "route?fullShape=true&key=" + Config.mapquest_key + "&from=" + from + "&to=" + to + "&routeType=" + mode + "&shapeFormat=cmp";
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                var resultResponse = JsonConvert.DeserializeObject<MQDirection>(content);
                return resultResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             GetMQDirection ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<MQAlternativeDirection> GetMQAlternativeDirection(string from, string to, string mode)
        {
            string url = mqBaseURL + "alternateroutes?maxRoutes=6&key=" + Config.mapquest_key + "&from=" + from + "&to=" + to + "&routeType=" + mode + "&shapeFormat=cmp";
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("alternativeroutecontent", content);
                var resultResponse = JsonConvert.DeserializeObject<MQAlternativeDirection>(content);

                return resultResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             Get GetMQAlternativeDirection ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<Pollution> GetPollution(string request) {
            string url = wilinskyURL + "multi";
            var uri = new Uri(url);
            try
            {
                Debug.WriteLine("Pollution request------" + request);
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("request", request),
                });
                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, formContent).ConfigureAwait(false);
                Debug.WriteLine("Pollution response------" + response);
                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Pollution result------" + result);
                var pollution = JsonConvert.DeserializeObject<Pollution>(result);
                return pollution;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             GetPollution ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<Login> Login(string UN, string PW, string DID)
        {
            string url = wilinskyURL + "start";
            var uri = new Uri(url);
            try
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("UN", UN),
                    new KeyValuePair<string, string>("PW", PW),
                    new KeyValuePair<string, string>("DID", DID),
                });
                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, formContent).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync();

                var login = JsonConvert.DeserializeObject<Login>(result);
                return login;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             Login ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<Login> Register(string N, string UN, string PW, string PC, string G, string DID)
        {
            string url = wilinskyURL + "new";
            var uri = new Uri(url);
            try
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    //new KeyValuePair<string, string>("N", N),
                    new KeyValuePair<string, string>("UN", UN),
                    new KeyValuePair<string, string>("PW", PW),
                    //new KeyValuePair<string, string>("PC", PC),
                    //new KeyValuePair<string, string>("G", G),
                    //new KeyValuePair<string, string>("DID", DID),
                });
                HttpResponseMessage response = null;
                Debug.WriteLine(UN + ", "+ PW + ", " + DID);
                response = await client.PostAsync(uri, formContent).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync();
                var login = JsonConvert.DeserializeObject<Login>(result);
                return login;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             Reg ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<Base> SaveRoute(string DID, List<KeyValuePair<string, string>> keys)
        {
            string url = wilinskyURL + "save&&DID="+DID;
            var uri = new Uri(url);
            try
            {
                var formContent = new FormUrlEncodedContent(keys.ToArray());
                HttpResponseMessage response = null;
               
                response = await client.PostAsync(uri, formContent).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Base>(result);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             Reg ERROR {0}", ex.Message);
            }
            return null;
        }
    }
}
