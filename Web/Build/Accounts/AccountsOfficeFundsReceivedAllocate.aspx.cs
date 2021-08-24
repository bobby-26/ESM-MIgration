using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsOfficeFundsReceivedAllocate : PhoenixBasePage
{
    public string strCurrency = string.Empty;
    public string strAmountTotal = string.Empty;
    public decimal totalAmount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["Source"] = null;
                ViewState["Source"] = Request.QueryString["Source"].ToString();
                ViewState["Ownerfundreceived"] = Request.QueryString["Ownerfundreceived"].ToString();

                ViewState["OwnerOfficeFundId"] = "";
                if (Request.QueryString["OwnerOfficeFundId"] != null)
                    ViewState["OwnerOfficeFundId"] = Request.QueryString["OwnerOfficeFundId"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["VoucherStatus"] = "";
                ViewState["CurrencyCode"] = ""; 
                if (ViewState["OwnerOfficeFundId"] != null)
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOwnerOfficeSingleDepartmentAllocate.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() 
                        + "&Source=" + ViewState["Source"].ToString() + "&Ispopup=popup" + "&Ownerfundreceived=" + ViewState["Ownerfundreceived"].ToString();
                FundReceivedVoucherEdit();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (ViewState["Source"].ToString() == "1")
            {               
                toolbar.AddButton("Allocate", "ALLOCATE",ToolBarDirection.Right);
                toolbar.AddButton("Owner Fund", "FUND", ToolBarDirection.Right);
                MenuOfficeFund.AccessRights = this.ViewState;
                MenuOfficeFund.MenuList = toolbar.Show();
                MenuOfficeFund.SelectedMenuIndex = 0;
            }
            if (ViewState["Source"].ToString() == "2")
            {             
                toolbar.AddButton("Allocate", "ALLOCATE",ToolBarDirection.Right);
                toolbar.AddButton("Office Fund", "FUND", ToolBarDirection.Right);
                MenuOfficeFund.AccessRights = this.ViewState;
                MenuOfficeFund.MenuList = toolbar.Show();
                MenuOfficeFund.SelectedMenuIndex = 0;
            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOfficeFundsReceivedAllocate.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFundAllocate')", "Print Grid", "icon_print.png", "PRINT");
            MenuOfficeFundGrid.AccessRights = this.ViewState;
            MenuOfficeFundGrid.MenuList = toolbargrid.Show();
            gvFundAllocate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOfficeFund_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FUND"))
            {
                if (ViewState["Source"].ToString() == "1")
                {
                    Response.Redirect("../Accounts/AccountsOwnerFundsReceived.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString());
                }
                if (ViewState["Source"].ToString() == "2")
                {
                    Response.Redirect("../Accounts/AccountsOfficeFundsReceived.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() );
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOfficeFundGrid_TabStripCommand(object sender, EventArgs e)
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidLineItem(string description, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Allocated amount is required.";

        return (!ucError.IsError);
    }

    protected void gvFundAllocate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEditCancel = (ImageButton)e.Item.FindControl("cmdEditCancel");
            if (cmdEditCancel != null) cmdEditCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditCancel.CommandName);

            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
        }
        if (e.Item is GridDataItem)
        {
            RadLabel lblCurrencyCode = (RadLabel)e.Item.FindControl("lblCurrencyCode");
            RadLabel lblOwnerOfficeAllocateId = (RadLabel)e.Item.FindControl("lblOwnerOfficeAllocateId");
            ImageButton cmdConvert = (ImageButton)e.Item.FindControl("cmdConvert");
            RadLabel lblAllocatedAmountBankCur = (RadLabel)e.Item.FindControl("lblAllocatedAmountBankCur");
            RadLabel lblAllocatedAmount = (RadLabel)e.Item.FindControl("lblAllocatedAmount");

            if (cmdConvert != null)
            {
                cmdConvert.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsOwnerOfficeSingleDepartmentConvertCurrency.aspx?OwnerOfficeAllocateId=" + lblOwnerOfficeAllocateId.Text +
                    "&BankCurrency=" + ViewState["CurrencyCode"].ToString() + "&Currency=" + lblCurrencyCode.Text + "&AllocatedAmount=" + lblAllocatedAmount.Text + "'); return true;");
            }

            if (ViewState["CurrencyCode"].ToString() != lblCurrencyCode.Text && ViewState["VoucherStatus"].ToString() == "Draft")
            {
                if (cmdConvert != null) cmdConvert.Visible = true;
            }
            else
            {
                if (cmdConvert != null) cmdConvert.Visible = false;
            }
            totalAmount = totalAmount + Convert.ToDecimal(lblAllocatedAmountBankCur.Text == "" ? "0" : lblAllocatedAmountBankCur.Text);
            strAmountTotal = totalAmount.ToString();
        }
    }

    protected void gvFundAllocate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
           // int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidLineItem(((RadTextBox)e.Item.FindControl("lblDescriptionEdit")).Text
                                        , ((UserControlMaskNumber)e.Item.FindControl("ucAllocatedAmountEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOwnerOfficeSingleDepartment.FundDebitCreditNoteAllocateUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(((RadLabel)e.Item.FindControl("lblOwnerOfficeAllocateId")).Text)
                     , ((RadTextBox)e.Item.FindControl("lblDescriptionEdit")).Text
                     , Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAllocatedAmountEdit")).Text));
                ucStatus.Text = "Travel claim line item updated";

                Rebind();
                FundReceivedVoucherEdit();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsOwnerOfficeSingleDepartment.FundDebitCreditNoteAllocateDelete(new Guid(((RadLabel)e.Item.FindControl("lblOwnerOfficeAllocateId")).Text)
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                ucStatus.Text = "Deleted successfully";
                Rebind();
                FundReceivedVoucherEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            totalAmount = 0;

            string[] alColumns = { "FLDREFERENCEDOCUMENTNO", "FLDLONGDESCRIPTION", "FLDCURRENCY", "FLDALLOCATIONAMOUNT", "FLDALLOCATIONAMOUNTBANKCUR" };
            string[] alCaptions = { "Reference No", "Description", "Currency", "Allocated Amount", "Allocated Amount" };

            DataSet ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundDebitCreditNoteAllocateList(General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()),
                                                                                                        (int)ViewState["PAGENUMBER"],
                                                                                                        gvFundAllocate.PageSize,
                                                                                                        ref iRowCount,
                                                                                                        ref iTotalPageCount);

            General.SetPrintOptions("gvFundAllocate", "Allocate List", alCaptions, alColumns, ds);

            gvFundAllocate.DataSource = ds;
            gvFundAllocate.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                totalAmount = 0;
            }
            else
            {
                DataTable dt = ds.Tables[0];
            }

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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREFERENCEDOCUMENTNO", "FLDLONGDESCRIPTION", "FLDCURRENCY", "FLDALLOCATIONAMOUNT", "FLDALLOCATIONAMOUNTBANKCUR" };
        string[] alCaptions = { "Reference No", "Description", "Currency", "Allocated Amount", "Allocated Amount" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundDebitCreditNoteAllocateList(General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()),
                                                                                                    (int)ViewState["PAGENUMBER"],
                                                                                                    PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                                                    ref iRowCount,
                                                                                                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=AllocateList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Allocate List</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    public void FundReceivedVoucherEdit()
    {
        try
        {
            DataSet ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherEdit(General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["VoucherStatus"] = dr["FLDVOUCHERSTATUS"].ToString();
                ViewState["CurrencyCode"] = dr["FLDCURRENCYCODE"].ToString();
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
        gvFundAllocate.Rebind();
    }
    protected void Rebind()
    {
        gvFundAllocate.SelectedIndexes.Clear();
        gvFundAllocate.EditIndexes.Clear();
        gvFundAllocate.DataSource = null;
        gvFundAllocate.Rebind();
    }
    protected void gvFundAllocate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFundAllocate.CurrentPageIndex + 1;
        BindData();
    }
}
