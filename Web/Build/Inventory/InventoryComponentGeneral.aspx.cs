using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryComponentGeneral : PhoenixBasePage
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
                BindFields();
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "', true);");
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

            imgShowMaker.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo', true);");
            imgShowVendor.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo', true);");
            imgShowParentComponent.Attributes.Add("onclick", "javascript:return showPickList('spnPickListParentComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?framename=ifMoreInfo', true);");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
            {
                DataSet ds = PhoenixInventoryComponent.ListComponent(new Guid(Request.QueryString["COMPONENTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];

                txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                txtComponentNumber.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                txtSerialNumber.Text = dr["FLDSERIALNUMBER"].ToString();
                txtType.Text = dr["FLDTYPE"].ToString();
                txtMakerId.Text = dr["FLDMAKERID"].ToString();
                txtMakerCode.Text = dr["MAKERCODE"].ToString();
                txtMakerName.Text = dr["MAKERNAME"].ToString();
                txtVendorId.Text = dr["FLDVENDORID"].ToString();
                txtVendorName.Text = dr["VENDORNAME"].ToString();
                txtVendorCode.Text = dr["VENDORCODE"].ToString();
                txtLocation.Text = dr["FLDLOCATION"].ToString();
                txtParentComponentID.Text = dr["FLDPARENTID"].ToString();
                txtParentComponentName.Text = dr["FLDPARENTID"].ToString();
                txtParentComponentID.Text = dr["PARENTCOMPONENTID"].ToString();
                txtParentComponentNumber.Text = dr["PARENTCOMPONENTNUMBER"].ToString();
                txtParentComponentName.Text = dr["PARENTCOMPONENTNAME"].ToString();
                txtClassCode.Text = dr["FLDCLASSCODE"].ToString();
                txtBudgetCode.Text = dr["MAINBUDGETCODE"].ToString();
                txtBudgetName.Text = dr["MAINBUDGETDESCRIPTION"].ToString();
                txtBudgetId.Text = dr["FLDMAINTBUDGETID"].ToString();
                //if (dr["FLDISCRITICAL"].ToString() == "1")
                //    chkIsCritical.Checked = true;

                if (dr["FLDOPERATIONALCRITICALYN"].ToString() == "1")
                    chkOperationalCritical.Checked = true;

                if (dr["FLDENVIRONMENTALCRITICALYN"].ToString() == "1")
                    chkEnvironmentalCritical.Checked = true;

                if (General.GetNullableDateTime(dr["FLDINSTALLDATE"].ToString()) != null)
                    txtinstallation.Text = dr["FLDINSTALLDATE"].ToString();

                SessionUtil.PageAccessRights(this.ViewState);
                
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
            else
            {
                ViewState["OPERATIONMODE"] = "ADD";
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
        txtParentComponentName.Enabled = SessionUtil.CanAccess(this.ViewState, "txtParentComponentName"); //General.IsEditableInOffice();
        txtParentComponentNumber.Enabled = SessionUtil.CanAccess(this.ViewState, "txtParentComponentNumber"); //General.IsEditableInOffice();
        txtSerialNumber.Enabled = SessionUtil.CanAccess(this.ViewState, "txtSerialNumber"); //General.IsEditableInOffice();
        txtMakerId.Enabled = SessionUtil.CanAccess(this.ViewState, "txtMakerId"); //General.IsEditableInOffice();
        txtVendorId.Enabled = SessionUtil.CanAccess(this.ViewState, "txtVendorId"); //General.IsEditableInOffice();
        txtVendorCode.Enabled = SessionUtil.CanAccess(this.ViewState, "txtVendorCode"); //General.IsEditableInOffice();
        txtVendorName.Enabled = SessionUtil.CanAccess(this.ViewState, "txtVendorName"); //General.IsEditableInOffice();
        txtLocation.Enabled = SessionUtil.CanAccess(this.ViewState, "txtLocation"); //General.IsEditableInOffice();
        txtMakerCode.Enabled = SessionUtil.CanAccess(this.ViewState, "txtMakerCode"); //General.IsEditableInOffice();
        txtMakerName.Enabled = SessionUtil.CanAccess(this.ViewState, "txtMakerName"); //General.IsEditableInOffice();
        txtClassCode.Enabled = SessionUtil.CanAccess(this.ViewState, "txtClassCode"); //General.IsEditableInOffice();
        txtType.Enabled         = SessionUtil.CanAccess(this.ViewState, "txtType"); //General.IsEditableInOffice();
        //chkIsCritical.Enabled = SessionUtil.CanAccess(this.ViewState, "chkIsCritical"); //General.IsEditableInOffice();
        imgShowMaker.Visible = SessionUtil.CanAccess(this.ViewState, "imgShowMaker"); //General.IsEditableInOffice();
        imgShowParentComponent.Visible = SessionUtil.CanAccess(this.ViewState, "imgShowParentComponent"); //General.IsEditableInOffice();
        imgShowVendor.Visible = SessionUtil.CanAccess(this.ViewState, "imgShowVendor"); //General.IsEditableInOffice();
        txtBudgetCode.Enabled = SessionUtil.CanAccess(this.ViewState, "txtBudgetCode");
        txtBudgetName.Enabled = SessionUtil.CanAccess(this.ViewState, "txtBudgetName");
        txtBudgetId.Enabled = SessionUtil.CanAccess(this.ViewState, "txtBudgetId");
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
                if (!IsValidComponent(txtComponentNumber.TextWithLiterals, txtComponentName.Text, txtParentComponentID.Text, ucStatus.SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }

                int? iscritical = null;

                if (rbCompClasification.SelectedValue !=  "0")
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
                        , txtSerialNumber.Text.Trim(), txtParentComponentID.Text
                        , General.GetNullableInteger(txtVendorId.Text), General.GetNullableInteger(txtMakerId.Text)
                        , General.GetNullableString(txtLocation.Text.Trim()), General.GetNullableInteger(txtBudgetId.Text)
                        , null//General.GetNullableInteger(txtSbudgetId.Text)
                        , txtClassCode.Text.Trim(), null, General.GetNullableInteger(string.Empty/*ddlComponentClass.SelectedQuick*/)
                        , txtType.Text.Trim(), iscritical
                        , General.GetNullableInteger(ucStatus.SelectedHard)
                        , General.GetNullableDateTime(txtinstallation.Text)
                        , chkOperationalCritical.Checked == true ? 1 : 0
                        , chkEnvironmentalCritical.Checked == true ? 1 : 0
                        , General.GetNullableInteger(rbCompClasification.SelectedValue));
                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    string CompNo = "";
                    if (txtComponentNumber.TextWithLiterals.Length >= 3)
                    {
                        CompNo = txtComponentNumber.TextWithLiterals;
                    }
                    PhoenixInventoryComponent.InsertComponent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , CompNo, txtComponentName.Text.Trim(), txtSerialNumber.Text.Trim()
                        , txtParentComponentID.Text, General.GetNullableInteger(txtVendorId.Text)
                        , General.GetNullableInteger(txtMakerId.Text), General.GetNullableString(txtLocation.Text.Trim())
                        , General.GetNullableInteger(txtBudgetId.Text)
                        , null//General.GetNullableInteger(txtSbudgetId.Text)
                        , txtClassCode.Text.Trim(), null, General.GetNullableInteger(string.Empty/*ddlComponentClass.SelectedQuick*/)
                        , txtType.Text.Trim(), iscritical
                        , General.GetNullableInteger(ucStatus.SelectedHard)
                        , General.GetNullableDateTime(txtinstallation.Text)
                        , chkOperationalCritical.Checked == true ? 1 : 0
                        , chkEnvironmentalCritical.Checked == true ? 1 : 0
                        , General.GetNullableInteger(rbCompClasification.SelectedValue));
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
                PhoenixInventoryComponent.RestoreComponent(new Guid(Request.QueryString["COMPONENTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID,txtParentComponentID.Text);
                String script = String.Format("javascript:fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindFields();
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                txtComponentNumber.Text = "";
                txtComponentName.Text = "";
                txtParentComponentID.Text = "";
                txtParentComponentName.Text = "";
                txtParentComponentNumber.Text = "";
                txtSerialNumber.Text = "";
                txtMakerId.Text = "";
                txtVendorId.Text = "";
                txtVendorCode.Text = "";
                txtVendorName.Text = "";
                txtLocation.Text = "";
                txtMakerCode.Text = "";
                txtMakerName.Text = "";
                txtClassCode.Text = "";
                txtType.Text = "";
                //chkIsCritical.Checked = false;
                chkEnvironmentalCritical.Checked = false;
                chkOperationalCritical.Checked = false;
                txtinstallation.Text = "";
                ucStatus.SelectedHard = "";
                //ddlComponentClass.SelectedQuick = "";

                ViewState["OPERATIONMODE"] = "ADD";
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
        txtBudgetCode.Text = "";
        txtBudgetName.Text = "";
        txtBudgetId.Text = "";
    }

    protected void cmdMakerClear_Click(object sender, EventArgs e)
    {
        txtMakerCode.Text = "";
        txtMakerName.Text = "";
        txtMakerId.Text = "";
    }

    protected void cmdVendorClear_Click(object sender, EventArgs e)
    {
        txtVendorCode.Text = "";
        txtVendorName.Text = "";
        txtVendorId.Text = "";
    }

    protected void cmdClearParentComponent_Click(object sender, EventArgs e)
    {
        txtParentComponentNumber.Text = "";
        txtParentComponentName.Text = "";
        txtParentComponentID.Text = "";
    }
}
