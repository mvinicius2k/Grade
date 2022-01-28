namespace Grade.Models
{
    public interface IResolveTypesPgsql
    {
        /// <summary>
        /// Converterá os campos do objeto para tipos mapeáveis ao Pgsql.
        /// </summary>
        public void ResolveTypesPgsql();
    }
}
