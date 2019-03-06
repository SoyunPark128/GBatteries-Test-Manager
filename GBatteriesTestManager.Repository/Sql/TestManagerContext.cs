using GBatteriesTestManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBatteriesTestManager.Repository.Sql
{
    /// <summary>
    /// Entity Framework Core DbContext for GBatteries Test Manager.
    /// </summary>
    public class TestManagerContext : DbContext
    {

        /// <summary>
        /// Creates a new Test Manager DbContext.
        /// </summary>
        public TestManagerContext(DbContextOptions<TestManagerContext> options) : base(options)
        { }

        /// <summary>
        /// Gets the todos DbSet.
        /// </summary>
        public DbSet<Battery> Batteries { get; set; }

        /// <summary>
        /// Gets the test DbSet.
        /// </summary>
        public DbSet<Test> Tests { get; set; }
    }
}
