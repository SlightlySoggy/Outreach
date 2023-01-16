﻿//using Outreach.Pages.Clients;
using System.Data;
using System.Data.SqlClient; 


namespace Outreach.Pages.Utilities
{
    public class ReportBug // Create class properties (This file can be referenced across the solution by typing "using Outreach.Pages.Utilities;")
    {
        public string Id; 
        public string Name;
        public string Email; 
        public string CreateDate;
        public string UserId;
        public string Subject;
        public string FileId;
        public string Message; 
        public string StatusId;
        public string Status;  

        public ReportBug()
        {
            Id = ""; 
            Name = "";
            Email = ""; 
            CreateDate = "";
            UserId = "";
            FileId = "";
            Subject = "";
            Message = ""; 
            StatusId = "1";//default status is new
            Status = ""; // the status name 

        }
        public ReportBug(string ReportBugId)
        { // retrive ReportBug data by ReportBug ID
            try
            {
                GeneralUtilities ut = new GeneralUtilities();
                var builder = WebApplication.CreateBuilder();
                var connectionString = builder.Configuration.GetConnectionString("MyAffDBConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "";
//                    if (ReportBugId.Trim() != "")
                    sql = "select t.Id,t.Name,t.Email,t.CreateDate,t.UserId,t.Subject,t.Message,FileId=u.Id,t.StatusId,Status=S.StatusName from ReportBug t with(nolock) " +
                        " left join StandardStatus2 S on S.Id=t.StatusId " +
                        " left join UploadFile U on U.GroupTypeId='5' and u.LinkedGroupId=t.Id " +
                        " where t.Id='" + ReportBugId + "'";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            { 
                                Id = reader.GetInt32(0).ToString();
                                if (reader["Id"].GetType() != typeof(DBNull))
                                {
                                    Id = reader["Id"].ToString();
                                }

                                if (reader["Name"].GetType() != typeof(DBNull))
                                {
                                    Name = reader["Name"].ToString();
                                }

                                if (reader["Email"].GetType() != typeof(DBNull))
                                {
                                    Email = reader["Email"].ToString();
                                }

                                if (reader["CreateDate"].GetType() != typeof(DBNull))
                                {
                                    CreateDate = reader["CreateDate"].ToString();
                                }

                                if (reader["UserId"].GetType() != typeof(DBNull))
                                {
                                    UserId = reader["UserId"].ToString();
                                }

                                if (reader["Subject"].GetType() != typeof(DBNull))
                                {
                                    Subject = ut.EmptyDateConvert(reader["Subject"].ToString());
                                }

                                if (reader["Message"].GetType() != typeof(DBNull))
                                {
                                    Message = ut.EmptyDateConvert(reader["Message"].ToString());
                                }


                                if (reader["StatusId"].GetType() != typeof(DBNull))
                                {
                                    StatusId = reader["StatusId"].ToString();
                                }

                                if (reader["FileId"].GetType() != typeof(DBNull))
                                {
                                    FileId = reader["FileId"].ToString();
                                }

                                if (reader["Status"].GetType() != typeof(DBNull))
                                {
                                    Status = reader["Status"].ToString();
                                } 


                                //ReportBugLinkage = new ReportBugLinkage(ReportBugId);  

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message.ToString());
            }

        }

        public string Save() // int Id, string Name, string Email, string EstimatedBudget, string ActualSpent, int OrganizationId, string CreateDate, int UserId, int StatusId, string Subject, string Message,CompletionDate, string Tags)
        {
            //save the new ReportBug into the database

            string result = "";
            int newReportBugID = 0;
            try
            {
                var builder = WebApplication.CreateBuilder();
                var connectionString = builder.Configuration.GetConnectionString("MyAffDBConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "";

                    if (this.Id == "" || this.Id == "0")
                    {
                        sql = "INSERT INTO ReportBug " +
                                      "(Name,Email,CreateDate,UserId,Subject,Message,StatusId) VALUES " +
                                      "(@Name,@Email,@CreateDate,@UserId,@Subject,@Message,@StatusId);" +
                                      "Select newID=MAX(id) FROM ReportBug"; 
                    }
                    else
                    {
                        sql = "Update ReportBug " +
                               "set Name = @Name," +
                                   "Email = @Email," + 
                                   "CreateDate = @CreateDate," +
                                   "UserId = @UserId," + 
                                   "Subject = @Subject," +
                                   "Message = @Message," +
                                   "StatusId = @StatusId " + 
                                " where id = '" + this.Id + "' ; Select newID=" + this.Id + "";
                    }

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    { 
                        cmd.Parameters.AddWithValue("@Name", this.Name);
                        cmd.Parameters.AddWithValue("@Email", this.Email); 
                        cmd.Parameters.AddWithValue("@CreateDate", this.CreateDate);
                        cmd.Parameters.AddWithValue("@UserId", this.UserId); 
                        cmd.Parameters.AddWithValue("@Subject", this.Subject);
                        cmd.Parameters.AddWithValue("@Message", this.Message); 
                        cmd.Parameters.AddWithValue("@StatusId", this.StatusId);
                        //cmd.ExecuteNonQuery();
                        newReportBugID = (Int32)cmd.ExecuteScalar();
                        result = newReportBugID.ToString();

                        //if (this.Id == "" && newReportBugID != 0)
                        //{
                        //    this.ReportBugLinkage.ReportBugId = newReportBugID.ToString();
                        //    this.ReportBugLinkage.Save();
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                result = "failed" + ex.Message;
            }
            return result;
        }

         

        public string Delete(string ReportBugId) // int Id, string Name, string Email, string EstimatedBudget, string ActualSpent, int OrganizationId, string CreateDate, int UserId, int StatusId, string Subject, string Message,CompletionDate, string Tags)
        {
            //save the new ReportBug into the database

            string result = "ok";

            try
            {
                var builder = WebApplication.CreateBuilder();
                var connectionString = builder.Configuration.GetConnectionString("MyAffDBConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "Update ReportBug Set StatusId=4 WHERE id=@id";  //StandardStatus2 table
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", ReportBugId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                result = "failed" + ex.Message;
            }
            return result;
        }
    }



}
