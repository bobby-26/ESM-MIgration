using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewReadyToJoin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            toolbarmain.AddButton("New", "NEW");
            CrewJoinTab.AccessRights = this.ViewState;
            CrewJoinTab.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                SetEmployeePrimaryDetails();
                SetEmployeeLastVoyages();
                SetEmployeeLastActivity();
                SetEmployeeReadyToJoin();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewJoinTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if(dce.CommandName.ToString().ToUpper().Equals("SAVE"))
            {
                if (Filter.CurrentCrewSelection != null)
                {
                    SaveEmployeeReadyToJoin();
                }
                else
                {
                    ucError.ErrorMessage = "Select Employee to Update Information";
                    ucError.Visible = true;
                    return;
                }
                
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SetEmployeeReadyToJoin()
    {
        try
        {
            DataTable dt = PhoenixCrewReadyToJoin.ListEmployeeReadyToJoin(Convert.ToInt32(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                ddlVessel.SelectedVessel = dt.Rows[0]["FLDJOININGVESSEL"].ToString();
                ddlContract.SelectedValue = dt.Rows[0]["FLDCONTRACT"].ToString();
                ddlPool.SelectedPool = dt.Rows[0]["FLDJOININGPOOL"].ToString();
                txtJoiningDate.Text = dt.Rows[0]["FLDEXPECTEDJOININGDATE"].ToString();
                ddlJoiningRank.SelectedRank = dt.Rows[0]["FLDJONINGRANK"].ToString();
                ddlJoiningPort.SelectedSeaport = dt.Rows[0]["FLDJOININGPORT"].ToString();

                ViewState["CREWREADYTOJOINID"] = dt.Rows[0]["FLDEMPLOYEEREADYTOJOINID"].ToString(); ;
            }
            else
            {
                ViewState["CREWREADYTOJOINID"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
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
    public void SetEmployeeLastVoyages()
    {
        try
        {
            DataTable dt = PhoenixCrewReadyToJoin.ListEmployeeLastVoyages(Convert.ToInt32(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                txtLastVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                //txtPresentRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtSingOffDate.Text = dt.Rows[0]["FLDSIGNOFFDATE"].ToString();
                //txtDOA.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeeLastActivity()
    {
        try
        {
            DataTable dt = PhoenixCrewReadyToJoin.ListEmployeeLastActivity(Convert.ToInt32(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                txtLastActivity.Text = dt.Rows[0]["ACTIVITY"].ToString();
                txtFromDate.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
                txtTodate.Text = dt.Rows[0]["FLDTODATE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SaveEmployeeReadyToJoin()
    {

        if(!IsValideJoining(txtJoiningDate.Text,ddlVessel.SelectedVessel,ddlJoiningRank.SelectedRank,ddlPool.SelectedPool,ddlJoiningPort.SelectedSeaport))
        {
            ucError.Visible = true;
            return;
        }

        try
        {
            int newemployeejoiningid = PhoenixCrewReadyToJoin.InsertEmployeeReadyToJoin(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                                                               , Convert.ToInt32(Filter.CurrentCrewSelection)
                                                               , Convert.ToInt32(ddlVessel.SelectedVessel)
                                                               , General.GetNullableInteger(ddlContract.SelectedValue)
                                                               , Convert.ToInt32(ddlPool.SelectedPool)
                                                               , Convert.ToDateTime(txtJoiningDate.Text)
                                                               , Convert.ToInt32(ddlJoiningRank.SelectedRank)
                                                               , Convert.ToInt32(ddlJoiningPort.SelectedSeaport)
                                                               );
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public bool IsValideJoining(string joingdate, string joingvessel, string joingrank, string joingpool, string joingport)
    {
       ucError.HeaderMessage = "Please provide the following required information";
        if (joingdate.Trim() == "")
        {
            ucError.ErrorMessage = "Joining Date can not be blank";
        }
        if (joingvessel.Trim() == "Dummy")
        {
            ucError.ErrorMessage = "Select Vessel";
        }
        if (joingrank.Trim() == "Dummy")
        {
            ucError.ErrorMessage = "Select Rank";
        }
        if (joingpool.Trim() == "Dummy")
        {
            ucError.ErrorMessage = "Select Pool";
        }
        if (joingport.Trim() == "Dummy")
        {
            ucError.ErrorMessage = "Select Port";
        }
        return (!ucError.IsError);
    }

}
