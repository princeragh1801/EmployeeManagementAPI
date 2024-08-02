﻿using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using EmployeeSystem.Contract.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Provider.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ApiResponse<List<AttendanceDto>>> GetByEmployeeId(int employeeId)
        {
            try
            {
                var response = new ApiResponse<List<AttendanceDto>>();

                var attendances = await _context.Attendances
                    .Where(a => a.EmployeeId == employeeId)
                    .Select(a => new AttendanceDto
                    {
                        DateOnly = a.DateOnly
                    }).ToListAsync();
                response.Message = "Attendance Fetched";
                response.Data = attendances;
                if(attendances == null)
                {
                    response.Status = 404;
                }

                return response;

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> Add(int employeeId)
        {
            try
            {
                var response = new ApiResponse<int>();
                var attendance = new Attendance
                {
                    EmployeeId = employeeId
                };

                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();

                response.Message = "Attendance added";
                response.Data = attendance.Id;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
