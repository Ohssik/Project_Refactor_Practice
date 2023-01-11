using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace prjMSIT145_Final.Models
{
    public partial class ispanMsit145shibaContext : DbContext
    {
        public ispanMsit145shibaContext()
        {
        }

        public ispanMsit145shibaContext(DbContextOptions<ispanMsit145shibaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdImg> AdImgs { get; set; } = null!;
        public virtual DbSet<AdminMember> AdminMembers { get; set; } = null!;
        public virtual DbSet<BusinessImg> BusinessImgs { get; set; } = null!;
        public virtual DbSet<BusinessMember> BusinessMembers { get; set; } = null!;
        public virtual DbSet<Coupon> Coupons { get; set; } = null!;
        public virtual DbSet<Coupon2NormalMember> Coupon2NormalMembers { get; set; } = null!;
        public virtual DbSet<NormalMember> NormalMembers { get; set; } = null!;
        public virtual DbSet<OptionsToProduct> OptionsToProducts { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<OrderSerialNumber> OrderSerialNumbers { get; set; } = null!;
        public virtual DbSet<PaymentTerm2BusiMember> PaymentTerm2BusiMembers { get; set; } = null!;
        public virtual DbSet<PaymentTermCategory> PaymentTermCategories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<ProductImg> ProductImgs { get; set; } = null!;
        public virtual DbSet<ProductOption> ProductOptions { get; set; } = null!;
        public virtual DbSet<ProductOptionGroup> ProductOptionGroups { get; set; } = null!;
        public virtual DbSet<Sysdiagram> Sysdiagrams { get; set; } = null!;
        public virtual DbSet<ViewOptionsToGroup> ViewOptionsToGroups { get; set; } = null!;
        public virtual DbSet<ViewOrderDetailList> ViewOrderDetailLists { get; set; } = null!;
        public virtual DbSet<ViewOrderDetailNonOptionGroupName> ViewOrderDetailNonOptionGroupNames { get; set; } = null!;
        public virtual DbSet<ViewOrderDetailWithOptionGroupName> ViewOrderDetailWithOptionGroupNames { get; set; } = null!;
        public virtual DbSet<ViewShowProductList> ViewShowProductLists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdImg>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("AdImg");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endTime");

                entity.Property(e => e.Hyperlink)
                    .HasMaxLength(50)
                    .HasColumnName("hyperlink");

                entity.Property(e => e.ImgName)
                    .HasMaxLength(50)
                    .HasColumnName("imgName");

                entity.Property(e => e.OrderBy).HasColumnName("orderBy");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");
            });

            modelBuilder.Entity<AdminMember>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AdminMember");

                entity.Property(e => e.Account).HasMaxLength(50);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.RoleLevel).HasColumnName("roleLevel");
            });

            modelBuilder.Entity<BusinessImg>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("BusinessImg");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.BannerImgFileName1)
                    .HasMaxLength(50)
                    .HasColumnName("Banner_ImgFileName_1");

                entity.Property(e => e.BannerImgFileName2)
                    .HasMaxLength(50)
                    .HasColumnName("Banner_ImgFileName_2");

                entity.Property(e => e.BannerImgFileName3)
                    .HasMaxLength(50)
                    .HasColumnName("Banner_ImgFileName_3");

                entity.Property(e => e.LogoImgFileName)
                    .HasMaxLength(50)
                    .HasColumnName("LOGO_ImgFileName");

                entity.Property(e => e.SighImgFileName)
                    .HasMaxLength(50)
                    .HasColumnName("SIgh_ImgFileName");
            });

            modelBuilder.Entity<BusinessMember>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("BusinessMember");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Brand).HasMaxLength(50);

                entity.Property(e => e.ContactPerson).HasMaxLength(50);

                entity.Property(e => e.DeliveryAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmailCertified)
                    .HasColumnName("emailCertified")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Gps)
                    .HasMaxLength(300)
                    .HasColumnName("GPS");

                entity.Property(e => e.IsSuspensed)
                    .HasColumnName("isSuspensed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MemberAccount).HasMaxLength(50);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.RegisterTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ShopType).HasMaxLength(50);
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CouponCode)
                    .HasMaxLength(100)
                    .HasColumnName("couponCode");

                entity.Property(e => e.IsUsed)
                    .HasColumnName("isUsed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Coupon2NormalMember>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Coupon2NormalMember");

                entity.Property(e => e.CouponId).HasColumnName("couponId");

                entity.Property(e => e.MemberId).HasColumnName("memberId");
            });

            modelBuilder.Entity<NormalMember>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("NormalMember");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.AddressArea)
                    .HasMaxLength(50)
                    .HasColumnName("Address_Area");

                entity.Property(e => e.AddressCity)
                    .HasMaxLength(50)
                    .HasColumnName("Address_City");

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmailCertified)
                    .HasColumnName("emailCertified")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.IsSuspensed)
                    .HasColumnName("isSuspensed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MemberAccount).HasMaxLength(50);

                entity.Property(e => e.MemberName).HasMaxLength(50);

                entity.Property(e => e.MemberPhotoFile).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.RegisterTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<OptionsToProduct>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.OptionGroupFid).HasColumnName("OptionGroup_fid");

                entity.Property(e => e.ProductFid).HasColumnName("Product_fid");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.BMemberName)
                    .HasMaxLength(50)
                    .HasColumnName("B_MemberName");

                entity.Property(e => e.BMemberPhone)
                    .HasMaxLength(50)
                    .HasColumnName("B_MemberPhone");

                entity.Property(e => e.Memo).HasMaxLength(100);

                entity.Property(e => e.NFid).HasColumnName("N_fid");

                entity.Property(e => e.OrderISerialId)
                    .HasMaxLength(10)
                    .HasColumnName("OrderI_SerialID")
                    .HasDefaultValueSql("([dbo].[udf_get_serialno]())")
                    .IsFixedLength();

                entity.Property(e => e.OrderState).HasMaxLength(20);

                entity.Property(e => e.OrderTime).HasColumnType("datetime");

                entity.Property(e => e.PayTermCatId).HasColumnName("PayTermCatID");

                entity.Property(e => e.PickUpDate).HasColumnType("date");

                entity.Property(e => e.PickUpPerson).HasMaxLength(50);

                entity.Property(e => e.PickUpPersonPhone)
                    .HasMaxLength(50)
                    .HasColumnName("PickUpPerson_Phone");

                entity.Property(e => e.PickUpType).HasMaxLength(20);

                entity.Property(e => e.PlasticBag).HasMaxLength(5);

                entity.Property(e => e.TaxIdnum)
                    .HasMaxLength(10)
                    .HasColumnName("TaxIDNum");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OrderDetail");

                entity.Property(e => e.ItemId).HasMaxLength(10);

                entity.Property(e => e.OptionFid).HasColumnName("Option_fid");

                entity.Property(e => e.OptionGroupFid).HasColumnName("OptionGroup_fid");

                entity.Property(e => e.ProductFid).HasColumnName("Product_fid");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ItemId)
                    .HasMaxLength(10)
                    .HasColumnName("itemID");

                entity.Property(e => e.OrderFid).HasColumnName("Order_fid");
            });

            modelBuilder.Entity<OrderSerialNumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OrderSerialNumber");

                entity.Property(e => e.OrderISerialId)
                    .HasMaxLength(10)
                    .HasColumnName("OrderI_SerialID")
                    .IsFixedLength();
            });

            modelBuilder.Entity<PaymentTerm2BusiMember>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PaymentTerm2BusiMember");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.PayAmountLimit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PayAmount_Limit");

                entity.Property(e => e.PayTermCatId).HasColumnName("PayTermCatID");
            });

            modelBuilder.Entity<PaymentTermCategory>(entity =>
            {
                entity.ToTable("PaymentTermCategory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PaymentType).HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.CategoryFid).HasColumnName("Category_fid");

                entity.Property(e => e.IsForSale).HasMaxLength(1);

                entity.Property(e => e.Memo).HasMaxLength(100);

                entity.Property(e => e.MenuFid)
                    .HasMaxLength(50)
                    .HasColumnName("Menu_fid");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("ProductCategory");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.CategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<ProductImg>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("ProductImg");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.ImgPath).HasMaxLength(50);

                entity.Property(e => e.ProductFid).HasColumnName("Product_fid");
            });

            modelBuilder.Entity<ProductOption>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.OptionGroupFid).HasColumnName("OptionGroup_fid");

                entity.Property(e => e.OptionName).HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ProductOptionGroup>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("ProductOptionGroup");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.Memo).HasMaxLength(50);

                entity.Property(e => e.OptionGroupName).HasMaxLength(50);
            });

            modelBuilder.Entity<Sysdiagram>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sysdiagrams");

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.Property(e => e.DiagramId).HasColumnName("diagram_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.PrincipalId).HasColumnName("principal_id");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            modelBuilder.Entity<ViewOptionsToGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_OptionsToGroups");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Memo).HasMaxLength(50);

                entity.Property(e => e.OptionGroupFid).HasColumnName("OptionGroup_fid");

                entity.Property(e => e.OptionGroupName).HasMaxLength(50);

                entity.Property(e => e.OptionName).HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ViewOrderDetailList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_OrderDetailList");

                entity.Property(e => e.Itemid)
                    .HasMaxLength(10)
                    .HasColumnName("itemid");

                entity.Property(e => e.ProductFId).HasColumnName("Product_fID");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.SubTotal)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("subTotal");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(38, 2)");
            });

            modelBuilder.Entity<ViewOrderDetailNonOptionGroupName>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_OrderDetail_NonOptionGroupName");

                entity.Property(e => e.Itemid)
                    .HasMaxLength(10)
                    .HasColumnName("itemid");

                entity.Property(e => e.OptionUp)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("OptionUP");

                entity.Property(e => e.ProductFId).HasColumnName("Product_fID");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.ProductUp)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("ProductUP");
            });

            modelBuilder.Entity<ViewOrderDetailWithOptionGroupName>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_OrderDetail_with_OptionGroupName");

                entity.Property(e => e.ItemId).HasMaxLength(10);

                entity.Property(e => e.OUp)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("O_UP");

                entity.Property(e => e.OptionGroupName).HasMaxLength(50);

                entity.Property(e => e.OptionName).HasMaxLength(50);

                entity.Property(e => e.OrderFid).HasColumnName("Order_fid");

                entity.Property(e => e.PUnitPrice)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("P_UnitPrice");

                entity.Property(e => e.ProductFid).HasColumnName("Product_fid");

                entity.Property(e => e.ProductName).HasMaxLength(50);
            });

            modelBuilder.Entity<ViewShowProductList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_showProductList");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.CategoryFid).HasColumnName("Category_fid");

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.IsForSale)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Memo).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
