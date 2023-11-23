using Domain.Entities;
using Domain.IRepositorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pesistence.AppDbContext;
using Pesistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pesistence.Repositorys
{
    public class EmailSendRepository : EFRepository<EmailSend, int>, IEmailSendRepository
    {
        
        public EmailSendRepository(ApplicationDbContext context) : base(context)
        {
           


        }

        public void AddEmailSend(EmailSend e)
        {
            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            using (var db = new ApplicationDbContext(builder.Options))
            {  
                db.EmailSends.Add(e);
                db.SaveChanges();

            }

             
        }
    }
}
