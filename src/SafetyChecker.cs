using System;

namespace CryptoTerminalLauncher
{
    /// <summary>
    /// Provides safety checks for downloaded terminal sources.
    /// </summary>
    public class SafetyChecker
    {
        /// <summary>
        /// Checks whether a URL points to a known modified or unofficial source.
        /// </summary>
        /// <param name="url">URL to check.</param>
        /// <returns>Warning message if the source looks suspicious, or null if safe.</returns>
        public string? CheckTerminalSafety(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "URL is empty.";

            var lower = url.ToLowerInvariant();

            // Keywords that indicate unofficial or modified builds.
            var suspiciousKeywords = new[]
            {
                "keygen",
                "patched",
                "pre-activated",
                "modified-build",
                "unofficial-release"
            };

            foreach (var keyword in suspiciousKeywords)
            {
                if (lower.Contains(keyword))
                {
                    return $"URL contains suspicious keyword '{keyword}'. This may be modified software with security risks.";
                }
            }

            // Only allow GitHub and known safe hosts.
            if (!lower.Contains("github.com") && !lower.Contains("gitlab.com"))
            {
                return "URL is not from a known open-source host. Download only from official repositories.";
            }

            return null;
        }
    }
}