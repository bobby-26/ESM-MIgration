using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTracker_DefectTrackerIssueScriptAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["bugid"] != null)
            {
                ViewState["BUGID"] = Request.QueryString["bugid"].ToString();
            }
            if (Request.QueryString["script"] != null)
            {
                ViewState["SCRIPT"] = Request.QueryString["script"].ToString();
            }
            BindData();
        }
    }

    private void BindData()
    {
        DataTable dt = PhoenixDefectTracker.BugScript(ViewState["BUGID"].ToString(), ViewState["SCRIPT"].ToString());
        if (dt.Rows.Count > 0)
        {
            ucTitle.Text = "Script Tracker - [" + dt.Rows[0]["FLDBUGID"].ToString() + "] ";
            ucTitle.Visible = true;
            gvAttachment.DataSource = dt;
            gvAttachment.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvAttachment);
        }
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string path = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");

            path = Session["sitepath"].ToString() + "/Attachments/Scripts/";

            lnk.NavigateUrl = path + lblFileName.Text;

            Label lbtn = (Label)e.Row.FindControl("lblSubject");
            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("uclblSubject");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            Label lblDeployedOnHeader = (Label)e.Row.FindControl("lblDeployedon");
            UserControlToolTip ucTotooltip = (UserControlToolTip)e.Row.FindControl("uclbldeployedon");
            lblDeployedOnHeader.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucTotooltip.ToolTip + "', 'visible');");
            lblDeployedOnHeader.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucTotooltip.ToolTip + "', 'hidden');");
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

    protected void gvAttachment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvAttachment, "Select$" + e.Row.RowIndex.ToString(), false);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvAttachment, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
