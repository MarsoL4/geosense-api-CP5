using System.Collections.Generic;

namespace GeoSense.API.DTOs
{
    public class PagedHateoasDTO<T>
    {
        public required IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public required List<LinkDTO> Links { get; set; }
    }

    public class LinkDTO
    {
        public required string Rel { get; set; }
        public required string Method { get; set; }
        public required string Href { get; set; }
    }
}