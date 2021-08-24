using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using System.Collections.Specialized;

public partial class InspectionMOCadd : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        imgPersonInCharge.Attributes.Add("onclick", "return showPickList('spnPersonInCharge', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId=" + ViewState["Vesselid"].ToString() + "', true); ");
        imgPersonInChargeOffice.Attributes.Add("onclick", "return showPickList('spnPersonInChargeOffice', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=&mod=', true);");

        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("List", "BACK", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuMOC.AccessRights = this.ViewState;
        MenuMOC.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            //MenuMOC.SetTrigger(pnlMOC);
            ViewState["MOCID"] = "";
            ViewState["Vesselid"] = ddlvessel.SelectedVessel;
            txtCrewId.Attributes.Add("style", "visibility:hidden");
            txtPersonInChargeOfficeId.Attributes.Add("style", "visibility:hidden");
            txtPersonInChargeOfficeEmail.Attributes.Add("style", "visibility:hidden");
            spnPersonInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ddlvessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["Vesselid"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlvessel.Enabled = false;
            }
            else
            {
                ddlvessel.SelectedVessel = Filter.CurrentVesselConfiguration != null ? Filter.CurrentVesselConfiguration.ToString() : "";
                ucCompany.Enabled = true;
                ucCompany.CssClass = "input_mandatory";
            }
        }
    }

    protected void MOC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidMOCAdd(ddlvessel.SelectedVessel
                                    , ucCompany.SelectedCompany
                                    , ucMOCDate.Text
                                    , (ddlvessel.SelectedVessel == "0") ? txtPersonInChargeOfficeId.Text : txtCrewId.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionMOCTemplate.MOCInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                  , General.GetNullableGuid((ViewState["MOCID"]).ToString())
                                                  , int.Parse(ddlvessel.SelectedVessel)
                                                  , General.GetNullableString(txtmoctitle.Text)
                                                  , (ddlvessel.SelectedVessel == "0") ? General.GetNullableInteger(txtPersonInChargeOfficeId.Text) : General.GetNullableInteger(txtCrewId.Text)
                                                  , (ddlvessel.SelectedVessel == "0") ? General.GetNullableString(txtOfficePersonName.Text) : General.GetNullableString(txtCrewName.Text)
                                                  , (ddlvessel.SelectedVessel == "0") ? General.GetNullableString(txtOfficePersonDesignation.Text) : General.GetNullableString(txtCrewRank.Text)
                                                  , General.GetNullableInteger(ddlstatus.SelectedValue)
                                                  , General.GetNullableDateTime(ucMOCDate.Text)
                                                  , null
                                                  , null
                                                  , null
                                                  , ucCompany.SelectedCompany
                                                  , null
                                                  );

                ucStatus.Text = "Mangement Of Change inserted successfully.";

                Response.Redirect("../Inspection/InspectionManagementOfChangeList.aspx", true);
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionManagementOfChangeList.aspx", true);
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMOCAdd(string MOCType, string vessel, string date, string proposerid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vessel) != null)
        {
            if (vessel.Equals("-- OFFICE --"))
            {
                if (General.GetNullableInteger(vessel) == null)
                    ucError.ErrorMessage = "'Company' is required";
            }
        }
        if (MOCType.Equals("Dummy"))
            ucError.ErrorMessage = "Office/Ship is required.";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";

        if (General.GetNullableInteger(proposerid) == null)
            ucError.ErrorMessage = "Proposer(Name/Rank) is required.";

        return (!ucError.IsError);
    }
    public void ddlvessel_textchanged(object sender, EventArgs e)
    {
        if (ddlvessel.SelectedVessel == "0")
        {
            spnPersonInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = true;
            ViewState["Vesselid"] = ddlvessel.SelectedVessel;
            ucCompany.Enabled = true;
            ucCompany.CssClass = "input_mandatory";
        }
        else
        {
            spnPersonInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;
            ViewState["Vesselid"] = ddlvessel.SelectedVessel;
            ucCompany.Enabled = false;
            ucCompany.CssClass = "input";
        }
    }
}
