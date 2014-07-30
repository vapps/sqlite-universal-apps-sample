using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SQLTest
{
    class CreateDatabase
    {
        internal async static Task LoadDatabase(SQLitePCL.SQLiteConnection conn)
        {
            string sql = @"CREATE TABLE IF NOT EXISTS
                                  Customer (Id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                            Name    VARCHAR( 140 ) ,
                                            City    VARCHAR( 140 ) ,
                                            Contact VARCHAR( 140 )
                            );";
            using (var statement = conn.Prepare(sql))
            {
                await Task.Run(() =>
                {
                    statement.Step();
                });
            }
            //ResetDataAsync(conn);
        }
        public async static Task ResetDataAsync(SQLiteConnection db)
        {
            // Empty the Customer and Project tables 
            string sql = @"DELETE FROM Customer";
            using (var statement = db.Prepare(sql))
            {
                statement.Step();
            }

            List<Task> tasks = new List<Task>();

            // Add seed customers and projects
            await Storage.Insert("Adventure Works", "Bellevue", "Mu Han");
            await Storage.Insert("Contoso", "Seattle", "David Hamilton");
            await Storage.Insert("Fabrikam", "Redmond", "Guido Pica");
            await Storage.Insert("Tailspin Toys", "Kent", "Michelle Alexander");
        }
    }
}
