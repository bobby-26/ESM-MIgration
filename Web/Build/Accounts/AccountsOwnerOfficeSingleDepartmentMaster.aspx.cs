using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsOwnerOfficeSingleDepartmentMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerOfficeSingleDepartmentMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFundReceived')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerOfficeSingleDepartmentMaster.aspx", "Find", "search.png", "FIND");
            MenuSingleDepartmentGrid.AccessRights = this.ViewState;
            MenuSingleDepartmentGrid.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["OwnerOfficeFundId"] = null;

                ViewState["PAGENUMBER"] = 1;
                gvFundReceived.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSingleDepartmentGrid_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvFundReceived.Rebind();
    }

    protected void gvFundReceived_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvFundReceived_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
                ViewState["OwnerOfficeFundId"] = ((RadLabel)e.Item.FindControl("lblOwnerOfficeFundId")).Text;
                if (ViewState["OwnerOfficeFundId"] != null)
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOwnerOfficeSingleDepartment.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblVoucherid = ((RadLabel)e.Item.FindControl("lblVoucherid"));

                if (lblVoucherid.Text != null)
                {
                    PhoenixAccountsOwnerOfficeSingleDepartment.FundRequestBankReceiptDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(lblVoucherid.Text));
                }
                ucStatus.Text = "Deleted Successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFundReceived_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        _gridView.SelectedIndex = se.NewSelectedIndex;
        string OwnerOfficeFundId = _gridView.DataKeys[se.NewSelectedIndex].Value.ToString();
        ViewState["OwnerOfficeFundId"] = OwnerOfficeFundId;
        Rebind();
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERSTATUS", "FLDDATE", "FLDCURRENCYCODE", "FLDAMOUNT" };
            string[] alCaptions = { "Voucher Number", "Voucher Status", "Date", "Currency", "Amount" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherList(txtVoucherNumber.Text, null, 1231,
                                                                                  gvFundReceived.CurrentPageIndex + 1,
                                                                                  gvFundReceived.PageSize,
                                                                                  ref iRowCount,
                                                                                  ref iTotalPageCount);

            General.SetPrintOptions("gvFundReceived", "Voucher", alCaptions, alColumns, ds);

            gvFundReceived.DataSource = ds;
            gvFundReceived.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (ViewState["OwnerOfficeFundId"] == null)
                {
                    ViewState["OwnerOfficeFundId"] = dr["FLDOWNEROFFICEFUNDID"].ToString();
                 }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                 ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOwnerOfficeSingleDepartment.aspx?OwnerOfficeFundId=";
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERSTATUS", "FLDDATE", "FLDCURRENCYCODE", "FLDAMOUNT" };
        string[] alCaptions = { "Voucher Number", "Voucher Status", "Date", "Currency", "Amount" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherList(txtVoucherNumber.Text, null, 1231,
                                                                              gvFundReceived.CurrentPageIndex + 1,
                                                                             PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                              ref iRowCount,
                                                                              ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename= Voucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Voucher</h3></td>");
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

    private void SetRowSelection()
    {
        gvFundReceived.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvFundReceived.Items)
        {
            if (item.GetDataKeyValue("FLDOWNEROFFICEFUNDID").ToString().Equals(ViewState["OwnerOfficeFundId"].ToString()))
            {
                gvFundReceived.SelectedIndexes.Add(item.ItemIndex);
            }
        }
        if (ViewState["OwnerOfficeFundId"].ToString() != null)
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOwnerOfficeSingleDepartment.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblOwnerOfficeFundId = ((RadLabel)gvFundReceived.Items[rowindex].FindControl("lblOwnerOfficeFundId"));
            if (lblOwnerOfficeFundId != null)
                ViewState["OwnerOfficeFundId"] = lblOwnerOfficeFundId.Text;
            if (ViewState["OwnerOfficeFundId"].ToString() != null)
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOwnerOfficeSingleDepartment.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvFundReceived.SelectedIndexes.Clear();
        gvFundReceived.EditIndexes.Clear();
        gvFundReceived.DataSource = null;
        gvFundReceived.Rebind();
    }
    protected void gvFundReceived_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFundReceived.CurrentPageIndex + 1;
        BindData();
    }
}
