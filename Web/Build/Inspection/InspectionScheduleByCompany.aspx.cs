using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class InspectionScheduleByCompany : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvScheduleForCompany.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvScheduleForCompany.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Inspection/InspectionScheduleByCompany.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvScheduleForCompany')", "Print Grid", "icon_print.png", "PRINT");
                //toolbar.AddImageButton("javascript:Openpopup('Filter','','InspectionScheduleByCompanyFilter.aspx'); return false;", "Find", "search.png", "FIND");
                toolbar.AddImageButton("../Inspection/InspectionScheduleByCompany.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
                //toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','InspectionScheduleByCompanyManual.aspx')", "Add Manual Inspection", "add.png", "ADD");
                toolbar.AddImageButton("javascript:Openpopup('SIRE','','../Inspection/InspectionSIREDue.aspx',null);return true;", "SIRE Next Due", "SIRE_Due.png", "SIRE");
                MenuScheduleGroup.AccessRights = this.ViewState;
                MenuScheduleGroup.MenuList = toolbar.Show();
                MenuScheduleGroup.SetTrigger(pnlBudgetGroup);

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                VesselConfiguration();
                ucConfirm.Visible = false;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Search", "SEARCH");
                MenuGeneral.AccessRights = this.ViewState;
                MenuGeneral.MenuList = toolbarmain.Show();
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

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionScheduleByCompanyFilter.aspx");
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDISMANUAL", "FLDCOMPANYNAME", "FLDINSPECTIONSHORTCODE", "FLDLASTDONEDATE", "FLDDUEDATE", 
                                 "FLDBASISDETAILS", "FLDPREVIOUSSCHEDULENUMBER", "FLDPLANNEDDATE", "FLDSEAPORTNAME", "FLDNAMEOFINSPECTOR", 
                                 "FLDSCHEDULESTATUS" };
        string[] alCaptions = { "Vessel", "M/C", "Company", "Inspection", "Last Done", "Due", "Basis", "Last Done Ref", "Planned",
                                  "Planned Port", "Inspector", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentScheduleByCompanyFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentScheduleByCompanyFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        ds = PhoenixInspectionSchedule.InspectionScheduleByCompanySearch(
                                                    vesselid,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ddlCompany")) : null,
                                                    chkShowAll.Checked == true ? 1 : 0,//General.GetNullableInteger(null),
                                                    sortexpression, sortdirection,
                                                    (int)ViewState["PAGENUMBER"],
                                                    General.ShowRecords(null),
                                                    ref iRowCount,
                                                    ref iTotalPageCount,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ucVetting")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ddlPlanned")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucDoneFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucDoneTo")) : null,
                                                    nvc != null ? General.GetNullableString(nvc.Get("txtInspector")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ddlBasis")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucPlannedFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucPlannedTo")) : null,
                                                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=Schedule.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>CDI / SIRE Schedule</h3></td>");
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

    protected void BudgetGroup_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvScheduleForCompany.SelectedIndex = -1;
                gvScheduleForCompany.EditIndex = -1;

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
                Filter.CurrentScheduleByCompanyFilter = null;
                ViewState["PAGENUMBER"] = 1;
                txtnopage.Text = "";
                BindData();
                SetPageNavigator();
            }
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
        int vesselid = -1;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDVESSELNAME", "FLDISMANUAL", "FLDCOMPANYNAME", "FLDINSPECTIONSHORTCODE", "FLDLASTDONEDATE", "FLDDUEDATE", 
                                 "FLDBASISDETAILS", "FLDPREVIOUSSCHEDULENUMBER", "FLDPLANNEDDATE", "FLDSEAPORTNAME", "FLDNAMEOFINSPECTOR", 
                                 "FLDSCHEDULESTATUS" };
        string[] alCaptions = { "Vessel", "M/C", "Company", "Inspection", "Last Done", "Due", "Basis", "Last Done Ref", "Planned",
                                  "Planned Port", "Inspector", "Status" };

        NameValueCollection nvc = Filter.CurrentScheduleByCompanyFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentScheduleByCompanyFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        DataSet ds = PhoenixInspectionSchedule.InspectionScheduleByCompanySearch(
                                                    vesselid,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ddlCompany")) : null,
                                                    chkShowAll.Checked == true ? 1 : 0,//General.GetNullableInteger(null),
                                                    sortexpression, sortdirection,
                                                    (int)ViewState["PAGENUMBER"],
                                                    General.ShowRecords(null),
                                                    ref iRowCount,
                                                    ref iTotalPageCount,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ucVetting")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ddlPlanned")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucDoneFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucDoneTo")) : null,
                                                    nvc != null ? General.GetNullableString(nvc.Get("txtInspector")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ddlBasis")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucPlannedFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucPlannedTo")) : null,
                                                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvScheduleForCompany", "CDI / SIRE Schedule", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvScheduleForCompany.DataSource = ds;
            gvScheduleForCompany.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvScheduleForCompany);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void gvScheduleForCompany_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvScheduleForCompany_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvScheduleForCompany.SelectedIndex = -1;
        gvScheduleForCompany.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvScheduleForCompany_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            ViewState["currentEditRow"] = nCurrentRow;

            Label lblScheduleId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblScheduleId");

            if (!IsValidPlan(((Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblStatusId")).Text,
                ((UserControlDate)gvScheduleForCompany.Rows[nCurrentRow].FindControl("ucPlannedDateEdit")).Text
                ))
            {
                ucError.Visible = true;
                return;
            }

            InsertScheduleDetails(
                       int.Parse(((Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblVesselId")).Text)
                     , ((UserControlDate)gvScheduleForCompany.Rows[nCurrentRow].FindControl("ucLastDoneDateEdit")).Text
                     , ((UserControlDate)gvScheduleForCompany.Rows[nCurrentRow].FindControl("ucDueDateEdit")).Text
                     , ((Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblCompanyId")).Text
                     , ((TextBox)gvScheduleForCompany.Rows[nCurrentRow].FindControl("txtBasisScheduleId")).Text
                     , ((Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblScheduleId")).Text
                     , ((Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblScheduleByCompanyId")).Text
                     , ((UserControlDate)gvScheduleForCompany.Rows[nCurrentRow].FindControl("ucPlannedDateEdit")).Text
                     , ((UserControlSeaport)gvScheduleForCompany.Rows[nCurrentRow].FindControl("ucSeaportEdit")).SelectedSeaport
                     , ((TextBox)gvScheduleForCompany.Rows[nCurrentRow].FindControl("txtInspectorEdit")).Text
                     , ((Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblIsManual")).Text);


            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvScheduleForCompany_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvScheduleForCompany.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvScheduleForCompany_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvScheduleForCompany_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("CREATESCHEDULE"))
            {
                Label lblDueDate = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDueDate");
                string strduedate = lblDueDate.Text;

                if (General.GetNullableDateTime(strduedate) == null)
                {
                    ucError.ErrorMessage = "Due date is required to create the schedule.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["CURRENTROW"] = nCurrentRow;
                    ucConfirm.Visible = true;
                    ucConfirm.Text = "Are you sure to plan this inspection?";
                    return;
                }

            }
            else if (e.CommandName.ToUpper().Equals("NEWSCHEDULE"))
            {
                Label lblScheduleByCompanyId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblScheduleByCompanyId");
                PhoenixInspectionSchedule.InsertInspectionSchedulebyBasis(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(lblScheduleByCompanyId.Text));

                ucStatus.Text = "Inspection is planned successfully.";
                BindData();
                SetPageNavigator();
            }
            else if (e.CommandName.ToUpper().Equals("UNPLAN"))
            {
                Label lblScheduleByCompanyId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblScheduleByCompanyId");
                Label lblSchedule = (Label)_gridView.Rows[nCurrentRow].FindControl("lblScheduleId");

                PhoenixInspectionSchedule.UnplanInspectionSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(lblScheduleByCompanyId.Text)
                                            , new Guid(lblSchedule.Text));

                ucStatus.Text = "Inspection is un-planned successfully.";

                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvScheduleForCompany_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvScheduleForCompany_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            DataRowView dv = (DataRowView)e.Row.DataItem;
            Image imgFlag = e.Row.FindControl("imgFlag") as Image;
            if (imgFlag != null && dv["FLDDUEOVERDUEYN"].ToString().Equals("3"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                imgFlag.ToolTip = "Overdue";
            }
            else if (imgFlag != null && dv["FLDDUEOVERDUEYN"].ToString().Equals("2"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
                imgFlag.ToolTip = "Due within 30 days";
            }
            else if (imgFlag != null && dv["FLDDUEOVERDUEYN"].ToString().Equals("1"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "green-symbol.png";
                imgFlag.ToolTip = "Due within 60 days";
            }
            else
            {
                if (imgFlag != null) imgFlag.Visible = false;
            }

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, sb.CommandName))
                    sb.Visible = false;
            }

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cb.CommandName))
                    cb.Visible = false;
            }

            ImageButton imgNewSchedule = (ImageButton)e.Row.FindControl("imgNewSchedule");
            if (imgNewSchedule != null)
            {
                if (General.GetNullableGuid(dv["FLDBASISID"].ToString()) != null && General.GetNullableGuid(dv["FLDSCHEDULEBYCOMPANYIDFORBASIS"].ToString()) == null)
                    imgNewSchedule.Visible = true;
            }
            ImageButton imgCreateSchedule = (ImageButton)e.Row.FindControl("imgCreateSchedule");
            if (imgCreateSchedule != null && General.GetNullableGuid(dv["FLDBASISID"].ToString()) != null)
            {
                imgCreateSchedule.Visible = false;
            }

            ImageButton cmdReport = (ImageButton)e.Row.FindControl("cmdReport");
            if (cmdReport != null)
            {
                if (dv["FLDSHEDULEID"] != null && dv["FLDSHEDULEID"].ToString() != "")
                    cmdReport.Attributes.Add("onclick", "javascript:Openpopup('Report','','../Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + dv["FLDSHEDULEID"].ToString() + "'); return true;");
            }

            HtmlImage imgBasis = (HtmlImage)e.Row.FindControl("imgBasis");
            Label lblVesselId = (Label)e.Row.FindControl("lblVesselId");
            Label lblCompanyId = (Label)e.Row.FindControl("lblCompanyId");
            Label lblInspectionId = (Label)e.Row.FindControl("lblInspectionId");

            if (imgBasis != null)
                imgBasis.Attributes.Add("onclick", "return showPickList('spanBasisInspectionSchedule', 'codehelp1', '', '../Common/CommonPickListBasisInspectionSchedule.aspx?VESSELID=" + lblVesselId.Text + "&COMPANYID=" + lblCompanyId.Text + "&INSPECTIONID=" + lblInspectionId.Text + "', true);");

            UserControlDate ucPlannedDateEdit = (UserControlDate)e.Row.FindControl("ucPlannedDateEdit");
            UserControlSeaport ucSeaportEdit = (UserControlSeaport)e.Row.FindControl("ucSeaportEdit");
            TextBox txtInspectorEdit = (TextBox)e.Row.FindControl("txtInspectorEdit");
            Label lblPlannedDateEdit = (Label)e.Row.FindControl("lblPlannedDateEdit");
            Label lblPlannedPortEdit = (Label)e.Row.FindControl("lblPlannedPortEdit");
            Label lblInspectorEdit = (Label)e.Row.FindControl("lblInspectorEdit");

            Label lblStatusId = (Label)e.Row.FindControl("lblStatusId");
            if (lblStatusId != null && lblStatusId.Text != "")
            {
                if (imgCreateSchedule != null) imgCreateSchedule.Visible = false;
            }
            else
            {
                if (cmdReport != null) cmdReport.Visible = false;
            }
            if (ucSeaportEdit != null)
            {
                ucSeaportEdit.SeaportList = PhoenixRegistersSeaport.ListSeaport();
                ucSeaportEdit.DataBind();
                ucSeaportEdit.SelectedSeaport = dv["FLDPORTID"].ToString();
            }
            if (lblStatusId != null && lblStatusId.Text == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
            {
                if (ucPlannedDateEdit != null) ucPlannedDateEdit.Visible = true;
                if (ucSeaportEdit != null) ucSeaportEdit.Visible = true;
                if (txtInspectorEdit != null) txtInspectorEdit.Visible = true;
                if (lblPlannedDateEdit != null) lblPlannedDateEdit.Visible = false;
                if (lblPlannedPortEdit != null) lblPlannedPortEdit.Visible = false;
                if (lblInspectorEdit != null) lblInspectorEdit.Visible = false;
            }
            else
            {
                if (ucPlannedDateEdit != null) ucPlannedDateEdit.Visible = false;
                if (ucSeaportEdit != null) ucSeaportEdit.Visible = false;
                if (txtInspectorEdit != null) txtInspectorEdit.Visible = false;
                if (lblPlannedDateEdit != null) lblPlannedDateEdit.Visible = true;
                if (lblPlannedPortEdit != null) lblPlannedPortEdit.Visible = true;
                if (lblInspectorEdit != null) lblInspectorEdit.Visible = true;
            }

            LinkButton lnkScheduleNumber = (LinkButton)e.Row.FindControl("lnkScheduleNumber");
            if (lnkScheduleNumber != null)
            {
                if (dv["FLDPREVIOUSSCHEDULEID"] != null && dv["FLDPREVIOUSSCHEDULEID"].ToString() != "")
                    lnkScheduleNumber.Attributes.Add("onclick", "javascript:Openpopup('Details','','../Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + dv["FLDPREVIOUSSCHEDULEID"].ToString() + "&viewonly=1'); return true;");
            }

            LinkButton lnkBasisDetails = (LinkButton)e.Row.FindControl("lnkBasisDetails");
            if (lnkBasisDetails != null)
            {
                if (dv["FLDBASISID"] != null && dv["FLDBASISID"].ToString() != "")
                    lnkBasisDetails.Attributes.Add("onclick", "javascript:Openpopup('Details','','../Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + dv["FLDBASISID"].ToString() + "&viewonly=1'); return true;");
            }

            Label lblPlannedPort = (Label)e.Row.FindControl("lblPlannedPort");
            if (lblPlannedPort != null)
            {
                UserControlToolTip ucToolTipPort = (UserControlToolTip)e.Row.FindControl("ucToolTipPort");
                lblPlannedPort.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipPort.ToolTip + "', 'visible');");
                lblPlannedPort.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipPort.ToolTip + "', 'hidden');");
            }

            Label lblInspector = (Label)e.Row.FindControl("lblInspector");
            if (lblInspector != null)
            {
                UserControlToolTip ucToolTipInspector = (UserControlToolTip)e.Row.FindControl("ucToolTipInspector");
                lblInspector.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipInspector.ToolTip + "', 'visible');");
                lblInspector.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipInspector.ToolTip + "', 'hidden');");
            }

            ImageButton imgUnPlan = (ImageButton)e.Row.FindControl("imgUnPlan");

            if (dv["FLDSHEDULEID"] != null && dv["FLDSHEDULEID"].ToString() != "" && imgUnPlan != null)
            {
                imgUnPlan.Visible = true;

                if (!SessionUtil.CanAccess(this.ViewState, imgUnPlan.CommandName))
                    imgUnPlan.Visible = false;

                imgUnPlan.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you Sure you want to UnPlan?'); return false;");
            }

            if (imgNewSchedule != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgNewSchedule.CommandName))
                    imgNewSchedule.Visible = false;
            }
            if (imgCreateSchedule != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCreateSchedule.CommandName))
                    imgCreateSchedule.Visible = false;
            }
            if (cmdReport != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdReport.CommandName))
                    cmdReport.Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvScheduleForCompany.SelectedIndex = -1;
        gvScheduleForCompany.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvScheduleForCompany.SelectedIndex = -1;
        gvScheduleForCompany.EditIndex = -1;

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
        gvScheduleForCompany.SelectedIndex = -1;
        gvScheduleForCompany.EditIndex = -1;
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
        {
            return true;
        }

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

    private void InsertScheduleDetails(int vesselid, string lastdonedate, string duedate, string company, string basis,
        string scheduleid, string schedulebycompanyid, string planneddate, string plannedport, string nameofinspector, string ismanualinspection)
    {
        try
        {
            PhoenixInspectionSchedule.InsertInspectionScheduleByCompany(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , vesselid
                , General.GetNullableDateTime(lastdonedate)
                , General.GetNullableDateTime(duedate)
                , General.GetNullableGuid(company)
                , General.GetNullableGuid(basis)
                , General.GetNullableGuid(scheduleid)
                , General.GetNullableGuid(schedulebycompanyid)
                , General.GetNullableDateTime(planneddate)
                , General.GetNullableInteger(plannedport)
                , General.GetNullableString(nameofinspector)
                , General.GetNullableInteger(ismanualinspection)
                );
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void chkShowAll_Changed(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    private bool IsValidDetails(string vesselid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required";

        return (!ucError.IsError);
    }

    private bool IsValidPlan(string status, string planneddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (status == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
        {
            //if (General.GetNullableDateTime(planneddate) == null)
            //    ucError.ErrorMessage = "Planned Date is required.";
        }

        return (!ucError.IsError);
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                int nCurrentRow = int.Parse(ViewState["CURRENTROW"].ToString());

                Label lblDueDate = (Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblDueDate");
                string strduedate = lblDueDate.Text;

                Label lblBasisCompanyId = (Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblBasisCompanyId");
                Label lblLastDoneDate = (Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblLastDoneDate");
                Label lblScheduleByCompanyId = (Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblScheduleByCompanyId");
                Label lblCompanyId = (Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblCompanyId");
                Label lblVesselId = (Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblVesselId");
                Label lblInspectionId = (Label)gvScheduleForCompany.Rows[nCurrentRow].FindControl("lblInspectionId");

                PhoenixInspectionSchedule.InspectionForCompanyInsert(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , new Guid(lblInspectionId.Text)
                                , Int16.Parse(lblVesselId.Text)
                                , lblLastDoneDate != null ? General.GetNullableDateTime(lblLastDoneDate.Text) : null
                                , null
                                , null
                                , null
                                , DateTime.Parse(strduedate)
                                , lblScheduleByCompanyId != null ? General.GetNullableGuid(lblScheduleByCompanyId.Text) : null
                                , General.GetNullableGuid(lblCompanyId.Text)
                                );

                ucStatus.Text = "Inspection is planned successfully.";
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Text_Changed(object sender, EventArgs e)
    {
        BindData();
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ucLastDoneDateEdit_TextChanged(object sender, EventArgs e)
    {
        UserControlDate ucLastDoneDate = (UserControlDate)sender;
        GridViewRow row = (GridViewRow)ucLastDoneDate.Parent.Parent;
        ucLastDoneDate = (UserControlDate)row.FindControl("ucLastDoneDateEdit");
        UserControlDate ucDueDate = (UserControlDate)row.FindControl("ucDueDateEdit");
        Label lblInspectionIdEdit = (Label)row.FindControl("lblInspectionIdEdit");
        int frequency = 0;
        DataSet ds = PhoenixInspection.EditInspection(new Guid(lblInspectionIdEdit.Text));
        if (ds.Tables[0].Rows.Count > 0)
            frequency = int.Parse(ds.Tables[0].Rows[0]["FLDFREQUENCYINMONTHS"].ToString());

        if (ucLastDoneDate != null && General.GetNullableDateTime(ucLastDoneDate.Text) != null)
        {
            DateTime dtLastDoneDate = Convert.ToDateTime(ucLastDoneDate.Text);
            DateTime dtDueDate = dtLastDoneDate.AddMonths(frequency);
            ucDueDate.Text = dtDueDate.ToString();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void ClearBasis(object sender, EventArgs e)
    {
        ImageButton imgClearBasis = (ImageButton)sender;
        GridViewRow row = (GridViewRow)imgClearBasis.Parent.Parent;
        imgClearBasis = (ImageButton)row.FindControl("imgClearBasis");
        TextBox txtCompany = (TextBox)row.FindControl("txtCompany");
        TextBox txtBasis = (TextBox)row.FindControl("txtBasis");
        TextBox txtBasisScheduleId = (TextBox)row.FindControl("txtBasisScheduleId");
        if (txtCompany != null) txtCompany.Text = "";
        if (txtBasis != null) txtBasis.Text = "";
        if (txtBasisScheduleId != null) txtBasisScheduleId.Text = "";
    }
}
