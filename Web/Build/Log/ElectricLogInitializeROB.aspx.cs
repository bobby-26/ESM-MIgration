using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ASP;

public partial class Log_ElectricLogInitializeROB : PhoenixBasePage
{
    Guid LocationId;
    int usercode;
    int vesselid;
    Decimal Rob;

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
            ShowToolBar();

            if (String.IsNullOrWhiteSpace(Request.QueryString["ROB"]) == false)
            {
                Rob = Convert.ToDecimal(Request.QueryString["ROB"]);
                txtRob.Text = Rob.ToString();
            }
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        logROBEdit.AccessRights = this.ViewState;
        logROBEdit.MenuList = toolbarmain.Show();
    }

    protected void logROBEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                decimal rob = txtRob.Text == "" ? 0 : Convert.ToDecimal(txtRob.Text);
                if (IsValidReport(rob))
                {
                    //PhoenixElog.LocationROBUpdate(usercode, LocationId, rob, vesselid);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('tankROB', 'tank');", true);
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

    private bool IsValidReport(decimal rob)
    {
        bool validatePass = true;
        ucError.HeaderMessage = "Please provide the following required information";
        if (rob <= 0)
        {
            ucError.ErrorMessage = "ROB cannot be empty";
            validatePass = false;
        }
        return validatePass;
    }

    private void ClearUserInput()
    {
        txtRob.Text = null;
    }
}