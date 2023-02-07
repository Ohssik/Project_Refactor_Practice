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
        public virtual DbSet<ChangeRequestPassword> ChangeRequestPasswords { get; set; } = null!;
        public virtual DbSet<Chat2User> Chat2Users { get; set; } = null!;
        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public virtual DbSet<Chatroom> Chatrooms { get; set; } = null!;
        public virtual DbSet<ChatroomUser> ChatroomUsers { get; set; } = null!;
        public virtual DbSet<Coupon> Coupons { get; set; } = null!;
        public virtual DbSet<Coupon2NormalMember> Coupon2NormalMembers { get; set; } = null!;
        public virtual DbSet<NormalMember> NormalMembers { get; set; } = null!;
        public virtual DbSet<OptionsToProduct> OptionsToProducts { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<OrderOptionsDetail> OrderOptionsDetails { get; set; } = null!;
        public virtual DbSet<OrderSerialNumber> OrderSerialNumbers { get; set; } = null!;
        public virtual DbSet<PaymentTerm2BusiMember> PaymentTerm2BusiMembers { get; set; } = null!;
        public virtual DbSet<PaymentTermCategory> PaymentTermCategories { get; set; } = null!;
        public virtual DbSet<ProblemAnswer> ProblemAnswers { get; set; } = null!;
        public virtual DbSet<ProblemQuestion> ProblemQuestions { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<ProductOption> ProductOptions { get; set; } = null!;
        public virtual DbSet<ProductOptionGroup> ProductOptionGroups { get; set; } = null!;
        public virtual DbSet<ViewOptionsToGroup> ViewOptionsToGroups { get; set; } = null!;
        public virtual DbSet<ViewOrderDetail> ViewOrderDetails { get; set; } = null!;
        public virtual DbSet<ViewOrderDetailList> ViewOrderDetailLists { get; set; } = null!;
        public virtual DbSet<ViewOrderDetailNonOptionGroupName> ViewOrderDetailNonOptionGroupNames { get; set; } = null!;
        public virtual DbSet<ViewShowFullOrder> ViewShowFullOrders { get; set; } = null!;
        public virtual DbSet<ViewShowProductList> ViewShowProductLists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=ispan-msit145-shiba2.database.windows.net;Initial Catalog=ispanMsit145shiba;Persist Security Info=True;User ID=msit145Shiba;Password=sh1baMsite45");
            }
        }

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

                entity.Property(e => e.OrderBy)
                    .HasColumnName("orderBy")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");
            });

            modelBuilder.Entity<AdminMember>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("AdminMember");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Account).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

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

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmailCertified)
                    .HasColumnName("emailCertified")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Gps)
                    .HasMaxLength(300)
                    .HasColumnName("GPS");

                entity.Property(e => e.IsOpened)
                    .HasColumnName("isOpened")
                    .HasDefaultValueSql("((0))");

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

            modelBuilder.Entity<ChangeRequestPassword>(entity =>
            {
                entity.HasKey(e => e.Token);

                entity.ToTable("ChangeRequestPassword");

                entity.Property(e => e.Token).HasMaxLength(50);

                entity.Property(e => e.Account).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Expire)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(dateadd(minute,(10),getdate()))");
            });

            modelBuilder.Entity<Chat2User>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("Chat2User");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.Chat2Users)
                    .HasForeignKey(d => d.Chatid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat2User_Chatroom");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Chat2Users)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat2User_ChatroomUser");
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("ChatMessage");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.SendTime).HasColumnType("datetime");

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.Chatid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_Chatroom");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.Senderid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_ChatroomUser");
            });

            modelBuilder.Entity<Chatroom>(entity =>
            {
                entity.HasKey(e => e.Chatid);

                entity.ToTable("Chatroom");

                entity.Property(e => e.ChatName).HasMaxLength(50);
            });

            modelBuilder.Entity<ChatroomUser>(entity =>
            {
                entity.ToTable("ChatroomUser");

                entity.Property(e => e.LastOnlineTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.CouponCode)
                    .HasMaxLength(100)
                    .HasColumnName("couponCode");

                entity.Property(e => e.IsUsed)
                    .HasColumnName("isUsed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Memo)
                    .HasMaxLength(200)
                    .HasColumnName("memo");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Coupon2NormalMember>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("Coupon2NormalMember");

                entity.Property(e => e.Fid).HasColumnName("fid");

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
                    .HasDefaultValueSql("((1))");

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
                entity.HasKey(e => e.Fid);

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.OptionGroupFid).HasColumnName("OptionGroup_fid");

                entity.Property(e => e.ProductFid).HasColumnName("Product_fid");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

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

                entity.Property(e => e.TaxIdnum)
                    .HasMaxLength(10)
                    .HasColumnName("TaxIDNum");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.OrderFid).HasColumnName("Order_fid");

                entity.Property(e => e.ProductFid).HasColumnName("Product_fid");
            });

            modelBuilder.Entity<OrderOptionsDetail>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("OrderOptionsDetail");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.ItemFid).HasColumnName("item_fid");

                entity.Property(e => e.OptionFid).HasColumnName("Option_fid");
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
                entity.HasKey(e => e.Fid);

                entity.ToTable("PaymentTerm2BusiMember");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.PayAmountLimit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PayAmount_Limit");

                entity.Property(e => e.PayTermCatId).HasColumnName("PayTermCatID");
            });

            modelBuilder.Entity<PaymentTermCategory>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.ToTable("PaymentTermCategory");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.PaymentType).HasMaxLength(50);
            });

            modelBuilder.Entity<ProblemAnswer>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ProblemAnswer");

                entity.Property(e => e.Fid).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ProblemQuestion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ProblemQuestion");

                entity.Property(e => e.AnswerFid).HasColumnName("answer_Fid");

                entity.Property(e => e.Fid).ValueGeneratedOnAdd();

                entity.Property(e => e.Question).HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Fid);

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.CategoryFid).HasColumnName("Category_fid");

                entity.Property(e => e.IsForSale).HasDefaultValueSql("((0))");

                entity.Property(e => e.Memo).HasMaxLength(100);

                entity.Property(e => e.Photo).HasMaxLength(50);

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

                entity.Property(e => e.IsMultiple)
                    .HasColumnName("isMultiple")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Memo).HasMaxLength(50);

                entity.Property(e => e.OptionGroupName).HasMaxLength(50);
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

            modelBuilder.Entity<ViewOrderDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_OrderDetail");

                entity.Property(e => e.ItemFid).HasColumnName("item_fid");

                entity.Property(e => e.OUp)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("O_UP");

                entity.Property(e => e.OpGroupFid).HasColumnName("OpGroup_fid");

                entity.Property(e => e.OptionFid).HasColumnName("Option_fid");

                entity.Property(e => e.OptionGroupName).HasMaxLength(50);

                entity.Property(e => e.OptionName).HasMaxLength(50);

                entity.Property(e => e.OrderFid).HasColumnName("Order_fid");

                entity.Property(e => e.PUnitPrice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("P_UnitPrice");

                entity.Property(e => e.ProductFid).HasColumnName("Product_fid");

                entity.Property(e => e.ProductName).HasMaxLength(50);
            });

            modelBuilder.Entity<ViewOrderDetailList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_OrderDetailList");

                entity.Property(e => e.ItemFid).HasColumnName("item_fid");

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

                entity.Property(e => e.ItemFid).HasColumnName("item_fid");

                entity.Property(e => e.OptionUp)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("OptionUP");

                entity.Property(e => e.ProductFId).HasColumnName("Product_fID");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.ProductUp)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ProductUP");
            });

            modelBuilder.Entity<ViewShowFullOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_ShowFullOrder");

                entity.Property(e => e.BFid).HasColumnName("B_fid");

                entity.Property(e => e.BMemberName)
                    .HasMaxLength(50)
                    .HasColumnName("B_MemberName");

                entity.Property(e => e.BMemberPhone)
                    .HasMaxLength(50)
                    .HasColumnName("B_MemberPhone");

                entity.Property(e => e.ItemFid).HasColumnName("item_fid");

                entity.Property(e => e.Memo).HasMaxLength(100);

                entity.Property(e => e.NFid).HasColumnName("N_fid");

                entity.Property(e => e.OrderFid).HasColumnName("order_fid");

                entity.Property(e => e.OrderISerialId)
                    .HasMaxLength(10)
                    .HasColumnName("OrderI_SerialID")
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

                entity.Property(e => e.ProductFId).HasColumnName("Product_fID");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.SubTotal)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("subTotal");

                entity.Property(e => e.TaxIdnum)
                    .HasMaxLength(10)
                    .HasColumnName("TaxIDNum");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(38, 2)");
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
