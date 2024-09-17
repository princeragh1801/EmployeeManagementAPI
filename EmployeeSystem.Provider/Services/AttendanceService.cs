namespace EmployeeSystem.Provider.Services;
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
                    Id = a.Id,
                    DateOnly = a.DateOnly
                }).AsNoTracking().ToListAsync();
            response.Message = "Attendance Fetched";
            response.Data = attendances;
            if (attendances == null)
            {
                response.Status = 404;
            }

            return response;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<int>> Add(int employeeId)
    {
        try
        {
            var response = new ApiResponse<int>();
            var checkAlreadyAdded = await _context.Attendances
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            var todaysDate = DateOnly.FromDateTime(DateTime.Now);
            if (checkAlreadyAdded != null && todaysDate == checkAlreadyAdded.DateOnly)
            {
                response.Status = 409;
                response.Message = "Attendance already added";
                response.Data = checkAlreadyAdded.Id;
                return response;
            }

            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                DateOnly = DateOnly.FromDateTime(DateTime.Now)
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
