using FinalProject.Model;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserWishList> UserWishlists { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Product> Products { get; set; }
       

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }     
        public DbSet<FeaturedProduct> FeaturedProducts { get; set; }
        public DbSet<BigSaleProduct> BigSaleProducts { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserAddres> UserAddres { get; set; }
        public DbSet<GendrType> GendrTypes { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<HomeAbout> HomeAbouts { get; set; }
        public DbSet<HomeFag> HomeFags { get; set; }
        public DbSet<FAG> Fags { get; set; }
        public DbSet<TeamAbout> TeamAbouts { get; set; }
        public DbSet<Team> Teams { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Surname).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Email).IsRequired().HasMaxLength(100);

                entity.Property(p => p.ProfilePicture).HasMaxLength(1000);

                entity.Property(p => p.Phone).IsRequired().HasMaxLength(15);

                entity.Property(p => p.PasswordHash).IsRequired().HasMaxLength(32);

                entity.Property(p => p.Salt).IsRequired();

                entity.Property(p => p.UserRoleId).IsRequired().HasDefaultValue(false);

                entity.Property(p => p.EmailConfirmed).IsRequired();

            });
            #endregion

            #region TeamAbout
            modelBuilder.Entity<TeamAbout>(entity =>
            {
               entity.Property(p => p.Title).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Description).IsRequired().HasMaxLength(100);           
             
                entity.Property(p => p.ImageUrl).IsRequired().HasMaxLength(32);




            });
            #endregion

            #region Team
            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(p => p.TeamMemberPosition).IsRequired().HasMaxLength(100);

                entity.Property(p => p.TeamMemberName).IsRequired().HasMaxLength(100);

                entity.Property(p => p.profileImageUrl).IsRequired().HasMaxLength(32);
                entity.Property(p => p.TeamMemberDescription).IsRequired().HasMaxLength(100);




            });
            #endregion



            #region Contact
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);

                entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Email).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Subject).HasMaxLength(1000);

                entity.Property(p => p.Message).IsRequired().HasMaxLength(15);

                

            });
            #endregion

            #region Support
            modelBuilder.Entity<Support>(entity =>
            {
                entity.Property(p => p.Title).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Description).IsRequired().HasMaxLength(100);


            });
            #endregion

            #region HomeFag
            modelBuilder.Entity<HomeFag>(entity =>
            {
                entity.Property(p => p.Title).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Description).IsRequired().HasMaxLength(100);


            });
            #endregion

            #region Fag
            modelBuilder.Entity<FAG>(entity =>
            {
                entity.Property(p => p.QuestionsTitle).IsRequired().HasMaxLength(100);

                entity.Property(p => p.QuestionsDescription).IsRequired().HasMaxLength(100);


            });
            #endregion

            #region HomeAbout
            modelBuilder.Entity<HomeAbout>(entity =>
            {
                entity.Property(p => p.Title).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Description).IsRequired().HasMaxLength(100);


            });
            #endregion

            #region User address
            modelBuilder.Entity<UserAddres>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);

                entity.Property(p => p.Addres).IsRequired().HasMaxLength(500);

                entity.Property(p => p.Phone).IsRequired().HasMaxLength(15);

                entity.Property(p => p.CityId).IsRequired();

                entity.Property(p => p.UserId).IsRequired();
            });
            #endregion

            #region User role
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            });
            #endregion

            #region Featured products
            modelBuilder.Entity<FeaturedProduct>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);

                entity.Property(p => p.Slug).IsRequired().HasMaxLength(500);

                entity.Property(p => p.ImageURL).IsRequired().HasMaxLength(1000);

                entity.Property(p => p.Price).IsRequired();

                entity.Property(p => p.ProductId).IsRequired();
            });
            #endregion

            #region BigSale products

            modelBuilder.Entity<BigSaleProduct>(entity =>
            {
                entity.Property(p => p.Title).IsRequired().HasMaxLength(200);

                entity.Property(p => p.DiscountInfo).IsRequired().HasMaxLength(500);

                entity.Property(p => p.ImageURL).IsRequired().HasMaxLength(1000);

                entity.Property(p => p.Link).IsRequired();
                entity.Property(p => p.Description).IsRequired().HasMaxLength(500);



            });
            #endregion

            

            #region Slider
            modelBuilder.Entity<Slider>(entity =>
            {
                entity.Property(p => p.Title).IsRequired().HasMaxLength(200);

                entity.Property(p => p.Slogan).IsRequired().HasMaxLength(500);

                entity.Property(p => p.Link).IsRequired();

                entity.Property(p => p.BackgroundImageUrl).IsRequired().HasMaxLength(1000);
            });
            #endregion

            
            #region ProductVariant
            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(500);
            });
            #endregion



            #region Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.ProductVariantId).IsRequired();

                entity.Property(p => p.Name).IsRequired().HasMaxLength(1000);

                entity.Property(p => p.Slug).IsRequired().HasMaxLength(1000);

                entity.Property(p => p.Barcode).IsRequired().HasMaxLength(50);

                entity.Property(p => p.CategoryId).IsRequired();

                entity.Property(p => p.GendrTypeId).IsRequired();

                entity.Property(p => p.Description).IsRequired().HasMaxLength(1000);

                entity.Property(p => p.SellAmount).IsRequired();
                entity.Property(p => p.TotalPrice).IsRequired();

                entity.Property(p => p.BuyAmount).IsRequired();

                entity.Property(p => p.BuyLimit).IsRequired().HasDefaultValue(10);

                entity.Property(p => p.Quantity).IsRequired();

                entity.Property(p => p.InStock).IsRequired();

                entity.Property(p => p.HasShipping).IsRequired();
            });
            #endregion
            #region Product Cart
            modelBuilder.Entity<Cart>(entity =>
            {            
                entity.Property(p => p.Name).IsRequired().HasMaxLength(1000);
                
                entity.Property(p => p.Barcode).IsRequired().HasMaxLength(50);

                entity.Property(p => p.SellAmount).IsRequired();

              entity.Property(p => p.Quantity).IsRequired();

                entity.Property(p => p.TotalPrice).IsRequired();



            });
            #endregion

            #region Product photo 
            modelBuilder.Entity<ProductPhoto>(entity =>
            {
                entity.Property(p => p.ImageUrl).IsRequired().HasMaxLength(1000);

                entity.Property(p => p.ProductId).IsRequired();
            });
            #endregion

            #region Product review
            modelBuilder.Entity<ProductReview>(entity =>
            {
                entity.Property(p => p.ProductId).IsRequired();

                entity.Property(p => p.UserId).IsRequired();

                entity.Property(p => p.Text).IsRequired().HasMaxLength(500);

                entity.Property(p => p.ProductReviewStatusId).IsRequired();
            });
            #endregion

            #region Product option
            modelBuilder.Entity<ProductOption>(entity =>
            {
                entity.Property(p => p.ProductId).IsRequired();
                entity.Property(p => p.OptionId).IsRequired();

            });
            #endregion

            #region Option
            modelBuilder.Entity<Option>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);

                entity.Property(p => p.OptionGroupId).IsRequired();
               
            });
            #endregion

            #region Option group
            modelBuilder.Entity<OptionGroup>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
               
            });
            #endregion

            #region Gender type
            modelBuilder.Entity<GendrType>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
            });
            #endregion

            #region City
            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);

                entity.Property(p => p.CountryId).IsRequired();
            });
            #endregion

            #region Country
            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            });
            #endregion

            #region Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);

                entity.Property(p => p.ImageUrl).HasMaxLength(1000);

                entity.Property(p => p.Slogan).HasMaxLength(1000);

                entity.Property(p => p.IsMainPage).IsRequired().HasDefaultValue(false);
            });
            #endregion
        }
    }
}
