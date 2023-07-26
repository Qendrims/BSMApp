using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Repo.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (!context.Roles.Any())
                {
                    context.Roles.Add(new IdentityRole()
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    });
                    context.Roles.Add(new IdentityRole()
                    {
                        Name="UserRole",
                        NormalizedName = "UserRole"
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
