using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WMUtils
{
    public class Utils
    {

        public string ConvertProductDataTableToString(string stringConnection)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(stringConnection))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Product", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return serializer.Serialize(rows);
                }
            }
        }

    }


}