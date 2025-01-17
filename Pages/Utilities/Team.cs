﻿//using Outreach.Pages.Clients;
using Microsoft.CodeAnalysis;
using System.Data;
using System.Data.SqlClient;


namespace Outreach.Pages.Utilities
{
    public class Team
    {
        public string Id;
        public string Name;
        public string Description;
        public string OrganizationId; 
        public string CreatedDate;
        public string CreatedUserId;
        public string StatusId;
        public string Status;
        public string EstimatedBudget;
        public string ActualSpent;
        public List<TeamUser> TeamManagerUserIds;
        public List<TeamUser> TeamMemberUserIds;
        public Team()
        {
            Id = "";
            Name = "";
            Description = "";
            OrganizationId = "";
            CreatedDate = "";
            CreatedUserId = "";
            StatusId = "";
            Status = "";
            EstimatedBudget = "";
            ActualSpent = "";
            TeamManagerUserIds = new List<TeamUser>();
            TeamMemberUserIds = new List<TeamUser>();

        }
        public Team(string teamId)
        { // retrive Organization data by Organization ID
            try
            {
                GeneralUtilities ut = new GeneralUtilities();
                var builder = WebApplication.CreateBuilder();
                var connectionString = builder.Configuration.GetConnectionString("MyAffDBConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Select t.Id,Name,Description,OrganizationId,CreatedDate,CreatedUserId,StatusId ,Status=st.StatusName from Team t with(nolock)  left join StandardStatus st on st.Id = t.StatusId  where t.Id='" + teamId + "' ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //ClientInfo clientInfo = new ClientInfo();
                                Id = reader.GetInt32(0).ToString();
                                if (reader["Name"].GetType() != typeof(DBNull))
                                {
                                    Name = reader["Name"].ToString();
                                }

                                if (reader["Description"].GetType() != typeof(DBNull))
                                {
                                    Description = reader["Description"].ToString();
                                }

                                if (reader["OrganizationId"].GetType() != typeof(DBNull))
                                {
                                    OrganizationId = reader["OrganizationId"].ToString();
                                }

                                if (reader["CreatedDate"].GetType() != typeof(DBNull))
                                {
                                    CreatedDate = reader["CreatedDate"].ToString();
                                }

                                if (reader["CreatedUserId"].GetType() != typeof(DBNull))
                                {
                                    CreatedUserId = reader["CreatedUserId"].ToString();
                                }

                                if (reader["StatusId"].GetType() != typeof(DBNull))
                                {
                                    StatusId = reader["StatusId"].ToString();
                                }

                                if (reader["Status"].GetType() != typeof(DBNull))
                                {
                                    Status = reader["Status"].ToString();
                                }

                                TeamManagerUserIds = ut.GetTeamUserList(teamId,  "true");
                                TeamMemberUserIds = ut.GetTeamUserList(teamId, "false");

                                // listOrgs.Add(Org);
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


        public string Save() // int Id, string Name, string Description, string OrganizationId, string ActualSpent, int CreatedOrgId, string CreatedDate, int CreatedUserId, int StatusId, string StartDate, string DueDate,CompletionDate, string Tags)
        {
            //save the new Project into the database

            string result = "ok";
            int newProdID = 0;
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
                        sql = "INSERT INTO Team " +
                                      "(Name,Description,OrganizationId,CreatedDate,CreatedUserId,StatusId) VALUES " +
                                      "(@Name,@Description,@OrganizationId,@CreatedDate,@CreatedUserId,@StatusId);" +
                                      "Select newID=MAX(id) FROM Project";
                    }
                    else
                    {
                        sql = "Update Team " +
                               "set Name = @Name," +
                                   "Description = @Description," +
                                   "OrganizationId = @OrganizationId," + 
                                   "CreatedDate = @CreatedDate," +
                                   "CreatedUserId = @CreatedUserId," + 
                                   "StatusId = @StatusId " +
                                " where id = '" + this.Id + "' ; Select newID=" + this.Id + "";
                    }

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", this.Name);
                        cmd.Parameters.AddWithValue("@Description", this.Description);
                        cmd.Parameters.AddWithValue("@OrganizationId", this.OrganizationId); 
                        cmd.Parameters.AddWithValue("@CreatedDate", this.CreatedDate);
                        cmd.Parameters.AddWithValue("@CreatedUserId", this.CreatedUserId); 
                        cmd.Parameters.AddWithValue("@StatusId", this.StatusId);
                        //cmd.ExecuteNonQuery();
                        newProdID = (Int32)cmd.ExecuteScalar();

                    }
                }
            }
            catch (Exception ex)
            {
                result = "failed" + ex.Message;
            }
            return result;
        }


        public string Delete(string DeptId) // int Id, string ProjectName, string Description, string EstimatedBudget, string ActualSpent, int CreatedOrgId, string CreatedDate, int CreatedUserId, int ProjectTaskStatusId, string StartDate, string DueDate,CompletionDate, string Tags)
        {
            //save the new Project into the database

            string result = "ok";

            try
            {
                var builder = WebApplication.CreateBuilder();
                var connectionString = builder.Configuration.GetConnectionString("MyAffDBConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "Update Team Set atusId=3 WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", DeptId);

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
