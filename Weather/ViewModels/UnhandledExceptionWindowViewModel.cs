using PropertyChanged;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Input;
using Weather.Common.Interfaces;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class UnhandledExceptionWindowViewModel
    {
        public Window Window { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        private ISettings _settings;

        public bool ShowDetails { get; set; }

        public UnhandledExceptionWindowViewModel(ISettings settings)
        {
            _settings = settings;
        }

        public ICommand DetailsCommand
        {
            get { return new RelayCommand(Details, x => true); }
        }

        public ICommand SendErrorReportCommand
        {
            get { return new RelayCommand(SendErrorReport, x => true); }
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
            string[] lines = { Message, Source, StackTrace };
            System.IO.File.WriteAllLines(Path.Combine(_settings.ErrorPath, "errorreport.txt"), lines);
            Zip();

            File.Delete(Path.Combine(_settings.ErrorPath, "errorreport.txt"));
            File.Delete(Path.Combine(_settings.ErrorPath, "unhandledexception.jpg"));

            //TODO Email report

            Environment.Exit(1);
        }

        private void Zip()
        {
            var zipName = DateTime.Now.ToString("ddMMyyyyHHmmss") + ".zip";

            string startPath = _settings.ErrorPath;
            string zipPath = Path.Combine(_settings.ApplicationPath, zipName);

            ZipFile.CreateFromDirectory(startPath, zipPath);

            File.Move(zipPath, Path.Combine(_settings.ErrorPath, zipName));
        }
    }
}