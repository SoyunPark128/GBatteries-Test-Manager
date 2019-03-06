using CsvHelper;
using CsvHelper.Configuration.Attributes;
using GBatteriesTestManager.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace GBatteriesTestManager.ViewModels
{
    public class MainViewModel : BindableBase
    {
        /// <summary>
        /// Creates a new MainViewModel.
        /// </summary>
        /// 
        public MainViewModel()
        {
        }

        /// <summary>
        /// The collection of batteries in the list. 
        /// </summary>
        public ObservableCollection<BatteryViewModel> Batteries { get; }
            = new ObservableCollection<BatteryViewModel>();

        public ObservableCollection<string> BatteryVariations { get; }
            = new ObservableCollection<string>();

        public ObservableCollection<TestData> CurrentTestDatas { get; }
            = new ObservableCollection<TestData>();

        /// <summary>
        /// The collection of tests in the list. 
        /// </summary>
        public ObservableCollection<TestViewModel> Tests { get; }
            = new ObservableCollection<TestViewModel>();

        private BatteryViewModel _selectedBattery;

        /// <summary>
        /// Gets or sets the selected battery, or null if no battery is selected. 
        /// </summary>
        public BatteryViewModel SelectedBattery
        {
            get => _selectedBattery;
            set => Set(ref _selectedBattery, value);
        }

        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the batteries list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        /// <summary>
        /// Gets the complete list of batteries from the database.
        /// </summary>
        public async Task GetBatteryListAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);
            try
            {
                var batteries = await App.Repository.Batteries.GetAsync();
                if (batteries == null)
                {
                    return;
                }
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Batteries.Clear();
                    BatteryVariations.Clear();

                    var defaultBattery = new BatteryViewModel();
                    defaultBattery.Name = "Add New Battery...";
                    Batteries.Add(defaultBattery);

                    foreach (var c in batteries)
                    {
                        Batteries.Add(new BatteryViewModel(c));

                        if (BatteryVariations.Contains(c.Name) != true)
                        {
                            BatteryVariations.Add(c.Name);
                        }
                    }
                    IsLoading = false;
                });
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Gets the complete list of batteries from the database.
        /// </summary>
        public async Task<List<Battery>> GetBatteryListAsync(string Name)
        {
            List<Battery> batteries = new List<Battery>();

            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);
            batteries = await App.Repository.Batteries.GetAsync(Name) as List<Battery>;
            if (batteries == null)
            {
                return null;
            }
            return batteries;
        }

        /// <summary>
        /// Gets the complete list of tests from the database.
        /// </summary>
        public async Task GetTestListAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);
            try
            {
                var tests = await App.Repository.Tests.GetAsync();
                if (tests.Count() == 0)
                {
                    await GenerateMockTestDatas().ContinueWith(
                    delegate { GetTestListAsync(); }).ContinueWith(
                    delegate { GetBatteryListAsync(); });
                }
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Tests.Clear();

                    var defaultTest = new TestViewModel();
                    Tests.Add(defaultTest);

                    foreach (var c in tests)
                    {
                        Tests.Add(new TestViewModel(c));
                    }
                    IsLoading = false;
                });
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Gets the complete list of tests from the database with battery id
        /// </summary>
        public async Task GetTestListAsync(Guid batteryId)
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);
            try
            {
                var tests = await App.Repository.Tests.GetAsyncByBattery(batteryId);
                if (tests.Count() == 0)
                {
                    return;
                }
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Tests.Clear();

                    var defaultTest = new TestViewModel();
                    Tests.Add(defaultTest);

                    foreach (var c in tests)
                    {
                        Tests.Add(new TestViewModel(c));
                    }
                    IsLoading = false;
                });
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }

            
        }

        /// <summary>
        /// Gets the complete list of tests from the database with battery id
        /// </summary>
        public async Task GetTestListAsync(DateTime start, DateTime end)
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);
            try
            {
                var tests = await App.Repository.Tests.GetAsyncByDate(start, end);
                if (tests == null)
                {
                    return;
                }
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Tests.Clear();

                    var defaultTest = new TestViewModel();
                    Tests.Add(defaultTest);

                    foreach (var c in tests)
                    {
                        Tests.Add(new TestViewModel(c));
                    }
                    IsLoading = false;
                });
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }

        public void GetTestData()
        {
            DateTime _now = DateTime.Now;
            WebClient webClient = new WebClient();
            string csvFilePath = System.IO.Path.GetTempPath() + @"text.csv";
            webClient.DownloadFileCompleted += (sender, e) => Console.WriteLine("Finished");
            Uri _downloadURI = new Uri(@"http://mirrorpark.cf/downloadTest");
            webClient.DownloadFile(_downloadURI, csvFilePath);

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = false;
                var _records = csv.GetRecords<TestData>();
                CurrentTestDatas.Clear();
                foreach (var row in _records)
                {
                    CurrentTestDatas.Add(row);
                }
            }

        }

        /// <summary>
        /// Generate mock test datas for first start
        /// </summary>
        public async Task GenerateMockTestDatas()
        {
            if (Tests.Count != 0 && Batteries.Count != 0)
            {
                return;
            }

            // Generate Mock Battery Informations
            Battery _mockBattery1 = new Battery();
            Battery _mockBattery2 = new Battery();

            BatteryViewModel _mockBatteryViewModel1 = new BatteryViewModel(_mockBattery1);
            BatteryViewModel _mockBatteryViewModel2 = new BatteryViewModel(_mockBattery2);

            _mockBatteryViewModel1.Name = "Samsung INR18650-20R";
            _mockBatteryViewModel1.Capacity = 2000;
            _mockBatteryViewModel1.Voltage = (float)3.6;
            _mockBatteryViewModel1.InitInternalImpedance = 18;
            _mockBatteryViewModel1.NominalCycleLife = 250;
            await _mockBatteryViewModel1.SaveBatteryAsync();

            _mockBatteryViewModel2.Name = "Sony Ericsson BST-33";
            _mockBatteryViewModel2.Capacity = 950;
            _mockBatteryViewModel2.Voltage = (float)3.6;
            await _mockBatteryViewModel2.SaveBatteryAsync();


            // Generate Mock Test Datas
            Random rand = new Random();

            string[] testers = { "Eric Fiore", "Sara Dominguez","Dana Golden","Colin Falls" };
            BatteryViewModel[] batteries = { _mockBatteryViewModel1, _mockBatteryViewModel2 };
            string[] offset = { "+", "-" };
            
            for (int i = 0; i < 5000; i++)
            {
                Test _newTest = new Test();
                TestViewModel _newTestViewModel = new TestViewModel(_newTest);

                int offsetIndex = rand.Next(offset.Length);
                int batteryIndex = rand.Next(batteries.Length);
                int testerIndex = rand.Next(testers.Length);

                _newTestViewModel.IsNewTest = true;
                _newTestViewModel.BatteryId = batteries[batteryIndex].Id;
                _newTestViewModel.BatteryName = batteries[batteryIndex].Name;
                _newTestViewModel.PulseAlgorithm = (PulseAlgorithm)rand.Next(0,10);
                _newTestViewModel.TesterName = testers[testerIndex];
                _newTestViewModel.Cycle = batteries[batteryIndex].PerformedCycle;
                batteries[batteryIndex].PerformedCycle++;
                _newTestViewModel.FullChargedMin = new TimeSpan(6000000000);
                _newTestViewModel.AverageEnvTemperature = (float)(rand.Next(200, 300)/10.0);
                _newTestViewModel.ChangedCapacity = (float)(rand.Next(1, 10) / 20.0); 
                _newTestViewModel.DOD = 95;
                _newTestViewModel.Offset = offset[offsetIndex];

                await _newTestViewModel.SaveTestAsync();
            }

        }
    }
    public class TestData
    {
        [Index(0)]
        public float Current { get; set; }

        [Index(1)]
        public float Volt { get; set; }

        [Index(2)]
        public float Temperature { get; set; }
    }
}
