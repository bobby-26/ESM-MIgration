using System;
using System.Collections.Specialized;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class VesselAccountsOnboardTrainingGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenPick.Attributes.Add("style", "display:none");
            confirm.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ViewState["ONBOARDTRAININGID"] = Request.QueryString["OnboardTrainingid"];
                if (ViewState["ONBOARDTRAININGID"] != null)
                    EditOnboardTraining(new Guid(ViewState["ONBOARDTRAININGID"].ToString()));
                imgOnBoardEmployeeEdit.Attributes.Add("onclick", "showPickList('spnOnBoardEmployee', 'codehelp1', '', '../Common/CommonPickListCrewOnboardWithRank.aspx?VesselId=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "', true); return false;");
                imgOnBoardSelectedEmployeeEdit.Attributes.Add("onclick", "showPickList('spnOnBoardSelectedEmployee', 'codehelp1', '', '../Common/CommonPickListCrewOnboardMultiSelect.aspx?VesselId=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "', true); return false;");
                txtEmployeeIdEdit.Attributes.Add("style", "display:none");
                txtSelectedEmployeeIdEdit.Attributes.Add("style", "display:none");

            }
            MainMenu();
            TextBoxCssSettings();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SaveTraining(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["ONBOARDTRAININGID"] == null)
            {
                PhoenixVesselAccountsOnboardTraining.OnboardTrainingInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , General.GetNullableInteger(ucSubject.SelectedOnboardTrainingTopic)
                                                                            , (ucSubject.SelectedOnboardTrainingTopic == "Dummy" ? txtTraningName.Text : null)
                                                                            , General.GetNullableDateTime(txtFromDate.Text)
                                                                            , General.GetNullableDateTime(txtToDate.Text)
                                                                            , General.GetNullableDecimal(txtDuration.Text)
                                                                            , txtSelectedEmployeeIdEdit.Text
                                                                            , General.GetNullableInteger(txtEmployeeIdEdit.Text)
                                                                            , null
                                                                            , (txtEmployeeIdEdit.Text == "" ? txtTrainerName.Text : null)
                                                                            , txtRemarks.Text);
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOnboardTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTraning((ucSubject.SelectedOnboardTrainingTopic != "Dummy" ? ucSubject.SelectedOnboardTrainingTopic : txtTraningName.Text), txtFromDate.Text, txtToDate.Text, txtSelectedEmployeenameEdit.Text, (txtEmployeeIdEdit.Text == "" ? txtTrainerName.Text : txtEmployeeIdEdit.Text), txtDuration.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ONBOARDTRAININGID"] == null)
                {
                    string[] strEmpid = null;
                    char[] splitchar = { ',' };
                    strEmpid = txtSelectedEmployeeIdEdit.Text.Split(splitchar);
                    int count = 0;
                    for (int i = 0; i <= strEmpid.Length - 1; i++)
                    {
                        if (strEmpid[i] == (txtEmployeeIdEdit.Text) && strEmpid[i] != "")
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        ucError.ErrorMessage = "Training Conducted Person Should not be present in Trainee List";
                        ucError.Visible = true;
                        return;
                    }
                    RadWindowManager1.RadConfirm("The Training Details will be saved once you click the Ok button and you will not be able to make any changes. Hence Kindly Confirm to Save Or click on Cancel to continue Editing?", "confirm", 320, 150, null, "Confirm");

                }
                else
                {
                    ucError.ErrorMessage = "Already Saved.Hence No more Change can be made";
                    ucError.Visible = true;
                }
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["ONBOARDTRAININGID"] = null;
                MainMenu();
                txtTraningName.CssClass = "input_mandatory";
                txtTrainerName.CssClass = "input_mandatory";
                ucSubject.SelectedOnboardTrainingTopic = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtDuration.Text = "";
                txtTraningName.Text = "";
                txtRemarks.Text = "";
                txtEmployeeIdEdit.Text = "";
                txtEmployeepRankEdit.Text = "";
                txtEmployeenameEdit.Text = "";
                txtTrainerName.Text = "";
                txtSelectedEmployeeIdEdit.Text = "";
                txtSelectedEmployeenameEdit.Text = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditOnboardTraining(Guid OnboardTrainingId)
    {
        DataTable dt = PhoenixVesselAccountsOnboardTraining.EditOnboardTraining(OnboardTrainingId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtFromDate.Text = dr["FLDFROMDATE"].ToString();
            txtToDate.Text = dr["FLDTODATE"].ToString();
            txtDuration.Text = dr["FLDDURATION"].ToString();

            ucSubject.SelectedOnboardTrainingTopic = dr["FLDSUBJECTID"].ToString();
            txtTraningName.Text = dr["FLDSUBJECTNAME"].ToString();

            txtRemarks.Text = dr["FLDREMARKS"].ToString();

            txtEmployeeIdEdit.Text = dr["FLDTRAINERID"].ToString();
            txtEmployeepRankEdit.Text = dr["FLDTRAINERRANKNAME"].ToString();
            txtEmployeenameEdit.Text = dr["FLDTRAINERNAME"].ToString();
            txtTrainerName.Text = dr["FLDOTHERTRAINERNAME"].ToString();

            txtSelectedEmployeeIdEdit.Text = dr["FLDTRAINEES"].ToString();
            txtSelectedEmployeenameEdit.Text = dr["FLDTRAINEESNAME"].ToString();
            MainMenu();

        }
    }
    private bool IsValidTraning(string TraningName, string FromDate, string ToDate, string TrainingConductedFor, string TrainingConductedBy, string Duration)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;
        if (TraningName.Trim() == "")
        {
            ucError.ErrorMessage = "Training is required.";
        }
        if (!General.GetNullableDateTime(FromDate).HasValue)
        {
            ucError.ErrorMessage = "From Date is required.";
        }
        if (!General.GetNullableDateTime(ToDate).HasValue)
        {
            ucError.ErrorMessage = "To Date is required.";
        }
        if (Duration.Trim() == "")
        {
            ucError.ErrorMessage = "Duration is required.";
        }
        if (TrainingConductedFor.Trim() == "")
        {
            ucError.ErrorMessage = "Training For is required.";
        }
        if (TrainingConductedBy.Trim() == "")
        {
            ucError.ErrorMessage = "Training By is required.";
        }

        if (!string.IsNullOrEmpty(FromDate))
        {
            if (DateTime.TryParse(FromDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "From Date should be earlier than current date.";
            }
        }
        if (!string.IsNullOrEmpty(ToDate))
        {
            if (DateTime.TryParse(ToDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "To Date  should be earlier than current date.";
            }
        }
        if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
        {
            if (DateTime.Parse(FromDate) > DateTime.Parse(ToDate))
            {
                ucError.ErrorMessage = "From Date should be earlier than Todate.";
            }
        }
        return (!ucError.IsError);
    }
    private void TextBoxCssSettings()
    {
        if (ucSubject.SelectedOnboardTrainingTopic == "Dummy")
            txtTraningName.CssClass = "input_mandatory";
        else
            txtTraningName.CssClass = "input";
        if (txtEmployeeIdEdit.Text == "")
            txtTrainerName.CssClass = "input_mandatory";
        else
            txtTrainerName.CssClass = "input";
    }
    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["ONBOARDTRAININGID"] != null)
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        else
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        }        //toolbar.AddButton("Confirm", "CONFIRM");
        MenuOnboardTraining.AccessRights = this.ViewState;
        MenuOnboardTraining.MenuList = toolbar.Show();
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        ArrayList SelectedEmpName = new ArrayList();
        string selectedempname = ",";
        if (Session["CHECKED_EMPNAME"] != null)
        {
            SelectedEmpName = (ArrayList)Session["CHECKED_EMPNAME"];
            if (SelectedEmpName != null && SelectedEmpName.Count > 0)
            {
                foreach (string index in SelectedEmpName)
                { selectedempname = selectedempname + index + ","; }
                txtSelectedEmployeenameEdit.Text = selectedempname;
            }
        }
        ArrayList SelectedEmpId = new ArrayList();
        string selectedempids = ",";
        if (Session["CHECKED_EMPIDS"] != null)
        {
            SelectedEmpId = (ArrayList)Session["CHECKED_EMPIDS"];
            if (SelectedEmpId != null && SelectedEmpId.Count > 0)
            {
                foreach (int indexn in SelectedEmpId)
                { selectedempids = selectedempids + indexn + ","; }
                txtSelectedEmployeeIdEdit.Text = selectedempids;
            }
        }

        NameValueCollection nvc;
        nvc = Filter.CurrentPickListSelection;
        if (nvc != null)
        {
            txtEmployeenameEdit.Text = nvc.Get("txtEmployeenameEdit");
            txtEmployeepRankEdit.Text = nvc.Get("txtEmployeepRankEdit");
            txtEmployeeIdEdit.Text = nvc.Get("txtEmployeeIdEdit");
        }
    }
}
