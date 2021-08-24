using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogCRBTankAdd : PhoenixBasePage
{
    int usercode;
    int vesselid;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (IsPostBack == false)
        {
            //BindTreeData();
            ShowToolBar();
            BindTankDropDown();
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

        if (string.IsNullOrWhiteSpace(txtTankName.Text))
        {
            ucError.ErrorMessage = "Tank Name is Required";
            validatePass = false;
        }
        return validatePass;
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
                    string variant = string.Empty;
                    string code = string.Empty;
                    decimal capacity = Convert.ToDecimal(txtCapacity.Text);
                    string ioppname = txtIoppName.Text;
                    Guid tankType = new Guid(ddlTankType.SelectedItem.Value);
                    PhoenixMarpolLogCRB.InsertCRBLocation(usercode, code, txtTankName.Text, vesselid, txtIoppName.Text, tankType, capacity, txtFrameFrom.Text, txtLateralPosition.Text);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('tankAdd', 'ifMoreInfo');", true);
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

    private void ClearUserInput()
    {
        txtCapacity.Text = null;
        txtIoppName.Text = null;
        ddlTankType.ClearSelection();
    }

    public void BindTankDropDown()
    {
        ddlTankType.DataSource = PhoenixMarpolLogCRB.GetCRBTankType("CRB");
        ddlTankType.DataTextField = "FLDNAME";
        ddlTankType.DataValueField = "FLDLOGTANKTYPEID";
        ddlTankType.DataBind();
    }

    protected void tvwComponent_NodeClickEvent(object sender, EventArgs args)
    {
        try
        {
            RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
            if (e.Node.Value.ToLower() != "_root")
            {
                string selectednode = e.Node.Value.ToString();
                string selectedvalue = e.Node.Text.ToString();
                int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string variant = string.Empty;
                string code = string.Empty;
                string ioppName = string.Empty;
                int vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                //PhoenixElog.InsertLocation(usercode, variant, code, selectedvalue, true, vesselId, ioppName, new Guid(selectednode));
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('tankAdd', 'tank');", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('tankAdd2', 'ifMoreInfo');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}