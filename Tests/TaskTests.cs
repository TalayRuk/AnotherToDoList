using System;
using Xunit;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ToDo;


namespace ToDo
{
  public class TaskTest : IDisposable
  {
    public TaskTest()
    {
      string dataSource = "Data Source=(localdb)\\mssqllocaldb"; // Data Source identifies the server.
      string dbName = "todo_test"; // Initial Catalog is the database name
      //Integrated Security sets the security of the database access to the Windows user that is currently logged in.
      DBConfiguration.ConnectionString = dataSource+";Initial Catalog="+dbName+";Integrated Security=SSPI;";
    }
    [Fact]
    public void Test1_AreTestWork_true()
    {
      //Arrange
      Task testTask = new Task("a");
      //Assert
      Assert.Equal("a", testTask.GetDescription());
    }
    [Fact]
    public void Test2_IsTheDatabaseEmpty()
    {
      //Act
     int AmountOfRows = Task.GetAll().Count;
     //Assert
     Assert.Equal(0, AmountOfRows);
    }
    [Fact]
    public void Test3_IsDataOverRideWorks()
    {
      //Arrange
      Task firstTask = new Task ("any");
      Task secondTask = new Task ("any");
      //Assert
      Assert.Equal (firstTask, secondTask);
    }
    [Fact]
    public void Test4_CanWeSave()
    {
      //Arrange
      Task testTask = new Task("Do laundry");
      //Act
      testTask.Save();
      //Assert
      List<Task> saved = Task.GetAll();
      List<Task> notsaved = new List<Task>{testTask};
      Assert.Equal(notsaved, saved);
    }
    [Fact]
    public void Test5_SaveOneItem()
    {
      //Arrange
      Task testTask = new Task("Update Computers");
      //Act
      testTask.Save();
      //Assert
      Task savedTask = Task.GetAll()[0];
      int result = savedTask.GetId();
      int testId = testTask.GetId();
      Assert.Equal(result, testId);
    }
    [Fact]
    public void Test6_Find_TaskInDatabase()
    {
      //Arrange
      Task testTask = new Task("start code");
      //Act
      testTask.Save();
      //Assert
      Task foundTask = Task.Find(testTask.GetId());
      Assert.Equal(foundTask, testTask);
    }

    public void Dispose()
    {
      Task.DeleteAll();
    }
  }
}
