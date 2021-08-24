using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewTravelNewRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);      
        MenuCrewTraveladd.AccessRights = this.ViewState;
        MenuCrewTraveladd.MenuList = toolbar.Show();
        if (!IsPostBack)
        {            
            if (Request.QueryString["travelid"]!=null && Request.QueryString["travelid"] != string.Empty)
            {
                ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();                
            }
            BindVesselAccount();
        }
     
    }
    protected void MenuCrewTraveladd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CANCEL"))
            {
                Response.Redirect("CrewTravelPassengersList.aspx?travelid=" + ViewState["TRAVELID"], false);
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTravelRequest())
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? travelid = null;
                PhoenixCrewTravelRequest.TravelNewRequesInsert
                            (General.GetNullableInteger(ucpurpose.SelectedReason),
                            General.GetNullableDateTime(DateTime.Now.ToLongDateString()),
                            int.Parse(ucvessel.SelectedVessel),
                            General.GetNullableInteger(ddlPassfrom.SelectedValue.ToString())
                            ,ref travelid
                            ,General.GetNullableString(txtPurposeDesc.Text.Trim())
                            ,General.GetNullableInteger(ddlAccountDetails.SelectedValue));

                Filter.CurrentOfficeTravelRequestFilter = null;
                ViewState["TRAVELID"] = travelid;
                Session["ENTRYPAGE"] = "1";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTravelRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ucvessel.SelectedVessel.ToUpper() == "DUMMY" || General.GetNullableString(ucvessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ucvessel.SelectedVessel) != null & General.GetNullableInteger(ucvessel.SelectedVessel) > 0)
        {
            if (General.GetNullableInteger(ddlAccountDetails.SelectedValue) == null)
                ucError.ErrorMessage = "Vessel Account is required.";
        }
        if (ucpurpose.SelectedReason.ToUpper() == "DUMMY" || General.GetNullableString(ucpurpose.SelectedReason) == null)        
            ucError.ErrorMessage = "Purpose is required.";

        if (ddlPassfrom.SelectedValue.ToUpper() == "DUMMY" || General.GetNullableInteger(ddlPassfrom.SelectedValue) == null)
            ucError.ErrorMessage = "Passenger From required.";

        return (!ucError.IsError);
    }
    public void BindVesselAccount()
    {
        ddlAccountDetails.SelectedValue = "";
     
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(ucvessel.SelectedVessel) == 0 ? null : General.GetNullableInteger(ucvessel.SelectedVessel), 1);
        ddlAccountDetails.DataBind();
    }
    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ucVessel_OnTextChanged(object sender, EventArgs e)
    {
        BindVesselAccount();
    }
   
}
