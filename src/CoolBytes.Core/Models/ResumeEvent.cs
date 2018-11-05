using System;

namespace CoolBytes.Core.Models
{
    public class ResumeEvent
    {
        public int Id { get; private set; }
        public Author Author { get; private set; }
        public int AuthorId { get; set; }
        public DateRange DateRange { get; private set; }
        public string Name { get; private set; }
        public string Message { get; private set; }

        public ResumeEvent(Author author, DateRange dateRange, string name, string message)
        {
            Author = author ?? throw new ArgumentNullException(nameof(author));

            Validate(dateRange, name, message);
        }

        public void Update(string name, string message)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        private void Validate(DateRange dateRange, string name, string message)
        {
            DateRange = dateRange ?? throw new ArgumentNullException(nameof(dateRange));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        private ResumeEvent() { }
    }
}
