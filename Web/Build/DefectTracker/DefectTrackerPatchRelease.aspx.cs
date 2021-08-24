using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerPatchRelease : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["VESSELID"] = (Request.QueryString["vesselid"] != null) ? (Request.QueryString["vesselid"].ToString()) : "0";
                PhoenixToolbar toolbaredit = new PhoenixToolbar();
                toolbaredit.AddButton("Back", "BACK");
                MenuSubmit.AccessRights = this.ViewState;
                MenuSubmit.MenuList = toolbaredit.Show();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixDefectTracker.GetPatchList(int.Parse(ViewState["VESSELID"].ToString()), "");
        DataTable dt = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAttachment.DataSource = dt;
            gvAttachment.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvAttachment);
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
    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            Label lblFilePath = (Label)e.Row.FindControl("lblFilePath");
            lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + lblFilePath.Text;
        }
    }
    protected void MenuPatchRelease_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("~/DefectTracker/DefectTrackerAuthendicateSeafarer.aspx");
        }
    }
}
