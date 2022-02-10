namespace Grade.Models.Dto
{
    public class PresenterDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public ResourceDto ImageResource { get; set; }

        public int[] SectionsId { get; set; }
        
    }
}
