using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class InspectionIncidentPollutionGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("New", "NEW");
                toolbar.AddButton("Save", "SAVE");
                if (Filter.CurrentSelectedIncidentMenu == null)
                    MenuInspectionIncident.MenuList = toolbar.Show();

                ucReleaseCategory.HazardTypeList = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(int.Parse("2"),0);
                BindEnvironmentalSubCategory();

                if (Request.QueryString["pollutionid"] != null)
                {
                    ViewState["POLLUTIONID"] = Request.QueryString["pollutionid"].ToString();
                }

                if (ViewState["POLLUTIONID"] != null && !string.IsNullOrEmpty(ViewState["POLLUTIONID"].ToString()))
                {
                    BindInspectionIncident();
                }
                else
                {
                    Reset();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindEnvironmentalSubCategory()
    {
        DataTable dt = PhoenixInspectionIncident.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucReleaseCategory.SelectedHazardType));
        ddlReleaseSubCategory.Items.Clear();
        ddlReleaseSubCategory.DataSource = dt;
        ddlReleaseSubCategory.DataTextField = "FLDNAME";
        ddlReleaseSubCategory.DataValueField = "FLDSUBHAZARDID";
        ddlReleaseSubCategory.DataBind();
        ddlReleaseSubCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void ucReleaseCategory_Changed(object sender, EventArgs e)
    {
        BindEnvironmentalSubCategory();
    }

    private void BindInspectionIncident()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionIncidentPollution.EditIncidentPollution(General.GetNullableGuid(ViewState["POLLUTIONID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtSubstanceName.Text = ds.Tables[0].Rows[0]["FLDNAMEOFSUBSTANCE"].ToString();            
            //ucQuantity.SelectedQuick = ds.Tables[0].Rows[0]["FLDQUANTITYINBBLSORMT"].ToString();
            ucReleaseCategory.Type = "2";
            ucReleaseCategory.DataBind();
            ucReleaseCategory.SelectedHazardType = ds.Tables[0].Rows[0]["FLDRELEASECATEGORY"].ToString();
            BindEnvironmentalSubCategory();
            ddlReleaseSubCategory.SelectedValue = ds.Tables[0].Rows[0]["FLDSUBTYPE"].ToString();
            txtCategory.Text = ds.Tables[0].Rows[0]["FLDCATEGORY"].ToString();
            ucReleaseType.SelectedQuick = ds.Tables[0].Rows[0]["FLDRELEASETYPE"].ToString();
            ucExtimatedCost.Text = ds.Tables[0].Rows[0]["FLDESTIMATEDCOST"].ToString();

        }
    }

    protected void InspectionIncident_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidInspectionIncident())
                {
                    if (ViewState["POLLUTIONID"] == null || string.IsNullOrEmpty(ViewState["POLLUTIONID"].ToString()))
                    {
                        PhoenixInspectionIncidentPollution.IncidentPollutionInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            General.GetNullableGuid(Filter.CurrentIncidentID),
                            txtSubstanceName.Text,
                            int.Parse(ucReleaseType.SelectedQuick),
                            null, //int.Parse(ucQuantity.SelectedQuick),
                            int.Parse(ucReleaseCategory.SelectedHazardType),
                            General.GetNullableDecimal(ucExtimatedCost.Text),
                            General.GetNullableGuid(ddlReleaseSubCategory.SelectedValue));

                        ucStatus.Text = "Environment Release details added.";
                    }
                    else
                    {
                        PhoenixInspectionIncidentPollution.IncidentPollutionUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           General.GetNullableGuid(ViewState["POLLUTIONID"].ToString()),
                           txtSubstanceName.Text,
                           int.Parse(ucReleaseType.SelectedQuick),
                           null, //int.Parse(ucQuantity.SelectedQuick),
                          int.Parse(ucReleaseCategory.SelectedHazardType),
                          General.GetNullableDecimal(ucExtimatedCost.Text),
                          General.GetNullableGuid(ddlReleaseSubCategory.SelectedValue));

                        ucStatus.Text = "Environment Release details updated.";
                    }

                    String script = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["POLLUTIONID"] = null;
                Reset();
            }
        }
        catch(Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidInspectionIncident()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtSubstanceName.Text) == null)
            ucError.ErrorMessage = "Name of Substance is required.";        

        //if (General.GetNullableInteger(ucQuantity.SelectedQuick) == null)
        //    ucError.ErrorMessage = "Quantity is required.";        

        if (General.GetNullableInteger(ucReleaseCategory.SelectedHazardType) == null)
            ucError.ErrorMessage = "Release Category is required.";

        if (General.GetNullableGuid(ddlReleaseSubCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Release Subcategory is required.";

        if (General.GetNullableInteger(ucReleaseType.SelectedQuick) == null)
            ucError.ErrorMessage = "Release Type is required.";

        return (!ucError.IsError);
    }

    private void Reset()
    {        
        //ucQuantity.SelectedQuick = "";
        txtSubstanceName.Text = "";
        ucReleaseCategory.SelectedHazardType = "";
        ucReleaseType.SelectedQuick = "";
        ucExtimatedCost.Text = "";
        ddlReleaseSubCategory.SelectedIndex = 0;
        txtCategory.Text = "";
    }
}
