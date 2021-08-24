using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;
using System.Text;

public partial class PreSeaBatchManageDetails : PhoenixBasePage
{
    string strBatchId;

    protected void Page_Load(object sender, EventArgs e)
    {


        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            strBatchId = Filter.CurrentPreSeaBatchManagerSelection;
            if (!IsPostBack)
            {
                PhoenixToolbar MainToolbar = new PhoenixToolbar();

                MainToolbar.AddButton("Batch", "BATCH");
                MainToolbar.AddButton("Details", "DETAIL");
               // MainToolbar.AddButton("Events", "EVENT");
                MainToolbar.AddButton("Semester", "SEMESTER");
                MainToolbar.AddButton("Subjects", "SUBJECTS");
                MainToolbar.AddButton("Exam", "EXAM");
                //MainToolbar.AddButton("Contact", "CONTACT");

                MenuBatchPlanner.AccessRights = this.ViewState;
                MenuBatchPlanner.MenuList = MainToolbar.Show();

                MenuBatchPlanner.SelectedMenuIndex = 1;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");
                MenuPreSea.AccessRights = this.ViewState;
                MenuPreSea.MenuList = toolbar.Show();

                BindFacultyTDC(ddlFacultyTDC);
                BindFacultyCourse(ddlFacultyCourse);
                SetPrimaryBatchDetails();
                FillBatchDetailsFields();
               
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BatchPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(Filter.CurrentPreSeaBatchManagerSelection))
            {
                ucError.ErrorMessage = "Please select batch to view/modify plan details.";
                ucError.Visible = true;
                return;
            }
            else
            {

                DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
                if (dce.CommandName.ToUpper().Equals("BATCH"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchManager.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("DETAIL"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchManageDetails.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("EVENT"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchEvents.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchSemester.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchSubjects.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("EXAM"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchExamSchedule.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("CONTACT"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchContact.aspx");
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void FillBatchDetailsFields()
    {
        DataTable dt = PhoenixPreSeaBatchManager.ListBatchDetails(General.GetNullableInteger(strBatchId));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucMinLimit.Text = dr["FLDMINNOOFSTUDENTS"].ToString();
            ucMaxLimit.Text = dr["FLDMAXNOOFSTUDENTS"].ToString();
            txtNoOfSem.Text = dr["FLDNOOFSEMESTER"].ToString();
            txtNoofSection.Text = dr["FLDNOOFSECTION"].ToString();
            txtNoDivperSection.Text = dr["FLDNOOFDIVPERSECTION"].ToString();
            ddlFacultyTDC.SelectedValue = dr["FLDTDCINCHARGE"].ToString();
            ddlFacultyCourse.SelectedValue = dr["FLDCOURSEINCHARGE"].ToString();
        }
        else
            ResetFields();
    }

    private void ResetFields()
    {
        string empty = String.Empty;
        ucMinLimit.Text = empty;
        ucMaxLimit.Text = empty;
        txtNoOfSem.Text = empty;
        txtNoDivperSection.Text = "";
        txtNoofSection.Text = "";
        ddlFacultyCourse.SelectedValue = "DUMMY";
        ddlFacultyTDC.SelectedValue = "DUMMY";
    }

    public bool IsValidBatchManagerDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(strBatchId) == null)
            ucError.ErrorMessage = "Select a Batch from Batch Manager";

        if (!General.GetNullableInteger(txtNoDivperSection.Text).HasValue)
            ucError.ErrorMessage = "No of Practical div per sect is required";

        if (!General.GetNullableInteger(txtNoofSection.Text).HasValue)
            ucError.ErrorMessage = "No of section is required";

        if (!General.GetNullableInteger(ddlFacultyTDC.SelectedValue).HasValue)
            ucError.ErrorMessage = "Select a faculty for TDC In-Charge";

        if (!General.GetNullableInteger(ddlFacultyCourse.SelectedValue).HasValue)
            ucError.ErrorMessage = "Select a faculty for Course In-Charge";
        return (!ucError.IsError);

    }

    private void SetPrimaryBatchDetails()
    {
        DataSet ds = PhoenixPreSeaBatch.EditBatch(int.Parse(strBatchId));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtBatchName.Text = ds.Tables[0].Rows[0]["FLDBATCH"].ToString();
        }
    }

    protected void BindFacultyTDC(DropDownList ddl)
    {
        if (General.GetNullableInteger(strBatchId).HasValue)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem("--Select--", "DUMMY");
            ddl.Items.Add(li);

            //DataSet ds = PhoenixPreSeaBatchManager.ListPreSeaBatchContact(Convert.ToInt32(ucBatch.SelectedBatch));
            DataSet ds = PhoenixPreSeaBatchAdmissionContact.ListPreSeaBatchAdmissionContact(Convert.ToInt32(strBatchId));
            ddl.DataSource = ds.Tables[0];
            ddl.DataTextField = "FLDCONTACTNAME";
            ddl.DataValueField = "FLDUSERCODE";
            ddl.DataBind();
        }
    }

    protected void BindFacultyCourse(DropDownList ddl)
    {
        if (General.GetNullableInteger(strBatchId).HasValue)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem("--Select--", "DUMMY");
            ddl.Items.Add(li);

            //DataSet ds = PhoenixPreSeaBatchManager.ListPreSeaBatchContact(Convert.ToInt32(ucBatch.SelectedBatch));
            DataSet ds = PhoenixPreSeaBatchAdmissionContact.ListPreSeaBatchAdmissionContact(Convert.ToInt32(strBatchId));
            ddl.DataSource = ds.Tables[0];
            ddl.DataTextField = "FLDCONTACTNAME";
            ddl.DataValueField = "FLDUSERCODE";
            ddl.DataBind();
        }
    }


    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidBatchManagerDetails())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPreSeaBatchManager.UpdateBatchDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(strBatchId)
                    , General.GetNullableInteger(txtNoDivperSection.Text)
                    , General.GetNullableInteger(txtNoofSection.Text)
                    , General.GetNullableInteger (ddlFacultyTDC.SelectedValue)
                    , General.GetNullableInteger(ddlFacultyCourse.SelectedValue)
                    );

                ucStatus.Text = "Batch Plan Details saved Successfully.";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
