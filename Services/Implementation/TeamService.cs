using DataAccess.Interfaces;
using DataAccess.Models;
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
    public class TeamService : Service<Team, TeamDto, TeamFilter>, ITeamService
    {
        public TeamService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }        

        public override TeamDto Get(string id)
        {
            Team entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<TeamDto> Get(TeamFilter filter)
        {
            Func<Team, bool> predicate = GetFilter(filter);
            List<Team> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(TeamDto dto)
        {
            Team checkEntity = Repository
                .Get(e => e.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Team entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            Team entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(TeamDto dto)
        {
            Team entity = Repository
            .Get(e => e.Id == dto.Id)
            .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Name = dto.Name;
            entity.ProjectId = dto.ProjectId;
            entity.TeamLeadId = dto.TeamLeadId;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override TeamDto MapToDto(Team entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            TeamDto dto = new TeamDto
            {
                Id = entity.Id,
                Name = entity.Name,
                ProjectId = entity.ProjectId,
                TeamLeadId = entity.TeamLeadId
            };

            return dto;
        }

        protected override Team MapToEntity(TeamDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Team entity = new Team
            {
                Id = dto.Id,
                Name = dto.Name,
                ProjectId = dto.ProjectId,
                TeamLeadId = dto.TeamLeadId
            };

            return entity;
        }

        private Func<Team, bool> GetFilter(TeamFilter filter)
        {
            Func<Team, bool> result = e => true;
            if (!String.IsNullOrEmpty(filter?.Name))
            {
                result += e => e.Name == filter.Name;
            }

            if (!String.IsNullOrEmpty(filter?.ProjectId))
            {
                result += e => e.ProjectId == filter.ProjectId;
            }

            return result;
        }
    }
}
