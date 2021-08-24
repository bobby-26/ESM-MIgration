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
using System.Web.UI.HtmlControls;

public partial class InspectionIncidentPropertyDamageGeneral : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("New", "NEW");
                toolbar.AddButton("Save", "SAVE");
                MenuIncidentDamageGeneral.AccessRights = this.ViewState;
                if (Filter.CurrentSelectedIncidentMenu == null)
                    MenuIncidentDamageGeneral.MenuList = toolbar.Show();

                BindPropertyDamageList();
                BindSubPropertyDamageList();
                DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string vesselid = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                    imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponentTreeView.aspx?vesselid=" + vesselid + "', true);");
                }

                if (Request.QueryString["propertydamageid"] != null)
                {
                    ViewState["PROPERTYDAMAGEID"] = Request.QueryString["propertydamageid"].ToString();
                }

                if (ViewState["PROPERTYDAMAGEID"] != null)
                {
                    BindIncidentPropertyDamage();
                }
                else
                {
                    ViewState["PROPERTYDAMAGEID"] = "";
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

    private void BindIncidentPropertyDamage()
    {
        DataSet ds;

        if (General.GetNullableGuid(ViewState["PROPERTYDAMAGEID"].ToString()) != null)
        {
            ds = PhoenixInspectionIncidentDamage.EditIncidentPropertyDamage(new Guid(ViewState["PROPERTYDAMAGEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ucProperty.SelectedQuick = dr["FLDPROPERTYID"].ToString();
                txtEstimatedCost.Text = string.Format(String.Format("{0:#####.00}", dr["FLDESTIMATEDCOST"]));
                BindPropertyDamageList();
                ddlPropertyDamage.SelectedValue = dr["FLDTYPEOFPROPERTYDAMAGE"].ToString();
                BindSubPropertyDamageList();
                ddlSubPropertyDamage.SelectedValue = dr["FLDSUBTYPE"].ToString();
                //ucTypeOfPropertyDamage.SelectedHard = dr["FLDTYPEOFPROPERTYDAMAGE"].ToString();

                txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
                txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                txtCategory.Text = dr["FLDCATEGORY"].ToString();

                if (dr["FLDTYPEOFPROPERTYDAMAGENAME"].ToString().ToUpper().Contains("MACHINERY"))
                {
                    txtComponentCode.Visible = true;
                    txtComponentName.Visible = true;
                    txtComponentId.Visible = true;
                    imgComponent.Visible = true;
                    ucProperty.Visible = false;
                }
                else
                {
                    txtComponentCode.Visible = false;
                    txtComponentName.Visible = false;
                    txtComponentId.Visible = false;
                    imgComponent.Visible = false;
                    ucProperty.Visible = true;
                }
            }
        }
    }

    protected void BindPropertyDamageList()
    {
        string prop = PhoenixCommonRegisters.GetHardCode(1, 204, "PDM");
        DataTable dt = PhoenixInspectionIncident.IncidentConsequenceList(int.Parse(prop));
        ddlPropertyDamage.Items.Clear();
        ddlPropertyDamage.DataSource = dt;
        ddlPropertyDamage.DataTextField = "FLDNAME";
        ddlPropertyDamage.DataValueField = "FLDHAZARDID";
        ddlPropertyDamage.DataBind();
        ddlPropertyDamage.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindSubPropertyDamageList()
    {
        DataTable dt = PhoenixInspectionIncident.ListRiskAssessmentSubHazard(General.GetNullableInteger(ddlPropertyDamage.SelectedValue));
        ddlSubPropertyDamage.Items.Clear();
        ddlSubPropertyDamage.DataSource = dt;
        ddlSubPropertyDamage.DataTextField = "FLDNAME";
        ddlSubPropertyDamage.DataValueField = "FLDSUBHAZARDID";
        ddlSubPropertyDamage.DataBind();
        ddlSubPropertyDamage.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void ddlPropertyDamage_Changed(object sender, EventArgs e)
    {
        BindSubPropertyDamageList();

        if (ddlPropertyDamage.SelectedItem.Text.ToUpper().Contains("MACHINERY"))
        {
            txtComponentCode.Visible = true;
            txtComponentName.Visible = true;
            txtComponentId.Visible = true;
            imgComponent.Visible = true;
            ucProperty.Visible = false;
        }
        else
        {
            txtComponentCode.Visible = false;
            txtComponentName.Visible = false;
            txtComponentId.Visible = false;
            imgComponent.Visible = false;
            ucProperty.Visible = true;
        }
    }
    protected void IncidentDamageGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidIncidentPropertyDamage())
                {
                    if (General.GetNullableGuid(ViewState["PROPERTYDAMAGEID"].ToString()) == null)
                    {

                        PhoenixInspectionIncidentDamage.InsertIncidentPropertyDamage(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(Filter.CurrentIncidentID),
                            General.GetNullableInteger(ucProperty.SelectedQuick),
                            int.Parse(ddlPropertyDamage.SelectedValue),
                            General.GetNullableDecimal(txtEstimatedCost.Text),
                            General.GetNullableGuid(txtComponentId.Text),
                            new Guid(ddlSubPropertyDamage.SelectedValue)
                            );

                        ucStatus.Text = "Property Damage details added.";
                    }
                    else
                    {
                        PhoenixInspectionIncidentDamage.UpdateIncidentPropertyDamage(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["PROPERTYDAMAGEID"].ToString()),
                            new Guid(Filter.CurrentIncidentID),
                            General.GetNullableInteger(ucProperty.SelectedQuick),
                            int.Parse(ddlPropertyDamage.SelectedValue),
                            General.GetNullableDecimal(txtEstimatedCost.Text),
                            General.GetNullableGuid(txtComponentId.Text),
                            new Guid(ddlSubPropertyDamage.SelectedValue)
                            );

                        ucStatus.Text = "Property Damage details updated.";
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
                ViewState["PROPERTYDAMAGEID"] = "";
                Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidIncidentPropertyDamage()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlPropertyDamage.SelectedValue) == null)
            ucError.ErrorMessage = "'Type of Damage' is required.";

        if (General.GetNullableGuid(ddlSubPropertyDamage.SelectedValue) == null)
            ucError.ErrorMessage = "'Subtype of Damage' is required.";

        if (ddlPropertyDamage.SelectedItem.Text.ToUpper().Contains("MACHINERY"))
        {
            if (General.GetNullableGuid(txtComponentId.Text) == null)
                ucError.ErrorMessage = "'Damaged Property' is required.";
        }
        else
        {
            if (General.GetNullableInteger(ucProperty.SelectedQuick) == null)
                ucError.ErrorMessage = "'Damaged Property' is required.";
        }        

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ucProperty.SelectedQuick = "";
        txtEstimatedCost.Text = "";
        //ucTypeOfPropertyDamage.SelectedHard = "";
        ddlPropertyDamage.SelectedIndex = 0;
        ddlSubPropertyDamage.SelectedIndex = 0;

        txtComponentCode.Text = "";
        txtComponentId.Text = "";
        txtComponentName.Text = "";

        txtComponentCode.Visible = false;
        txtComponentName.Visible = false;
        txtComponentId.Visible = false;
        imgComponent.Visible = false;
        ucProperty.Visible = true;
        txtCategory.Text = "";
    }
}
