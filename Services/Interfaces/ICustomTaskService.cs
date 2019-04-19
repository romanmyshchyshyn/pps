using Services.Dto;
using Services.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface ICustomTaskService : IService<CustomTaskDto, CustomTaskFilter>
    {
    }
}
