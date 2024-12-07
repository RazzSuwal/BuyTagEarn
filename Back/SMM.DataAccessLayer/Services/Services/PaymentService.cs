using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SMM.DataAccessLayer.Services.IServices;
using SMM.Models.DTO;
using System.Data;

namespace SMM.DataAccessLayer.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> PaymentRequest(PaymentDTO paymentRequestDTO)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = @"
                        INSERT INTO [dbo].[PaymentRequest] ([UserId], [PaidByUserId], [UserPostId], [MobileNo], [IsPaid], [RequestedDate], [Amount])
                        VALUES (@UserId,@PaidByUserId,@UserPostId,@MobileNo,0,@RequestedDate, @Amount)";

                    var parameters = new
                    {
                        PaidByUserId = paymentRequestDTO.PaidByUserId,
                        UserPostId = paymentRequestDTO.UserPostId,
                        MobileNo = paymentRequestDTO.MobileNo,
                        UserId = paymentRequestDTO.UserId,
                        RequestedDate = DateTime.Now,
                        Amount = paymentRequestDTO.Amount

                    };

                    int rowsAffected = await dbConnection.ExecuteAsync(query, parameters);

                    if (rowsAffected > 0)
                    {
                        return "Payment requested successfully!";
                    }
                    return "Error Encountered";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<dynamic> GetAllPaymentRequest()
        {
            try
            {

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = @"SELECT 
                                    p.RequestId,
                                    p.UserId,
                                    p.UserPostId,
                                    p.MobileNo,
                                    p.IsPaid,
                                    p.RequestedDate,
	                                u.Name,
	                                up.ProductId,
									pr.ProductName,
									pr.ProductType,
	                                up.PostUrl
                                FROM [dbo].[PaymentRequest] AS p
                                LEFT JOIN dbo.AspNetUsers AS u ON u.Id = p.UserId
                                LEFT JOIN dbo.UserPost As up On up.UserPostId = p.UserPostId
								Left Join dbo.Product as pr On pr.ProductId = up.ProductId;";

                    // Fetch the result
                    var result = await dbConnection.QueryAsync<dynamic>(query);
                    if (result == null || !result.Any())
                    {
                        return "No records found!";
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }

        public async Task<dynamic> PaymentById(int requestId)
        {
            try
            {

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {


                    string query = @"  Update [dbo].[PaymentRequest] Set IsPaid = 1 Where RequestId = @RequestId";

                    var parameters = new
                    {
                        RequestId = requestId
                    };

                    // Fetch the result
                    var result = await dbConnection.QueryAsync<dynamic>(query, parameters);

                    return "Pay Sucessfully";
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }

        public async Task<dynamic> UpdatePaymentRequestImageUrl(int requestId, string imageUrl)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = @"UPDATE [dbo].[PaymentRequest] 
                                 SET VoucherImageUrl = @ImageUrl,
                                 IsPaid = 1
                                 WHERE RequestId = @RequestId";

                    var parameters = new
                    {
                        RequestId = requestId,
                        ImageUrl = imageUrl
                    };

                    var result = await dbConnection.ExecuteAsync(query, parameters);

                    if (result > 0)
                    {
                        return "Image updated successfully";
                    }
                    else
                    {
                        return "No record found with the specified RequestId";
                    }
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }

        public async Task<dynamic> GetAllPaidById(string? userId)
        {
            try
            {

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = @" Select pd.ProductName, pd.ProductType, u.UserName as BrandName , pr.Amount
                                      From [dbo].[PaymentRequest] as pr
                                      Left Join dbo.UserPost as p On p.UserPostId = pr.UserPostId
                                      Left Join dbo.Product as pd On pd.ProductId = p.ProductId
                                      Left Join dbo.AspNetUsers as u On u.Id = pr.PaidByUserId
                                     Where pr.IsPaid = 1 AND pr.UserId = @UserId";

                    var parameters = new
                    {
                        UserId = userId
                    };
                    var result = await dbConnection.QueryAsync<dynamic>(query, parameters);
                    if (result == null || !result.Any())
                    {
                        return "No records found!";
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, StackTrace = ex.StackTrace };
            }
        }
    }

}
