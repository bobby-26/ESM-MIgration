using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewLandTravelRequestQuotationDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["REQUESTID"] = "";

                if (Request.QueryString["requestid"] != null && Request.QueryString["requestid"].ToString() != "")
                {
                    ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();
                }

                if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
                {
                    EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));

                }
            }

            if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
            {
                divReqDetail.Visible = true;
            }
            else
                divReqDetail.Visible = false;

            BindSubMenu();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindSubMenu()
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuRequest.AccessRights = this.ViewState;
        MenuRequest.MenuList = toolbarsub.Show();
    }
    
    protected void MenuRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            Guid? requestid = General.GetNullableGuid(ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "" ? ViewState["REQUESTID"].ToString().ToString() : null);
            if (CommandName.ToUpper() == "SAVE")
            {
                if (ViewState["REQUESTID"].ToString() != "")
                {
                    if (!IsValidRequest())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        PhoenixCrewLandTravelRequest.UpdateLandTravelRequestAgentUpdate(new Guid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString() : null)
                        , decimal.Parse(txtAmount.Text)
                        , int.Parse(ddlCurrencyCode.SelectedCurrency));

                        EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
                    }

                    ucStatus.Text = "Information Saved";
                }
            }
            BindSubMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPassengers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPassengers();
    }

    protected void gvPassengers_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvPassengers_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void BindPassengers()
    {
        try
        {
            string[] alColumns = { "FLDPASSENGERNAME", "FLDDESIGNATIONNAME", "FLDREASON", "FLDBUDGET", "FLDTENTATIVEVESSELNAME", "FLDPAYABLENAME", "FLDCOST" };
            string[] alCaptions = { "Passenger Name", "Designation", "Reason", "Budget Code", "Tentative Vessel", "Payable by", "Cost" };

            DataSet ds = new DataSet();

            ds = PhoenixCrewLandTravelPassengers.LandTravelPassengersSearch(ViewState["REQUESTID"] != null ? General.GetNullableGuid(ViewState["REQUESTID"].ToString()) : null);
            
            gvPassengers.DataSource = ds;

            General.SetPrintOptions("gvPassengers", "Land Travel Passengers", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(txtAmount.Text) == null)
            ucError.ErrorMessage = "Amount is Required.";
        if (General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is Required.";

        return (!ucError.IsError);
    }

    private void EditLandTravelRequest(Guid requestid)
    {
        DataTable dt = PhoenixCrewLandTravelRequest.EditLandTravelRequest(requestid);

        if (dt.Rows.Count > 0)
        {
            txtReqNo.Text = dt.Rows[0]["FLDREFERENCENO"].ToString();
            txtCity.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
            txtFromPlace.Text = dt.Rows[0]["FLDFROMPLACE"].ToString();
            txtToPlace.Text = dt.Rows[0]["FLDTOPLACE"].ToString();
            txtTravelDate.Text = dt.Rows[0]["FLDTRAVELDATE"].ToString();
            txtFromTime.Text = dt.Rows[0]["FLDFROMTIME"].ToString();
            txtToTime.Text = dt.Rows[0]["FLDTOTIME"].ToString();
            txtTypeofTransport.Text = dt.Rows[0]["FLDTYPEOFTRANSPORT"].ToString();
            txtTypeofDuty.Text = dt.Rows[0]["FLDTYPEOFDUTYNAME"].ToString();
            txtOtherInfo.Text = dt.Rows[0]["FLDOTHERINFORMATION"].ToString();
            txtMobileNumber.Text = dt.Rows[0]["FLDCONTACTNO"].ToString();
            txtNoofPassengers.Text = dt.Rows[0]["FLDNOOFPASSENGERS"].ToString();
            txtType.Text = dt.Rows[0]["FLDPACKAGETYPENAME"].ToString();
            ddlCurrencyCode.SelectedCurrency = dt.Rows[0]["FLDCURRENCYID"].ToString();
            txtAmount.Text = dt.Rows[0]["FLDAMOUNT"].ToString();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs args)
    {
        EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
    }

}
