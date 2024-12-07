using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SMM.DataAccessLayer.Services.IServices;
using SMM.Models.DTO;
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
                    var result = new List<dynamic>();
                    if (type != null)
                    {
                        query = @"
                            SELECT [UserPostId]
                                   ,[PostUrl]
                                   ,[IsTag]
                                   ,[PostedOn]
                                   ,[IsPaid]
                                   ,up.[IsApproved]
                                   ,up.[ImageUrl]
                                   ,up.[CreatedDate]
                                   ,p.ProductName
                                   ,p.ProductType
                                   ,u.Name as BrandName
                                   ,u2.UserName
                            FROM [dbo].[UserPost] as up
                            LEFT JOIN dbo.Product as p ON p.ProductId = up.ProductId
                            LEFT JOIN dbo.AspNetUsers as u ON u.Id = up.BrandId
                            LEFT JOIN dbo.AspNetUsers as u2 ON u2.Id = up.UserId
                            WHERE u.Id = @Type";
                        result = (await dbConnection.QueryAsync<dynamic>(query, new { Type = type })).ToList();
                    }
                    else
                    {
                        //query = @"SELECT * FROM [dbo].[UserPost] WHERE IsApproved = 0";
                        query = @"SELECT [UserPostId]
                                  ,[PostUrl]
                                  ,[IsTag]
                                  ,[PostedOn]
                                  ,[IsPaid]
                                  ,up.[IsApproved]
                                  ,up.[ImageUrl]
                                  ,up.[CreatedDate]
                                  ,p.ProductName
                                  ,p.ProductType
                                  ,u.Name as BrandName
	                              ,u2.UserName
                              FROM [dbo].[UserPost] as up
                              Left Join dbo.Product as p On p.ProductId = up.ProductId
                              Left Join dbo.AspNetUsers as u On u.Id = up.BrandId
                              Left Join dbo.AspNetUsers as u2 On u2.Id = up.UserId";
                        result = (await dbConnection.QueryAsync<dynamic>(query)).ToList();
                    }

                    // Fetch the result
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



        public async Task<dynamic> GetProductImageByProductName(int ProductName)
        {
            try
            {

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = "";
                    var result = new List<dynamic>();
                        query = @"
                            SELECT *
                            FROM [dbo].[Product] Where ProductId = @ProductName";
                        result = (await dbConnection.QueryAsync<dynamic>(query, new { ProductName = ProductName })).ToList();

                    // Fetch the result
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

        #region Product CURD
        public async Task<string> CreateUpdateProduct(ProductDTO productDTO)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    if (productDTO.ProductId == null)
                    {
                        string query = @"
                        INSERT INTO [dbo].[Product] ([UserId], [ProductName], [ProductType], [CreatedDate], [IsApproved] , ImageUrl)
                        VALUES (@UserId,@ProductName,@ProductType, GETDATE(),0, @ImageUrl)";

                        var parameters = new
                        {
                            UserId = productDTO.UserId,
                            ProductName = productDTO.ProductName,
                            ProductType = productDTO.ProductType,
                            ImageUrl = productDTO.ImageUrl,

                        };

                        int rowsAffected = await dbConnection.ExecuteAsync(query, parameters);

                        if (rowsAffected > 0)
                        {
                            return "Insert successfully!";
                        }
                        return "Error Encountered";
                    }
                    else
                    {
                        string query = @"
                        UPDATE [dbo].[Product]
                        SET 
                            [ProductName] = @ProductName,
                            [ProductType] = @ProductType
                        WHERE [ProductId] = @ProductId";

                        var parameters = new
                        {
                            ProductName = productDTO.ProductName,
                            ProductType = productDTO.ProductType,
                            ProductId = productDTO.ProductId
                        };

                        int rowsAffected = await dbConnection.ExecuteAsync(query, parameters);

                        if (rowsAffected > 0)
                        {
                            return "Update successfully!";
                        }
                        return "Error Encountered";

                    }

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<dynamic> GetAllProductById(string? UserId)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query;
                    IEnumerable<dynamic> result;
                    if (UserId == "-1")
                    {
                        query = @"Select [ProductId]
                                  ,[UserId]
                                  ,[ProductName]
                                  ,[ProductType]
                                  ,[CreatedDate]
                                  ,[IsApproved] 
	                              ,u.Name From [dbo].[Product] as p
                            Left Join dbo.AspNetUsers as u On u.Id = p.UserId";
                        result = await dbConnection.QueryAsync<dynamic>(query);
                    }
                    else
                    {
                        query = @"Select * From [dbo].[Product] Where UserId = @UserId";
                        var parameters = new { UserId = UserId };
                        result = await dbConnection.QueryAsync<dynamic>(query, parameters);
                    }

                    if (result == null || !result.Any())
                    {
                        return new { data = new List<dynamic>() };
                    }

                    return new { data = result };
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }

        #endregion

        public async Task<dynamic> GetAllBrand()
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = @"SELECT 
                                      u.[Name] as UserName,
                                      u.Id,
	                                  r.Name,
                                      u.PhoneNumber,
                                      u.Email
                                  FROM [dbo].[AspNetUsers] as u
                                  Left Join [dbo].[AspNetUserRoles] as ar on ar.UserId = u.Id
                                  left join [dbo].[AspNetRoles] as r on ar.RoleId = r.Id
                                  Where r.Name = 'BRAND'";

                    var result = await dbConnection.QueryAsync<dynamic>(query);
                    if (result == null || !result.Any())
                    {
                        return new { data = new List<dynamic>() };
                    }

                    return new { data = result };
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }

        public async Task<dynamic> AprovedBrandProduct(int productId, int IsApproved)
        {
            try
            {

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = "";
                    if (IsApproved == 1)
                    {
                        query = @"UPDATE [dbo].[Product] SET IsApproved = 1 WHERE ProductId = @ProductId";

                    }
                    else
                    {
                        query = @"UPDATE [dbo].[Product] SET IsApproved = 0 WHERE ProductId = @ProductId";
                    }
                    var parameters = new
                    {
                        ProductId = productId
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
        public async Task<dynamic> DeleteProductById(int productId)
        {
            try
            {

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = @"Delete [dbo].[Product] WHERE ProductId = @ProductId";
 
                    var parameters = new
                    {
                        ProductId = productId
                    };

                    // Fetch the result
                    var result = await dbConnection.QueryAsync<dynamic>(query, parameters);

                    return "Delete Sucessfully";
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }
    }
}
