using System;

namespace WallOfNotes.Models
{
    public class Note
    {
        public Note()
        {
            CreatedAt = DateTime.Now.Ticks;
        }

        public Note(string author, string msg)
        {
            Name = author;
            Message = msg;
            CreatedAt = DateTime.Now.Ticks;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public long CreatedAt { get; set; }
    }
}
