﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using WebApplication2.Models;
using System.Data.SqlClient;
using System.IO;

namespace WebApplication2.Models
{
    public class GestorLogin
    {
        //private string StrConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public bool ValidarLogin(LoginRequest loginRequest)
        {
            string strConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            bool result = false;
            //string nick = loginRequest.username;
            string pass = loginRequest.password;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                SqlCommand comm = new SqlCommand("generar_login", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("@nick", loginRequest.username));
                comm.Parameters.Add(new SqlParameter("@pass", pass));

                //(DEMO) CRUD CON ASP .NET MVC5, C# Y ANGULAR.JS - VIDEO 04

                SqlDataReader reader = comm.ExecuteReader();

                if (reader.HasRows)
                {
                    result = true;
                }

            }
            return result;

        }

    }
}