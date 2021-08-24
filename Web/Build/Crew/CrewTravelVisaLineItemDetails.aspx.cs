using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewTravelVisaLineItemDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["TRAVELVISAID"] = null;
                ViewState["VISALINEITEMID"] = null;

                if (Request.QueryString["travelvisaid"] != null && Request.QueryString["visalineitemid"] != null)
                {
                    ViewState["TRAVELVISAID"] = Request.QueryString["TravelVisaId"].ToString();
                    ViewState["VISALINEITEMID"] = Request.QueryString["VisaLineItemId"].ToString();
                }

                GetVisaStatusList();               
                EditCrewTravelVisa();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuCrewVisa.AccessRights = this.ViewState;
            MenuCrewVisa.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void EditCrewTravelVisa()
    {
        DataTable dt = PhoenixCrewTravelVisa.EditCrewTravelVisaLineItem(new Guid(ViewState["TRAVELVISAID"].ToString())
            , new Guid(ViewState["VISALINEITEMID"].ToString()));

        DataRow dr = dt.Rows[0];

        txtName.Text = dr["FLDNAME"].ToString();
        txtRank.Text = dr["FLDRANKNAME"].ToString();
        txtPassportNo.Text = dr["FLDPASSPORTNO"].ToString();
        txtVisaProcessType.Text = dr["FLDVISAPROCESSTYPENAME"].ToString();
        ucVisaType.SelectedHard = dr["FLDVISATYPE"].ToString();
        ucVisaReason.SelectedReason = dr["FLDVISAREASON"].ToString();
        ucPaymentMode.SelectedHard = dr["FLDPAYMENTMODE"].ToString();
        txtAmount.Text = dr["FLDAMOUNT"].ToString();
        txtAppliedon.Text = dr["FLDAPPLIEDON"].ToString();
        txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
        txtExpectedCollon.Text = dr["FLDEXPECTEDCOLLECTIONON"].ToString();
        txtDocReceivedOn.Text = dr["FLDDOCRECEIVEDON"].ToString();
        ddlVisaStatus.SelectedValue = dr["FLDVISASTATUS"].ToString();
        ddlvessel.SelectedVessel = dr["FLDJOINEDVESSEL"].ToString();
    }
    protected void MenuCrewVisa_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper() == "SAVE")
            {
                if (!IsValidTravelVisa())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewTravelVisa.UpdateCrewTravelVisaLineItem(new Guid(ViewState["TRAVELVISAID"].ToString())
                         , new Guid(ViewState["VISALINEITEMID"].ToString())
                         , General.GetNullableInteger(ucVisaType.SelectedHard)
                         , General.GetNullableInteger(ucVisaReason.SelectedReason)
                         , int.Parse(ucPaymentMode.SelectedHard)
                         , General.GetNullableDateTime(txtAppliedon.Text)
                         , General.GetNullableDateTime(txtExpectedCollon.Text)
                         , General.GetNullableDateTime(txtDocReceivedOn.Text)
                         , General.GetNullableInteger(ddlVisaStatus.SelectedValue)
                         , General.GetNullableInteger(ddlvessel.SelectedVessel));

                }
                GetVisaStatusList();
                EditCrewTravelVisa();
                
                ucStatus.Visible = true;
                ucStatus.Text = "Information Updated";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void GetVisaStatusList()
    {
        DataTable dt = PhoenixCrewTravelVisa.VisaProcessNextStatusList(new Guid(ViewState["TRAVELVISAID"].ToString())
               , new Guid(ViewState["VISALINEITEMID"].ToString()));
        ddlVisaStatus.DataSource = dt;
        ddlVisaStatus.DataBind();

    }
    private bool IsValidTravelVisa()
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucPaymentMode.SelectedHard) == null)
            ucError.ErrorMessage = "Paymentmode is required.";

        if (General.GetNullableInteger(ucVisaReason.SelectedReason) == null)
            ucError.ErrorMessage = "Reason is required.";

        if (General.GetNullableDateTime(txtAppliedon.Text) != null && General.GetNullableDateTime(txtDocReceivedOn.Text) != null)
        {
            resultdate = Convert.ToDateTime(txtDocReceivedOn.Text);
            if (DateTime.Compare(DateTime.Parse(txtAppliedon.Text), resultdate) > 0)
                ucError.ErrorMessage = "Document Received date should be greater or equal to visa applied date";
        }
        if (General.GetNullableDateTime(txtAppliedon.Text) != null && General.GetNullableDateTime(txtExpectedCollon.Text) != null)
        {
            resultdate = Convert.ToDateTime(txtExpectedCollon.Text);
            if (DateTime.Compare(DateTime.Parse(txtAppliedon.Text), resultdate) > 0)
                ucError.ErrorMessage = "Expected collection date should be greater or equal to visa applied date";
        }
        return (!ucError.IsError);
    }
}
