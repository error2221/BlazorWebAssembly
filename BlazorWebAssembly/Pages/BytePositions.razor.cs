using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorWebAssembly.Pages
{
    public partial class BytePositions
    {
        List<BytePosition> BAZA = new List<BytePosition>();
        List<BytePosition> resultList = new List<BytePosition>();

        string bazaCount;

        string searchString;

        void FindPositions()
        {
            isLoading = true;
            bazaCount = "Загрузка...";
            HttpClient client = new HttpClient();
            resultList = BAZA.FindAll(x => x.Name.Trim().ToUpper().Contains(searchString.Trim().ToUpper()));
            bazaCount = resultList.Count.ToString();
            isLoading = false;
        }

        

        bool isLoading;
        [Inject] HttpClient Http { get; set; }
        public async Task LoadBazaAsync()
        {
            var bigString = await Http.GetStringAsync("sample-data/data.txt");
            BAZA.Clear();

            var strings = bigString.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in strings)
            {
                try
                {
                    BAZA.Add(new BytePosition(line));
                }
                catch
                {
                    Console.WriteLine($"Были ошибки: {line}");
                }
                
            }
            
        }

        protected override void OnInitialized()
        {
            LoadBazaAsync();
        }
    }

    public class BytePosition
    {
        public string Name { get; set; }
        public string SiteCategory { get; set; }

        public string Uid { get; set; }

        public bool Good;

        public BytePosition()
        {

        }

        public BytePosition(string line)
        {
            Good = false;
            var temp = line.Split(';');
            if (temp.Length == 3)
            {
                Name = temp[0];
                SiteCategory = temp[1];
                Uid = temp[2];
                Good = true;
            }


        }
    }
}
    
