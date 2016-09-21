using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace ToDo
{
  public class Task
  {
    private int _id;
    private string _description;

    public Task(string Description, int Id = 0)
    {
      _id = Id;
      _description = Description;
    }

    public string GetDescription()
    {
      return _description;
    }
    public int GetId()
    {
      return _id;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    //Instance Methods

    //Static Methods
    //Get All Rows from
    public static List<Task> GetAll()
    {
      List<Task> allList = new List<Task> {};

      SqlConnection conn = DB.Connection();
      conn.Open();

      string statement = "SELECT * FROM tasks;";
      SqlCommand cmd = new SqlCommand (statement, conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read() )
      {
        int id = rdr.GetInt32(0);
        string description = rdr.GetString(1);
        Task tempTask = new Task (description, id);
        allList.Add(tempTask);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allList;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string statement = "INSERT INTO tasks (description) OUTPUT INSERTED.id VALUES (@Task);";
      SqlCommand cmd = new SqlCommand (statement, conn);
      SqlParameter dp = new SqlParameter();
      dp.ParameterName = "@Task";
      dp.Value = this.GetDescription();
      cmd.Parameters.Add(dp);
      SqlDataReader rdr = cmd.ExecuteReader();

      while ( rdr.Read() )
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string statement = "SELECT * FROM tasks WHERE id = @TaskId;";
      SqlCommand cmd = new SqlCommand(statement, conn);
      SqlParameter taskId = new SqlParameter();
      taskId.ParameterName = "@TaskId";
      taskId.Value = id.ToString();
      cmd.Parameters.Add(taskId);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;

      while (rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
      }
      Task foundTask = new Task(foundTaskDescription, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundTask;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string line = "DELETE FROM tasks;";
      SqlCommand cmd = new SqlCommand(line,conn);
      cmd.ExecuteNonQuery();

      conn.Close();
    }
    //Overrides
    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task)) {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask;
        bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
        bool idEquality = (this.GetId() == newTask.GetId());
        return (descriptionEquality && idEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetDescription().GetHashCode();
    }

  }
}
