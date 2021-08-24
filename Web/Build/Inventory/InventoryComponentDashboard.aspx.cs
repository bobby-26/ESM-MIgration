using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Dashboard;
using System.Drawing;

public partial class Inventory_InventoryComponentDashboard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (!IsPostBack)
            {
                ViewState["COMPONENTMODE"] = "";
                ViewState["COMPONENTNAME"] = "";
                BindLocation();
                BindFields();
                // btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "', true);");

                //gvComponentJob.PageSize=int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (ViewState["COMPONENTMODE"].ToString() == "1")
            {
                toolbarmain.AddButton("Restore", "RESTORE", ToolBarDirection.Right);
            }
            else
            {
                toolbarmain.AddButton("Delete", "DELETE", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
            }
            MenuComponentGeneral.AccessRights = this.ViewState;
            MenuComponentGeneral.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbarwo = new PhoenixToolbar();
            toolbarwo.AddLinkButton("javascript:showDialog();", "Create Work Order", "ADD", ToolBarDirection.Right);
            MenuWorkOrderRequestion.AccessRights = this.ViewState;
            MenuWorkOrderRequestion.MenuList = toolbarwo.Show();

            PhoenixToolbar toolWoCreate = new PhoenixToolbar();
            toolWoCreate.AddButton("Save", "WOCREATE", ToolBarDirection.Right);
            menuWorkorderCreate.AccessRights = this.ViewState;
            menuWorkorderCreate.MenuList = toolWoCreate.Show();


            btnchangerequest.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentChangeRequestList.aspx?COMPONENTID=" + Request.QueryString["COMPONENTID"].ToString() + "', true);");
            btnparts.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Inventory/InventoryComponentSpareItem.aspx?COMPONENTID="+ Request.QueryString["COMPONENTID"].ToString() + "', true);");
            ViewState["DTKEY"] = PhoenixInventoryComponent.ListComponent(new Guid(Request.QueryString["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID).Tables[0].Rows[0]["FLDDTKEY"].ToString();
            btnattachment.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=INVENTORY', true);");
            btncounter.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentCounters.aspx?COMPONENTID=" + Request.QueryString["COMPONENTID"].ToString() + "', true);");
            btntechspec.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Inventory/InventoryComponentDetail.aspx?COMPONENTID=" + Request.QueryString["COMPONENTID"].ToString() + "', true);");
            //btnWorkorderDue.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentWorkOrderList.aspx?COMPONENTID=" + Request.QueryString["COMPONENTID"].ToString() + "', true);");

            //imgShowMaker.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo', true);");
            //imgShowVendor.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo', true);");
            //imgShowParentComponent.Attributes.Add("onclick", "javascript:return showPickList('spnPickListParentComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?framename=ifMoreInfo', true);");

            btnManuals.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + Request.QueryString["COMPONENTID"].ToString() + "&JOBYN=0');");
            btnDocument.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementComponentJobFormList.aspx?COMPONENTJOBID=" + Request.QueryString["COMPONENTID"].ToString() + "');");
            btnRA.Attributes.Add("onclick", "javascript:openNewWindow('RA', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRAHistory.aspx?COMPONENTID=" + Request.QueryString["COMPONENTID"].ToString() + "');");
            btnReportingTemp.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentHistoryTemplate.aspx?COMPONENTID=" + Request.QueryString["COMPONENTID"].ToString() + "');");

            ViewState["RESP"] = string.Empty;
            ViewState["CNO"] = string.Empty;
            ViewState["WONAME"] = string.Empty;
            ViewState["CNAME"] = string.Empty;
            ViewState["FDATE"] = string.Empty;
            ViewState["TDATE"] = string.Empty;
            ViewState["MDUE"] = string.Empty;
            ViewState["JCATNAME"] = string.Empty;
            ViewState["DUE"] = string.Empty;
            ViewState["DAYS"] = string.Empty;
            ViewState["JCAT"] = Request.QueryString["jc"] == null ? string.Empty : Request.QueryString["jc"];
            ViewState["CATEGORY"] = Request.QueryString["cc"] == null ? string.Empty : Request.QueryString["cc"];
            ViewState["ISCRITICAL"] = Request.QueryString["iscr"] == null ? string.Empty : Request.QueryString["iscr"];
            ViewState["JOBNOTPLAN"] = Request.QueryString["JobNotPlan"] == null ? string.Empty : Request.QueryString["JobNotPlan"];
            ViewState["FREQUENCYTYPE"] = string.Empty;
            ViewState["FREQUENCY"] = string.Empty;
            ViewState["OVERDUE"] = string.Empty;
            ViewState["STATUS"] = string.Empty;
            if (Request.QueryString["resp"] != null)
                ViewState["RESP"] = Request.QueryString["resp"];
            if (Request.QueryString["d"] != null)
                ViewState["DUE"] = Request.QueryString["d"].Replace("D", "").Trim();
            ViewState["DAYS"] = ViewState["DUE"].ToString() == "0" ? string.Empty : ViewState["DUE"].ToString();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindLocation()
    {
        DataTable dt = PhoenixInventoryComponent.GetLocationComponents();
        ddlLocation.DataSource = dt;
        ddlLocation.DataBind();

        ddlLocation.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
            {
                DataTable dt = PhoenixInventoryComponent.EditComponent(new Guid(Request.QueryString["COMPONENTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    ViewState["COMPONENTNAME"] = dr["FLDCOMPONENTNAME"].ToString();
                    txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                    txtComponentNumber.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                    txtSerialNumber.Text = dr["FLDSERIALNUMBER"].ToString();
                    txtType.Text = dr["FLDTYPE"].ToString();
                    txtMakerId.SelectedValue = dr["FLDMAKERID"].ToString();
                    txtMakerId.Text = dr["FLDMAKERNAME"].ToString();
                    //  txtMakerName.Text = dr["MAKERNAME"].ToString();
                    txtVendorId.SelectedValue = dr["FLDVENDORID"].ToString();
                    txtVendorId.Text = dr["FLDVENDORNAME"].ToString();
                    //txtVendorCode.Text = dr["VENDORCODE"].ToString();
                    //txtLocation.Text = dr["FLDLOCATION"].ToString();

                    ddlLocation.SelectedValue = General.GetNullableGuid(dr["FLDGLOBALLOCATIONID"].ToString()) != null ? dr["FLDGLOBALLOCATIONID"].ToString() : "Dummy"; ;
                    txtParentComponentID.SelectedValue = dr["FLDPARENTID"].ToString();
                    //txtParentComponentName.Text = dr["FLDPARENTID"].ToString();                    
                    //txtParentComponentNumber.Text = dr["PARENTCOMPONENTNUMBER"].ToString();
                    txtParentComponentID.Text = dr["FLDPARENTCOMPONENTNAME"].ToString();
                    txtClassCode.Text = dr["FLDCLASSCODE"].ToString();
                    //txtBudgetCode.Text = dr["MAINBUDGETCODE"].ToString();
                    //txtBudgetName.Text = dr["MAINBUDGETDESCRIPTION"].ToString();
                    //txtBudgetId.SelectedValue = dr["FLDMAINTBUDGETID"].ToString();
                    //txtBudgetId.Text= dr["MAINBUDGETDESCRIPTION"].ToString();
                    //if (dr["FLDISCRITICAL"].ToString() == "1")
                    //    chkIsCritical.Checked = true;

                    ucCompCategory.SelectedQuick = dr["FLDCATEGORYID"].ToString();

                    btnAdditions.Attributes.Add("onclick", "javascript:return showPickList('spnAdditions', 'Additions', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceAdditionsToComponent.aspx?ComponentID=" + Request.QueryString["COMPONENTID"].ToString() + "&VesselID=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()
                        + "&Componentname=" + ViewState["COMPONENTNAME"].ToString() + "&Componentnumber=" + dr["FLDCOMPONENTNUMBER"].ToString() + "', true);");

                    if (dr["FLDOPERATIONALCRITICALYN"].ToString() == "1")
                        chkOperationalCritical.Checked = true;

                    if (dr["FLDENVIRONMENTALCRITICALYN"].ToString() == "1")
                        chkEnvironmentalCritical.Checked = true;

                    if (General.GetNullableDateTime(dr["FLDINSTALLDATE"].ToString()) != null)
                        txtinstallation.Text = dr["FLDINSTALLDATE"].ToString();
                   
                    PhoenixToolbar toolbarmain = new PhoenixToolbar();
                    ViewState["COMPONENTMODE"] = dr["FLDISDELETED"].ToString();
                    if (ViewState["COMPONENTMODE"].ToString() == "1")
                    {
                        toolbarmain.AddButton("Restore", "RESTORE", ToolBarDirection.Right);
                    }
                    else
                    {
                        toolbarmain.AddButton("Delete", "DELETE", ToolBarDirection.Right);
                        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                        toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
                    }
                    MenuComponentGeneral.AccessRights = this.ViewState;
                    MenuComponentGeneral.MenuList = toolbarmain.Show();

                    ucStatus.SelectedHard = dr["FLDCOMPONENTSTATUS"].ToString();
                    rbCompClasification.SelectedValue = dr["FLDCRITICALCATEGORY"].ToString();

                    ViewState["OPERATIONMODE"] = "EDIT";
                }
            }
            else
            {
                ViewState["OPERATIONMODE"] = "ADD";
                txtParentComponentID.SelectedValue = Request.QueryString["COMPONENTID"];
                txtParentComponentID.Text = ViewState["COMPONENTNAME"].ToString();
            }
            SetEditableControls();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetEditableControls()
    {

        txtComponentNumber.Enabled = SessionUtil.CanAccess(this.ViewState, "txtComponentNumber"); // General.IsEditableInOffice();
        txtComponentName.Enabled = SessionUtil.CanAccess(this.ViewState, "txtComponentName"); //General.IsEditableInOffice();
        txtParentComponentID.Enabled = SessionUtil.CanAccess(this.ViewState, "txtParentComponentID"); //General.IsEditableInOffice();
        //txtParentComponentName.Enabled = SessionUtil.CanAccess(this.ViewState, "txtParentComponentName"); //General.IsEditableInOffice();
        //txtParentComponentNumber.Enabled = SessionUtil.CanAccess(this.ViewState, "txtParentComponentNumber"); //General.IsEditableInOffice();
        txtSerialNumber.Enabled = SessionUtil.CanAccess(this.ViewState, "txtSerialNumber"); //General.IsEditableInOffice();
        txtMakerId.Enabled = SessionUtil.CanAccess(this.ViewState, "txtMakerId"); //General.IsEditableInOffice();
        txtVendorId.Enabled = SessionUtil.CanAccess(this.ViewState, "txtVendorId"); //General.IsEditableInOffice();
        //txtVendorCode.Enabled = SessionUtil.CanAccess(this.ViewState, "txtVendorCode"); //General.IsEditableInOffice();
        //txtVendorName.Enabled = SessionUtil.CanAccess(this.ViewState, "txtVendorName"); //General.IsEditableInOffice();
        ddlLocation.Enabled = SessionUtil.CanAccess(this.ViewState, "txtLocation"); //General.IsEditableInOffice();
        //txtMakerCode.Enabled = SessionUtil.CanAccess(this.ViewState, "txtMakerCode"); //General.IsEditableInOffice();
        //txtMakerName.Enabled = SessionUtil.CanAccess(this.ViewState, "txtMakerName"); //General.IsEditableInOffice();
        txtClassCode.Enabled = SessionUtil.CanAccess(this.ViewState, "txtClassCode"); //General.IsEditableInOffice();
        txtType.Enabled = SessionUtil.CanAccess(this.ViewState, "txtType"); //General.IsEditableInOffice();
        //chkIsCritical.Enabled = SessionUtil.CanAccess(this.ViewState, "chkIsCritical"); //General.IsEditableInOffice();
        txtMakerId.Visible = SessionUtil.CanAccess(this.ViewState, "imgShowMaker"); //General.IsEditableInOffice();
        txtParentComponentID.Visible = SessionUtil.CanAccess(this.ViewState, "imgShowParentComponent"); //General.IsEditableInOffice();
        txtVendorId.Visible = SessionUtil.CanAccess(this.ViewState, "imgShowVendor"); //General.IsEditableInOffice();
        //txtBudgetCode.Enabled = SessionUtil.CanAccess(this.ViewState, "txtBudgetCode");
        //txtBudgetName.Enabled = SessionUtil.CanAccess(this.ViewState, "txtBudgetName");
        //txtBudgetId.Enabled = SessionUtil.CanAccess(this.ViewState, "txtBudgetId");
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            txtType.Enabled = General.IsEditableInShip();
            txtSerialNumber.Enabled = General.IsEditableInShip();
        }
    }

    protected void PlannedMaintenanceComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidComponent(txtComponentNumber.TextWithLiterals.TrimEnd('.'), txtComponentName.Text, txtParentComponentID.Text, ucStatus.SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }

                int? iscritical = null;
                if (rbCompClasification.SelectedValue != "0")
                {
                    iscritical = 1;
                }
                else
                {
                    iscritical = 0;
                }

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhoenixInventoryComponent.UpdateComponent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["COMPONENTID"])
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtComponentNumber.TextWithLiterals.Replace("_", "").TrimEnd('.'), txtComponentName.Text.Trim()
                        , txtSerialNumber.Text.Trim(), txtParentComponentID.SelectedValue
                        , General.GetNullableInteger(txtVendorId.SelectedValue), General.GetNullableInteger(txtMakerId.SelectedValue)
                        , General.GetNullableGuid(ddlLocation.SelectedValue) != null ? General.GetNullableString(ddlLocation.SelectedItem.Text) : null//, General.GetNullableInteger(txtBudgetId.SelectedValue)
                        , null//General.GetNullableInteger(txtSbudgetId.Text)
                        , null
                        , txtClassCode.Text.Trim(), null, General.GetNullableInteger(string.Empty/*ddlComponentClass.SelectedQuick*/)
                        , txtType.Text.Trim(), iscritical
                        , General.GetNullableInteger(ucStatus.SelectedHard)
                        , General.GetNullableDateTime(txtinstallation.Text)
                        , chkOperationalCritical.Checked == true ? 1 : 0
                        , chkEnvironmentalCritical.Checked == true ? 1 : 0
                        , General.GetNullableInteger(rbCompClasification.SelectedValue)
                        , General.GetNullableGuid(ddlLocation.SelectedValue)
                        ,General.GetNullableInteger(ucCompCategory.SelectedValue)
                       );
                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    string CompNo = "";
                    if (txtComponentNumber.TextWithLiterals.Length >= 3)
                    {
                        CompNo = txtComponentNumber.TextWithLiterals.TrimEnd('.');
                    }
                    PhoenixInventoryComponent.InsertComponent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , CompNo, txtComponentName.Text.Trim(), txtSerialNumber.Text.Trim()
                        , txtParentComponentID.SelectedValue, General.GetNullableInteger(txtVendorId.SelectedValue)
                        , General.GetNullableInteger(txtMakerId.SelectedValue)
                        , General.GetNullableGuid(ddlLocation.SelectedValue) != null ? General.GetNullableString(ddlLocation.SelectedItem.Text) : null
                        ,null,null
                        , txtClassCode.Text.Trim()
                        , null, null
                        , txtType.Text.Trim(), iscritical
                        , General.GetNullableInteger(ucStatus.SelectedHard)
                        , General.GetNullableDateTime(txtinstallation.Text)
                        , chkOperationalCritical.Checked == true ? 1 : 0
                        , chkEnvironmentalCritical.Checked == true ? 1 : 0
                        , General.GetNullableInteger(rbCompClasification.SelectedValue)
                        , General.GetNullableGuid(ddlLocation.SelectedValue)
                        , General.GetNullableInteger(ucCompCategory.SelectedValue)
                        );
                }
                String script = String.Format("javascript:fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindFields();
            }
            if (CommandName.ToUpper().Equals("DELETE"))
            {
                ucConfirm.ErrorMessage = "1. Deleting a component will cancel the related component job and its work order. <br/>2. Component's sub-components will also get deleted and its corresponding component job and work order will get cancelled.<br/> Are you sure to delete the component ?";
                ucConfirm.Visible = true;
            }
            if (CommandName.ToUpper().Equals("RESTORE"))
            {
                PhoenixInventoryComponent.RestoreComponent(new Guid(Request.QueryString["COMPONENTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtParentComponentID.Text);
                String script = String.Format("javascript:fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindFields();
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                txtComponentNumber.Text = "";
                txtComponentName.Text = "";
                txtParentComponentID.Text = "";
                //txtParentComponentName.Text = "";
                //txtParentComponentNumber.Text = "";
                txtSerialNumber.Text = "";
                txtMakerId.Text = "";
                txtVendorId.Text = "";
                //txtVendorCode.Text = "";
                //txtVendorName.Text = "";
                ddlLocation.SelectedValue = "Dummy";
                //txtMakerCode.Text = "";
                //txtMakerName.Text = "";
                txtClassCode.Text = "";
                txtType.Text = "";
                //chkIsCritical.Checked = false;
                chkEnvironmentalCritical.Checked = false;
                chkOperationalCritical.Checked = false;
                txtinstallation.Text = "";
                ucStatus.SelectedHard = "";
                //ddlComponentClass.SelectedQuick = "";
                txtParentComponentID.SelectedValue = Request.QueryString["COMPONENTID"];
                txtParentComponentID.Text = ViewState["COMPONENTNAME"].ToString();
                ViewState["OPERATIONMODE"] = "ADD";
                gvWorkOrder.DataSource = "";
                gvWorkOrder.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidComponent(string componentnumber, string componentname, string componentparentid, string status)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (componentnumber.Replace("_", "").TrimEnd('.').Equals(""))
            ucError.ErrorMessage = "Component Number can not be blank";

        if (componentname.Trim().Equals(""))
            ucError.ErrorMessage = "Component Name can not be blank";

        if (General.GetNullableInteger(status) == null)
            ucError.ErrorMessage = "Status is required.";

        if (General.GetNullableInteger(ucCompCategory.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Category is required.";
        }

        return (!ucError.IsError);
    }

    protected void ucConfirm_ConfirmMesage(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                PhoenixInventoryComponent.DeleteComponent(new Guid(Request.QueryString["COMPONENTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                String script = String.Format("javascript:fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindFields();
                Response.Redirect("../Inventory/InventoryComponentTreeDashboard.aspx");
            }
        }
        catch (Exception ex)
        {
            ucConfirm.Visible = false;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void imgClearBudget_Click(object sender, EventArgs e)
    {
        //txtBudgetCode.Text = "";
        //txtBudgetName.Text = "";
       // txtBudgetId.Text = "";
    }

    protected void cmdMakerClear_Click(object sender, EventArgs e)
    {
        //txtMakerCode.Text = "";
        //txtMakerName.Text = "";
        txtMakerId.Text = "";
    }

    protected void cmdVendorClear_Click(object sender, EventArgs e)
    {
        //txtVendorCode.Text = "";
        //txtVendorName.Text = "";
        txtVendorId.Text = "";
    }

    protected void cmdClearParentComponent_Click(object sender, EventArgs e)
    {
        //txtParentComponentNumber.Text = "";
        //txtParentComponentName.Text = "";
        txtParentComponentID.Text = "";
    }

    //private void BindData()
    //{
    //    try
    //    {
    //        int iRowCount = 0;
    //        int iTotalPageCount = 0;

    //        string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDPRIORITY", "FLDDISCIPLINENAME", "FLDJOBNEXTDUEDATE" };
    //        string[] alCaptions = { "Job Code", "Job Title", "Frequency", "Last Done date", "Priority", "Resp Discipline", "Next Due Date" };

    //        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
    //        int? sortdirection = null;
    //        if (ViewState["SORTDIRECTION"] != null)
    //            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
    //        int cancelledjob = 0;// chkCancelledjob.Checked == true ? 1 : 0;

    //        DataSet ds;
    //        ds = PhoenixPlannedMaintenanceComponentJob.ComponentJobSearchDashboard(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(Request.QueryString["COMPONENTID"].ToString()), sortexpression, sortdirection,
    //                     int.Parse(ViewState["PAGENUMBER"].ToString()),
    //                     gvComponentJob.PageSize,
    //                     ref iRowCount,
    //                     ref iTotalPageCount,
    //                     cancelledjob);
    //        General.SetPrintOptions("gvComponentJob", "Component - Job", alCaptions, alColumns, ds);

    //        gvComponentJob.DataSource = ds;
    //        gvComponentJob.VirtualItemCount = iRowCount;

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
                

    //            if (ViewState["COMPONENTJOBID"] == null)
    //            {
    //                ViewState["COMPONENTJOBID"] = ds.Tables[0].Rows[0]["FLDCOMPONENTJOBID"].ToString();
    //                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
    //            }
    //        }
    //        //else
    //        //{
    //        //    DataTable dt = ds.Tables[0];
    //        //    gvComponentJob.DataSource = "";
    //        //}

    //        ViewState["ROWCOUNT"] = iRowCount;
    //        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvComponentJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    try
    //    {
    //        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvComponentJob.CurrentPageIndex + 1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}



    protected void btnchangerequest_Click(object sender, EventArgs e)
    {
        btnchangerequest.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo', true);");
    }

    //protected void gvComponentJob_ItemCommand(object sender, GridCommandEventArgs e)
    //{
    //    if(e.CommandName.ToUpper()== "WOHISTORY")
    //    {
           
    //    }
    //}

    //protected void gvComponentJob_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    if(e.Item is GridDataItem)
    //    {
    //        GridDataItem item = (GridDataItem)e.Item;
    //        DataRowView drv = (DataRowView)item.DataItem;
    //        LinkButton cmdWoHistory = (LinkButton)e.Item.FindControl("cmdWoHistory");
    //        RadLabel lblComponentJobId = (RadLabel)e.Item.FindControl("lblComponentJobId");
    //        RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");

    //        RadLabel lblJobID = (RadLabel)e.Item.FindControl("lblJobID");
    //        if (cmdWoHistory != null)
    //            cmdWoHistory.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID="+ lblComponentJobId .Text+ "', true);");
            
    //        LinkButton cmdRaHistory = (LinkButton)e.Item.FindControl("cmdRaHistory");
    //        if (cmdRaHistory != null)
    //            cmdRaHistory.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobRAHistory.aspx?COMPONENTJOBID=" + lblComponentJobId.Text + "', true);");

    //        LinkButton cmdPtwHistory = (LinkButton)e.Item.FindControl("cmdPtwHistory");
    //        if (cmdPtwHistory != null)
    //            cmdPtwHistory.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobPtwList.aspx?JobId=" + lblJobID.Text + "', true);");

    //        LinkButton cmdChangeReq = (LinkButton)e.Item.FindControl("cmdChangeReq");
    //        if (cmdChangeReq != null)
    //            cmdChangeReq.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobChangeRequest.aspx?JOBID=" + lblJobID.Text + "', true);");

    //        LinkButton vw = (LinkButton)e.Item.FindControl("lnkJobView");
    //        if (vw != null)
    //        {
    //            vw.Attributes.Add("onclick", "window.parent.location.href='../PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?tv=1&COMPONENTJOBID=" + lblComponentJobId.Text + "&COMPONENTID=" + lblComponentId.Text + "&hierarchy=1&Cancelledjob=0'");
    //            vw.Visible = SessionUtil.CanAccess(this.ViewState, vw.CommandName);
    //        }
    //        LinkButton lnkpostpone = (LinkButton)e.Item.FindControl("cmdReschedule");
    //        if (lnkpostpone != null && General.GetNullableGuid(drv["FLDWORKORDERID"].ToString()) != null)
    //        {
    //            lnkpostpone.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false;");
    //            lnkpostpone.Visible = SessionUtil.CanAccess(this.ViewState, lnkpostpone.CommandName);
    //        }
    //        else if (lnkpostpone != null)
    //        {
    //            lnkpostpone.Visible = false;
    //        }
    //        LinkButton lblWorkorderNumber = (LinkButton)e.Item.FindControl("lblWorkorderNumber");
    //        if (lblWorkorderNumber != null && General.GetNullableGuid(drv["FLDWORKORDERGROUPID"].ToString()) != null)
    //        {
    //            lblWorkorderNumber.Attributes.Add("onclick", "javascript:openNewWindow('WorkOrder','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWORKORDERGROUPID"].ToString() + "&WONUMBER=" + drv["FLDWORKORDERGROUPNO"].ToString() + "'); return false;");
    //            lblWorkorderNumber.Visible = SessionUtil.CanAccess(this.ViewState, lblWorkorderNumber.CommandName);
    //        }


    //    }
    //}
    protected void menuWorkorderCreate_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("WOCREATE"))
        {
            Guid? groupId = Guid.Empty;

            string csvjobList = GetSelectedJobList();
            if (csvjobList.Trim().Equals(""))
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "Select atleast one job";
                //Validator.ValidationGroup = "Group1";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
            }

            if (txtTitle.Text == string.Empty)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "Title is required";
                //Validator.ValidationGroup = "Group1";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
            }

            string PlannedDate = txtDueDate.SelectedDate.ToString();

            if (string.IsNullOrEmpty(PlannedDate))
            {
                RequiredFieldValidator validator = new RequiredFieldValidator();
                validator.ErrorMessage = "Planned date is required";
                validator.IsValid = false;
                validator.Visible = false;
                Page.Form.Controls.Add(validator);
            }
            if (General.GetNullableInteger(ddlResponsible.SelectedDiscipline) == null)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "Assigned To is required";
                //Validator.ValidationGroup = "Group1";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
            }

            if (Page.IsValid)
            {
                try
                {
                    int isUnplanned = int.Parse(rblPlannedJob.SelectedValue);
                    PhoenixPlannedMaintenanceWorkOrderGroup.GroupCreate(csvjobList, null, ref groupId, isUnplanned, txtDueDate.SelectedDate, txtTitle.Text, General.GetNullableInteger(ddlResponsible.SelectedDiscipline));
                    //PhoenixPlannedMaintenanceWorkOrderGroup.GroupDetailUpdate(groupId.Value, txtTitle.Text, txtDueDate.SelectedDate, General.GetNullableInteger(ddlResponsible.SelectedDiscipline), null, null);
                    ViewState["GROUPID"] = groupId.ToString();
                    PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(ViewState["GROUPID"].ToString()));

                    gvWorkOrder.Rebind();
                    string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                }
                catch (Exception ex)
                {
                    RequiredFieldValidator Validator = new RequiredFieldValidator();
                    Validator.ErrorMessage = "* " + ex.Message;
                    //Validator.ValidationGroup = "Group1";
                    Validator.IsValid = false;
                    Validator.Visible = false;
                    Page.Form.Controls.Add(Validator);
                }
            }
        }


    }
    private string GetSelectedJobList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvWorkOrder.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvWorkOrder.SelectedItems)
            {
                RadLabel lblworkorId = (RadLabel)gv.FindControl("lblWorkOrderId");
                strlist.Append(lblworkorId.Text + ",");
            }
        }
        return strlist.ToString();
    }
    protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixDashboardTechnical.DashboardJobCategoryCompHierarchyList(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , ViewState["JCAT"].ToString()
                    , General.GetNullableInteger(ViewState["JCATNAME"].ToString())
                    , General.GetNullableInteger(ViewState["DUE"].ToString())
                    , General.GetNullableInteger(ViewState["RESP"].ToString())
                    , General.GetNullableInteger(ViewState["CATEGORY"].ToString())
                    , txtComponentNumber.TextWithLiterals.TrimEnd('.')
                    , ViewState["WONAME"].ToString()
                    , txtComponentName.Text
                    , null
                    , null
                    , (byte?)General.GetNullableInteger(ViewState["ISCRITICAL"].ToString())
                    , sortexpression, sortdirection
                    , gvWorkOrder.CurrentPageIndex + 1
                    , gvWorkOrder.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , (byte?)General.GetNullableInteger(ViewState["OVERDUE"].ToString())
                    , (byte?)General.GetNullableInteger(ViewState["JOBNOTPLAN"].ToString())
                    , General.GetNullableInteger(ViewState["FREQUENCYTYPE"].ToString())
                    , General.GetNullableInteger(ViewState["FREQUENCY"].ToString())
                    , ViewState["STATUS"].ToString() == string.Empty ? null : ViewState["STATUS"].ToString());

            gvWorkOrder.DataSource = dt;
            gvWorkOrder.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper() == "FIND")
            {
                GridDataItem item = (GridDataItem)e.Item;
                gvWorkOrder.CurrentPageIndex = 0;
                ViewState["RESP"] = ((RadLabel)item.FindControl("lblRespId")).Text;
                ViewState["CATEGORY"] = ((RadLabel)item.FindControl("lblCategoryId")).Text;
                ViewState["CNO"] = string.Empty;
                ViewState["WONAME"] = string.Empty;
                ViewState["CNAME"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["JCATNAME"] = string.Empty;
                ViewState["ISCRITICAL"] = string.Empty;
                ViewState["DAYS"] = string.Empty;
                ViewState["OVERDUE"] = string.Empty;
                gvWorkOrder.Rebind();
            }
            else if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)e.CommandArgument;
                string value = filterPair.First.ToString();//accessing function name                
                gvWorkOrder.CurrentPageIndex = 0;
                ViewState["CNO"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue;
                ViewState["CNAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue;
                ViewState["WONAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue;
                ViewState["RESP"] = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue;
                ViewState["JCATNAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;

                string freqfilter = gvWorkOrder.MasterTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue.ToString();
                if (freqfilter != "")
                {
                    ViewState["FREQUENCY"] = freqfilter.Split('~')[0];
                    ViewState["FREQUENCYTYPE"] = freqfilter.Split('~')[1];
                }

                //ViewState["FREQUENCYTYPE"] = gvWorkOrder.MasterTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue; 
                ViewState["CATEGORY"] = string.Empty;
                string daterange = gvWorkOrder.MasterTableView.GetColumn("FLDDUEDATE").CurrentFilterValue;
                string[] dates = daterange.Split('~');
                ViewState["FDATE"] = (dates.Length > 0 ? dates[0] : string.Empty);
                ViewState["TDATE"] = (dates.Length > 1 ? dates[1] : string.Empty);
                DateTime? frmDate = General.GetNullableDateTime(ViewState["FDATE"].ToString());
                DateTime? toDate = General.GetNullableDateTime(ViewState["TDATE"].ToString());
                if (frmDate.HasValue && toDate.HasValue)
                {
                    ViewState["DAYS"] = (toDate.Value - frmDate.Value).TotalDays;
                    ViewState["DUE"] = (toDate.Value - frmDate.Value).TotalDays;

                    if (ViewState["DUE"].ToString() == "0")
                    {
                        ViewState["FDATE"] = string.Empty;
                        ViewState["TDATE"] = string.Empty;
                    }
                }
                else if (dates.Length > 1)           // only in due fitler change
                {
                    ViewState["DAYS"] = string.Empty;
                    ViewState["DUE"] = string.Empty;
                }
                ViewState["ISCRITICAL"] = gvWorkOrder.MasterTableView.GetColumn("FLDISCRITICAL").CurrentFilterValue;
                ViewState["STATUS"] = gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERSTATUS").CurrentFilterValue;
                //ViewState["OVERDUE"] = gvWorkOrder.MasterTableView.GetColumn("FLDOVERDUE").CurrentFilterValue;
                gvWorkOrder.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    private void ClearGridFilter(GridColumn column)
    {
        column.ListOfFilterValues = null;
        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
        column.CurrentFilterValue = string.Empty;
    }
    protected void ddlResponsibility_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlDiscipline = sender as RadComboBox;
        ddlDiscipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        ddlDiscipline.DataTextField = "FLDDISCIPLINENAME";
        ddlDiscipline.DataValueField = "FLDDISCIPLINEID";
        ddlDiscipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlDiscipline.SelectedValue = ViewState["RESP"].ToString();
    }

    protected void ddlJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (drv["FLDWORKORDERSTATUS"].ToString().ToUpper() == "2ND POSTPONED" || drv["FLDWORKORDERSTATUS"].ToString().ToUpper() == "POSTPONED")
            {
                //item.BackColor = System.Drawing.Color.Yellow;
                item.Attributes["style"] = "background-color: yellow !important;";
            }

            if (drv["FLDWORKORDERGROUPID"].ToString() != "")
            {
                CheckBox checkBox = (CheckBox)item["ClientSelectColumn"].Controls[0];
                checkBox.Enabled = false;
                //item.SelectableMode = GridItemSelectableMode.None;
                //item["ClientSelectColumn"].Attributes.Add( = GridItemSelectableMode.None;
            }
            LinkButton lblGroupNo = (LinkButton)e.Item.FindControl("lnkGroupNo");
            if (lblGroupNo != null)
            {
                if (drv["FLDWORKORDERGROUPID"] != null)
                {
                    lblGroupNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + "'); return false;");
                }
            }
            LinkButton cmdWoHistory = (LinkButton)e.Item.FindControl("cmdWoHistory");
            RadLabel lblComponentJobId = (RadLabel)e.Item.FindControl("lblComponentJobId");
            RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");

            RadLabel lblJobID = (RadLabel)e.Item.FindControl("lblJobID");
            if (cmdWoHistory != null)
                cmdWoHistory.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID=" + lblComponentJobId.Text + "', true);");

            LinkButton cmdRaHistory = (LinkButton)e.Item.FindControl("cmdRaHistory");
            if (cmdRaHistory != null)
                cmdRaHistory.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobRAHistory.aspx?COMPONENTJOBID=" + lblComponentJobId.Text + "', true);");

            LinkButton cmdPtwHistory = (LinkButton)e.Item.FindControl("cmdPtwHistory");
            if (cmdPtwHistory != null)
                cmdPtwHistory.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobPtwList.aspx?JobId=" + lblJobID.Text + "', true);");

            
            LinkButton cmdChangeReq = (LinkButton)e.Item.FindControl("cmdChangeReq");
            if (cmdChangeReq != null)
                cmdChangeReq.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobChangeRequest.aspx?JOBID=" + lblJobID.Text + "', true);");

            if (General.GetNullableGuid(lblJobID.Text) == null)
                cmdChangeReq.Visible = false;

                LinkButton vw = (LinkButton)e.Item.FindControl("lnkJobView");
            if (vw != null)
            {
                if (General.GetNullableGuid(lblComponentJobId.Text) != null)
                    vw.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?tv=1&COMPONENTJOBID=" + lblComponentJobId.Text + "&COMPONENTID=" + lblComponentId.Text + "&hierarchy=1&Cancelledjob=0','','1200','600');return false");
                else
                    vw.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID="+ drv["FLDWORKORDERID"].ToString() + "','','1200','600');return false");
                //vw.Visible = SessionUtil.CanAccess(this.ViewState, vw.CommandName);
            }
            LinkButton lnkpostpone = (LinkButton)e.Item.FindControl("cmdReschedule");
            if (lnkpostpone != null && General.GetNullableGuid(drv["FLDWORKORDERID"].ToString()) != null)
            {
                lnkpostpone.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false;");
                lnkpostpone.Visible = SessionUtil.CanAccess(this.ViewState, lnkpostpone.CommandName);
            }
            else if (lnkpostpone != null)
            {
                lnkpostpone.Visible = false;
            }
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");
            if (lnkTitle != null)
            {
                if (General.GetNullableGuid(lblComponentJobId.Text) != null)
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?tv=1&COMPONENTJOBID=" + lblComponentJobId.Text + "&COMPONENTID=" + lblComponentId.Text + "&hierarchy=1&Cancelledjob=0','','1200','600');return false");
                else
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "','','1200','600');return false");
            }
            
        }
    }

    protected void chkIsCritical_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        ViewState["ISCRITICAL"] = chk.Checked.HasValue && chk.Checked.Value ? "1" : string.Empty;
        gvWorkOrder.DataSource = null;
        gvWorkOrder.MasterTableView.Rebind();
    }
    protected void ChkNotPlanned_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        ViewState["JOBNOTPLAN"] = chk.Checked.HasValue && chk.Checked.Value ? "1" : string.Empty;
        gvWorkOrder.DataSource = null;
        gvWorkOrder.MasterTableView.Rebind();
    }
    protected void cblFrequencyType_DataBinding(object sender, EventArgs e)
    {
        RadComboBox frequency = sender as RadComboBox;
        frequency.DataSource = PhoenixRegistersHard.ListHard(1, 7);
        frequency.DataTextField = "FLDHARDNAME";
        frequency.DataValueField = "FLDHARDCODE";

        frequency.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        RadComboBox status = sender as RadComboBox;
        status.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 10, 0, "PPP,POP,2PP");
        status.DataTextField = "FLDHARDNAME";
        status.DataValueField = "FLDSHORTNAME";

        status.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        status.Items.Insert(1, new RadComboBoxItem("Overdue", "OVERDUE"));
        status.Items.Insert(2, new RadComboBoxItem("Due", "DUE"));
    }
}