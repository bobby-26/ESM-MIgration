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

public partial class Inspection_InspectionDashBoardMocActionPlanTask : PhoenixBasePage
{    

    protected override void Render(HtmlTextWriter writer)
    {
        //foreach (GridViewRow r in gvMOCShipboardTask.Rows)
        //{
        //    if (r.RowType == DataControlRowType.DataRow)
        //    {
        //        Page.ClientScript.RegisterForEventValidation(gvMOCShipboardTask.UniqueID, "Select$" + r.RowIndex.ToString());
        //    }
        //}
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardMocActionPlanTask.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMOCShipboardTask')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuShipBoardTasks.AccessRights = this.ViewState;
        MenuShipBoardTasks.MenuList = toolbar.Show();

        cmdHiddenSubmit.Attributes.Add("style", "display:none");

        if (!IsPostBack)
        {

            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["OFFICEYN"] = "";

            if (Request.QueryString["Officeyn"].ToString() != null && Request.QueryString["Officeyn"].ToString() != string.Empty)
                ViewState["OFFICEYN"] = Request.QueryString["Officeyn"].ToString();

            gvMOCShipboardTask.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME","FLDSOURCE", "FLDMOCREFERENCENO", "FLDDEPARTMENTNAME", "FLDACTIONTOBETAKEN", "FLDPICNAME",
                                 "FLDSTATUSNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDCLOSEDDATE" };
        string[] alCaptions = { "Vessel","Source", "Reference Number", "Department", "Task", "Person In Charge","Task Status",
                                  "Target Date", "Completed Date", "Closed Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionOfficeDashboard.DashBoardActionPlanSearch(General.GetNullableInteger(ViewState["OFFICEYN"].ToString())                                                                
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvMOCShipboardTask.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                             );

        General.SetPrintOptions("gvMOCShipboardTask", "MOC Task List", alCaptions, alColumns, ds);

        gvMOCShipboardTask.DataSource = ds;
        gvMOCShipboardTask.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

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

        DataSet ds = PhoenixInspectionOfficeDashboard.DashBoardActionPlanSearch(General.GetNullableInteger(ViewState["OFFICEYN"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvMOCShipboardTask.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                             );

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
        Rebind();
    }

    protected void gvMOCShipboardTask_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }

    protected void gvMOCShipboardTask_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        //{
        //    e.Row.TabIndex = -1;
        //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvMOCShipboardTask, "Select$" + e.Row.RowIndex.ToString(), false);
        //}
    }

    protected void gvMOCShipboardTask_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        Rebind();
    }

    protected void gvMOCShipboardTask_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
                Response.Redirect("../Inspection/InspectionMOCShipBoardTaskDetails.aspx?MOCActionplanid=" + ((RadLabel)e.Item.FindControl("lblmocactionplanid")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblVesselId")).Text, false);
            }

            else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCShipboardTask_ItemDataBound(Object sender, GridItemEventArgs e)
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
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=MOCACTIONPLAN&cmdname=MOCACTIONPLANUPLOAD&VESSELID=" + lblVesselId.Text + "'); return true;");
            }
        }
    }

    //protected void gvMOCShipboardTask_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvMOCShipboardTask.SelectedIndex = se.NewSelectedIndex;
    //}

    protected void gvMOCShipboardTask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOCShipboardTask.CurrentPageIndex + 1;
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
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvMOCShipboardTask.SelectedIndexes.Clear();
        gvMOCShipboardTask.EditIndexes.Clear();
        gvMOCShipboardTask.DataSource = null;
        gvMOCShipboardTask.Rebind();
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //if (CommandName.ToUpper().Equals("SEARCH"))
        //{
        //    Response.Redirect("../Inspection/InspectionShipBoardTaskFilter.aspx", true);
        //}
        if (CommandName.ToUpper().Equals("CARTASK"))
        {
            Response.Redirect("../Inspection/InspectionShipBoardTasks.aspx", true);
        }
        if (CommandName.ToUpper().Equals("PATASK"))
        {
            Response.Redirect("../Inspection/InspectionPreventiveTaskList.aspx", true);
        }
        if (CommandName.ToUpper().Equals("RATASK"))
        {
            Response.Redirect("../Inspection/InspectionShipboardRATask.aspx", false);
        }
    }
}