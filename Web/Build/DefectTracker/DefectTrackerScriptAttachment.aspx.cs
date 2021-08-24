using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerScriptAttachment : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvAttachment.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvAttachment.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageLink("javascript:Openpopup('MoreInfo','','DefectTrackerScriptAttachmentAdd.aspx')", "Add", "add.png", "ADD");
            toolbar.AddImageButton("../DefectTracker/DefectTrackerScriptAttachment.aspx", "Search", "search.png", "SEARCH");

            DefectTrackerScriptAdd.AccessRights = this.ViewState;
            DefectTrackerScriptAdd.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;                          
           
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = -1;
            BindData();
            SetPageNavigator();
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

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataTable dt = new DataTable();

        dt = PhoenixDefectTracker.ScriptAttachments(
                                        General.GetNullableInteger(ucModuleSearch.SelectedValue),
                                        txtSubjectSearch.Text,
                                        (int)ViewState["PAGENUMBER"],
                                        General.ShowRecords(null),
                                        ref iRowCount,
                                        ref iTotalPageCount
                                       );
        if (dt.Rows.Count > 0)
        {
            gvAttachment.DataSource = dt;
            gvAttachment.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvAttachment);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
        gvAttachment.EditIndex = -1;
        gvAttachment.SelectedIndex = -1;
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
        gvAttachment.SelectedIndex = -1;
        gvAttachment.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

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
    protected void DefectTrackerScriptSearch_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
    }
}
