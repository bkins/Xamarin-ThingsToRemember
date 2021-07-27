﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationExceptions;
using ThingsToRemember.Models;
using Xunit;
using ThingsToRemember.Services;
using Xunit.Abstractions;

namespace ThingsToTest
{
    public class EndToEnds
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private static   Database          _database;
        private static readonly string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                                                           , "WorkoutDatabase_test.db3");
        public static Database Database => _database ??= new Database(DbPath);

        public EndToEnds(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _testOutputHelper.WriteLine($"Database path: {DbPath}");
        }

        /// <summary>
        /// Test of all elements of a journal: JournalType, Entry: Entry's mood, and all properties of the journal.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void FullTestOfJournal()
        {
            //Start with fresh DB
            Database.DropTables();
            Database.CreateTables();

            //Arrange
            var expectedJournalTypeTitle = "Test Journal Type";
            var expectedMoodHappy        = "Happy";
            var expectedMoodSad          = "Sad";
            var expectedMoodMad          = "Mad";
            var expectedMoodMeh          = "Meh";
            var expectedEntry1Title      = "Entry 1";
            var expectedEntry2Title      = "Entry 2";
            var expectedEntry3Title      = "Entry 3";
            var expectedEntryNTitle      = "Entry n";
            var expectedJournalTitle     = "Test Journal";

            var journalType = new JournalType()
                              {
                                  Title = expectedJournalTypeTitle
                              };

            var moodHappy = new Mood()
                            {
                                Title = expectedMoodHappy
                              , Emoji = ":)"
                            };
            
            var moodSad = new Mood()
                          {
                              Title = expectedMoodSad
                            , Emoji = ":("
                          };
            
            var moodMad = new Mood()
                          {
                              Title = expectedMoodMad
                            , Emoji = ">:("
                          };
            
            var moodMeh = new Mood()
                          {
                              Title = expectedMoodMeh
                            , Emoji = ":|"
                          };

            var entry1 = new Entry()
                         {
                             Title          = expectedEntry1Title
                           , Text           = "Test entry"
                           , CreateDateTime = DateTime.Now
                           , EntryMood      = moodHappy
                         };

            var entry2 = new Entry()
                         {
                             Title          = expectedEntry2Title
                           , CreateDateTime = DateTime.Now
                           , EntryMood      = moodSad
                         };

            var entry3 = new Entry()
                         {
                             Title          = expectedEntry3Title
                           , CreateDateTime = DateTime.Now
                           , EntryMood      = moodMad
                         };
            
            var entries = new List<Entry>
                          {
                              entry1
                            , entry2
                            , entry3
                          };

            var journal = new Journal()
                          {
                              Title       = expectedJournalTitle
                            , JournalType = journalType
                            , Entries     = entries
                          };

            var lastMinuteEntry = new Entry()
                                  {
                                      Title          = expectedEntryNTitle
                                    , Text           = "Entry to be added to a journal that has already been add to the DB."
                                    , CreateDateTime = DateTime.Now
                                    , EntryMood      = moodMeh
                                  };
            
            //Act: Add

            Database.AddJournalWithChildren(journal);

            //journal.Entries.Add(lastMinuteEntry);
            Database.AddEntryWIthChildren(lastMinuteEntry, journal.Id);

            Database.UpdateJournal(journal);

            var newJournal = Database.GetJournal(journal.Id);

            //Assert: Add
            Assert.True(newJournal.Entries.Any());

            Assert.Equal(expectedJournalTitle, newJournal.Title);

            Assert.Equal(expectedJournalTypeTitle, journal.JournalType.Title);
            Assert.Contains(journal.Entries, item => item.Title           == expectedEntry1Title);
            Assert.Contains(journal.Entries, item => item.Title           == expectedEntry2Title);
            Assert.Contains(journal.Entries, item => item.Title           == expectedEntry3Title);
            Assert.Contains(journal.Entries, item => item.Title           == expectedEntryNTitle);
            Assert.Contains(journal.Entries, item => item.EntryMood.Title == expectedMoodHappy);
            Assert.Contains(journal.Entries, item => item.EntryMood.Title == expectedMoodSad);
            Assert.Contains(journal.Entries, item => item.EntryMood.Title == expectedMoodMad);
            
            Assert.Equal(expectedJournalTypeTitle, newJournal.JournalType.Title);
            Assert.Contains(newJournal.Entries, item => item.Title           == expectedEntry1Title);
            Assert.Contains(newJournal.Entries, item => item.Title           == expectedEntry2Title);
            Assert.Contains(newJournal.Entries, item => item.Title           == expectedEntry3Title);
            Assert.Contains(newJournal.Entries, item => item.Title           == expectedEntryNTitle);
            Assert.Contains(newJournal.Entries, item => item.EntryMood.Title == expectedMoodHappy);
            Assert.Contains(newJournal.Entries, item => item.EntryMood.Title == expectedMoodSad);
            Assert.Contains(newJournal.Entries, item => item.EntryMood.Title == expectedMoodMad);

            //Act: Edit
            journal.Title = "New title";

            Database.UpdateJournal(journal);

            Database.GetJournal(journal.Id);

            //Assert: Edit
            Assert.Equal("New title", journal.Title);

            //Act: Get

            var actualEntries = Database.GetEntries().ToList();
            var actualEntry   = Database.GetEntry(1);

            //Assert: Get (Entries)

            Assert.True(actualEntries != null && actualEntries.Any());

            Assert.Contains(actualEntries, item => item.Title           == expectedEntry1Title);
            Assert.Contains(actualEntries, item => item.Title           == expectedEntry2Title);
            Assert.Contains(actualEntries, item => item.Title           == expectedEntry3Title);
            Assert.Contains(actualEntries, item => item.Title           == expectedEntryNTitle);
            Assert.Contains(actualEntries, item => item.EntryMood.Title == expectedMoodHappy);
            Assert.Contains(actualEntries, item => item.EntryMood.Title == expectedMoodSad);
            Assert.Contains(actualEntries, item => item.EntryMood.Title == expectedMoodMad);
            
            //Assert: Get (Entry)
            Assert.True(actualEntry.Text == expectedEntry1Title);
            Assert.True(actualEntry.EntryMood.Title == expectedMoodHappy);

            //Act: Delete
            
            Database.DeleteJournal(journal);

            //Try to get the delete journal
            //Database.GetJournal(journal.Id);
            Assert.Throws<SequenceContainsNoElementsException>( () =>  Database.GetJournal(journal.Id));

            //Assert that everything that was supposed to be deleted was, and what wasn't, wasn't

            var journalTypes = Database.GetJournalTypes();
            
            Assert.True(journalTypes != null && journalTypes.Any());

            var allEntries = Database.GetEntries();

            Assert.True(allEntries == null || ! allEntries.Any());

        }

    }
}
