using System;

namespace Gifer
{
    public partial class LoadingWindow
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        public void SetProgress(double progress)
        {
            ProgressBar.Value = progress;
            if (Math.Abs(progress) < 0.0001D)
            {
                ProgressBar.IsIndeterminate = false;
            }
        }

        public double Progress
        {
            get { return ProgressBar.Value; }
            set
            {
                ProgressBar.Value = value;
                ProgressBar.IsIndeterminate = Math.Abs(value) > 0.0001D;
            }
        }
    }
}
