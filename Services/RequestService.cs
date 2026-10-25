using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MajesticDesktop.Services
{
    public class RequestService
    {
        public void SaveRequest(Request request)
        {
            if (request == null || request.Details.Count == 0)
                throw new ArgumentException("الطلب فارغ، يجب إضافة منتجات للطلب أولاً.");

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string insertRequestQuery = @"
                            INSERT INTO Requests (LocationId, RequestDate, Status) 
                            VALUES (@LocationId, @RequestDate, 'Pending');
                            SELECT SCOPE_IDENTITY();";

                        int requestId;
                        using (SqlCommand cmdRequest = new SqlCommand(insertRequestQuery, connection, transaction))
                        {
                            cmdRequest.Parameters.Add("@LocationId", SqlDbType.Int).Value = request.LocationId;
                            cmdRequest.Parameters.Add("@RequestDate", SqlDbType.DateTime).Value = request.RequestDate;
                            requestId = Convert.ToInt32(cmdRequest.ExecuteScalar());
                        }

                        const string insertDetailQuery = @"
                            INSERT INTO RequestDetails (RequestId, ProductId, RequestedQuantity, ApprovedQuantity) 
                            VALUES (@RequestId, @ProductId, @RequestedQuantity, 0);";

                        foreach (RequestDetail detail in request.Details)
                        {
                            using (SqlCommand cmdDetail = new SqlCommand(insertDetailQuery, connection, transaction))
                            {
                                cmdDetail.Parameters.Add("@RequestId", SqlDbType.Int).Value = requestId;
                                cmdDetail.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdDetail.Parameters.Add("@RequestedQuantity", SqlDbType.Int).Value = detail.RequestedQuantity;
                                cmdDetail.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // تم تحسين الأداء (Big-O = O(1) DB Calls, O(N) Memory Mapping)
        // تفادينا مشكلة N+1 بتنفيذ استعلام واحد بيربط الطلبات بالتفاصيل
        public List<Request> GetAllPendingRequests()
        {
            Dictionary<int, Request> requestDictionary = new Dictionary<int, Request>();

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT r.Id, r.LocationId, r.RequestDate, r.Status, r.Notes,
                           d.ProductId, d.RequestedQuantity, d.ApprovedQuantity
                    FROM Requests r
                    LEFT JOIN RequestDetails d ON r.Id = d.RequestId
                    WHERE r.Status IN ('Pending', N'قيد الانتظار')
                    ORDER BY r.RequestDate ASC, r.Id ASC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int requestId = Convert.ToInt32(reader["Id"]);

                        if (!requestDictionary.TryGetValue(requestId, out Request req))
                        {
                            req = new Request
                            {
                                Id = requestId,
                                LocationId = Convert.ToInt32(reader["LocationId"]),
                                RequestDate = Convert.ToDateTime(reader["RequestDate"]),
                                Status = reader["Status"].ToString(),
                                Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString(),
                                Details = new List<RequestDetail>()
                            };
                            requestDictionary.Add(requestId, req);
                        }

                        if (reader["ProductId"] != DBNull.Value)
                        {
                            req.Details.Add(new RequestDetail
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                RequestedQuantity = Convert.ToInt32(reader["RequestedQuantity"]),
                                ApprovedQuantity = Convert.ToInt32(reader["ApprovedQuantity"])
                            });
                        }
                    }
                }
            }
            return new List<Request>(requestDictionary.Values);
        }

        public void ProcessRequestDecision(int requestId, string status, string managerNotes, List<RequestDetail> processedDetails)
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string updateReqQuery = @"
                            UPDATE Requests 
                            SET Status = @Status, Notes = @Notes 
                            WHERE Id = @Id";

                        using (SqlCommand cmd = new SqlCommand(updateReqQuery, connection, transaction))
                        {
                            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = status;
                            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(managerNotes) ? (object)DBNull.Value : managerNotes;
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = requestId;
                            cmd.ExecuteNonQuery();
                        }

                        const string updateDetQuery = @"
                            UPDATE RequestDetails 
                            SET ApprovedQuantity = @ApprovedQuantity 
                            WHERE RequestId = @RequestId AND ProductId = @ProductId";

                        foreach (var detail in processedDetails)
                        {
                            using (SqlCommand cmd = new SqlCommand(updateDetQuery, connection, transaction))
                            {
                                cmd.Parameters.Add("@ApprovedQuantity", SqlDbType.Int).Value = detail.ApprovedQuantity;
                                cmd.Parameters.Add("@RequestId", SqlDbType.Int).Value = requestId;
                                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // تم تحسين الأداء (Big-O = O(1) DB Calls)
        // استخدام Common Table Expression (CTE) للحصول على أحدث طلب ومحتوياته في ضربة واحدة
        public Request GetApprovedRequestByLocation(int locationId)
        {
            Request approvedRequest = null;

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                const string query = @"
                    WITH TopRequest AS (
                        SELECT TOP 1 Id, LocationId, RequestDate, Status, Notes 
                        FROM Requests 
                        WHERE LocationId = @LocationId AND Status IN ('Approved', 'Partially Approved', N'مقبول كلياً', N'مقبول جزئياً')
                        ORDER BY Id ASC
                    )
                    SELECT r.Id, r.LocationId, r.RequestDate, r.Status, r.Notes,
                           d.ProductId, d.RequestedQuantity, d.ApprovedQuantity
                    FROM TopRequest r
                    LEFT JOIN RequestDetails d ON r.Id = d.RequestId AND d.ApprovedQuantity > 0;";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (approvedRequest == null)
                            {
                                approvedRequest = new Request
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    LocationId = Convert.ToInt32(reader["LocationId"]),
                                    RequestDate = Convert.ToDateTime(reader["RequestDate"]),
                                    Status = reader["Status"].ToString(),
                                    Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString(),
                                    Details = new List<RequestDetail>()
                                };
                            }

                            if (reader["ProductId"] != DBNull.Value)
                            {
                                approvedRequest.Details.Add(new RequestDetail
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    RequestedQuantity = Convert.ToInt32(reader["RequestedQuantity"]),
                                    ApprovedQuantity = Convert.ToInt32(reader["ApprovedQuantity"])
                                });
                            }
                        }
                    }
                }
            }
            return approvedRequest;
        }

        public void UpdateRequestStatus(int requestId, string newStatus)
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                const string updateQuery = "UPDATE Requests SET Status = @Status WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = newStatus;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = requestId;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}