using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace NucleoAyC.Controllers
{
    [Route("web/[controller]/[action]")]
    public class HelpController : Controller
    {
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        public HelpController(IApiDescriptionGroupCollectionProvider apiExplorer)
        {
            _apiExplorer = apiExplorer;
        }

        public IActionResult Index()
        {
            return View(_apiExplorer);
        }

        public IActionResult BuscarRecursos(string buscar="credito")
        {
            List<ApiDescription> descripciones = _apiExplorer
                .ApiDescriptionGroups
                .Items.AsEnumerable()
                .Where(
                    k=>k
                        .Items
                        .Any(f=>f.RelativePath.ToUpper()
                        .Contains(buscar.ToUpper()))
                 ).SelectMany(s=>s.Items).ToList();

            return View(descripciones);
        }

        public IActionResult Detalle(string recurso)
        {
            ApiDescription detalle= _apiExplorer
                .ApiDescriptionGroups
                .Items
                .Single(
                    k => k
                        .Items
                        .Any(f => f.RelativePath==recurso)
                 ).Items.Single(d=>d.RelativePath==recurso);
            return View(detalle);
        }
    }
}