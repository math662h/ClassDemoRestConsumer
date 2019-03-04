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

        private const string URI = "http://localhost:50935/api/Hotels";

        public Worker()
        {
        }

        public void Start()
        {
            List<Hotel> hotels = GetAll();

            foreach (var hotel in hotels)
            {
                Console.WriteLine("Hotel:: " + hotel);
            }

            Console.WriteLine("Henter nummer 55");
            Console.WriteLine("Hotel :: " + GetOne(55));


            Console.WriteLine("Sletter nummer 55");
            Console.WriteLine("Resultat = " + Delete(55));

            Console.WriteLine("Opretter nyt hotel object id findes ");
            Console.WriteLine("Resultat = " + Post(new Hotel(5, "Findes", "vej3")));

            Console.WriteLine("Opretter nyt hotel object id findes ikke");
            Console.WriteLine("Resultat = " + Post(new Hotel(49, "Findes ikke", "vej3")));

            Console.WriteLine("Opdaterer nr 50");
            Console.WriteLine("Resultat = " + Put(50, new Hotel(50, "Pouls", "Hillerød")));
        }


        private List<Hotel> GetAll()
        {
            List<Hotel> hoteller = new List<Hotel>();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI);
                String jsonStr = resTask.Result;

                hoteller = JsonConvert.DeserializeObject<List<Hotel>>(jsonStr);
            }


            return hoteller;
        }
        


        private Hotel GetOne(int id)
        {
            Hotel hotel = new Hotel();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI + "/" + id);
                String jsonStr = resTask.Result;

                hotel = JsonConvert.DeserializeObject<Hotel>(jsonStr);
            }


            return hotel;
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

        private bool Post(Hotel hotel)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(hotel);
                StringContent content = new StringContent(jsonStr, Encoding.ASCII, "application/json");

                Task<HttpResponseMessage> postAsync = client.PostAsync(URI,content);

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

        private bool Put(int id, Hotel hotel)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(hotel);
                StringContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> putAsync = client.PutAsync(URI+ "/" + id, content);

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