using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebNews.Models
{
    public partial class Data_NewsContext : DbContext
    {
        public Data_NewsContext()
        {
        }

        public Data_NewsContext(DbContextOptions<Data_NewsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BaiViet> BaiViets { get; set; } = null!;
        public virtual DbSet<BinhLuan> BinhLuans { get; set; } = null!;
        public virtual DbSet<ChuDe> ChuDes { get; set; } = null!;
        public virtual DbSet<DanhMuc> DanhMucs { get; set; } = null!;
        public virtual DbSet<EmailThongBao> EmailThongBaos { get; set; } = null!;
        public virtual DbSet<LuotThich> LuotThiches { get; set; } = null!;
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public virtual DbSet<Quyen> Quyens { get; set; } = null!;
        public virtual DbSet<ViPham> ViPhams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=ADMIN-PC;Initial Catalog=Data_News;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaiViet>(entity =>
            {
                entity.HasKey(e => e.MaBaiViet)
                    .HasName("PK__BaiViet__AEDD56475AB99EF6");

                entity.ToTable("BaiViet");

                entity.Property(e => e.BinhLuan).HasDefaultValueSql("((0))");

                entity.Property(e => e.LuotThich).HasDefaultValueSql("((0))");

                entity.Property(e => e.LuotXem).HasDefaultValueSql("((0))");

                entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");

                entity.Property(e => e.NgayDang).HasColumnType("datetime");

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenBaiViet).HasMaxLength(200);

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ChuDe)
                    .WithMany(p => p.BaiViets)
                    .HasForeignKey(d => d.MaChuDe)
                    .HasConstraintName("FK__BaiViet__MaChuDe__29572725");

                entity.HasOne(d => d.NguoiDung)
                    .WithMany(p => p.BaiViets)
                    .HasForeignKey(d => d.TaiKhoan)
                    .HasConstraintName("FK__BaiViet__TaiKhoa__2A4B4B5E");
            });

            modelBuilder.Entity<BinhLuan>(entity =>
            {
                entity.HasKey(e => e.MaBinhLuan)
                    .HasName("PK__BinhLuan__87CB66A0B503C4E5");

                entity.ToTable("BinhLuan");

                entity.Property(e => e.NgayBinhLuan)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BaiViet)
                    .WithMany(p => p.BinhLuans)
                    .HasForeignKey(d => d.MaBaiViet)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__BinhLuan__MaBaiV__2E1BDC42");

                entity.HasOne(d => d.NguoiDung)
                    .WithMany(p => p.BinhLuans)
                    .HasForeignKey(d => d.TaiKhoan)
                    .HasConstraintName("FK__BinhLuan__TaiKho__2F10007B");
            });

            modelBuilder.Entity<ChuDe>(entity =>
            {
                entity.HasKey(e => e.MaChuDe)
                    .HasName("PK__ChuDe__358545112CE493DE");

                entity.ToTable("ChuDe");

                entity.Property(e => e.DuongDan)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HienThi).HasDefaultValueSql("((1))");

                entity.Property(e => e.LuotXem).HasDefaultValueSql("((0))");

                entity.Property(e => e.TenChuDe).HasMaxLength(100);

                entity.HasOne(d => d.DanhMuc)
                    .WithMany(p => p.ChuDes)
                    .HasForeignKey(d => d.MaDanhMuc)
                    .HasConstraintName("FK__ChuDe__MaDanhMuc__15502E78");
            });

            modelBuilder.Entity<DanhMuc>(entity =>
            {
                entity.HasKey(e => e.MaDanhMuc)
                    .HasName("PK__DanhMuc__B3750887288C4E12");

                entity.ToTable("DanhMuc");

                entity.Property(e => e.DuongDan)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HienThi).HasDefaultValueSql("((1))");

                entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
            });

            modelBuilder.Entity<EmailThongBao>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__EmailTho__A9D105359A747690");

                entity.ToTable("EmailThongBao");

                entity.Property(e => e.Email).HasMaxLength(200);
            });

            modelBuilder.Entity<LuotThich>(entity =>
            {
                entity.HasKey(e => new { e.MaBaiViet, e.TaiKhoan })
                    .HasName("PK__LuotThic__A386DA38670C194D");

                entity.ToTable("LuotThich");

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NgayThich)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.BaiViet)
                    .WithMany(p => p.LuotThiches)
                    .HasForeignKey(d => d.MaBaiViet)
                    .HasConstraintName("FK__LuotThich__MaBai__32E0915F");

                entity.HasOne(d => d.NguoiDung)
                    .WithMany(p => p.LuotThiches)
                    .HasForeignKey(d => d.TaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LuotThich__TaiKh__33D4B598");
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.TaiKhoan)
                    .HasName("PK__NguoiDun__D5B8C7F139E21BDD");

                entity.ToTable("NguoiDung");

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiemThanhTich).HasDefaultValueSql("((0))");

                entity.Property(e => e.GioiTinh).HasDefaultValueSql("((0))");

                entity.Property(e => e.HinhAnh)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("(N'AnhMacDinh.png')");

                entity.Property(e => e.Ho).HasMaxLength(100);

                entity.Property(e => e.MatKhau)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.SoBaiViet).HasDefaultValueSql("((0))");

                entity.Property(e => e.Ten).HasMaxLength(50);

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Quyen)
                    .WithMany(p => p.NguoiDungs)
                    .HasForeignKey(d => d.MaQuyen)
                    .HasConstraintName("FK__NguoiDung__MaQuy__1ED998B2");
            });

            modelBuilder.Entity<Quyen>(entity =>
            {
                entity.HasKey(e => e.MaQuyen)
                    .HasName("PK__Quyen__1D4B7ED4DEC78E57");

                entity.ToTable("Quyen");

                entity.Property(e => e.TenQuyen).HasMaxLength(100);
            });

            modelBuilder.Entity<ViPham>(entity =>
            {
                entity.HasKey(e => e.MaViPham)
                    .HasName("PK__ViPham__F1921D89016037C3");

                entity.ToTable("ViPham");

                entity.Property(e => e.NoiDungViPham).HasMaxLength(100);

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.NguoiDung)
                    .WithMany(p => p.ViPhams)
                    .HasForeignKey(d => d.TaiKhoan)
                    .HasConstraintName("FK__ViPham__TaiKhoan__21B6055D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
