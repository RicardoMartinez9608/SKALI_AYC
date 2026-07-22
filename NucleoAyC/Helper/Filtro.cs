using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using NucleoAyC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NucleoAyC.Helper
{
    public class Filtro
    {
        private readonly NegocioContexto _context;

        public Filtro(NegocioContexto gn)
        {
            _context = gn;
        }

        public List<Persona> PersonasPorAgencia(int id_agencia)
        {
            List<Persona> personas_por_agencia = new List<Persona>();
            List<Usuario_Agencia> usuarios = _context.Agencia.AsNoTracking().Include("Usuario_Agencias").SingleOrDefault(t => t.Id_Agencia == id_agencia).Usuario_Agencias.ToList();
            foreach (Usuario_Agencia ua in usuarios)
            {
                string id_usuario = ua.Id_Usuario;
                Func<Cartera, bool> filtro_cartera = new Func<Cartera, bool>(c=>c.Id_Usuario==id_usuario);
                bool tiene_Cartera = _context.Cartera.Any(filtro_cartera);
                if (tiene_Cartera)
                {
                    List<Persona> personas = _context.Cartera
                        .Include(d=>d.Personas)
                        .AsNoTracking()
                        .SingleOrDefault(l => l.Id_Usuario == id_usuario)
                        .Personas.ToList();
                    personas_por_agencia.AddRange(personas);
                }
            }
            return personas_por_agencia;
        }
    }
}
