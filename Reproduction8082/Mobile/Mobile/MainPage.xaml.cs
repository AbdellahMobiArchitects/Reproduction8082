using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Mobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        const string todosUrl = "https://jsonplaceholder.typicode.com/todos";

        public MainPage()
        {
            InitializeComponent();
        }

        List<Todo> Todos = new List<Todo>();
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await GetSomeTodos();

        }

        private void UpdateResults()
        {
            LabelResult.Text = Todos.Count.ToString();
        }

        private async Task GetSomeTodos()
        {
            while (true)
            {
                var task1 = new List<Task>()
                {
                    GetTodos(),
                    GetTodos(),
                    GetTodos(),
                    GetTodos(),
                    GetTodos(),
                    GetTodos(),
                    GetTodos(),
                    GetTodos(),
                    GetTodos(),
                    GetTodos(),
                };

                await Task.Run(async() => await GetTodos());
                await Task.Run(() => GetTodos());
                await Task.WhenAll(task1);


                UpdateResults();
                await Task.Delay(5000);
            }
        }

        private async Task GetTodos()
        {
            var result = await GetRequest<List<Todo>>(todosUrl);
            Todos.AddRange(result);
        }

        private async Task<T> GetRequest<T>(string url)
        {
            using (var client = new HttpClient())
            {
                //setup client
                Uri uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = new HttpResponseMessage();

                response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var text = await StreamToStringAsync(stream).ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<T>(text);
                }
            }
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }
    }


    public class Todo
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }




}
