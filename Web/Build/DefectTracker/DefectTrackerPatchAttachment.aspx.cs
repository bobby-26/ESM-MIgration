using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerPatchAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            string js = "";
            if (Request.QueryString["projectdtkey"] != null)
                js = "javascript:Openpopup('codehelp1','','DefectTrackerPatchAddEdit.aspx?projectdtkey=" + Request.QueryString["projectdtkey"].ToString() + "')";
            toolbar.AddImageLink(js, "Add", "add.png", "ADDCOMPANY");
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
            ViewState["PAGENUMBER"] = 1;
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
        Guid? patchprojectdtkey = null;
        if (Request.QueryString["projectdtkey"] != null)
            patchprojectdtkey = General.GetNullableGuid(Request.QueryString["projectdtkey"].ToString());

        dt = PhoenixDefectTracker.PatchAttachments(
                                    patchprojectdtkey,
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
    protected void gvAttachment_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }

    protected void gvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            Label lbldtkey = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey");

            if (e.CommandName.ToUpper().Equals("SENDEMAIL"))
            {
                if (Request.QueryString["projectdtkey"] != null)
                {
                    Guid? patchprojectdtkey = null;
                    patchprojectdtkey = General.GetNullableGuid(Request.QueryString["projectdtkey"].ToString());

                    PhoenixDefectTracker.BulkPatchToContact(PhoenixSecurityContext.CurrentSecurityContext.UserCode, patchprojectdtkey, new Guid(lbldtkey.Text), "SEP");
                }

                _gridView.EditIndex = -1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton anl = (ImageButton)e.Row.FindControl("cmdAcknowledgement");
            Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
            if (anl != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                anl.Attributes.Add("onclick", "javascript:parent.Openpopup('MoreInfo', '', 'DefectTrackerPatchVesselList.aspx?dtkey=" + lblDTKey.Text + "&projectdtkey=" + Request.QueryString["projectdtkey"].ToString() + "'); return false;");
            }

            LinkButton lbtn = (LinkButton)e.Row.FindControl("lblSubject");
            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("uclblSubject");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                eb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerPatchAddEdit.aspx?dtkey=" + lblDTKey.Text + "&projectdtkey=" + Request.QueryString["projectdtkey"].ToString() + "'); return false;");
            }

            LinkButton lb = (LinkButton)e.Row.FindControl("lblSubject");
            if (lbtn != null)
            {
                if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerPatchAddEdit.aspx?dtkey=" + lblDTKey.Text + "&projectdtkey=" + Request.QueryString["projectdtkey"].ToString() + "'); return false;");
            }


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

}
