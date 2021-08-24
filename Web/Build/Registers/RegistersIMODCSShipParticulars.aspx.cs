using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersIMODCSShipParticulars : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

           PhoenixToolbar toolbarmain = new PhoenixToolbar();
           toolbarmain.AddButton("Back", "BACK",ToolBarDirection.Right);
           MenuProcedureDetailList.AccessRights = this.ViewState;
           MenuProcedureDetailList.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
            TabProcedure.AccessRights = this.ViewState;
            TabProcedure.MenuList = toolbartab.Show();
            if (!IsPostBack)
            {              
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

        ds = PhoenixRegistersEUMRVEmisionSource.RegistersIMODCSShipDetails(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            txtIMOIdentificationNumber.Text = dt.Rows[0]["FLDIMONUMBER"].ToString();
            txtIMOUniqueCompany.Text = dt.Rows[0]["FLDUNIQUCOMPANY"].ToString();
            txtDeadweight.Text = dt.Rows[0]["FLDDWT"].ToString();
            txtnt.Text = dt.Rows[0]["FLDNET"].ToString();
            txtGrosstonnage.Text = dt.Rows[0]["FLDGROSSTONNAGE"].ToString();
            txtIceClass.Text = dt.Rows[0]["FLDICECLASS"].ToString();
            txtFlagState.Text = dt.Rows[0]["FLDFLAGNAME"].ToString();
            txtEEDI.Text = dt.Rows[0]["FLDEEDI"].ToString();
            txtEIV.Text = dt.Rows[0]["FLDEIV"].ToString();
            txtvessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            txtshiptype.Text= dt.Rows[0]["FLDVESSELTYPE"].ToString();
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

                PhoenixRegistersEUMRVEmisionSource.RegistersIMODCSShipDetailsInsert(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                       ,General.GetNullableDecimal(txtEEDI.Text.Trim())
                                                                       , General.GetNullableDecimal(txtEIV.Text.Trim()));

                ucStatus.Text = "Ship details saved successfully.";
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
        if (CommandName.ToUpper().Equals("BACK"))
        {
           Response.Redirect("../VesselPosition/VesselPositionIMODCSConfiguration.aspx?Lanchfrom=0");
        }
    }
}
