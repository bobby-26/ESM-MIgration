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

public partial class CrewOffshoreEvaluationEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        confirm.Attributes.Add("style", "display:none;");
        if (!IsPostBack)
        {
            ViewState["CVSL"] = -1;
            ViewState["CRNK"] = -1;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["RANKID"] = "";

            ViewState["INTERVIEWSTATUS"] = null;


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

            if (Request.QueryString["interviewid"] != null && Request.QueryString["interviewid"].ToString() != "")
                ViewState["interviewid"] = Request.QueryString["interviewid"].ToString();

            txtSuperintendentName.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName;

            SetEmployeePrimaryDetails();


            SetCrewInterviewSummary();
        }

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Finalize", "CONFIRM", ToolBarDirection.Right);
        if (ViewState["INTERVIEWSTATUS"].ToString() != string.Empty && ViewState["INTERVIEWSTATUS"].ToString() == "6")
            toolbarsub.AddButton("Save", "SISAVE", ToolBarDirection.Right);
        else
            toolbarsub.AddButton("Save", "PISAVE", ToolBarDirection.Right);

        toolbarsub.AddButton("Back", "BACK", ToolBarDirection.Right);

        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbarsub.Show();

        BindInterviewQuestions();
        BindGmInterviewQuestions();
    }

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("PISAVE"))
            {
                SaveCrewInterviewSummary();
                BindGmInterviewQuestions();
                BindInterviewQuestions();
                gvGmInterview.Rebind();
                gvInterview.Rebind();

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddButton("Finalize", "CONFIRM", ToolBarDirection.Right);
                if (ViewState["INTERVIEWSTATUS"].ToString() != string.Empty && ViewState["INTERVIEWSTATUS"].ToString() == "6")
                    toolbarsub.AddButton("Save", "SISAVE", ToolBarDirection.Right);
                else
                    toolbarsub.AddButton("Save", "PISAVE", ToolBarDirection.Right);

                CrewMenuGeneral.AccessRights = this.ViewState;
                CrewMenuGeneral.MenuList = toolbarsub.Show();

            }
            if (CommandName.ToUpper().Equals("SISAVE"))
            {
                SaveCrewGMInterviewSummary();
                BindGmInterviewQuestions();
                BindInterviewQuestions();
                gvGmInterview.Rebind();
                gvInterview.Rebind();

            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Interview sheet will be locked and you will not be able to change anything. Are you sure want to continue?";
                RadWindowManager1.RadConfirm("Interview sheet will be locked and you will not be able to change anything.Are you sure want to continue?", "confirm", 320, 150, null, "Confirm");

            }
            if(CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("CrewOffshoreEvaluationList.aspx?empid=" + ViewState["employeeid"] + "&newapp=" + ViewState["newapp"], false);

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
    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (!IsValidateInterview(ddlVessel.SelectedVessel))
            {
                ucError.Visible = true;
                return;
            }

            string status = "";
            if (pnlgminterview.Visible == true)
            {

                //GM Interview question
                if (rblgmstatus.SelectedIndex < 0)
                    ucError.ErrorMessage = "GM status is required";
                if (General.GetNullableString(txtgmcomment.Text) == null)
                    ucError.ErrorMessage = "GM comments is required";
                if (General.GetNullableString(txtgminterviewdate.Text) == null)
                    ucError.ErrorMessage = "GM interview date is required";
                if (General.GetNullableString(txtgmname.Text) == null)
                    ucError.ErrorMessage = "GM name is required";



                if (!IsValidateGMInterview(ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
                status = rblgmstatus.SelectedValue.ToString();
            }
            else
                status = rblStatus.SelectedValue.ToString();

            PhoenixCrewOffshoreInterview.UpdateInterviewSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                     , Convert.ToInt32(ViewState["employeeid"].ToString())
                                     , int.Parse(ViewState["interviewid"].ToString())
                                     , General.GetNullableInteger(status)
                                     , General.GetNullableInteger(ddlVessel.SelectedVessel)
                                     , General.GetNullableInteger(ViewState["RANKID"].ToString())
                                     , General.GetNullableDateTime(txtJoinDate.Text)
                                     , General.GetNullableString(txtSuperintendentName.Text)
                                     , General.GetNullableDateTime(txtDate.Text)
                                     , General.GetNullableString(txtRemaks.Text)
                                     , txtSalAgreed.Text
                                     , General.GetNullableInteger("1")
                                     , General.GetNullableDateTime(txtgminterviewdate.Text)
                                     , General.GetNullableInteger(rblgmstatus.SelectedValue)
                                     , General.GetNullableString(txtgmcomment.Text)
                                     , General.GetNullableString(txtgmname.Text)
                                    );

            ucStatus.Text = "Information Updated.";

            SetCrewInterviewSummary();
            BindInterviewQuestions();
            gvGmInterview.Rebind();
            gvInterview.Rebind();
            //string Script = "";
            //Script += "fnReloadList(null,'ifMoreInfo','keepopen');";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);
            Response.Redirect("CrewOffshoreEvaluationList.aspx?empid=" + ViewState["employeeid"] + "&newapp=" + ViewState["newapp"], false);


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    public void SetCrewInterviewSummary()
    {

        DataTable dt = PhoenixNewApplicantInterviewSummary.ListNewApplicantInterviewSummary(Convert.ToInt32(ViewState["employeeid"].ToString())
            , General.GetNullableInteger(ViewState["interviewid"].ToString()));

        DataSet ds = PhoenixCrewOffshoreInterview.SearchFinalGMInterviewQuestions(General.GetNullableInteger(ViewState["interviewid"].ToString())
                                                                                   , General.GetNullableInteger(ViewState["RANKID"].ToString()));

        if (ds.Tables[0].Rows.Count > 1)
        {
            rblStatus.Items.Clear();
            rblStatus.Items.Insert(0, new ListItem("Rejected", "3"));
            rblStatus.Items.Insert(1, new ListItem("To be considered later", "4"));
            rblStatus.Items.Insert(2, new ListItem("Awaiting GM Approval", "6"));


        }
        else
        {
            rblStatus.Items.Clear();
            rblStatus.Items.Insert(0, new ListItem("Rejected", "3"));
            rblStatus.Items.Insert(1, new ListItem("To be considered later", "4"));
            rblStatus.Items.Insert(2, new ListItem("Approved", "5"));
        }
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDINTERVIEWSTATUS"].ToString() != string.Empty)
            {
                if (rblStatus.Items.FindByValue(dt.Rows[0]["FLDINTERVIEWSTATUS"].ToString()) != null)
                    rblStatus.Items.FindByValue(dt.Rows[0]["FLDINTERVIEWSTATUS"].ToString()).Selected = true;
                if (dt.Rows[0]["FLDINTERVIEWSTATUS"].ToString() == "5" && rblStatus.SelectedIndex < 0)
                    rblStatus.Items.FindByValue("6").Selected = true;
                ViewState["APPROVED"] = 1;
                rblStatus.Enabled = false;
            }
            else
                rblStatus.Enabled = true;

            ViewState["INTERVIEWSTATUS"] = dt.Rows[0]["FLDINTERVIEWSTATUS"].ToString();

            txtJoinDate.Text = dt.Rows[0]["FLDEXPECTEDJOINDATE"].ToString();
            ddlVessel.SelectedVessel = dt.Rows[0]["FLDPLANNEDVESSEL"].ToString();
            ddlRank.DataBind();
            ddlRank.SelectedRank = dt.Rows[0]["FLDOFFSIGNERRANK"].ToString();
            ViewState["RANKID"] = dt.Rows[0]["FLDOFFSIGNERRANK"].ToString();
            txtSuperintendentName.Text = dt.Rows[0]["FLDASSESSMENT"].ToString();
            txtDate.Text = dt.Rows[0]["FLDASSESSMENTDATE"].ToString();
            txtRemaks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
            txtSalAgreed.Text = dt.Rows[0]["FLDSALAGREED"].ToString();

            if (rblStatus.Enabled != false)
            {
                rblStatus.CssClass = "input_mandatory";
            }

            if (string.IsNullOrEmpty(txtSuperintendentName.Text))
                txtSuperintendentName.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName;

            //GM Interview information


            if (dt.Rows[0]["FLDGMSTATUS"].ToString() != string.Empty)
            {
                if (rblgmstatus.Items.FindByValue(dt.Rows[0]["FLDGMSTATUS"].ToString()) != null)
                    rblgmstatus.Items.FindByValue(dt.Rows[0]["FLDGMSTATUS"].ToString()).Selected = true;
                ViewState["APPROVED"] = 1;
                rblgmstatus.Enabled = false;
            }
            else
                rblgmstatus.Enabled = true;

            txtgmname.Text = dt.Rows[0]["FLDGMNAME"].ToString();
            txtgminterviewdate.Text = dt.Rows[0]["FLDGMINTERVIEWDATE"].ToString();
            txtgmcomment.Text = dt.Rows[0]["FLDGMCOMMENT"].ToString();
        }
    }

    public void SaveCrewGMInterviewSummary()
    {
        if (!IsValidateGMInterview(ddlVessel.SelectedVessel))
        {
            ucError.Visible = true;
            return;
        }
        //update the GM Interview questions  
        try
        {
            int allanswered = 0;
            for (int nCurrentRow = 0; nCurrentRow <= gvGmInterview.Items.Count - 2; nCurrentRow = nCurrentRow + 1)
            {

                string dtkey = ((RadLabel)gvGmInterview.Items[nCurrentRow].FindControl("lblDTKey")).Text;
                string questionid = ((RadLabel)gvGmInterview.Items[nCurrentRow].FindControl("lblQuestionId")).Text;
                string answerid = ((RadioButtonList)gvGmInterview.Items[nCurrentRow].FindControl("rblAnswerEdit")).Text;
                string remarks = ((RadTextBox)gvGmInterview.Items[nCurrentRow].FindControl("txtRemarksEdit")).Text;

                //if (!IsValidateInput(answerid))
                //{
                //    ucError.Visible = true;
                //    BindInterviewQuestions();
                //    return;
                //}

                if (General.GetNullableGuid(dtkey) == null && answerid != "")
                {
                    PhoenixCrewOffshoreInterview.InsertFinalGmInterviewQuestions(int.Parse(ViewState["interviewid"].ToString())
                                        , int.Parse(questionid), int.Parse(answerid), General.GetNullableString(remarks),1);
                }
                else if (answerid != "")
                {
                    PhoenixCrewOffshoreInterview.UpdateFinalGmInterviewQuestions(new Guid(dtkey),
                        int.Parse(answerid), General.GetNullableString(remarks), General.GetNullableInteger(ViewState["employeeid"].ToString())
                        , General.GetNullableInteger(ViewState["interviewid"].ToString()),1);
                   
                }
                else if(answerid=="")
                {
                    allanswered = 1;
                }



            }

            if (allanswered == 0)
            {

                PhoenixCrewOffshoreInterview.UpdateInterviewSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt32(ViewState["employeeid"].ToString())
                , int.Parse(ViewState["interviewid"].ToString())
                , General.GetNullableInteger(rblStatus.SelectedValue.ToString())
                , General.GetNullableInteger(ddlVessel.SelectedVessel)
                , General.GetNullableInteger(ViewState["RANKID"].ToString())
                , General.GetNullableDateTime(txtJoinDate.Text)
                , General.GetNullableString(txtSuperintendentName.Text)
                , General.GetNullableDateTime(txtDate.Text)
                , General.GetNullableString(txtRemaks.Text)
                , txtSalAgreed.Text
                , General.GetNullableDateTime(txtgminterviewdate.Text)
               , General.GetNullableInteger(rblgmstatus.SelectedValue)
               , General.GetNullableString(txtgmcomment.Text)
               , General.GetNullableString(txtgmname.Text)
                );
            }
            else
            {
                ucError.ErrorMessage = "All questions must be answered";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public void SaveCrewInterviewSummary()
    {
       

            if (!IsValidateInterview(ddlVessel.SelectedVessel))
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                //update the gridview rows           
                for (int nCurrentRow = 0; nCurrentRow <= gvInterview.Items.Count - 2; nCurrentRow = nCurrentRow + 1)
                {

                    string dtkey = ((RadLabel)gvInterview.Items[nCurrentRow].FindControl("lblDTKey")).Text;
                    string questionid = ((RadLabel)gvInterview.Items[nCurrentRow].FindControl("lblQuestionId")).Text;
                    string answerid = ((RadioButtonList)gvInterview.Items[nCurrentRow].FindControl("rblAnswerEdit")).Text;
                    string remarks = ((RadTextBox)gvInterview.Items[nCurrentRow].FindControl("txtRemarksEdit")).Text;

                    //if (!IsValidateInput(answerid))
                    //{
                    //    ucError.Visible = true;
                    //    BindInterviewQuestions();
                    //    return;
                    //}

                    if (General.GetNullableGuid(dtkey) == null && answerid != "")
                    {
                        PhoenixCrewOffshoreInterview.InsertFinalInterviewQuestions(int.Parse(ViewState["interviewid"].ToString())
                                            , int.Parse(questionid), int.Parse(answerid), General.GetNullableString(remarks),1);
                    }
                    else if (answerid != "")
                    {
                        PhoenixCrewOffshoreInterview.UpdateFinalInterviewQuestions(new Guid(dtkey),
                            int.Parse(answerid), General.GetNullableString(remarks), General.GetNullableInteger(ViewState["employeeid"].ToString())
                            , General.GetNullableInteger(ViewState["interviewid"].ToString()),1);
                    }



                }

                ////update the GM Interview questions           
                //for (int nCurrentRow = 0; nCurrentRow <= gvGmInterview.Items.Count - 2; nCurrentRow = nCurrentRow + 1)
                //{
                //    try
                //    {
                //        string dtkey = ((RadLabel)gvGmInterview.Items[nCurrentRow].FindControl("lblDTKey")).Text;
                //        string questionid = ((RadLabel)gvGmInterview.Items[nCurrentRow].FindControl("lblQuestionId")).Text;
                //        string answerid = ((RadioButtonList)gvGmInterview.Items[nCurrentRow].FindControl("rblAnswerEdit")).Text;
                //        string remarks = ((RadTextBox)gvGmInterview.Items[nCurrentRow].FindControl("txtRemarksEdit")).Text;

                //        //if (!IsValidateInput(answerid))
                //        //{
                //        //    ucError.Visible = true;
                //        //    BindInterviewQuestions();
                //        //    return;
                //        //}

                //        if (General.GetNullableGuid(dtkey) == null && answerid != "")
                //        {
                //            PhoenixCrewOffshoreInterview.InsertFinalGmInterviewQuestions(int.Parse(ViewState["interviewid"].ToString())
                //                                , int.Parse(questionid), int.Parse(answerid), General.GetNullableString(remarks));
                //        }
                //        else if (answerid != "")
                //        {
                //            PhoenixCrewOffshoreInterview.UpdateFinalGmInterviewQuestions(new Guid(dtkey),
                //                int.Parse(answerid), General.GetNullableString(remarks), General.GetNullableInteger(ViewState["employeeid"].ToString())
                //                , General.GetNullableInteger(ViewState["interviewid"].ToString()));
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        ucError.ErrorMessage = ex.Message;
                //        ucError.Visible = true;
                //    }
                //}
                //if (!IsValidate(ddlVessel.SelectedVessel))
                //{
                //    ucError.Visible = true;
                //    return;
                //}


                PhoenixCrewOffshoreInterview.UpdateInterviewSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt32(ViewState["employeeid"].ToString())
                , int.Parse(ViewState["interviewid"].ToString())
                , General.GetNullableInteger(rblStatus.SelectedValue.ToString())
                , General.GetNullableInteger(ddlVessel.SelectedVessel)
                , General.GetNullableInteger(ViewState["RANKID"].ToString())
                , General.GetNullableDateTime(txtJoinDate.Text)
                , General.GetNullableString(txtSuperintendentName.Text)
                , General.GetNullableDateTime(txtDate.Text)
                , General.GetNullableString(txtRemaks.Text)
                , txtSalAgreed.Text
                , null
               , null
               , null
               , null
                );


                ucStatus.Text = "Information Updated.";

                SetCrewInterviewSummary();
                BindInterviewQuestions();
                BindGmInterviewQuestions();

            //string Script = "";
            //Script += "fnReloadList(null,'ifMoreInfo','keepopen');";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);
            Response.Redirect("CrewOffshoreEvaluationList.aspx?empid=" + ViewState["employeeid"] + "&newapp=" + ViewState["newapp"], false);

        }
        catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

    private bool IsValidateInterview(string strVessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        //DateTime resultDate;
        decimal resultDecimal;

        if (General.GetNullableInteger(ddlRank.SelectedRank) == null)
            ucError.ErrorMessage = "Rank of the person to be relieved is required.";

        //else if (DateTime.TryParse(txtJoinDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0 && rblStatus.SelectedValue == "")
        //{
        //    ucError.ErrorMessage = "Expected Joining Date should be later than current date";
        //}
        //if (string.IsNullOrEmpty(txtSalAgreed.Text))
        //    ucError.ErrorMessage = "Salary Agreed is required.";
        if (!string.IsNullOrEmpty(txtSalAgreed.Text) && !decimal.TryParse(txtSalAgreed.Text, out resultDecimal))
        {
            txtSalAgreed.Text = "";
            ucError.ErrorMessage = "Please enter valid salary.";
        }

        if (General.GetNullableString(txtSuperintendentName.Text) == null)
            ucError.ErrorMessage = "Interviewer name is required.";

        if (General.GetNullableDateTime(txtDate.Text) == null)
            ucError.ErrorMessage = "Interview date is required.";
        else if (General.GetNullableDateTime(txtDate.Text) > System.DateTime.Now)
            ucError.ErrorMessage = "Interview date cannot be future date.";

        if (General.GetNullableString(txtRemaks.Text) == null)
            ucError.ErrorMessage = "Comments is required.";


        return (!ucError.IsError);
    }

    private bool IsValidateGMInterview(string strVessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        //DateTime resultDate;

        if (General.GetNullableDateTime(txtgminterviewdate.Text) != null &&
        General.GetNullableDateTime(txtgminterviewdate.Text) < General.GetNullableDateTime(txtDate.Text))
            ucError.ErrorMessage = "GM Interview date is must be greater than interview date.";


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
                    ViewState["status"] = dt.Rows[0]["FLDSTATUSNAME"].ToString();

                    ViewState["RANKID"] = dt.Rows[0]["FLDRANK"].ToString();
                }
            }
            else
            {
                dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
                if (dt.Rows.Count > 0)
                {
                    txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                    txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                    txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                    txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                    txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();

                    ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindInterviewQuestions()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewOffshoreInterview.SearchFinalInterviewQuestions(General.GetNullableInteger(ViewState["interviewid"].ToString())
                                                                                    , General.GetNullableInteger(ViewState["RANKID"].ToString()));
            gvInterview.DataSource = ds;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }




    protected void gvInterview_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindInterviewQuestions();
        ((RadTextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtRemarksEdit")).Focus();
    }

    protected void gvInterview_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            string dtkey = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
            string questionid = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblQuestionId")).Text;
            string answerid = ((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblAnswerEdit")).Text;
            string remarks = ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text;

            if (!IsValidateInput(answerid))
            {
                ucError.Visible = true;
                BindInterviewQuestions();
                return;
            }

            if (General.GetNullableGuid(dtkey) == null)
            {
                PhoenixCrewOffshoreInterview.InsertFinalInterviewQuestions(int.Parse(ViewState["interviewid"].ToString())
                                    , int.Parse(questionid), int.Parse(answerid), General.GetNullableString(remarks));
            }
            else
            {
                PhoenixCrewOffshoreInterview.UpdateFinalInterviewQuestions(new Guid(dtkey),
                    int.Parse(answerid), General.GetNullableString(remarks));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindInterviewQuestions();
    }

    private bool IsValidateInput(string answerid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(answerid) == null)
            ucError.ErrorMessage = "Answer is required to all Questions.";

        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlRank_Changed(object sender, EventArgs e)
    {
        ViewState["RANKID"] = ddlRank.SelectedRank;
        BindInterviewQuestions();
        BindGmInterviewQuestions();
    }

    /* GM Interview questions */
    private void BindGmInterviewQuestions()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewOffshoreInterview.SearchFinalGMInterviewQuestions(General.GetNullableInteger(ViewState["interviewid"].ToString())
                                                                                    , General.GetNullableInteger(ViewState["RANKID"].ToString()));
            gvGmInterview.DataSource = ds;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            //Hide and Unhide GM questions
            if (ds.Tables[0].Rows.Count > 1)
            {
                pnlgminterview.Visible = true;


            }
            else
            {
                pnlgminterview.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGmInterview_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    }

    protected void gvGmInterview_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindGmInterviewQuestions();
    }
    protected void gvGmInterview_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            string dtkey = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
            string questionid = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblQuestionId")).Text;
            string answerid = ((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblAnswerEdit")).Text;
            string remarks = ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text;

            if (!IsValidateInput(answerid))
            {
                ucError.Visible = true;
                BindGmInterviewQuestions();
                return;
            }

            if (General.GetNullableGuid(dtkey) == null)
            {
                PhoenixCrewOffshoreInterview.InsertFinalGmInterviewQuestions(int.Parse(ViewState["interviewid"].ToString())
                                    , int.Parse(questionid), int.Parse(answerid), General.GetNullableString(remarks));
            }
            else
            {
                PhoenixCrewOffshoreInterview.UpdateFinalGmInterviewQuestions(new Guid(dtkey),
                    int.Parse(answerid), General.GetNullableString(remarks));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindGmInterviewQuestions();
    }
    protected void gvGmInterview_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindGmInterviewQuestions();
        ((RadTextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtRemarksEdit")).Focus();
    }

    protected void gvInterview_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindInterviewQuestions();
    }

    protected void gvInterview_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblQuestionId = (RadLabel)e.Item.FindControl("lblQuestionId");
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");

            if (lblQuestionId != null)
            {
                if (string.IsNullOrEmpty(lblQuestionId.Text) || lblQuestionId.Text == "")
                {
                    RadLabel lblQuestion = (RadLabel)e.Item.FindControl("lblQuestion");
                    RadLabel lblScore = (RadLabel)e.Item.FindControl("lblScore");
                    RadLabel lblAnswer = (RadLabel)e.Item.FindControl("lblAnswer");
                    RadLabel lblRemakrs = (RadLabel)e.Item.FindControl("lblRemakrs");
                    RadTextBox txtRemarks = (RadTextBox)e.Item.FindControl("txtRemarksEdit");
                    if (lblQuestion != null) lblQuestion.Font.Bold = true;
                    if (lblAnswer != null) lblAnswer.Font.Bold = true;
                    if (lblScore != null) lblScore.Font.Bold = true;
                    txtRemarks.Visible = false;
                    if (lblRemakrs != null)
                    {
                        lblRemakrs.Font.Bold = true;
                        lblRemakrs.Visible = true;

                    }
                    if (cmdEdit != null) cmdEdit.Visible = false;
                }
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadioButtonList rblAnswerEdit = (RadioButtonList)e.Item.FindControl("rblAnswerEdit");
            if (rblAnswerEdit != null && lblQuestionId != null && lblQuestionId.Text != "")
            {
                rblAnswerEdit.DataSource = PhoenixCrewOffshoreInterviewAnswer.ListInterviewAnswers(int.Parse(dr["FLDQUESTIONID"].ToString()), null);
                rblAnswerEdit.DataTextField = "FLDANSWER";
                rblAnswerEdit.DataValueField = "FLDANSWERID";
                rblAnswerEdit.DataBind();
                rblAnswerEdit.SelectedValue = dr["FLDANSWERID"].ToString();
            }

            if (cmdEdit != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
            }
        }
    }

    protected void gvInterview_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvGmInterview_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGmInterviewQuestions();
    }

    protected void gvGmInterview_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblQuestionId = (RadLabel)e.Item.FindControl("lblQuestionId");
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");

            if (lblQuestionId != null)
            {
                if (string.IsNullOrEmpty(lblQuestionId.Text) || lblQuestionId.Text == "")
                {
                    RadLabel lblQuestion = (RadLabel)e.Item.FindControl("lblQuestion");
                    RadLabel lblScore = (RadLabel)e.Item.FindControl("lblScore");
                    RadLabel lblAnswer = (RadLabel)e.Item.FindControl("lblAnswer");
                    RadLabel lblRemakrs = (RadLabel)e.Item.FindControl("lblRemakrs");
                    RadTextBox txtRemarks = (RadTextBox)e.Item.FindControl("txtRemarksEdit");
                    if (lblQuestion != null) lblQuestion.Font.Bold = true;
                    if (lblAnswer != null) lblAnswer.Font.Bold = true;
                    if (lblScore != null) lblScore.Font.Bold = true;
                    txtRemarks.Visible = false;
                    if (lblRemakrs != null)
                    {
                        lblRemakrs.Font.Bold = true;
                        lblRemakrs.Visible = true;

                    }
                    if (cmdEdit != null) cmdEdit.Visible = false;
                }
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadioButtonList rblAnswerEdit = (RadioButtonList)e.Item.FindControl("rblAnswerEdit");
            if (rblAnswerEdit != null && lblQuestionId != null && lblQuestionId.Text != "")
            {
                rblAnswerEdit.DataSource = PhoenixCrewOffshoreGMInterviewAnswer.ListInterviewAnswers(int.Parse(dr["FLDQUESTIONID"].ToString()), null);
                rblAnswerEdit.DataTextField = "FLDANSWER";
                rblAnswerEdit.DataValueField = "FLDANSWERID";
                rblAnswerEdit.DataBind();
                rblAnswerEdit.SelectedValue = dr["FLDANSWERID"].ToString();
            }

            if (cmdEdit != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
            }
        }
    }

    protected void gvinterviewdocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindInterviewDocuments();
    }
    public void BindInterviewDocuments()
    {
        DataTable dt = PhoenixRegisterInterviewDocumentCheckList.InterviewDocumentStatusList(Convert.ToInt64(ViewState["employeeid"].ToString())
                                                                                            , Convert.ToInt32(ddlRank.SelectedValue.ToString()));

        gvinterviewdocument.DataSource = dt;
    }

    protected void gvinterviewdocument_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {

            RadLabel lbldocstatus = (RadLabel)e.Item.FindControl("lbldocstatus");
            if (lbldocstatus != null)
            {
                if (drv["FLDSTATUS"].ToString().ToUpper() != "AVAILABLE")
                    lbldocstatus.ForeColor = System.Drawing.Color.Red;
            }

        }
    }
}
