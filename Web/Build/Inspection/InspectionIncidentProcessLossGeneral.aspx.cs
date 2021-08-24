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

public partial class InspectionIncidentProcessLossGeneral : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {        
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PROCESSLOSSID"] = null;           

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("New", "NEW");
            toolbar.AddButton("Save", "SAVE");
            MenuIncidentInjuryGeneral.AccessRights = this.ViewState;
            BindProcessLossList();
            BindSubProcessLossList();

            if (Filter.CurrentSelectedIncidentMenu == null)
                MenuIncidentInjuryGeneral.MenuList = toolbar.Show();

            if (Request.QueryString["PROCESSLOSSID"] != null && Request.QueryString["PROCESSLOSSID"].ToString() != "")
                ViewState["PROCESSLOSSID"] = Request.QueryString["PROCESSLOSSID"].ToString();

            EditIncidentInjury();
        }
    }

    protected void BindProcessLossList()
    {
        string procloss = PhoenixCommonRegisters.GetHardCode(1, 204, "PLS");
        DataTable dt = PhoenixInspectionIncident.IncidentConsequenceList(int.Parse(procloss));
        ddlTypeOfProcessLoss.Items.Clear();
        ddlTypeOfProcessLoss.DataSource = dt;
        ddlTypeOfProcessLoss.DataTextField = "FLDNAME";
        ddlTypeOfProcessLoss.DataValueField = "FLDHAZARDID";
        ddlTypeOfProcessLoss.DataBind();
        ddlTypeOfProcessLoss.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindSubProcessLossList()
    {
        DataTable dt = PhoenixInspectionIncident.ListRiskAssessmentSubHazard(General.GetNullableInteger(ddlTypeOfProcessLoss.SelectedValue));
        ddlSubProcessLoss.Items.Clear();
        ddlSubProcessLoss.DataSource = dt;
        ddlSubProcessLoss.DataTextField = "FLDNAME";
        ddlSubProcessLoss.DataValueField = "FLDSUBHAZARDID";
        ddlSubProcessLoss.DataBind();
        ddlSubProcessLoss.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void ddlTypeOfProcessLoss_Changed(object sender, EventArgs e)
    {
        BindSubProcessLossList();
    }

    protected void MenuIncidentInjuryGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidInspectionIncidentInjury())
                {
                    if (ViewState["PROCESSLOSSID"] == null)
                    {
                        PhoenixInspectionIncidentProcessLoss.InsertIncidentProcessLoss(
                                                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode                            
                                                                                    , new Guid(Filter.CurrentIncidentID)
                                                                                    , int.Parse(ddlTypeOfProcessLoss.SelectedValue)
                                                                                    , General.GetNullableDecimal(ucExtimatedCost.Text)
                                                                                    , General.GetNullableInteger(txtOffhiredays.Text)
                                                                                    , General.GetNullableInteger(txtOffhirehours.Text)
                                                                                    , General.GetNullableInteger(txtOffhiremins.Text)
                                                                                    , General.GetNullableGuid(ddlSubProcessLoss.SelectedValue)
                                                                                    );

                        ucStatus.Text = "Process Loss details are added.";
                    }
                    else
                    {
                        PhoenixInspectionIncidentProcessLoss.UpdateIncidentProcessLoss(
                                                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(ViewState["PROCESSLOSSID"].ToString())
                                                                                    , new Guid(ViewState["INSPECTIONINCIDENTID"].ToString())
                                                                                    , int.Parse(ddlTypeOfProcessLoss.SelectedValue)
                                                                                    , General.GetNullableDecimal(ucExtimatedCost.Text)
                                                                                    , General.GetNullableInteger(txtOffhiredays.Text)
                                                                                    , General.GetNullableInteger(txtOffhirehours.Text)
                                                                                    , General.GetNullableInteger(txtOffhiremins.Text)
                                                                                    , General.GetNullableGuid(ddlSubProcessLoss.SelectedValue)
                                                                                    );
                        ucStatus.Text = "Process Loss details are updated.";
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
                ViewState["PROCESSLOSSID"] = null;
                Reset();
                EditIncidentInjury();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void EditIncidentInjury()
    {
        if (ViewState["PROCESSLOSSID"] != null)
        {
            DataSet ds = PhoenixInspectionIncidentProcessLoss.EditIncidentProcessLoss(new Guid(ViewState["PROCESSLOSSID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                
                ucExtimatedCost.Text = dr["FLDESTIMATEDCOST"].ToString();
                BindProcessLossList();
                ddlTypeOfProcessLoss.SelectedValue = dr["FLDTYPEOFPROCESSLOSS"].ToString();
                BindSubProcessLossList();
                ddlSubProcessLoss.SelectedValue = dr["FLDSUBTYPE"].ToString();
                txtCategory.Text = dr["FLDCATEGORY"].ToString();
                ViewState["INSPECTIONINCIDENTID"] = dr["FLDINSPECTIONINCIDENTID"].ToString();
                txtOffhiredays.Text = dr["FLDOFFHIREDAYS"].ToString();
                txtOffhirehours.Text = dr["FLDOFFHIREHOURS"].ToString();
                txtOffhiremins.Text = dr["FLDOFFHIREMINS"].ToString();
            }
        }
        else
        {
            Reset();
        }

    }
    private bool IsValidInspectionIncidentInjury()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlTypeOfProcessLoss.SelectedValue) == null)
            ucError.ErrorMessage = "'Type of Process Loss' is required.";

        if (General.GetNullableGuid(ddlSubProcessLoss.SelectedValue) == null)
            ucError.ErrorMessage = "'Subtype of Process Loss' is required.";

        //if (string.IsNullOrEmpty(ucExtimatedCost.SelectedQuick) || ucExtimatedCost.SelectedQuick.ToUpper().ToString() == "DUMMY")
        //    ucError.ErrorMessage = "Estimated Cost is required.";       
        
        return (!ucError.IsError);
    }
    private void Reset()
    {        
        ddlTypeOfProcessLoss.SelectedIndex = 0;
        ddlSubProcessLoss.SelectedIndex = 0;
        ucExtimatedCost.Text = "";
        txtOffhiredays.Text = "";
        txtOffhirehours.Text = "";
        txtOffhiremins.Text = "";
        txtCategory.Text = "";
    }
}
