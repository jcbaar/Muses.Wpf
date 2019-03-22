using Microsoft.Win32;
using Muses.Wpf.Helpers;
using System;
using System.Security;
using System.Windows;

namespace Muses.Wpf.Themes
{
    /// <summary>
    /// Simple <see cref="Application"/> derived class that enables automatically
    /// following the dark/light system theme settings of Windows 10.
    /// </summary>
    public class ThemedApplication : Application
    {
        // The registry key we are monitoring for the theme changes.
        const string subKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        RegistryMonitor _watcher;
        bool _followSystemTheme = true;

        /// <summary>
        /// Constructor. Initializes and instance of the object.
        /// </summary>
        public ThemedApplication()
        {
            _watcher = new RegistryMonitor(RegistryHive.CurrentUser, subKey);
            _watcher.RegChanged += Watcher_RegChanged;
            _watcher.RegChangeNotifyFilter = RegChangeNotifyFilter.Value;

            Startup += ThemedApplication_Startup;
            Exit += ThemedApplication_Exit;
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether or not we are
        /// following the system theme. Defaults to true.
        /// </summary>
        public bool FollowSystemTheme
        {
            get
            {
                return _followSystemTheme;
            }
            set
            {
                if(_followSystemTheme != value)
                {
                    _followSystemTheme = value;

                    // We need to start or stop the registry key monitoring
                    // accordingly.
                    if (_watcher != null)
                    {
                        if (value)
                        {
                            CheckSystemTheme();
                            _watcher.Start();
                        }
                        else
                            _watcher.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Called when the application exits. Cleans up the resources.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event parameters.</param>
        private void ThemedApplication_Exit(object sender, ExitEventArgs e)
        {
            Startup -= ThemedApplication_Startup;
            Exit -= ThemedApplication_Exit;

            if (_watcher != null)
            {
                _watcher.RegChanged -= Watcher_RegChanged;
                _watcher.Stop();
                _watcher.Dispose();
                _watcher = null;
            }
        }

        /// <summary>
        /// Called when the application has started. Starts monitoring if necessary.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event parameters.</param>
        private void ThemedApplication_Startup(object sender, StartupEventArgs e)
        {
            // We only start monitoring when told to do so.
            if (FollowSystemTheme)
            {
                CheckSystemTheme();
                _watcher.Start();
            }
        }

        /// <summary>
        /// Called when in the monitored registry key a value changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event parameters.</param>
        private void Watcher_RegChanged(object sender, EventArgs e)
        {
            CheckSystemTheme();
        }

        /// <summary>
        /// Helper function that reads the "AppsUseLightTheme" value from the 
        /// current user registry to see what system theme was chosen.
        /// </summary>
        private void CheckSystemTheme()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(subKey))
                {
                    if (key != null)
                    {
                        object o = key.GetValue("AppsUseLightTheme");
                        if (o != null)
                        {
                            // A value != 0 means light theme. Setting the 
                            // theme will check if it has actually changed.
                            if((int)o != 0) ThemeHelper.Theme = Theme.Light;
                            else ThemeHelper.Theme = Theme.Dark;
                        }
                    }
                }
            }
            // We will silently let this pass...
            catch(SecurityException)
            {

            }
        }
    }
}
