using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewOffshoreEvaluationAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        if (!IsPostBack)
        {
            ViewState["CVSL"] = -1;
            ViewState["CRNK"] = -1;          

            if (Request.QueryString["newapp"] != null && Request.QueryString["newapp"].ToString() != "")
            {
                ViewState["newapp"] = "1";
                ViewState["employeeid"] = General.GetNullableInteger(Filter.CurrentNewApplicantSelection.ToString());
            }
            else
            {
                ViewState["newapp"] = "0";
                ViewState["employeeid"] = General.GetNullableInteger(Filter.CurrentCrewSelection.ToString());
            }

            SetEmployeePrimaryDetails();
        }

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Save", "SAVE",ToolBarDirection.Right);
        CrewMenuGeneral.MenuList = toolbarsub.Show();
    }

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (!IsValidate(ddlVessel.SelectedVessel))
            {
                ucError.Visible = true;
                return;
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveCrewInterviewSummary();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //SetCrewInterviewSummary();
    }

    public void SaveCrewInterviewSummary()
    {
        string Script = "";
        Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
        try
        {
            int? interviewid = null;
            PhoenixCrewOffshoreInterview.InsertInterviewSummary(Convert.ToInt32(ViewState["employeeid"].ToString())
                ,null
                ,General.GetNullableInteger(ddlRank.SelectedRank)
                ,General.GetNullableDateTime(txtJoinDate.Text)
                ,ref interviewid
                );



            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);
            Response.Redirect("CrewOffshoreEvaluationList.aspx?empid=" + ViewState["employeeid"] + "&newapp=" + ViewState["newapp"], false);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidate(string strVessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        //if (!int.TryParse(strVessel, out resultInt))
        //{
        //    ucError.ErrorMessage = "Expected Joining Vessel is required.";
        //}
        //else if (int.TryParse(strVessel, out resultInt))
        //{
        //    DataSet ds = PhoenixRegistersVessel.EditVessel(Convert.ToInt32(strVessel));
        //    if (ds.Tables[0].Rows[0]["FLDENGINETYPE"].ToString() == "")
        //        ucError.ErrorMessage = "Please go to vessel master and map engine type for the vessel " + ds.Tables[0].Rows[0]["FLDVESSELNAME"];
        //}

        if (General.GetNullableInteger(ddlRank.SelectedRank) == null)
            ucError.ErrorMessage = "Rank of the person to be relieved is required.";

        //if (!DateTime.TryParse(txtJoinDate.Text, out resultDate))
        //{
        //    ucError.ErrorMessage = "Expected Joining Date is required.";
        //}
        else if (DateTime.TryParse(txtJoinDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "Expected Joining Date should be later than current date";
        }

        return (!ucError.IsError);
    }

    private void ResetFormControlValues(Control parent)
    {

        try
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = 0;
                            break;
                        case "System.Web.UI.WebControls.ListBox":
                            ((ListBox)c).SelectedIndex = 0;
                            break;

                    }
                }
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
            DataTable dt = new DataTable();
            if (ViewState["newapp"].ToString().Equals("1"))
            {
                dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
                if (dt.Rows.Count > 0)
                {
                    txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();
                    txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                    txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                    txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                    txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                    ddlRank.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
                    ViewState["status"] = dt.Rows[0]["FLDSTATUSNAME"].ToString();
                }
            }
            else
            {
                dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
                if (dt.Rows.Count > 0)
                {
                    txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                    txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                    ddlRank.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
                    txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                    txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                    txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
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
