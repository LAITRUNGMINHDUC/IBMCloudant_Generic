    using GenericCloudant.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GenericCloudant.Services
{
    public class GenericCloudantService<T> where T : CloudantModel, new()
    {
        private static readonly string _dbName = "todos";
        private readonly Creds _cloudantCreds;        

        public GenericCloudantService()
        {
            _cloudantCreds = new Creds(){
                  host = "******.cloudant.com",
                  username = "*******",
                  password = "*******"
            };                     
        }

        public async Task<dynamic> CreateAsync(T item)
        {
            using (var client = CloudantClient())
            {
                var response = await client.PostAsJsonAsync(_dbName, item);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsAsync<T>();
                    return JsonConvert.SerializeObject(new { id = responseJson.id, rev = responseJson.rev });
                }
                string msg = "Failure to POST. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
                Console.WriteLine(msg);
                return JsonConvert.SerializeObject(new { msg = "Failure to POST. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase });
            }

        }

        public async Task<dynamic> DeleteAsync(T item)
        {
            using (var client = CloudantClient())
            {
                var response = await client.DeleteAsync(_dbName + "/" + item.id + "?rev=" + item.rev);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsAsync<T>();
                    return JsonConvert.SerializeObject(new { id = responseJson.id, rev = responseJson.rev });
                }
                string msg = "Failure to DELETE. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
                Console.WriteLine(msg);
                return JsonConvert.SerializeObject(new { msg = msg });
            }
        }

        public async Task<dynamic> GetAllAsync(string model)
        {
            using (var client = CloudantClient())
            {
                try
                {
                    dynamic jsonObject = new JObject();
                    jsonObject.selector = new JObject();
                    jsonObject.selector.ClassName = model;

                    var response = await client.PostAsJsonAsync
                        (_dbName + "/_find", (JObject)jsonObject);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    string msg = "Failure to GET. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
                    Console.WriteLine(msg);
                    return JsonConvert.SerializeObject(new { msg = msg });

                }
                catch (Exception)
                {                    
                    throw;
                }
            }
        }

        public async Task<string> UpdateAsync(T item)
        {
            using (var client = CloudantClient())
            {
                var response = await client.PutAsJsonAsync(_dbName + "/" + item.id + "?rev=" + item.rev, item);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsAsync<T>();
                    return JsonConvert.SerializeObject(new { id = responseJson.id, rev = responseJson.rev });
                }
                string msg = "Failure to PUT. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
                Console.WriteLine(msg);
                return JsonConvert.SerializeObject(new { msg = msg });
            }
        }


        private HttpClient CloudantClient()
        {
            if (_cloudantCreds.username == null || _cloudantCreds.password == null || _cloudantCreds.host == null)
            {
                throw new Exception("Missing Cloudant NoSQL DB service credentials");
            }

            var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(_cloudantCreds.username + ":" + _cloudantCreds.password));

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://" + _cloudantCreds.host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            return client;
        }

    }
}
