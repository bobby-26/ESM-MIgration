using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class Inspection_InspectionShipboardRATask : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionShipboardRATask.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvShipboardRATask')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardRATaskFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionShipboardRATask.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuShipBoardRATasks.AccessRights = this.ViewState;
        MenuShipBoardRATasks.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("RA Task", "RATASK", ToolBarDirection.Right);
        toolbarmain.AddButton("MOC Task", "MOCTASK", ToolBarDirection.Right);
        toolbarmain.AddButton("Preventive Task", "PATASK", ToolBarDirection.Right);
        toolbarmain.AddButton("Corrective Task", "CARTASK", ToolBarDirection.Right);
        //toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuRATask.AccessRights = this.ViewState;
        MenuRATask.MenuList = toolbarmain.Show();
        MenuRATask.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {
            Session["New"] = "N";
            if (Session["CHECKED_ITEMS"] != null)
                Session.Remove("CHECKED_ITEMS");


            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            DateTime now = DateTime.Now;
            string FromDate = now.Date.AddMonths(-3).ToShortDateString();
            string ToDate = now.Date.AddMonths(+3).ToShortDateString();
            string Status = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");

            ViewState["FROMDATE"] = FromDate.ToString();
            ViewState["TODATE"] = ToDate.ToString();
            ViewState["Status"] = Status.ToString();

            gvShipboardRATask.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["TASKID"] != null && Request.QueryString["TASKID"].ToString() != "")
                ViewState["TASKID"] = Request.QueryString["TASKID"].ToString();
            else
                ViewState["TASKID"] = null;

            if (Request.QueryString["DTKEY"] != null && Request.QueryString["DTKEY"].ToString() != null)
                ViewState["DTKEY"] = Request.QueryString["DTKEY"];
            else
                ViewState["DTKEY"] = null;
        }
    }

    protected void MenuRATask_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionShipBoardTaskFilter.aspx", true);
        }

        if (CommandName.ToUpper().Equals("CARTASK"))
        {
            Response.Redirect("../Inspection/InspectionShipBoardTasks.aspx", true);
        }

        if (CommandName.ToUpper().Equals("PATASK"))
        {
            Response.Redirect("../Inspection/InspectionPreventiveTaskList.aspx", true);
        }
        if (CommandName.ToUpper().Equals("MOCTASK"))
        {
            Response.Redirect("../Inspection/InspectionMOCActionPlanShipboardTaskList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("RATASK"))
        {
            Response.Redirect("../Inspection/InspectionShipboardRATask.aspx", false);
        }
    }

    protected void MenuShipBoardRATasks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvShipboardRATask.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            InspectionFilter.CurrentShipBoardRATaskFilter = null;
            BindData();
            gvShipboardRATask.Rebind();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;
        // int? departmenttype = 1;

        string[] alColumns = { "FLDVESSELNAME", "FLDREFNO", "FLDTASK", "FLDPICNAME", "FLDESTIMATEDFINISHDATE", "FLDACTUALFINISHDATE" };
        string[] alCaptions = { "Vessel", "RA Reference No", "Task", "Responsibility", "Target", "Completion" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = InspectionFilter.CurrentShipBoardRATaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (InspectionFilter.CurrentShipBoardRATaskFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }
        DataSet ds = PhoenixInspectionRiskAssessmentMachineryExtn.ShipBoardRATasksSearch(
                                                                  null
                                                                , vesselid
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSourceRefNo")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvShipboardRATask.CurrentPageIndex + 1
                                                                , gvShipboardRATask.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=ShipboardRATaskList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Shipboard RA Tasks</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDREFNO", "FLDTASK", "FLDPICNAME", "FLDESTIMATEDFINISHDATE", "FLDACTUALFINISHDATE" };
        string[] alCaptions = { "Vessel", "RA Reference No", "Task", "Responsibility", "Target", "Completion" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = InspectionFilter.CurrentShipBoardRATaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (InspectionFilter.CurrentShipBoardRATaskFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        DataSet ds = PhoenixInspectionRiskAssessmentMachineryExtn.ShipBoardRATasksSearch(
                                                                  null
                                                                , vesselid
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSourceRefNo")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvShipboardRATask.CurrentPageIndex + 1
                                                                , gvShipboardRATask.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                );

        General.SetPrintOptions("gvShipboardRATask", "Shipboard RA Tasks", alCaptions, alColumns, ds);

        gvShipboardRATask.DataSource = ds;
        gvShipboardRATask.VirtualItemCount = iRowCount;
    }

    protected void gvShipboardRATask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvShipboardRATask.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvShipboardRATask_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        RadLabel lblRaMachinaryId = (RadLabel)e.Item.FindControl("lblSourceId");

        if (eb != null)
            eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionShipboardRATaskEdit.aspx?RAMACHINERYID=" + lblRaMachinaryId.Text + "'); return false;");
    }

    protected void gvShipboardRATask_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
            }
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
            gvShipboardRATask.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}