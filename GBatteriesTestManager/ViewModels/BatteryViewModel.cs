using GBatteriesTestManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBatteriesTestManager.ViewModels
{
    public class BatteryViewModel : BindableBase
    {
        private Battery _battery;

        /// <summary>
        /// Initializes a new instance of the TodoViewModel class that wraps a Battery object.
        /// </summary>
        public BatteryViewModel(Battery battery = null)
        {
            Battery = battery ?? new Battery();
        }

        /// <summary>
        /// Gets or sets the underlying Battery object.
        /// </summary>
        public Battery Battery
        {
            get => _battery;
            set
            {
                if (_battery != value)
                {
                    _battery = value;
                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the battery's name.
        /// </summary>
        public Guid Id
        {
            get => Battery.Id;
        }

        /// <summary>
        /// Gets or sets the battery's name.
        /// </summary>
        public string Name
        {
            get => Battery.Name;
            set
            {
                if (value != Battery.Name)
                {
                    Battery.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the battery's capacity.
        /// </summary>
        public int Capacity
        {
            get => Battery.Capacity;
            set
            {
                if (value != Battery.Capacity)
                {
                    Battery.Capacity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Watt));
                }
            }
        }

        /// <summary>
        /// Gets or sets the battery's voltage.
        /// </summary>
        public float Voltage
        {
            get => Battery.Voltage;
            set
            {
                if (value != Battery.Voltage)
                {
                    Battery.Voltage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Watt));
                }
            }
        }
        /// <summary>
        /// Gets or sets the battery's watt amount.
        /// </summary>
        public float Watt
        {
            get => Capacity * Voltage * 36;
        }
        /// <summary>
        /// Gets or sets the battery's init internal impedance.
        /// </summary>
        public int InitInternalImpedance
        {
            get => Battery.InitInternalImpedance;
            set
            {
                if (value != Battery.InitInternalImpedance)
                {
                    Battery.InitInternalImpedance = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the battery's nominal cycle life.
        /// </summary>
        public int NominalCycleLife
        {
            get => Battery.NominalCycleLife;
            set
            {
                if (value != Battery.NominalCycleLife)
                {
                    Battery.NominalCycleLife = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the battery's performed cycle.
        /// </summary>
        public int PerformedCycle
        {
            get => Battery.PerformedCycle;
            set
            {
                if (value != Battery.PerformedCycle)
                {
                    Battery.PerformedCycle = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool _isNewBattery;
        /// <summary>
        /// Gets or sets a value that indicates whether this is a new Battery.
        /// </summary>
        public bool IsNewBattery
        {
            get => _isNewBattery;
            set => Set(ref _isNewBattery, value);
        }



        /// <summary>
        /// Saves battery data that has been edited.
        /// </summary>
        public async Task SaveBatteryAsync()
        {
            if (IsNewBattery)
            {
                App.ViewModel.Batteries.Add(this);

                if (App.ViewModel.BatteryVariations.Contains(Name) != true)
                {
                    App.ViewModel.BatteryVariations.Add(Name);
                }
                IsNewBattery = false;
            }

            await App.Repository.Batteries.UpsertAsync(Battery);
        }
    }
}
