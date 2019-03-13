using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModelLib.model;
using Newtonsoft.Json;

namespace ClassDemoRestConsumer
{
    internal class Worker
    {

        private const string URI = "http://localhost:50935/api/Faciliteter";

        public Worker()
        {
        }

        public void Start()
        {
            List<Faciliteter> faciliteters = GetAll();

            foreach (var hotel in faciliteters)
            {
                Console.WriteLine("Faciliteter:: " + hotel.Name);
            }

            Console.WriteLine("Henter nummer 6");
            Console.WriteLine("Faciliteter :: " + GetOne(6));


            Console.WriteLine("Sletter nummer 6");
            Console.WriteLine("Resultat = " + Delete(6));

            Console.WriteLine("Opretter nyt facilitetsobject id findes ");
            Console.WriteLine("Resultat = " + Post(new Faciliteter(5, "Findes")));

            Console.WriteLine("Opretter nyt facilitetobject id findes ikke");
            Console.WriteLine("Resultat = " + Post(new Faciliteter(7, "Parkeringsplads")));

            Console.WriteLine("Opdaterer nr 6");
            Console.WriteLine("Resultat = " + Put(6, new Faciliteter(5, "Stripperstang")));
        }


        private List<Faciliteter> GetAll()
        {
            List<Faciliteter> faciliteters = new List<Faciliteter>();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI);
                String jsonStr = resTask.Result;

                faciliteters = JsonConvert.DeserializeObject<List<Faciliteter>>(jsonStr);
            }

            return faciliteters;
        }

        private Faciliteter GetOne(int id)
        {
            Faciliteter faciliteter = new Faciliteter();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI + "/" + id);
                String jsonStr = resTask.Result;

                faciliteter = JsonConvert.DeserializeObject<Faciliteter>(jsonStr);
            }

            return faciliteter;
        }

        private bool Delete(int id)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> deleteAsync = client.DeleteAsync(URI + "/" + id);

                HttpResponseMessage resp = deleteAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    String jsonStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonStr);
                }
                else
                {
                    ok = false;
                }
            }

            return ok;
        }

        private bool Post(Faciliteter faciliteter)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(faciliteter);
                StringContent content = new StringContent(jsonStr, Encoding.ASCII, "application/json");

                Task<HttpResponseMessage> postAsync = client.PostAsync(URI, content);

                HttpResponseMessage resp = postAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    String jsonResStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonResStr);
                }
                else
                {
                    ok = false;
                }
            }

            return ok;
        }

        private bool Put(int id, Faciliteter faciliteter)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(faciliteter);
                StringContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> putAsync = client.PutAsync(URI + "/" + id, content);

                HttpResponseMessage resp = putAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    String jsonResStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonResStr);
                }
                else
                {
                    ok = false;
                }
            }

            return ok;
        }

    }
}