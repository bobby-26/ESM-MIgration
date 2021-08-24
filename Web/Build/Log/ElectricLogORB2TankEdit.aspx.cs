using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2TankEdit : PhoenixBasePage
{
    Guid LocationId;
    int usercode;
    int vesselid;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (Request.QueryString["LocationID"] != null)
        {
            LocationId = new Guid(Request.QueryString["LocationID"]);
        }


        if (IsPostBack == false)
        {
            BindTankDropDown();
            GetLocationDetails();
            ShowToolBar();
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        logTankEdit.AccessRights = this.ViewState;
        logTankEdit.MenuList = toolbarmain.Show();
    }

    public void BindTankDropDown()
    {

        ddlTankType.DataSource = PhoenixMarbolLogORB2.GetORB2TankType("ORB2");
        ddlTankType.DataTextField = "FLDNAME";
        ddlTankType.DataValueField = "FLDLOGTANKTYPEID";
        ddlTankType.DataBind();
    }

    protected void logTankEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidReport())
                {
                    decimal capacity = Convert.ToDecimal(txtCapacity.Text);
                    string ioppname = txtIoppName.Text;
                    Guid tankType = new Guid(ddlTankType.SelectedItem.Value);
                    PhoenixMarbolLogORB2.EditORB2Location(usercode, LocationId, ioppname, capacity, vesselid, tankType, txtFrameFrom.Text, txtLateralPosition.Text);
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('tankEdit', 'tank');", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('tankEdit', 'ifMoreInfo');", true);
                }
                else
                {
                    ucError.Visible = true;
                }
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearUserInput();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void GetLocationDetails()
    {

        DataSet ds = PhoenixMarbolLogORB2.LocationORB2Select(usercode, LocationId);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            txtIoppName.Text = row["FLDIOPPNAME"].ToString();
            txtCapacity.Text = row["FLDCAPACITY"].ToString();
            RadComboBoxItem item = ddlTankType.FindItemByText(row["FLDNAME"].ToString());
            txtLateralPosition.Text = row["FLDLATERALPOSITION"].ToString();
            txtFrameFrom.Text = row["FLDFRAMESFROMTO"].ToString();
            item.Selected = true;
        }
    }

    private bool IsValidReport()
    {
        bool validatePass = true;
        decimal capacity = txtCapacity.Text == "" ? 0 : Convert.ToDecimal(txtCapacity.Text);
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrWhiteSpace(txtIoppName.Text))
        {
            ucError.ErrorMessage = "IOPP Name Cannot be Empty";
            validatePass = false;
        }

        if (capacity == 0 || capacity <= 0)
        {
            ucError.ErrorMessage = "Capacity Cannot be Zero or Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(ddlTankType.Text))
        {
            ucError.ErrorMessage = "Tank Type is Required";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(txtLateralPosition.Text))
        {
            ucError.ErrorMessage = "Lateral Position is Required";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(txtFrameFrom.Text))
        {
            ucError.ErrorMessage = "Frame From is Required";
            validatePass = false;
        }
        return validatePass;
    }

    private void ClearUserInput()
    {
        txtCapacity.Text = null;
        txtIoppName.Text = null;
        ddlTankType.ClearSelection();
        txtFrameFrom.Text = null;
        txtIoppName.Text = null;
    }
}