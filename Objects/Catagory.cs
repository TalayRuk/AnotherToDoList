using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ToDo
{
  public class Category
  {
    private int _id;
    private string _description;

    public Category (string des, int id = 0)
    {
      _id = id;
      _description = des;
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

    //overrides
    public override bool Equals(System.Object otherCategory)
    {
      if (!(otherCategory is Category)) {
        return false;
      }
      else
      {
        Category newCat = (Category) otherCategory;
        bool desEquality = (this.GetDescription() == newCat.GetDescription());
        bool idEquality = (this.GetId() == newCat.GetId());
        return (desEquality && idEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetDescription().GetHashCode();
    }

    //static List GetAll
    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category> {};

      SqlConnection conn = DB.Connection();
      conn.Open();

      string q = "SELECT * FROM categories;";
      SqlCommand cmd = new SqlCommand(q,conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while ( rdr.Read() )
      {
        int id = rdr.GetInt32(0);
        string description = rdr.GetString(1);
        Category tempCat = new Category(description, id);
        allCategories.Add(tempCat);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCategories;
    }

    //Save
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string q = "INSERT INTO categories (description) OUTPUT INSERTED.id VALUES(@categoryDescription);";
      SqlCommand cmd = new SqlCommand (q, conn);
      SqlParameter pams = new SqlParameter("@categoryDescription", this.GetDescription());
      cmd.Parameters.Add(pams);
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

    //Find
    public static Category Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string q = "SELECT * FROM categories WHERE id = @CatId;";
      SqlCommand cmd = new SqlCommand(q, conn);
      SqlParameter CatId = new SqlParameter("@CatId", id.ToString());
      cmd.Parameters.Add(CatId);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCatId = 0;
      string foundCatDescription = null;

      while ( rdr.Read() )
      {
        foundCatId = rdr.GetInt32(0);
        foundCatDescription = rdr.GetString(1);
      }
      Category foundCat = new Category (foundCatDescription, foundCatId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundCat;
    }


    //delete
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string q = "DELETE FROM categories;";
      SqlCommand cmd = new SqlCommand (q, conn);
      cmd.ExecuteNonQuery();

      conn.Close();
    }


  }
}
