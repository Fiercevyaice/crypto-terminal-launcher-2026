using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoTerminalLauncher
{
    /// <summary>
    /// Represents information about an available crypto terminal.
    /// </summary>
    public class TerminalInfo
    {
        public string Name { get; set; } = string.Empty;
        public string RepoUrl { get; set; } = string.Empty;
        public string ReleaseUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsOpenSource { get; set; }
    }

    /// <summary>
    /// Manages available crypto terminals: listing, downloading, and launching.
    /// </summary>
    public class TerminalManager
    {
        private readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };

        public TerminalManager()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoTerminalLauncher");
        }

        /// <summary>
        /// Returns the list of available crypto terminals.
        /// </summary>
        public List<TerminalInfo> GetAvailableTerminals()
        {
            return new List<TerminalInfo>
            {
                new TerminalInfo
                {
                    Name = "Depth Terminal",
                    RepoUrl = "https://github.com/depth-terminal/depth",
                    ReleaseUrl = "https://github.com/depth-terminal/depth/releases/latest",
                    Description = "Open-source crypto trading terminal with depth chart and order book.",
                    IsOpenSource = true
                },
                new TerminalInfo
                {
                    Name = "HypeTerminal",
                    RepoUrl = "https://github.com/hypeterminal/hypeterminal",
                    ReleaseUrl = "https://github.com/hypeterminal/hypeterminal/releases/latest",
                    Description = "Community-driven crypto terminal with social signals.",
                    IsOpenSource = true
                },
                new TerminalInfo
                {
                    Name = "Tradoor",
                    RepoUrl = "https://github.com/tradoor/tradoor",
                    ReleaseUrl = "https://github.com/tradoor/tradoor/releases/latest",
                    Description = "Lightweight crypto trading terminal for multiple exchanges.",
                    IsOpenSource = true
                }
            };
        }

        /// <summary>
        /// Downloads a ZIP release from GitHub and extracts it to the target path.
        /// </summary>
        /// <param name="terminalName">Name of the terminal to download.</param>
        /// <param name="targetPath">Directory where files will be extracted.</param>
        public async Task DownloadAndExtractAsync(string terminalName, string targetPath)
        {
            var terminal = GetAvailableTerminals().Find(t => t.Name.Equals(terminalName, StringComparison.OrdinalIgnoreCase));
            if (terminal == null)
                throw new ArgumentException($"Terminal '{terminalName}' not found.", nameof(terminalName));

            var zipUrl = terminal.ReleaseUrl.Replace("/releases/latest", "/releases/download/latest/terminal.zip");
            // TODO: replace with actual GitHub release URLs for each terminal.

            Directory.CreateDirectory(targetPath);
            var zipPath = Path.Combine(targetPath, "terminal.zip");

            using var response = await _httpClient.GetAsync(zipUrl);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Failed to download terminal: {response.StatusCode}");

            using var zipStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = File.Create(zipPath);
            await zipStream.CopyToAsync(fileStream);

            ZipFile.ExtractToDirectory(zipPath, targetPath, overwriteFiles: true);
            File.Delete(zipPath);
        }

        /// <summary>
        /// Launches a terminal executable.
        /// </summary>
        /// <param name="terminalName">Name of the terminal (for logging).</param>
        /// <param name="exePath">Path to the executable file.</param>
        public void LaunchTerminal(string terminalName, string exePath)
        {
            if (!File.Exists(exePath))
                throw new FileNotFoundException($"Executable not found: {exePath}");

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = true
            });
        }
    }
}