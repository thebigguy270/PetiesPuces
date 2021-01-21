using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

public abstract class SqlDAOAttributes<T>
    where T : SqlEntity
{
    private readonly string TableName;
    private List<string> IdColumnNames = new List<string>();

    public List<T> Values = new List<T>();
    protected List<List<object>> IdsChanged = new List<List<object>>();

    private List<object> GetIdsIn(T value)
    {
        List<object> lstIds = new List<object>();

        foreach (string colId in IdColumnNames)
            lstIds.Add(value.GetType().GetProperty(colId).GetValue(value));

        return lstIds;
    }

    private string GetSqlIdsAnd(T value)
    {
        string ret = "";

        for (int i = 0; i < IdColumnNames.Count; i++)
        {
            string colId = IdColumnNames[i];
            ret += $"{colId} = {value.GetType().GetProperty(colId).GetValue(value)}";

            if (i != IdColumnNames.Count - 1)
                ret += " AND ";
        }

        return ret;
    }

    private string GetSqlWhere(T value) => $"WHERE {GetSqlIdsAnd(value)}";

    private string GetSqlWhere(List<T> values)
    {
        string ret = "WHERE ";

        for (int i = 0; i < values.Count; i++)
        {
            ret += GetSqlIdsAnd(values[i]);

            if (i != values.Count - 1)
                ret += " OR ";
        }

        return ret;
    }

    /// <summary>
    /// Creates a DAO that allows you to access, modify and delete values in a table
    /// </summary>
    public SqlDAOAttributes()
    {
        TableName = GetType().Name;

        SqlFunctions.ExecuterRequete($"SELECT * FROM {TableName}", reader =>
        {
            T value = (T)Activator.CreateInstance(
                typeof(T),
                new object[] { reader });

            Values.Add(value);
        });

        // Find all IDs attributes
        T defaultedValue = (T)Activator.CreateInstance(
            typeof(T),
            new object[] { null });
        foreach (PropertyInfo info in defaultedValue.GetType().GetProperties())
        {
            var attr = info.CustomAttributes;
            if (attr.Any(x => x.AttributeType.Name == "SqlID"))
                IdColumnNames.Add(info.Name);
        }
    }

    /// <summary>
    /// Deletes the given value from the table
    /// </summary>
    /// <param name="value"></param>
    public bool Remove(T value)
    {
        int rows = SqlFunctions.ExecuterRequete($"DELETE FROM {TableName} {GetSqlWhere(value)}");
        bool changed = rows > 0;

        if (changed)
            Values.Remove(value);

        return changed;
    }

    public bool Remove(List<T> values)
    {
        int rows = SqlFunctions.ExecuterRequete($"DELETE FROM {TableName} {GetSqlWhere(values)}");
        bool changed = rows > 0;

        if (changed)
            foreach (T val in values)
                Values.Remove(val);

        return changed;
    }

    /// <summary>
    /// Deletes everythin in the table
    /// </summary>
    public void RemoveAll()
    {
        Values.Clear();
        SqlFunctions.ExecuterRequete($"DELETE FROM {TableName}");
    }

    /// <summary>
    /// Adds the value to the table
    /// </summary>
    /// <param name="value">value to add</param>
    public void Add(T value)
    {
        List<string> lstPropNames = value.GetPropertiesName();
        List<string> lstPropValues = value.GetSqlFormatedValues();

        SqlFunctions.ExecuterRequete($"INSERT INTO {TableName} ({string.Join(", ", lstPropNames)})\n" +
            $"VALUES ({string.Join(", ", lstPropValues)})");

        Values.Add(value);
    }

    /// <summary>
    /// Adds the values to the table
    /// </summary>
    /// <param name="values">values to add</param>
    public void AddAll(T[] values)
    {
        List<string> lstPropNames = values[0].GetPropertiesName();

        string req = $"INSERT INTO {TableName} ({string.Join(", ", lstPropNames)})\nVALUES ";

        for (int i = 0; i < values.Length; i++)
        {
            List<string> lstPropValues = values[i].GetSqlFormatedValues();

            req += $"({string.Join(", ", lstPropValues)})";

            if (i != values.Length - 1)
                req += ",";
        }

        SqlFunctions.ExecuterRequete(req);
    }

    /// <summary>
    /// Adds the values to the table
    /// </summary>
    /// <param name="values">values to add</param>
    public void AddAll(List<T> values) => AddAll(values.ToArray());

    /// <summary>
    /// Notifies the DAO that the value has been updated
    /// </summary>
    /// <param name="value">The value that you updated</param>
    public void NotifyUpdated(T newValue) => IdsChanged.Add(GetIdsIn(newValue));

    /// <summary>
    /// Notifies the DAO that the value has been updated outside of itself
    /// </summary>
    /// <param name="value">The value that you updated</param>
    public void NotifyUpdatedOutside(T newValue)
    {
        NotifyUpdated(newValue);

        int id = Values.FindIndex(x => GetIdsIn(x).All(GetIdsIn(newValue).Contains));
        Values[id] = newValue;
    }

    /// <summary>
    /// Sends the updates to the sql server
    /// </summary>
    public void Update()
    {
        foreach(List<object> ids in IdsChanged)
        {
            T value = Values.Find(x => GetIdsIn(x).All(ids.Contains));

            string newRow = value.GetSqlFormatedAssignements(IdColumnNames.ToArray());

            SqlFunctions.ExecuterRequete($"UPDATE {TableName} SET {newRow} {GetSqlWhere(value)}");
        }

        IdsChanged.Clear();
    }
}
