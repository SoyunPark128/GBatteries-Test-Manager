using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBatteriesTestManager.Repository.Sql
{
    /// <summary>
    /// Contains methods for interacting with the app backend using 
    /// SQL via Entity Framework Core 2.2. 
    /// </summary>
    public class SqlTestManagerRepository : ITestManagerRepository
    {
        private readonly DbContextOptions<TestManagerContext> _dbOptions;

        public SqlTestManagerRepository(DbContextOptionsBuilder<TestManagerContext> dbOptionsBuilder)
        {
            _dbOptions = dbOptionsBuilder.Options;
            using (var db = new TestManagerContext(_dbOptions))
            {
                //db.Database.Migrate();
                db.Database.EnsureCreated();
            }
        }


        public IBatteryRepository Batteries => new SqlBatteryRepository(new TestManagerContext(_dbOptions));

        public ITestRepository Tests => new SqlTestRepository(new TestManagerContext(_dbOptions));
    }
}
