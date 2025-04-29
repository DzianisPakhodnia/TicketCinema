namespace TicketCinema.Models
{
    public class Note
    {
        public Note(string title, string description)
        {
            Title = title;
            Description = description;
            CreatedAt = DateTime.Now;


        }

        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public DateTime CreatedAt { get; init; }

    }
}
