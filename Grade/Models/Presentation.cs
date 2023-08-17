namespace Grade.Models
{

    public class Presentation : IId
    {
        public int Id { get; set; }
        public int PresenterId { get; set; }
        public int SectionId { get; set; }

        public Presenter Presenter { get; set; }
        public Section Section { get; set; }

        public static Presentation[] CreateObjects(int[] presenterIds, int sectionId)
        {
            var presentations = new Presentation[presenterIds.Length];

            for (int i = 0; i < presentations.Length; i++)
            {
                presentations[i] = new Presentation()
                {
                    PresenterId = presenterIds[i],
                    SectionId = sectionId
                };
            }

            return presentations;
        }
        
    }
}
