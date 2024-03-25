using Microsoft.EntityFrameworkCore;
using TECNOSISTEMAS.Data.Entidades;
using static TECNOSISTEMAS.Data.EntityConfig;

namespace TECNOSISTEMAS.Data
{
    public class TecnosistemasDbContext:DbContext
    {
        public TecnosistemasDbContext(DbContextOptions<TecnosistemasDbContext> options) : base(options) { }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Sar> Sar { get; set; }
        public DbSet<Impuesto> Impuesto { get; set; }
        public DbSet<Descuento> Descuento { get; set; }
        public DbSet<Cliente> Cliente { get; set; }


        //Variables de sesion
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Modulo> Modulo { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<AgrupadoModulos> AgrupadoModulos { get; set; }
        public DbSet<ModulosRoles> ModulosRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProveedorConfig());
            modelBuilder.ApplyConfiguration(new CategoriaConfig());
            modelBuilder.ApplyConfiguration(new ProductoConfig());
            modelBuilder.ApplyConfiguration(new InventarioConfig());
            modelBuilder.ApplyConfiguration(new SarConfig());
            modelBuilder.ApplyConfiguration(new ImpuestoConfig());
            modelBuilder.ApplyConfiguration(new DescuentoConfig());
            modelBuilder.ApplyConfiguration(new ClienteConfig());

            //Variables de Sesion
            modelBuilder.ApplyConfiguration(new RolConfig());
            modelBuilder.ApplyConfiguration(new ModulosConfig());
            modelBuilder.ApplyConfiguration(new UsuarioConfig());
            modelBuilder.ApplyConfiguration(new AgrupadoModulosConfig());
            modelBuilder.ApplyConfiguration(new ModulosRolesConfig());
        }
    }
}
