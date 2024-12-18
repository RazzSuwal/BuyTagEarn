﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SMM.DataAccessLayer.Services.IServices;
using SMM.Models.DTO;
using System.Data;

namespace SMM.DataAccessLayer.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //bndrapqyhcfozlek
        public async Task<string> UserPost(PostRequestDTO postRequestDTO)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = @"
                        INSERT INTO [dbo].[UserPost] ([UserId], [ProductId], [PostUrl], [BrandId], [IsTag], [PostedOn], [IsPaid], [IsApproved], [ImageUrl], [CreatedDate])
                        VALUES (@UserId,@ProductName,@PostUrl,@BrandName,@IsTag,@PostedOn,@IsPaid,0,@ImageUrl,@CreatedDate)";

                    var parameters = new
                    {
                        ProductName = postRequestDTO.ProductName,
                        PostUrl = postRequestDTO.PostUrl,
                        ImageUrl = postRequestDTO.ImageUrl,
                        BrandName = postRequestDTO.BrandName,
                        IsTag = postRequestDTO.IsTag,
                        PostedOn = postRequestDTO.PostedOn,
                        IsPaid = postRequestDTO.IsPaid,
                        UserId = postRequestDTO.UserId,
                        CreatedDate = DateTime.Now

                    };

                    int rowsAffected = await dbConnection.ExecuteAsync(query, parameters);

                    if (rowsAffected > 0)
                    {
                        return "Add successfully created!";
                    }
                    return "Error Encountered";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<dynamic> GetUserPost(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return "UserId cannot be null or empty";
                }

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = @"
                SELECT * FROM [dbo].[UserPost] WHERE UserId = @UserId";

                    var parameters = new
                    {
                        UserId = userId
                    };

                    // Fetch the result
                    var result = await dbConnection.QueryAsync<dynamic>(query, parameters);
                    if (result == null || !result.Any())
                    {
                        return "No records found for the given UserId";
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }

        public async Task<dynamic> GetUserPostsDetails(int? userPostId)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = @"
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
                  FROM [dbo].[UserPost] as up
                  Left Join dbo.Product as p On p.ProductId = up.ProductId
                  Left Join dbo.AspNetUsers as u On u.Id = up.BrandId
                  Where up.UserPostId = @UserPostId";

                    var parameters = new { UserPostId = userPostId };

                    var result = await dbConnection.QueryAsync<dynamic>(query, parameters);
                    if (result == null || !result.Any())
                    {
                        return "No records found for the given UserId";
                    }

                    // Get the image path from the result
                    var postDetail = result.First();
                    if (postDetail.ImageUrl != null)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), postDetail.ImageUrl.TrimStart('/'));

                        if (File.Exists(filePath))
                        {
                            byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
                            postDetail.ImageBase64 = Convert.ToBase64String(imageBytes);
                        }
                    }

                    return postDetail;
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }

    }
}
