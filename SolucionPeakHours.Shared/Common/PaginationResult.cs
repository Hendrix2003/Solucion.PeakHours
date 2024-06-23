using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucionPeakHours.Shared.Common
{
    public class PaginationResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public static class PaginatorExtensions
    {
        public static async Task<PaginationResult<T>> GetPage<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            int totalItems = query.Count();
            List<T> items = query.Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            await Task.CompletedTask;

            return new PaginationResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
