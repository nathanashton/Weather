using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class UnhandledExceptionWindowViewModel
    {
        private readonly ISettings _settings;

        public Window Window { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }

        public bool ShowDetails { get; set; }

        public ICommand DetailsCommand
        {
            get { return new RelayCommand(Details, x => true); }
        }

        public ICommand SendErrorReportCommand
        {
            get { return new RelayCommand(SendErrorReport, x => true); }
        }

        public UnhandledExceptionWindowViewModel(ISettings settings)
        {
            _settings = settings;
        }

        private void Details(object obj)
        {
            if (ShowDetails)
            {
                ShowDetails = false;
                Window.SizeToContent = SizeToContent.Height;
            }
            else
            {
                ShowDetails = true;
                Window.SizeToContent = SizeToContent.Height;
            }
        }

        private void SendErrorReport(object obj)
        {
            string[] lines = {Message, Source, StackTrace};
            File.WriteAllLines(Path.Combine(_settings.ErrorPath, "errorreport.txt"), lines);
            Zip();

            File.Delete(Path.Combine(_settings.ErrorPath, "errorreport.txt"));
            File.Delete(Path.Combine(_settings.ErrorPath, "unhandledexception.jpg"));

            //TODO Email report

            Environment.Exit(1);
        }

        private void Zip()
        {
            var zipName = DateTime.Now.ToString("ddMMyyyyHHmmss") + ".zip";

            var startPath = _settings.ErrorPath;
            var zipPath = Path.Combine(_settings.ApplicationPath, zipName);

            ZipFile.CreateFromDirectory(startPath, zipPath);

            File.Move(zipPath, Path.Combine(_settings.ErrorPath, zipName));
        }
    }
}