using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolBytes.Core.Models
{
    public class Resume
    {
        public Author Author { get; }
        public IDictionary<string, IEnumerable<ResumeEvent>> ResumeEvents { get; private set; }

        public Resume(Author author, IEnumerable<ResumeEvent> resumeEvents)
        {
            Author = author ?? throw new ArgumentNullException(nameof(author));

            if (author.AuthorProfile == null)
                throw new ArgumentNullException(nameof(author.AuthorProfile));

            if (resumeEvents == null)
                throw new ArgumentNullException(nameof(resumeEvents));

            GroupResumeEvents(resumeEvents);
        }

        private void GroupResumeEvents(IEnumerable<ResumeEvent> resumeEvents)
        {
            var groups = resumeEvents.OrderByDescending(r => r.DateRange.StartDate).GroupBy(r => r.DateRange.StartDate.Year, (key, events) => events);

            ResumeEvents = new Dictionary<string, IEnumerable<ResumeEvent>>();

            foreach (var group in groups)
            {
                var collection = group.ToArray();
                var count = collection.Count();
                var resumeEvent = collection.First();

                if (count == 1 && resumeEvent.DateRange.StartDate.Year != resumeEvent.DateRange.EndDate.Year)
                {
                    ResumeEvents[$"{resumeEvent.DateRange.StartDate.Year} - {resumeEvent.DateRange.EndDate.Year}"] = collection;
                }
                else
                {
                    ResumeEvents[$"{resumeEvent.DateRange.StartDate.Year}"] = collection;
                }
            }
        }
    }
}