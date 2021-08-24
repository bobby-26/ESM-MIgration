using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersEUMRVFuelConcumptionFreight : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["Lanchfrom"] != null)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                if (Request.QueryString["Lanchfrom"].ToString() == "0")
                    toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);
                if (Request.QueryString["Lanchfrom"].ToString() == "1")
                    toolbarmain.AddButton("Ship Specific Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);
                MenuProcedureDetailList.AccessRights = this.ViewState;
                MenuProcedureDetailList.MenuList = toolbarmain.Show();
            }
            else
                MenuProcedureDetailList.Visible = false;

            if (Request.QueryString["view"] == null)
            {
                PhoenixToolbar toolbartab = new PhoenixToolbar();
                toolbartab.AddButton("Save", "SAVE",ToolBarDirection.Right);
                TabProcedure.AccessRights = this.ViewState;
                TabProcedure.MenuList = toolbartab.Show();
            }
            else
            {
                txtAreaFreight.Enabled = false;
                //txtDataSources.Enabled = false;
                txtFormulae.Enabled = false;
                txtFuelConsumption.Enabled = false;
                txtLocationRecord.Enabled = false;
                txtMassFreight.Enabled = false;
                txtNameSystem.Enabled = false;
                txtPostionResponsible.Enabled = false;
                ddlAppliedAllocation.Enabled = false;
            }
            if (!IsPostBack)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(6);
                ddlAppliedAllocation.DataSource = ds;
                ddlAppliedAllocation.DataTextField = "FLDNAME";
                ddlAppliedAllocation.DataValueField = "FLDEUMRVCATEGORIESID";
                ddlAppliedAllocation.DataBind();
                ddlAppliedAllocation.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlAppliedAllocation.SelectedIndex = 0;
                ViewState["DTKEY"] = null;
                ProcedureDetailEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }
    }
    protected void TabProcedure_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixRegistersEUMRVfreightandpassenger.EUMRVfreightandpassengerInsert(General.GetNullableInteger(ddlAppliedAllocation.SelectedValue)
                  , General.GetNullableString(txtMassFreight.Text.Trim()), General.GetNullableString(txtAreaFreight.Text.Trim()), General.GetNullableString(txtFuelConsumption.Text.Trim()), General.GetNullableString(txtPostionResponsible.Text.Trim())
                  , General.GetNullableString(txtFormulae.Text.Trim()), null, General.GetNullableString(txtLocationRecord.Text.Trim())
                  , General.GetNullableString(txtNameSystem.Text.Trim()), General.GetNullableGuid(ViewState["DTKEY"] == null ? null : ViewState["DTKEY"].ToString()));
                ucstatus.Text = "Procedure saved successfully.";
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ProcedureDetailEdit()
    {
        DataSet ds = PhoenixRegistersEUMRVfreightandpassenger.EUMRVfreightandpassengerEdit("C.2.9");
        DataTable dt = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];
        if (dt.Rows.Count > 0)
        {
            txtMassFreight.Text = dt.Rows[0]["FLDMASSDESC"].ToString();
            txtAreaFreight.Text = dt.Rows[0]["FLDAREADESC"].ToString();
            txtFuelConsumption.Text = dt.Rows[0]["FLDFUELCONSUMPTION"].ToString();
            txtPostionResponsible.Text = dt.Rows[0]["FLDPERSONRESPONDIBLE"].ToString();
            txtFormulae.Text = dt.Rows[0]["FLDFOMULAE"].ToString();
            txtLocationRecord.Text = dt.Rows[0]["FLDLOCATION"].ToString();
            txtNameSystem.Text = dt.Rows[0]["FLDNAMEITSYSTEM"].ToString();
            
            ddlAppliedAllocation.SelectedValue = dt.Rows[0]["FLDAPPLIEDALLOCATION"].ToString();
            ViewState["DTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
        }
        else
        {
            Reset();
        }
        if (dt2.Rows.Count > 0)
        {

            if (dt2.Rows[0]["FLDAPPLICABLEYN"].ToString() == "0")
            {
                chkapplicableYN.Checked = true;
                TabProcedure.Visible = false;
            }
            else
            {
                chkapplicableYN.Checked = false;
                TabProcedure.Visible = true;
            }


            if (dt2.Rows[0]["FLDOPTIONALYN"].ToString() == "0")
            {
                lblapplicableyn.Visible = false;
                chkapplicableYN.Visible = false;
            }
        }


    }
    protected void chkapplicableYN_CheckedChanged(object sender, EventArgs e)
    {
        if (chkapplicableYN.Checked == true)
        {
            TabProcedure.Visible = false;
            PhoenixRegistersEUMRVfreightandpassenger.EUMRVProcedureConfigUpdate(new Guid(Request.QueryString["ProcedureID"].ToString()), 0);
            ProcedureDetailEdit();
        }
        else
        {
            TabProcedure.Visible = true;
            PhoenixRegistersEUMRVfreightandpassenger.EUMRVProcedureConfigUpdate(new Guid(Request.QueryString["ProcedureID"].ToString()), 1);
            ProcedureDetailEdit();
        }

    }
    protected void Reset()
    {
        txtAreaFreight.Text = "";
        txtFormulae.Text = "";
        txtFuelConsumption.Text = "";
        txtLocationRecord.Text = "";
        txtMassFreight.Text = "";
        txtNameSystem.Text = "";
        txtPostionResponsible.Text = "";
        ddlAppliedAllocation.SelectedValue = "Dummy";
        ViewState["DTKEY"] = "";
    }


}
