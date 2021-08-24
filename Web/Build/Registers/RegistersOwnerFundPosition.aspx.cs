using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Drawing;
using System.Data;
using System.IO;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class Registers_RegistersOwnerFundPosition : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Generate", "GENERATE");
            OwnerFund.AccessRights = this.ViewState;
            OwnerFund.MenuList = toolbar.Show();

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
           
            toolbargrid.AddButton("Summary", "SUMMARY");
            toolbargrid.AddButton("OWNER", "OWNER");
            OwnerFundPosition.AccessRights = this.ViewState;
            OwnerFundPosition.MenuList = toolbargrid.Show();
            OwnerFundPosition.SelectedMenuIndex = 0;
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
              
                
            }
           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void OwnerFundPositiontab_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
          
            if (dce.CommandName.ToUpper().Equals("SUMMARY"))
            {
                Response.Redirect("../Registers/RegistersOwnerFundPosition.aspx");
            }
            if (dce.CommandName.ToUpper().Equals("OWNER"))
            {
                Response.Redirect("../Registers/RegistersOwnerFundPositionOwner.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
       
    }
    protected void OwnerFundPosition_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERATE"))
            {
                PhoenixRegisterOwnerFundPosition.OwnerFundSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucOwner.SelectedAddress),General.GetNullableInteger(ddlvessel.SelectedValue), General.GetNullableDateTime(ucAsondate.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void ucOwner_Onchange(object sender, EventArgs e)
    {
        DataSet dsveaaelname = null;
        dsveaaelname = PhoenixCommonBudgetGroupAllocation.OwnerVessellist(Convert.ToInt32(ucOwner.SelectedAddress));
        ddlvessel.DataSource = dsveaaelname;
        ddlvessel.DataTextField = "FLDVESSELNAME";
        ddlvessel.DataValueField = "FLDVESSELID";
        ddlvessel.DataBind();
        ddlvessel.Items.Insert(0, new ListItem("--Select--", ""));
    }
}
