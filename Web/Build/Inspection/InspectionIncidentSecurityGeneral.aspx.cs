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

public partial class InspectionIncidentSecurityGeneral : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {

        }
        base.Render(writer);
    }

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
                    MenuIncidentSecurityGeneral.MenuList = toolbar.Show();

                if (Request.QueryString["securityid"] != null)
                {
                    ViewState["SECURITYID"] = Request.QueryString["securityid"].ToString();
                }

                if (ViewState["SECURITYID"] != null)
                {
                    BindIncidentSecurity();
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

    private void BindIncidentSecurity()
    {
        DataSet ds;

        ds = PhoenixInspectionIncidentSecurity.EditIncidentSecurity(new Guid(ViewState["SECURITYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucExtimatedCost.Text = dr["FLDESTIMATEDCOST"].ToString();
            ucTypeOfSecurity.SelectedHard = dr["FLDTYPEOFSECURITY"].ToString();
            txtCategory.Text = dr["FLDCATEGORY"].ToString();
        }
    }

    protected void IncidentSecurityGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidIncidentSecurity())
                {
                    if (ViewState["SECURITYID"] == null)
                    {
                        PhoenixInspectionIncidentSecurity.InsertIncidentSecurity(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(Filter.CurrentIncidentID),
                            int.Parse(ucTypeOfSecurity.SelectedHard),
                            General.GetNullableDecimal(ucExtimatedCost.Text));

                        ucStatus.Text = "Security details added.";
                    }
                    else
                    {
                        PhoenixInspectionIncidentSecurity.UpdateIncidentSecurity(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["SECURITYID"].ToString()),
                            new Guid(Filter.CurrentIncidentID),
                            int.Parse(ucTypeOfSecurity.SelectedHard),
                            General.GetNullableDecimal(ucExtimatedCost.Text));

                        ucStatus.Text = "Security details updated.";
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
                ViewState["SECURITYID"] = null;
                Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidIncidentSecurity()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucTypeOfSecurity.SelectedHard) == null)
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ucExtimatedCost.Text = "";
        ucTypeOfSecurity.SelectedHard = "";
        txtCategory.Text = "";
    }
}
