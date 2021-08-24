using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class RegistersAdminAssetAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("New", "NEW");
        toolbar1.AddButton("Save", "SAVE");
        MenuAdminAssetAdd.AccessRights = this.ViewState;
        MenuAdminAssetAdd.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["AssetId"] != null)
                        ViewState["ASSETID"] = Request.QueryString["AssetId"];
                    else
                        ViewState["ASSETID"] = String.Empty;

                    if (Request.QueryString["ZoneId"] != null)
                    {
                        ViewState["ZONEID"] = int.Parse(Request.QueryString["ZoneId"]);
                        ddlLocation.SelectedZone = ViewState["ZONEID"].ToString();
                    }
                    else
                        ViewState["ZONEID"] = String.Empty;

                    BindYear();
                    BindAssetType();
                    if (!String.IsNullOrEmpty(ViewState["ASSETID"].ToString()))
                        EditAsset(General.GetNullableGuid(ViewState["ASSETID"].ToString()));
                }
            }
            catch(Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
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
        ddlYear.Items.Insert(0, new ListItem("--Select--", ""));
    }
    protected void BindAssetType()
    {
        ddlAssetType.Items.Clear();
        DataTable dt = PhoenixRegistersAssetType.ListAssetType(1, 2);
        ddlAssetType.DataSource = dt;
        ddlAssetType.DataBind();
        ddlAssetType.Items.Insert(0, new ListItem("--Select--", ""));
    }

    private void EditAsset(Guid? Assetid)
    {
        DataTable dt = PhoenixAdministrationAsset.EditAsset(Assetid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtAssetName.Text = dr["FLDNAME"].ToString();
            txtSerialNo.Text = dr["FLDSERIALNO"].ToString();
            Txtdescriptionadd.Text = dr["FLDDESCRIPTION"].ToString();            
            txtModel.Text = dr["FLDIDENTIFICATIONNUMBER"].ToString();            
            txtMaker.Text = dr["FLDMAKER"].ToString();            
            ddlAssetType.SelectedValue = dr["FLDASSETTYPEID"].ToString();
            TxtPoreference.Text = dr["FLDPOREFERENCE"].ToString();
            UcInvoiceDate.Text = dr["FLDINVOICEDATE"].ToString();
            TxtInvoiceno.Text = dr["FLDINVOICENO"].ToString();
            ddlYear.SelectedValue = dr["FLDBUDGETYEAR"].ToString();            
            UcExpirydate.Text = dr["FLDEXPIRYDATE"].ToString();
            ucDisposalDate.Text = dr["FLDDISPOSALDATE"].ToString();
            txtDisposalReason.Text = dr["FLDDISPOSALREASON"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            txtTagNumber.Text = dr["FLDTAGNUMBER"].ToString();
            txtAssetName.Focus();
        }
    }
    private void Reset()
    {
        try
        {
            txtAssetName.Text = "";
            txtSerialNo.Text = "";
            Txtdescriptionadd.Text = "";
            txtModel.Text = "";
            txtMaker.Text = "";
            ddlAssetType.SelectedValue = null;
            TxtPoreference.Text = "";
            UcInvoiceDate.Text = "";
            TxtInvoiceno.Text = "";
            ddlYear.SelectedValue = null;
            UcExpirydate.Text = "";
            ucDisposalDate.Text = "";
            txtDisposalReason.Text = "";
            txtRemarks.Text = "";
            txtTagNumber.Text = "";
            ddlLocation.SelectedZone = "";
            ViewState["ASSETID"] = String.Empty;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAdminAssetAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidAsset(ddlAssetType.SelectedValue, txtAssetName.Text, txtMaker.Text, txtSerialNo.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(ViewState["ASSETID"].ToString()))
                {
                    DataSet ds = new DataSet();
                    Guid? AssetId = Guid.Empty;

                    ds = PhoenixAdministrationAsset.InsertAdminAsset
                    (
                        txtAssetName.Text
                        , Txtdescriptionadd.Text
                        , null
                        , txtModel.Text
                        , null
                        , txtSerialNo.Text
                        , null
                        , 1
                        , General.GetNullableInteger(ddlAssetType.SelectedValue)
                        , TxtPoreference.Text
                        , General.GetNullableDateTime(UcInvoiceDate.Text)
                        , TxtInvoiceno.Text
                        , General.GetNullableInteger(ddlYear.SelectedValue)
                        , null
                        , General.GetNullableDateTime(UcExpirydate.Text)
                        , General.GetNullableDateTime(ucDisposalDate.Text)
                        , txtDisposalReason.Text
                        , 1
                        , txtRemarks.Text
                        , General.GetNullableInteger(ddlLocation.SelectedZone)
                        , txtMaker.Text
                        , txtTagNumber.Text
                        , null
                        , ref AssetId
                     );

                    ViewState["ASSETID"] = AssetId;     //Output AssetId
                    ucStatus.Text = "Asset added";
                    
                    if (!String.IsNullOrEmpty(ViewState["ASSETID"].ToString()))
                        EditAsset(General.GetNullableGuid(ViewState["ASSETID"].ToString()));

                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                else
                {
                    if (IsValidAsset(ddlAssetType.SelectedValue, txtAssetName.Text, txtMaker.Text, txtSerialNo.Text))
                    {
                        PhoenixAdministrationAsset.UpdateAdminAsset(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                              , General.GetNullableGuid(ViewState["ASSETID"].ToString())
                                                              , txtAssetName.Text
                                                              , Txtdescriptionadd.Text
                                                              , null
                                                              , txtModel.Text
                                                              , null
                                                              , txtSerialNo.Text
                                                              , null
                                                              , 1
                                                              , General.GetNullableInteger(ddlAssetType.SelectedValue)
                                                              , TxtPoreference.Text
                                                              , General.GetNullableDateTime(UcInvoiceDate.Text)
                                                              , TxtInvoiceno.Text
                                                              , General.GetNullableInteger(ddlYear.SelectedValue)
                                                              , null
                                                              , General.GetNullableDateTime(UcExpirydate.Text)
                                                              , General.GetNullableDateTime(ucDisposalDate.Text)
                                                              , txtDisposalReason.Text
                                                              , 1
                                                              , txtMaker.Text
                                                              , txtRemarks.Text
                                                              , General.GetNullableInteger(ddlLocation.SelectedZone)
                                                              , txtTagNumber.Text
                                                              , null
                                                              );
                        
                        ucStatus.Text = "Asset Updated";

                        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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
    private bool IsValidAsset(string assesttypeid, string AssetName, string maker, string serialnumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(assesttypeid))
            ucError.ErrorMessage = "Asset is Required";
        if (string.IsNullOrEmpty(AssetName))
            ucError.ErrorMessage = "Name is Required";
        if (string.IsNullOrEmpty(maker))
            ucError.ErrorMessage = "Maker is Required.";
        if (string.IsNullOrEmpty(serialnumber))
            ucError.ErrorMessage = "Serial Number is Required.";

        return (!ucError.IsError);
    }

    public void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}
