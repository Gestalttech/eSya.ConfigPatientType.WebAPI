using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eSya.ConfigPatientType.DL.Entities
{
    public partial class eSyaEnterprise : DbContext
    {
        public static string _connString = "";

        public eSyaEnterprise()
        {
        }

        public eSyaEnterprise(DbContextOptions<eSyaEnterprise> options)
            : base(options)
        {
        }

        public virtual DbSet<GtEcapcd> GtEcapcds { get; set; } = null!;
        public virtual DbSet<GtEcbsln> GtEcbslns { get; set; } = null!;
        public virtual DbSet<GtEcpad> GtEcpads { get; set; } = null!;
        public virtual DbSet<GtEcpapc> GtEcpapcs { get; set; } = null!;
        public virtual DbSet<GtEcpcdh> GtEcpcdhs { get; set; } = null!;
        public virtual DbSet<GtEcpcsc> GtEcpcscs { get; set; } = null!;
        public virtual DbSet<GtEcpcsl> GtEcpcsls { get; set; } = null!;
        public virtual DbSet<GtEcptcb> GtEcptcbs { get; set; } = null!;
        public virtual DbSet<GtEcptcd> GtEcptcds { get; set; } = null!;
        public virtual DbSet<GtEcptch> GtEcptches { get; set; } = null!;
        public virtual DbSet<GtEcptdp> GtEcptdps { get; set; } = null!;
        public virtual DbSet<GtEcptsp> GtEcptsps { get; set; } = null!;
        public virtual DbSet<GtEcptsr> GtEcptsrs { get; set; } = null!;
        public virtual DbSet<GtEsspbl> GtEsspbls { get; set; } = null!;
        public virtual DbSet<GtEsspcd> GtEsspcds { get; set; } = null!;
        public virtual DbSet<GtEssrbl> GtEssrbls { get; set; } = null!;
        public virtual DbSet<GtEssrcl> GtEssrcls { get; set; } = null!;
        public virtual DbSet<GtEssrm> GtEssrms { get; set; } = null!;
        public virtual DbSet<GtEssrty> GtEssrties { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(_connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GtEcapcd>(entity =>
            {
                entity.HasKey(e => e.ApplicationCode)
                    .HasName("PK_GT_ECAPCD_1");

                entity.ToTable("GT_ECAPCD");

                entity.Property(e => e.ApplicationCode).ValueGeneratedNever();

                entity.Property(e => e.CodeDesc).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ShortCode).HasMaxLength(15);
            });

            modelBuilder.Entity<GtEcbsln>(entity =>
            {
                entity.HasKey(e => new { e.BusinessId, e.LocationId });

                entity.ToTable("GT_ECBSLN");

                entity.HasIndex(e => e.BusinessKey, "IX_GT_ECBSLN")
                    .IsUnique();

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.BusinessName).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.CurrencyCode).HasMaxLength(4);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.LocationDescription).HasMaxLength(150);

                entity.Property(e => e.Lstatus).HasColumnName("LStatus");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ShortDesc).HasMaxLength(15);

                entity.Property(e => e.TocurrConversion).HasColumnName("TOCurrConversion");

                entity.Property(e => e.TolocalCurrency)
                    .IsRequired()
                    .HasColumnName("TOLocalCurrency")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TorealCurrency).HasColumnName("TORealCurrency");
            });

            modelBuilder.Entity<GtEcpad>(entity =>
            {
                entity.HasKey(e => new { e.PatientCategoryId, e.DiscountFor, e.DiscountAt, e.ServiceClassId, e.ServiceId, e.ParameterId });

                entity.ToTable("GT_ECPADS");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.ServiceClassId).HasColumnName("ServiceClassID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ParmDesc).HasMaxLength(15);

                entity.Property(e => e.ParmPerc).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.ParmValue).HasColumnType("numeric(18, 6)");
            });

            modelBuilder.Entity<GtEcpapc>(entity =>
            {
                entity.HasKey(e => new { e.PatientTypeId, e.PatientCategoryId, e.ParameterId });

                entity.ToTable("GT_ECPAPC");

                entity.Property(e => e.PatientTypeId).HasColumnName("PatientTypeID");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ParmDesc).HasMaxLength(15);

                entity.Property(e => e.ParmPerc).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.ParmValue).HasColumnType("numeric(18, 6)");
            });

            modelBuilder.Entity<GtEcpcdh>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.PatientCategoryId, e.DiscountFor });

                entity.ToTable("GT_ECPCDH");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcpcsc>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.PatientCategoryId, e.DiscountFor, e.ServiceId });

                entity.ToTable("GT_ECPCSC");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.DiscountPerc).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ServiceChargePerc).HasColumnType("numeric(5, 2)");
            });

            modelBuilder.Entity<GtEcpcsl>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.PatientCategoryId, e.DiscountFor, e.ServiceClassId });

                entity.ToTable("GT_ECPCSL");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.ServiceClassId).HasColumnName("ServiceClassID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.DiscountPerc).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ServiceChargePerc).HasColumnType("numeric(5, 2)");
            });

            modelBuilder.Entity<GtEcptcb>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.PatientTypeId, e.PatientCategoryId });

                entity.ToTable("GT_ECPTCB");

                entity.Property(e => e.PatientTypeId).HasColumnName("PatientTypeID");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcptcd>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.PatientTypeId, e.PatientCategoryId, e.PatientCatgDocId });

                entity.ToTable("GT_ECPTCD");

                entity.Property(e => e.PatientTypeId).HasColumnName("PatientTypeID");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.PatientCatgDocId).HasColumnName("PatientCatgDocID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcptch>(entity =>
            {
                entity.HasKey(e => new { e.PatientTypeId, e.PatientCategoryId });

                entity.ToTable("GT_ECPTCH");

                entity.Property(e => e.PatientTypeId).HasColumnName("PatientTypeID");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcptdp>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.PatientTypeId, e.PatientCategoryId, e.Relationship });

                entity.ToTable("GT_ECPTDP");

                entity.Property(e => e.PatientTypeId).HasColumnName("PatientTypeID");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcptsp>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.PatientTypeId, e.PatientCategoryId, e.SpecialtyId });

                entity.ToTable("GT_ECPTSP");

                entity.Property(e => e.PatientTypeId).HasColumnName("PatientTypeID");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.SpecialtyId).HasColumnName("SpecialtyID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcptsr>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.PatientTypeId, e.PatientCategoryId, e.ServiceType, e.RateType });

                entity.ToTable("GT_ECPTSR");

                entity.Property(e => e.PatientTypeId).HasColumnName("PatientTypeID");

                entity.Property(e => e.PatientCategoryId).HasColumnName("PatientCategoryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEsspbl>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.SpecialtyId })
                    .HasName("PK_GT_ESSPBL_1");

                entity.ToTable("GT_ESSPBL");

                entity.Property(e => e.SpecialtyId).HasColumnName("SpecialtyID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEsspcd>(entity =>
            {
                entity.HasKey(e => e.SpecialtyId);

                entity.ToTable("GT_ESSPCD");

                entity.Property(e => e.SpecialtyId)
                    .ValueGeneratedNever()
                    .HasColumnName("SpecialtyID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FocusArea).HasMaxLength(2000);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MedicalIcon).HasMaxLength(150);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.SpecialtyDesc).HasMaxLength(50);

                entity.Property(e => e.SpecialtyGroup)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SpecialtyType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<GtEssrbl>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.ServiceId });

                entity.ToTable("GT_ESSRBL");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ServiceCost).HasColumnType("numeric(18, 6)");
            });

            modelBuilder.Entity<GtEssrcl>(entity =>
            {
                entity.HasKey(e => e.ServiceClassId);

                entity.ToTable("GT_ESSRCL");

                entity.Property(e => e.ServiceClassId)
                    .ValueGeneratedNever()
                    .HasColumnName("ServiceClassID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.ServiceClassDesc).HasMaxLength(50);

                entity.Property(e => e.ServiceGroupId).HasColumnName("ServiceGroupID");
            });

            modelBuilder.Entity<GtEssrm>(entity =>
            {
                entity.HasKey(e => e.ServiceId);

                entity.ToTable("GT_ESSRMS");

                entity.Property(e => e.ServiceId)
                    .ValueGeneratedNever()
                    .HasColumnName("ServiceID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InternalServiceCode).HasMaxLength(15);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ServiceClassId).HasColumnName("ServiceClassID");

                entity.Property(e => e.ServiceDesc).HasMaxLength(75);

                entity.Property(e => e.ServiceShortDesc)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.HasOne(d => d.ServiceClass)
                    .WithMany(p => p.GtEssrms)
                    .HasForeignKey(d => d.ServiceClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ESSRMS_GT_ESSRCL");
            });

            modelBuilder.Entity<GtEssrty>(entity =>
            {
                entity.HasKey(e => e.ServiceTypeId);

                entity.ToTable("GT_ESSRTY");

                entity.Property(e => e.ServiceTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("ServiceTypeID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ServiceTypeDesc).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
