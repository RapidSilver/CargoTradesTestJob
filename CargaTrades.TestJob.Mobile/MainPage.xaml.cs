using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Net.Http;
using Newtonsoft;
using Newtonsoft.Json;
using CargoTrades.TestJob.Mobile.Data;

namespace CargaTrades.TestJob.Mobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string sendUrl = "api/records/save";
        private readonly string getUrl = "api/records/list";

        public MainPage()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("http://10.0.2.2:6600/");
        }

        private async void BtClicked1(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(EntryName.Text) || String.IsNullOrWhiteSpace(EntryPhone.Text))
            {
                await DisplayAlert("Ошибка", "Заполните обязательные поля!", "Закрыть");
                return;
            }

            Record record = new Record()
            { 
                Name = EntryName.Text,
                Phone = EntryPhone.Text
            };
            string jsonStr = JsonConvert.SerializeObject(record);

            var response = await client.PostAsync(sendUrl, new StringContent(jsonStr, Encoding.UTF8, "application/json"));
            string result = await response.Content.ReadAsStringAsync();

            var responseObj = JsonConvert.DeserializeObject<Record>(result);
            await DisplayAlert("Статус", $" контакт {responseObj.Name} добавлен", "ок");
        }

        private async void BtClicked2(object sender, EventArgs e)
        {
            var response = await client.GetAsync(getUrl);

            string result = await response.Content.ReadAsStringAsync();
            var responseList = JsonConvert.DeserializeObject<List<Record>>(result);
            if (responseList == null || responseList.Count == 0)
            {
                await DisplayAlert("Все контакты", "тут пусто :(", "Закрыть");
            }
            else
            {               
                StringBuilder sb = new StringBuilder();
                foreach (var record in responseList)
                {
                    sb.AppendLine($"{record.Id}: {record.Name} - {record.Phone}");
                }
                await DisplayAlert("Все контакты", sb.ToString(), "Закрыть");
            }
        }
    }
}
