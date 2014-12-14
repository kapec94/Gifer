using System.Media;
using System.Windows;

namespace Gifer
{
    public partial class CrashMessageBox
    {
        public CrashMessageBox()
        {
            InitializeComponent();
        }

        public FFmpegException TheException
        {
            get
            {
                return _exception;
            }
            set
            {
                _exception = value;
                ExceptionTextBox.Text = _exception.Message;
            }
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var sound = SystemSounds.Exclamation;
            sound.Play();
        }

        private FFmpegException _exception;
    }
}
