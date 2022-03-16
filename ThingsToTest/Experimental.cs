using System;
using System.IO;
using System.Threading.Tasks;
using ApplicationExceptions;
using ThingsToRemember.Models;
using Xunit;
using ThingsToRemember.Services;
using Xunit.Abstractions;

namespace ThingsToTest
{
    /// <summary>
    /// This is for trying things out or exploring how to do things
    /// </summary>
    public class Experimental : IClassFixture<TestsFixture>, IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private static   Database          _database;
        private static readonly string     DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                                                                      , "WorkoutDatabase_test.db3");
        public static Database Database { get; set; }//=> _database ?? new Database(DbPath);

        public Experimental(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _testOutputHelper.WriteLine($"Database path: {DbPath}");
            
            Database = _database ?? new Database(DbPath);
            
            Database.DropUserTables();
            Database.DropAppTables();
            
            Database.CreateUserTables();
            Database.CreateAppTables();
            
            
        }

        [Fact]
        public void AddEditDeleteJournal()
        {
            //Add
            var journal = new Journal()
                          {
                              Title = "Test Journal"
                          };

            Database.AddJournal(journal);

            var newJournal = Database.GetJournal(journal.Id);

            Assert.Equal("Test Journal", newJournal.Title);

            //Edit
            journal.Title = "New title";

            Database.UpdateJournal(journal);

            var updatedJournal = Database.GetJournal(journal.Id);

            Assert.Equal("New title", updatedJournal.Title);

            //Delete
            
            Database.DeleteJournal(ref updatedJournal);

            Assert.Throws<NullReferenceException>( () => Database.GetJournal(updatedJournal.Id));
        }

        [Fact]
        public async Task AddEditDeleteJournalAndJournalTYpe()
        {
            var journal = new Journal()
                          {
                              Title = "Test Journal"
                          };

            var journalType = new JournalType()
                              {
                                  Title = "Test Type"
                              };

            Database.AddJournal(journal);
            Database.AddJournalType(journalType);

            journal.JournalType = journalType;
            Database.UpdateJournal(journal);

            var final = await Task.FromResult(true);
            Assert.True(final);
        }

        [Fact]
        public void GetAllJournals()
        {
            var journals = Database.GetJournals();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_database != null)
                {
                    _database.DropUserTables();                    
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Experimental()
        {
            Dispose(false);
        }
    }
    
}
