using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lucene.Net.Search.FieldValueHitQueue;

namespace Core.Context
{
    public class BookEventDbContext:DbContext
    {
        public BookEventDbContext(DbContextOptions<BookEventDbContext> options) : base(options) { }

        public DbSet<BookedEvents> BookedEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // No explicit foreign key constraint to Umbraco's data store
            // You might add an index for UmbracoPageUdi for faster lookups
            modelBuilder.Entity<BookedEvents>()
                .HasIndex(e => e.EventId);
        }
    }
}
