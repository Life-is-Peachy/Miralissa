using Microsoft.EntityFrameworkCore;
using System;

namespace Miralissa.Server
{
    public class PipelineContext : DbContext
    {
        public PipelineContext(DbContextOptions<PipelineContext> options)
            : base(options) { }

        public virtual DbSet<Egrp> Egrps { get; set; }
        public virtual DbSet<KeysSchedule> KeysSchedules { get; set; }
        public virtual DbSet<Subscript> Pipeline { get; set; }
        public virtual DbSet<RosrKey> RosrKeys { get; set; }
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<Xml> Xmls { get; set; }
        public virtual DbSet<XmlHref> XmlHrefs { get; set; }


        [DbFunction("decompress", "dbo")]
        public static byte[] Decompress(byte[] strongBytes)
		{
            throw new NotImplementedException();
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Egrp>(entity =>
            {
                entity.ToTable("EGRP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Fio)
                    .HasMaxLength(300)
                    .HasColumnName("FIO");

                entity.Property(e => e.IdPipeline).HasColumnName("ID_Pipeline");

                entity.Property(e => e.RegDate).HasColumnType("date");
            });

            modelBuilder.Entity<KeysSchedule>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("KeysSchedule");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.IdRosrKeys).HasColumnName("ID_RosrKeys");
            });

            modelBuilder.Entity<Subscript>(entity =>
            {
                entity.ToTable("Pipeline");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.AddressRecivedAt).HasColumnType("date");

                entity.Property(e => e.CadastralNumber).HasMaxLength(70);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Corp).HasMaxLength(100);

                entity.Property(e => e.District).HasMaxLength(100);

                entity.Property(e => e.Flat).HasMaxLength(100);

                entity.Property(e => e.HasXml).HasDefaultValueSql("((0))");

                entity.Property(e => e.Home).HasMaxLength(100);

                entity.Property(e => e.ID_Request).HasColumnName("ID_Request");

                entity.Property(e => e.LastUploadAttempt).HasColumnType("datetime");

                entity.Property(e => e.NumRequest).HasMaxLength(50);

                entity.Property(e => e.Priority).HasDefaultValueSql("((1))");

                entity.Property(e => e.R_CadastralCost)
                    .HasColumnType("money")
                    .HasColumnName("R_CadastralCost");

                entity.Property(e => e.R_CadastralCostDate)
                    .HasColumnType("date")
                    .HasColumnName("R_CadastralCostDate");

                entity.Property(e => e.R_FullAddress)
                    .HasMaxLength(250)
                    .HasColumnName("R_FullAddress");

                entity.Property(e => e.R_FuncName)
                    .HasMaxLength(200)
                    .HasColumnName("R_FuncName");

                entity.Property(e => e.R_LiterBTI)
                    .HasMaxLength(50)
                    .HasColumnName("R_LiterBTI");

                entity.Property(e => e.R_NumStoreys)
                    .HasMaxLength(50)
                    .HasColumnName("R_NumStoreys");

                entity.Property(e => e.R_ObjType)
                    .HasMaxLength(200)
                    .HasColumnName("R_ObjType");

                entity.Property(e => e.R_Square)
                    .HasMaxLength(100)
                    .HasColumnName("R_Square");

                entity.Property(e => e.R_Status)
                    .HasMaxLength(100)
                    .HasColumnName("R_Status");

                entity.Property(e => e.R_SteadCategory)
                    .HasMaxLength(100)
                    .HasColumnName("R_SteadCategory");

                entity.Property(e => e.R_SteadKind)
                    .HasMaxLength(200)
                    .HasColumnName("R_SteadKind");

                entity.Property(e => e.R_UpdateInfoDate)
                    .HasColumnType("date")
                    .HasColumnName("R_UpdateInfoDate");

                entity.Property(e => e.RequestRecivedAt).HasColumnType("datetime");

                entity.Property(e => e.Result).HasMaxLength(100);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Square).HasMaxLength(50);

                entity.Property(e => e.Street).HasMaxLength(100);

                entity.Property(e => e.Town).HasMaxLength(100);

                entity.Property(e => e.Worker).HasMaxLength(100);
            });

            modelBuilder.Entity<RosrKey>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InLoading).HasMaxLength(50);

                entity.Property(e => e.InOrdering).HasMaxLength(80);

                entity.Property(e => e.LoginKey)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Source>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Source1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Source");
            });

            modelBuilder.Entity<Xml>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Xml");

                entity.Property(e => e.IdPipeline).HasColumnName("ID_Pipeline");

                entity.Property(e => e.XslPath).HasMaxLength(400);
            });

            modelBuilder.Entity<XmlHref>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("XmlHref");

                entity.Property(e => e.FactoryType).HasMaxLength(100);

                entity.Property(e => e.Href).HasMaxLength(300);
            });
        }
	}
}
