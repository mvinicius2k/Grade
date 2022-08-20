using Grade.Models;
using Microsoft.EntityFrameworkCore;

namespace Grade.Helpers
{
    public static class Extensions
    {

        public static IEnumerable<T> Paginate<T>(this IQueryable<T> query, int lastId, int pageSize) where T : IId
        =>
            query.OrderBy(x => x.Id)
            .Where(x => x.Id == lastId)
            .Take(pageSize);
    }
}
