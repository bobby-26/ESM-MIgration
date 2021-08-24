using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class InspectionAuditScheduleMaster : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvAuditSchedule.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$lnkDoubleClick");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$lnkDoubleClickEdit");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Inspection/InspectionAuditScheduleMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAuditSchedule')", "Print Grid", "icon_print.png", "PRINT");            
            toolbar.AddImageLink("javascript:Openpopup('Filter','','InspectionAuditScheduleFilter.aspx?callfrom=ISM'); return false;", "Filter", "search.png", "FIND");
            toolbar.AddImageButton("../Inspection/InspectionAuditScheduleMaster.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuInspectionScheduleSearch.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                VesselConfiguration();

                if (Request.QueryString["ShowNavigationError"] != null)
                    ShowNavigationError();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SECTIONID"] = "";                

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Schedule", "SCHEDULE");
                toolbarmain.AddButton("Plan", "PLAN");
                toolbarmain.AddButton("Audit Plan", "AUDITPROG");
                MenuInspectionSchedulemaster.MenuList = toolbarmain.Show();
                MenuInspectionSchedulemaster.SelectedMenuIndex = 0;
            }

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void ShowNavigationError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please Select a Inspection and Navigate to other Tabs";
        ucError.Visible = true;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;
        string status = null;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDREVIEWTYPENAME", "FLDREVIEWCATEGORY", "FLDREVIEWNAME", "FLDVESSELSHORTCODE", "FLDLASTDONEDATE", "FLDREVIEWDATE", "FLDRANGEFROMDATE", "FLDSTATUSNAME" };
        string[] alCaptions = { "Serial Number", "Type", "Category", "Audit Name", "Vessel/Company Code", "Last Done Date", "Due Date", "Planned Date", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentAuditScheduleFilterCriteria;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentAuditScheduleFilterCriteria == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }

        if (nvc != null)
            status = General.GetNullableString(nvc.Get("ucStatus"));
        else
        {
            status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "ASG");
            status = status + "," + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "PLA");
        }

        DataSet ds = PhoenixInspectionAuditSchedule.AuditScheduleSearch(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditCategory")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucAudit")) : null
            , vesselid
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null
            , status
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount,
            nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : null
             , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucCharterer")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtExternalInspector")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtExternalOrganization")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlInspectorName")) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename=AuditInspectionScheduleList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Schedule</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void InspectionScheduleSearch_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentAuditScheduleFilterCriteria = null;
            BindData();
            SetPageNavigator();
        }
    }

    protected void MenuInspectionSchedulemaster_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        
        if (dce.CommandName.ToUpper().Equals("SCHEDULE"))
        {
            Response.Redirect("../Inspection/InspectionAuditScheduleMaster.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("PLAN"))
        {
            Response.Redirect("../Inspection/InspectionAuditRecordMaster.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("AUDITPROG"))
        {
            Response.Redirect("../Inspection/InspectionAuditRecordMaster.aspx?callfor=auditprogram");
            BindData();
            SetPageNavigator();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;
        string status = null;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDREVIEWTYPENAME", "FLDREVIEWCATEGORY", "FLDREVIEWNAME", "FLDVESSELSHORTCODE", "FLDLASTDONEDATE", "FLDREVIEWDATE", "FLDRANGEFROMDATE", "FLDSTATUSNAME" };
        string[] alCaptions = { "Serial Number", "Type", "Category", "Audit Name", "Vessel/Company Code", "Last Done Date", "Due Date", "Planned Date", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentAuditScheduleFilterCriteria;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentAuditScheduleFilterCriteria == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        if (nvc != null)
            status = General.GetNullableString(nvc.Get("ucStatus"));
        if(string.IsNullOrEmpty(status))
        {
            status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "ASG");
            status = status + "," + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "PLA");
        }

        DataSet ds = PhoenixInspectionAuditSchedule.AuditScheduleSearch(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditCategory")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucAudit")) : null
            , vesselid
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null
            , status
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : null
             , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucCharterer")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtExternalInspector")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtExternalOrganization")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlInspectorName")) : null);

        General.SetPrintOptions("gvAuditSchedule", "Schedule", alCaptions, alColumns, ds);


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAuditSchedule.DataSource = ds;
            gvAuditSchedule.DataBind();

            if (Filter.CurrentSelectedAuditSchedule == null)
            {
                Filter.CurrentSelectedAuditSchedule = ds.Tables[0].Rows[0]["FLDREVIEWSCHEDULEID"].ToString();
                gvAuditSchedule.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Inspection/InspectionAuditScheduleGeneral.aspx?AUDITSCHEDULEID=" + Filter.CurrentSelectedAuditSchedule;
                ViewState["PAGEURL"] = "../Inspection/InspectionAuditScheduleGeneral.aspx?AUDITSCHEDULEID=" + Filter.CurrentSelectedAuditSchedule;
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Inspection/InspectionAuditScheduleGeneral.aspx?AUDITSCHEDULEID=";
            }
            ds.Tables[0].Rows.Clear();
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvAuditSchedule);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        string a = Filter.CurrentSelectedAuditSchedule.ToString();
        gvAuditSchedule.SelectedIndex = -1;
        for (int i = 0; i < gvAuditSchedule.Rows.Count; i++)
        {
            if (gvAuditSchedule.DataKeys[i].Value.ToString().Equals(Filter.CurrentSelectedAuditSchedule.ToString()))
            {
                gvAuditSchedule.SelectedIndex = i;
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            Label lblInspectionScheduleId = (Label)gvAuditSchedule.Rows[rowindex].FindControl("lblInspectionScheduleId");
            if (lblInspectionScheduleId != null)
            {
                Filter.CurrentSelectedAuditSchedule = lblInspectionScheduleId.Text;
                ifMoreInfo.Attributes["src"] = "../Inspection/InspectionAuditScheduleGeneral.aspx?AUDITSCHEDULEID=" + Filter.CurrentSelectedAuditSchedule;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvAuditSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string auditscheduleid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblInspectionScheduleId")).Text;
                if (auditscheduleid != null)
                {
                    DeleteAuditSchedule(new Guid(auditscheduleid));                    
                    Filter.CurrentSelectedAuditSchedule = null;
                    ViewState["PAGEURL"] = null;
                    BindData();
                }

            }
            else
            {
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

    protected void gvAuditSchedule_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.NewSelectedIndex;
        BindPageURL(e.NewSelectedIndex);
    }

    private void DeleteAuditSchedule(Guid auditscheduleid)
    {
        PhoenixInspectionAuditSchedule.DeleteAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, auditscheduleid);
    }

    protected void gvAuditSchedule_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvAuditSchedule_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void gvAuditSchedule_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAuditSchedule.SelectedIndex = -1;
        gvAuditSchedule.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvAuditSchedule_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            Image imgFlag = e.Row.FindControl("imgFlag") as Image;
            if (imgFlag != null && dr["FLDOVERDUE"].ToString().Equals("2"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                imgFlag.ToolTip = "Over Due";
            }
            else if (imgFlag != null && dr["FLDOVERDUE"].ToString().Equals("1"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
                imgFlag.ToolTip = "Due";
            }
            else if (imgFlag != null && dr["FLDOVERDUE"].ToString().Equals("0"))
                imgFlag.Visible = false; 
        }

    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvAuditSchedule.SelectedIndex = -1;
        gvAuditSchedule.EditIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvAuditSchedule.SelectedIndex = -1;
        gvAuditSchedule.EditIndex = -1;
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
        gvAuditSchedule.SelectedIndex = -1;
        gvAuditSchedule.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
        if (Session["NSchedule"] != null && Session["NSchedule"].ToString() == "Y")
        {
            gvAuditSchedule.SelectedIndex = 0;
            Session["NSchedule"] = "N";
            BindPageURL(gvAuditSchedule.SelectedIndex);
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

}
