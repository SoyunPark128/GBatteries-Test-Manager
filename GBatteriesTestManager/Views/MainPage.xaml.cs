using CsvHelper;
using CsvHelper.Configuration.Attributes;
using GBatteriesTestManager.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GBatteriesTestManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime _now = DateTime.Now;
            WebClient webClient = new WebClient();
            string csvFilePath = System.IO.Path.GetTempPath() + @"text.csv";
            webClient.DownloadFile(@"http://127.0.0.1:5000/downloadTest", csvFilePath);

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = false;
                var records = csv.GetRecords<Data>();

                string result = "";
                foreach (var row in records)
                {
                    result += $"Battery Temperature : {row.Temperature}   |   {row.Current}A X {row.Volt}V = {row.Current * row.Volt}W\n";
                }
                //textBlock.Text = result;
            }

        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {

            AppShell.CurrentPage.AppFrame.Navigate(typeof(TestSetupPage));
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            AppShell.CurrentPage.AppFrame.Navigate(typeof(HistoryPage));
        }

    }
    public class Data
    {
        [Index(0)]
        public float Current { get; set; }

        [Index(1)]
        public float Volt { get; set; }

        [Index(2)]
        public float Temperature { get; set; }
    }
}
