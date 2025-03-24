using Microsoft.AspNetCore.Identity;
using CinemaStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CinemaStore.Data.Static;

namespace CinemaStore.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.Migrate();

                // Proverite i dodajte početne žanrove i formate ako nisu već dodati
                if (!context.Genres.Any())
                {
                    context.Genres.AddRange(new List<Genre>
                    {
                        new Genre { Name = "Fantazija" },
                        new Genre { Name = "Akcija" },
                        new Genre { Name = "Drama" },
                        new Genre { Name = "Triler" },
                        new Genre { Name = "Misterija" },
                        new Genre { Name = "Dokumentarni" },
                        new Genre { Name = "Komedija" },
                        new Genre { Name = "Romantika" },
                        new Genre { Name = "Animirani" },
                        new Genre { Name = "Porodični" },
                        new Genre { Name = "Balet" },
                        new Genre { Name = "Opera" },
                        new Genre { Name = "Predstava" }  
                    });
                    context.SaveChanges();
                }

                if (!context.Formats.Any())
                {
                    context.Formats.AddRange(new List<Format>
                    {
                        new Format { Name = "2D" },
                        new Format { Name = "3D" },
                        new Format { Name = "4D" },
                        new Format { Name = "Uživo" }
                    });
                    context.SaveChanges();
                }

                // Proverite i dodajte početne filmove
                if (!context.Tickets.Any())
                {
                    // Kreirajte Ticket
                    var avatarTicket = new Ticket
                    {
                        Name = "Avatar",
                        Description = "A paraplegic Marine dispatched to the moon Pandora on a unique mission becomes torn between following his orders and protecting the world he feels is his home.",
                        Price = 10,
                        TicketPictureUrl = "https://www.imdb.com/title/tt0499549/mediaviewer/rm2864126209/?ref_=tt_ov_i",
                        Duration = 120,
                        Producer = "James Cameron",
                        Actors = "Sam Worthington, Zoe Saldana, Sigourney Weaver",
                        TicketType = 1
                    };

                    // Dodajte žanrove
                    var fantasyGenre = context.Genres.FirstOrDefault(g => g.Name == "Fantasy");
                    var actionGenre = context.Genres.FirstOrDefault(g => g.Name == "Action");

                    if (fantasyGenre != null && actionGenre != null)
                    {
                        avatarTicket.TicketGenres = new List<TicketGenre>
                        {
                            new TicketGenre { GenreId = fantasyGenre.Id },
                            new TicketGenre { GenreId = actionGenre.Id }
                        };
                    }

                    // Dodajte formate
                    var twoDFormat = context.Formats.FirstOrDefault(f => f.Name == "2D");
                    if (twoDFormat != null && !context.TicketFormats.Any(tf => tf.FormatId == twoDFormat.Id))
                    {
                        var ticketFormat = new TicketFormat
                        {
                            FormatId = twoDFormat.Id,
                            Ticket = avatarTicket, // Povezujemo format sa kartom
                            ShowTimes = new List<TicketFormatShowTime> // Dodajemo termine prikazivanja
                            {
                                new TicketFormatShowTime
                                {
                                    ShowTime = DateTime.Now.AddHours(2),
                                    AvailableSeats = 50
                                }
                            }
                        };

                        avatarTicket.TicketFormats = new List<TicketFormat> { ticketFormat };
                    }

                    context.Tickets.Add(avatarTicket);
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string adminUserEmail = "admin@cinemastore.com";
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Administrator",
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true

                    };
                    await userManager.CreateAsync(newAdminUser, "Admin1234!");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
            }
        }
    }
}


        //public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        //{
        //    using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        //    {
        //        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        //            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        //        if (!await roleManager.RoleExistsAsync(UserRoles.User))
        //            await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        //        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        //        string adminUserEmail = "admin@pcshop.com";
        //        var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
        //        if (adminUser == null)
        //        {
        //            var newAdminUser = new ApplicationUser()
        //            {
        //                FullName = "Administrator",
        //                UserName = "admin",
        //                Email = adminUserEmail,
        //                EmailConfirmed = true

        //            };
        //            await userManager.CreateAsync(newAdminUser, "Admin1234!");
        //            await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
        //        }


        //        string appUserEmail = "user@pcshop.com";
        //        var appUser = await userManager.FindByEmailAsync(appUserEmail);
        //        if (appUser == null)
        //        {
        //            var newAppUser = new ApplicationUser()
        //            {
        //                FullName = "Korisnik",
        //                UserName = "korisnik",
        //                Email = appUserEmail,
        //                EmailConfirmed = true

        //            };
        //            await userManager.CreateAsync(newAppUser, "Korisnik1234!");
        //            await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
        //        }
        //    }
        //}
