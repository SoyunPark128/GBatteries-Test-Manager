using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GBatteriesTestManager.Models
{
    public class Test
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid BatteryId { get; set; }
        public string BatteryName { get; set; }

        public string TesterName { get; set; }
        public DateTime TestDate { get; set; } = DateTime.Now;

        public float AverageEnvTemperature { get; set; }
        public FastChargeType FastChargeType { get; set; }
        public PulseAlgorithm PulseAlgorithm { get; set; }

        [Required]
        public int Cycle { get; set; }
        public TimeSpan FullChargedMin { get; set; }
        public string Offset { get; set; }
        public float ChangedCapacity { get; set; }
        public int DOD { get; set; }
    }

    public enum FastChargeType
    {
        GBatteries = 0,
        CCCV = 1
    }

    public enum PulseAlgorithm
    {
        Algorithm_01 = 0,
        Algorithm_02 = 1,
        Algorithm_03 = 2,
        Algorithm_04 = 3,
        Algorithm_05 = 4,
        Algorithm_06 = 5,
        Algorithm_07 = 6,
        Algorithm_08 = 7,
        Algorithm_09 = 8,
        Algorithm_10 = 9
    }

}
