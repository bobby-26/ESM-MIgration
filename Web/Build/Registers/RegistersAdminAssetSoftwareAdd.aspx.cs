using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class RegistersAdminAssetSoftwareAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Save", "SAVE");

            MenuAdminAssetAdd.AccessRights = this.ViewState;
            MenuAdminAssetAdd.MenuList = toolbar1.Show();


            if (Request.QueryString["Assetid"] != null)
                ViewState["AssetNo"] = Request.QueryString["Assetid"];
            else
                ViewState["AssetNo"] = String.Empty;

            if (!String.IsNullOrEmpty(ViewState["AssetNo"].ToString()))
                EditAsset(General.GetNullableGuid(ViewState["AssetNo"].ToString()));

            BindYear();
            BindAssetType();
        }
    }

    protected void BindYear()
    {
        for (int i = 2005; i <= (DateTime.Today.Year) + 1; i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        ddlLocation.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
    }

    protected void BindAssetType()
    {
        ddlAssetType.Items.Clear();
        ListItem li = new ListItem("--Select--", "");
        ddlAssetType.Items.Add(li);
        DataTable dt = PhoenixRegistersAssetType.ListAssetType(2,1); //software
        ddlAssetType.DataSource = dt;
        //ddlAssetType.SelectedValue = "FLDASSETTYPEID";
        ddlAssetType.DataBind();
    }

    private void EditAsset(Guid? Assetid)
    {
        DataTable dt = PhoenixAdministrationAsset.EditAsset(Assetid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtAssetName.Text = dr["FLDNAME"].ToString();
            Txtdescriptionadd.Text = dr["FLDDESCRIPTION"].ToString();
            TxtIdentityno.Text = dr["FLDIDENTIFICATIONNUMBER"].ToString();
            Txtserialno.Text = dr["FLDSERIALNO"].ToString();
            TxtVersion.Text = dr["FLDVERSION"].ToString();
            ucQty.Text = dr["FLDQUANTITY"].ToString();
            ddlAssetType.SelectedValue = dr["FLDASSETTYPEID"].ToString();
            TxtPoreference.Text = dr["FLDPOREFERENCE"].ToString();
            UcInvoiceDate.Text = dr["FLDINVOICEDATE"].ToString();
            TxtInvoiceno.Text = dr["FLDINVOICENO"].ToString();
            ddlYear.SelectedValue = dr["FLDBUDGETYEAR"].ToString();
            UcExpirydate.Text = dr["FLDEXPIRYDATE"].ToString();
            ucDisposalDate.Text = dr["FLDDISPOSALDATE"].ToString();
            txtDisposalReason.Text = dr["FLDDISPOSALREASON"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            ddlLocation.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
            txtAssetName.Focus();

        }
    }

    protected void MenuAdminAssetAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (string.IsNullOrEmpty(ViewState["AssetNo"].ToString()))
                {
                    if (IsValidAsset(ddlAssetType.SelectedValue, txtAssetName.Text, Txtserialno.Text))
                    {
                        InsertAsset(txtAssetName.Text, Txtdescriptionadd.Text, null, TxtIdentityno.Text,
                                    null, Txtserialno.Text, null, ucQty.Text, ddlAssetType.SelectedValue, TxtPoreference.Text,
                                    UcInvoiceDate.Text, TxtInvoiceno.Text, ddlYear.SelectedValue, null, UcExpirydate.Text, txtRemarks.Text,TxtVersion.Text);
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }

                else
                {
                    if (IsValidAsset(ddlAssetType.SelectedValue, txtAssetName.Text, Txtserialno.Text))
                    {

                        PhoenixAdministrationAsset.UpdateAsset(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                              , General.GetNullableGuid(ViewState["AssetNo"].ToString())
                                                              , txtAssetName.Text
                                                              , Txtdescriptionadd.Text
                                                              , null
                                                              , TxtIdentityno.Text
                                                              , null
                                                              , Txtserialno.Text
                                                              , null
                                                              , General.GetNullableInteger(ucQty.Text)
                                                              , General.GetNullableInteger(ddlAssetType.SelectedValue)
                                                              , TxtPoreference.Text
                                                              , General.GetNullableDateTime(UcInvoiceDate.Text)
                                                              , TxtInvoiceno.Text
                                                              , General.GetNullableInteger(ddlYear.SelectedValue)
                                                              , null
                                                              , General.GetNullableDateTime(UcExpirydate.Text)
                                                              , General.GetNullableDateTime(ucDisposalDate.Text)
                                                              , txtDisposalReason.Text
                                                              , 2
                                                              , txtRemarks.Text
                                                              , TxtVersion.Text
                                                              );
                        ucStatus.Text = "Asset Updated";
                        EditAsset(General.GetNullableGuid(ViewState["AssetNo"].ToString()));
                        if (Request.QueryString["norefresh"] == null)
                        {
                            String script = "javascript:fnReloadList('code1');";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                        }
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    private void InsertAsset(string AssetName, string Description, string identificationcapion, string identificationno, string serialcaption,
                             string serialno, string quantitycaption, string quantity, string assesttypeid, string poreference, string invoicedate, string invoiceno,
                             string budgetyear, string expirydatecaption, string expirydate, string remarks,string version)
    {
        Guid? AssetId = Guid.Empty;
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixAdministrationAsset.InsertAsset(rowusercode, AssetName, Description, null, identificationno, null,
                                                serialno, null, General.GetNullableInteger(quantity), General.GetNullableInteger(assesttypeid), poreference,
                                                General.GetNullableDateTime(invoicedate), invoiceno, General.GetNullableInteger(budgetyear), null, General.GetNullableDateTime(expirydate)
                                                , General.GetNullableDateTime(ucDisposalDate.Text), txtDisposalReason.Text, 2, remarks, version, ref AssetId); //2 -software
        ucStatus.Text = "Asset added";
        if (Request.QueryString["norefresh"] == null)
        {
            String script = "javascript:fnReloadList('code1');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }

    }
    private bool IsValidAsset(string assesttypeid, string AssetName, string serialno)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(assesttypeid))
            ucError.ErrorMessage = "Asset Type is Required";
        if (string.IsNullOrEmpty(AssetName))
            ucError.ErrorMessage = "Asset Name is Required";
        if (string.IsNullOrEmpty(serialno))
            ucError.ErrorMessage = "Media Part No is Required.";
        if (General.GetNullableDateTime(UcInvoiceDate.Text) > System.DateTime.Today)
            ucError.ErrorMessage = "Invoice Date should not be greater than today's date.";

        return (!ucError.IsError);
    }

}
