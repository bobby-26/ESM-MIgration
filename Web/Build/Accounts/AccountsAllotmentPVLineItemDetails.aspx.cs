using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Accounts_AccountsAllotmentPVLineItemDetails : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        // txtVendorId.Attributes.Add("style", "visibility:hidden");
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAllotmentPVLineItemDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAllotmentVesselList')", "Print Grid", "icon_print.png", "PRINT");
            MenuItemDetail.AccessRights = this.ViewState;
            MenuItemDetail.MenuList = toolbargrid.Show();

            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddImageButton("../Accounts/AccountsAllotmentPVLineItemDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolgrid.AddImageLink("javascript:CallPrint('gvAllotmentEmployeeList')", "Print Grid", "icon_print.png", "PRINT");
            MenuEmployeeList.AccessRights = this.ViewState;
            MenuEmployeeList.MenuList = toolgrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);
            MenuVoucher.AccessRights = this.ViewState;
            MenuVoucher.MenuList = toolbarmain.Show();

            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];

            if (!IsPostBack)
            {
                BindPVLineItemDetails();
                ViewState["VESSELID"] = "";
                ViewState["EMPLOYEEID"] = "";
                ViewState["BANKACCOUNTID"] = "";
                ViewState["VESSEL"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvAllotmentVesselList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvAllotmentEmployeeList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvAllotmentVesselList.Rebind();
        gvAllotmentEmployeeList.Rebind();

    }


    protected void gvAllotmentVesselList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindDataVessel();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotmentVesselList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("lblAmount");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
    }

    protected void gvAllotmentVesselList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("AMOUNT"))
            {
                ViewState["VESSELID"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                LinkButton db1 = (LinkButton)e.Item.FindControl("lblAmount");

                //{
                //    DataTable dt = PhoenixAccountsInvoicePaymentVoucher.AllotmentPVLineItemEmployee(new Guid(ViewState["voucherid"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()));
                //    gvAllotmentEmployeeList.DataSource = dt;
                //    gvAllotmentEmployeeList.VirtualItemCount = dt.Rows.Count;
                //}
                gvAllotmentEmployeeList.DataSource = null;
                gvAllotmentEmployeeList.Rebind();
                //  RebindEmployeeList();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvAllotmentEmployeeList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindDataEmployee();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotmentEmployeeList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ViewState["EMPLOYEEID"] = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
            ViewState["BANKACCOUNTID"] = ((RadLabel)e.Item.FindControl("lblBankAccountId")).Text;
            ViewState["VESSEL"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

            UserControlCommonToolTip ttip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            if (ttip != null)
            {
                ttip.Screen = "Accounts/AccountsLineItemEmployeeListToolTip.aspx?employeeid=" + ViewState["EMPLOYEEID"].ToString() + "&BANKACCOUNTID=" + ViewState["BANKACCOUNTID"].ToString() + "&VESSELID=" + ViewState["VESSEL"].ToString() + "&PVID=" + ViewState["voucherid"].ToString();

            }

        }
    }

    protected void gvAllotmentEmployeeList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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

    protected void MenuItemDetail_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {

                string[] alColumns = { "FLDVESSELCODE", "FLDVESSELNAME", "FLDCOUNT", "FLDALLTOMENTTOTAL" };
                string[] alCaptions = { "Vessel Code", "Vessel Name", "No.of Transaction", "Amount" };
                DataTable dt = PhoenixAccountsInvoicePaymentVoucher.AllotmentPVLineItemVessel(new Guid(ViewState["voucherid"].ToString()));
                General.ShowExcel("Allotment PV LineItem Vessel List", dt, alColumns, alCaptions, null, string.Empty);
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {

                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        gvAllotmentVesselList.SelectedIndexes.Clear();
        gvAllotmentVesselList.EditIndexes.Clear();
        gvAllotmentVesselList.DataSource = null;
        gvAllotmentVesselList.Rebind();
    }
    protected void RebindEmployeeList()
    {
        gvAllotmentEmployeeList.SelectedIndexes.Clear();
        gvAllotmentEmployeeList.EditIndexes.Clear();
        gvAllotmentEmployeeList.DataSource = null;
        gvAllotmentEmployeeList.Rebind();
    }

    private void BindDataVessel()
    {

        string[] alColumns = { "FLDVESSELCODE", "FLDVESSELNAME", "FLDCOUNT", "FLDALLTOMENTTOTAL" };
        string[] alCaptions = { "Vessel Code", "Vessel Name", "No.of Transaction", "Amount" };
        DataTable dt = PhoenixAccountsInvoicePaymentVoucher.AllotmentPVLineItemVessel(new Guid(ViewState["voucherid"].ToString()));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvAllotmentVesselList", "Allotment Vessel List", alCaptions, alColumns, ds);
        gvAllotmentVesselList.DataSource = dt;
        gvAllotmentVesselList.VirtualItemCount = dt.Rows.Count;
    }

    private void BindDataEmployee()
    {
        string[] alColumns = { "FLDVESSELCODE", "FLDVESSELNAME", "FLDEMPLOYEENAME", "FLDFILENO", "FLDRANKCODE", "FLDALLTOMENTTOTAL", "FLDACCOUNTNAME", "FLDBANKNAME", "FLDBANKSWIFTCODE", "FLDACCOUNTNUMBER", "FLDINTERMEDIATEBANKSWIFTCODE" };
        string[] alCaptions = { "Vessel Code", "Vessel Name", "Name", "File No.", "Rank", "Amount", "Benificiary Name", "Bank Name", "SWIFT Code", "Account No.", "Intermediary Swift" };
        DataTable dt = PhoenixAccountsInvoicePaymentVoucher.AllotmentPVLineItemEmployee(new Guid(ViewState["voucherid"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvAllotmentEmployeeList", "Allotment Employee List", alCaptions, alColumns, ds);
        gvAllotmentEmployeeList.DataSource = dt;
        gvAllotmentEmployeeList.VirtualItemCount = dt.Rows.Count;
    }

    protected void MenuEmployeeList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {

                string[] alColumns = { "FLDVESSELCODE", "FLDVESSELNAME", "FLDEMPLOYEENAME", "FLDFILENO", "FLDRANKCODE", "FLDALLTOMENTTOTAL", "FLDACCOUNTNAME", "FLDBANKNAME", "FLDBANKSWIFTCODE", "FLDACCOUNTNUMBER", "FLDINTERMEDIATEBANKSWIFTCODE" };
                string[] alCaptions = { "Vessel Code", "Vessel Name", "Name", "File No.", "Rank", "Amount", "Benificiary Name", "Bank Name", "SWIFT Code", "Account No.", "Intermediary Swift" };
                DataTable dt = PhoenixAccountsInvoicePaymentVoucher.AllotmentPVLineItemEmployee(new Guid(ViewState["voucherid"].ToString()), null);
                General.ShowExcel("Allotment PV LineItem Employee List", dt, alColumns, alCaptions, null, string.Empty);
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {

                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
                // Response.Redirect("../Accounts/AccountsInvoicePaymentVoucher.aspx?voucherid=" + ViewState["voucherid"]);
                // ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucher.aspx?voucherid=" + ViewState["voucherid"];
            }


            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["voucherid"] != null && ViewState["voucherid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsAllotmentPVLineItemDetails.aspx?voucherid=" + ViewState["voucherid"]);
            }

            else
                MenuVoucher.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindPVLineItemDetails()
    {
        DataTable dt = PhoenixAccountsInvoicePaymentVoucher.AllotmentPVLineItemDetails(new Guid(ViewState["voucherid"].ToString()));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        DataRow dr = ds.Tables[0].Rows[0];
        txtName.Text = dr["FLDCOMPANYNAME"].ToString();
        txtDetail.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
        txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
        txtRemittingAgent.Text = dr["FLDREMITTINGAGENT"].ToString();
        txtBeneficiary.Text = dr["FLDBENEFICIARYNAME"].ToString();
        txtAccountNo.Text = dr["FLDACCOUNTNUMBER"].ToString();
        txtPreparedBy.Text = dr["FLDPREPAREDBY"].ToString();
        txtDate.Text = dr["FLDPREPAREDDATE"].ToString();
        txtApprovedBy.Text = dr["FLDAPPROVEDBY"].ToString();
        txtBank.Text = dr["FLDBANKNAME"].ToString();
        txtSwiftCode.Text = dr["FLDSWIFTCODE"].ToString();
        txtIntermediarySwiftCode.Text = dr["FLDIRSWIFTCODE"].ToString();
    }

}