using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using SouthNests.Phoenix.Framework;

public partial class Options_OptionsQueryWindow : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public DataSet ExecSPReturnDataSet(string ProcedureName, List<SqlParameter> Parameters)
    {
        DataSet ds = new DataSet();

        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;

        try
        {
            using (SqlConnection conn = new SqlConnection(DataAccess.GetConnectionString()))
            {
                conn.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                {

                    txtResult.Text += "\n" + e.Message;

                };
                conn.Open();
                cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                DataAccess.CollectParameters(cmd, Parameters);
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataAccess.CollectOutputParameters(cmd, Parameters);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd = null;
        }
        return ds;
    }


    protected void cmdExecute_Click(object sender, EventArgs e)
    {
        try
        {
            txtResult.Text = "";
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@SQL", SqlDbType.NVarChar, -1, ParameterDirection.Input, txtQuery.Text));

            ds = ExecSPReturnDataSet("PREXECUTESQL", ParameterList);

            if (ds.Tables.Count > 1)
            {
                gvResults.DataSource = ds.Tables[0];
                gvResults.DataBind();
            }
            else
            {
                foreach (DataTable dt in ds.Tables)
                {
                    txtResult.Text = txtResult.Text + "\n";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        txtResult.Text = txtResult.Text + String.Format("{0}\t", dc.ColumnName.ToString());
                    }
                    txtResult.Text = txtResult.Text + String.Format("{0}", "\n");

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            txtResult.Text = txtResult.Text + String.Format("{0}\t", dr[dc].ToString());
                        }
                        txtResult.Text = txtResult.Text + String.Format("{0}", "\n");
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            txtResult.Text = ex.Message;
        }
    }
}
