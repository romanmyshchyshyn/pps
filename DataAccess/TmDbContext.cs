using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class TmDbContext : IdentityDbContext
    {
        public TmDbContext(DbContextOptions<TmDbContext> options)
            : base(options)
        {
        }
    }
}
