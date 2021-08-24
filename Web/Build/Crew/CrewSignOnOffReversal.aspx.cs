using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
public partial class CrewSignOnOffReversal : PhoenixBasePage
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
		SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Confirm", "CONFIRM",ToolBarDirection.Right);
        MenuCrewSignOnOffReversal.AccessRights = this.ViewState;
        MenuCrewSignOnOffReversal.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
        
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeListByFileNo(txtFileNo.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void CrewSignOnOffReversal_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (IsValidRevesalRequest())
                {
                    if (String.IsNullOrEmpty(txtFirstName.Text.Trim()))
                    {
                        ucError.ErrorMessage = "Please verify the entered file number is right before confrim <br/>  by clicking search icon next to File number textbox";
                        ucError.Visible = true;
                        return;
                    }

                    if (drReversalType.SelectedValue == "0")
                    {
                        PhoenixCrewSignOnOffRevesal.ReversaltoOnBoard(txtFileNo.Text.Trim(), int.Parse(ucVessel.SelectedVessel));
                    }
                    else if (drReversalType.SelectedValue == "1")
                    {
                        PhoenixCrewSignOnOffRevesal.ReversaltoOnLeave(txtFileNo.Text.Trim(), int.Parse(ucVessel.SelectedVessel));
                    }
                    ucStatus.Text = "Reversal done Successfully.";
                }
                else
                {
                    ucError.Visible = true;
                    return;
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

    private bool IsValidRevesalRequest()
    {
        Int32 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (drReversalType.SelectedValue == "-1")
            ucError.ErrorMessage = "Reversal type id required.";

        if ((!Int32.TryParse(ucVessel.SelectedVessel, out resultInt)) || ucVessel.SelectedVessel == "0")
            ucError.ErrorMessage = "Vessel is required.";

        if (String.IsNullOrEmpty(txtFileNo.Text.Trim()))
            ucError.ErrorMessage = "File Number is required";
  
        return (!ucError.IsError);

    }

    private bool IsValidFileNoCheck()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(txtFileNo.Text.Trim()))
            ucError.ErrorMessage = "File Number is required";

        return (!ucError.IsError);
    }

  
    protected void ImgBtnValidFileno_Click1(object sender, EventArgs e)
    {
        if (IsValidFileNoCheck())
        {
            SetEmployeePrimaryDetails();
        }
        else
        {
            ucError.Visible = true;
            return;
        }
    }
}
