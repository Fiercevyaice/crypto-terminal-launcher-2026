using System;
using System.IO;
using System.Windows;

namespace CryptoTerminalLauncher
{
    /// <summary>
    /// Main window for Crypto Terminal Launcher.
    /// Handles terminal selection, download, and launch.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TerminalManager _terminalManager = new TerminalManager();
        private readonly SafetyChecker _safetyChecker = new SafetyChecker();
        private readonly ConfigManager _config = ConfigManager.Load();

        public MainWindow()
        {
            InitializeComponent();
            LoadTerminals();
            txtStatus.Text = "Ready";
        }

        private void LoadTerminals()
        {
            var terminals = _terminalManager.GetAvailableTerminals();
            lstTerminals.ItemsSource = terminals;
        }

        private void LstTerminals_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lstTerminals.SelectedItem is TerminalInfo selected)
            {
                var warning = _safetyChecker.CheckTerminalSafety(selected.ReleaseUrl);
                if (warning != null)
                {
                    txtStatus.Text = warning;
                }
                else
                {
                    txtStatus.Text = $"Selected: {selected.Name}";
                }
            }
        }

        private async void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string name)
            {
                var terminal = _terminalManager.GetAvailableTerminals().Find(t => t.Name == name);
                if (terminal == null) return;

                var targetPath = string.IsNullOrEmpty(_config.InstallPath)
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CryptoTerminalLauncher", name)
                    : Path.Combine(_config.InstallPath, name);

                txtStatus.Text = $"Downloading {name}...";

                try
                {
                    await _terminalManager.DownloadAndExtractAsync(name, targetPath);
                    txtStatus.Text = $"Downloaded {name} to {targetPath}";

                    // Look for the main executable in the extracted folder.
                    var exeFiles = Directory.GetFiles(targetPath, "*.exe", SearchOption.AllDirectories);
                    if (exeFiles.Length > 0)
                    {
                        _terminalManager.LaunchTerminal(name, exeFiles[0]);
                        txtStatus.Text = $"Launched {name}";
                    }
                    else
                    {
                        txtStatus.Text = $"No executable found in {targetPath}";
                    }
                }
                catch (Exception ex)
                {
                    txtStatus.Text = $"Error: {ex.Message}";
                }
            }
        }
    }
}