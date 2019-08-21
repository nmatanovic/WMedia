using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.IO;

namespace WMDAL
{
    public interface IWMReprository<T>
    {
        IEnumerable<T> GetAll();
        void Save(T entity);
        T FindById(int Id);
        void Update(T entity);
        void Delete(int Id);
    }

    //
    //
    //
    public class WMProductReprositry : IWMReprository<Product>
    {
        string m_ConnectionString = @"Data source = (local)\SQLEXPRESS; Initial catalog = WMDB; Integrated Security=True;";

        public WMProductReprositry()
        {
        }

        public WMProductReprositry(string connectionString)
        {
            m_ConnectionString = connectionString;
        }

        public IEnumerable<Product> GetAll()
        {
            DataTable dtblProduct = new DataTable();

            using (var sqlCon = new SqlConnection(m_ConnectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Products", sqlCon);

                sqlDa.Fill(dtblProduct);
            }


            var result = dtblProduct.AsEnumerable().Select(row => new Product()
            {
                ID = Convert.ToInt32(row["ID"]),
                Name = Convert.ToString(row["Name"]),
                Price = Convert.ToDecimal(row["Price"]),
            });


            return result;

        }

        public void Save(Product p)
        {

            using (var sqlCon = new SqlConnection(m_ConnectionString))
            {
                sqlCon.Open();
                string query = @"INSERT INTO PRODUCTS (Name,Price) VALUES (@Name, @Price)";

                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Name", p.Name);
                sqlCmd.Parameters.AddWithValue("@Price", p.Price);

                sqlCmd.ExecuteNonQuery();

            }
        }

        //
        // returns null if not found
        //
        public Product FindById(int Id)
        {
            DataTable dtblProduct = new DataTable();

            using (var sqlCon = new SqlConnection(m_ConnectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Products WHERE Id = @ProductID", sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductID", Id);
                sqlDa.Fill(dtblProduct);
            }

            if (dtblProduct.Rows.Count == 1)
            {
                Product result = new Product();

                result.ID = Convert.ToInt32(dtblProduct.Rows[0][0]);
                result.Name = Convert.ToString(dtblProduct.Rows[0][1]);
                result.Price = Convert.ToDecimal(dtblProduct.Rows[0][2]);

                return result; // Found;
            }


            return null; // Not Found
        }

        // product is new info
        public void Update(Product product)
        {
            using (var sqlCon = new SqlConnection(m_ConnectionString))
            {
                sqlCon.Open();
                string query = @"UPDATE Products SET Name = @ProductName, Price = @Price WHERE ID = @ProductID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

                sqlCmd.Parameters.AddWithValue("@ProductID", product.ID);
                sqlCmd.Parameters.AddWithValue("@ProductName", product.Name);
                sqlCmd.Parameters.AddWithValue("@Price", product.Price);

                sqlCmd.ExecuteNonQuery();
            }
        }

        public void Delete(int Id)
        {
            using (var sqlCon = new SqlConnection(m_ConnectionString))
            {
                sqlCon.Open();
                string query = @"DELETE FROM Products WHERE ID = @ProductID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

                sqlCmd.Parameters.AddWithValue("@ProductID", Id);
                sqlCmd.ExecuteNonQuery();
            }

        }
    }

    //
    //
    //

    public class WMJSONProductReprository : IWMReprository<Product>
    {

        private static List<Product> m_Products = new List<Product>();
        DataTable m_ProductDataTable = new DataTable();
        string path = @"e:\PROBAJSON.txt";

        // Last inserted, anyway Flat JSON should be used....
        private int GetLargestId()
        {
            int maxID = 0;

            try
            { 
                maxID = m_Products.Max(s => s.ID);
            }
            catch(Exception)
            {
            }

            return maxID;
        }

        public WMJSONProductReprository()
        {
        }

        public WMJSONProductReprository(string connectionString)
        {
            string contents = File.ReadAllText(path);
            m_Products = JsonConvert.DeserializeObject<List<Product>>(contents);
        }

        public IEnumerable<Product> GetAll()
        {
            return m_Products;
        }

        public void Save(Product p)
        {

            p.ID = GetLargestId();

            m_Products.Add(p);
        }

        //
        // returns null if not found
        //
        public Product FindById(int Id)
        {
            Product res = null;
            try
            {
                res = m_Products.First(i => i.ID == Id);
            }
            catch(Exception)
            {
            }

            return res;
        }

        // product is new info
        public void Update(Product product)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Id)
        {
            Product p = FindById(Id);

            m_Products.Remove(p);
        }
    }



 


    public static class Factory
    {
        public static IWMReprository<Product> Get(string dbType)
        {
            if (dbType == "DB")
            {
                return new WMProductReprositry();
            }
            else if(dbType == "JSON")
            {
                return new WMJSONProductReprository();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static WMJSONProductReprository GetReprository(string sw)
        {
            return new WMJSONProductReprository();
        }

        private static WMProductReprositry GetReprositoryDB()
        {
            return new WMProductReprositry();
        }

    }
}
