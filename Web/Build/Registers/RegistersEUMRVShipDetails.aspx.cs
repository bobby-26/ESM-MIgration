using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersEUMRVShipDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
            {
                MenuProcedureDetailList.Visible = false;
            }
            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Save", "SAVE",ToolBarDirection.Right);
            TabProcedure.AccessRights = this.ViewState;
            TabProcedure.MenuList = toolbartab.Show();
            if (!IsPostBack)
            {
                if(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())!=null && PhoenixSecurityContext.CurrentSecurityContext.VesselID>0)
                {
                    UcVessel.bind();
                    UcVessel.DataBind();
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
                    
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVShipDetails(General.GetNullableInteger(UcVessel.SelectedVessel));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            txtIMOIdentificationNumber.Text = dt.Rows[0]["FLDIMONUMBER"].ToString();
            txtPortofRegistry.Text = dt.Rows[0]["FLDPORTREGISTERED"].ToString();
            txtHomePort.Text = dt.Rows[0]["FLDHOMEPORT"].ToString();
            txtNameoftheShipOwner.Text = dt.Rows[0]["FLDOWNER"].ToString();
            txtIMOUniqueCompany.Text = dt.Rows[0]["FLDUNIQUCOMPANY"].ToString();
            txtTypeoftheShip.Text = dt.Rows[0]["FLDVESSELTYPE"].ToString();
            txtDeadweight.Text = dt.Rows[0]["FLDNET"].ToString();
            txtGrosstonnage.Text = dt.Rows[0]["FLDGROSSTONNAGE"].ToString();
            txtClassificationSociety.Text = dt.Rows[0]["FLDCLASSNAME"].ToString();
            txtIceClass.Text = dt.Rows[0]["FLDICECLASS"].ToString();
            txtFlagState.Text = dt.Rows[0]["FLDFLAGNAME"].ToString();
            txtAdditionalDescription.Text = dt.Rows[0]["FLDADDITIONALDECRIPTION"].ToString();
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

                PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVShipDetailsInsert(int.Parse(UcVessel.SelectedVessel),
                                                                        txtHomePort.Text.Trim(),
                                                                        txtAdditionalDescription.Text.Trim());

                ucStatus.Text = "Ship details saved successfully.";
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void VesselChange(object sender, EventArgs e)
    {
        try
        {
            BindData();
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
}
