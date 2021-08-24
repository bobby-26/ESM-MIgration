using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionNewRASelection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
        MenuGeneric.AccessRights = this.ViewState;
        MenuGeneric.MenuList = toolbar.Show();

        if(!IsPostBack)
        {
            BindType();
        }
    }

    private void BindType()
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivityExtn.ListNonRoutineRiskAssessmentType();
        ddlRAType.DataSource = dt;
        ddlRAType.DataTextField = "FLDNAME";
        ddlRAType.DataValueField = "FLDCATEGORYID";
        ddlRAType.DataBind();
        ddlRAType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void MenuGeneric_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LIST"))
            {                
               Response.Redirect("../Inspection/InspectionNonRoutineRiskAssessmentListExtn.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlRAType_TextChanged(object sender, EventArgs e)
    {
        if(ddlRAType.SelectedValue.Equals("2"))
        {
            Response.Redirect("../Inspection/InspectionRANavigationExtn.aspx?status=", false);
        }
        if (ddlRAType.SelectedValue.Equals("1"))
        {
            Response.Redirect("../Inspection/InspectionRAGenericExtn.aspx?status=", false);
        }
        if (ddlRAType.SelectedValue.Equals("3"))
        {
            Response.Redirect("../Inspection/InspectionRAMachineryExtn.aspx?status=", false);
        }
        if (ddlRAType.SelectedValue.Equals("4"))
        {
            Response.Redirect("../Inspection/InspectionRACargoExtn.aspx?status=", false);
        }
    }
}