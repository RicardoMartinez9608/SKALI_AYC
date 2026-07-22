using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.CodeAnalysis.Operations;

namespace ModelNucleo.Model
{
    public class Menu_Contenido_Vista
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Menu_Contenido_Vista { get; set; }

        public int Id_Menu_Categoria_Vista { get; set; }
        public Menu_Categoria_vista Menu_Categoria_Vista { get; set; }

        public string Titulo { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icono { get; set; }

        public ICollection<Menu_Rol_Contenido> MenuRolContenidos { get; set; }
    }

    public class Menu_Categoria_vista
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Menu_Categoria_Vista { get; set; }
        public string Titulo { get; set; }
        public int Nivel { get; set; }
        public string Icono { get; set; }

        public ICollection<Menu_Contenido_Vista> Menu_Contenido { get; set; }

        public ICollection<Menu_Rol_Categoria> MenuRolCategorias { get; set; }

    }

    public class Menu_Rol_Contenido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Menu_Rol_Contenido { get; set; }

        public int Id_Menu_Contenido_Vista { get; set; }
        public Menu_Contenido_Vista MenuContenidoVista { get; set; }

        public string Rol { get; set; }
    }

    public class Menu_Rol_Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Menu_Rol_Categoria { get; set; }
        public int Id_Menu_Categoria_Vista { get; set; }
        public Menu_Categoria_vista MenuCategoriaVista { get; set; }
        public string Rol { get; set; }
    }
}
