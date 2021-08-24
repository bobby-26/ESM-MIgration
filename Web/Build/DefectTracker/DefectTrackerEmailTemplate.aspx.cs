using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DefectTracker;
using SouthNests.Phoenix.Framework;


public partial class DefectTracker_DefectTrackerEmailTemplate : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvEmailTemplate.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvEmailTemplate.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            PhoenixToolbar toolbaraddtemplate = new PhoenixToolbar();
            toolbaraddtemplate.AddImageLink("javascript:parent.Openpopup('MoreInfo','','DefectTrackerEditEmailTemplate.aspx?'); return false;", "Add", "add.png", "ADD");
            toolbaraddtemplate.AddImageButton("../DefectTracker/DefectTrackerEmailTemplate.aspx", "Search", "search.png", "SEARCH");

            AddEmailTemplate.AccessRights = this.ViewState;
            AddEmailTemplate.MenuList = toolbaraddtemplate.Show();
        }
        BindData();
        SetPageNavigator();
    }
    protected void EmailTemplate_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
          && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
          && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvEmailTemplate, "Select$" + e.Row.RowIndex.ToString(), false);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvEmailTemplate, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuEmailTemplate_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
    }

    protected void EmailTemplate_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            string path = drv["FLDFILEPATH"].ToString();
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblIncidentId");
                eb.Attributes.Add("onclick", "Openpopup('codehelp1', '','DefectTrackerEditEmailTemplate.aspx?templateid=" + lbl.Text + "'); return false;");
            }

            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            Label lblAttachments = (Label)e.Row.FindControl("lblFileName");

            if ((drv["FLDFILENAMES"].ToString() == "") || (drv["FLDFILENAMES"].ToString() == null))
            {
                lnk.ImageUrl = Session["images"] + "/no-attachment.png";
            }
            else
            {
                lnk.ImageUrl = Session["images"] + "/attachment.png";
                lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" +path +"\\"+ lblAttachments.Text.ToString();
            }

            Label lbtn = (Label)e.Row.FindControl("lblBody");
            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("uclblBody");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
        }
    }
    protected void EmailTemplate_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
    }
    protected void EmailTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDefectTracker.DeleteTemplate(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblIncidentId")).Text));
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixDefectTracker.GetMailTemplateSearch(
                        General.GetNullableInteger(ucModuleSearch.SelectedValue)
                        ,txtSubjectSearch.Text
                        ,General.GetNullableGuid("")
                        ,(int)ViewState["PAGENUMBER"]
                        ,General.ShowRecords(null)
                        ,ref iRowCount
                        ,ref iTotalPageCount
                       );

        if (dt.Rows.Count > 0)
        {
            gvEmailTemplate.DataSource = dt;
            gvEmailTemplate.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvEmailTemplate);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
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
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvEmailTemplate.EditIndex = -1;
        gvEmailTemplate.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvEmailTemplate.SelectedIndex = -1;
        gvEmailTemplate.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
}
