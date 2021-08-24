using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCActionPlanOfficeTaskList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        //foreach (GridViewRow r in gvMOCOfficeTask.Rows)
        //{
        //    if (r.RowType == DataControlRowType.DataRow)
        //    {
        //        Page.ClientScript.RegisterForEventValidation(gvMOCOfficeTask.UniqueID, "Select$" + r.RowIndex.ToString());
        //    }
        //}
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCActionPlanOfficeTaskList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMOCOfficeTask')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"]+ "/Inspection/InspectionMOCTaskListFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCActionPlanOfficeTaskList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuShipBoardTasks.AccessRights = this.ViewState;
        MenuShipBoardTasks.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();

        toolbar.AddButton("RA Task", "RATASK", ToolBarDirection.Right);
        toolbar.AddButton("MOC Task", "MOCTASK", ToolBarDirection.Right);
        toolbar.AddButton("Preventive Task", "TASK", ToolBarDirection.Right);
        toolbar.AddButton("Corrective Task", "CARTASK", ToolBarDirection.Right);

        MenuOrderFormMain.AccessRights = this.ViewState;
        MenuOrderFormMain.MenuList = toolbar.Show();
        MenuOrderFormMain.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {
            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            gvMOCOfficeTask.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void MenuShipBoardTasks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentSelectedMOCTask = null;
            Filter.CurrentMOCTaskFilter = null;
            Rebind();
        }
    }
    protected void Rebind()
    {
        gvMOCOfficeTask.SelectedIndexes.Clear();
        gvMOCOfficeTask.EditIndexes.Clear();
        gvMOCOfficeTask.DataSource = null;
        gvMOCOfficeTask.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME","FLDSOURCE", "FLDMOCREFERENCENO", "FLDDEPARTMENTNAME", "FLDACTIONTOBETAKEN", "FLDPICNAME",
                                 "FLDSTATUSNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDCLOSEDDATE" };
        string[] alCaptions = { "Vessel","Source", "Reference Number", "Department", "Task", "Person In Charge","Task Status",
                                  "Target Date", "Completed Date", "Closed Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentMOCTaskFilter;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataSet ds = PhoenixInspectionMOCActionPlan.MOCOfficeTaskSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                                                                , vesselid
                                                                , General.GetNullableString((nvc != null ? nvc["txtMOCRefNo"] : string.Empty))
                                                                , General.GetNullableDateTime((nvc != null ? nvc["txtFrom"] : string.Empty))
                                                                , General.GetNullableDateTime((nvc != null ? nvc["txtTo"] : string.Empty))
                                                                , General.GetNullableDateTime((nvc != null ? nvc["txtDoneDateFrom"] : string.Empty))
                                                                , General.GetNullableDateTime((nvc != null ? nvc["txtDoneDateTo"] : string.Empty))
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvMOCOfficeTask.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        General.SetPrintOptions("gvMOCOfficeTask", "MOC Task List", alCaptions, alColumns, ds);
        gvMOCOfficeTask.DataSource = ds;
        gvMOCOfficeTask.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME","FLDSOURCE", "FLDMOCREFERENCENO", "FLDDEPARTMENTNAME", "FLDACTIONTOBETAKEN", "FLDPICNAME",
                                 "FLDSTATUSNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDCLOSEDDATE" };
        string[] alCaptions = { "Vessel","Source", "Reference Number", "Department", "Task", "Person In Charge","Task Status",
                                  "Target Date", "Completed Date", "Closed Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentMOCTaskFilter;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataSet ds = PhoenixInspectionMOCActionPlan.MOCOfficeTaskSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                                                                , vesselid
                                                                , General.GetNullableString((nvc != null ? nvc["txtMOCRefNo"] : string.Empty))
                                                                , General.GetNullableDateTime((nvc != null ? nvc["txtFrom"] : string.Empty))
                                                                , General.GetNullableDateTime((nvc != null ? nvc["txtTo"] : string.Empty))
                                                                , General.GetNullableDateTime((nvc != null ? nvc["txtDoneDateFrom"] : string.Empty))
                                                                , General.GetNullableDateTime((nvc != null ? nvc["txtDoneDateTo"] : string.Empty))
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvMOCOfficeTask.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        Response.AddHeader("Content-Disposition", "attachment; filename=MOCTaskList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>MOC Task List</h3></td>");
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

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvMOCOfficeTask_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }

    protected void gvMOCOfficeTask_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        //{
        //    e.Row.TabIndex = -1;
        //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvMOCOfficeTask, "Select$" + e.Row.RowIndex.ToString(), false);
        //}
    }

    protected void gvMOCOfficeTask_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        Rebind();
    }

    protected void gvMOCOfficeTask_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


            if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
                Response.Redirect("../Inspection/InspectionMOCShipBoardTaskDetails.aspx?&departmentid=1&MOCActionplanid=" + ((RadLabel)e.Item.FindControl("lblmocactionplanid")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblVesselId")).Text, false);
            }

            else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {

            }
            else if (e.CommandName=="Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCOfficeTask_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {

    }

    protected void gvMOCOfficeTask_RowEditing(object sender, GridViewEditEventArgs de)
    {
    }

    protected void gvMOCOfficeTask_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadLabel lblPIC = (RadLabel)e.Item.FindControl("lblPIC");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucPIC");
            if (uct != null)
            {
                lblPIC.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblPIC.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            LinkButton lnkTask = (LinkButton)e.Item.FindControl("lnkTask");
            UserControlToolTip uct1 = (UserControlToolTip)e.Item.FindControl("ucToolTip");
            if (uct != null)
            {
                lnkTask.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct1.ToolTip + "', 'visible');");
                lnkTask.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct1.ToolTip + "', 'hidden');");
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.QUALITY + "&type=MOCACTIONPLAN&cmdname=MOCACTIONPLANUPLOAD&VESSELID=" + lblVesselId.Text + "'); return true;");
            }
        }
    }

    //protected void gvMOCOfficeTask_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvMOCOfficeTask.SelectedIndex = se.NewSelectedIndex;
    //}

    protected void gvMOCOfficeTask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOCOfficeTask.CurrentPageIndex + 1;
            BindData();
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
            if (Session["NewTask"] != null && Session["NewTask"].ToString() == "Y")
            {
                ViewState["TASKID"] = null;
                Session["NewTask"] = "N";
            }
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void selection_Changed(Object sender, EventArgs e)
    {
        Rebind();
        //SetPageNavigator();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //if (CommandName.ToUpper().Equals("SEARCH"))
        //{
        //    Response.Redirect("../Inspection/InspectionOfficeTaskFilter.aspx", true);
        //}
        if ((CommandName.ToUpper().Equals("TASK")) && (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "PHOENIX"))
        {
            Response.Redirect("../Inspection/InspectionLongTermActionOfficeList.aspx", true);
        }
        if ((CommandName.ToUpper().Equals("TASK")) && (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE"))
        {
            Response.Redirect("../Inspection/InspectionLongTermActionList.aspx", true);
        }
        if (CommandName.ToUpper().Equals("CARTASK"))
        {
            Response.Redirect("../Inspection/InspectionOfficeCorrectiveTasks.aspx", true);
        }
        if (CommandName.ToUpper().Equals("RATASK"))
        {
            Response.Redirect("../Inspection/InspectionOfficeCorrectiveRATasks.aspx", true);
        }
    }
}
