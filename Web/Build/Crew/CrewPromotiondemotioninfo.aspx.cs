using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewPromotiondemotioninfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection nvc = HttpUtility.ParseQueryString(Request.QueryString.ToString().Replace("amp;", "").Replace("amp%3b", ""));
        
        ViewState["EMPLOYEEID"] = nvc["empid"];
        BindData();
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {

        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }
    public void BindData()
    {
        DataSet ds;
        ds = PhoenixCrewManagement.CrewPromotionDemotionList(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()));
        
        if (ds.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                HtmlTableRow row = new HtmlTableRow();

                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerText = dr["FLDRANKFROM"].ToString();
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = dr["FLDRANKTO"].ToString();
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = dr["FLDDATE"].ToString();
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = dr["FLDSTATUSDESC"].ToString();
                row.Cells.Add(cell);

                tableContent.Rows.Add(row);           
            }

        }









    }

}
