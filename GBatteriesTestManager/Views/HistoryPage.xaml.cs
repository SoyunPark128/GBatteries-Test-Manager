using GBatteriesTestManager.Models;
using GBatteriesTestManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class HistoryPage : Page
    {
        public MainViewModel ViewModel => App.ViewModel;
        public ObservableCollection<ResultData> TestResults = new ObservableCollection<ResultData>();

        public HistoryPage()
        {
            this.InitializeComponent();
        }

        private async void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if(StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
            {
                await ViewModel.GetTestListAsync(StartDatePicker.Date.Date, EndDatePicker.Date.Date);
                ComboboxBatteryType.SelectedItem = null;
                ComboboxBatteryId.SelectedItem = null;
            }
        }

        private void TestHistoryListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            args.ItemContainer.Background = Application.Current.Resources["CustomLightGrayGradientBrush"] as Brush;
        }

        private async void ComboboxBatteryType_DropDownClosed(object sender, object e)
        {
            
            if (ComboboxBatteryType.SelectedItem != null)
            {
                StartDatePicker.SelectedDate = null;
                EndDatePicker.SelectedDate = null;

                List<Battery> SelectedBatteries = new List<Battery>();
                SelectedBatteries = await ViewModel.GetBatteryListAsync(ComboboxBatteryType.SelectedValue as string);
                ComboboxBatteryId.ItemsSource = SelectedBatteries;
                ComboboxBatteryId.IsEnabled = true;
            }
        }

        private async void ComboboxBatteryId_DropDownClosed(object sender, object e)
        {
            Battery _selectedBattery = ComboboxBatteryId.SelectedItem as Battery;
            if (ComboboxBatteryId.SelectedItem != null)
            {
                await ViewModel.GetTestListAsync(_selectedBattery.Id);

                if (ToggleButtonReview.IsChecked == true)
                {
                    TestResults.Clear();

                    float _initCapacity = _selectedBattery.Capacity;
                    foreach (var t in ViewModel.Tests.Reverse())
                    {
                        ResultData _resultDate = new ResultData();
                        _resultDate.Cycle = t.Cycle;

                        if (t.Offset == "+")
                        {
                            _initCapacity += t.ChangedCapacity;
                        }
                        else
                        {
                            _initCapacity -= t.ChangedCapacity;
                        }
                        _resultDate.Capacity = _initCapacity;

                        TestResults.Add(_resultDate);
                    }
                }
                else if (ToggleButtonHistory.IsChecked == true)
                {
                    StartDatePicker.SelectedDate = null;
                    EndDatePicker.SelectedDate = null;
                }
                
            }
        }

        private void ToggleButtonReview_Checked(object sender, RoutedEventArgs e)
        {
            if (ToggleButtonHistory.IsChecked != null)
            {
                ToggleButtonHistory.IsChecked = false;
            }
            TestHistoryListView.Visibility = Visibility.Collapsed;
            ResultsGraph.Visibility = Visibility.Visible;
            StartDatePicker.IsEnabled = false;
            EndDatePicker.IsEnabled = false;
        }

        private void ToggleButtonHistory_Checked(object sender, RoutedEventArgs e)
        {
            if (ToggleButtonReview != null)
            {
                if (ToggleButtonReview.IsChecked != null)
                {
                    ToggleButtonReview.IsChecked = false;
                }
                TestHistoryListView.Visibility = Visibility.Visible;
                ResultsGraph.Visibility = Visibility.Collapsed;
                StartDatePicker.IsEnabled = true;
                EndDatePicker.IsEnabled = true;
            }
            
        }

        private void ResultsGraph_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {

        }
    }

    public struct ResultData
    {
        public int Cycle { get; set; }
        public float Capacity { get; set; }
    }
}
