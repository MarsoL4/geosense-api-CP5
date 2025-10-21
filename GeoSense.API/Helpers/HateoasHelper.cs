using GeoSense.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GeoSense.API.Helpers
{
    public static class HateoasHelper
    {
        public static List<LinkDTO> GetPagedLinks(IUrlHelper url, string resource, int page, int pageSize, int totalCount)
        {
            // resource normalmente recebido como "Motos", "Vagas", "Patios", "Usuarios"
            // controllerName assumido como singular (removendo 's' final quando aplicável)
            var controllerName = resource;
            if (resource.EndsWith("s") && resource.Length > 1)
                controllerName = resource.Substring(0, resource.Length - 1);

            // actionName esperado: "Get" + resource (ex: GetMotos)
            var actionName = "Get" + resource;

            var links = new List<LinkDTO>
            {
                new LinkDTO
                {
                    Rel = "self",
                    Method = "GET",
                    Href = url.Action(actionName, controllerName, new { page, pageSize }) ?? string.Empty
                }
            };

            int totalPages = (int)System.Math.Ceiling((double)totalCount / pageSize);

            if (page > 1)
            {
                links.Add(new LinkDTO
                {
                    Rel = "prev",
                    Method = "GET",
                    Href = url.Action(actionName, controllerName, new { page = page - 1, pageSize }) ?? string.Empty
                });
            }

            if (page < totalPages)
            {
                links.Add(new LinkDTO
                {
                    Rel = "next",
                    Method = "GET",
                    Href = url.Action(actionName, controllerName, new { page = page + 1, pageSize }) ?? string.Empty
                });
            }

            return links;
        }
    }
}