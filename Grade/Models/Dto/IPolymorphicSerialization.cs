namespace Grade.Models.Dto
{
    public interface IPolymorphicSerialization<T> where T : Enum
    {
        
        public T DerivatedBy { get; set; }
    }
}
