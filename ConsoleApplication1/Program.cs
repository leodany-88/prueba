using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace ConsoleApplication1
{
    class rest
    {
        static HttpClient client = new HttpClient();
 

        static void ShowItem(Item product)
        {
            Console.WriteLine("Name: " + product.Name + "Price: " + product.Price + "Detail: " +product.Detail);
        }

        static async Task<Uri> CreateProductAsync(Item product)
        {
            string json = @"{
                                 'condition': 
                                    [
                                      {'$q':{'key':'prueba'}},
                                      {'$q':{'key':{'key':{'$inlike':'%prueba%'}}},
                                      {'$q':{'key':{'$nlike':'%prueba%'}}}
                                    ],
                                  'order': {'name':1, 'id_item_type':0},
                                  'limit': 1,
                                  'offset': 0   
                                }";

            HttpResponseMessage response = await client.PostAsJsonAsync(
                "v1/item/view", product);
            string dataEnviada = json;
            Byte[] bytesSentMessage1 = Encoding.UTF8.GetBytes(dataEnviada);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<Item> GetProductAsync(string path)
        {
            Item product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Item>();
            }
            return product;
        }



        static void Main(string[] args)
        {

            try
            {
                RunAsync().GetAwaiter().GetResult();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task RunAsync()
        {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:8787/v1/item/view");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Item product = new Item
                {
                };

                var url = await CreateProductAsync(product);
                Console.WriteLine("Created at "+ url);

                // Get the product
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
                Console.ReadLine();
        }

    }

    public class Item
    {
        public string Id_item { get; set; }
        public string Id_char { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string Price { get; set; }
        public string Created_at { get; set; }
        public string Last_updated_at { get; set; }
        public string Id_item_status { get; set; }
        public string Id_item_type { get; set; }
        public string Id_item_unit_measuremente { get; set; }
    }

}
