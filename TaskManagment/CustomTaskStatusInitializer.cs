using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagment
{
    public class CustomTaskStatusInitializer
    {
        public static void Initialize(TmDbContext db)
        {
            if (!db.CustomTaskStatuses.Any())
            {
                db.CustomTaskStatuses.AddRange(new List<CustomTaskStatus>
                {
                    new CustomTaskStatus {Name = "Backlog", Index = 1},
                    new CustomTaskStatus {Name = "In Progress", Index = 2},
                    new CustomTaskStatus {Name = "Completed", Index = 3}
                });

                db.SaveChanges();
            }
        }
    }
}
