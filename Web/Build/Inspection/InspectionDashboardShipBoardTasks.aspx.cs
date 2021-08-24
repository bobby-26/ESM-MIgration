using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionDashboardShipBoardTasks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardShipBoardTasks.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvShipBoardTasks')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionDashboardShipBoardTasks.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardShipBoardTasks.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuShipBoardTasks.AccessRights = this.ViewState;
        MenuShipBoardTasks.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            Session["New"] = "N";
            ViewState["OVERDUEYN"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CARYN"] = "";

            if (Request.QueryString["OVERDUEYN"] != null && Request.QueryString["OVERDUEYN"].ToString() != "")
                ViewState["OVERDUEYN"] = Request.QueryString["OVERDUEYN"].ToString();
            else
                ViewState["OVERDUEYN"] = "0";

            if (Session["CHECKED_ITEMS"] != null)
                Session.Remove("CHECKED_ITEMS");

            VesselConfiguration();


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

            gvShipBoardTasks.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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

    protected void MenuShipBoardTasks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvShipBoardTasks.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvShipBoardTasks.Rebind();
        }
    }
    protected void ReBind()
    {
        gvShipBoardTasks.SelectedIndexes.Clear();
        gvShipBoardTasks.EditIndexes.Clear();
        gvShipBoardTasks.DataSource = null;
        gvShipBoardTasks.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCREATEDFROMNAME", "FLDSOURCENAME", "FLDHARDNAME", "FLDDEPARTMENTNAME", "FLDITEMNAME", "FLDDEFICIENCYDETAILS", "FLDTASK", "FLDTARGETDATE" };
        string[] alCaptions = { "Source Reference", "Source", "Type", "Assigned Department", "Item", "Deficiency Details", "Task", "Target" };

        DataSet ds = PhoenixInspectionLongTermAction.InspectionDashboardTaskList(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                           , General.GetNullableInteger(ViewState["OVERDUEYN"].ToString())
                                                                           , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                           , gvShipBoardTasks.PageSize
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount);

        General.SetPrintOptions("gvShipBoardTasks", "Corrective Tasks", alCaptions, alColumns, ds);


        gvShipBoardTasks.DataSource = ds;
        gvShipBoardTasks.VirtualItemCount = iRowCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCREATEDFROMNAME", "FLDSOURCENAME", "FLDHARDNAME", "FLDDEPARTMENTNAME", "FLDITEMNAME", "FLDDEFICIENCYDETAILS", "FLDTASK", "FLDTARGETDATE" };
        string[] alCaptions = { "Source Reference", "Source", "Type", "Assigned Department", "Item", "Deficiency Details", "Task", "Target" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixInspectionLongTermAction.InspectionDashboardTaskList(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                          , General.GetNullableInteger(ViewState["OVERDUEYN"].ToString())
                                                                          , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                          , gvShipBoardTasks.PageSize
                                                                          , ref iRowCount
                                                                          , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=TaskList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Corrective Tasks</h3></td>");
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
    protected void gvShipBoardTasks_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = e.Item.ItemIndex;

                if (e.CommandName.ToUpper().Equals("SELECT"))
                {
                    RadLabel lblCorrectiveActionid = (RadLabel)_gridView.Items[nRow].FindControl("lblLongTermActionId");
                    RadLabel lbl = (RadLabel)_gridView.Items[nRow].FindControl("lblcaryntext");

                    
                }
            }
                if (e.CommandName == "Page")
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
    protected void gvShipBoardTasks_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lnkRefno = (LinkButton)e.Item.FindControl("lnkTaskDetails");
            RadLabel lblCorrectiveActionid = (RadLabel)e.Item.FindControl("lblCorrectiveActionid");
            RadLabel lblcaryntext = (RadLabel)e.Item.FindControl("lblcaryntext");
            if (lnkRefno != null)
            {
                lnkRefno.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefno.CommandName);

                if (lblcaryntext.Text.Equals("1"))
                {
                    lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('Tasks', '', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?correctiveactionid=" + lblCorrectiveActionid.Text + "&DASHBOARD=1" + "');");
                }
                if (lblcaryntext.Text.Equals("0"))
                {
                    lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('Tasks', '', '" + Session["sitepath"] + "/Inspection/InspectionPreventiveTasksDetails.aspx?preventiveactionid=" + lblCorrectiveActionid.Text + "&DASHBOARD=1" + "');");
                }
            }

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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
        private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    protected void gvShipBoardTasks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvShipBoardTasks.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
