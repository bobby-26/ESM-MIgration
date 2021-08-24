using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsInvoiceQueryMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
            MenuInvoiceQueryGrid.AccessRights = this.ViewState;
            //MenuInvoiceQueryGrid.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["invoicecode"] = null;
                ViewState["callfrom"] = null;
                ViewState["PAGEURL"] = null;
                gvInvoiceQuery.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            if (Request.QueryString["qinvoicecode"] != null)
            {
                ViewState["invoicecode"] = Request.QueryString["qinvoicecode"].ToString();

                if (Request.QueryString["qcallfrom"] != null && Request.QueryString["qcallfrom"] != string.Empty)
                    ViewState["callfrom"] = Request.QueryString["qcallfrom"];

                DataSet ds = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["invoicecode"].ToString()));
                if (ds.Tables[0].Rows[0]["FLDINVOICESTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 60, "INR"))
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceQueryQuestions.aspx?qinvoicecode=" + ViewState["invoicecode"];
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceQueryClarification.aspx?qinvoicecode=" + ViewState["invoicecode"];
                }
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();      

            if(ViewState["callfrom"].ToString() == "invoice")
            {
                toolbarmain.AddButton("Invoice", "INVOICE");
                toolbarmain.AddButton("PO", "PO");
                toolbarmain.AddButton("Direct PO", "DPO");
                toolbarmain.AddButton("Attachments", "ATTACHMENTS");
                toolbarmain.AddButton("Queries", "QUERIES");
                toolbarmain.AddButton("History", "HISTORY");
                MenuInvoiceQuery.SelectedMenuIndex = 4;
            }
            else
            {
                toolbarmain.AddButton("Invoice", "INVOICE");
                toolbarmain.AddButton("PO", "PO");
                toolbarmain.AddButton("Attachments", "ATTACHMENTS");
                toolbarmain.AddButton("Queries", "QUERIES");
                toolbarmain.AddButton("History", "HISTORY");
                MenuInvoiceQuery.SelectedMenuIndex = 3;
            }

            MenuInvoiceQuery.AccessRights = this.ViewState;
            MenuInvoiceQuery.MenuList = toolbarmain.Show();
            
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
            DataSet ds = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["invoicecode"].ToString()));
            if (ds.Tables[0].Rows[0]["FLDINVOICESTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 60, "INR"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceQueryQuestions.aspx?qinvoicecode=" + ViewState["invoicecode"];
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceQueryClarification.aspx?qinvoicecode=" + ViewState["invoicecode"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuInvoiceQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                if (ViewState["callfrom"].ToString() == "invoice")
                {
                    Response.Redirect("../Accounts/AccountsInvoiceMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
                if (ViewState["callfrom"].ToString() == "AD")
                {
                    Response.Redirect("../Accounts/AccountsInvoiceAdjustmentMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
                if (ViewState["callfrom"].ToString() == "PR")
                {
                    Response.Redirect("../Accounts/AccountsPostInvoiceMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
            }
            if (CommandName.ToUpper().Equals("PO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=" + ViewState["callfrom"]);
            }
            if (CommandName.ToUpper().Equals("DPO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceDirectPO.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=invoice");
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=" + ViewState["callfrom"]);
            }
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceHistory.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=" + ViewState["callfrom"], false);
            }
            if (CommandName.ToUpper().Equals("QUERIES") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceQueryMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=invoice", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuInvoiceQueryGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ViewState["PAGENUMBER"] = 1;
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
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
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixAccountsInvoiceQuery.InvoiceQueryList(new Guid(ViewState["invoicecode"].ToString())
                                                                      , (int)ViewState["PAGENUMBER"]
                                                                      , gvInvoiceQuery.PageSize
                                                                      , ref iRowCount
                                                                      , ref iTotalPageCount);

            //General.SetPrintOptions("gvInvoiceQuery", "Debit/Credit Note List", alCaptions, alColumns, ds);
            gvInvoiceQuery.DataSource = ds;
            gvInvoiceQuery.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
    }
    protected void gvInvoiceQuery_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvInvoiceQuery_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvoiceQuery.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvInvoiceQuery.SelectedIndexes.Clear();
        gvInvoiceQuery.EditIndexes.Clear();
        gvInvoiceQuery.DataSource = null;
        gvInvoiceQuery.Rebind();
    }
}
