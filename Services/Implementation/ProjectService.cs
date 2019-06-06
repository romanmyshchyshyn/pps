﻿using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using Services.Exceptions;
using Services.Filters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Services.Implementation
{
    public class ProjectService : Service<Project, ProjectDto, ProjectFilter>, IProjectService
    {
        public ProjectService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {            
        }        

        public override ProjectDto Get(string id)
        {
            Project entity = Repository
              .Get(e => e.Id == id)
              .Include(e => e.CustomTasks)
              .ThenInclude<Project, CustomTask, User>(ct => ct.UserAssignee)
              .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<ProjectDto> Get(ProjectFilter filter)
        {
            Func<Project, bool> predicate = GetFilter(filter);
            List<Project> entities = Repository
              .Get(p => predicate(p))
              .Include(e => e.CustomTasks)
              .ThenInclude<Project, CustomTask, User>(ct => ct.UserAssignee)
              .ToList();

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(ProjectDto dto)
        {
            Project checkEntity = Repository
                .Get(e => e.Id == dto.Id || e.Name == dto.Name)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Project entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            Project entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(ProjectDto dto)
        {
            Project entity = Repository
             .Get(e => e.Id == dto.Id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Mapper.Map<ProjectDto, Project>(dto, entity);

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override ProjectDto MapToDto(Project entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
                       
            return Mapper.Map<Project, ProjectDto>(entity);
        }

        protected override Project MapToEntity(ProjectDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            return Mapper.Map<ProjectDto, Project>(dto);
        }

        private Func<Project, bool> GetFilter(ProjectFilter filter)
        {
            Func<Project, bool> result = e => true;
            if (!String.IsNullOrEmpty(filter?.Name))
            {
                result += e => e.Name == filter.Name;
            }

            return result;
        }
    }
}
