using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.ComponentModel;
using System.Configuration;
using System.Media;

namespace Pomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NotifyIcon ni;
        System.ComponentModel.BackgroundWorker WorkTime;
        public MainWindow()
        {
            WorkTime = new BackgroundWorker();
            WorkTime.DoWork += WorkTime_DoWork;
            WorkTime.RunWorkerCompleted += WorkTime_Done;

            InitializeComponent();

            ni = new NotifyIcon();
            ni.Icon = Pomodoro.Properties.Resources.pomodoro;
            ToolStripMenuItem tmiClose = new ToolStripMenuItem();
            tmiClose.Text = "Close";
            tmiClose.Click += new System.EventHandler(tmiClose_Click);
            ContextMenuStrip cmsClose = new ContextMenuStrip();
            cmsClose.Items.Add(tmiClose);
            ni.ContextMenuStrip = cmsClose;
            ni.Visible = true;
        }

        void tmiClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void cmClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnWork_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Hide();
            WorkTime.RunWorkerAsync();
        }

        private void WorkTime_Done(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Show();
            SoundPlayer p = new SoundPlayer(Pomodoro.Properties.Resources.beeps);
            p.Play();
        }

        private void WorkTime_DoWork(object sender, DoWorkEventArgs e)
        {
            AppSettingsReader asr = new AppSettingsReader();
            int workTime = 60 * 1000 * (int)asr.GetValue("PomodoroPeriod", typeof(int));
            BackgroundWorker worker = (sender as BackgroundWorker);
            System.Threading.Thread.Sleep(workTime);
        }
    }
}
