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

public partial class InspectionIncidentDamageGeneral : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {

        }
        imgComponent.Attributes.Add("onclick",
                "return showPickList('spnPickListComponent', 'codehelp1', '','../Common/CommonPickListComponent.aspx', true); ");

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                txtComponentId.Attributes.Add("style", "visibility:hidden");

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("New", "NEW");
                toolbar.AddButton("Save", "SAVE");
                MenuIncidentDamageGeneral.AccessRights = this.ViewState;
                if (Filter.CurrentSelectedIncidentMenu == null)
                    MenuIncidentDamageGeneral.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Damage", "DAMAGE");
                //toolbar.AddButton("Work Order", "WORKORDER");
                toolbar.AddButton("Defect Work Order", "WORKREQUEST");
                IncidentDamageGeneralMain.AccessRights = this.ViewState;
                IncidentDamageGeneralMain.MenuList = toolbar.Show();
                IncidentDamageGeneralMain.SelectedMenuIndex = 0;


                if (Request.QueryString["damageid"] != null)
                {
                    ViewState["DAMAGEID"] = Request.QueryString["damageid"].ToString();
                }

                if (ViewState["DAMAGEID"] != null)
                {
                    BindIncidentDamage();
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

    private void BindIncidentDamage()
    {
        DataSet ds;

        ds = PhoenixInspectionIncidentDamage.EditIncidentDamage(new Guid(ViewState["DAMAGEID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
            txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
            txtEstimatedCost.Text = dr["FLDESTIMATEDCOST"].ToString();
            ucTypeOfDamage.SelectedQuick = dr["FLDTYPEOFDAMAGE"].ToString();
        }
    }
    protected void IncidentDamageGeneralMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (ViewState["DAMAGEID"] != null && ViewState["DAMAGEID"].ToString() != "")
        {
            if (dce.CommandName.ToUpper().Equals("DAMAGE"))
            {
                Response.Redirect("../Inspection/InspectionIncidentDamageGeneral.aspx?damageid=" + ViewState["DAMAGEID"].ToString());
            }
            else if (dce.CommandName.ToUpper().Equals("WORKORDER"))
            {
                //Response.Redirect("../Inspection/InspectionIncidentWorkOrder.aspx?damageid=" + ViewState["DAMAGEID"].ToString());
            }
            else if (dce.CommandName.ToUpper().Equals("WORKREQUEST"))
            {
                Response.Redirect("../Inspection/InspectionIncidentDefectWorkRequest.aspx?damageid=" + ViewState["DAMAGEID"].ToString());
            }
        }
        else
        {
            ucError.ErrorMessage = "Please Select Damaged Component to proceed Further.";
            ucError.Visible = true;
        }
    }
    protected void IncidentDamageGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (IsValidIncidentDamage())
            {
                if (ViewState["DAMAGEID"] == null)
                {
                    PhoenixInspectionIncidentDamage.InsertIncidentDamage(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(Filter.CurrentIncidentID),
                        new Guid(txtComponentId.Text),
                        int.Parse(ucTypeOfDamage.SelectedQuick),
                        General.GetNullableDecimal(txtEstimatedCost.Text));

                    ucStatus.Text = "Damage details added.";
                }
                else
                {
                    PhoenixInspectionIncidentDamage.UpdateIncidentDamage(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["DAMAGEID"].ToString()),
                        new Guid(Filter.CurrentIncidentID),
                        new Guid(txtComponentId.Text),
                        int.Parse(ucTypeOfDamage.SelectedQuick),
                        General.GetNullableDecimal(txtEstimatedCost.Text));

                    ucStatus.Text = "Damage details updated.";
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
            ViewState["DAMAGEID"] = null;
            Reset();
        }
    }

    private bool IsValidIncidentDamage()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucTypeOfDamage.SelectedQuick) == null)
            ucError.ErrorMessage = "'Type of Damage' is required.";

        if (General.GetNullableGuid(txtComponentId.Text) == null)
            ucError.ErrorMessage = "'Damaged Component' is required.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        txtComponentId.Text = "";
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtEstimatedCost.Text = "";
        ucTypeOfDamage.SelectedQuick = "";
    }
}
