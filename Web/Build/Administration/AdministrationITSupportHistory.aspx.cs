using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;


public partial class AdministrationITSupportHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind();

        PhoenixToolbar toolbar = new PhoenixToolbar();        
        toolbar.AddButton("Edit", "BUGEDIT");
        toolbar.AddButton("History", "HISTORY");
       
        MenuIssueTrack.AccessRights = this.ViewState;
        MenuIssueTrack.MenuList = toolbar.Show();
        MenuIssueTrack.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {
            if (Request.QueryString["dtkey"] != null)
            {
                ViewState["BUGDTKEY"] = Request.QueryString["dtkey"].ToString();
            }
            BindData();
        }
    }
    private void BindData()
    {

        DataTable dt = PhoenixAdministrationITSupport.EditITSupportHistory(new Guid(ViewState["BUGDTKEY"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ucTitle.Text = "Bug Track - [" + dt.Rows[0]["FLDBUGID"].ToString() + "] ";
            gvIssueTrack.DataSource = dt;
            gvIssueTrack.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvIssueTrack);
        }
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void MenuIssueTrack_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("AdministrationITSupportHistory.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
            }            
            if (dce.CommandName.ToUpper().Equals("BUGEDIT"))
            {
                Response.Redirect("AdministrationITSupportEdit.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvIssueTrack_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }

        }
        catch (Exception er)
        {
            ucError.ErrorMessage = er.Message;
            ucError.Visible = true;
        }
    }
}
