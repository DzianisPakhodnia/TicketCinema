namespace TicketCinema.Contracts
{
    public record GetNotesResponse(List<NoteDTO> notes);
    public record NoteDTO(Guid Id,string Title, string Description, DateTime CreatedAt);

}
