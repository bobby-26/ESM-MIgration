using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsRemittanceHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoiceHistory')", "Print Grid", "icon_print.png", "PRINT");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("History", "HISTORY", ToolBarDirection.Right);
            toolbarmain.AddButton("Invoice", "INVOICE", ToolBarDirection.Right);
            toolbarmain.AddButton("Line Items", "LINEITEMS", ToolBarDirection.Right);
            toolbarmain.AddButton("Remittance", "REMITTANCE", ToolBarDirection.Right);
          
            if (!IsPostBack)
            {
                ViewState["RemittanceNumber"] = "";
                if (Request.QueryString["REMITTANCEID"] != null)
                    ViewState["REMITTANCEID"] = Request.QueryString["REMITTANCEID"].ToString();
                else
                    ViewState["REMITTANCEID"] = Guid.Empty;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;
            }

            if (ViewState["REMITTANCEID"] != null)
            {
                MenuOrderFormMain.Title = "Remittance History-(" + PhoenixAccountsVoucher.VoucherNumber + ") ";
            }

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 0;
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
            if (CommandName.ToUpper().Equals("REMITTANCE"))
            {
                Response.Redirect("../Accounts/AccountsRemittanceMaster.aspx");
            }
            if (CommandName.ToUpper().Equals("LINEITEMS") && ViewState["REMITTANCEID"] != null && ViewState["REMITTANCEID"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsRemittanceRequestLineItem.aspx?REMITTENCEID=" + ViewState["REMITTANCEID"]);
            }
            if (CommandName.ToUpper().Equals("INVOICE") && ViewState["REMITTANCEID"] != null && ViewState["REMITTANCEID"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsRemittanceInvoiceMaster.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"]);
            }

            //if (CommandName.ToUpper().Equals("SUBMITFORMDAPPROVAL"))
            //{
            //    PhoenixAccountsRemittance.PrepareRemittanceInstruction(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            //    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            //}
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
        int iRowCount = 0;
        //int iTotalPageCount = 0;
        string type = "";

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsRemittance.RemittanceHistoryList(new Guid(ViewState["REMITTANCEID"].ToString())
                                                     , type);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsRemittanceHistory.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + frmTitle.Text + "</h3></td>");
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
        foreach (DataRow dr in ds.Tables[0].Rows)
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetRedirectURL()
    //{
    //    if (ViewState["qfrom"] != null)
    //    {
    //        if (ViewState["qfrom"].ToString() == "invoice")
    //        {
    //            ViewState["URL"] = "../Accounts/AccountsInvoiceMaster.aspx?qinvoicecode=";
    //        }
    //        else if (ViewState["qfrom"].ToString() == "AD")
    //        {
    //            ViewState["URL"] = "../Accounts/AccountsInvoiceAdjustmentMaster.aspx?qinvoicecode=";
    //        }
    //        else if (ViewState["qfrom"].ToString() == "PR")
    //        {
    //            ViewState["URL"] = "../Accounts/AccountsPostInvoiceMaster.aspx?qinvoicecode=";
    //        }
    //        else if (ViewState["qfrom"].ToString() == "invoiceforpurchase")
    //        {
    //            ViewState["URL"] = "../Accounts/AccountsInvoiceMasterForPurchase.aspx?qinvoicecode=";
    //        }
    //    }
    //}

    private void BindRemittanceNumber()
    {

        frmTitle.Text = frmTitle.Text + " - " + PhoenixAccountsVoucher.VoucherNumber + "";


    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string type = "";
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        type = rblHistoryType.SelectedItem.Value;
        ds = PhoenixAccountsRemittance.RemittanceHistoryList(new Guid(ViewState["REMITTANCEID"].ToString())
                                                     , type);
        General.SetPrintOptions("gvInvoiceHistory", "Accounts Invoice History" + "-" + ViewState["Invoicenumber"], alCaptions, alColumns, ds);

        gvInvoiceHistory.DataSource = ds;
        gvInvoiceHistory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ReBindData(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            // gvInvoiceHistory.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvInvoiceHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
