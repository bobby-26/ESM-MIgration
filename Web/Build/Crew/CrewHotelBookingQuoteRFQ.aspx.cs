using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;
public partial class CrewHotelBookingQuoteRFQ : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            CheckWebSessionStatus();

            if ((Request.QueryString["VIEWONLY"] == null) && (ViewState["WEBSESSIONSTATUS"].ToString() == "Y"))
            {
                toolbarmain.AddButton("Send to Office", "CONFIRM", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

                MenuQuote.MenuList = toolbarmain.Show();
            }
            else
            {
                toolbarmain.AddButton("Send to Office", "CONFIRM", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

                MenuQuote.MenuList = toolbarmain.Show();

            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingQuoteRFQ.aspx?SESSIONID=" + Request.QueryString["SESSIONID"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvHBQuote')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            if (!IsPostBack)
            {
                btnconfirm.Attributes.Add("style", "display:none");

                ViewState["QTNNO"] = null;

                if (Request.QueryString["SESSIONID"] != null)
                {
                    ViewState["QUOTEID"] = Request.QueryString["SESSIONID"].ToString();
                    if (ViewState["QUOTEID"] != null)
                        ListHotelBookingRequest(General.GetNullableGuid(ViewState["QUOTEID"].ToString()));
                }
                else
                {
                    ViewState["QUOTEID"] = "";
                }
           
                BindData();
            }
   
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuQuote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string quotedrooms = txtNoOfRoomsQuote.Text;
                string quotedextrabeds = txtExtraBedsQuote.Text;
                string quotedextrabedsamount = txtExtraBedAmount.Text;
                string amount = txtAmount.Text;
                string discount = txtDiscount.Text;
                string currency = ucCurrency.SelectedCurrency;

                if (!IsValidQuote(quotedrooms, amount, currency))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewHotelBookingQuote.UpdateHotelBookingQuoteByAgent(
                    General.GetNullableGuid(ViewState["QUOTEID"].ToString())
                    , General.GetNullableInteger(quotedrooms)
                    , General.GetNullableInteger(quotedextrabeds)
                    , General.GetNullableDecimal(amount)
                    , General.GetNullableDecimal(quotedextrabedsamount)
                    , General.GetNullableDecimal(discount)
                    , General.GetNullableInteger(currency)
                    );

                BindData();
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                RadWindowManager1.RadConfirm("Are you sure to want to confirm. Please Click Yes To Continue?", "btnconfirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void Confirm_Click(object sender, EventArgs e)
    {
        try
        {

            PhoenixCrewHotelBookingQuote.FinalizeHotelBookingQuoteForAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["QUOTEID"].ToString()));
            PhoenixCommoneProcessing.CloseUserSession(new Guid(ViewState["QUOTEID"].ToString()));
            QuoteConfirmationSent();
            CheckWebSessionStatus();
            ucStatus.Text = "Quotation is confirmed";
            Response.Redirect("..\\Crew\\CrewHotelBookingQuoteConfirmation.aspx");

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

            ds = PhoenixCrewHotelBookingQuote.HotelBookingQuoteFromAgentSearch(
                   General.GetNullableGuid(ViewState["QUOTEID"] != null ? ViewState["QUOTEID"].ToString() : null)
                , null, null, 1, 100, ref iRowCount, ref iTotalPageCount
                );

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtNoOfRoomsQuote.Text = dr["FLDQUOTEDNOOFROOMS"].ToString();
                txtExtraBedsQuote.Text = dr["FLDQUOTEDEXTRABEDS"].ToString();
                ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
                txtAmount.Text = dr["FLDAMOUNT"].ToString();
                txtExtraBedAmount.Text = dr["FLDEXTRABEDSAMOUNT"].ToString();
                txtDiscount.Text = dr["FLDDISCOUNT"].ToString();
                txtTotalAmount.Text = dr["FLDTOTALAMOUNT"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ListHotelBookingRequest(Guid? quoteid)
    {
        DataSet ds = PhoenixCrewHotelBookingQuote.ListHotelBookingRequestForAgent(quoteid);
        DataTable dt = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtRoomType.Text = dt.Rows[0]["FLDROOMTYPENAME"].ToString();
            txtNoOfBeds.Text = dt.Rows[0]["FLDNOOFBEDS"].ToString();
            txtNoOfRooms.Text = dt.Rows[0]["FLDNOOFROOMS"].ToString();
            txtExtraBeds.Text = dt.Rows[0]["FLDEXTRABEDS"].ToString();
            txtCheckinDate.Text = dt.Rows[0]["FLDCHECKINDATE"].ToString();
            txtCheckoutDate.Text = dt.Rows[0]["FLDCHECKOUTDATE"].ToString();
            txtNoOfNights.Text = dt.Rows[0]["FLDNOOFNIGHTS"].ToString();
            txtSenderName.Text = dt.Rows[0]["FLDSENDERNAME"].ToString();
            txtSenderMail.Text = dt.Rows[0]["FLDSENDEREMAIL"].ToString();
            txtTimeOfCheckIn.Text = dt.Rows[0]["FLDCHECKINDATETIME"].ToString();
            txtTimeOfCheckOut.Text = dt.Rows[0]["FLDCHECKOUTDATETIME"].ToString();
            if (dt.Rows[0]["FLDDAYUSEONLYYN"].ToString() == "1")
                txtDayUseYN.Text = "Yes";
            else
                txtDayUseYN.Text = "No";
        }
    }

    private bool IsValidQuote(string noofrooms, string amount, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(noofrooms) == null)
            ucError.ErrorMessage = "No of rooms is required";

        if (General.GetNullableInteger(currency) == null)
            ucError.ErrorMessage = "Currency is required";

        if (General.GetNullableDecimal(amount) == null)
            ucError.ErrorMessage = "Amount is required";

        return (!ucError.IsError);
    }

    private void BindDataTax()
    {

        DataTable dt = new DataTable();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        dt = PhoenixCrewHotelBookingQuoteTaxMap.HotelBookingQuoteTaxMapForAgentList((ViewState["QUOTEID"] != null ? General.GetNullableGuid(ViewState["QUOTEID"].ToString()) : null)
        );

        gvTax.DataSource = dt;

    }


    protected void gvTax_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataTax();
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
    }

    protected void gvTax_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
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
                                );
                BindDataTax();
                gvTax.Rebind();

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTax_UpdateCommand(object sender, GridCommandEventArgs e)
    {

        if (!IsValidTax(
          ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.ToString().Trim(),
          ((UserControlTaxType)e.Item.FindControl("ucTaxTypeEdit")).TaxType.ToString(),
          ((UserControlDecimal)e.Item.FindControl("txtValueEdit")).Text.ToString()))
        {
            ucError.Visible = true;
            return;
        }

        UpdateQuotationTax(
                              new Guid(((RadTextBox)e.Item.FindControl("txtTaxMapIdEdit")).Text.ToString())
                            , new Guid(((RadLabel)e.Item.FindControl("lblBookingIdEdit")).Text.ToString())
                            , (ViewState["QUOTEID"] != null ? General.GetNullableGuid(ViewState["QUOTEID"].ToString()) : null)
                            , ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.ToString().Trim()
                            , int.Parse(((UserControlTaxType)e.Item.FindControl("ucTaxTypeEdit")).TaxType)
                            , decimal.Parse(((UserControlDecimal)e.Item.FindControl("txtValueEdit")).Text.ToString())
                        );
        BindDataTax();
        gvTax.Rebind();
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void gvTax_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        DeleteQuotationTax(new Guid(((RadLabel)e.Item.FindControl("lblBookingId")).Text.ToString())
                   , new Guid(((RadTextBox)e.Item.FindControl("txtTaxMapId")).Text.ToString())
                   , new Guid(ViewState["QUOTEID"].ToString()));

        BindDataTax();
        gvTax.Rebind();
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    
    protected void InsertQuotationTax(Guid? quoteid, string strDescription, int iTaxType, decimal dValue)
    {
        try
        {
            PhoenixCrewHotelBookingQuoteTaxMap.InsertHotelBookingQuoteTaxMapByAgent(quoteid
                , strDescription
                , iTaxType
                , dValue);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateQuotationTax(Guid uTaxMapId, Guid bookingid, Guid? uQuoteId, string strDescription, int iTaxType, decimal dValue)
    {
        try
        {
            PhoenixCrewHotelBookingQuoteTaxMap.UpdateHotelBookingQuoteTaxMapByAgent(
                  bookingid
                , uQuoteId
                , uTaxMapId
                , strDescription
                , iTaxType
                , dValue);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void DeleteQuotationTax(Guid bookingid, Guid uTaxMapCode, Guid uQuotationId)
    {
        PhoenixCrewHotelBookingQuoteTaxMap.DeleteHotelBookingQuoteTaxMap(bookingid
            , uQuotationId
            , uTaxMapCode
            );
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
    private void CheckWebSessionStatus()
    {
        DataTable dt = PhoenixCommoneProcessing.GetSessionStatus(new Guid(Request.QueryString["SESSIONID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["WEBSESSIONSTATUS"] = dt.Rows[0]["FLDACTIVE"].ToString();
        }
    }
    private void QuoteConfirmationSent()
    {
        string emailbodytext = "";
        DataSet ds = new DataSet();
        ds = PhoenixCrewHotelBookingQuote.QuotationAgentEmailDetails(General.GetNullableGuid(ViewState["QUOTEID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            emailbodytext = PrepareApprovalText(ds.Tables[0]);
            DataRow dr = ds.Tables[0].Rows[0];
            PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                null,
                null,
                dr["FLDSUBJECT"].ToString(),
                emailbodytext,
                true,
                System.Net.Mail.MailPriority.Normal,
                "", null,
                null);
        }
    }
    protected string PrepareApprovalText(DataTable dt)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Purchaser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Quotation is Received from  " + dr["FLDNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");

        return sbemailbody.ToString();

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }


}
