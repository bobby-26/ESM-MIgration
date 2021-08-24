using System;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionPEARSRAConsequenceImpactAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuRAImpactAdd.AccessRights = this.ViewState;
            MenuRAImpactAdd.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["IMPACTID"] = "";

                if (Request.QueryString["IMPACTID"] != null && Request.QueryString["IMPACTID"].ToString() != string.Empty)
                    ViewState["IMPACTID"] = Request.QueryString["IMPACTID"].ToString();

                BindImpact();
                BindHazardCategory();
                BindSeverity();
            }         
        }
        catch(Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void BindHazardCategory()
    {
        ddlHazard.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRAHazardCategory();
        ddlHazard.DataBind();
        ddlHazard.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void BindSeverity()
    {
        ddlSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlSeverity.DataBind();
        ddlSeverity.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void MenuRAImpactAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                InserUpdatetImpact();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void InserUpdatetImpact()
    {
        try
        {
            if (!IsValidImpact())
            {
                ucError.Visible = true;
                return;
            }

            int Activeyn = cbActiveyn.Checked == true ? 1 : 0;

            PhoenixInspectionPEARSRiskassessmentConsequenceImpact.InsertRAConsequenceImpact(General.GetNullableGuid(ViewState["IMPACTID"].ToString())
                , Int32.Parse(ddlHazard.SelectedValue.ToString())
                , Int32.Parse(ddlSeverity.SelectedValue.ToString())
                , General.GetNullableString(txtImpact.Text)
                , Activeyn);            

            ucStatus.Text = "Information updated.";

            String script = String.Format("javascript:fnReloadList('Impact',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    public void BindImpact()
    {
        try
        {
            if(ViewState["IMPACTID"] !=null && ViewState["IMPACTID"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixInspectionPEARSRiskassessmentConsequenceImpact.EditRAConsequenceImpact(new Guid(ViewState["IMPACTID"].ToString()));

                if(ds.Tables[0].Rows.Count >0)
                {
                    ddlHazard.ClearSelection();
                    ddlSeverity.ClearSelection();
                    ddlHazard.SelectedValue = ds.Tables[0].Rows[0]["FLDHAZARDCATEGORYID"].ToString();
                    ddlSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDSEVERITYID"].ToString();
                    txtImpact.Text = ds.Tables[0].Rows[0]["FLDCONSEQUENCEIMPACT"].ToString();
                    cbActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                }
            }
        }
        catch(Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidImpact()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtImpact.Text.Trim()) == null)
            ucError.ErrorMessage = "Consequence Impact is required.";

        if (General.GetNullableString(ddlHazard.SelectedValue) == null)
            ucError.ErrorMessage = "Hazard Category is required.";

        if (General.GetNullableString(ddlSeverity.SelectedValue) == null)
            ucError.ErrorMessage = "Severity is required.";

        return (!ucError.IsError);
    }
}