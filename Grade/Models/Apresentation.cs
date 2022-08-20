namespace Grade.Models
{

    public class Apresentation : IId
    {
        public int Id { get; set; }
        public int PresenterId { get; set; }
        public int SectionId { get; set; }

        public Presenter Presenter { get; set; }
        public Section Section { get; set; }

        public static Apresentation[] CreateObjects(int[] presenterIds, int sectionId)
        {
            var apresentations = new Apresentation[presenterIds.Length];

            for (int i = 0; i < apresentations.Length; i++)
            {
                apresentations[i] = new Apresentation()
                {
                    PresenterId = presenterIds[i],
                    SectionId = sectionId
                };
            }

            return apresentations;
        }
        
    }
}
