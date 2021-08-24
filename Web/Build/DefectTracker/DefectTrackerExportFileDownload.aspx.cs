using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerExportFileDownload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["VESSELID"] = (Request.QueryString["vesselid"] != null) ? (Request.QueryString["vesselid"].ToString()) : "0";
                ViewState["FILENO"] = (Request.QueryString["fileno"] != null) ? (Request.QueryString["fileno"].ToString()) : "";
                ViewState["SEAFARER"] = (Request.QueryString["seafarer"] != null) ? (Request.QueryString["seafarer"].ToString()) : "";
                ViewState["VESSELNAME"] = (Request.QueryString["vesselname"] != null) ? (Request.QueryString["VESSELNAME"].ToString()) : "";
                ucTitle.Text = ucTitle.Text + " [" + ViewState["SEAFARER"].ToString() + "]" + "[" + ViewState["VESSELNAME"].ToString() + "]";
                PhoenixToolbar toolbaredit = new PhoenixToolbar();
                toolbaredit.AddButton("Back", "BACK");
                MenuSubmit.AccessRights = this.ViewState;
                MenuSubmit.MenuList = toolbaredit.Show();
            }
            BindDataExport();
            BindAttachmentExport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataExport()
    {
        DataSet ds = new DataSet();

        ds = PhoenixDefectTracker.GetExportfileList(int.Parse(ViewState["VESSELID"].ToString()), 1);
        DataTable dt = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDataExport.DataSource = dt;
            gvDataExport.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvDataExport);
        }
    }

    private void BindAttachmentExport()
    {
        DataSet ds = new DataSet();

        ds = PhoenixDefectTracker.GetExportfileList(int.Parse(ViewState["VESSELID"].ToString()), 0);
        DataTable dt = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAttachmentExport.DataSource = dt;
            gvAttachmentExport.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvAttachmentExport);
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

    protected void gvDataExport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            Label httppath = (Label)e.Row.FindControl("lblhttppath");
            LinkButton lblFileName = (LinkButton)e.Row.FindControl("lblFileName");
            lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + httppath.Text + "\\" + lblFileName.Text;

            ImageButton anl = (ImageButton)e.Row.FindControl("cmdDownloadHistory");
            if (anl != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
                anl.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerExportDownloadHistory.aspx?exportdtkey=" + lblDTKey.Text + "'); return false;");
            }

        }
    }

    protected void gvDataExport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            string httppath = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblhttppath")).Text);
            string filename = (((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lblFileName")).Text);
            Guid exportdtkey = new Guid((((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text));

            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                PhoenixDefectTracker.ExportDownloadTrack(PhoenixSecurityContext.CurrentSecurityContext.UserCode, exportdtkey, ViewState["FILENO"].ToString(), ViewState["SEAFARER"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString());
                Response.Redirect(httppath + filename);
            }
            BindDataExport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void gvAttachmentExport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            Label httppath = (Label)e.Row.FindControl("lblhttppath");
            LinkButton lblFileName = (LinkButton)e.Row.FindControl("lblFileName");
            lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + httppath.Text + "\\" + lblFileName.Text;

            ImageButton anl = (ImageButton)e.Row.FindControl("cmdDownloadHistory");
            if (anl != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
                anl.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerExportDownloadHistory.aspx?exportdtkey=" + lblDTKey.Text + "'); return false;");
            }

        }
    }

    protected void gvAttachmentExport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            string httppath = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblhttppath")).Text);
            string filename = (((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lblFileName")).Text);
            Guid exportdtkey = new Guid((((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text));

            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                PhoenixDefectTracker.ExportDownloadTrack(PhoenixSecurityContext.CurrentSecurityContext.UserCode, exportdtkey, ViewState["FILENO"].ToString(), ViewState["SEAFARER"].ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString());
                Response.Redirect(httppath + filename);
            }
            BindAttachmentExport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
