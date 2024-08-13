﻿using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Provider.Services
{
    public class SprintService : ISprintService
    {
        private readonly ApplicationDbContext _context;

        public SprintService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<int> Add(AddSprintDto addSprintDto)
        {
            try
            {
                var sprint = new Sprint
                {
                    Name = addSprintDto.Name,
                    StartDate = addSprintDto.StartDate,
                    EndDate = addSprintDto.EndDate,
                };
                _context.Sprints.Add(sprint);
                await _context.SaveChangesAsync();
                return sprint.Id;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SprintInfo>> GetAll()
        {
            try
            {
                var sprints = await _context.Sprints.Select(s => new SprintInfo
                {
                    Name = s.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                }).ToListAsync();
                return sprints;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SprintInfo?> GetById(int id)
        {
            try
            {
                var sprint = await _context.Sprints.FirstOrDefaultAsync(s => s.Id == id);
                if (sprint == null) { return null; }
                return new SprintInfo { Name = sprint.Name, StartDate = sprint.StartDate, EndDate = sprint.EndDate, };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                var sprint = await _context.Sprints.FirstOrDefaultAsync(s => s.Id == id);
                if (sprint == null) return false;
                _context.Sprints.Remove(sprint);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
