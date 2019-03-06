using GBatteriesTestManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBatteriesTestManager.Repository.Sql
{
    public class SqlTestRepository : ITestRepository
    {
        private readonly TestManagerContext _db;

        public SqlTestRepository(TestManagerContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Test>> GetAsync()
        {
            return await _db.Tests
                .AsNoTracking()
                .OrderByDescending(tr => tr.TestDate)
                .ToListAsync();
        }

        public async Task<Test> GetAsync(Guid id)
        {
            return await _db.Tests
                .AsNoTracking()
                .OrderByDescending(tr => tr.TestDate)
                .FirstOrDefaultAsync(test => test.Id == id);
        }


        public async Task<IEnumerable<Test>> GetAsyncByTest(Guid testId)
        {
            return await _db.Tests
                .AsNoTracking()
                .Where(tr => tr.Id == testId)
                .OrderByDescending(tr => tr.TestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Test>> GetAsyncByBattery(Guid batteryId)
        {
            return await _db.Tests
               .AsNoTracking()
               .Where(tr => tr.BatteryId == batteryId)
               .OrderByDescending(tr => tr.TestDate)
               .ToListAsync();
        }

        public async Task<IEnumerable<Test>> GetAsyncByDate(DateTime testDate)
        {
            return await _db.Tests
                .AsNoTracking()
                .Where(tr => tr.TestDate == testDate)
                .OrderByDescending(tr => tr.TestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Test>> GetAsyncByDate(DateTime testDayStart, DateTime testDayEnd)
        {
            return await _db.Tests
                .AsNoTracking()
                .Where(tr => tr.TestDate > testDayStart && tr.TestDate <= testDayEnd)
                .OrderByDescending(tr => tr.TestDate)
                .ToListAsync();
        }

        public async Task<Test> UpsertAsync(Test test)
        {
            var current = await _db.Tests.FirstOrDefaultAsync(_test => _test.Id == test.Id);
            if (null == current)
            {
                _db.Tests.Add(test);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(test);
            }
            await _db.SaveChangesAsync();
            return test;
        }

        public async Task DeleteAsync(Guid id)
        {
            var test = await _db.Tests.FirstOrDefaultAsync(_test => _test.Id == id);
            if (null != test)
            {
                _db.Tests.Remove(test);
                await _db.SaveChangesAsync();
            }
        }

    }
}
