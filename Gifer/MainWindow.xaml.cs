using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Gifer
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
        }
        
        public delegate void UrlLoadEventHandler(object sender, UrlLoadEventArgs e);

        public event UrlLoadEventHandler UrlLoad;

        private void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            var binding = UriTextBox.GetBindingExpression(TextBox.TextProperty);
            if (binding == null)
            {
                throw new Exception("UriTextBox binding is null. WTF?");
            }

            binding.UpdateSource();

            if (!binding.HasValidationError)
            {
                var result = _dialog.ShowDialog();
                if (result.GetValueOrDefault(false))
                {
                    UrlLoad.Invoke(this, new UrlLoadEventArgs { OutputPath = _dialog.FileName });
                }
            }
        }

        private readonly SaveFileDialog _dialog = new SaveFileDialog
        {
            DefaultExt = "gif",
            Filter = "GIF images (*.gif)|*.gif|All files (*.*)|*.*",
            Title = "Save GIF..."
        };
    }

    public class UrlLoadEventArgs : EventArgs
    {
        public String OutputPath { get; set; }
    }
}
