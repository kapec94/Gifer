using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using Gifer.Annotations;

namespace Gifer
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel(MainWindow window)
        {
            _parent = window;
            _parent.UrlLoad += OnUrlLoad;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLoading { get { return _loadingWindow != null; } }

        public Uri VideoUri
        {
            get { return _videoUri; }
            set
            {
                _videoUri = value;
                OnPropertyChanged();
            }
        }

        public LoadingWindow OpenLoadingWindow()
        {
            if (IsLoading)
            {
                throw new InvalidOperationException("Loading window already opened");
            }

            _loadingWindow = new LoadingWindow {Owner = _parent};
            _loadingWindow.Show();

            return _loadingWindow;
        }

        public void CloseLoadingWindow()
        {
            if (!IsLoading)
            {
                throw new InvalidOperationException("Loading window not opened");
            }

            _loadingWindow.Close();
            _loadingWindow = null;
        }

        private void OnUrlLoad(object sender, UrlLoadEventArgs eventArgs)
        {
            var output = eventArgs.OutputPath;
            var tempFile = Path.GetTempFileName();
            var loadingWindow = OpenLoadingWindow();
            var client = new WebClient();

            client.DownloadProgressChanged += (o, args) =>
                loadingWindow.Progress = args.ProgressPercentage;

            client.DownloadFileCompleted += (o, args) =>
            {
                FFmpegException exception = null;
                try
                {
                    GifConverter.Convert(tempFile, output);
                }
                catch (FFmpegException e)
                {
                    exception = e;
                }

                CloseLoadingWindow();
                if (exception == null)
                {
                    Process.Start("explorer.exe", String.Format("/select,\"{0}\"", output));
                }
                else
                {
                    var box = new CrashMessageBox { TheException = exception, Owner = _parent };
                    box.ShowDialog();
                }

                VideoUri = null;
            };

            client.DownloadFileAsync(VideoUri, tempFile);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private LoadingWindow _loadingWindow;
        private Uri _videoUri;

        private readonly MainWindow _parent;
    }
}
