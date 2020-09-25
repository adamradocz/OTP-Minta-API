using Otp.WindowsForms.Helpers;
using Otp.WindowsForms.Presenters;
using Otp.WindowsForms.Views;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace Otp.WindowsForms
{
    public partial class MainView : Form, IMainView
    {
        public string ApiAddress { get => textBoxAddress.Text; set => textBoxAddress.Text = value; }
        public string DownloadFilePath { get; private set; }
        public string UploadFilePath { get => textBoxSelectedDokumentum.Text; }
        public string[] Messages { set => listBoxMessages.DataSource = value; }

        private readonly MainPresenter _presenter;
        private readonly string _dialogBoxFilter = "Minden fájl (*.*)|*.*|Text fájl|*.txt|PDF fájl|*.pdf";

        public MainView()
        {
            InitializeComponent();
            _presenter = new MainPresenter(this);
        }

        private async void buttonGetDokumentumok_Click(object sender, EventArgs e)
        {
            if (_presenter.UrlIsValid())
            {
                await _presenter.GetDokumentumokAsync();
            }
            else
            {
                errorProvider.SetError(textBoxAddress, "Érvénytelen API cím.");
            }            
        }

        private async void buttonDokumentumDownload_Click(object sender, EventArgs e)
        {
            if (_presenter.UrlIsValid())
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Dokumentum mentése",
                    Filter = _dialogBoxFilter,
                    FileName = Path.GetFileName(textBoxAddress.Text)
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DownloadFilePath = saveFileDialog.FileName;
                    await _presenter.DownloadDokumentumAsync();
                }
            }
            else
            {
                errorProvider.SetError(textBoxAddress, "Érvénytelen API cím.");
            }
        }

        private async void buttonDokumentumUpload_Click(object sender, EventArgs e)
        {
            if (_presenter.UrlIsValid())
            {
                await _presenter.UploadDokumentumAsync();
            }
            else
            {
                errorProvider.SetError(textBoxAddress, "Érvénytelen API cím.");
            }
        }

        private void buttonDokumentumSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Dokumentum feltöltése",
                Filter = _dialogBoxFilter
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxSelectedDokumentum.Text = openFileDialog.FileName;
                string previousFileName = Path.GetFileName(textBoxAddress.Text);
                textBoxAddress.Text = textBoxAddress.Text.TrimEnd(previousFileName.ToCharArray()) + Path.GetFileName(openFileDialog.FileName);
            }
        }

        private async void buttonDokumentumGetSize_Click(object sender, EventArgs e)
        {
            if (_presenter.UrlIsValid())
            {
                await _presenter.GetDokumentumSizeAsync();
            }
            else
            {
                errorProvider.SetError(textBoxAddress, "Érvénytelen API cím.");
            }
        }
    }
}
