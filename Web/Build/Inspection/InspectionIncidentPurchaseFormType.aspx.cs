using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionIncidentPurchaseFormType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuStockItemGeneral.AccessRights = this.ViewState;
        if (Filter.CurrentSelectedIncidentMenu == null)
            MenuStockItemGeneral.MenuList = toolbarmain.Show();
        try
        {
            txtStockItemID.Attributes.Add("style", "visibility:hidden");
            txtStockItemToName.Attributes.Add("style", "visibility:hidden");
            txtStockItemName.Attributes.Add("style", "visibility:hidden");
            txtStockItemToId.Attributes.Add("style", "visibility:hidden");
            txtContractorId.Attributes.Add("style", "visibility:hidden");
            txtStockClassId.Attributes.Add("style", "visibility:hidden");
            if (!IsPostBack)
            {
                if (Filter.CurrentIncidentID != null && Filter.CurrentIncidentID != "")
                    ViewState["INCIDENTID"] = Filter.CurrentIncidentID;
                else
                    ViewState["INCIDENTID"] = "";                
                rdoFormType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE));
                rdoFormType.DataBindings.DataTextField = "FLDHARDNAME";
                rdoFormType.DataBindings.DataValueField = "FLDHARDCODE";
                rdoFormType.DataBind();
                rdoFormType.SelectedValue = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "RQ");
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                if (Filter.CurrentVesselConfiguration == null || Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    if (Filter.LastVesselSelected != null)
                    {
                        NameValueCollection nvc = Filter.LastVesselSelected;
                        int vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());
                        ucVessel.SelectedVessel = vesselid.ToString();
                    }
                }
                else
                {
                    ucVessel.SelectedVessel = Filter.CurrentVesselConfiguration.ToString();
                    ucVessel.Enabled = false;
                    rdoFormType.Enabled = false;
                }
                SetVesselIdForOffice();
                btnStockClassPickList.Attributes.Add("onclick", "return showPickList('spnPickListClass', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponentClass.aspx',true);");
                btnPicstockItem.Attributes.Add("onclick", "return showPickList('spnPickStock', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListStockItem.aspx',true);");
                btnStocitemTo.Attributes.Add("onclick", "return showPickList('spnPickStockTo', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListStockItem.aspx',true);");
                btnContractPickList.Attributes.Add("onclick", "return showPickList('spnPickListContract', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo',true);");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        Filter.CurrentInspectionPurchaseStockType = ddlStockType.SelectedValue;
        Filter.CurrentInspectionPurchaseVesselSelection = (General.GetNullableInteger(ucVessel.SelectedVessel) == null) ? -1 : int.Parse(ucVessel.SelectedVessel);


        if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SPARE"))
        {
            btnPicstockItem.OnClientClick = "return showPickList('spnPickStock', 'codehelp1', '', '../Common/CommonPickListStockItem.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "', true); ";
            btnStocitemTo.OnClientClick = "return showPickList('spnPickStockTo', 'codehelp1', '', '../Common/CommonPickListStockItem.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "', true); ";
            btnStockClassPickList.OnClientClick = "return showPickList('spnPickListClass', 'codehelp1', '', '../Common/CommonPickListComponentClass.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "', true); ";
            lblClass.Text = "Component Class";
            tblRange.Visible = true;
            rblCreation.Enabled = true;
        }
        else
        {
            btnPicstockItem.OnClientClick = "return showPickList('spnPickStock', 'codehelp1', '', '../Common/CommonPickListStoreItem.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "', true); ";
            btnStocitemTo.OnClientClick = "return showPickList('spnPickStockTo', 'codehelp1', '', '../Common/CommonPickListStoreItem.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "', true);";
            //btnStockClassPickList.OnClientClick = "return showPickList('spnPickListClass', 'codehelp1', '', '../Common/CommonPickListStoreType.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "', true);";
            btnStockClassPickList.OnClientClick = "return showPickList('spnPickListClass', 'codehelp1', '', '../Common/CommonPickListType.aspx?stocktypecode=97', true);";
            lblClass.Text = "Store Type";
            tblRange.Visible = true;
            rblCreation.Enabled = true;
        }
    }

    private void SetVesselIdForOffice()
    {
        DataSet ds = new DataSet();
        if (ViewState["INCIDENTID"] != null && ViewState["INCIDENTID"].ToString() != "")
        {
            ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(ViewState["INCIDENTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                    ucVessel.Enabled = false;
                }
            }                      
        }
    }

    private void GetDocumentNumber()
    {
        DataTable dt = PhoenixInspectionAuditPurchaseForm.GetDocumentNumber();
        if (dt.Rows.Count > 0)
        {
            ViewState["DocumentNumber"] = dt.Rows[0]["FLDDOCUMENTTYPEID"].ToString();
        }
        else
        {
            ViewState["DocumentNumber"] = "0";
        }
    }


    protected void InventoryStockItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["INCIDENTID"] != null && !string.IsNullOrEmpty(ViewState["INCIDENTID"].ToString()))
                    InsertOrderForm();
                else
                {
                    ucError.Text = "Select the 'Incident' to create a requisition.";
                    ucError.Visible = true;
                }
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('code1', 'wo');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void InsertOrderForm()
    {
        GetDocumentNumber();
        if (!IsValidForm())
        {
            ucError.Visible = true;
            return;
        }

        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ucVessel", ucVessel.SelectedVessel);
        Filter.LastVesselSelected = criteria;

        DataSet ds = new DataSet();
        if (rblCreation.SelectedValue.ToUpper().Equals("MANUAL"))
        {
            PhoenixInspectionAuditPurchaseForm.InsertOrderFormManual(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                Int32.Parse(rdoFormType.SelectedValue.ToString()),
                General.GetNullableInteger(txtStockClassId.Text),
                int.Parse(ucVessel.SelectedVessel),
                Int32.Parse(ViewState["DocumentNumber"].ToString()),
                ddlStockType.SelectedValue,
                new Guid(ViewState["INCIDENTID"].ToString()));

            Session["New"] = "Y";
        }
        if (rblCreation.SelectedValue.ToUpper().Equals("AUTOMATIC"))
        {
            ds = PhoenixInspectionAuditPurchaseForm.GetWantedItemList(General.GetNullableInteger(txtStockClassId.Text), General.GetNullableInteger(txtContractorId.Text), txtStockItemFrom.Text, txtStockItemTo.Text, int.Parse(ucVessel.SelectedVessel));
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    PhoenixInspectionAuditPurchaseForm.InsertOrderFormAutomatic(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Int32.Parse(rdoFormType.SelectedValue.ToString()),
                        General.GetNullableInteger(txtStockClassId.Text),
                        General.GetNullableInteger(dr["VENDORID"].ToString()),
                        txtStockItemFrom.Text, txtStockItemTo.Text,
                        int.Parse(ucVessel.SelectedVessel),
                        Int32.Parse(ViewState["DocumentNumber"].ToString()),
                        ddlStockType.SelectedValue,
                        new Guid(ViewState["INCIDENTID"].ToString())
                        );
                    Session["New"] = "Y";
                }
            }
        }
        String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required";
        if (General.GetNullableString(ddlStockType.SelectedValue) == null)
            ucError.ErrorMessage = "Stock Type is required";
        if (ViewState["DocumentNumber"].ToString() == "0")
            ucError.ErrorMessage = "Document number is required.";
        if (rdoFormType.SelectedIndex == -1)
            ucError.ErrorMessage = "Form Type is required.";
        return (!ucError.IsError);
    }
}
