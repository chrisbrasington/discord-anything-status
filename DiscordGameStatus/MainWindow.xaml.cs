using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace DiscordGameStatus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LaunchWindow : Window
    {
        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);

        private string _gameListName = "GameList";
        private string _inputTextName = "Input";

        /// <summary>
        /// game list
        /// TODO: make editable
        /// </summary>
        private List<string> _gamelist = new List<string>();


        /// <summary>
        /// Launch Window
        /// </summary>
        public LaunchWindow()
        {
            Setup();
        }

        /// <summary>
        /// Initial Setup
        /// </summary>
        public void Setup()
        {
            //var currentProc = System.Diagnostics.Process.GetCurrentProcess();
            //string name = currentProc.ProcessName;

            InitializeComponent();
            SetupGameList();
        }

        /// <summary>
        /// Setup game list dropdown
        /// </summary>
        private void SetupGameList()
        {
            _gamelist.Add("Monster Hunter Rise");
            _gamelist.Add("Splatoon 2");

            var comboBox = (ComboBox)this.FindName(_gameListName);

            foreach (string game in _gamelist)
            {
                comboBox.Items.Add(game);
            }

            comboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// click GO button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // get active game to set
            string _activeGame = "";
            var inputText = (TextBox)this.FindName(_inputTextName);

            // use text box
            if (!string.IsNullOrEmpty(inputText.Text))
            {
                _activeGame = inputText.Text;
            }
            // use dropdown (text box is empty)
            else
            {
                var comboBox = (ComboBox)this.FindName(_gameListName);
                _activeGame = comboBox.SelectedItem.ToString();
            }

            // source running executable
            string source = $"{Environment.CurrentDirectory}\\DiscordGameStatus.exe";
            // destination running executable
            string destination = $"{Environment.CurrentDirectory}\\{_activeGame}.exe";

            // do nothing, already active
            if(source == destination)
            {
                return;
            }

            // create new executible (see README.md)
            File.Copy(source, destination, true);

            // start process
            Process p = Process.Start(destination);
            SpinWait.SpinUntil(() => p.MainWindowHandle != IntPtr.Zero);
            SetWindowText(p.MainWindowHandle, _activeGame);

            // terminiate this process
            this.Close();
        }
    }
}
