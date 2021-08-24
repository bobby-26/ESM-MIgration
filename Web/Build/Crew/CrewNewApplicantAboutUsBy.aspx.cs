using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewNewApplicantAboutUsBy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarAddress = new PhoenixToolbar();
            
            toolbarAddress.AddButton("Save", "SAVE",ToolBarDirection.Right);
            CrewNewApplicantAboutUsByMain.AccessRights = this.ViewState;
            CrewNewApplicantAboutUsByMain.MenuList = toolbarAddress.Show();
            if (!IsPostBack)
            {
                ViewState["FLDABOUTUSBY"] = null;
               

                cblAboutUsBy.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                                                                Convert.ToInt32(PhoenixHardTypeCode.ABOUTUSBY));
                //cblAboutUsBy.DataTextField = "FLDHARDNAME";
                //cblAboutUsBy.DataValueField = "FLDHARDCODE";
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
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {                
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void CrewNewApplicantAboutUsByMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder straboutusby = new StringBuilder();
                foreach (ButtonListItem li in cblAboutUsBy.SelectedItems)
                {
                    straboutusby.Append(li.Value.ToString());
                    straboutusby.Append(",");
                }

                //foreach (RadCheckBoxList item in cblAboutUsBy.Items)
                //{
                //    if (item.Selected == true)
                //    {
                //        straboutusby.Append(item.Value.ToString());
                //        straboutusby.Append(",");
                //    }
                //}
                if (straboutusby.Length > 1)
                {
                    straboutusby.Remove(straboutusby.Length - 1, 1);
                }

                if (straboutusby.ToString() == "")
                {
                    ucError.ErrorMessage = "Please select  atleast one option";
                    ucError.Visible = true;
                    return;
                }
                PhoenixNewApplicantManagement.UpdateEmployeeAboutus(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , Convert.ToString(straboutusby)
                                                        , Convert.ToInt32(Filter.CurrentNewApplicantSelection));
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


            DataTable dtaboutusby = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dtaboutusby.Rows.Count > 0)
            {
                string[] aboutusby = dtaboutusby.Rows[0]["FLDABOUTUSBY"].ToString().Split(',');

                foreach(string item in aboutusby)
                {
                    if (item != "")
                    {
                        //cblAboutUsBy.Items.FindByValue(item).Selected = true;
                        //cblAboutUsBy.SelectedValue = item;
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
