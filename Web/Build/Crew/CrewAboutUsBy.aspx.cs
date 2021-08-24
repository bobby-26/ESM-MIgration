using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewAboutUsBy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ViewState["FLDABOUTUSBY"] = null;

            PhoenixToolbar toolbarAddress = new PhoenixToolbar();
            toolbarAddress.AddButton("Save", "SAVE", ToolBarDirection.Right);
            CrewAboutUsByMain.AccessRights = this.ViewState;
            CrewAboutUsByMain.MenuList = toolbarAddress.Show();
            if (!IsPostBack)
            {
                cblAboutUsBy.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixHardTypeCode.ABOUTUSBY));
                cblAboutUsBy.DataBind();

                SetEmployeePrimaryDetails();
                AboutUsByList();
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
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
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

    protected void CrewAboutUsByMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                string straboutusby = string.Empty;
                
                foreach (ButtonListItem li in cblAboutUsBy.Items)
                {
                    straboutusby += li.Selected ? li.Value + "," : string.Empty;
                }
                straboutusby = straboutusby.TrimEnd(',');

                if (straboutusby.ToString() == "")
                {
                    ucError.ErrorMessage = "Please select  atleast one option";
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewPersonal.UpdateEmployeeAboutus(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , Convert.ToString(straboutusby)
                                                          , Convert.ToInt32(Filter.CurrentCrewSelection));

                ucStatus.Text = "Saved Successfully";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AboutUsByList()
    {
        try
        {
            DataTable dtaboutusby = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dtaboutusby.Rows.Count > 0)
            {
                string[] aboutusby = dtaboutusby.Rows[0]["FLDABOUTUSBY"].ToString().Split(',');

                foreach (string item in aboutusby)
                {
                    if (item != "")
                    {
                        foreach (ButtonListItem li in cblAboutUsBy.Items)
                        {
                            if (li.Value == item)
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
                if (aboutusby[0] != "")
                {
                    ViewState["FLDABOUTUSBY"] = dtaboutusby.Rows[0]["FLDABOUTUSBY"].ToString();
                }
                else
                {
                    ViewState["FLDABOUTUSBY"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
