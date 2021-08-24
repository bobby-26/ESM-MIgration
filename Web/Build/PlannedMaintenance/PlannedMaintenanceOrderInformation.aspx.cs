using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceOrderInformation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);       

        if (!IsPostBack)
        {
            ViewState["type"] = string.Empty;
            ViewState["STATUS"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["status"]))
            {
                ViewState["STATUS"] = Request.QueryString["status"];
            }
            ViewState["ISREADORDERS"] = string.Empty;
            ViewState["ISOFFICEUSER"] = string.Empty;
            ViewState["FDATE"] = General.GetDateTimeToString(DateTime.Now.AddDays(-7));
            if (!string.IsNullOrEmpty(Request.QueryString["fd"]))
            {
                ViewState["FDATE"] = General.GetNullableDateTime(Request.QueryString["fd"]).Value;
            }            
            ViewState["TDATE"] = General.GetDateTimeToString(DateTime.Now);
            if (!string.IsNullOrEmpty(Request.QueryString["t"]))
            {
                ViewState["type"] = Request.QueryString["t"];
            }
            modalPopup.NavigateUrl = Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceOrderInformationAdd.aspx?type=" + ViewState["type"];
        }
    }
    protected void MenuOrderInformation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string grouprank = string.Empty;
        byte? isofficeuser = 0;

        string[] alColumns = { "FLDDATE", "FLDDETAIL", "FLDAPPLICABLETONAME", "FLDSTATUS" };
        string[] alCaptions = { "Date", "Detail", "Applicable To", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceOrderInformation.Search(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , General.GetNullableInteger(ViewState["type"].ToString())
                                         , ViewState["STATUS"].ToString()
                                         , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                         , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                                         , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , ref grouprank
                                         , ref isofficeuser
                                         , (byte?)General.GetNullableInteger(ViewState["ISREADORDERS"].ToString())
                                        );

        General.ShowExcel("Order's & Information", dt, alColumns, alCaptions, null, null);
    }
    private bool IsValidTimeSheet(string vesselstatus, DateTime? datetime, string details)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(vesselstatus) == null)
            ucError.ErrorMessage = "Vessel Status is required.";

        if (datetime == null)
            ucError.ErrorMessage = "Time is required.";

        if (details.Trim().Equals(""))
            ucError.ErrorMessage = "Details is required.";

        return (!ucError.IsError);
    }

    protected void gvOrderInformation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            bool issued = false;
            bool isackreq = false;
            bool isread = false;
            bool isofficeuser = false;
            if (drv != null)
            {
                if (drv["FLDISSUED"].ToString() == "1")
                    issued = true;
                if (drv["FLDISACKREQUIRED"].ToString() == "1")
                    isackreq = true;
                if (drv["FLDISREAD"].ToString() == "1")
                    isread = true;
            }
            if(ViewState["ISOFFICEUSER"].ToString().Equals("1"))
            {
                isofficeuser = true;
            }
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = issued || isofficeuser ? false : SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                ed.Attributes.Add("onclick", "$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceOrderInformationAdd.aspx?id=" + drv["FLDORDERINFORMATIONID"].ToString() + "&type=" + ViewState["type"] + "';showDialog('Edit'); return false;");
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = issued || isofficeuser ? false : SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }
            LinkButton cpy = (LinkButton)e.Item.FindControl("cmdCopy");
            if (cpy != null)
            {
                cpy.Visible = isofficeuser || isackreq ? false : SessionUtil.CanAccess(this.ViewState, cpy.CommandName);
                cpy.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want to copy this record?'); return false;");
            }
            LinkButton iss = (LinkButton)e.Item.FindControl("cmdIssue");
            if (iss != null)
            {
                iss.Visible = issued || isofficeuser ? false : SessionUtil.CanAccess(this.ViewState, iss.CommandName);
                iss.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want to issue this record?'); return false;");
            }
            LinkButton ack = (LinkButton)e.Item.FindControl("cmdAck");
            if (ack != null)
            {
                ack.Visible = isread || !isackreq || isofficeuser ? false : SessionUtil.CanAccess(this.ViewState, ack.CommandName);
                ack.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want to acknowledge this record?'); return false;");
            }
            LinkButton acklist = (LinkButton)e.Item.FindControl("cmdAckList");
            if (acklist != null)
            {
                acklist.Visible = isackreq || !issued || isofficeuser ? false : SessionUtil.CanAccess(this.ViewState, acklist.CommandName);
                acklist.Attributes.Add("onclick", "javascript: top.openNewWindow('AcknowledgeList','Read & Acknowledge List','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceOrderInformationAcknowledgeList.aspx?o=" + drv["FLDORDERINFORMATIONID"].ToString() + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOrderInformation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            RadGrid grid = (RadGrid)sender;
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string grouprank = string.Empty;
            byte? isofficeuser = 0;

            string[] alColumns = { "FLDDATE", "FLDDETAIL", "FLDAPPLICABLETONAME", "FLDSTATUS" };
            string[] alCaptions = { "Date", "Detail", "Applicable To", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceOrderInformation.Search(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , General.GetNullableInteger(ViewState["type"].ToString())
                                         , ViewState["STATUS"].ToString()
                                         , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                         , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                                         , sortexpression, sortdirection
                                         , grid.CurrentPageIndex + 1
                                         , grid.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , ref grouprank
                                         , ref isofficeuser
                                         , (byte?)General.GetNullableInteger(ViewState["ISREADORDERS"].ToString())
                                         );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvOrderInformation", "Orders & Information", alCaptions, alColumns, ds);

            grid.DataSource = dt;
            grid.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceOrderInformation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOrderInformation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            if (
                (grouprank.ToUpper().Equals("MST") && ViewState["type"].ToString().Equals("1"))
                || (grouprank.ToUpper().Equals("CE") && ViewState["type"].ToString().Equals("2"))
                || (grouprank.ToUpper().Equals("CO") && ViewState["type"].ToString().Equals("3"))
                )
            {
                toolbar.AddFontAwesomeButton("javascript:showDialog('Add');", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            }
            ViewState["ISOFFICEUSER"] = isofficeuser;
            ViewState["GROUP"] = grouprank;

            MenuOrderInformation.AccessRights = this.ViewState;
            MenuOrderInformation.MenuList = toolbar.Show();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvOrderInformation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            //if (e.CommandName.ToUpper() == "EDITR")
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    string id = item.GetDataKeyValue("FLDORDERINFORMATIONID").ToString();
            //    string script = "$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceOrderInformationAdd.aspx?id=" + id + "';showDialog('Edit');";
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            //}
            //else
            if (e.CommandName.ToUpper() == "COPY")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDORDERINFORMATIONID").ToString();
                PhoenixPlannedMaintenanceOrderInformation.Copy(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                grid.Rebind();
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDORDERINFORMATIONID").ToString();
                PhoenixPlannedMaintenanceOrderInformation.Delete(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                grid.Rebind();
            }
            else if (e.CommandName.ToUpper() == "ISSUE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDORDERINFORMATIONID").ToString();
                PhoenixPlannedMaintenanceOrderInformation.Issue(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                grid.Rebind();
            }
            else if (e.CommandName.ToUpper() == "ACK")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDORDERINFORMATIONID").ToString();
                PhoenixPlannedMaintenanceOrderInformation.Acknowledge(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                grid.Rebind();
            }
            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                ViewState["STATUS"] = grid.MasterTableView.GetColumn("FLDSTATUSID").CurrentFilterValue;
                Pair filterPair = (Pair)e.CommandArgument;
                if (filterPair.First.ToString() == "Custom")
                {
                    ViewState["ISREADORDERS"] = "1";
                }
                else if (filterPair.First.ToString() == "Contains")
                {
                    ViewState["ISREADORDERS"] = string.Empty;
                }
                string daterange = grid.MasterTableView.GetColumn("FLDDATE").CurrentFilterValue;
                string[] dates = daterange.Split('~');
                if (dates.Length > 0 && dates[0] != string.Empty)
                    ViewState["FDATE"] = dates[0];
                if (dates.Length > 1 && dates[1] != string.Empty)
                    ViewState["TDATE"] = dates[1];

                grid.MasterTableView.GetColumn("FLDSTATUSID").CurrentFilterValue = string.Empty;
 
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        RadComboBox status = sender as RadComboBox;
        status.DataSource = PhoenixRegistersHardExtn.ListHardExtn(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 262);
        status.DataTextField = "FLDHARDNAME";
        status.DataValueField = "FLDHARDCODE";
        //status.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    protected void chkReadOrders_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        ViewState["STATUS"] = string.Empty;
        ViewState["ISREADORDERS"] = string.Empty;
        if (chk.Checked.HasValue && chk.Checked.Value)
        {
            ViewState["ISREADORDERS"] = "1";
            ViewState["STATUS"] = "1,2,3,4";
        }        
        gvOrderInformation.Rebind();
    }

    protected void gvOrderInformation_PreRender(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        foreach (GridFilteringItem filterItem in grid.MasterTableView.GetItems(GridItemType.FilteringItem))
        {
            string type = ViewState["type"].ToString();
            bool ismaster = (ViewState["GROUP"].ToString().ToUpper().Equals("MST") ? true : false);
            bool isce = (ViewState["GROUP"].ToString().ToUpper().Equals("CE") ? true : false);
            bool isco = (ViewState["GROUP"].ToString().ToUpper().Equals("CO") ? true : false);
            RadComboBox combo = (RadComboBox)filterItem.FindControl("ddlStatus");           
            RadCheckBox chk = (RadCheckBox)filterItem.FindControl("chkReadOrders");
            if (combo != null && chk != null)
            {
                if (ismaster)
                {
                    chk.Visible = false;
                }
                if ((isce || isco) && type == "1")
                {
                    chk.Visible = true;
                    combo.Visible = false;
                }
                if ((isce || isco) && type != "1")
                {
                    chk.Visible = false;
                }
                if (!ismaster && !isce && !isco)
                {
                    combo.Visible = false;
                }
            }
        }
    }

    protected void CmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvOrderInformation.Rebind();
    }
}