using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Accounts_AccountsCreditDebitNoteRebateReceivable : PhoenixBasePage
{
    public decimal totalAmount = 0;
    public string strAmountTotal = string.Empty;
    PhoenixToolbar toolbargrid;
    PhoenixToolbar toolbarmain;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ISPOSTED"] = null;
                ViewState["ISATTACHMENT"] = null;

                gvRebateReceivable.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["creditdebitnoteid"] != null && Request.QueryString["creditdebitnoteid"] != string.Empty)
                    ViewState["creditdebitnoteid"] = Request.QueryString["creditdebitnoteid"];
            }
            BindDataMain();

            SessionUtil.PageAccessRights(this.ViewState);
            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Credit Note", "VOUCHER");
            toolbarmain.AddButton("Rebate Receivable", "REBATE RECEIVABLE");
            if (ViewState["ISPOSTED"].ToString() != "1")
                toolbarmain.AddButton("Post", "POST", ToolBarDirection.Right);
            MenuRebateReceivable.AccessRights = this.ViewState;
            MenuRebateReceivable.MenuList = toolbarmain.Show();
            MenuRebateReceivable.SelectedMenuIndex = 1;

            SessionUtil.PageAccessRights(this.ViewState);
            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsCreditDebitNoteRebateReceivable.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRebateReceivable')", "Print Grid", "icon_print.png", "PRINT");
            if (ViewState["ISPOSTED"].ToString() != "1")
                toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsCreditNoteRebateVoucherallocation.aspx?&creditdebitnoteid=" + ViewState["creditdebitnoteid"] + "');return false;", "Add", "add.png", "ADDDISPATCH");

            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();

            // BindData();
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderRebateReceivable_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void MenuRebateReceivable_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsCreditDebitNoteMaster.aspx?creditdebitnoteid=" + ViewState["creditdebitnoteid"].ToString());
            }
            else if (CommandName.ToUpper().Equals("POST"))
            {

                if (!IsValidPost(strAmountTotal, int.Parse(ViewState["ISATTACHMENT"].ToString())))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsCreditDebitNoteRebateReceivable.RebateReceivablePost(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                        new Guid(ViewState["creditdebitnoteid"].ToString()),
                                                                                        PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                ucStatus.Text = "Rebate Receivable Posted";
                BindDataMain();
                Rebind();

                SessionUtil.PageAccessRights(this.ViewState);
                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Credit Note", "VOUCHER");
                toolbarmain.AddButton("Line Items", "GENERAL");
                MenuRebateReceivable.AccessRights = this.ViewState;
                MenuRebateReceivable.MenuList = toolbarmain.Show();

                SessionUtil.PageAccessRights(this.ViewState);
                toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Accounts/AccountsCreditDebitNoteRebateReceivable.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvRebateReceivable')", "Print Grid", "icon_print.png", "PRINT");
                MenuOrderLineItem.AccessRights = this.ViewState;
                MenuOrderLineItem.MenuList = toolbargrid.Show();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvRebateReceivable_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;


        if (e.Item is GridEditableItem)
        {
            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");

            LinkButton lnkVoucherLineItemNo = (LinkButton)e.Item.FindControl("lnkVoucherLineItemNo");

            RadLabel lblAccount = (RadLabel)e.Item.FindControl("lblAccount");

            if (lnkVoucherLineItemNo.Text == "10")
            {
                cmdDelete.Visible = false;
                cmdEdit.Visible = false;
            }
            else if (lblAccount.Text == "1201004" && drv["FLDSOURCEVOUCHERNUMBER"].ToString() != "" && drv["FLDSOURCEVOUCHERNUMBER"].ToString() != null)
            {
                cmdDelete.Visible = false;
                cmdEdit.Visible = false;
            }
            else
            {
                if (cmdDelete != null) cmdDelete.Visible = true;
                if (drv["FLDSOURCEVOUCHERNUMBER"].ToString() == "")
                {
                    if (cmdEdit != null) cmdEdit.Visible = true;
                }
                else
                {
                    if (cmdEdit != null) cmdEdit.Visible = false;
                }
            }

            ImageButton imgShowBudget = (ImageButton)e.Item.FindControl("imgShowBudget");
            if (imgShowBudget != null)
            {
                imgShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListSubAccount', 'codehelp1', '', '../Common/CommonPickListSubAccount.aspx', true); ");
                imgShowBudget.Visible = SessionUtil.CanAccess(this.ViewState, imgShowBudget.CommandName);
            }
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton imgShowAccount = (ImageButton)e.Item.FindControl("imgShowAccount");
            if (imgShowAccount != null)
            {
                imgShowAccount.Attributes.Add("onclick", "return showPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx', true); ");
                imgShowAccount.Visible = SessionUtil.CanAccess(this.ViewState, imgShowAccount.CommandName);
            }
        }
    }
    protected void gvRebateReceivable_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRebateReceivable.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRebateReceivable_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtAccountId")).Text,
                    ((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency,
                    ((UserControlMaskNumber)e.Item.FindControl("txtBaseAmount")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsCreditDebitNoteRebateReceivable.InsertRebateReceivable(new Guid(ViewState["creditdebitnoteid"].ToString()),
                                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                        Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtBaseAmount")).Text),
                                                                                        int.Parse(((RadTextBox)e.Item.FindControl("txtAccountId")).Text),
                                                                                        int.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency),
                                                                                        ((RadTextBox)e.Item.FindControl("txtLongDescription")).Text,
                                                                                        PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                ((UserControlMaskNumber)e.Item.FindControl("txtBaseAmount")).Focus();
                totalAmount = 0;
                Rebind();

            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                Guid lblVoucherLineItemAllocatedId = new Guid(((RadLabel)e.Item.FindControl("lblVoucherLineItemAllocatedId")).Text);
                Guid txtBudgetIdEdit = new Guid(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
              
                PhoenixAccountsCreditDebitNoteRebateReceivable.RebateReceivableUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                        lblVoucherLineItemAllocatedId,
                                                                                       txtBudgetIdEdit);
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsCreditDebitNoteRebateReceivable.DebitNoteRebateReceivableAllocateddelete(new Guid(((RadLabel)e.Item.FindControl("lblVoucherLineItemAllocatedId")).Text),
                                                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                totalAmount = 0;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindDataMain()
    {
        DataSet ds = PhoenixAccountsCreditDebitNoteRebateReceivable.RebateReceivableMain(new Guid(ViewState["creditdebitnoteid"].ToString()));

        DataRow dr = ds.Tables[0].Rows[0];
        txtRegisterNo.Text = dr["FLDCNREGISTERNO"].ToString();
        txtSupplier.Text = dr["FLDCODE"].ToString() + " / " + dr["FLDNAME"].ToString();
        txtCurrencyAmount.Text = dr["FLDCURRENCYCODE"].ToString() + " / " + string.Format(String.Format("{0:###,###,###.00}", dr["FLDAMOUNT"]));
        txtStatus.Text = dr["FLDHARDNAME"].ToString();
        ViewState["ISPOSTED"] = dr["FLDISPOSTED"].ToString();
        ViewState["ISATTACHMENT"] = dr["FLDISATTACHMENT"].ToString();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iTotalAmount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = {"FLDROWNO","FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDCURRENCYNAME","FLDLONGDESCRIPTION",
                                 "FLDAMOUNT"};
        string[] alCaptions = {"Row No.","Account Code", "Account Description","Sub Account Code","Transaction Currency","Long Description",
                                 "Prime Amount"};

        ds = PhoenixAccountsCreditDebitNoteRebateReceivable.RebateReceivablelist(new Guid(ViewState["creditdebitnoteid"].ToString())
                                                                                  , (int)ViewState["PAGENUMBER"]
                                                                                  , gvRebateReceivable.PageSize
                                                                                  , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                  , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                                  , ref iRowCount, ref iTotalPageCount
                                                                                  , ref iTotalAmount);
        strAmountTotal = String.Format("{0:n}", iTotalAmount);

        General.SetPrintOptions("gvRebateReceivable", "Rebate Receivable Details", alCaptions, alColumns, ds);
        gvRebateReceivable.DataSource = ds;
        gvRebateReceivable.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            totalAmount = 0;


            if (ViewState["creditdebitnoteid"] != null)
            {
                if (ViewState["ISPOSTED"].ToString() == "1")
                    gvRebateReceivable.Columns[7].Visible = false;
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iTotalAmount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        string[] alColumns = {"FLDROWNO","FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDCURRENCYNAME","FLDLONGDESCRIPTION",
                                 "FLDAMOUNT"};
        string[] alCaptions = {"Row No.","Account Code", "Account Description","Sub Account Code","Transaction Currency","Long Description",
                                 "Prime Amount"};


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixAccountsCreditDebitNoteRebateReceivable.RebateReceivablelist(new Guid(ViewState["creditdebitnoteid"].ToString())
                                                                                  , (int)ViewState["PAGENUMBER"]
                                                                                  , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                                  , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                  , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                                  , ref iRowCount, ref iTotalPageCount
                                                                                  , ref iTotalAmount);

        Response.AddHeader("Content-Disposition", "attachment; filename=RebateReceivableDetails.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Rebate Receivable Details</h3></td>");
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

    private bool IsValidData(string account, string currency, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (account.Trim().Equals(""))
            ucError.ErrorMessage = "Account code is required.";
        if (currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required.";
        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        return (!ucError.IsError);
    }

    private bool IsValidPost(string amount, int attachment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Convert.ToDecimal(amount) != 0)
            ucError.ErrorMessage = "Rebate Receivable cannot be post. Total amount is not equal to zero.";

        if (attachment == 0)
            ucError.ErrorMessage = "There is no Attachment";

        return (!ucError.IsError);
    }

    private void Rebind()
    {
        gvRebateReceivable.EditIndexes.Clear();
        gvRebateReceivable.SelectedIndexes.Clear();
        gvRebateReceivable.DataSource = null;
        gvRebateReceivable.Rebind();
    }

}
