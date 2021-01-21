using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

public partial class SqlEntity
{
    public void Init(SqlDataReader reader)
    {
        if (reader != null)
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
                prop.SmartDBCast(this, reader[prop.Name]);
        }
    }

    /*public SqlEntity(SqlDataReader reader)
    {
        foreach (PropertyInfo prop in GetType().GetProperties())
            prop.SmartDBCast(this, reader[prop.Name]);
    }*/

    public List<string> GetPropertiesName() => GetType().GetProperties().Select(prop => prop.Name).ToList();

    public List<string> GetSqlFormatedValues()
    {
        List<string> lst = new List<string>();

        foreach (PropertyInfo prop in GetType().GetProperties())
        {
            var val = prop.GetValue(this);
            if (val != null)
            {
                if (val.GetType() == typeof(string) || val.GetType() == typeof(DateTime))
                {
                    string s = val.ToString().Replace("'", "''");
                    lst.Add($"'{s}'");
                }
                else if (val.GetType() == typeof(bool))
                    lst.Add(((bool)val ? 1 : 0).ToString());
                else if (val.GetType() == typeof(decimal))
                {
                    string d = ((decimal)val).ToString();
                    lst.Add(d.Replace(',', '.'));
                }
                else
                    lst.Add(val.ToString());
            }
            else
            {
                lst.Add("null");
            }
        }

        return lst;
    }

    public string GetSqlFormatedAssignements(string[] exclude)
    {
        List<string> lstPropNames = GetPropertiesName();
        List<string> lstPropValues = GetSqlFormatedValues();

        string newRow = "";

        for (int i = 0; i < lstPropNames.Count; i++)
        {
            if (!exclude.Contains(lstPropNames[i]))
            {
                newRow += $"{lstPropNames[i]} = {lstPropValues[i]}";
                if (i != lstPropNames.Count - 1)
                    newRow += ", ";
            }
        }

        return newRow;
    }

    public string GetSqlFormatedAssignements(string exclude = "") => GetSqlFormatedAssignements(new string[] { exclude });
}