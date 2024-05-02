using System;
using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class Note
    {
        [Key]
        public Guid id { get; set; }

        public string content { get; set; }

        public DateTime date { get; set; }

        public bool completed { get; set; }

        public Note()
        {
            id = Guid.NewGuid();
            date = DateTime.Now;
            completed = false;
        }
    }
}
