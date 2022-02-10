namespace Grade.Models.Dto
{
    public abstract class SectionDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public bool Active { get; set; }

        public ResourceDto ImageResource { get; set; }

        public int[] PresentersId { get; set; }


    }
}
