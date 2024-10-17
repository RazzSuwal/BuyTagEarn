using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SMM.DataAccessLayer.Services.IServices;
using System.Data;

namespace SMM.DataAccessLayer.Services.Services
{
    public class PostService : IPostService
    {
        private readonly IConfiguration _configuration;

        public PostService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<dynamic> GetAllUserPost(string? type)
        {
            try
            {

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = "";
                    if (type == "Approved")
                    {
                        query = @"SELECT * FROM [dbo].[UserPost] WHERE IsApproved = 1";
                    }
                    else
                    {
                        //query = @"SELECT * FROM [dbo].[UserPost] WHERE IsApproved = 0";
                        query = @"SELECT * FROM [dbo].[UserPost]";
                    }

                    // Fetch the result
                    var result = await dbConnection.QueryAsync<dynamic>(query);
                    if (result == null || !result.Any())
                    {
                        return "No records found for the given Type";
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }

        public async Task<dynamic> AprovedUserPost(int postId, int IsApproved)
        {
            try
            {

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = "";
                    if (IsApproved == 1)
                    {
                        query = @"UPDATE [dbo].[UserPost] SET IsApproved = 1 WHERE UserPostId = @PostId";

                    }
                    else
                    {
                        query = @"UPDATE [dbo].[UserPost] SET IsApproved = 0 WHERE UserPostId = @PostId";
                    }
                    var parameters = new
                    {
                        PostId = postId
                    };

                    // Fetch the result
                    var result = await dbConnection.QueryAsync<dynamic>(query, parameters);

                    return "Approved Sucessfully";
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }


    }
}
