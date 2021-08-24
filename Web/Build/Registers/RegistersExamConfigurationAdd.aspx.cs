using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewOffshore;
public partial class Registers_RegistersExamConfigurationAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuRegistersExamConfigAdd.AccessRights = this.ViewState;
        MenuRegistersExamConfigAdd.MenuList = toolbar.Show();
        
        if (!IsPostBack)
        {
            //ddlLevel.DataSource = PhoenixRegistersExamConfiguration.ListExamLevel();
            //ddlLevel.DataTextField = "FLDEXAMLEVEL";
            //ddlLevel.DataValueField = "FLDEXAMLEVELID";
            //ddlLevel.Items.Insert(0, new ListItem("--Select--", ""));
            //ddlLevel.DataBind();
            PopulateDetails(Request.QueryString["type"], Request.QueryString["ExamId"]);
            txtExam.Focus();
        }
    }
    private bool IsValidConfiguration(string examname, string courseid, string level, string maxquestion, string passmark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (examname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableInteger(courseid) == null)
            ucError.ErrorMessage = "Course is required.";
        //if (General.GetNullableInteger(level) == null)
        //    ucError.ErrorMessage = "Level is required.";
        if (General.GetNullableInteger(maxquestion) == null)
            ucError.ErrorMessage = "No Of Question is required.";
        if (General.GetNullableInteger(passmark) == null)
            ucError.ErrorMessage = "Passmark is required.";


        return (!ucError.IsError);
    }
    private void PopulateDetails(string type, string ExamId)
    {
        if (type.ToUpper() == "EDIT" && General.GetNullableGuid(ExamId) != null && ExamId != string.Empty)
        {
            DataSet ds = PhoenixRegistersExamConfiguration.EditConfiguration(General.GetNullableGuid(ExamId));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtExam.Text = ds.Tables[0].Rows[0]["FLDEXAMNAME"].ToString();
                ddlCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDCOURSEID"].ToString();
                //ddlLevel.SelectedValue = ds.Tables[0].Rows[0]["FLDEXAMLEVELID"].ToString();
                ucMaxQust.Text = ds.Tables[0].Rows[0]["FLDNOOFQUESTIONS"].ToString(); ;
                ucPassMarks.Text = ds.Tables[0].Rows[0]["FLDPASSMARK"].ToString();
                
            }
        }
        else
        {
            txtExam.Text = string.Empty;
            ddlCourse.SelectedCourse = string.Empty;
           // ddlLevel.SelectedValue = string.Empty;
            ucMaxQust.Text = string.Empty;
            ucPassMarks.Text = string.Empty;
            
        }
    }
    protected void MenuRegistersExamConfigAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidConfiguration(txtExam.Text,ddlCourse.SelectedCourse,null,ucMaxQust.Text,ucPassMarks.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (Request.QueryString["type"].ToUpper() == "ADD")
                {
                   PhoenixRegistersExamConfiguration.InsertConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        txtExam.Text, int.Parse(ddlCourse.SelectedCourse)
                        ,null, General.GetNullableInteger(ucMaxQust.Text), General.GetNullableInteger(ucPassMarks.Text));
                    String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
                }
                else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToUpper() == "EDIT"
                    && Request.QueryString["ExamId"] != null && Request.QueryString["ExamId"] != string.Empty)
                {
                    PhoenixRegistersExamConfiguration.UpdateConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                General.GetNullableGuid(Request.QueryString["ExamId"]), txtExam.Text
                                , int.Parse(ddlCourse.SelectedCourse),null
                                , General.GetNullableInteger(ucMaxQust.Text), General.GetNullableInteger(ucPassMarks.Text));
                    String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
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
