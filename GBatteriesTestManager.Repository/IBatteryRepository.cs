using GBatteriesTestManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GBatteriesTestManager.Repository
{
    public interface IBatteryRepository
    {
        /// <summary>
        /// Returns all Batteries. 
        /// </summary>
        Task<IEnumerable<Battery>> GetAsync();

        /// <summary>
        /// Returns all Batteries with a data field matching the start of the given name. 
        /// </summary>
        Task<IEnumerable<Battery>> GetAsync(string name);

        /// <summary>
        /// Returns the Batteries with the given id. 
        /// </summary>
        Task<Battery> GetAsync(Guid id);

        /// <summary>
        /// Adds a new Battery if the Todo does not exist, updates the 
        /// existing Battery otherwise.
        /// </summary>
        Task<Battery> UpsertAsync(Battery battery);

        /// <summary>
        /// Deletes a Battery.
        /// </summary>
        Task DeleteAsync(Guid batteryId);
    }
}
