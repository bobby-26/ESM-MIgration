using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class RegistersCurrencyExchangeRateHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuExchangeRateHistory.AccessRights = this.ViewState;
        MenuExchangeRateHistory.MenuList = toolbar1.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersCurrencyExchangeRateHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('dgExchangerate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersCurrencyExchangeRateHistory.aspx", "<b>Find</b>", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersCurrencyExchangeRateHistory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuExchangeRate.AccessRights = this.ViewState;
        MenuExchangeRate.MenuList = toolbar.Show();
        //MenuExchangeRate.SetTrigger(pnlExchangeRate);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (Request.QueryString["qCurrencyid"] != null && Request.QueryString["qCurrencyid"] != string.Empty)
                ViewState["CURRENCYID"] = Request.QueryString["qCurrencyid"];

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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEXCHANGERATE", "FLDEXCHANGERATEUSD", "FLDCHANGEPERCENTAGE", "FLDMODIFIEDUSERNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Indirect Rate to USD", "Direct Rate to USD", "Variance (%) ", "Modified By", "Modified Date" };
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
        ds = PhoenixAccountsExchangeRateHistory.CurrencyExchangeRateHistorySearch(int.Parse(ViewState["CURRENCYID"].ToString()), dtFromDate, dtToDate, sortexpression, sortdirection,
            dgExchangerate.CurrentPageIndex + 1,
            dgExchangerate.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename= Currency_ExchangeRate_History.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Currency Exchange Rate History</h3></td>");

        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");

        Response.Write("<tr>");
        Response.Write("<td><h5>Currency Name: " + ds.Tables[0].Rows[0]["FLDCURRENCYNAME"] + "</h5> </td>");
        Response.Write("<td><h5>Currency Code: " + ds.Tables[0].Rows[0]["FLDCURRENCYCODE"] + "</h5> </td>");

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
            dgExchangerate.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtFromdate.Text = "";
            txtTodate.Text = "";
            dgExchangerate.EditIndexes.Clear();
            dgExchangerate.SelectedIndexes.Clear();
            dgExchangerate.DataSource = null;
            dgExchangerate.Rebind();
        }
    }

    protected void ExchangeRateHistory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.Equals("BACK"))
            Response.Redirect("RegistersExchangeRateHistory.aspx");
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DateTime? dtFromDate = null;
        DateTime? dtToDate = null;

        string[] alColumns = { "FLDEXCHANGERATE", "FLDEXCHANGERATEUSD", "FLDCHANGEPERCENTAGE", "FLDMODIFIEDUSERNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Indirect Rate to USD", "Direct Rate to USD", "Variance (%) ", "Modified By","Modified Date" };

        if (txtFromdate.Text != null && txtFromdate.Text != string.Empty)
            dtFromDate = Convert.ToDateTime(txtFromdate.Text.ToString());
        if (txtTodate.Text != null && txtTodate.Text != string.Empty)
            dtToDate = Convert.ToDateTime(txtTodate.Text.ToString());

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixAccountsExchangeRateHistory.CurrencyExchangeRateHistorySearch(int.Parse(ViewState["CURRENCYID"].ToString()), dtFromDate, dtToDate, sortexpression, sortdirection,
            dgExchangerate.CurrentPageIndex + 1,
            dgExchangerate.PageSize,
            ref iRowCount,
            ref iTotalPageCount);



        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCurrencyName.Text = ds.Tables[0].Rows[0]["FLDCURRENCYNAME"].ToString();
            txtCurrencyCode.Text = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
            txtCurrencyName.Enabled = false;
            txtCurrencyCode.Enabled = false;
            dgExchangerate.DataSource = ds;
            dgExchangerate.VirtualItemCount = iRowCount;
        }

        General.SetPrintOptions("dgExchangerate", "" + " Currency Exchange Rate History" + "-" + txtCurrencyName.Text + " (" + txtCurrencyCode.Text + ") ", alCaptions, alColumns, ds);
    }
    protected void dgExchangerate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        RadGrid _gridView = (RadGrid)sender;
        GridEditableItem eeditedItem = e.Item as GridEditableItem;

        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            UpdateExchangeRate(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 ((RadTextBox)eeditedItem.FindControl("txtExchangeRateHistoryId")).Text,
                DateTime.Now,
                ((UserControlDecimal)eeditedItem.FindControl("txtExchangerateEdit")).Text,
                 ((UserControlDecimal)eeditedItem.FindControl("txtExchangerateEditUSD")).Text, 1
             //(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYNedit")).Checked) ? 1 : 0
             );

            dgExchangerate.EditIndexes.Clear();
            dgExchangerate.SelectedIndexes.Clear();
            dgExchangerate.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            dgExchangerate.Rebind();
        }
        else
        {
            dgExchangerate.EditIndexes.Clear();
            dgExchangerate.SelectedIndexes.Clear();
            dgExchangerate.Rebind();
        }
    }
    protected void dgExchangerate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            // Get the LinkButton control in the first cell
            ElasticButton _doubleClickButton = (ElasticButton)e.Item.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Item.Attributes["ondblclick"] = _jsDouble;
            UserControlDecimal tb = (UserControlDecimal)e.Item.FindControl("txtExchangerateEdit");
            UserControlDecimal tb1 = (UserControlDecimal)e.Item.FindControl("txtExchangerateEditUSD");
            if (tb != null)
                tb.Attributes.Add("onblur", "return fnPostToWebFunction('ConvertExchangeRate','spnDirectExchangeEdit','spnInDirectExchangeEdit'); ");
            if (tb1 != null)
                tb1.Attributes.Add("onblur", "return fnPostToWebFunction('ConvertExchangeRate','spnInDirectExchangeEdit','spnDirectExchangeEdit');");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblvarieschange");
            if (!lb.Text.Equals(""))
            {
                if (Double.Parse(lb.Text) < -10 || Double.Parse(lb.Text) > 10)
                {
                    lb.ForeColor = System.Drawing.Color.Red;
                }
            }
            RadImageButton eb = (RadImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            RadImageButton sb = (RadImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            RadImageButton cb = (RadImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }
    private void UpdateExchangeRate(int iRowuserCode, string gExchangeRateId, DateTime dtModifiedDate, string strExchangeRate, string strExchangeRateUsd, int isactive)
    {
        if (!IsValidExchangeRate(strExchangeRate))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixAccountsExchangeRateHistory.UpdateCurrencyExchangeRate(iRowuserCode, int.Parse(ViewState["CURRENCYID"].ToString()), dtModifiedDate, decimal.Parse(strExchangeRate), decimal.Parse(strExchangeRateUsd), isactive);
    }

    private bool IsValidExchangeRate(string strExchangeRate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = dgExchangerate;

        if (strExchangeRate.Trim().Equals(""))
            ucError.ErrorMessage = "Exchange Rate is required.";
        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
