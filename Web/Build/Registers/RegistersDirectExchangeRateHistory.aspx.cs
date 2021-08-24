using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersDirectExchangeRateHistory : PhoenixBasePage
{
    public int iUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Currency to USD", "DIRECT", ToolBarDirection.Left);
        toolbar1.AddButton("USD to Currency", "INDIRECT", ToolBarDirection.Left);
        MenuMain.AccessRights = this.ViewState;
        MenuMain.MenuList = toolbar1.Show();
        MenuMain.SelectedMenuIndex = 1;
        toolbar.AddFontAwesomeButton("../Registers/RegistersDirectExchangeRateHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('dgExchangerate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersDirectExchangeRateHistory.aspx", "<b>Find</b>", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersDirectExchangeRateHistory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuExchangeRate.AccessRights = this.ViewState;
        MenuExchangeRate.MenuList = toolbar.Show();
        //MenuExchangeRate.SetTrigger(pnlExchangeRate);

        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        if (!IsPostBack)
        {
            chkActive.Checked = true;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            dgExchangerate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void dgExchangerate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetPrintOptions(DataSet ds)
    {
        string[] alColumns = { "FLDCURRENCYNAME", "FLDCURRENCYCODE", "FLDEXCHANGERATE", "FLDEXCHANGERATEUSD", "FLDCHANGEPERCENTAGE", "FLDMODIFIEDUSERNAME" };
        string[] alCaptions = { "Currency", "Code", "USD to Currency", "Currency to USD", "Variance (%) ", "Modified By" };
        Session["PRINTHEADER"] = "Exchange Rate History";
        Session["PRINTCOLUMNS"] = alColumns;
        Session["PRINTCAPTIONS"] = alCaptions;
        Session["PRINTDATA"] = ds;
    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("INDIRECT"))
        {
            Response.Redirect("../Registers/RegistersDirectExchangeRateHistory.aspx");
        }
        else if (CommandName.ToUpper().Equals("DIRECT"))
        {
            Response.Redirect("../Registers/RegistersExchangeRateHistory.aspx");
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCURRENCYNAME", "FLDCURRENCYCODE", "FLDEXCHANGERATE", "FLDEXCHANGERATEUSD", "FLDCHANGEPERCENTAGE", "FLDMODIFIEDUSERNAME" };
        string[] alCaptions = { "Currency", "Code", "USD to Currency", "Currency to USD", "Variance (%) ", "Modified By" };
        string sortexpression;
        int? sortdirection = null;
        DateTime? dtFromDate = null;
        DateTime? dtToDate = null;

        if (txtFromdate.Text != null && txtFromdate.Text != string.Empty)
            dtFromDate = Convert.ToDateTime(txtFromdate.Text.ToString());
        if (txtTodate.Text != null && txtTodate.Text != string.Empty)
            dtToDate = Convert.ToDateTime(txtTodate.Text.ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int active = 0;
        if (chkActive.Checked == true)
        {
            active = 1;
        }

        ds = PhoenixAccountsExchangeRateHistory.ExchangeRateSearch(txtCurrencyName.Text, txtCurrencyCode.Text, dtFromDate, dtToDate, active, sortexpression, sortdirection,
            dgExchangerate.CurrentPageIndex + 1,
            dgExchangerate.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=ExchangeRates.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Exchange Rates</h3></td>");
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

    protected void ExchangeRate_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            dgExchangerate.SelectedIndexes.Clear();
            dgExchangerate.EditIndexes.Clear();
            dgExchangerate.DataSource = null;
            dgExchangerate.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtCurrencyCode.Text = "";
            txtCurrencyName.Text = "";
            txtFromdate.Text = "";
            txtTodate.Text = "";
            chkActive.Checked = true;
            dgExchangerate.SelectedIndexes.Clear();
            dgExchangerate.EditIndexes.Clear();
            dgExchangerate.DataSource = null;
            dgExchangerate.Rebind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DateTime? dtFromDate = null;
        DateTime? dtToDate = null;
        string[] alColumns = { "FLDCURRENCYNAME", "FLDCURRENCYCODE", "FLDEXCHANGERATE", "FLDEXCHANGERATEUSD", "FLDCHANGEPERCENTAGE", "FLDMODIFIEDUSERNAME" };
        string[] alCaptions = { "Currency", "Code", "USD to Currency", "Currency to USD", "Variance (%) ", "Modified By" };
        if (txtFromdate.Text != null && txtFromdate.Text != string.Empty)
            dtFromDate = Convert.ToDateTime(txtFromdate.Text.ToString());
        if (txtTodate.Text != null && txtTodate.Text != string.Empty)
            dtToDate = Convert.ToDateTime(txtTodate.Text.ToString());

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        int active = 0;
        if (chkActive.Checked == true)
        {
            active = 1;
        }
        DataSet ds = PhoenixAccountsExchangeRateHistory.ExchangeRateSearch(txtCurrencyName.Text.Trim(), txtCurrencyCode.Text.Trim(), dtFromDate, dtToDate, active, sortexpression, sortdirection,
            dgExchangerate.CurrentPageIndex + 1,
            dgExchangerate.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("dgExchangerate", "Exchange Rates", alCaptions, alColumns, ds);

        dgExchangerate.DataSource = ds;
        dgExchangerate.VirtualItemCount = iRowCount;
    }
    protected void dgExchangerate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;

        if (e.CommandName == RadGrid.InitInsertCommandName)
        {
            _gridView.MasterTableView.ClearEditItems();
        }

        if (e.CommandName == "EDIT")
        {
            e.Item.OwnerTableView.IsItemInserted = false;
        }

        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            string ExchangeRateId = ((RadLabel)e.Item.FindControl("RadlblExchangeRateId")).Text;

            if (ExchangeRateId == "")
            {
                string currencycode = ((UserControlCurrency)e.Item.FindControl("ddlCurrencyCode")).SelectedCurrency;
                string exchangerate = null;
                string exchangerateusd = ((RadTextBox)e.Item.FindControl("txtExchangerateEditUSD")).Text;
                int active = 1;

                if (!IsValidExchangeRate(currencycode, exchangerate, exchangerateusd))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsExchangeRateHistory.UpdateExchangeRate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(currencycode), DateTime.Now, General.GetNullableDecimal(exchangerate), General.GetNullableDecimal(exchangerateusd), active);
            }

            if (ExchangeRateId != "")
            {
                int iCurrencyId = int.Parse((((RadTextBox)e.Item.FindControl("txtCurrencyid")).Text).ToString());
                DateTime dtModifiedDate = DateTime.Now;
                string strExchangeRate = null;
                string exchangerateusd = ((RadTextBox)e.Item.FindControl("txtExchangerateEditUSD")).Text;
                int isactive = 1;

                if (!IsValidExchangeRate(iCurrencyId.ToString(), strExchangeRate, exchangerateusd))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsExchangeRateHistory.UpdateExchangeRate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , iCurrencyId, dtModifiedDate, General.GetNullableDecimal(strExchangeRate), General.GetNullableDecimal(exchangerateusd), isactive);
                ucStatus.Text = "Exchange Rate Saved Successfully.";
            }

            dgExchangerate.EditIndexes.Clear();
            dgExchangerate.SelectedIndexes.Clear();
            dgExchangerate.DataSource = null;
            dgExchangerate.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            dgExchangerate.DataSource = null;
            dgExchangerate.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("RegistersCurrencyExchangeRateHistory.aspx?qCurrencyid=" + e.CommandArgument);
        }
    }
    protected void dgExchangerate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            UserControlCurrency ddlCurrency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyCode");
            string CurrencyId = ((RadLabel)e.Item.FindControl("lblCurrencyId")).Text;
            if (CurrencyId != null)
            {
                ddlCurrency.SelectedCurrency = CurrencyId;
            }

            // Get the LinkButton control in the first cell
            ElasticButton _doubleClickButton = (ElasticButton)e.Item.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Item.Attributes["ondblclick"] = _jsDouble;

            RadLabel lb = (RadLabel)e.Item.FindControl("lblvarieschange");
            if (!lb.Text.Equals(""))
            {
                if (Double.Parse(lb.Text) < -10 || Double.Parse(lb.Text) > 10)
                {
                    lb.ForeColor = System.Drawing.Color.Red;
                }
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }
        if ((e.Item is GridEditableItem) && (e.Item.IsInEditMode) && (!e.Item.OwnerTableView.IsItemInserted))
        {
            GridEditableItem editItem = (GridEditableItem)e.Item;
            TableCell Code = (TableCell)editItem["Code"];
            Code.Enabled = false;
        }
    }
    private bool IsValidExchangeRate(string strCurrencycode, string strExchangeRate, string dirExchangerate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strCurrencycode.Trim().ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Currency is required.";

        if (dirExchangerate.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Exchange Rate is required.";

        }

        else if (General.GetNullableDecimal(dirExchangerate) == null)
        {
            ucError.ErrorMessage = "Invalied Exchange Rate.";
        }
        else if ((Convert.ToDecimal(dirExchangerate)) <= 0 || (Convert.ToDecimal(dirExchangerate)) >= 99999)
        {
            ucError.ErrorMessage = "Exchange Rate Between 0.0000000000000001 to 99999.";
        }

        return (!ucError.IsError);
    }
    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{
    //    dgExchangerate.DataSource = null;
    //    dgExchangerate.Rebind();
    //}
}
