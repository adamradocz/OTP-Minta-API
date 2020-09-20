using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace Otp.WindowsForms
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient _client = new HttpClient();
        private string _dialogBoxFiler = "Minden fájl (*.*)|*.*|Text fájl|*.txt|PDF fájl|*.pdf";

        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonGetDokumentumok_Click(object sender, EventArgs e)
        {
            SetInfoTextBoxDefaultColor();
            textBoxInfo.Text = "Töltés...";
            HttpResponseMessage response = await _client.GetAsync(textBoxAddress.Text);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var fileSystemEntities = JsonConvert.DeserializeObject<string[]>(jsonContent);
                textBoxInfo.Text = string.Join(Environment.NewLine, fileSystemEntities);
            }
            else
            {
                SetInfoTextBoxErrorColor();
                textBoxInfo.Text = await response.Content.ReadAsStringAsync();
            }
        }

        private async void buttonDokumentumDownload_Click(object sender, EventArgs e)
        {
            HttpResponseMessage response = await _client.GetAsync(textBoxAddress.Text);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Dokumentum mentése";
                saveFileDialog.Filter = _dialogBoxFiler;
                saveFileDialog.FileName = Path.GetFileName(textBoxAddress.Text);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SetInfoTextBoxDefaultColor();
                    textBoxInfo.Text = "Töltés...";
                    try
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, Convert.FromBase64String(content));
                        textBoxInfo.Text = $"Fájl mentése sikerült: {saveFileDialog.FileName}";

                    }
                    catch (Exception)
                    {
                        SetInfoTextBoxErrorColor();
                        textBoxInfo.Text = $"Fájl mentése nem sikerült.";
                    }
                }
            }
            else
            {
                SetInfoTextBoxErrorColor();
                textBoxInfo.Text = await response.Content.ReadAsStringAsync();
            }
        }

        private void buttonDokumentumSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Dokumentum feltöltése";
            openFileDialog.Filter = _dialogBoxFiler;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxSelectedDokumentum.Text = openFileDialog.FileName;
                string previousFileName = Path.GetFileName(textBoxAddress.Text);
                textBoxAddress.Text = textBoxAddress.Text.TrimEnd(previousFileName.ToCharArray()) + Path.GetFileName(openFileDialog.FileName);
            }
        }

        private async void buttonDokumentumUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSelectedDokumentum.Text))
            {
                SetInfoTextBoxErrorColor();
                textBoxInfo.Text = "Feltöltéshez ki kellválasztani egy fájlt.";
                return;
            }

            if (!File.Exists(textBoxSelectedDokumentum.Text))
            {
                SetInfoTextBoxErrorColor();
                textBoxInfo.Text = "Feltöltéshez kiválasztott fájl nem létezik.";
                return;
            }

            SetInfoTextBoxDefaultColor();
            textBoxInfo.Text = "Töltés...";

            var fileData = File.ReadAllBytes(textBoxSelectedDokumentum.Text);
            var base64File = Convert.ToBase64String(fileData);
            var stringContent = new StringContent(JsonConvert.SerializeObject(base64File), Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await _client.PostAsync(textBoxAddress.Text, stringContent);
            if (!response.IsSuccessStatusCode)
            {
                SetInfoTextBoxErrorColor();
            }

            textBoxInfo.Text = await response.Content.ReadAsStringAsync();
        }

        private void SetInfoTextBoxErrorColor()
        {
            textBoxInfo.ForeColor = Color.Red;
        }

        private void SetInfoTextBoxDefaultColor()
        {
            textBoxInfo.ForeColor = Color.Black;
        }
    }
}
