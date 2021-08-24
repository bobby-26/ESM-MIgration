using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using Telerik.Web.UI;

public partial class OwnersMaintenanceWorkOrder : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersMaintenanceWorkOrder.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersMaintenanceWorkOrder.aspx?" + Request.QueryString, "Filter", "<i class=\"fa fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Owners/OwnersMaintenanceWorkOrder.aspx?" + Request.QueryString, "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuMaintenanceDue.AccessRights = this.ViewState;
            MenuMaintenanceDue.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                if (Request.QueryString["od"] != null)
                {
                    ViewState["od"] = Request.QueryString["od"].ToString();
                    txtToDate.Text = DateTime.Now.ToString();
                }
                else
                {
                    txtFromDate.Text = DateTime.Now.ToString();
                    txtToDate.Text = DateTime.Now.AddMonths(1).ToString();
                }
                ddlUserVessel.SelectedVessel = Filter.SelectedOwnersReportVessel;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvWorkOrder.SelectedIndexes.Clear();
        gvWorkOrder.EditIndexes.Clear();
        gvWorkOrder.DataSource = null;
        gvWorkOrder.Rebind();
    }
    protected void MenuMaintenanceDue_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
                ShowExcel();
            if (CommandName.ToUpper().Equals("FIND"))
            {

                if (!IsValidFilter(GetVessel()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();
                }
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
                if (Request.QueryString["od"] != null)
                {
                    txtToDate.Text = DateTime.Now.ToString();
                }
                else
                {
                    txtFromDate.Text = DateTime.Now.ToString();
                    txtToDate.Text = DateTime.Now.AddMonths(1).ToString();
                }
                txtComponentName.Text = string.Empty;
                txtWorkorderName.Text = "";
                txtWorkordernumber.Text = "";
                ddlPriority.SelectedValue = "";
                ucDiscipline.SelectedDiscipline = "";
                ddlUserVessel.SelectedVessel = "";
                ddlVessel.SelectedVessel = "";
                chkDefect.Checked = false;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE", "FLDRUNHOURSINCE" };
        string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed", "Run Hours Since" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixOwnersPlannedMaintenance.OwnersWorkOrderSearch(General.GetNullableInteger(GetVessel()).HasValue ? int.Parse(GetVessel()) : 0
                   , General.GetNullableString(txtWorkordernumber.Text), General.GetNullableString(txtWorkorderName.Text), null
                   , null, General.GetNullableString(txtComponentName.Text), (ViewState["od"] != null ? "19" : "1296,21,18,20")
                   , null, null, General.GetNullableDateTime(txtFromDate.Text)
                   , General.GetNullableDateTime(txtToDate.Text), null, null, null, null, null, General.GetNullableInteger(ddlPriority.SelectedValue)
                   , General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(chkDefect.Checked == true ? "1" : "")
                   , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
                   , iRowCount
                   , ref iRowCount
                   , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MaintenanceDue.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Maintenance Due</h3></td>");
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
    private string GetVessel()
    {
        string vessel = Filter.SelectedOwnersReportVessel;
        if (string.IsNullOrEmpty(Request.QueryString["p"]))
        {
            vessel = ddlVessel.SelectedVessel;
            ddlUserVessel.Visible = false;
        }
        else
        {
            vessel = ddlUserVessel.SelectedVessel;
            ddlVessel.Visible = false;
        }
        return vessel;
    }
    public bool IsValidFilter(string vessellist)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessellist.Equals("") || vessellist.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }

        return (!ucError.IsError);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE", "FLDRUNHOURSINCE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed", "Run Hours Since" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixOwnersPlannedMaintenance.OwnersWorkOrderSearch(General.GetNullableInteger(GetVessel()).HasValue ? int.Parse(GetVessel()) : 0
                    , General.GetNullableString(txtWorkordernumber.Text), General.GetNullableString(txtWorkorderName.Text), null
                    , null, General.GetNullableString(txtComponentName.Text), (Request.QueryString["od"] != null ? "19" : "1296,21,18,20")
                    , null, null, General.GetNullableDateTime(txtFromDate.Text)
                    , General.GetNullableDateTime(txtToDate.Text), null, null, null, null, null, General.GetNullableInteger(ddlPriority.SelectedValue)
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(chkDefect.Checked == true ? "1" : "")
                    , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvWorkOrder.PageSize
                    , ref iRowCount, ref iTotalPageCount);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrder_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image imgFlag = e.Item.FindControl("imgFlag") as Image;
            if (drv["FLDDUESTATUS"].ToString() != "0")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + (drv["FLDDUESTATUS"].ToString() == "1" ? "yellow-symbol.png" : (drv["FLDDUESTATUS"].ToString() == "3" ? "red-symbol.png" : "yellow-symbol.png"));
                imgFlag.ToolTip = (drv["FLDDUESTATUS"].ToString() == "1" ? "Due" : (drv["FLDDUESTATUS"].ToString() == "3" ? "Over Due" : "Due"));
            }
            else
                imgFlag.Visible = false;

            RadLabel jobid = (RadLabel)e.Item.FindControl("lblJobId");
            LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkJobID");
            if (lbtn != null) lbtn.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Owners/OwnersJobsDetails.aspx?jobid=" + jobid.Text + "');return false;");

        }
    }
    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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
    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkOrder.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
}
