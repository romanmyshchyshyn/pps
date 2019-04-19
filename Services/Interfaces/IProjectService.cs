using Services.Dto;
using Services.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IProjectService : IService<ProjectDto, ProjectFilter>
    {
    }
}
