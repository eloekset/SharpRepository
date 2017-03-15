#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace SharpRepository.Tests.TestObjects
{
    public class TestObjectEntities : DbContext
    {
#if NETSTANDARD1_6
        public TestObjectEntities(DbContextOptions options) : base(options)
        {

        }
#else
        public TestObjectEntities(string connectionString) : base(connectionString)
        {
            
        }
#endif

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<TripleCompoundKeyItemInts> TripleCompoundKeyItems { get; set; }
    }
}
