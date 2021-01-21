using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public static class SqlFunctions
{
    public static void ExecuterRequete(string req, Action<SqlDataReader> action)
    {
        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionInfo"]))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(req, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
                action(reader);

            reader.Close();

            connection.Close();
        }
    }

    public static int ExecuterRequete(string req)
    {
        int nbChanged = -1;

        using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionInfo"]))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(req, connection);
            nbChanged = command.ExecuteNonQuery();

            connection.Close();
        }

        return nbChanged;
    }
}