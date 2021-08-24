using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceBatchTaxandChargesList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "1")
            {
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid = new PhoenixToolbar();
                toolbargrid.AddButton("List", "BATCHLIST");
                toolbargrid.AddButton("Line Item", "LINEITEM");
                toolbargrid.AddButton("History", "HISTORY");
                toolbargrid.AddButton("Tax and Charges", "TAXANDCHARGES");
                MenuOrderFormMain.AccessRights = this.ViewState;
                MenuOrderFormMain.MenuList = toolbargrid.Show();
                MenuOrderFormMain.SelectedMenuIndex = 3;
               // MenuOrderFormMain.SetTrigger(pnlRemittance);
                //frmTitle.ShowMenu = "true";
            }

            if (!IsPostBack)
            {

                ViewState["Batchid"] = null;
                if (Request.QueryString["Batchid"] != null && Request.QueryString["Batchid"].Length >= 36)
                    ViewState["Batchid"] = Request.QueryString["Batchid"].ToString();             
                BatchEdit();
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (txtVoucherno.Text.Trim() == "")
            {
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                toolbarmain.AddButton("Post", "POST", ToolBarDirection.Right);
                Menusub.AccessRights = this.ViewState;
                Menusub.MenuList = toolbarmain.Show();
                SessionUtil.PageAccessRights(this.ViewState);
            }

            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BatchEdit()
    {
        DataSet ds = new DataSet();
        ds = PhoenixAccountsAllotmentRemittance.RemittanceBatchEdit(new Guid(ViewState["Batchid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtAccountCode.Text = ds.Tables[0].Rows[0]["FLDBANKACCOUNT"].ToString();
            txtAmountinbankCurrency.Text = ds.Tables[0].Rows[0]["FLDTOTALAMOUNTINBANKEXCHANGERATE"].ToString();
            txtAmountinUSD.Text = ds.Tables[0].Rows[0]["FLDTOTALAMOUNTINUSD"].ToString();
            txtBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString();
            txtNoOfRemittance.Text = ds.Tables[0].Rows[0]["FLDNOOFREMITTANCE"].ToString();
            txtpaydate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDPAYMENTDATE"].ToString());
            txtPaymentMode.Text = ds.Tables[0].Rows[0]["FLDPAYMENTMODENAME"].ToString();
            txtPerRemittance.Text = ds.Tables[0].Rows[0]["FLDBANKCHARGESPERREMITTANCE"].ToString();
            txtVoucherno.Text = ds.Tables[0].Rows[0]["FLDBANKCHARGESVOUCHERNO"].ToString();
            txtBankExchangerate.Text = ds.Tables[0].Rows[0]["FLDBANKEXCHAGERATE"].ToString();
            txtBankTotalchargesUSD.Text = ds.Tables[0].Rows[0]["FLDTOTALBANKCHARGES"].ToString();
            lblBankcurrency.Text = ds.Tables[0].Rows[0]["FLDBANKCURRENCYCODE"].ToString();
            lblcurrency.Text = ds.Tables[0].Rows[0]["FLDBANKCURRENCYCODE"].ToString();
            txtbanktotalinbankcur.Text = ds.Tables[0].Rows[0]["FLDAMOUNTBANKCURRENCY"].ToString();
            txtTotalAmountinUSD.Text = ds.Tables[0].Rows[0]["FLDTOTALAMOUNT"].ToString();
            txtTotalAmountCurr.Text = ds.Tables[0].Rows[0]["FLDTOTALAMOUNTCURR"].ToString();
            lblBankcurrency2.Text = ds.Tables[0].Rows[0]["FLDBANKCURRENCYCODE"].ToString();
         
        }
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("BATCHLIST"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchDownLoadHistory.aspx?type=1&Batchid=" + ViewState["Batchid"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchInstructionList.aspx?Batchid=" + ViewState["Batchid"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TAXANDCHARGES"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchTaxandChargesList.aspx?type=1&Batchid=" + ViewState["Batchid"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alCaptions = { "Vessel", "No of Remittance", "Bank Charges" };
        string[] alColumns = { "FLDVESSELNAME", "FLDNOOFREMITANCE", "FLDTOTALBANKCHARGESINUSD" };

        ds = PhoenixAccountsAllotmentRemittance.RemittanceBankChargesList(new Guid(ViewState["Batchid"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountRemittancetaxandcharges.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Remittance</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[1].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void Menusub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (txtBankExchangerate.Text == "")
                    ucError.ErrorMessage = "Exchange rate is required.";
                if (ucError.IsError)
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsAllotmentRemittance.RemittanceBankChargesInsertUpdate(new Guid(ViewState["Batchid"].ToString()), decimal.Parse(txtBankExchangerate.Text));
                BatchEdit();
            }
            if (CommandName.ToUpper().Equals("POST"))
            {
                PhoenixAccountsAllotmentRemittance.PostTaxRemittance(new Guid(ViewState["Batchid"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                BatchEdit();
            }
            gvTaxandCharges.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["Batchid"] != null)
        {
            ds = PhoenixAccountsAllotmentRemittance.RemittanceBankChargesList(new Guid(ViewState["Batchid"].ToString()));
            string[] alCaptions = { "Vessel", "No of Remittance", "Bank Charges" };
            string[] alColumns = { "FLDVESSELNAME", "FLDNOOFREMITANCE", "FLDTOTALBANKCHARGESINUSD" };
            General.SetPrintOptions("gvTaxandCharges", "Tax and Charges", alCaptions, alColumns, ds);

            gvTaxandCharges.DataSource = ds.Tables[0];

        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvTaxandCharges.Rebind();
    }
   
    protected void gvTaxandCharges_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
