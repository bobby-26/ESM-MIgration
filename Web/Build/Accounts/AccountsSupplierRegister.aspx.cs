using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class Accounts_AccountsSupplierRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            // cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            toolbar.AddImageButton("../Accounts/AccountsSupplierRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAddress')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsSupplierRegister.aspx", "Filter", "search.png", "FIND");
            //toolbar.AddImageButton("../Accounts/AccountsSupplierRegister.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvAddress.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{
    //    gvAddress.CurrentPageIndex = 0;
    //    gvAddress.Rebind();
    //}

    protected void gvAddress_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAddress.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOffice_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtSuppliercode.Text = string.Empty;
                txtSuppliername.Text = string.Empty;
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
        gvAddress.SelectedIndexes.Clear();
        gvAddress.EditIndexes.Clear();
        gvAddress.DataSource = null;
        gvAddress.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds;
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDPHONE1", "FLDEMAIL1", "FLDCITY", "FLDCOUNTRYNAME", "FLDHARDNAME" };
        string[] alCaptions = { "Code", "Name", "Phone1", "Email", "City", "Country", "Status" }; ;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsInvoice.AccountsAddressSearch(
                txtSuppliercode.Text.Trim() == "" ? null : txtSuppliercode.Text.Trim(),
                txtSuppliername.Text.Trim() == "" ? null : txtSuppliername.Text.Trim(),
                null,
                null, null,
                null,
                //General.GetNullableString(",131,"),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvAddress.PageSize,
                ref iRowCount,
                ref iTotalPageCount
               , txtbankaccountnumber.Text.Trim() == "" ? null : txtbankaccountnumber.Text.Trim()
                );

        General.SetPrintOptions("gvAddress", "Address", alCaptions, alColumns, ds);

        gvAddress.DataSource = ds;
        gvAddress.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvAddress_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {

                LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkAddressName");
                lbtn.Attributes.Add("onclick", "javascript:openNewWindow('AddAddress', '','" + Session["sitepath"] + "/Registers/RegistersSupplierInvoiceApprovalUserMap.aspx?addresscode=" + lbtn.CommandArgument + "',false,1000,500);");
        //        cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Accounts/AccountsInterCompanyTransferContraVoucher.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&offsettinglineitemid=" + ViewState["offsettinglineitemid"] + "&callfrom=INTER" + "&interlineitemisposted=" + posted + "',false,1000,500);");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
            }
        }

    }
    protected void gvAddress_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string countryid = null;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDPHONE1", "FLDEMAIL1", "FLDCITY", "FLDCOUNTRYNAME", "FLDHARDNAME" };
        string[] alCaptions = { "Code", "Name", "Phone1", "Email", "City", "Country", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        gvAddress.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


        if (Filter.CurrentAddressFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentAddressFilterCriteria;

            countryid = nvc.Get("ucCountry").ToString();

            ds = PhoenixCommonRegisters.AddressSearch(
                nvc.Get("txtcode").ToString(),
                nvc.Get("txtName").ToString(),
                General.GetNullableInteger(countryid),
                null, General.GetNullableString(nvc.Get("txtCity").ToString()),
                null,
                General.GetNullableString(nvc.Get("addresstype").ToString()),
                General.GetNullableString(nvc.Get("producttype").ToString()),
                General.GetNullableString(nvc.Get("txtPostalCode").ToString()),
                General.GetNullableString(nvc.Get("txtPhone1").ToString()),
                General.GetNullableString(nvc.Get("txtEMail1").ToString()),
                General.GetNullableInteger(nvc.Get("status").ToString()),
                General.GetNullableInteger(nvc.Get("qagrading").ToString()),
                General.GetNullableString(nvc.Get("txtBusinessProfile").ToString()),
                General.GetNullableString(nvc.Get("addressdepartment").ToString()),
                sortexpression, sortdirection,
                gvAddress.CurrentPageIndex + 1,
                PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                ref iRowCount,
                ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCommonRegisters.AddressSearch(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                    gvAddress.CurrentPageIndex + 1,
                   PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                    ref iRowCount,
                    ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=Address.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Address List</h3></td>");
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
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
    }
    protected void gvAddress_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

}
