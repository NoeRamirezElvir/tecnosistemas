using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TECNOSISTEMAS.Data.Entidades;

namespace TECNOSISTEMAS.Data
{
    public class EntityConfig
    {
        //CONFIG PROVEEDOR
        public class ProveedorConfig : IEntityTypeConfiguration<Proveedor>
        {
            public void Configure(EntityTypeBuilder<Proveedor> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Nombre).HasColumnType("varchar(50)");
                builder.Property(x => x.Direccion).HasColumnType("varchar(50)");
                builder.Property(x => x.Telefono).HasColumnType("varchar(15)");
                builder.Property(x => x.CorreoElectronico).HasColumnType("varchar(50)");
            }

        }
        //CONFIG CATEGORIA
        public class CategoriaConfig : IEntityTypeConfiguration<Categoria>
        {
            public void Configure(EntityTypeBuilder<Categoria> builder)
            {
                builder.HasKey(x => x.Id);
                builder.HasMany(x => x.Producto).WithOne(a => a.Categoria).HasForeignKey(x => x.IdCategoria);
                builder.Property(x => x.Nombre).HasColumnType("varchar(50)");
                builder.Property(x => x.Descripcion).HasColumnType("varchar(50)");
            }

        }
        //CONFIG PRODUCTO
        public class ProductoConfig : IEntityTypeConfiguration<Producto>
        {
            public void Configure(EntityTypeBuilder<Producto> builder)
            {
                builder.HasKey(x => x.Id);
                builder.HasMany(x => x.Inventario).WithOne(a => a.Producto).HasForeignKey(x => x.IdProducto);
                builder.Property(x => x.Nombre).HasColumnType("varchar(50)");
                builder.Property(x => x.Precio).HasColumnType("decimal(10,2)");
                builder.Property(x => x.Costo).HasColumnType("decimal(10,2)");
                builder.Property(x => x.Activo);

            }

        }
        //CONFIG INVENTARIO
        public class InventarioConfig : IEntityTypeConfiguration<Inventario>
        {
            public void Configure(EntityTypeBuilder<Inventario> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.StockActual).HasColumnType("int");
                builder.Property(x => x.StockMinimo).HasColumnType("int");
                builder.Property(x => x.StockMaximo).HasColumnType("int");

            }

        }
        //SAR Config
        public class SarConfig : IEntityTypeConfiguration<Sar>
        {
            public void Configure(EntityTypeBuilder<Sar> builder)
            {
                builder.HasKey(e => e.Id);
                builder.Property(s => s.CAI).HasColumnType("varchar(40)").HasColumnName("CAI");
                builder.Property(s => s.RangoInicial).HasColumnType("varchar(40)").HasColumnName("RangoInicial");
                builder.Property(s => s.RangoFinal).HasColumnType("varchar(40)").HasColumnName("RangoFinal");
                builder.Property(s => s.LimitEmision).HasColumnType("varchar(50)").HasColumnName("Limite Emision");

            }
        }
        //Impuesto Config
        public class ImpuestoConfig : IEntityTypeConfiguration<Impuesto>
        {
            public void Configure(EntityTypeBuilder<Impuesto> builder)
            {
                builder.HasKey(e => e.Id);
                builder.Property(s => s.Nombre).HasColumnType("varchar(50)").HasColumnName("Nombre");
                builder.Property(s => s.Valor).HasColumnType("decimal(18,2)").HasColumnName("Valor");
            }
        }
        //Descuento Config
        public class DescuentoConfig : IEntityTypeConfiguration<Descuento>
        {
            public void Configure(EntityTypeBuilder<Descuento> builder)
            {
                builder.HasKey(e => e.Id);
                builder.Property(s => s.Nombre).HasColumnType("varchar(50)").HasColumnName("Nombre");
                builder.Property(s => s.Valor).HasColumnType("decimal(18,2)").HasColumnName("Valor");
            }
        }
        //Cliente Config
        public class ClienteConfig : IEntityTypeConfiguration<Cliente>
        {
            public void Configure(EntityTypeBuilder<Cliente> builder)
            {
                builder.HasKey(e => e.Id);
                builder.Property(s => s.Nombre).HasColumnType("varchar(30)").HasColumnName("Nombre");
                builder.Property(s => s.Apellido).HasColumnType("varchar(30)").HasColumnName("Apellido");
                builder.Property(s => s.Telefono).HasColumnType("varchar(15)").HasColumnName("Telefono");
                builder.Property(s => s.Direccion).HasColumnType("varchar(160)").HasColumnName("Direccion");
                builder.Property(s => s.RTN).HasColumnType("varchar(18)").HasColumnName("RTN");
            }
        }


        //Variables de Sesion
        public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
        {
            public void Configure(EntityTypeBuilder<Usuario> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Nombre).HasColumnType("varchar(255)");
            }
        }
        public class RolConfig : IEntityTypeConfiguration<Rol>
        {
            public void Configure(EntityTypeBuilder<Rol> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Descripcion).HasColumnType("varchar(25)");
                builder.HasMany(a => a.ModulosRoles).WithOne(a => a.Rol).HasForeignKey(c => c.RolId);
                builder.HasMany(a => a.Usuarios).WithOne(a => a.Rol).HasForeignKey(c => c.RolId);
            }
        }
        public class ModulosConfig : IEntityTypeConfiguration<Modulo>
        {
            public void Configure(EntityTypeBuilder<Modulo> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Nombre).HasColumnType("varchar(25)");
                builder.Property(x => x.Metodo).HasColumnType("varchar(25)");
                builder.Property(x => x.Controller).HasColumnType("varchar(25)");
                builder.HasMany(a => a.ModulosRoles).WithOne(a => a.Modulo).HasForeignKey(c => c.ModuloId);
            }
        }
        public class ModulosRolesConfig : IEntityTypeConfiguration<ModulosRoles>
        {
            public void Configure(EntityTypeBuilder<ModulosRoles> builder)
            {
                builder.HasKey(x => x.Id);
            }
        }

        public class AgrupadoModulosConfig : IEntityTypeConfiguration<AgrupadoModulos>
        {
            public void Configure(EntityTypeBuilder<AgrupadoModulos> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Descripcion).HasColumnType("varchar(25)");
                builder.HasMany(a => a.Modulos).WithOne(a => a.AgrupadoModulos).HasForeignKey(c => c.AgrupadoModulosId);
            }
        }
    }
}
