using GBatteriesTestManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GBatteriesTestManager.Repository
{
    public interface ITestRepository
    {
        /// <summary>
        /// Returns all Tests. 
        /// </summary>
        Task<IEnumerable<Test>> GetAsync();

        /// <summary>
        /// Returns the Test with the given id. 
        /// </summary>
        Task<Test> GetAsync(Guid id);


        /// <summary>
        /// Returns the Test with the given BatteryId. 
        /// </summary>
        Task<IEnumerable<Test>> GetAsyncByBattery(Guid batteryId);


        /// <summary>
        /// Returns the Test with the given Date. 
        /// </summary>
        Task<IEnumerable<Test>> GetAsyncByDate(DateTime testDate);
        Task<IEnumerable<Test>> GetAsyncByDate(DateTime testDayStart, DateTime testDayEnd);

        /// <summary>
        /// Adds a new Test if the Test does not exist, updates the 
        /// existing Test otherwise.
        /// </summary>
        Task<Test> UpsertAsync(Test test);

        /// <summary>
        /// Deletes a Test.
        /// </summary>
        Task DeleteAsync(Guid testId);
    }
}
