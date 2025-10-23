using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using GeoSense.API.Application.DTOs;

namespace GeoSense.API.Api.Helpers
{
    public static class HateoasHelper
    {
        public static List<LinkDTO> GetPagedLinks(IUrlHelper url, string resource, int page, int pageSize, int totalCount)
        {
            // Normalize resource: accept "Motos", "motos", "MOTO", etc.
            if (string.IsNullOrWhiteSpace(resource)) resource = "resource";
            var resourceTrim = resource.Trim();
            // Try to derive singular controller name (basic approach)
            var controllerBase = resourceTrim;
            if (resourceTrim.EndsWith("s", StringComparison.OrdinalIgnoreCase) && resourceTrim.Length > 1)
                controllerBase = resourceTrim.Substring(0, resourceTrim.Length - 1);

            // Action name candidates (Try plural then singular)
            var actionCandidates = new[]
            {
                "Get" + resourceTrim,         // e.g. GetMotos
                "Get" + controllerBase,       // e.g. GetMoto
                "Get" + resourceTrim.TrimEnd('s'), // fallback
            };

            // Try to resolve URL via IUrlHelper (respects ApiVersion if present)
            string? hrefSelf = null;
            string routeVersion = "1";
            try
            {
                routeVersion = url.ActionContext.RouteData.Values["version"]?.ToString() ?? "1";
                foreach (var actionName in actionCandidates)
                {
                    try
                    {
                        hrefSelf = url.Action(actionName, controllerBase, new { page, pageSize, version = routeVersion });
                        if (!string.IsNullOrWhiteSpace(hrefSelf)) break;
                    }
                    catch
                    {
                        // ignore and try next candidate
                        hrefSelf = null;
                    }
                }
            }
            catch
            {
                hrefSelf = null;
            }

            // fallbackBase: build manual fallback route using version found or default v1
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

            int totalPages = pageSize > 0 ? (int)Math.Ceiling((double)totalCount / pageSize) : 1;

            if (page > 1)
            {
                var prevHref = hrefSelf;
                if (string.IsNullOrWhiteSpace(prevHref))
                {
                    prevHref = $"{fallbackBase}?page={page - 1}&pageSize={pageSize}";
                }
                else
                {
                    // try to generate prev via Url.Action too
                    try
                    {
                        prevHref = url.Action(actionCandidates[0], controllerBase, new { page = page - 1, pageSize, version = routeVersion }) ?? prevHref;
                    }
                    catch { }
                }

                links.Add(new LinkDTO
                {
                    Rel = "prev",
                    Method = "GET",
                    Href = prevHref
                });
            }

            if (page < totalPages)
            {
                var nextHref = hrefSelf;
                if (string.IsNullOrWhiteSpace(nextHref))
                {
                    nextHref = $"{fallbackBase}?page={page + 1}&pageSize={pageSize}";
                }
                else
                {
                    try
                    {
                        nextHref = url.Action(actionCandidates[0], controllerBase, new { page = page + 1, pageSize, version = routeVersion }) ?? nextHref;
                    }
                    catch { }
                }

                links.Add(new LinkDTO
                {
                    Rel = "next",
                    Method = "GET",
                    Href = nextHref
                });
            }

            return links;
        }
    }
}