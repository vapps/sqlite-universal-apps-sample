using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLTest
{
    class SQLiteStorage<TItem> : IStorage<TItem>
    {
        private SQLiteConnection conn;
        public SQLiteStorage()
        {
            conn = new SQLiteConnection("sqlitepcldemo.db");
            CreateDatabase.LoadDatabase(conn);
        }

        public async Task<int> Insert(TItem item)
        {
            var db = App.conn;
            try
            {
                await Task.Run(() =>
                {
                    StringBuilder propsStr = new StringBuilder();
                    StringBuilder valuesStr = new StringBuilder();
                    Type t = item.GetType();
                    IEnumerator<PropertyInfo> e = t.GetRuntimeProperties().GetEnumerator();

                    while (e.MoveNext())
                    {
                        PropertyInfo i = e.Current as PropertyInfo;
                        propsStr.Append(i.Name);
                        propsStr.Append(",");

                        valuesStr.Append(i.GetValue(item));
                        valuesStr.Append(",");
                    }
                    propsStr.Remove(propsStr.Length - 1, 1);
                    valuesStr.Remove(propsStr.Length - 1, 1);

                    using (var custstmt = db.Prepare("INSERT INTO Customer ("+propsStr+") VALUES ("+valuesStr+")"))
                    {
                        custstmt.Step();
                    }
                });
            }
            catch (Exception ex)
            {
                return 0;
            }
            using (var idstmt = db.Prepare("SELECT last_insert_rowid()"))
            {
                idstmt.Step();
                return (int)idstmt[0];
            }
        }

        public async Task<Customer> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(TItem item)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ObservableCollection<TItem>> GetAll()
        {

            var items = new ObservableCollection<TItem>();
            Customer customer = null;
            using (var statement = App.conn.Prepare("SELECT Id, Name, City, Contact FROM Customer"))
            {
                //while (statement.Step() == SQLiteResult.ROW)
                //{
                //    Customer cs = CreateItem(statement);
                //    items.Add(cs);
                //}
                return items;
            }
        }
    }
}
