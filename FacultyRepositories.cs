using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using COLLAGEAPI.Models;

namespace COLLAGEAPI.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly IConfiguration _configuration;

        public FacultyRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Create SQL connection
        private IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("CollegeDB")
            );
        }

        // =========================
        // GET ALL FACULTIES
        // =========================
        public async Task<IEnumerable<Faculty>> GetFaculties()
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<Faculty>(
                "sp_GetFaculties",
                commandType: CommandType.StoredProcedure
            );
        }

        // =========================
        // GET FACULTY BY ID
        // =========================
        public async Task<Faculty?> GetFacultyById(int id)
        {
            using var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Faculty>(
                "sp_GetFacultyById",
                new
                {
                    FacultyID = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // =========================
        // ADD FACULTY
        // =========================
        public async Task<int> AddFaculty(Faculty faculty)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                "sp_AddFaculty",
                new
                {
                    faculty.FacultyName,
                    faculty.Email,
                    faculty.Phone,
                    faculty.DepartmentID,
                    faculty.CreatedBy
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // =========================
        // UPDATE FACULTY
        // =========================
        public async Task<int> UpdateFaculty(Faculty faculty)
        {
            using var connection = CreateConnection();

            int rowsAffected = await connection.ExecuteAsync(
                "sp_UpdateFaculty",
                new
                {
                    faculty.FacultyID,
                    faculty.FacultyName,
                    faculty.Email,
                    faculty.Phone,
                    faculty.DepartmentID,
                    faculty.IsActive,
                    faculty.ModifiedBy
                },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }

        // =========================
        // DELETE FACULTY
        // =========================
        public async Task<int> DeleteFaculty(int id)
        {
            using var connection = CreateConnection();

            int rowsAffected = await connection.ExecuteAsync(
                "sp_DeleteFaculty",
                new
                {
                    FacultyID = id
                },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }
    }
}