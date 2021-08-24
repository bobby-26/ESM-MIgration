using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryComponent : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentTreeDashboard.aspx", "Hierarchy View", "<i class=\"fas fa-sitemap\"></i>", "VIEW");
            MenuRegistersComponent.AccessRights = this.ViewState;
            MenuRegistersComponent.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("Parts", "PART");
                toolbarmain.AddButton("History Template", "HISTORYTEMPLATE");
                toolbarmain.AddButton("Details", "DETAILS");
                toolbarmain.AddButton("Jobs", "JOB");
                toolbarmain.AddButton("Attachment", "ATTACHMENT");
                toolbarmain.AddButton("Counter", "COUNTER");
                toolbarmain.AddButton("Work Order", "WORKORDER");
                toolbarmain.AddButton("Work Done History", "WORKDONEHISTORY");
                toolbarmain.AddButton("RA History", "RAHISTORY");
                toolbarmain.AddButton("Manuals", "MANUAL");
                MenuComponent.AccessRights = this.ViewState;
                MenuComponent.MenuList = toolbarmain.Show();

                ViewState["ISTREENODECLICK"] = false;
                ViewState["PAGENUMBER"] = Request.QueryString["p"] != null ? int.Parse(Request.QueryString["p"]) : 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["COMPONENTID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["COMPONENTID"] != null && Request.QueryString["COMPONENTID"] != "")
                {
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                    ViewState["DTKEY"] = PhoenixInventoryComponent.ListComponent(new Guid(ViewState["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID).Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventoryComponentGeneral.aspx?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTID=" + Request.QueryString["COMPONENTID"].ToString() + "&p=" + ViewState["PAGENUMBER"];
                }
                if (Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString();
                }
                if (Request.QueryString["FromRA"] != null && Request.QueryString["FromRA"] == "1")
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx";
                }

                MenuComponent.SelectedMenuIndex = 0;
                gvComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            PhoenixInventorySpareItem objinventorystockitem = new PhoenixInventorySpareItem();

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../Inventory/InventoryComponentFilter.aspx");
            }
            else if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentGeneral.aspx";
            }
            else if (CommandName.ToUpper().Equals("HISTORYTEMPLATE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentHistoryTemplate.aspx";
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentDetail.aspx";
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }
            else if (CommandName.ToUpper().Equals("PART"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentSpareItem.aspx";
            }
            else if (CommandName.ToUpper().Equals("JOB"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobList.aspx";
            }
            else if (CommandName.ToUpper().Equals("COUNTER"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentCounters.aspx";

            }
            else if (CommandName.ToUpper().Equals("WORKORDER"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentWorkOrderList.aspx";
            }
            else if (CommandName.ToUpper().Equals("WORKDONEHISTORY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx";
            }
            else if (CommandName.ToUpper().Equals("RAHISTORY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRAHistory.aspx";
            }
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
            }
            else if (CommandName.ToUpper().Equals("MANUAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx";
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + ViewState["COMPONENTID"].ToString() + "&JOBYN=0";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTID=" + ViewState["COMPONENTID"] + "&p=" + ViewState["PAGENUMBER"];
            }
            SetTabHighlight();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDSERIALNUMBER", "FLDCOMPONENTSTATUSNAME", "FLDTYPE", "FLDCLASSCODE" };
            string[] alCaptions = { "Number", "Name", "Serial Number", "Status", "Type", "Class Code" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;
            NameValueCollection nvc = Filter.CurrentComponentFilterCriteria;
            int? status = null;
            if (Filter.CurrentComponentFilterCriteria == null)
                status = 35;

            ds = PhoenixCommonInventory.ComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                 , nvc != null ? nvc.Get("txtNumber").ToString() : null
                 , nvc != null ? nvc.Get("txtName").ToString() : null
                 , null
                 , General.GetNullableInteger(nvc != null ? nvc.Get("txtVendorId").ToString() : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc.Get("txtMakerid").ToString() : string.Empty)
                 , status
                 , General.GetNullableInteger(nvc != null ? nvc.Get("txtComponentClassId").ToString() : string.Empty)
                 , (byte?)General.GetNullableInteger(nvc != null ? nvc.Get("chkCrtitcal").ToString() : string.Empty)
                 , nvc != null ? nvc.Get("txtClassCode").ToString() : null
                 , nvc != null ? nvc.Get("txtType").ToString() : null
                 , General.GetNullableInteger(nvc != null ? nvc["ucComponentCategory"] : null)
                 , sortexpression, sortdirection
                 , gvComponent.CurrentPageIndex + 1
                 , gvComponent.PageSize
                 , ref iRowCount
                 , ref iTotalPageCount);

            General.ShowExcel("Component", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRegistersComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("VIEW"))
            {
                Response.Redirect("../Inventory/InventoryComponentTree.aspx?componentid=" + ViewState["COMPONENTID"].ToString());
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDSERIALNUMBER", "FLDCOMPONENTSTATUSNAME", "FLDTYPE", "FLDCLASSCODE" };
            string[] alCaptions = { "Number", "Name", "Serial Number", "Status", "Type", "Class Code" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = new DataSet();
            NameValueCollection nvc = Filter.CurrentComponentFilterCriteria;

            int? status = null;
            //if (Filter.CurrentComponentFilterCriteria == null)
            //    status = 35;

            if (Request.QueryString["tv"] == null)
            {
                ds = PhoenixCommonInventory.ComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , nvc != null ? nvc.Get("txtNumber").ToString() : null
                    , nvc != null ? nvc.Get("txtName").ToString() : null
                    , null
                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtVendorId").ToString() : string.Empty)
                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtMakerid").ToString() : string.Empty)
                    , status
                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtComponentClassId").ToString() : string.Empty)
                    , (byte?)General.GetNullableInteger(nvc != null ? nvc.Get("chkCrtitcal").ToString() : string.Empty)
                    , nvc != null ? nvc.Get("txtClassCode").ToString() : null
                    , nvc != null ? nvc.Get("txtType").ToString() : null
                    , General.GetNullableInteger(nvc != null ? nvc["ucComponentCategory"] : null)
                    , sortexpression, sortdirection,
                       gvComponent.CurrentPageIndex + 1,
                        gvComponent.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);


                General.SetPrintOptions("gvComponent", "Component", alCaptions, alColumns, ds);

                gvComponent.DataSource = ds;
                gvComponent.VirtualItemCount = iRowCount;

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    DataTable dt = ds.Tables[0];
                    gvComponent.DataSource = "";
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventoryComponentGeneral.aspx";
                    SetRowSelection();
                }

                ViewState["ROWCOUNT"] = iRowCount;
                ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString() != string.Empty)
            {
                ViewState["COMPONENTID"] = ds.Tables[0].Rows[0][0].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            }

            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentGeneral.aspx";
            }

            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobManual.aspx"))
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + ViewState["COMPONENTID"].ToString() + "&JOBYN=0";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTID=" + ViewState["COMPONENTID"] + "&p=" + ViewState["PAGENUMBER"];
            }
            SetTabHighlight();
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
            BindData();
            gvComponent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentGeneral.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentSpareItem.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 1;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentHistoryTemplate.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 2;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentDetail.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 3;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobList.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 4;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 5;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentCounters.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 6;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentWorkOrderList.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 7;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderDone.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 8;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobManual.aspx"))
            {
                MenuComponent.SelectedMenuIndex = 10;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                    Image img = (Image)e.Item.FindControl("imgFlag");
                    string IScritical = ((RadLabel)e.Item.FindControl("lblcritical")).Text;
                    if (drv["FLDISCRITICAL"].ToString() == "1")
                    {
                        img.Visible = true;
                        img.ImageUrl = Session["images"] + "/red-symbol.png";
                        img.ToolTip = "Critical Component";
                    }
                }
                LinkButton sb = (LinkButton)e.Item.FindControl("cmdCriticalItem");
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
                    sb.Visible = false;
            }


            RadLabel lblRID = ((RadLabel)e.Item.FindControl("lblNumber"));

            LinkButton atbtn = (LinkButton)e.Item.FindControl("cmdCriticalItem");
            if (atbtn != null)
            {
                atbtn.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentSearchAcrossVessel.aspx?ItemNo=" + lblRID.Text + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("COMPONENTSEARCH"))
        {
            RadLabel lblRID = ((RadLabel)e.Item.FindControl("lblNumber"));

            LinkButton atbtn = (LinkButton)e.Item.FindControl("cmdCriticalItem");
            if (atbtn != null)
            {
                atbtn.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','TEST','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentSearchAcrossVessel.aspx?ItemNo=" + lblRID.Text + "'); return false;");
            }
        }

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            ViewState["COMPONENTID"] = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
            ucConfirm.ErrorMessage = "1. Deleting a component will cancel the related component job and its work order. <br/>2. Component's sub-components will also get deleted and its corresponding component job and work order will get cancelled.<br/> Are you sure to delete the component ?";
            ucConfirm.Visible = true;
        }

        if (e.CommandName.Equals("RowClick"))
        {
            ViewState["COMPONENTID"] = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
            ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
            ViewState["PAGENUMBER"] = gvComponent.CurrentPageIndex;
            BindUrl();
            SetTabHighlight();
        }
    }

    protected void ucConfirm_ConfirmMesage(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                PhoenixInventoryComponent.DeleteComponent(new Guid(ViewState["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ucConfirm.Visible = false;
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                ViewState["COMPONENTID"] = null;
                BindData();
                gvComponent.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucConfirm.Visible = false;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvComponent.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvComponent.Items)
        {
            if (item.GetDataKeyValue("FLDCOMPONENTID").ToString() == ViewState["COMPONENTID"].ToString())
            {
                gvComponent.SelectedIndexes.Add(item.ItemIndex);
                ViewState["DTKEY"] = ((RadLabel)item.FindControl("lbldtkey")).Text;
            }
        }
    }
    private void BindUrl()
    {
        if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentGeneral.aspx";
        }

        if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
        {
            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobManual.aspx"))
        {
            ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + ViewState["COMPONENTID"].ToString() + "&JOBYN=0";
        }
        else
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTID=" + ViewState["COMPONENTID"] + "&p=" + ViewState["PAGENUMBER"];
        }
    }
}
