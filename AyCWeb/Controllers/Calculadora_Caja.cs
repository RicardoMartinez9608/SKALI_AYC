using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;

namespace AyCWeb.Controllers
{
    public class Calculadora_Caja : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Calcular_Vuelto()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Operaciones(decimal valor_cuota, decimal valor_efectivo)
        {
            decimal total;
            total = valor_efectivo - valor_cuota;
            return Ok(total);
        }
    }
}
