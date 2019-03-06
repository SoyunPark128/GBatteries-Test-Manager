using GBatteriesTestManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBatteriesTestManager.ViewModels
{
    public class TestViewModel : BindableBase
    {
        private Test _test;

        /// <summary>
        /// Initializes a new instance of the TodoViewModel class that wraps a Todo object.
        /// </summary>
        public TestViewModel(Test test = null)
        {
            Test = test ?? new Test();
        }

        /// <summary>
        /// Gets or sets the underlying Test object.
        /// </summary>
        public Test Test
        {
            get => _test;
            set
            {
                if (_test != value)
                {
                    _test = value;
                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the tested battery's Id.
        /// </summary>
        public Guid BatteryId
        {
            get => Test.BatteryId;
            set
            {
                if (value != Test.BatteryId)
                {
                    Test.BatteryId = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the tested battery's name.
        /// </summary>
        public string BatteryName
        {
            get => Test.BatteryName;
            set
            {
                if (value != Test.BatteryName)
                {
                    Test.BatteryName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the test's name.
        /// </summary>
        public string TesterName
        {
            get => Test.TesterName;
            set
            {
                if (value != Test.TesterName)
                {
                    Test.TesterName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the date of test.
        /// </summary>
        public DateTime TestDate
        {
            get => Test.TestDate;
            set
            {
                if (value != Test.TestDate)
                {
                    Test.TestDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TestEnvDescription
        {
            get => String.Format($"DOD : {DOD.ToString()}%, {AverageEnvTemperature}℃");
        }

        public string TestResultDescription
        {
            get => String.Format($"Battery : {BatteryName}({Cycle.ToString()}Cycles)"+
                $"\nTime to full charge :{String.Format("{0:00}:{1:00}:{2:00}.{3:00}", FullChargedMin.Hours, FullChargedMin.Minutes, FullChargedMin.Seconds, FullChargedMin.Milliseconds / 10)}" +
                $"\nChanged Capacity : {Offset}{ChangedCapacity.ToString()}");
        }

        public string TestSetupDescription
        {
            get => String.Format($"Fast Charge Type : {FastChargeType.ToString()} {PulseAlgorithm.ToString()}");
        }

        /// <summary>
        /// Gets or sets the test's average temperature of environment.
        /// </summary>
        public float AverageEnvTemperature
        {
            get => Test.AverageEnvTemperature;
            set
            {
                if (value != Test.AverageEnvTemperature)
                {
                    Test.AverageEnvTemperature = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the test's fast charge type.
        /// </summary>
        public FastChargeType FastChargeType
        {
            get => Test.FastChargeType;
            set
            {
                if (value != Test.FastChargeType)
                {
                    Test.FastChargeType = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the test's pulse algorithm.
        /// </summary>
        public PulseAlgorithm PulseAlgorithm
        {
            get => Test.PulseAlgorithm;
            set
            {
                if (value != Test.PulseAlgorithm)
                {
                    Test.PulseAlgorithm = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the test's performed cycle.
        /// </summary>
        public int Cycle
        {
            get => Test.Cycle;
            set
            {
                if (value != Test.Cycle)
                {
                    Test.Cycle = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the test's taken minutes for full charge.
        /// </summary>
        public TimeSpan FullChargedMin
        {
            get => Test.FullChargedMin;
            set
            {
                if (value != Test.FullChargedMin)
                {
                    Test.FullChargedMin = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the test's changed offset.
        /// </summary>
        public string Offset
        {
            get => Test.Offset;
            set
            {
                if (value != Test.Offset)
                {
                    Test.Offset = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the test's changed capacity.
        /// </summary>
        public float ChangedCapacity
        {
            get => Test.ChangedCapacity;
            set
            {
                if (value != Test.ChangedCapacity)
                {
                    Test.ChangedCapacity = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
         /// Gets or sets the test's Depth of Discharge.
         /// </summary>
        public int DOD
        {
            get => Test.DOD;
            set
            {
                if (value != Test.DOD)
                {
                    Test.DOD = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isNewTest;
        /// <summary>
        /// Gets or sets a value that indicates whether this is a new Test.
        /// </summary>
        public bool IsNewTest
        {
            get => _isNewTest;
            set => Set(ref _isNewTest, value);
        }

        /// <summary>
        /// Saves test data that has been edited.
        /// </summary>
        public async Task SaveTestAsync()
        {
            if (IsNewTest)
            {
                App.ViewModel.Tests.Insert(0, this);
                IsNewTest = false;
            }

            await App.Repository.Tests.UpsertAsync(Test);
        }

        
    }
}
