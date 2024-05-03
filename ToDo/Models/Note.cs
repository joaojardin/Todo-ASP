using System;
using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class Note
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public bool Completed { get; set; }

        public string Category { get; set; }

        public PriorityLevel Priority { get; set; }

        public Note()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            Completed = false;
        }
    }

    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }
}
