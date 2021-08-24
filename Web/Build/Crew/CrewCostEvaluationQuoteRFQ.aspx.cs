using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Web;

public partial class CrewCostEvaluationQuoteRFQ : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            CheckWebSessionStatus();

            if (!IsPostBack)
            {
                btnconfirm.Attributes.Add("style", "display:none");

                ViewState["QTNNO"] = null;
                ViewState["REQUESTID"] = null;
                ViewState["EVALUATIONPORTID"] = null;
                ViewState["APPROVEDYN"] = "0";
                ViewState["Title"] = "";

                if (Request.QueryString["SESSIONID"] != null)
                {
                    ViewState["QUOTEID"] = Request.QueryString["SESSIONID"].ToString();
                    DataTable dt = PhoenixCrewCostEvaluationQuote.EditCrewCostRequestid(new Guid(ViewState["QUOTEID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["REQUESTID"] = dt.Rows[0]["FLDREQUESTID"].ToString();
                        ViewState["Title"] = "Quotation Details (" + dt.Rows[0]["FLDAGENTNAME"].ToString() + ")";
                        ViewState["APPROVEDYN"] = dt.Rows[0]["FLDQUOTEAPPROVED"].ToString();
                    }

                    if ((Request.QueryString["VIEWONLY"] == null) && (ViewState["WEBSESSIONSTATUS"].ToString() == "Y"))
                    {

                        toolbarmain.AddButton("Send to Office", "CONFIRM", ToolBarDirection.Right);
                        MenuQuote.MenuList = toolbarmain.Show();
                    }
                    else
                    {
                        toolbarmain.AddButton("Send to Office", "CONFIRM", ToolBarDirection.Right);
                        MenuQuote.AccessRights = this.ViewState;
                        MenuQuote.Title = ViewState["Title"].ToString();
                        MenuQuote.MenuList = toolbarmain.Show();
                    }
                    SetInfromation();
                }
                else
                {
                    ViewState["QUOTEID"] = "";
                }

                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TOTALPAGECOUNT"] = null;

                ViewState["QPAGENUMBER"] = 1;
                ViewState["QSORTEXPRESSION"] = null;
                ViewState["QSORTDIRECTION"] = null;

                ViewState["CURRENTINDEX"] = 1;

            }
            if (ViewState["REQUESTID"] != null)
            {
                BindQuoteDetails();
                BindQuoteLineItem();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetInfromation()
    {
        DataTable dt = PhoenixCrewCostEvaluationRequest.EditCrewCostEvaluationRequest(new Guid(ViewState["REQUESTID"].ToString()));
        DataRow dr = dt.Rows[0];
        txtVessel.Text = dr["FLDVESSELNAME"].ToString();
        txtRequestNo.Text = dr["FLDREQUESTNO"].ToString();
        txtOnSigners.Text = dr["FLDNOOFJOINER"].ToString();
        txtOffSigners.Text = dr["FLDNOOFOFFSIGNER"].ToString();
    }

    protected void MenuQuote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                PhoenixCrewCostEvaluationQuote.CheckCrewCostQuoteForAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , new Guid(ViewState["REQUESTID"].ToString())
               , new Guid(ViewState["QUOTEID"].ToString())
               );

                RadWindowManager1.RadConfirm("Are you sure to want to confirm?. Please Click Yes to Continue", "btnconfirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewCostEvaluationQuote.FinalizeCrewCostQuoteForAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(ViewState["REQUESTID"].ToString())
            , new Guid(ViewState["QUOTEID"].ToString())
           );

            PhoenixCommoneProcessing.CloseUserSession(new Guid(ViewState["QUOTEID"].ToString()));

            ucStatus.Text = "Quotation is confirmed";
            Response.Redirect("..\\Crew\\CrewCostQuoteConfirmation.aspx");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CheckWebSessionStatus()
    {
        DataTable dt = PhoenixCommoneProcessing.GetSessionStatus(new Guid(Request.QueryString["SESSIONID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["WEBSESSIONSTATUS"] = dt.Rows[0]["FLDACTIVE"].ToString();
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


    private void BindQuoteDetails()
    {
        string[] alColumns = { "FLDAGENTNAME", "FLDSEAPORTNAME", "FLDQUOTEREFNO", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDTOTALUSDAMOUNT" };
        string[] alCaptions = { "Port Agent", "Port", "Quotation No.", "Currency", "Amount", "Amount(USD)" };
        string requestid = null;
        string portagentid = null;

        string sortexpression = (ViewState["QSORTEXPRESSION"] == null) ? null : (ViewState["QSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["QSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["QSORTDIRECTION"].ToString());
        if (ViewState["REQUESTID"] != null)
            requestid = ViewState["REQUESTID"].ToString();
        if (ViewState["PORTAGENTID"] != null)
            portagentid = ViewState["PORTAGENTID"].ToString();

        DataTable dt = PhoenixCrewCostEvaluationQuote.EditCrewCostQuote(new Guid(requestid)
        , General.GetNullableGuid(ViewState["QUOTEID"].ToString()));

        gvQuote.DataSource = dt;

        if (dt.Rows.Count > 0)
        {

            if (ViewState["QUOTEID"] == null)
            {
                ViewState["QUOTEID"] = dt.Rows[0]["FLDQUOTEID"].ToString();
            }
        }

    }

    protected void gvQuote_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindQuoteDetails();
    }

    protected void gvQuote_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvQuote_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string quoteno = ((RadTextBox)e.Item.FindControl("txtQtnNoEdit")).Text;
            UserControlCurrency ucCurrecy = (UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit");
            string quoteid = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
            string currency = ucCurrecy.SelectedCurrency;

            if (!IsValidQuote(quoteno, currency))
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                PhoenixCrewCostEvaluationQuote.UpdateCrewCostQuote(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REQUESTID"].ToString())
                    , new Guid(quoteid.ToString())
                    , quoteno
                    , Convert.ToInt16(currency)
                    );
            }

            BindQuoteDetails();
            gvQuote.Rebind();
            BindQuoteLineItem();
            gvLineItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvQuote_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton com = (LinkButton)e.Item.FindControl("cmdCommunication");

            RadLabel lbl = (RadLabel)e.Item.FindControl("lblRequestId");
            RadLabel lblAgentId = (RadLabel)e.Item.FindControl("lblAgentId");
            RadLabel lblPortAgentId = (RadLabel)e.Item.FindControl("lblPortAgentId");
            RadLabel lblAgentName = (RadLabel)e.Item.FindControl("lblAgentName");
            RadLabel lblQuoteId = (RadLabel)e.Item.FindControl("lblQuoteId");
            RadLabel lblApprovedYN = (RadLabel)e.Item.FindControl("lblApprovedYN");

            if (ViewState["APPROVEDYN"].ToString() == "1")
            {
                if (ed != null) ed.Visible = false;
            }

            RadLabel lblETA = (RadLabel)e.Item.FindControl("lblETA");
            RadLabel lblETD = (RadLabel)e.Item.FindControl("lblETD");

            if (lblETA != null)
            {
                DateTime? dt = General.GetNullableDateTime(lblETA.Text);
                if (dt != null)
                {
                    lblETA.Text = String.Concat(String.Format("{0:dd/MM/yyyy}", dt).ToString() + " " + String.Format("{0:t}", dt).ToString());
                    lblETA.Visible = true;
                }
            }

            if (lblETD != null)
            {
                DateTime? dt = General.GetNullableDateTime(lblETD.Text);
                if (dt != null)
                {
                    lblETD.Text = String.Concat(String.Format("{0:dd/MM/yyyy}", dt).ToString() + " " + String.Format("{0:t}", dt).ToString());
                    lblETD.Visible = true;
                }
            }
            if (com != null)
            {
                com.Visible = true;
                com.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewCostEvaluationQuoteChat.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString() + "&AGENTID=" + lblAgentId.Text + "&QUOTEID=" + lblQuoteId.Text + "&AGENTNAMEOLY= "+ HttpContext.Current.Session["companyname"] + "&AGENTNAME= " + HttpContext.Current.Session["companyname"] + " - " + txtRequestNo.Text.ToString() + "&ISOFFICE=0" + "');return false;");

            }
        }
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlCurrency ddlCurrency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit");
            if (ddlCurrency != null) ddlCurrency.SelectedCurrency = drv["FLDCURRENCYID"].ToString();
        }
    }

    private void BindQuoteLineItem()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSECTIONTYPE", "FLDSECTION", "FLDAMOUNT", "FLDREMARKS" };
        string[] alCaptions = { "Section Type", "Section", "Amount", "Remarks" };
        string requestid = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["REQUESTID"] != null)
            requestid = ViewState["REQUESTID"].ToString();

        DataSet ds = PhoenixCrewCostEvaluationQuote.CrewCostQuoteLineItemSearch(new Guid(requestid)
                                    , General.GetNullableGuid(ViewState["QUOTEID"] == null ? "" : ViewState["QUOTEID"].ToString())
                                    , (int)ViewState["PAGENUMBER"]
                                    , gvLineItem.PageSize
                                    , ref iRowCount, ref iTotalPageCount);

        gvLineItem.DataSource = ds;
        gvLineItem.VirtualItemCount = iRowCount;

        General.SetPrintOptions("gvLineItem", "Quotation Section Details", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;

        BindQuoteLineItem();
    }

    protected void gvLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            UserControlMaskNumber ucamount = (UserControlMaskNumber)e.Item.FindControl("txtAmountEdit");
            string amount = ucamount.Text;
            string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;
            string lineitemid = ((RadLabel)e.Item.FindControl("lblLineItemIdEdit")).Text;

            if (!IsValidQuoteAmount(amount))
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                PhoenixCrewCostEvaluationQuote.UpdateCostQuoteLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REQUESTID"].ToString())
                    , new Guid(ViewState["QUOTEID"].ToString())
                    , new Guid(lineitemid)
                    , Convert.ToDecimal(amount)
                    , General.GetNullableString(remarks));

                PhoenixCrewCostEvaluationQuote.UpdateCrewCostQuoteSetionTotal(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REQUESTID"].ToString())
                    , new Guid(ViewState["QUOTEID"].ToString()));

                PhoenixCrewCostEvaluationQuote.UpdateCrewCostQuoteTotal(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REQUESTID"].ToString())
                    , new Guid(ViewState["QUOTEID"].ToString()));
            }

            BindQuoteDetails();
            gvQuote.Rebind();
            BindQuoteLineItem();
            gvLineItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
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

    private decimal TotalAmount = 0;
    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {

        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            decimal.TryParse(drv["FLDTOTALAMOUNT"].ToString(), out TotalAmount);

            if (ViewState["APPROVEDYN"].ToString() == "1")
            {
                if (ed != null) ed.Visible = false;
            }
        }
        if (e.Item is GridFooterItem)
        {
            //e.Item.Cells[2].Text = TotalAmount.ToString();
            //e.Item.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            //e.Item.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            //e.Item.Font.Bold = true;

            GridFooterItem footer = (GridFooterItem)e.Item;
            (footer["AMOUNT"].FindControl("lblTotalA") as RadLabel).Text = TotalAmount.ToString();

        }
    }

    private Boolean IsValidQuote(string qtnno, string currencycode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(qtnno.Trim()) == null)
        {
            ucError.ErrorMessage = "Quotation no. required ";
        }
        if (General.GetNullableInteger(currencycode) == null)
        {
            ucError.ErrorMessage = "Currency code required ";
        }
        return (!ucError.IsError);
    }

    private Boolean IsValidQuoteAmount(string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(amount) == null)
        {
            ucError.ErrorMessage = "Amount required ";
        }
        return (!ucError.IsError);
    }

}
