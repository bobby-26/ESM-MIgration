using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerArchievedEmail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuArchiveHistory.AccessRights = this.ViewState;

        if (!IsPostBack)
        {
            if (Request.QueryString["mailid"] != null)
                ViewState["MAILID"] = Request.QueryString["mailid"].ToString();

            if (Request.QueryString["savemode"] == null)
                ArchiveRemarks.Visible = false;
            else
                MenuArchiveHistory.MenuList = toolbar.Show();

            BindData();
        }
    }

   private void BindData()
    {

        DataSet ds = PhoenixDefectTracker.ArcheiveMailList(new Guid(ViewState["MAILID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvArcheiveMail.DataSource = ds;
            gvArcheiveMail.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvArcheiveMail);
        }
    }

    protected void MenuArchiveHistory_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (!CheckValues(txtArchiveBy.Text, txtRemarks.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixMailManager.MailDelete(new Guid(ViewState["MAILID"].ToString()));
            PhoenixDefectTracker.InsertArcheiveHistory(new Guid(ViewState["MAILID"].ToString()), Request.ServerVariables["REMOTE_ADDR"].ToString(), txtArchiveBy.Text, txtRemarks.Text);
            BindData();
           
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1');", true);
        }

    }

    private bool CheckValues(string archivedby, string remark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (archivedby == "")
            ucError.ErrorMessage = "Archived by is required";

        if (remark == "")
            ucError.ErrorMessage = "Remark is required";

        return (!ucError.IsError);
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

    protected void gvArcheiveMail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lnkSubject");

                if (lb != null)
                {
                    Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                    if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                        lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerMailRead.aspx?mailid=" + lbl.Text + "'); return false;");
                }
            }

        }
        catch (Exception er)
        {
            ucError.ErrorMessage = er.Message;
            ucError.Visible = true;
        }
    }
}
