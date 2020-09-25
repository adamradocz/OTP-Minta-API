namespace Otp.WindowsForms.Views
{
    interface IMainView
    {
        public string ApiAddress { get; set; }
        public string DownloadFilePath { get; }
        public string UploadFilePath { get; }
        public string[] Messages { set; }
    }
}
