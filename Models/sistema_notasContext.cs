using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaNotas1.Models
{
    public partial class sistema_notasContext : DbContext
    {
        public sistema_notasContext()
        {
        }

        public sistema_notasContext(DbContextOptions<sistema_notasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbActividad> TbActividad { get; set; }
        public virtual DbSet<TbAdministracion> TbAdministracion { get; set; }
        public virtual DbSet<TbAlumno> TbAlumno { get; set; }
        public virtual DbSet<TbAlumnoPadreDeFamilia> TbAlumnoPadreDeFamilia { get; set; }
        public virtual DbSet<TbAño> TbAño { get; set; }
        public virtual DbSet<TbClase> TbClase { get; set; }
        public virtual DbSet<TbClaseAlumno> TbClaseAlumno { get; set; }
        public virtual DbSet<TbCurso> TbCurso { get; set; }
        public virtual DbSet<TbDocente> TbDocente { get; set; }
        public virtual DbSet<TbGrado> TbGrado { get; set; }
        public virtual DbSet<TbInformacionPersonal> TbInformacionPersonal { get; set; }
        public virtual DbSet<TbJornadas> TbJornadas { get; set; }
        public virtual DbSet<TbNivel> TbNivel { get; set; }
        public virtual DbSet<TbNota> TbNota { get; set; }
        public virtual DbSet<TbPadreDeFamilia> TbPadreDeFamilia { get; set; }
        public virtual DbSet<TbRol> TbRol { get; set; }
        public virtual DbSet<TbSeccion> TbSeccion { get; set; }
        public virtual DbSet<TbUnidad> TbUnidad { get; set; }
        public virtual DbSet<TbUsuario> TbUsuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("Server=sistemanotas.mysql.database.azure.com;Port=3306;Database=sistema_notas;Uid=blackdog;Pwd=azure123!;SslMode=Preferred;") ;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbActividad>(entity =>
            {
                entity.HasKey(e => e.CodActividad)
                    .HasName("PRIMARY");

                entity.ToTable("tb_actividad");

                entity.HasIndex(e => e.CodClase)
                    .HasName("Reftb_clase41");

                entity.Property(e => e.CodActividad).HasColumnName("cod_actividad");

                entity.Property(e => e.CodClase).HasColumnName("cod_clase");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Punteo)
                    .HasColumnName("punteo")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TbAdministracion>(entity =>
            {
                entity.HasKey(e => e.CodAdministracion)
                    .HasName("PRIMARY");

                entity.ToTable("tb_administracion");

                entity.HasIndex(e => e.CodInformacionPersonal)
                    .HasName("Reftb_informacion_personal13");

                entity.HasIndex(e => e.CodUsuario)
                    .HasName("Reftb_usuario16");

                entity.Property(e => e.CodAdministracion).HasColumnName("cod_administracion");

                entity.Property(e => e.CodInformacionPersonal).HasColumnName("cod_informacion_personal");

                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");
            });

            modelBuilder.Entity<TbAlumno>(entity =>
            {
                entity.HasKey(e => e.CodAlumno)
                    .HasName("PRIMARY");

                entity.ToTable("tb_alumno");

                entity.HasIndex(e => e.CodGrado)
                    .HasName("Reftb_grado45");

                entity.HasIndex(e => e.CodInformacionPersonal)
                    .HasName("Reftb_informacion_personal12");

                entity.HasIndex(e => e.CodUsuario)
                    .HasName("Reftb_usuario17");

                entity.Property(e => e.CodAlumno).HasColumnName("cod_alumno");

                entity.Property(e => e.CodGrado).HasColumnName("cod_grado");

                entity.Property(e => e.CodInformacionPersonal).HasColumnName("cod_informacion_personal");

                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");

                entity.Property(e => e.Codigo)
                    .HasColumnName("codigo")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbAlumnoPadreDeFamilia>(entity =>
            {
                entity.HasKey(e => e.CodAlumnoPadreDeFamilia)
                    .HasName("PRIMARY");

                entity.ToTable("tb_alumno_padre_de_familia");

                entity.HasIndex(e => e.CodAlumno)
                    .HasName("Reftb_alumno38");

                entity.HasIndex(e => e.CodPadreDeFamilia)
                    .HasName("Reftb_padre_de_familia40");

                entity.Property(e => e.CodAlumnoPadreDeFamilia).HasColumnName("cod_alumno_padre_de_familia");

                entity.Property(e => e.CodAlumno).HasColumnName("cod_alumno");

                entity.Property(e => e.CodPadreDeFamilia).HasColumnName("cod_padre_de_familia");
            });

            modelBuilder.Entity<TbAño>(entity =>
            {
                entity.HasKey(e => e.CodAño)
                    .HasName("PRIMARY");

                entity.ToTable("tb_año");

                entity.Property(e => e.CodAño).HasColumnName("cod_año");

                entity.Property(e => e.Numero).HasColumnName("numero");
            });

            modelBuilder.Entity<TbClase>(entity =>
            {
                entity.HasKey(e => e.CodClase)
                    .HasName("PRIMARY");

                entity.ToTable("tb_clase");

                entity.HasIndex(e => e.CodAño)
                    .HasName("Reftb_año19");

                entity.HasIndex(e => e.CodCurso)
                    .HasName("Reftb_curso26");

                entity.HasIndex(e => e.CodDocente)
                    .HasName("Reftb_docente27");

                entity.HasIndex(e => e.CodJornada)
                    .HasName("Reftb_jornadas20");

                entity.HasIndex(e => e.CodSeccion)
                    .HasName("Reftb_seccion42");

                entity.HasIndex(e => e.CodUnidad)
                    .HasName("Reftb_unidad18");

                entity.Property(e => e.CodClase).HasColumnName("cod_clase");

                entity.Property(e => e.CodAño).HasColumnName("cod_año");

                entity.Property(e => e.CodCurso).HasColumnName("cod_curso");

                entity.Property(e => e.CodDocente).HasColumnName("cod_docente");

                entity.Property(e => e.CodJornada).HasColumnName("cod_jornada");

                entity.Property(e => e.CodSeccion).HasColumnName("cod_seccion");

                entity.Property(e => e.CodUnidad).HasColumnName("cod_unidad");
            });

            modelBuilder.Entity<TbClaseAlumno>(entity =>
            {
                entity.HasKey(e => e.CodClaseAlumno)
                    .HasName("PRIMARY");

                entity.ToTable("tb_clase_alumno");

                entity.HasIndex(e => e.CodAlumno)
                    .HasName("Reftb_alumno43");

                entity.HasIndex(e => e.CodClase)
                    .HasName("Reftb_clase44");

                entity.Property(e => e.CodClaseAlumno).HasColumnName("cod_clase_alumno");

                entity.Property(e => e.Aprobado).HasColumnName("aprobado");

                entity.Property(e => e.CodAlumno).HasColumnName("cod_alumno");

                entity.Property(e => e.CodClase).HasColumnName("cod_clase");
            });

            modelBuilder.Entity<TbCurso>(entity =>
            {
                entity.HasKey(e => e.CodCurso)
                    .HasName("PRIMARY");

                entity.ToTable("tb_curso");

                entity.HasIndex(e => e.CodGrado)
                    .HasName("Reftb_grado25");

                entity.Property(e => e.CodCurso).HasColumnName("cod_curso");

                entity.Property(e => e.CodGrado).HasColumnName("cod_grado");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbDocente>(entity =>
            {
                entity.HasKey(e => e.CodDocente)
                    .HasName("PRIMARY");

                entity.ToTable("tb_docente");

                entity.HasIndex(e => e.CodInformacionPersonal)
                    .HasName("Reftb_informacion_personal11");

                entity.HasIndex(e => e.CodUsuario)
                    .HasName("Reftb_usuario4");

                entity.Property(e => e.CodDocente).HasColumnName("cod_docente");

                entity.Property(e => e.CodInformacionPersonal).HasColumnName("cod_informacion_personal");

                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");
            });

            modelBuilder.Entity<TbGrado>(entity =>
            {
                entity.HasKey(e => e.CodGrado)
                    .HasName("PRIMARY");

                entity.ToTable("tb_grado");

                entity.HasIndex(e => e.CodGradoSiguiente)
                    .HasName("Reftb_grado46");

                entity.HasIndex(e => e.CodNivel)
                    .HasName("Reftb_nivel36");

                entity.Property(e => e.CodGrado).HasColumnName("cod_grado");

                entity.Property(e => e.CodGradoSiguiente).HasColumnName("cod_grado_siguiente");

                entity.Property(e => e.CodNivel).HasColumnName("cod_nivel");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SiguienteNivel).HasColumnName("siguiente_nivel");
            });

            modelBuilder.Entity<TbInformacionPersonal>(entity =>
            {
                entity.HasKey(e => e.CodInformacionPersonal)
                    .HasName("PRIMARY");

                entity.ToTable("tb_informacion_personal");

                entity.Property(e => e.CodInformacionPersonal).HasColumnName("cod_informacion_personal");

                entity.Property(e => e.Apellido)
                    .HasColumnName("apellido")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasColumnName("correo_electronico")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cui)
                    .HasColumnName("cui")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Imagen)
                    .HasColumnName("imagen")
                    .HasColumnType("longblob");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbJornadas>(entity =>
            {
                entity.HasKey(e => e.CodJornada)
                    .HasName("PRIMARY");

                entity.ToTable("tb_jornadas");

                entity.Property(e => e.CodJornada).HasColumnName("cod_jornada");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbNivel>(entity =>
            {
                entity.HasKey(e => e.CodNivel)
                    .HasName("PRIMARY");

                entity.ToTable("tb_nivel");

                entity.Property(e => e.CodNivel).HasColumnName("cod_nivel");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbNota>(entity =>
            {
                entity.HasKey(e => e.CodNota)
                    .HasName("PRIMARY");

                entity.ToTable("tb_nota");

                entity.HasComment(@"Esto es una tabla para unificar las actividades con los usuarios.
Arriba España");

                entity.HasIndex(e => e.CodActividad)
                    .HasName("Reftb_nota2");

                entity.HasIndex(e => e.CodAlumno)
                    .HasName("Reftb_nota1");

                entity.Property(e => e.CodNota).HasColumnName("cod_nota");

                entity.Property(e => e.CodActividad).HasColumnName("cod_actividad");

                entity.Property(e => e.CodAlumno).HasColumnName("cod_alumno");

                entity.Property(e => e.Punteo)
                    .HasColumnName("punteo")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TbPadreDeFamilia>(entity =>
            {
                entity.HasKey(e => e.CodPadreDeFamilia)
                    .HasName("PRIMARY");

                entity.ToTable("tb_padre_de_familia");

                entity.HasIndex(e => e.CodInformacionPersonal)
                    .HasName("Reftb_informacion_personal14");

                entity.Property(e => e.CodPadreDeFamilia).HasColumnName("cod_padre_de_familia");

                entity.Property(e => e.CodInformacionPersonal).HasColumnName("cod_informacion_personal");
            });

            modelBuilder.Entity<TbRol>(entity =>
            {
                entity.HasKey(e => e.CodRol)
                    .HasName("PRIMARY");

                entity.ToTable("tb_rol");

                entity.Property(e => e.CodRol).HasColumnName("cod_rol");

                entity.Property(e => e.Rol)
                    .HasColumnName("rol")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbSeccion>(entity =>
            {
                entity.HasKey(e => e.CodSeccion)
                    .HasName("PRIMARY");

                entity.ToTable("tb_seccion");

                entity.Property(e => e.CodSeccion).HasColumnName("cod_seccion");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbUnidad>(entity =>
            {
                entity.HasKey(e => e.CodUnidad)
                    .HasName("PRIMARY");

                entity.ToTable("tb_unidad");

                entity.Property(e => e.CodUnidad).HasColumnName("cod_unidad");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbUsuario>(entity =>
            {
                entity.HasKey(e => e.CodUsuario)
                    .HasName("PRIMARY");

                entity.ToTable("tb_usuario");

                entity.HasIndex(e => e.CodRol)
                    .HasName("Reftb_rol10");

                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");

                entity.Property(e => e.CodRol).HasColumnName("cod_rol");

                entity.Property(e => e.Contraseña)
                    .HasColumnName("contraseña")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
