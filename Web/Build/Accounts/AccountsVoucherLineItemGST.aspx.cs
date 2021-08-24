using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Accounts_AccountsVoucherLineItemGST : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                if (Request.QueryString["qLineItemId"] != null && Request.QueryString["qLineItemId"] != string.Empty)
                {
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    toolbar.AddButton("Update Ledger", "UPDATELEDGER", ToolBarDirection.Right);
                    ViewState["LineItemId"] = Request.QueryString["qLineItemId"];
                    Reset();
                    EditGST(General.GetNullableGuid(ViewState["LineItemId"].ToString()));
                    ViewState["GSTDETAILID"] = txtgstlienid.Text;
                    BindHeader();
                }
                else
                {
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    toolbar.AddButton("Update Ledger", "UPDATELEDGER", ToolBarDirection.Right);
                }
                //toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuOrderFormMain.AccessRights = this.ViewState;
                MenuOrderFormMain.Title = "Row Number (" + Request.QueryString["qRowno"] + ")";
                MenuOrderFormMain.MenuList = toolbar.Show();
                //  Reset();

                BindGSTDetails();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindHeader()
    {
        DataSet ds = new DataSet();

        ds = PhoenixAccountsGST.AllocatedVoucherLineItemEdit(General.GetNullableGuid(ViewState["LineItemId"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtVoucherRow.Text = dr["FLDVOUCHERNUMBER"].ToString() + "-" + dr["FLDVOUCHERLINEITEMNO"].ToString();
            txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString() + "/" + dr["FLDAMOUNT"].ToString();
            txtAmount.Text = dr["FLDBALANCE"].ToString();

            txtVendorCode.Text = dr["FLDCODE"].ToString();
            txtVenderName.Text = dr["FLDNAME"].ToString();
            txtVendorId.Text = dr["FLDADDRESSCODE"].ToString();
            txtCompanyState.Text = dr["FLDSTATE"].ToString();
            txtvendorinvoiceno.Text = dr["FLDINVOICENUMBER"].ToString();
            txtVoucherDate.Text = dr["FLDINVOICEDATE"].ToString();
        }
    }
    private void EditGST(Guid? voucherlineitemid)
    {
        try
        {
            DataTable dt = PhoenixAccountsGST.GSTEdit(voucherlineitemid);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ddlplaceofbusiness.SelectedText = dr["FLDBUSINESSSTATE"].ToString();
                txtvendorinvoiceno.Text = dr["FLDVENDORINVOICENO"].ToString();
                txtVoucherDate.Text = dr["FLDVENDORINVOICEDATE"].ToString();
                txtigstamount.Text = dr["FLDIGSTVALUE"].ToString();
                txtigstpercentage.Text = dr["FLDIGSTPERCENTAGE"].ToString();
                txtcgstamount.Text = dr["FLDCGSTVALUE"].ToString();
                txtcgstpercentage.Text = dr["FLDCGSTPERCENTAGE"].ToString();
                txtsgstamount.Text = dr["FLDSGSTVALUE"].ToString();
                txtsgstpercentage.Text = dr["FLDSGSTPERCENTAGE"].ToString();
                txtugstamount.Text = dr["FLDUGSTVALUE"].ToString();
                txtugstpercentage.Text = dr["FLDUGSTPERCENTAGE"].ToString();
                txttaxamount.Text = dr["FLDTAXABLEVALUE"].ToString();
                txtremark.Text = dr["FLDGSTDETAILREMARKS"].ToString();
                txtgstlienid.Text = dr["FLDGSTDETAILID"].ToString();
                lblgsttype.Text = dr["GSTAPPLICABLE"].ToString();
                lblsupplierplace.Text = dr["BUSINESSSTATE"].ToString();

                // txtCompanyState.Text = "464";

                if (lblgsttype.Text == "2" || lblgsttype.Text == "3")
                {
                    txtcgstamount.Enabled = false;
                    txtsgstamount.Enabled = false;
                    txtugstamount.Enabled = false;
                }
                if (txtCompanyState.Text == (General.GetNullableInteger(ddlplaceofbusiness.SelectedValue).ToString()))
                {
                    if (lblgsttype.Text == "0")
                    {
                        txtigstamount.Enabled = false;
                        txtugstamount.Enabled = false;
                    }
                    else
                    {
                        txtigstamount.Enabled = false;
                        txtsgstamount.Enabled = false;
                        txtcgstamount.Enabled = false;
                    }

                }
                else
                {
                    txtcgstamount.Enabled = false;
                    txtsgstamount.Enabled = false;
                    txtugstamount.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidLineItem())
                {
                    string errormessage = "";
                    errormessage = ucError.ErrorMessage;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);

                    return;
                }
                try
                {
                    if (ViewState["GSTDETAILID"].ToString() == "")
                    {
                        PhoenixAccountsGST.GSTDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                  , General.GetNullableGuid(ViewState["LineItemId"].ToString())
                  , txtremark.Text
                  , decimal.Parse(txtigstpercentage.Text)
                  , decimal.Parse(txtigstamount.Text)
                  , decimal.Parse(txtcgstpercentage.Text)
                  , decimal.Parse(txtcgstamount.Text)
                  , decimal.Parse(txtsgstpercentage.Text)
                  , decimal.Parse(txtsgstamount.Text)
                  , decimal.Parse(txtugstpercentage.Text)
                  , decimal.Parse(txtugstamount.Text)
                  , decimal.Parse(txttaxamount.Text)
                  , txtvendorinvoiceno.Text
                  , General.GetNullableDateTime(txtVoucherDate.Text)
                  , General.GetNullableInteger(ddlplaceofbusiness.SelectedValue)
                  );
                        ucStatus.Text = "GST Details Added";
                    }
                    else
                    {
                        PhoenixAccountsGST.GSTDetailsUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["GSTDETAILID"].ToString())
                    , txtremark.Text
                    , decimal.Parse(txtigstpercentage.Text)
                    , decimal.Parse(txtigstamount.Text)
                    , decimal.Parse(txtcgstpercentage.Text)
                    , decimal.Parse(txtcgstamount.Text)
                    , decimal.Parse(txtsgstpercentage.Text)
                    , decimal.Parse(txtsgstamount.Text)
                    , decimal.Parse(txtugstpercentage.Text)
                    , decimal.Parse(txtugstamount.Text)
                    , decimal.Parse(txttaxamount.Text)
                    , txtvendorinvoiceno.Text
                    , General.GetNullableDateTime(txtVoucherDate.Text)
                    , General.GetNullableInteger(ddlplaceofbusiness.SelectedValue)
                    , General.GetNullableGuid(ViewState["LineItemId"].ToString())
                    );
                        ucStatus.Text = "GST Details Updated";
                    }

                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                BindHeader();
                String script = String.Format("javascript:fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                // Reset();

            }
            if (CommandName.ToUpper().Equals("UPDATELEDGER"))
            {
                if (!IsValidLineItem())
                {
                    string errormessage = "";
                    errormessage = ucError.ErrorMessage;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);

                    return;
                }
                try
                {
                    if (ViewState["GSTDETAILID"].ToString() == "")
                    {
                        PhoenixAccountsGST.GSTDetailsInsertLedger(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                  , General.GetNullableGuid(ViewState["LineItemId"].ToString())
                  , txtremark.Text
                  , decimal.Parse(txtigstpercentage.Text)
                  , decimal.Parse(txtigstamount.Text)
                  , decimal.Parse(txtcgstpercentage.Text)
                  , decimal.Parse(txtcgstamount.Text)
                  , decimal.Parse(txtsgstpercentage.Text)
                  , decimal.Parse(txtsgstamount.Text)
                  , decimal.Parse(txtugstpercentage.Text)
                  , decimal.Parse(txtugstamount.Text)
                  , decimal.Parse(txttaxamount.Text)
                  , txtvendorinvoiceno.Text
                  , General.GetNullableDateTime(txtVoucherDate.Text)
                  , General.GetNullableInteger(ddlplaceofbusiness.SelectedValue)
                  );
                        ucStatus.Text = "GST Details Added";
                    }
                    else
                    {
                        PhoenixAccountsGST.GSTDetailsUpdateLedger(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["GSTDETAILID"].ToString())
                    , txtremark.Text
                    , decimal.Parse(txtigstpercentage.Text)
                    , decimal.Parse(txtigstamount.Text)
                    , decimal.Parse(txtcgstpercentage.Text)
                    , decimal.Parse(txtcgstamount.Text)
                    , decimal.Parse(txtsgstpercentage.Text)
                    , decimal.Parse(txtsgstamount.Text)
                    , decimal.Parse(txtugstpercentage.Text)
                    , decimal.Parse(txtugstamount.Text)
                    , decimal.Parse(txttaxamount.Text)
                    , txtvendorinvoiceno.Text
                    , General.GetNullableDateTime(txtVoucherDate.Text)
                    , General.GetNullableInteger(ddlplaceofbusiness.SelectedValue)
                    , General.GetNullableGuid(ViewState["LineItemId"].ToString())
                    );
                        ucStatus.Text = "GST Details Updated";
                    }

                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                BindHeader();
                String script = String.Format("javascript:fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                // Reset();

            }
            //if (CommandName.ToUpper().Equals("NEW"))
            //{
            //    Reset();
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Reset()
    {

        txtigstpercentage.Text = "0.00";
        txtigstamount.Text = "0.00";
        txtcgstpercentage.Text = "0.00";
        txtcgstamount.Text = "0.00";
        txtsgstpercentage.Text = "0.00";
        txtsgstamount.Text = "0.00";
        txtugstpercentage.Text = "0.00";
        txtugstamount.Text = "0.00";
        txttaxamount.Text = "0.00";
    }
    protected void vendor_Changed(object sender, EventArgs e)
    {
        BindGSTDetails();

    }
    private void BindGSTDetails()
    {
        DataSet ds = PhoenixAccountsGST.GSTDetailslist(General.GetNullableInteger(txtVendorId.Text));
        ddlplaceofbusiness.DataSource = ds;
        ddlplaceofbusiness.DataTextField = "FLDBUSINESSSTATE";
        ddlplaceofbusiness.DataValueField = "FLDBUSINESSPLACE";
        ddlplaceofbusiness.DataBind();
        ddlplaceofbusiness.Items.Insert(0, new DropDownListItem("--Select--", ""));
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].Rows[0];
        //    txtgstno.Text = dr["FLDGSTNUMBER"].ToString();
        //}
    }

    public bool IsValidLineItem()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        ucError.clear = "";

        if (ddlplaceofbusiness.SelectedItem.Text == "--Select--")
            ucError.ErrorMessage = "Place of Business is Requires.";

        if (txtvendorinvoiceno.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor invoice No is required .";

        if (General.GetDateTimeToString(txtVoucherDate.Text) == null)
            ucError.ErrorMessage = "Vendor invoice Date is Required .";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void ddlplaceofbusiness_Changed(object sender, EventArgs e)
    {
        ddlplaceofbusiness.SelectedText = null;



        DataTable dt = PhoenixAccountsGST.GetStateType(General.GetNullableInteger(ddlplaceofbusiness.SelectedValue));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            lblgsttype.Text = dr["GSTAPPLICABLE"].ToString();
        }
        txtigstamount.Text = "0.00";
        txtigstpercentage.Text = "0.00";
        txtcgstamount.Text = "0.00";
        txtcgstpercentage.Text = "0.00";
        txtsgstamount.Text = "0.00";
        txtsgstpercentage.Text = "0.00";
        txtugstamount.Text = "0.00";
        txtugstpercentage.Text = "0.00";
        txttaxamount.Text = "0.00";
        txtremark.Text = null;
        txtigstamount.Enabled = true;
        txtcgstamount.Enabled = true;
        txtsgstamount.Enabled = true;
        txtugstamount.Enabled = true;
        if (lblgsttype.Text == "2" || lblgsttype.Text == "3")
        {
            txtcgstamount.Enabled = false;
            txtsgstamount.Enabled = false;
            txtugstamount.Enabled = false;
        }
        if (txtCompanyState.Text == (General.GetNullableInteger(ddlplaceofbusiness.SelectedValue).ToString()))
        {
            if (lblgsttype.Text == "0")
            {
                txtigstamount.Enabled = false;
                txtugstamount.Enabled = false;
            }
            else
            {
                txtigstamount.Enabled = false;
                txtsgstamount.Enabled = false;
                txtcgstamount.Enabled = false;
            }

        }
        else
        {
            txtcgstamount.Enabled = false;
            txtsgstamount.Enabled = false;
            txtugstamount.Enabled = false;
        }
    }
}