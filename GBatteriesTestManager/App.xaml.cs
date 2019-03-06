using GBatteriesTestManager.Repository;
using GBatteriesTestManager.Repository.Sql;
using GBatteriesTestManager.ViewModels;
using GBatteriesTestManager.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GBatteriesTestManager
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        /// <summary>
        /// Gets the app-wide MainViewModel singleton instance.
        /// </summary>
        public static MainViewModel ViewModel { get; } = new MainViewModel();

        public Frame rootFrame = new Frame();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            rootFrame = Window.Current.Content as Frame;
            
            // Load the database.
            UseSqlite();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                
            }
            
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Set active window colors
            titleBar.ForegroundColor = (Color)Current.Resources["GBatteriesAccentColor"];
            titleBar.BackgroundColor = (Color)Current.Resources["GBatteriesBlackColor"];

            titleBar.ButtonForegroundColor = (Color)Current.Resources["GBatteriesAccentColor"];
            titleBar.ButtonBackgroundColor = (Color)Current.Resources["GBatteriesBlackColor"];
            titleBar.ButtonHoverForegroundColor = (Color)Current.Resources["GBatteriesAccentColorLight"];
            titleBar.ButtonHoverBackgroundColor = (Color)Current.Resources["GBatteriesBlackColorLight"];
            titleBar.ButtonPressedForegroundColor = (Color)Current.Resources["GBatteriesAccentColorDark"];
            titleBar.ButtonPressedBackgroundColor = (Color)Current.Resources["GBatteriesBlackColorDark"]; ;

            // Set inactive window colors
            titleBar.InactiveForegroundColor = Windows.UI.Colors.Gray;
            titleBar.InactiveBackgroundColor = Windows.UI.Colors.SeaGreen;
            titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.SeaGreen;

            if (rootFrame.Content == null)
            {
                ExtendedSplash extendedSplash = new ExtendedSplash(e);
                Window.Current.Content = extendedSplash;
                Window.Current.Activate();
            }


        }
        
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Pipeline for interacting with backend service or database.
        /// </summary>
        public static ITestManagerRepository Repository { get; private set; }

        /// <summary>
        /// Configures the app to use the Sqlite data source. If no existing Sqlite database exists, 
        /// loads a demo database filled with fake data so the app has content.
        /// </summary>
        public static void UseSqlite()
        {
            String databasePath = ApplicationData.Current.LocalFolder.Path + @"\GBatteriesTest.db";

            var dbOptions = new DbContextOptionsBuilder<TestManagerContext>().UseSqlite(
                "Data Source=" + databasePath);
            Repository = new SqlTestManagerRepository(dbOptions);

        }
    }
}
