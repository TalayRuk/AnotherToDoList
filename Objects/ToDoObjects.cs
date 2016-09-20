using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ToDo;

namespace ToDo.Objects
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


    //Static Methods
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string line = "DELETE FROM tasks;";
      SqlCommand cmd = new SqlCommand(line,conn);
      cmd.ExecuteNonQuery();

      conn.Close();
    }


  }
}
