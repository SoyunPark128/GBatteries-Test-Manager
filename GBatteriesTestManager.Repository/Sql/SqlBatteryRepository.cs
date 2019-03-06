using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GBatteriesTestManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GBatteriesTestManager.Repository.Sql
{
    public class SqlBatteryRepository : IBatteryRepository
    {
        private readonly TestManagerContext _db;

        public SqlBatteryRepository(TestManagerContext db)
        {
            _db = db;
        }
        
        public async Task<IEnumerable<Battery>> GetAsync()
        {
            return await _db.Batteries
                .AsNoTracking()
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Battery>> GetAsync(string name)
        {
            return await _db.Batteries
                .AsNoTracking()
                .Where(b => b.Name == name)
                .ToListAsync();
        }

        public async Task<Battery> GetAsync(Guid id)
        {
            return await _db.Batteries
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Battery> UpsertAsync(Battery battery)
        {
            var current = await _db.Batteries.FirstOrDefaultAsync(b => b.Id == battery.Id);
            if (null == current)
            {
                _db.Batteries.Add(battery);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(battery);
            }
            await _db.SaveChangesAsync();
            return battery;
        }

        public async Task DeleteAsync(Guid batteryId)
        {
            var battery = await _db.Batteries.FirstOrDefaultAsync(b => b.Id == batteryId);
            if (null != battery)
            {
                _db.Batteries.Remove(battery);
                await _db.SaveChangesAsync();
            }
        }
    }
}
