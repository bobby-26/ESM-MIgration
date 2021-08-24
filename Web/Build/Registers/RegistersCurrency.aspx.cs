using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class RegistersCurrency : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCurrency.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCurrency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCurrency.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCurrency.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersCurrency.AccessRights = this.ViewState;
            MenuRegistersCurrency.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                chkActive.Checked = true;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCurrency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int active;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCURRENCYCODE", "FLDCURRENCYNAME", "FLDCOUNTRYNAME", "FLDEXCHANGERATEUSD", "FLDMODIFIEDDATE", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Country", "BaseLine Exchange rate (Indirect rate to 1 USD)", "Exchange rate date", "Active Y/N" };
        string sortexpression;
        int? sortdirection = null;
        if (chkActive.Checked == true)
        {
            active = 1;
        }
        else
        {
            active = 0;
        }
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCurrency.CurrencySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtCurrencyCode.Text, txtSearch.Text, active, sortexpression, sortdirection,
        1,
        iRowCount,
        ref iRowCount,
        ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Currency.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Currency</h3></td>");
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


    protected void RegistersCurrency_TabStripCommand(object sender, EventArgs e)
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
                txtSearch.Text = string.Empty;
                txtCurrencyCode.Text = string.Empty;
                chkActive.Checked = true;
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
        gvCurrency.SelectedIndexes.Clear();
        gvCurrency.EditIndexes.Clear();
        gvCurrency.DataSource = null;
        gvCurrency.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int active;

        string[] alColumns = { "FLDCURRENCYCODE", "FLDCURRENCYNAME", "FLDCOUNTRYNAME", "FLDEXCHANGERATEUSD", "FLDMODIFIEDDATE", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Country", "BaseLine Exchange rate (Indirect rate to 1 USD)", "Exchange rate date", "Active Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (chkActive.Checked == true)
        {
            active = 1;
        }
        else
        {
            active = 0;
        }
        DataSet ds = PhoenixRegistersCurrency.CurrencySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtCurrencyCode.Text, txtSearch.Text, active, sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCurrency.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvCurrency", "Currency", alCaptions, alColumns, ds);

        gvCurrency.DataSource = ds;
        gvCurrency.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvCurrency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCurrency(((RadTextBox)e.Item.FindControl("txtCurrencyCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtCurrencyNameAdd")).Text,
                    ((UserControlMaskedTextBox)e.Item.FindControl("txtExchangerateAddUSD")).TextWithLiterals,
                    ((UserControlDate)e.Item.FindControl("txtModifiedDateAdd")).Text,
                    ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).SelectedCountry
                ))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertCurrency(
                    ((RadTextBox)e.Item.FindControl("txtCurrencyCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtCurrencyNameAdd")).Text,
                    (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0,
                    ((UserControlMaskedTextBox)e.Item.FindControl("txtExchangerateAddUSD")).TextWithLiterals,
                    ((UserControlDate)e.Item.FindControl("txtModifiedDateAdd")).Text,
                    ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).SelectedCountry
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtCurrencyCodeAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCurrency(((RadTextBox)e.Item.FindControl("txtCurrencyCodeEdit")).Text,
                   ((RadTextBox)e.Item.FindControl("txtCurrencyNameEdit")).Text,
                   ((UserControlCountry)e.Item.FindControl("ucCountryEdit")).SelectedCountry))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdateCurrency(
                     Int32.Parse(((RadLabel)e.Item.FindControl("lblCurrencyIDEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtCurrencyCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtCurrencyNameEdit")).Text,
                     (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked.Equals(true)) ? 1 : 0,
                     ((UserControlCountry)e.Item.FindControl("ucCountryEdit")).SelectedCountry
                 );
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["CurrencyID"] = ((RadLabel)e.Item.FindControl("lblCurrencyID")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
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
    protected void gvCurrency_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item.IsInEditMode)
        {
            UserControlCountry ucCountry = (UserControlCountry)e.Item.FindControl("ucCountryEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCountry != null) ucCountry.SelectedCountry = DataBinder.Eval(e.Item.DataItem, "FLDCOUNTRYCODE").ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    private void InsertCurrency(string Currencycode, string Currencyname, int? activeyn, string strExchangerate, string dtExchangerateDate, string countrycode)
    {
        PhoenixRegistersCurrency.InsertCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                Currencycode, Currencyname, activeyn, decimal.Parse(strExchangerate), DateTime.Parse(dtExchangerateDate), General.GetNullableInteger(countrycode));
    }
    private void UpdateCurrency(int Currencyid, string Currencycode, string Currencyname, int? activeyn, string countrycode)
    {
        PhoenixRegistersCurrency.UpdateCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                Currencyid, Currencycode, Currencyname, activeyn, General.GetNullableInteger(countrycode));
        ucStatus.Text = "Currency information updated";
    }

    private bool IsValidCurrency(string Currencycode, string Currencyname, string countrycode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvCurrency;

        if (Currencyname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (Currencycode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (countrycode.Equals("Dummy") || countrycode.Equals(""))
            ucError.ErrorMessage = "Country is required.";

        return (!ucError.IsError);
    }


    private bool IsValidCurrency(string Currencycode, string Currencyname, string Exchangerate, string ExchangerateDate, string countrycode)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (ExchangerateDate == null)
            ExchangerateDate = "";
        RadGrid _gridView = gvCurrency;

        if (Currencyname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (Currencycode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (Exchangerate.Trim().Equals(""))
            ucError.ErrorMessage = "Exchange rate is required.";

        if (ExchangerateDate.Trim().Equals(""))
            ucError.ErrorMessage = "Exchange rate date is required.";

        if (countrycode.Equals("Dummy") || countrycode.Equals("") )
            ucError.ErrorMessage = "Country is required.";

        return (!ucError.IsError);
    }
    private void DeleteCurrency(int Currencycode)
    {
        PhoenixRegistersCurrency.DeleteCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Currencycode);
    }

    protected void gvCurrency_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCurrency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCurrency.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteCurrency(Int32.Parse(ViewState["CurrencyID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
