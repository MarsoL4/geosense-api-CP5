using GeoSense.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GeoSense.API.Helpers
{
    public static class HateoasHelper
    {
        public static List<LinkDTO> GetPagedLinks(IUrlHelper url, string resource, int page, int pageSize, int totalCount)
        {
            var links = new List<LinkDTO>
            {
                new LinkDTO
                {
                    Rel = "self",
                    Method = "GET",
                    Href = url.Action("Get" + resource, resource, new { page, pageSize }) ?? string.Empty
                }
            };

            int totalPages = (int)System.Math.Ceiling((double)totalCount / pageSize);

            if (page > 1)
            {
                links.Add(new LinkDTO
                {
                    Rel = "prev",
                    Method = "GET",
                    Href = url.Action("Get" + resource, resource, new { page = page - 1, pageSize }) ?? string.Empty
                });
            }

            if (page < totalPages)
            {
                links.Add(new LinkDTO
                {
                    Rel = "next",
                    Method = "GET",
                    Href = url.Action("Get" + resource, resource, new { page = page + 1, pageSize }) ?? string.Empty
                });
            }

            return links;
        }
    }
}