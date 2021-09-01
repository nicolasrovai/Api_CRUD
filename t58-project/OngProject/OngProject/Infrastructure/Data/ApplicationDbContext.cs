using Microsoft.EntityFrameworkCore;
using OngProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OngProject.Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserModel>()
                .HasIndex(u => u.email)
                .IsUnique();

            base.OnModelCreating(builder);
            this.SeedActivities(builder);
            this.SeedRoles(builder);
            this.SeedUsers(builder);

            this.SeedContacts(builder);
            this.SeedSlides(builder);
            this.SeedComments(builder);
            this.SeedTestimonials(builder);
            this.SeedMembers(builder);
            this.SeedCategories(builder);
            this.SeedNews(builder);
            this.SeedOrganization(builder);
        }
        public DbSet<MemberModel> Members { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<OrganizationModel> Organizations { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<SlideModel> Slides { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ContactsModel> Contacts { get; set; }
        public DbSet<ActivitiesModel> Activities { get; set; }
        public DbSet<NewsModel> News { get; set; }
        public DbSet<TestimonialsModel> Testimonials { get; set; }

        private void SeedActivities(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<ActivitiesModel>().HasData(
                    new ActivitiesModel
                    {
                        Id = i,
                        Name = "Activity " + i,
                        Image = null,
                        Content = "Content from activity " + i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 3; i++)
            {
                modelBuilder.Entity<RoleModel>().HasData(
                    new RoleModel
                    {
                        Id = i,
                        Name = i == 1 ? "Admin" : "Standard",
                        Description = i == 1 ? "Admin User" : "Standard User",
                        IsDeleted = false,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            string photo="";
            for (int i = 1; i < 21; i++)
            {
                switch (i)
                {
                    case 1: photo = "https://alkemy-ong.s3.amazonaws.com/user_27082021235906"; break;
                    case 2: photo = "https://alkemy-ong.s3.amazonaws.com/user_27082021235137"; break;
                    case 3: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100425"; break;
                    case 4: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100449"; break;
                    case 5: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100509"; break;
                    case 6: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100531"; break;
                    case 7: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100550"; break;
                    case 8: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100611"; break;
                    case 9: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100628"; break;
                    case 10: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100647"; break;
                    case 11: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100705"; break;
                    case 12: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100729"; break;
                    case 13: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100747"; break;
                    case 14: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100820"; break;
                    case 15: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100839"; break;
                    case 16: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100857"; break;
                    case 17: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100916"; break;
                    case 18: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202100942"; break;
                    case 19: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202101137"; break;
                    case 20: photo = "https://alkemy-ong.s3.amazonaws.com/user_2808202101201"; break;
                }
                modelBuilder.Entity<UserModel>().HasData(
                    new UserModel
                    {
                        Id = i,
                        firstName = "User " + i,
                        lastName = i < 11 ? "AdminUser " + i : "RegularUser " + i,
                        email = "mail" + i + "@Mail.com",
                        password = i < 11 ? UserModel.ComputeSha256Hash("Admin123") : UserModel.ComputeSha256Hash("User123"),
                        photo = photo,
                        roleId = i < 11 ? 1 : 2,
                        CreatedAt = DateTime.Now,
                        IsDeleted = false
                    }
                );
            }
        }



        public void SeedNews(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<NewsModel>().HasData(
                    new NewsModel
                    {
                        Id = i,
                        Name = "new's name ",
                        Content = "Content " + i + " Lorem ipsum dolor sit amet,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Image = "imageNewsNoValida" + i + ".jpg",
                        CategoryId = i,
                        CreatedAt = DateTime.Now,
                        IsDeleted = false

                    });
            }

            // add more new to categories 1

            for (int j = 11; j < 15; j++)
            {
                for (int i = 1; i < 3; i++, j++)
                {
                    modelBuilder.Entity<NewsModel>().HasData(
                           new NewsModel
                           {
                               Id = j,
                               Name = "new's name ",
                               Content = "Lorem ipsum dolor sit amet,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                               Image = "imageNews" + j + ".jpg",
                               CategoryId = i,
                               CreatedAt = DateTime.Now,
                               IsDeleted = false

                           });

                }

            }

        }
        public void SeedCategories(ModelBuilder modelBuilder)
        {
            string image = "";
            for (int i = 1; i < 11; i++)
            {
                switch (i)
                {
                    case 1: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132235"; break;
                    case 2: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132437"; break;
                    case 3: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132452"; break;
                    case 4: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132503"; break;
                    case 5: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132514"; break;
                    case 6: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132524"; break;
                    case 7: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132538"; break;
                    case 8: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132550"; break;
                    case 9: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132601"; break;
                    case 10: image = "https://alkemy-ong.s3.amazonaws.com/category_2808202132612"; break;
                }
                modelBuilder.Entity<CategoryModel>().HasData(
                    new CategoryModel
                    {
                        Id = i,
                        Name = "name " + i,
                        Description = "Descripcion " + i + "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
                        Image = image,
                        CreatedAt = DateTime.Now,
                        IsDeleted = false
                    }
                    );
            }
        }

        public void SeedMembers(ModelBuilder modelBuilder)
        {
            string image = "";
            for (int i = 1; i < 11; i++)
            {
                switch (i)
                {
                    case 1: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202130915"; break;
                    case 2: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202131017"; break;
                    case 3: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202131043"; break;
                    case 4: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202131054"; break;
                    case 5: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202131104"; break;
                    case 6: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202131112"; break;
                    case 7: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202131123"; break;
                    case 8: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202131133"; break;
                    case 9: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202131143"; break;
                    case 10: image = "https://alkemy-ong.s3.amazonaws.com/member_2808202131154"; break;
                }
                modelBuilder.Entity<MemberModel>().HasData(
                    new MemberModel
                    {
                        Id = i,
                        Name = "name" + i,
                        FacebookUrl = "https://facebook.com/member" + i,
                        InstagramUrl = "https://instagram/member" + i,
                        LinkedinUrl = "https://Linkedin/member" + i,
                        Image = image,
                        Description = "Descripcion" + i + "Lorem ipsum dolor sit amet,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        CreatedAt = DateTime.Now,
                        IsDeleted = false

                    }
                    );
            }
        }
        public void SeedTestimonials(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<TestimonialsModel>().HasData(
                    new TestimonialsModel
                    {
                        Id = i,
                        Name = "name " + i,
                        Image = "imageTestimonials" + i + ".jpg",
                        Content = "Content" + i + "Lorem ipsum dolor sit amet,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        CreatedAt = DateTime.Now,
                        IsDeleted = false
                    }
                    );
            }
        }


        public void SeedComments(ModelBuilder modelBuilder)
        {
            int k = 1;
            for (int i = 1; i < 11; i++) //post, usuario
            {
                for (int j = 1; j < 4; j++, k++) // agrega 3 comentarios para cada post_id o user_id
                    modelBuilder.Entity<CommentModel>().HasData(
                        new CommentModel
                        {
                            Id = k,
                            User_id = i,
                            post_id = i,
                            Body = "body of post_id=" + i,
                            CreatedAt = DateTime.Now,
                            IsDeleted = false
                        }
                        );
            }

        }


        public void SeedOrganization(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OrganizationModel>().HasData(
               new OrganizationModel
               {
                   Id = 1,
                   Name = "Somos Más",
                   Image = "imageOrganization.jpg",
                   Adress = "Catamarca 1585 , CP: 1585",
                   Phone = 44808900,
                   Email = "somomasong@gmail.com",
                   WelcomeText = "Bienvenidos a nuestro sitio web",
                   AboutUsText = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                   FacebookUrl = "https://facebook.com/organization",
                   CreatedAt = DateTime.Now,
                   IsDeleted = false
               });

        }
        private void SeedSlides(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<SlideModel>().HasData(
                    new SlideModel
                    {
                        Id = i,
                        ImageUrl = "imagenSlides" + i + ".jpg",
                        Order = i,
                        Text = "sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium",
                        OrganizationId = i.ToString(),
                        CreatedAt = DateTime.Now,
                        IsDeleted = false
                    }
                    );
            }

        }
        private void SeedContacts(ModelBuilder modelBuilder)
        {
            Random rPhone = new Random();

            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<ContactsModel>().HasData(
                    new ContactsModel
                    {
                        Id = i,
                        Name = "Contact " + i,
                        Phone = rPhone.Next(11111111, 99999999),
                        Email = "email" + i + "gmail.com",
                        Message = "Message Message Message Message" + i,
                        CreatedAt = DateTime.Now,
                        IsDeleted = false
                    }
                    );
            }

        }
    }
}
