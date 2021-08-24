using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsCurrencyConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCurrencyConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCurrencyConf')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCurrencyConfiguration.aspx", "Find", "<i class=\"fas fa-search\"></i>", "Find");
            MenuPhoneReq.AccessRights = this.ViewState;
            MenuPhoneReq.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                chkactiveyn.Checked = true;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvCurrencyConf.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            string[] alColumns = { "FLDCURRENCYCODE", "FLDACTIVEYNDESC" };
            string[] alCaptions = { "Currency", "ActiveYN" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.SearchCurrencyConfiguration(General.GetNullableString(txtCurrencyCode.Text.Trim())
                             , PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(chkactiveyn.Checked == true ? "1" : "0")
                             , sortexpression, sortdirection
                             , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                             , gvCurrencyConf.PageSize, ref iRowCount, ref iTotalPageCount);
            string title = "Currency Configuration.";
            General.SetPrintOptions("gvCurrencyConf", title, alCaptions, alColumns, ds);
            gvCurrencyConf.DataSource = ds;
            gvCurrencyConf.VirtualItemCount = iRowCount;
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCURRENCYCODE", "FLDACTIVEYNDESC" };
            string[] alCaptions = { "Currency", "ActiveYN" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


            DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.SearchCurrencyConfiguration(General.GetNullableString(txtCurrencyCode.Text.Trim())
                             , PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(chkactiveyn.Checked == true ? "1" : "0")
                             , sortexpression, sortdirection
                             , 1, iRowCount, ref iRowCount, ref iTotalPageCount);
            string title = "Phone Card Configuration ";
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCurrencyConf.SelectedIndexes.Clear();
        gvCurrencyConf.EditIndexes.Clear();
        gvCurrencyConf.DataSource = null;
        gvCurrencyConf.Rebind();
    }
    protected void MenuPhoneReq_TabStripCommand(object sender, EventArgs e)
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
            else if (CommandName.ToUpper().Equals("EXCEL"))
                ShowExcel();
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
    private bool IsValidPhoneCard(string Currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (Currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required";
        return (!ucError.IsError);
    }
    protected void gvCurrencyConf_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            UserControlCurrency currency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit");
            if (currency != null)
            {
                currency.CurrencyList = PhoenixRegistersCurrency.ListCurrency(1);
                currency.SelectedCurrency = drv["FLDCURRENCYID"].ToString();
            }
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
    protected void gvCurrencyConf_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Currencyid = ((UserControlCurrency)e.Item.FindControl("ddlCurrencyAdd")).SelectedCurrency;
                int Activeyn = int.Parse(((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked == true ? "1" : "0");
                decimal? CCround = General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtCCRoundoffAdd")).Text);
                decimal? Bondround = General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtBondRoundoffAdd")).Text);
                if (!IsValidPhoneCard(Currencyid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCurrencyConfiguration.InsertCurrencyConfiguration(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                         , int.Parse(Currencyid), Activeyn, Bondround, CCround);

                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadLabel lblCrewComp = (RadLabel)e.Item.FindControl("lblCurrencyConf");
                string Currencyid = ((UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit")).SelectedCurrency;
                decimal? CCround = General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtCCRoundoffEdit")).Text);
                decimal? Bondround = General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtBondRoundoffEdit")).Text);
                int Activeyn = int.Parse(((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true ? "1" : "0");
                if (!IsValidPhoneCard(Currencyid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCurrencyConfiguration.UpdateCurrencyConfiguration(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                         , int.Parse(Currencyid), Activeyn, new Guid(lblCrewComp.Text), Bondround, CCround);
                Rebind();
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
    protected void gvCurrencyConf_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCurrencyConf.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}