﻿namespace TECNOSISTEMAS.Data.Entidades
{
    public class Proveedor
    {
        //Atributos 
        public string Nombre { set; get; }
        public string Direccion { set; get; }
        public string Telefono { set; get; }
        public string CorreoElectronico { set; get; }
        //Relacion CompraProducto
        public ICollection<CompraProducto> CompraProducto { set; get; }

        //Constructor
        public Proveedor()
        {
            CompraProducto = new HashSet<CompraProducto>();
        }

        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }



    }
}
