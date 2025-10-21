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
            var controllerBase = resource;
            if (resource.EndsWith("s") && resource.Length > 1)
                controllerBase = resource.Substring(0, resource.Length - 1);

            var actionName = "Get" + resource;

            // Tenta gerar via IUrlHelper (respeita versionamento e rotas)
            string? hrefSelf = null;
            try
            {
                // tenta incluir version se estiver disponível no RouteData
                var version = url.ActionContext.RouteData.Values["version"]?.ToString();
                hrefSelf = url.Action(actionName, controllerBase, new { page, pageSize, version });
            }
            catch
            {
                hrefSelf = null;
            }

            // fallback: monta URL manualmente (usa versão encontrada no RouteData ou v1 por padrão)
            var routeVersion = url.ActionContext.RouteData.Values["version"]?.ToString() ?? "1";
            var fallbackBase = $"/api/v{routeVersion}/{controllerBase.ToLowerInvariant()}";

            var links = new List<LinkDTO>
            {
                new LinkDTO
                {
                    Rel = "self",
                    Method = "GET",
                    Href = hrefSelf ?? $"{fallbackBase}?page={page}&pageSize={pageSize}"
                }
            };

            int totalPages = (int)System.Math.Ceiling((double)totalCount / pageSize);

            if (page > 1)
            {
                links.Add(new LinkDTO
                {
                    Rel = "prev",
                    Method = "GET",
                    Href = hrefSelf != null ? url.Action(actionName, controllerBase, new { page = page - 1, pageSize }) ?? string.Empty : $"{fallbackBase}?page={page - 1}&pageSize={pageSize}"
                });
            }

            if (page < totalPages)
            {
                links.Add(new LinkDTO
                {
                    Rel = "next",
                    Method = "GET",
                    Href = hrefSelf != null ? url.Action(actionName, controllerBase, new { page = page + 1, pageSize }) ?? string.Empty : $"{fallbackBase}?page={page + 1}&pageSize={pageSize}"
                });
            }

            return links;
        }
    }
}