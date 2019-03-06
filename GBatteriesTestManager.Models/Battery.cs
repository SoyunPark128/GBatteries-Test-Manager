using System;
using System.ComponentModel.DataAnnotations;

namespace GBatteriesTestManager.Models
{
    public class Battery
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public float Voltage { get; set; }
        public int InitInternalImpedance { get; set; }
        public int NominalCycleLife { get; set; }
        public int PerformedCycle { get; set; } = 0;
    }
}
