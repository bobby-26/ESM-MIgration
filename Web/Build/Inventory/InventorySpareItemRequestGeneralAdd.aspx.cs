using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;
using System.Collections.Specialized;

public partial class Inventory_InventorySpareItemRequestGeneralAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);

            //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            //    toolbarmain.AddButton("Approve", "APPROVE");           

            MenuStockItemGeneral.AccessRights = this.ViewState;
            MenuStockItemGeneral.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                cmdShowItem.OnClientClick = "top.openNewWindow('spnPickItem', 'Spares','" + Session["sitepath"] + "/Common/CommonPickListSpareItemByComponent.aspx?ispopup=spnPickItem,dsd&mode=custom&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&txtnumber='+ document.getElementById('" + txtNumber.UniqueID + "').value);";
                ViewState["SPAREITEMID"] = "";
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"];
                BindFields();
            }
            //ImgShowMaker.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo', true);");
            //ImgShowMakerVendor.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo', true);");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindFields()
    {
        if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixInventorySpareItemRequest.EditSpareItemRequest(new Guid(ViewState["REQUESTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                BindFields(dt.Rows[0]);
                if (dt.Rows[0]["FLDSPAREITEMID"].ToString() != string.Empty)
                {
                    DataSet ds = PhoenixInventorySpareItem.ListSpareItem(new Guid(dt.Rows[0]["FLDSPAREITEMID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        BindFieldsActual(ds.Tables[0].Rows[0]);
                }
                ddlChangeReqType_SelectedIndexChanged(null, null);
            }
        }
    }
    private void BindFields(DataRow dr)
    {
        try
        {

            if (dr.Table.Columns.Contains("FLDREQUESTTYPE"))
                ddlChangeReqType.SelectedValue = dr["FLDREQUESTTYPE"].ToString();
            if (dr.Table.Columns.Contains("FLDREMARKS"))
                txtRemarksChange.Text = dr["FLDREMARKS"].ToString();
            txtName.Text = dr["FLDNAME"].ToString();
            txtNumber.Text = dr["FLDNUMBER"].ToString();
            //txtMakerCode.Text = dr["MAKERCODE"].ToString();
            //txtMakerName.Text = dr["MAKERNAME"].ToString();
            txtMakerId.SelectedValue = dr["FLDMAKER"].ToString();
            txtMakerId.Text = dr["MAKERNAME"].ToString();
            txtMakerReference.Text = dr["FLDMAKERREFERENCE"].ToString();
            //txtPreferredVendorCode.Text = dr["VENDORCODE"].ToString();
            //txtPreferredVendorName.Text = dr["PREFERREDVENDORNAME"].ToString();
            txtVendorId.SelectedValue = dr["VENDORCODE"].ToString();
            txtVendorId.Text = dr["PREFERREDVENDORNAME"].ToString();
            txtWantedQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDWANTED"]));
            txtStockMaximumQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDSPAREMAXIMUM"]));
            txtStockMinimumQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDSPAREMINIMUM"]));
            txtReOrderLevel.Text = string.Format(String.Format("{0:#######}", dr["FLDREORDERLEVEL"].ToString()));
            txtReOrderQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDREORDERQUANTITY"]));

            ddlStockUnit.SelectedUnit = dr["FLDUNITID"].ToString();

            //txtTotalStock.Text = string.Format(String.Format("{0:#######}", dr["TOTALSTOCKQUANTITY"]));

            if (dr["FLDISCRITICAL"].ToString() == "1")
                chkIsCritical.Checked = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFieldsActual(DataRow dr)
    {
        try
        {
            txtOName.Text = dr["FLDNAME"].ToString();
            txtONumber.Text = dr["FLDNUMBER"].ToString();
            txtOMakerCode.Text = dr["MAKERCODE"].ToString();
            txtOMakerName.Text = dr["MAKERNAME"].ToString();
            txtOMakerId.Text = dr["FLDMAKER"].ToString();
            txtOMakerReference.Text = dr["FLDMAKERREFERENCE"].ToString();
            txtOPreferredVendorCode.Text = dr["VENDORCODE"].ToString();
            txtOPreferredVendorName.Text = dr["PREFERREDVENDORNAME"].ToString();
            txtOVendorId.Text = dr["FLDPREFERREDVENDOR"].ToString();

            txtOWantedQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDWANTED"]));
            txtOStockMaximumQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDSPAREMAXIMUM"]));
            txtOStockMinimumQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDSPAREMINIMUM"]));
            txtOReOrderLevel.Text = string.Format(String.Format("{0:#######}", dr["FLDREORDERLEVEL"].ToString()));
            txtOReOrderQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDREORDERQUANTITY"]));

            ddlOStockUnit.SelectedUnit = dr["FLDUNITID"].ToString();

            if (dr["FLDISCRITICAL"].ToString() == "1")
                chkIsCritical.Checked = true;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void InventoryStockItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                string CompNo = txtNumber.TextWithLiterals;

                if (!IsValidStockItem(CompNo, txtName.Text, ddlStockUnit.SelectedUnit, ddlChangeReqType.SelectedValue,
                                      txtStockMaximumQuantity.Text, txtStockMinimumQuantity.Text, txtReOrderLevel.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                int? iscritical = null;

                if (chkIsCritical.Checked == true)
                    iscritical = 1;
                else
                    iscritical = 0;

                if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
                {
                    PhoenixInventorySpareItemRequest.UpdateSpareItemRequest(new Guid(ViewState["REQUESTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , General.GetNullableGuid(ViewState["SPAREITEMID"].ToString())
                        , byte.Parse(ddlChangeReqType.SelectedValue)
                        , CompNo
                        , txtName.Text
                        , General.GetNullableInteger(txtMakerId.SelectedValue)
                        , txtMakerReference.Text
                        , General.GetNullableInteger(txtVendorId.SelectedValue)
                        , null
                        , Convert.ToInt32(ddlStockUnit.SelectedValue)
                        , General.GetNullableDecimal(txtWantedQuantity.Text), General.GetNullableDecimal(txtStockMaximumQuantity.Text)
                        , General.GetNullableDecimal(txtStockMinimumQuantity.Text), General.GetNullableInteger(txtReOrderLevel.Text)
                        , General.GetNullableDecimal(txtReOrderQuantity.Text)
                        , iscritical
                        , txtRemarksChange.Text);


                    PhoenixInventorySpareItemRequest.SpareChangeRequesthistoryInsert(new Guid(ViewState["REQUESTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                }
                else if (ViewState["REQUESTID"] == null || ViewState["REQUESTID"].ToString() == "")
                {

                    PhoenixInventorySpareItemRequest.InsertSpareItemRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , General.GetNullableGuid(ViewState["SPAREITEMID"].ToString())
                        , byte.Parse(ddlChangeReqType.SelectedValue)
                        , CompNo
                        , txtName.Text, General.GetNullableInteger(txtMakerId.SelectedValue)
                        , txtMakerReference.Text
                        , General.GetNullableInteger(txtVendorId.SelectedValue)
                        , null
                        , Convert.ToInt32(ddlStockUnit.SelectedValue)
                        , General.GetNullableDecimal(txtWantedQuantity.Text), General.GetNullableDecimal(txtStockMaximumQuantity.Text)
                        , General.GetNullableDecimal(txtStockMinimumQuantity.Text), General.GetNullableInteger(txtReOrderLevel.Text)
                        , General.GetNullableDecimal(txtReOrderQuantity.Text)
                        , iscritical
                        , txtRemarksChange.Text);
                }
                Response.Redirect("InventorySpareItemRequestList.aspx", false);
                //String script = String.Format("javascript:fnReloadList('code1');");
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                //BindFields();
            }

            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                PhoenixInventorySpareItemRequest.ApproveSpareItemRequest(
                    new Guid(Request.QueryString["REQUESTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    );
                String script = String.Format("javascript:fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["SPAREITEMID"] = "";
                ViewState["REQUESTID"] = "";

                txtOName.Text = "";
                txtONumber.Text = "";
                txtOMakerCode.Text = "";
                txtOMakerName.Text = "";
                txtOMakerId.Text = "";
                txtOMakerReference.Text = "";
                txtOPreferredVendorCode.Text = "";
                txtOPreferredVendorName.Text = "";
                txtOVendorId.Text = "";
                txtOWantedQuantity.Text = "";
                txtOStockMaximumQuantity.Text = "";
                txtOStockMinimumQuantity.Text = "";
                txtOReOrderLevel.Text = "";
                txtOReOrderQuantity.Text = "";
                ddlOStockUnit.SelectedUnit = "";
                chkOIsCritical.Checked = false;

                txtName.Text = "";
                txtNumber.Text = "";
                //txtMakerCode.Text = "";
                //txtMakerName.Text = "";
                txtMakerId.Text = "";
                txtMakerReference.Text = "";
                //txtPreferredVendorCode.Text = "";
                //txtPreferredVendorName.Text = "";
                txtVendorId.Text = "";
                txtWantedQuantity.Text = "";
                txtStockMaximumQuantity.Text = "";
                txtStockMinimumQuantity.Text = "";
                txtReOrderLevel.Text = "";
                txtReOrderQuantity.Text = "";
                ddlStockUnit.SelectedUnit = "";
                chkIsCritical.Checked = false;

                txtRemarksChange.Text = "";
                ddlChangeReqType.SelectedIndex = -1;
            }
            if(CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("InventorySpareItemRequestList.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdNextNumber_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = PhoenixInventorySpareItem.AutoGenerateNumber(txtNumber.TextWithLiterals, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            txtNumber.Text = dt.Rows[0]["FLDNEXTNUMBER"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void ddlChangeReqType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlChangeReqType.SelectedValue == "0" || ddlChangeReqType.SelectedValue == "")
        {
            divOldValues.Visible = false;
        }
        else
        {
            divOldValues.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            txtNumber.Text = nvc.Get("lblStockItemNumber").ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnFetch_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = PhoenixInventorySpareItem.FetchSpareItem(txtNumber.TextWithLiterals, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                BindFieldsActual(dt.Rows[0]);
                BindFields(dt.Rows[0]);
            }
            else
            {
                throw new Exception("Spare Item Not found. Enter Valid Spare Item Number.");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidStockItem(string itemnumber, string itemname,
                                string stockunit, string requesttype, string maximumquantity, string mininumquantity, string reorderlevel)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (itemnumber.Trim().Equals(""))
            ucError.ErrorMessage = "Item number can not be blank.";

        if (itemname.Trim().Equals(""))
            ucError.ErrorMessage = "Item name can not be blank.";

        if (!General.GetNullableInteger(stockunit).HasValue)
        {
            ucError.ErrorMessage = "Please select item unit.";
        }
        if (!General.GetNullableInteger(requesttype).HasValue)
            ucError.ErrorMessage = "Request Type is mandatory.";

        if (maximumquantity.Trim() != "")
        {
            maximumquantity = maximumquantity != "" ? maximumquantity : "0";
            mininumquantity = mininumquantity != "" ? mininumquantity : "0";
            reorderlevel = reorderlevel != "" ? reorderlevel : "0";

            if (Convert.ToDecimal(maximumquantity.Trim()) < Convert.ToDecimal(mininumquantity.Trim()))
                ucError.ErrorMessage = "Minimum quantity should be less than maximum quantity";

            if (Convert.ToDecimal(maximumquantity.Trim()) < Convert.ToDecimal(reorderlevel.Trim()))
                ucError.ErrorMessage = "Reorder level quantity should be less than maximum quantity";
        }

        return (!ucError.IsError);
    }

}