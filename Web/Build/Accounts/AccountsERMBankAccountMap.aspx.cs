using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsERMBankAccountMap : PhoenixBasePage
{
    // protected override void Render(HtmlTextWriter writer)
    // {
    //     foreach (GridViewRow r in gvERMBankAccMap.Rows)
    //     {
    //         if (r.RowType == DataControlRowType.DataRow)
    //         {
    //             Page.ClientScript.RegisterForEventValidation(gvERMBankAccMap.UniqueID, "Edit$" + r.RowIndex.ToString());
    //         }
    //     }
    //
    //     base.Render(writer);
    // }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsERMBankAccountMap.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvERMBankAccMap')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsERMBankAccountMap.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsERMBankAccountMap.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuERMBankAccMap.AccessRights = this.ViewState;
            MenuERMBankAccMap.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvERMBankAccMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindData();
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuERMBankAccMap_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                // gvERMBankAccMap.EditIndex = -1;
                // gvERMBankAccMap.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                subaccount.Text = string.Empty;
                CompanyName.Text = string.Empty;
                description.Text = string.Empty;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvERMBankAccMap_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (Validate((((RadTextBox)e.Item.FindControl("txtCompanyPrefixAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtCompanyNameAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccIDAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtZIDAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtCurrencyCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtMappedERMCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXPayActAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtCompanyBankCurrencyAdd")).Text)))
                {
                    PhoenixERMBankAccMap.ERMBankAccMapInsert((((RadTextBox)e.Item.FindControl("txtCompanyPrefixAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtCompanyNameAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccIDAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtZIDAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtCurrencyCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtMappedERMCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXPayActAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtCompanyBankCurrencyAdd")).Text)
                                                          );
                    ucStatus.Text = "Account Saved Successfully";
                    gvERMBankAccMap.Rebind();
                }

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (Validate((((RadTextBox)e.Item.FindControl("txtCompanyPrefixEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtCompanyNameEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtSubAccIDEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtSubAccEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtZIDEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtCurrencyCodeEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtMappedERMCodeEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtXPayActEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtCompanyBankCurrencyEdit")).Text)))
                {

                    PhoenixERMBankAccMap.ERMBankAccMapUpdate((((RadTextBox)e.Item.FindControl("txtCompanyPrefixEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtCompanyNameEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtSubAccIDEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtSubAccEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtZIDEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtCurrencyCodeEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtMappedERMCodeEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtXPayActEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtCompanyBankCurrencyEdit")).Text),
                                                                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkeyEdit")).Text)
                                                              );

                    ucStatus.Text = "Account Updated Successfully";
                    gvERMBankAccMap.Rebind();
                }
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixERMBankAccMap.ERMBankAccMapDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkey")).Text));
                gvERMBankAccMap.Rebind();
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    private bool Validate(
                                                     string companyprefix
                                                    , string companyname
                                                    , string subaccid
                                                    , string subaccount
                                                    , string description
                                                    , string zid
                                                    , string currencycode
                                                    , string mappedermcode
                                                    , string xpayact
                                                    , string companycurrncy)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (string.IsNullOrEmpty(companyprefix))
        //    ucError.ErrorMessage = "Company Prefix is required.";
        //if (string.IsNullOrEmpty(companyname))
        //    ucError.ErrorMessage = "Company Name required.";
        //if (string.IsNullOrEmpty(subaccid))
        //    ucError.ErrorMessage = "Sub Account ID is required.";
        //if (string.IsNullOrEmpty(subaccount))
        //    ucError.ErrorMessage = "Sub Account is required.";
        //if (string.IsNullOrEmpty(description))
        //    ucError.ErrorMessage = "Description is required.";
        //if (string.IsNullOrEmpty(zid))
        //    ucError.ErrorMessage = "zid is required.";
        //if (string.IsNullOrEmpty(currencycode))
        //    ucError.ErrorMessage = "Currency Code is required.";
        //if (string.IsNullOrEmpty(mappedermcode))
        //    ucError.ErrorMessage = "Mapped ERM Code is required.";
        //if (string.IsNullOrEmpty(xpayact))
        //    ucError.ErrorMessage = "X Pay Act is required.";
        //if (string.IsNullOrEmpty(companycurrncy))
        //    ucError.ErrorMessage = "Company Currency is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCOMPANYPREFIX", "FLDCOMPANYNAME", "FLDSUBACCOUNTID", "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDZID", "FLDCURRENCYCODE", "FLDMAPPEDERMCODE", "FLDXPAYACT", "FLDCOMPANYBANKCUR" };
        string[] alCaptions = { "Company Prefix", "Company Name", "Sub Account Id", "Sub Account", "Description", "ZID", "Currency Code", "Mapped ERM Code", "X Pay Act", "Company Bank Currency" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixERMBankAccMap.ERMBankAccMapSearch(subaccount.Text, CompanyName.Text, description.Text, (int)ViewState["PAGENUMBER"],
                                                    gvERMBankAccMap.PageSize, ref iRowCount, ref iTotalPageCount
                                                   );

        General.SetPrintOptions("gvERMBankAccMap", "ERM Bank Account Map", alCaptions, alColumns, ds);

        gvERMBankAccMap.DataSource = ds;
        gvERMBankAccMap.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvERMBankAccMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvERMBankAccMap.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Rebind()
    {
        gvERMBankAccMap.EditIndexes.Clear();
        gvERMBankAccMap.SelectedIndexes.Clear();
        gvERMBankAccMap.DataSource = null;
        gvERMBankAccMap.Rebind();
    }
    protected void gvERMBankAccMap_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   // protected void gvERMBankAccMap_RowCreated(object sender, GridViewRowEventArgs e)
   // {
   //     try
   //     {
   //         if (e.Row.RowType == DataControlRowType.DataRow
   //     && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
   //     && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
   //         {
   //             e.Row.TabIndex = -1;
   //             e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvERMBankAccMap, "Edit$" + e.Row.RowIndex.ToString(), false);
   //         }
   //     }
   //     catch (Exception ex)
   //     {
   //
   //         throw ex;
   //     }
   // }
   
    protected void gvERMBankAccMap_Sorting(object sender, GridSortCommandEventArgs e)
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
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOMPANYPREFIX", "FLDCOMPANYNAME", "FLDSUBACCOUNTID", "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDZID", "FLDCURRENCYCODE", "FLDMAPPEDERMCODE", "FLDXPAYACT", "FLDCOMPANYBANKCUR" };
        string[] alCaptions = { "Company Prefix", "Company Name", "Sub Account Id", "Sub Account", "Description", "ZID", "Currency Code", "Mapped ERM Code", "X Pay Act", "Company Bank Currency" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixERMBankAccMap.ERMBankAccMapSearch(subaccount.Text, CompanyName.Text, description.Text, (int)ViewState["PAGENUMBER"],
                                                   PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount
                                                  );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ERMBankAccountMap.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ERM Bank Account Map</h3></td>");
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
}
