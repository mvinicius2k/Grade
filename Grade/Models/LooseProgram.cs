using System.ComponentModel.DataAnnotations;

namespace Grade.Models
{
    /// <summary>
    /// Programas avulsos serão sobrepostos aos programas semanais
    /// </summary>
    public class LooseProgram : ProgramBase, IResolveTypesPgsql
    {

        [DataType(DataType.DateTime)]
        public DateTime StartAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndAt { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// Todos os <see cref="DateTime"/> serão convertidos para UTC.
        /// </summary>
        public void ResolveTypesPgsql()
        {
            StartAt = StartAt.ToUniversalTime();
            EndAt = EndAt.ToUniversalTime();
        }
    }
}
