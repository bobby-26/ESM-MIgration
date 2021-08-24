using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsInvoiceLineItemSubAccount : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvInvoice.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");

        MenuInvoice1.AccessRights = this.ViewState;
        MenuInvoice1.MenuList = toolbar.Show();
        MenuInvoice1.SetTrigger(pnlInvoice);

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsInvoiceLineItemSubAccount.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
        MenuInvoice.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            ViewState["INVOICELINEITEMCODE"] = Request.QueryString["QINVOICELINEITEMCODE"];
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ACTIVITYID"] = null;
            InvoiceLineItemDisplay();
        }
        BindData();
    }

    private void Reset()
    {
        ViewState["INVOICELINEITEMSUBACCOUNTCODE"] = null;
        txtSubAccountCodeEdit.Text = "";
        txtSubAccAmountEdit.Text = "";
    }

    protected void RegistersAccountMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            BindData();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alCaptions = { 
                                "Sub Account Code",
                                "Amount"                           
                              };

        string[] alColumns = {  "FLDSUBACCOUNTCODE", 
                                "FLDAMOUNT"
                             };

        ds = PhoenixAccountsInvoice.InvoiceLineItemSubAccountList(new Guid(ViewState["INVOICELINEITEMCODE"].ToString()), null);

        Response.AddHeader("Content-Disposition", "attachment; filename=InvoiceLineItemSubAccount.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Invoice Line Item Sub Account</h3></td>");
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

    protected void InvoiceLineItemDisplay()
    {
        if (ViewState["INVOICELINEITEMCODE"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceLineItemEdit(new Guid(ViewState["INVOICELINEITEMCODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                txtInvoiceDateEdit.Text = General.GetDateTimeToString(drInvoice["FLDINVOICEDATE"].ToString());
                txtSupplierRefEdit.Text = drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString();
                txtVesselCodeEdit.Text = drInvoice["FLDVESSELCODE"].ToString();
                txtVesselNameEdit.Text = drInvoice["FLDVESSELNAME"].ToString();
                txtPurchaseOrderNumberEdit.Text = drInvoice["FLDPURCHASEORDERNUMBER"].ToString();
                txtPurchasePayableAmountEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDPURCHASEPAYABLEAMOUNT"]));
                txtPurchaseAdvanceAmountEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDPURCHASEADVANCEAMOUNT"]));
                txtInvoicePayableAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEPAYABLEAMOUNT"]));
                if (drInvoice["FLDISINCLUDEDINSOA"].ToString() == "1")
                    txtIncludedinSOAYNEdit.Text = "Yes";
                else
                    txtIncludedinSOAYNEdit.Text = "No";
            }
        }
    }

    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidActivity())
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["INVOICELINEITEMSUBACCOUNTCODE"] == null)
            {
                PhoenixAccountsInvoice.InvoiceItemSubAccountInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICELINEITEMCODE"].ToString()),
                                                                    int.Parse(txtSubAccountCodeEdit.Text),
                                                                    decimal.Parse(txtSubAccAmountEdit.Text)
                                                                    );
                Reset();
                BindData();
            }

            else
            {
                PhoenixAccountsInvoice.InvoiceItemSubAccountUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICELINEITEMSUBACCOUNTCODE"].ToString()),
                                                                     int.Parse(txtSubAccountCodeEdit.Text),
                                                                      decimal.Parse(txtSubAccAmountEdit.Text));
                Reset();
                BindData();

            }

        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
            ShowExcel();
    }

    public bool IsValidActivity()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtSubAccountCodeEdit.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Sub Account is required.";

        if (txtSubAccAmountEdit.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Amount is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsInvoice.InvoiceLineItemSubAccountList(new Guid(ViewState["INVOICELINEITEMCODE"].ToString()), null);

        string[] alCaptions = { 
                                "Sub Account Code",
                                "Amount"                           
                              };

        string[] alColumns = {  "FLDSUBACCOUNTCODE", 
                                "FLDAMOUNT"
                             };

        General.SetPrintOptions("gvInvoice", "Invoice Line Item", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvInvoice.DataSource = ds;
            gvInvoice.DataBind();
        }
    }


    protected void InvoiceClick(object sender, CommandEventArgs e)
    {
        ViewState["INVOICELINEITEMSUBACCOUNTCODE"] = e.CommandArgument;
        InvoiceLineItemEdit();
    }

    protected void InvoiceLineItemEdit()
    {
        if (ViewState["INVOICELINEITEMSUBACCOUNTCODE"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceLineItemSubAccountEdit(new Guid(ViewState["INVOICELINEITEMSUBACCOUNTCODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                txtInvoiceDateEdit.Text = General.GetDateTimeToString(drInvoice["FLDINVOICEDATE"].ToString());
                txtSupplierRefEdit.Text = drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString();
                txtVesselCodeEdit.Text = drInvoice["FLDVESSELCODE"].ToString();
                txtVesselNameEdit.Text = drInvoice["FLDVESSELNAME"].ToString();
                txtPurchaseOrderNumberEdit.Text = drInvoice["FLDPURCHASEORDERNUMBER"].ToString();
                txtPurchasePayableAmountEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDPURCHASEPAYABLEAMOUNT"]));
                txtPurchaseAdvanceAmountEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDPURCHASEADVANCEAMOUNT"]));
                txtInvoicePayableAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDINVOICEPAYABLEAMOUNT"]));
                if (drInvoice["FLDISINCLUDEDINSOA"].ToString() == "1")
                    txtIncludedinSOAYNEdit.Text = "Yes";
                else
                    txtIncludedinSOAYNEdit.Text = "No";
                txtSubAccountCodeEdit.Text = drInvoice["FLDSUBACCOUNTCODE"].ToString();
                txtSubAccAmountEdit.Text = string.Format(String.Format("{0:#####.00}", drInvoice["FLDAMOUNT"]));
                BindData();
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
    } 

    protected void gvInvoice_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    protected void gvInvoice_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
