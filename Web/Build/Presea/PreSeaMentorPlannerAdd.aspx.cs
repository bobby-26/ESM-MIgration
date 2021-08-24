using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Presea_PreSeaMentorPlannerAdd : PhoenixBasePage
{
    int plantype;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] != null)
        {
            if (Request.QueryString["type"] == "1")
            {
                titlepreseaMentorAdd.Text = "Mentor Planner";
                plantype = 1;
            }
            else if (Request.QueryString["type"] == "2")
            {
                titlepreseaMentorAdd.Text = "Buddy Planner";
                plantype = 2;
            }
        }
        PhoenixToolbar MainToolbar = new PhoenixToolbar();

        MainToolbar.AddButton("Save", "SAVE");

        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = MainToolbar.Show();
        if (!IsPostBack)
        {
            txtmentorPlanId.Attributes.Add("style", "display:none");
            BindFaculty();
            BindClassRoom();
            SetData();
        }
    }
    private void SetData()
    {
        string mentorplannerId = null;
        if (Request.QueryString["mentorplannerId"] != null && !string.IsNullOrEmpty(Request.QueryString["mentorplannerId"].ToString()))
            mentorplannerId = Request.QueryString["mentorplannerId"].ToString();
        if (!string.IsNullOrEmpty(mentorplannerId))
        {
            DataTable dt= PhoenixPreseaMentorPlanner.PreSeaMentorPlannerEdit(General.GetNullableInteger(mentorplannerId).Value);
            if (dt.Rows.Count > 0)
            {
                txtmentorPlanId.Text = dt.Rows[0]["FLDMENTORPLANID"].ToString();
                //ddlFaculty.SelectedValue = dt.Rows[0]["FLDMENTORNAME"].ToString();
                ddlFaculty.SelectedValue = dt.Rows[0]["FLDMENTORID"].ToString();
                txtDate.Text = dt.Rows[0]["FLDDATE"].ToString();
                ddlsession.SelectedValue= dt.Rows[0]["FLDSESSION"].ToString();
                txtStrength.Text = dt.Rows[0]["FLDSTRENGTH"].ToString();
                txtRemarks.Text= dt.Rows[0]["FLDREMARKS"].ToString();
                ddlClassRoomAdd.SelectedValue = dt.Rows[0]["FLDCLASSROOM"].ToString();
            }
        }
    }

    private void BindClassRoom()
    {
        ddlClassRoomAdd.Items.Clear();

        DataTable dt = PhoenixPreSeaWeeklyPlanner.ListAvailableClassRoom
            (General.GetNullableDateTime(txtDate.Text), null);
        ddlClassRoomAdd.DataTextField = "FLDROOMNAME";
        ddlClassRoomAdd.DataValueField = "FLDROOMID";
        ddlClassRoomAdd.DataSource = dt;
        ddlClassRoomAdd.DataBind();
        ListItem li = new ListItem("--Select--", "");
        ddlClassRoomAdd.Items.Insert(0,li);

    }
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                int?  classroom;                
                classroom = (General.GetNullableInteger(ddlClassRoomAdd.SelectedValue)==null ||
                            General.GetNullableInteger(ddlClassRoomAdd.SelectedValue).Value > 0) ? 
                            General.GetNullableInteger(ddlClassRoomAdd.SelectedValue) : null;
                //session = (General.GetNullableInteger(ddlsession.SelectedValue).Value > 0) ? General.GetNullableInteger(ddlsession.SelectedValue) : null;

                if (!IsValidPlan())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(txtmentorPlanId.Text))
                    {
                        PhoenixPreseaMentorPlanner.PreSeaMentorPlannerInsert(General.GetNullableInteger(ddlFaculty.SelectedValue).Value
                                                                            , General.GetNullableDateTime(txtDate.Text).Value
                                                                            , classroom
                                                                            , General.GetNullableInteger(txtStrength.Text)
                                                                            , General.GetNullableInteger(ddlsession.SelectedValue).Value
                                                                            , txtRemarks.Text
                                                                            , plantype);
                    }
                    else
                    {
                        PhoenixPreseaMentorPlanner.PreSeaMentorPlannerUpdate(General.GetNullableInteger(ddlFaculty.SelectedValue).Value
                                                                            , General.GetNullableDateTime(txtDate.Text).Value
                                                                            , classroom
                                                                            , General.GetNullableInteger(txtStrength.Text)
                                                                            , General.GetNullableInteger(ddlsession.SelectedValue).Value
                                                                            , txtRemarks.Text
                                                                            , General.GetNullableInteger(txtmentorPlanId.Text).Value
                                                                            , 0);
                    }
                    ucStatus.Text = "Plan Saved successfully";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidPlan()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(txtDate.Text) == null)
            ucError.ErrorMessage = "Date is Required.";

        if (ddlFaculty.SelectedIndex<=0)
            ucError.ErrorMessage = "Faculty is Required.";
        if (ddlsession.SelectedIndex <= 0)
            ucError.ErrorMessage = "Sesson is Required.";

        return (!ucError.IsError);
    }
    protected void BindFaculty()
    {
        DataTable dt = new DataTable();
        dt = PhoenixPreSeaBatchPlanner.ListPreSeaTrainingDepartmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        string DepartmentList = dt.Rows[0]["FLDDEPARTMENTIDLIST"].ToString();

        DataSet ds = PhoenixPreSeaCourseContact.ListPreSeaCourseContactUser(DepartmentList);
        ddlFaculty.DataTextField = "FLDCONTACTNAME";
        ddlFaculty.DataValueField = "FLDUSERCODE";
        ddlFaculty.DataSource = ds;
        ddlFaculty.DataBind();
        //ListItem li = new ListItem("--Select--", "DUMMY");
        //ddlFaculty.Items.Add(li);
        //ddlFaculty.DataBind();
    }
}