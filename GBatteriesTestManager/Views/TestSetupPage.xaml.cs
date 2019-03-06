using CsvHelper;
using GBatteriesTestManager.Models;
using GBatteriesTestManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GBatteriesTestManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestSetupPage : Page
    {
        public MainViewModel ViewModel => App.ViewModel;
        DispatcherTimer timer4Stopwatch = new DispatcherTimer();
        Stopwatch stopwatch = new Stopwatch();
        Random rand = new Random();
        string[] offset = { "+", "-"};
        BatteryViewModel selectedBattery;
        int TestDataIndex = 0;
        float TempTemperaure = 0;

        public TestSetupPage()
        {
            this.InitializeComponent();
            timer4Stopwatch.Interval = TimeSpan.FromMilliseconds(1);
        }

        public void TimeCountStart()
        {
            selectedBattery = ComboboxBattery.SelectedItem as BatteryViewModel;
            ViewModel.GetTestData();

            timer4Stopwatch.Tick += TimerTicking;
            timer4Stopwatch.Start();
            stopwatch.Start();
        }

        public async void TimeCountStop()
        {
            timer4Stopwatch.Stop();
            timer4Stopwatch.Tick -= TimerTicking;
            stopwatch.Stop();

            
            float _changedCapacity = (float)(rand.Next(1, 10) / 20.0);
            int offsetIndex = rand.Next(offset.Length);

            await Task.Run(()=> AddNewTestResult(offset[offsetIndex], _changedCapacity));
            
            ChargingProgressBar.Foreground = Application.Current.Resources["GBatteriesAccentColorBrush"] as Brush;
            ChargingMinutes.Foreground = Application.Current.Resources["GBatteriesAccentColorBrush"] as Brush;
            ChargingProgress.Foreground = Application.Current.Resources["GBatteriesAccentColorBrush"] as Brush;

            selectedBattery.PerformedCycle++;
            await selectedBattery.SaveBatteryAsync();

            string _testResult = $"TEST RESULT : \n\tCharging Times : {ChargingMinutes.Text} \n\tChanged Battery Capacity : {offset[offsetIndex]}{_changedCapacity}\n\t Average of Ambient Temperature : {TempTemperaure/TestDataIndex}℃";


            var messageDialog = new MessageDialog(_testResult);


            // Add commands and set their command ids
            messageDialog.Commands.Add(new UICommand("Go history page", null, 0));
            messageDialog.Commands.Add(new UICommand("Test again with the current setting", null, 1));
            messageDialog.Commands.Add(new UICommand("New test", null, 2));
            // Show the message dialog
            var commandChosen = await messageDialog.ShowAsync();

            if (commandChosen.Id is 0)
            {
                AppShell.CurrentPage.AppFrame.Navigate(typeof(HistoryPage));
                //add to new history
            }
            else
            {
                ChargingMinutes.Text = "00:00:00.00";
                ChargingProgress.Text = "0%";
                ChargingProgressBar.Value = 0;

                if (commandChosen.Id is 2)
                {
                    TextDate.Text = String.Empty;
                    TextExperimenter.Text = String.Empty;
                    TextBatteryName.Text = String.Empty;
                    TextFastTypeCharge.Text = String.Empty;
                    TextPulseMode.Text = String.Empty;
                    TextCapacity.Text = String.Empty;
                    TextVoltage.Text = String.Empty;
                    TextImpedance.Text = String.Empty;
                    TextCyclelife.Text = String.Empty;
                    TextCurrentCycle.Text = String.Empty;
                    TextDOD.Visibility = Visibility.Collapsed;
                    TextPulseModeLabel.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
                    TextPulseMode.Visibility = Visibility.Collapsed;

                    ComboboxBattery.SelectedItem = null;
                    ComboBoxExperimenter.SelectedItem = null;
                    ComboBoxChargeType.SelectedItem = null;
                    ComboBoxPulseMode.SelectedItem = null;
                }
                else
                {
                    TextCurrentCycle.Text = " " + selectedBattery.PerformedCycle.ToString();
                }
            }

            TestDataIndex = 0;
            TempTemperaure = 0;
        }

        public async void AddNewTestResult(string offset, float changedCapacity)
        {
            Test _newTest = new Test();
            TestViewModel _newTestViewModel = new TestViewModel();

            _newTestViewModel.IsNewTest = true;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _newTestViewModel.BatteryId = selectedBattery.Id;
                _newTestViewModel.BatteryName = selectedBattery.Name;
                _newTestViewModel.PulseAlgorithm = (PulseAlgorithm)ComboBoxPulseMode.SelectedIndex;
                _newTestViewModel.FastChargeType = (FastChargeType)ComboBoxChargeType.SelectedIndex;
                _newTestViewModel.TesterName = ComboBoxExperimenter.SelectedItem.ToString();
                _newTestViewModel.Cycle = selectedBattery.PerformedCycle;
                
            });
            _newTestViewModel.FullChargedMin = stopwatch.Elapsed;
            _newTestViewModel.AverageEnvTemperature = TempTemperaure/TestDataIndex;
            _newTestViewModel.ChangedCapacity = changedCapacity;
            _newTestViewModel.DOD = 95;
            _newTestViewModel.Offset = offset;
            await _newTestViewModel.SaveTestAsync();
        }

        public void TimerTicking(object sender, object e)
        {
            TimeSpan ts = stopwatch.Elapsed;

            ChargingMinutes.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            float _watt = ViewModel.CurrentTestDatas[TestDataIndex].Current * ViewModel.CurrentTestDatas[TestDataIndex].Volt;
            ChargingProgressBar.Value += _watt/ selectedBattery.Watt;
            //ChargingProgressBar.Value += rand.Next(1, 100) /30;
            ChargingProgress.Text = ChargingProgressBar.Value.ToString("0.00") + "%";
            TempTemperaure += ViewModel.CurrentTestDatas[TestDataIndex].Temperature;

            if (ChargingProgressBar.Value == 100)
            {
                TimeCountStop();
            }

            TestDataIndex++;
            
        }
        
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            TimeCountStart();
        }

        private void ComboboxBattery_DropDownClosed(object sender, object e)
        {
            if (ComboboxBattery.SelectedItem != null)
            {
                selectedBattery = ComboboxBattery.SelectedItem as BatteryViewModel;
                if (selectedBattery?.Name == "Add New Battery...")
                {
                    NewBatteryFlyout.ShowAt((FrameworkElement)sender);
                }
                else if (selectedBattery != null)
                {
                    TextDate.Text = " " + DateTime.Today.ToShortDateString();
                    TextBatteryName.Text = " " + selectedBattery.Name;
                    TextCapacity.Text = " " + selectedBattery.Capacity.ToString() + "mAh";
                    TextImpedance.Text = " " + selectedBattery.InitInternalImpedance.ToString() + "mΩ";
                    TextCyclelife.Text = " " + selectedBattery.NominalCycleLife.ToString();
                    TextCurrentCycle.Text = " " + selectedBattery.PerformedCycle.ToString();
                    TextVoltage.Text = " " + selectedBattery.Voltage.ToString() + "V";
                    TextDOD.Visibility = Visibility.Visible;
                }
            }
            
        }
        private void ComboBoxExperimenter_DropDownClosed(object sender, object e)
        {
            if (ComboBoxExperimenter.SelectedItem != null)
            {
                TextExperimenter.Text = " " + ComboBoxExperimenter.SelectedItem.ToString();
            }
        }

        private void ComboBoxChargeType_DropDownClosed(object sender, object e)
        {
            if (ComboBoxChargeType.SelectedItem != null)
            {
                TextFastTypeCharge.Text = " " + ComboBoxChargeType.SelectedItem.ToString();

                if (TextFastTypeCharge.Text == " GBatteries")
                {
                    ComboBoxPulseMode.IsEnabled = true;
                }
            }
        }

        private void ComboBoxPulseMode_DropDownClosed(object sender, object e)
        {
            if (ComboBoxPulseMode.SelectedItem != null)
            {
                TextPulseMode.Text = " " + ComboBoxPulseMode.SelectedItem.ToString();
                TextPulseModeLabel.Foreground = Application.Current.Resources["GBatteriesAccentColorBrush"] as Brush;
            }
        }

        private async void NewBatteryButton_Click(object sender, RoutedEventArgs e)
        {
            Battery _newBattery = new Battery();
            BatteryViewModel _newBatteryViewModel = new BatteryViewModel(_newBattery);
            _newBatteryViewModel.IsNewBattery = true;

            _newBatteryViewModel.Name = newBatteryName.Text;
            _newBatteryViewModel.Voltage = float.Parse(newBatteryVoltage.Text, CultureInfo.InvariantCulture.NumberFormat);
            _newBatteryViewModel.Capacity = Convert.ToInt32(newBatteryCapacity.Text);
            _newBatteryViewModel.InitInternalImpedance = Convert.ToInt32(newBatteryImpedence.Text);
            _newBatteryViewModel.NominalCycleLife = Convert.ToInt32(newBatteryCycleLife.Text);

            NewBatteryFlyout.Hide();

            await _newBatteryViewModel.SaveBatteryAsync();



            // Show notification of new Battery has been saved successfully
            int duration = 2000;
            NewBatteryNotification.Show("New Battery has been saved successfully", duration);
        }
        
        private void NewBattery_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (newBatteryName.Text?.Length != 0 &&
                newBatteryCapacity.Text?.Length != 0 &&
                newBatteryImpedence.Text?.Length != 0 &&
                newBatteryCycleLife.Text?.Length != 0 &&
                newBatteryVoltage.Text?.Length != 0)
            {
                NewBatteryButton.IsEnabled = true;
            }
            else
            {
                NewBatteryButton.IsEnabled = false;
            }
        }

    }
}
