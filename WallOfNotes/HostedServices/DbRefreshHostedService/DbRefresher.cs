using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WallOfNotes.Models;

namespace WallOfNotes.HostedServices.DbRefreshHostedService
{
    public class DbRefresher
    {
        private static readonly List<string> s_names = new List<string> {
          "John",
          "Jane",
          "Michael",
          "Robert",
          "Keisha",
          "Asia",
          "Gerard",
          "Jacob",
          "Nicole",
          "Sasha"
        };
        private static readonly char[] s_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$!%?#@".ToCharArray();
        private IServiceScopeFactory scopeFactory;
        private IServiceScope scope;
        private NoteContext noteContext;
        private Random random;

        public DbRefresher()
        {
        }

        public void Initialize(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            scope = this.scopeFactory.CreateScope();
            noteContext = scope.ServiceProvider.GetRequiredService<NoteContext>();
            random = new Random();
        }

        public async void RefreshDb(object state)
        {
            await DeleteNotes();
            await SeedNotes();
        }

        // synchronous because DB must be cleared before getting populated
        private async Task DeleteNotes()
        {
            await noteContext.Database.ExecuteSqlRawAsync("DELETE FROM Notes");
            await noteContext.SaveChangesAsync();
        }

        // generate 10 random posts and insert into db
        private async Task SeedNotes()
        {
            List<Note> notes = await GetNotes();
            await noteContext.Notes.AddRangeAsync(notes);
            await noteContext.SaveChangesAsync();
        }

        private async Task<List<Note>> GetNotes()
        {
            List<string> quotes = await QuoteGenerator.fetchMessages(10);
            List<string> authors = GenerateAuthors(quotes.Count);
            return Enumerable
                .Range(0, quotes.Count)
                .Select(i => new Note(authors[i], quotes[i]))
                .ToList();
        }

        private List<string> GenerateAuthors(int numAuthors)
        {
            List<string> authors = new List<string>();
            char[] randomChars = new char[3];
            int randomIndex;
            for (int i = 0; i < numAuthors; i++)
            {
                for (int j = 0; j < randomChars.Length; j++)
                {
                    randomIndex = random.Next(s_chars.Length);
                    randomChars[j] = s_chars[randomIndex];
                }
                randomIndex = random.Next(s_names.Count);
                // Ex: RobertC%W
                authors.Add(s_names[randomIndex] + new String(randomChars));
            }
            return authors;
        }

        public void Terminate()
        {
            noteContext.Dispose();
            scope.Dispose();
        }
    }
}
