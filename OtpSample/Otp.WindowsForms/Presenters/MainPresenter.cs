using Otp.WindowsForms.Helpers;
using Otp.WindowsForms.Views;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Otp.WindowsForms.Presenters
{
    class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IFileSystem _fileSystem = new FileSystem();
        private static readonly HttpClient _client = new HttpClient();

        public MainPresenter(IMainView view)
        {
            _view = view;
        }

        public bool UrlIsValid()
        {
            return Uri.TryCreate(_view.ApiAddress, UriKind.Absolute, out _);
        }

        public async Task GetDokumentumokAsync()
        {
            if (!string.IsNullOrEmpty(Path.GetFileName(_view.ApiAddress)))
            {
                _view.Messages = new string[] { $"Az URL fájl nevet tartalmaz." };
                return;
            }

            HttpResponseMessage response = await _client.GetAsync(_view.ApiAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var fileSystemEntities = JsonSerializer.Deserialize<string[]>(jsonContent);
                _view.Messages = fileSystemEntities;
            }
            else
            {
                string[] message = { await response.Content.ReadAsStringAsync() };
                _view.Messages = message;
            }
        }

        public async Task DownloadDokumentumAsync()
        {
            if (string.IsNullOrEmpty(Path.GetFileName(_view.ApiAddress)))
            {
                _view.Messages = new string[] { $"Az URL nem tartalmaz fájl nevet." };
                return;
            }

            HttpResponseMessage response = await _client.GetAsync(_view.ApiAddress);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                await FileProcessor.Encoder.DecodeFromBase64Async(_fileSystem, _view.DownloadFilePath, content);
                _view.Messages = new string[] { $"Fájl mentése sikerült: {_view.DownloadFilePath}" };
            }
            else
            {
                _view.Messages = new string[] { await response.Content.ReadAsStringAsync() };
            }
        }

        public async Task GetDokumentumSizeAsync()
        {
            if (string.IsNullOrEmpty(Path.GetFileName(_view.ApiAddress)))
            {
                _view.Messages = new string[] { $"Az URL nem tartalmaz fájl nevet." };
                return;
            }

            var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Head, _view.ApiAddress));
            if (response.IsSuccessStatusCode)
            {
                var contentLenght = response.Content.Headers.ContentLength;
                _view.Messages = new string[] { $"Fájlméret: {Formatter.BytesToHumanReadableFormat(contentLenght)}" };
            }
            else
            {
                _view.Messages = new string[] { await response.Content.ReadAsStringAsync() };
            }
        }

        public async Task UploadDokumentumAsync()
        {
            if (string.IsNullOrWhiteSpace(_view.UploadFilePath))
            {
                _view.Messages = new string[] { "Feltöltéshez ki kell választani egy fájlt." };
                return;
            }

            if (!File.Exists(_view.UploadFilePath))
            {
                _view.Messages = new string[] { "Feltöltéshez kiválasztott fájl nem létezik." };
                return;
            }

            var convertedData = await FileProcessor.Encoder.EncodeToBase64Async(_fileSystem, _view.UploadFilePath);
            var stringContent = new StringContent(JsonSerializer.Serialize(convertedData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_view.ApiAddress, stringContent);
            _view.Messages = new string[] { await response.Content.ReadAsStringAsync() };
        }
    }
}
