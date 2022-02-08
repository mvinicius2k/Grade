namespace Grade.Models
{

    public class Apresentation
    {
        public int Id { get; set; }
        public int PresenterId { get; set; }
        public int SectionId { get; set; }

        public Presenter Presenter { get; set; }
        public Section Section { get; set; }

        
    }
}
