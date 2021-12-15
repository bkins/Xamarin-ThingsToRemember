using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ApplicationExceptions;
using ThingsToRemember.Models;
using ThingsToRemember.Services;

namespace ThingsToRemember.ViewModels
{
    public class JournalViewModel : BaseViewModel
    {
        public Journal Journal { get; set; }

        public JournalViewModel(int id)
        {
            Journal = DataAccessLayer.GetJournal(id);
        }

        public JournalViewModel(string journalTitle
                              , string journalTypeTitle)
        {
            var journalType = ValidateJournalType(journalTypeTitle);

            Journal = new Journal
                      {
                          Title       = journalTitle
                        , JournalType = journalType
                      };
        }

        public void Save()
        {
            if (Journal.Id == 0)
            {
                var journals = DataAccessLayer.GetJournals(); //App.Database.GetJournals();
                var journalsAlreadyExists = journals.Any(fields => fields.Title          == Journal.Title 
                                                                && fields.JournalType.Id == Journal.JournalType.Id);

                if (journalsAlreadyExists)
                {
                    throw new DuplicateRecordException(nameof(Journal)
                                                     , Journal.Title);
                }
                DataAccessLayer.AddJournalWithChildren(Journal);
                //App.Database.AddJournalWithChildren(Journal);
            }
            else
            {
                DataAccessLayer.UpdateJournal(Journal);
                //App.Database.UpdateJournal(Journal);
            }
        }

        private JournalType ValidateJournalType(string journalTypeTitle)
        {
            var journalType = DataAccessLayer.GetJournalTypesList().FirstOrDefault(fields => fields.Title == journalTypeTitle);
            //var journalType_ = App.Database.GetJournalTypes()
            //                     .ToList()
            //                     .FirstOrDefault(fields => fields.Title == journalTypeTitle);

            if (journalType is null)
            {
                throw new RecordNotFoundException(nameof(JournalType)
                                                , journalTypeTitle);
            }

            return journalType;
        }
    }
}
