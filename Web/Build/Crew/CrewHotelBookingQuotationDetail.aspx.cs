using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewHotelBookingQuotationDetail : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["BOOKINGID"] = null;
                ViewState["QUOTEID"] = null;
                ViewState["VESSELID"] = "";

                if (Request.QueryString["bookingid"] != null)
                {
                    ViewState["BOOKINGID"] = Request.QueryString["bookingid"].ToString();

                    DataTable dt = PhoenixCrewHotelBooking.HotelBookingEdit(General.GetNullableGuid(ViewState["BOOKINGID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
                    }
                }
                if (Request.QueryString["quoteid"] != null)
                {
                    ViewState["QUOTEID"] = Request.QueryString["quoteid"].ToString();
                    ViewState["SAVESTATUS"] = "EDIT";
                    BindData();
                }

                gvHBQuote.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixCrewHotelBookingQuote.HotelBookingQuoteSearch(
                General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
                , General.GetNullableGuid(ViewState["QUOTEID"] != null ? ViewState["QUOTEID"].ToString() : null)
                , null
                , null
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , gvHBQuote.PageSize
                , ref iRowCount, ref iTotalPageCount
                );

            gvHBQuote.DataSource = ds;
            gvHBQuote.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvHBQuote_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHBQuote.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvHBQuote_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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

    protected void gvHBQuote_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        string quoteid = ((RadLabel)e.Item.FindControl("lblQuoteIDEdit")).Text;
        string currency = ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency;
        string quotedrooms = ((UserControlMaskNumber)e.Item.FindControl("txtNoOfRoomsQuoteEdit")).Text;
        string quotedextrabeds = ((UserControlMaskNumber)e.Item.FindControl("txtExtraBedsQuoteEdit")).Text;
        string quotedextrabedsamount = ((UserControlMaskNumber)e.Item.FindControl("txtExtraBedAmountEdit")).Text;
        string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;
        string discount = ((UserControlMaskNumber)e.Item.FindControl("txtDiscountEdit")).Text;

        if (!IsValidQuote(quotedrooms, amount, currency))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixCrewHotelBookingQuote.UpdateHotelBookingQuote(
            General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
            , General.GetNullableGuid(quoteid)
            , General.GetNullableInteger(quotedrooms)
            , General.GetNullableInteger(quotedextrabeds)
            , General.GetNullableDecimal(amount)
            , General.GetNullableDecimal(quotedextrabedsamount)
            , General.GetNullableDecimal(discount)
            , General.GetNullableInteger(currency)
            );

        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        BindData();
        gvHBQuote.Rebind();

    }

    protected void gvHBQuote_DeleteCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvHBQuote_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.IsInEditMode)
        {
            UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrencyEdit");
            if (ucCurrency != null)
            {
                ucCurrency.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , true);
                ucCurrency.DataBind();

                DataRowView dr = (DataRowView)e.Item.DataItem;
                if (dr != null)
                    ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            }

        }
    }

    private bool IsValidQuote(string noofrooms, string amount, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(noofrooms) == null)
            ucError.ErrorMessage = "No of room required";

        if (General.GetNullableDecimal(amount) == null)
            ucError.ErrorMessage = "Amount required";

        if (General.GetNullableInteger(currency) == null)
            ucError.ErrorMessage = "Currency required";

        return (!ucError.IsError);
    }


    private void BindDataTax()
    {

        DataTable dt = new DataTable();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        dt = PhoenixCrewHotelBookingQuoteTaxMap.HotelBookingQuoteTaxMapList(new Guid(ViewState["BOOKINGID"].ToString())
            , (ViewState["QUOTEID"] != null ? General.GetNullableGuid(ViewState["QUOTEID"].ToString()) : null)
        );

        gvTax.DataSource = dt;
       
    }

    protected void gvTax_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataTax();
    }

   

    protected void gvTax_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            if (!IsValidTax(
                ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text.ToString().Trim(),
                ((UserControlTaxType)e.Item.FindControl("ucTaxTypeAdd")).TaxType.ToString(),
                ((UserControlDecimal)e.Item.FindControl("txtValueAdd")).Text.ToString()))
            {
                ucError.Visible = true;
                return;
            }

            InsertQuotationTax(
                                (ViewState["QUOTEID"] != null ? General.GetNullableGuid(ViewState["QUOTEID"].ToString()) : null)
                                , ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text.ToString().Trim()
                                , int.Parse(((UserControlTaxType)e.Item.FindControl("ucTaxTypeAdd")).TaxType)
                                , decimal.Parse(((UserControlDecimal)e.Item.FindControl("txtValueAdd")).Text.ToString())
                                , ((RadTextBox)e.Item.FindControl("txtBudgetCodeAdd")).Text.ToString().Trim()
                                , ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdAdd")).Text.ToString().Trim()
                            );
            BindDataTax();
            gvTax.Rebind();

            String script = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    protected void gvTax_UpdateCommand(object sender, GridCommandEventArgs e)
    {

        if (!IsValidTax(((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.ToString().Trim(),
          ((UserControlTaxType)e.Item.FindControl("ucTaxTypeEdit")).TaxType.ToString(),
          ((UserControlDecimal)e.Item.FindControl("txtValueEdit")).Text.ToString()))
        {
            ucError.Visible = true;
            return;
        }

        UpdateQuotationTax(
                             new Guid(((RadTextBox)e.Item.FindControl("txtTaxMapIdEdit")).Text.ToString())
                            , (ViewState["QUOTEID"] != null ? General.GetNullableGuid(ViewState["QUOTEID"].ToString()) : null)
                            , ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.ToString().Trim()
                            , int.Parse(((UserControlTaxType)e.Item.FindControl("ucTaxTypeEdit")).TaxType)
                            , decimal.Parse(((UserControlDecimal)e.Item.FindControl("txtValueEdit")).Text.ToString())
                            , ((RadTextBox)e.Item.FindControl("txtBudgetCodeEdit")).Text.ToString().Trim()
                            , ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text.ToString().Trim()
                        );
      
        BindDataTax();
        gvTax.Rebind();

        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void gvTax_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        DeleteQuotationTax(new Guid(((RadTextBox)e.Item.FindControl("txtTaxMapId")).Text.ToString())
                             , new Guid(ViewState["QUOTEID"].ToString()));

        BindDataTax();
        gvTax.Rebind();

        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void gvTax_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.Footer)
        {
          
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            

        }
    }

    protected void gvTax_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            
        }

        if (e.Item.IsInEditMode)
        {
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            LinkButton ib = (LinkButton)e.Item.FindControl("btnShowBudgetEdit");
            LinkButton ib3 = (LinkButton)e.Item.FindControl("btnShowOwnerBudgetEdit");

            if (ib != null) ib.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?', true); ");

            if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) != null)
            {
                if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) > 0)
                    if (ib3 != null) ib3.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
            }

            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
           
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            RadTextBox tb = (RadTextBox)e.Item.FindControl("txtBudgetName");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtBudgetId");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtBudgetgroupId");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            LinkButton ib = (LinkButton)e.Item.FindControl("btnShowBudget");
            LinkButton ib3 = (LinkButton)e.Item.FindControl("btnShowOwnerBudgetAdd");

            if (ib != null) ib.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?', true); ");

            if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) != null)
            {
                if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) > 0)
                    if (ib3 != null) ib3.Attributes.Add("onclick", "return showPickList('spnPickListOwnerTaxBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
            }
            tb = (RadTextBox)e.Item.FindControl("txtBudgetOwnerNameAdd");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdAdd");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdAdd");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");

        }

    }

    protected void InsertQuotationTax(Guid? quoteid, string strDescription, int iTaxType, decimal dValue, string strBudgetCode, string ownerbudgetcode)
    {
        try
        {
            PhoenixCrewHotelBookingQuoteTaxMap.InsertHotelBookingQuoteTaxMap(new Guid(ViewState["BOOKINGID"].ToString())
                , quoteid
                , strDescription
                , iTaxType
                , dValue
                , General.GetNullableString(strBudgetCode)
                , General.GetNullableGuid(ownerbudgetcode));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateQuotationTax(Guid uTaxMapId, Guid? uQuoteId, string strDescription, int iTaxType, decimal dValue, string strBudgetCode, string ownerbudgetcode)
    {
        try
        {
            PhoenixCrewHotelBookingQuoteTaxMap.UpdateHotelBookingQuoteTaxMap(
                  new Guid(ViewState["BOOKINGID"].ToString())
                , uQuoteId
                , uTaxMapId
                , strDescription
                , iTaxType
                , dValue
                , General.GetNullableString(strBudgetCode)
                , General.GetNullableGuid(ownerbudgetcode));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void DeleteQuotationTax(Guid uTaxMapCode, Guid uQuotationId)
    {
        PhoenixCrewHotelBookingQuoteTaxMap.DeleteHotelBookingQuoteTaxMap(new Guid(ViewState["BOOKINGID"].ToString())
            , uQuotationId
            , uTaxMapCode
            );
    }

    private bool IsValidHBQuote(string hotelid, string qtnno, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(hotelid) == null)
        {
            ucError.ErrorMessage = "Hotel Required";
        }
        if (General.GetNullableString(qtnno) == null)
        {
            ucError.ErrorMessage = "Quotation No. Required";
        }
        if (General.GetNullableInteger(currency) == null)
        {
            ucError.ErrorMessage = "Currency Required";
        }
        return (!ucError.IsError);
    }
    protected bool IsValidTax(string strDescription, string strValueType, string strValue)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strDescription.Trim() == string.Empty)
            ucError.ErrorMessage = "Description is required.";
        if (strValue.Trim() == string.Empty)
            ucError.ErrorMessage = "Value is required.";
        if (strValueType.Trim() == string.Empty)
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }


}
