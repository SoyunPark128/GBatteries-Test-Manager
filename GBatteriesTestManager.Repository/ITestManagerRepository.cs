using System;

namespace GBatteriesTestManager.Repository
{
    public interface ITestManagerRepository
    {
        /// <summary>
        /// Returns the Batteries repository.
        /// </summary>
        IBatteryRepository Batteries { get; }

        /// <summary>
        /// Returns the Tests repository.
        /// </summary>
        ITestRepository Tests { get; }
    }
}
