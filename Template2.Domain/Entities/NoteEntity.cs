namespace Template2.Domain.Entities
{
    public class NoteEntity
    {
        public NoteEntity(string note)
        {
            Note = note;
        }
        public string Note { get; set; }

        public string Hint { get; set; } = "☞";
    }
}
