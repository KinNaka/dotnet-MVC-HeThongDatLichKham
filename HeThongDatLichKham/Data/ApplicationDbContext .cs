using HeThongDatLichKham.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HeThongDatLichKham.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserClass, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<BenhNhan> BenhNhans { get; set; }
        public DbSet<BacSi> BacSis { get; set; }
        public DbSet<ChuyenKhoa> ChuyenKhoas { get; set; }
        public DbSet<BenhVien> BenhViens { get; set; }
        public DbSet<DatLich> DatLiches { get; set; }
        public DbSet<HoSoBenhAn> HoSoBenhAns { get; set; }
        public DbSet<BenhVienDatLich> BenhVienDatLiches { get; set; }
        public DbSet<LichLamViec> LichLamViecs { get; set; }
        public DbSet<LichHen> LichHens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== BenhNhan ==========
            modelBuilder.Entity<BenhNhan>()
                .HasOne(bn => bn.TaiKhoanNguoiDung)
                .WithMany(user => user.BenhNhans)
                .HasForeignKey(bn => bn.IDNguoiDung)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== BacSi ==========
            modelBuilder.Entity<BacSi>()
                .HasOne(bs => bs.ChuyenKhoa)
                .WithMany(ck => ck.BacSis)
                .HasForeignKey(bs => bs.IDChuyenKhoa)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== DatLich ==========
            modelBuilder.Entity<DatLich>()
                .HasOne(dl => dl.BenhNhan)
                .WithMany()
                .HasForeignKey(dl => dl.CCCD)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DatLich>()
                .HasOne(dl => dl.BacSi)
                .WithMany()
                .HasForeignKey(dl => dl.MaBacSi)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DatLich>()
                .HasOne(dl => dl.BenhVien)
                .WithMany()
                .HasForeignKey(dl => dl.MaBenhVien)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== HoSoBenhAn ==========
            modelBuilder.Entity<HoSoBenhAn>()
                .HasOne(hs => hs.BacSi)
                .WithMany()
                .HasForeignKey(hs => hs.MaBacSi)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HoSoBenhAn>()
                .HasOne(hs => hs.BenhNhan)
                .WithMany(bn => bn.HoSos)
                .HasForeignKey(hs => hs.CCCD)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== BenhVienDatLich ==========
            modelBuilder.Entity<BenhVienDatLich>()
                .HasKey(bvd => new { bvd.MaBacSi, bvd.IDDatLich });

            modelBuilder.Entity<BenhVienDatLich>()
                .HasOne(bvd => bvd.BacSi)
                .WithMany()
                .HasForeignKey(bvd => bvd.MaBacSi)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BenhVienDatLich>()
                .HasOne(bvd => bvd.DatLich)
                .WithMany()
                .HasForeignKey(bvd => bvd.IDDatLich)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichHen>()
    .HasOne(lh => lh.LichLamViec)
    .WithMany()
    .HasForeignKey(lh => lh.MaLich)
    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
