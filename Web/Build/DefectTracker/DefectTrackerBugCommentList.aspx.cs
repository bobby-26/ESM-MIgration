using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using System.Data;
using System.Text;

public partial class DefectTracker_DefectTrackerBugCommentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            
            if (!IsPostBack)
            {
                BindProjectList();    
                BindDefaultVessel();
                ddlProject.SelectedValue = "24";
            }
            ViewState["PROJECTNAME"] = ddlProject.SelectedValue;
            lblsep.Text = "PROJECT - " + ddlProject.SelectedItem.Text.ToUpper();
            PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
            toolbarbuglist.AddImageLink("javascript:Openpopup('Filter','','DefectTrackerBugAdd.aspx?projectname=" + ViewState["PROJECTNAME"].ToString() + "')", "Add", "add.png", "ADDDEFECT");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerBugCommentList.aspx", "Search", "search.png", "SEARCH");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerBugCommentList.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuDefectTracker.AccessRights = this.ViewState;
            MenuDefectTracker.MenuList = toolbarbuglist.Show();

            BindData();
            SetPageNavigator();

            SEPStatus.ParentCode = "23";
            ucSEPType.ParentCode = "9";
            SEPSeverity.ParentCode = "19";
            SEPPriority.ParentCode = "15";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindProjectList()
    {
        DataTable dt = PhoenixDefectTracker.ProjectList(null);
        if (dt.Rows.Count > 0)
        {
            ddlProject.DataSource = dt;
            ddlProject.DataValueField = "FLDPROJECTID";
            ddlProject.DataTextField = "FLDPROJECTNAME";
            ddlProject.DataBind();
        }
        BindVesselList();
    }

    private void BindVesselList()
    {
        if (ddlProject.SelectedValue.Trim() != string.Empty)
        {
            DataTable dt = PhoenixDefectTracker.vessellist(int.Parse(ddlProject.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                ddlvesselcode.DataSource = dt;
                ddlvesselcode.DataValueField = "FLDVESSELID";
                ddlvesselcode.DataTextField = "FLDVESSELNAME";
                ddlvesselcode.DataBind();
                ddlvesselcode.Items.Insert(0, new ListItem("--SELECT--", ""));
            }
        }
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ucStatus.Text = ddlProject.SelectedItem.Text + " Issue Tracker";                        
            BindVesselList();
            BindData();
            ViewState["PROJECTNAME"] = ddlProject.SelectedValue;
            lblsep.Text = "PROJECT - " + ddlProject.SelectedItem.Text.ToUpper();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDefaultVessel()
    {
        DataTable dt = PhoenixDefectTracker.DefaultVessel();

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            int nVessel = int.Parse(dr["FLDINSTALLCODE"].ToString());
            ddlvesselcode.SelectedValue = dr["FLDINSTALLCODE"].ToString();

            if (nVessel > 0)
                ddlvesselcode.Enabled = false;
        }
    }

    protected void MenuDefectTracker_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            BindData();
            SetPageNavigator();
        }

        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        ucSEPModule.SelectedValue = "";
        SEPStatus.SelectedValue = "";
        SEPSeverity.SelectedValue = "";
        ucSEPType.SelectedValue = "";
        SEPPriority.SelectedValue = "";
        txtDescriptionSearch.Text = "";
        txtIDSearch.Text = "";
        txtSubjectSearch.Text = "";
        ucFromDate.Text = "";
        ucToDate.Text = "";
        BindData();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }
    protected void Filter_Changed(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDBUGID", "FLDTYPE", "FLDPROJECT", "FLDMODULE", "FLDSUBJECT", "FLDPRIORITY", "FLDREPORTEDBY" };
        string[] alCaptions = { "Bug Id", "Type", "Project", "Module", "Subject", "Priority", "Reported By" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixDefectTracker.BugCommentList(
            null,
            General.GetNullableString(ucSEPModule.SelectedValue),
            General.GetNullableString(SEPStatus.SelectedValue),
            General.GetNullableString(SEPPriority.SelectedValue),
            General.GetNullableString(SEPSeverity.SelectedValue),
            General.GetNullableInteger(ddlvesselcode.SelectedValue),
            General.GetNullableString(ucSEPType.SelectedValue),
            General.GetNullableString(txtIDSearch.Text),
            General.GetNullableString(txtSubjectSearch.Text),
            General.GetNullableString(txtDescriptionSearch.Text),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            int.Parse(ddlProject.SelectedValue),
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            SEPBugSearchGrid.DataSource = ds;
            SEPBugSearchGrid.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, SEPBugSearchGrid);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;       

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


    protected void SEPBugSearchGrid_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton anl = (ImageButton)e.Row.FindControl("cmdAnalysis");
            if (anl != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                anl.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugAnalysis.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                eb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugEdit.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAttachment");
            if (ab != null) ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
            if (ab != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                ab.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugAttachment.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            ImageButton cmb = (ImageButton)e.Row.FindControl("cmdComments");
            if (cmb != null) cmb.Visible = SessionUtil.CanAccess(this.ViewState, cmb.CommandName);
            if (cmb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                cmb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugComment.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            LinkButton lb = (LinkButton)e.Row.FindControl("lnkSubject");
            if (lb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugEdit.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            LinkButton lbE = (LinkButton)e.Row.FindControl("lnkBugId");
            if (lbE != null)
            {
                Label lblE = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lbE.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugEdit.aspx?dtkey=" + lblE.Text + "'); return false;");
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucDescription");
            lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
            lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");

        }

    }
    protected void SEPBugSearchGrid_Sorting(object sender, GridViewSortEventArgs se)
    {
        SEPBugSearchGrid.EditIndex = -1;
        SEPBugSearchGrid.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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
        SEPBugSearchGrid.EditIndex = -1;
        SEPBugSearchGrid.SelectedIndex = -1;
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
        SEPBugSearchGrid.SelectedIndex = -1;
        SEPBugSearchGrid.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    protected void SEPBugSearchGrid_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }
    protected void SEPBugSearchGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

}
