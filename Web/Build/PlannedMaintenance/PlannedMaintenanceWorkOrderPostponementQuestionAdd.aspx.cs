using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using Southnests.Phoenix.PlannedMaintenance;
public partial class PlannedMaintenance_PlannedMaintenanceWorkOrderPostponementQuestionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Close", "CLOSE", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuRAPostponementQuestions.AccessRights = this.ViewState;
        MenuRAPostponementQuestions.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            PopulateDetails(Request.QueryString["type"], Request.QueryString["QuestionId"]);
            txtQuestion.Focus();
        }
    }
    private void PopulateDetails(string type, string QuestionId)
    {
        if (type.ToUpper() == "EDIT" && General.GetNullableGuid(QuestionId) != null)
        {
            DataSet ds = PhoenixPlannedMaintenanceWOPostponementQuestion.EditRAPostponementQuestions(General.GetNullableGuid(QuestionId));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtQuestionNo.Text = ds.Tables[0].Rows[0]["FLDQUESTIONNO"].ToString();
                txtQuestion.Text = ds.Tables[0].Rows[0]["FLDQUESTIONNAME"].ToString();
                txtOrderNo.Text = ds.Tables[0].Rows[0]["FLDORDERNO"].ToString();
                chkCommentsYN.Checked = ds.Tables[0].Rows[0]["FLDISCOMMENT"].ToString() == "1" ? true : false;
                chkActiveYN.Checked = ds.Tables[0].Rows[0]["FLDISACTIVE"].ToString() == "1" ? true : false;
            }
        }
        else
        {
            txtQuestion.Text = string.Empty;
            txtOrderNo.Text = string.Empty;
            chkCommentsYN.Checked = false;
            chkActiveYN.Checked = false;
        }
    }
    protected void MenuRAPostponementQuestions_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidQuestion(txtQuestionNo.Text, txtQuestion.Text, txtOrderNo.Text))
            {
                ucError.Visible = true;
                return;
            }
            if (Request.QueryString["type"].ToUpper() == "ADD")
            {
                InsertQuestion(txtQuestionNo.Text, txtOrderNo.Text, txtQuestion.Text,
                        chkCommentsYN.Checked.Equals(true) == true ? 1 : 0, chkActiveYN.Checked.Equals(true) == true ? 1 : 0);
            }
            else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToUpper() == "EDIT"
                    && Request.QueryString["QuestionId"] != null && Request.QueryString["QuestionId"] != string.Empty)
            {
                UpdateQuestion(txtQuestionNo.Text, new Guid(Request.QueryString["QuestionId"]), txtOrderNo.Text, txtQuestion.Text,
                                 chkCommentsYN.Checked.Equals(true) ? 1 : 0, chkActiveYN.Checked.Equals(true) == true ? 1 : 0);
            }
        }
        if (CommandName.ToUpper().Equals("CLOSE"))
        {
            String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
        }
    }
    private void InsertQuestion(string QuestionNo, string OrderNo, string QuestionName, int Commentsyn, int Activeyn)
    {
        try
        {
            PhoenixPlannedMaintenanceWOPostponementQuestion.InsertRAPostponementQuestions(
            General.GetNullableInteger(QuestionNo)
            , General.GetNullableInteger(OrderNo)
            , QuestionName
            , Commentsyn
            , Activeyn
             );
            ucStatus.Text = "Question Added Successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateQuestion(string QuestionNo, Guid Questionid, string OrderNo, string QuestionName, int Commentsyn, int Activeyn)
    {
        try
        {
            PhoenixPlannedMaintenanceWOPostponementQuestion.UpdateRAPostponementQuestions(
            new Guid(Questionid.ToString())
            , General.GetNullableInteger(QuestionNo)
            , General.GetNullableInteger(OrderNo)
            , QuestionName
            , Commentsyn
            , Activeyn);
            ucStatus.Text = "Question Updated Successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidQuestion(string QuestionNo, string QuestionName, string OrderNo)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (QuestionNo == string.Empty)
            ucError.ErrorMessage = "Question No is required.";
        if (QuestionName == string.Empty)
            ucError.ErrorMessage = "Question Name is required.";
        if (OrderNo == string.Empty)
            ucError.ErrorMessage = "Order Number is required.";

        return (!ucError.IsError);
    }
}