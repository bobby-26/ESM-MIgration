using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class Registers_RegistersSignoffFeedBackQuestionsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuRegistersFBQstAdd.AccessRights = this.ViewState;
        MenuRegistersFBQstAdd.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            lstRank.DataSource = PhoenixRegistersRank.ListRank();
            lstRank.DataValueField = "FLDRANKID";
            lstRank.DataTextField = "FLDRANKNAME";
            lstRank.DataBind();
            PopulateDetails(Request.QueryString["type"], Request.QueryString["QuestionId"]);
            txtQuestion.Focus();
        }
    }

    private void PopulateDetails(string type, string QuestionId)
    {
        if (type.ToUpper() == "EDIT" && General.GetNullableInteger(QuestionId) != null)
        {
            DataSet ds = PhoenixRegistersSignoffFeedBackQuestions.EditFeedBackQuestions(General.GetNullableInteger(QuestionId));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtQuestion.Text = ds.Tables[0].Rows[0]["FLDQUESTIONNAME"].ToString();
                txtOrderNo.Text = ds.Tables[0].Rows[0]["FLDORDERNO"].ToString();
                chkCommentsYN.Checked = ds.Tables[0].Rows[0]["FLDISCOMMENTSYN"].ToString() == "1" ? true : false;
                chkActiveYN.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                string strlist = "," + ds.Tables[0].Rows[0]["FLDRANKAPPLICABLE"].ToString() + ",";
                foreach (RadListBoxItem item in lstRank.Items)
                {
                    item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
                }
            }
        }
        else
        {
            txtQuestion.Text = string.Empty;
            txtOrderNo.Text = string.Empty;
            lstRank.SelectedValue = string.Empty;
            chkCommentsYN.Checked = false;
            chkActiveYN.Checked = false;
        }
    }
    protected void MenuRegistersFBQstAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidQuestion(txtQuestion.Text, txtOrderNo.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                StringBuilder strlist = new StringBuilder();
                foreach (RadListBoxItem item in lstRank.Items)
                {
                    if (item.Selected == true)
                    {

                        strlist.Append(item.Value.ToString());
                        strlist.Append(",");
                    }

                }
                if (Request.QueryString["type"].ToUpper() == "ADD")
                {
                    InsertQuestion(txtOrderNo.Text, txtQuestion.Text, strlist.ToString(),
                        chkCommentsYN.Checked==true ? 1 : 0, chkActiveYN.Checked==true ? 1 : 0);
                    String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
                }
                else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToUpper() == "EDIT" 
                    && Request.QueryString["QuestionId"] != null && Request.QueryString["QuestionId"] != string.Empty)
                {
                    UpdateQuestion(Int32.Parse(Request.QueryString["QuestionId"]), txtOrderNo.Text, txtQuestion.Text, strlist.ToString(),
                                     chkCommentsYN.Checked==true ? 1 : 0, chkActiveYN.Checked == true ? 1 : 0);
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
    private bool IsValidQuestion(string QuestionName,string OrderNo)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (QuestionName == string.Empty)
            ucError.ErrorMessage = "Question Name is required.";
        if (OrderNo == string.Empty)
            ucError.ErrorMessage = "Order Number is required.";

        return (!ucError.IsError);
    }
    private void InsertQuestion(string OrderNo, string QuestionName, string RankApplicable, int Commentsyn, int Activeyn)
    {
        PhoenixRegistersSignoffFeedBackQuestions.InsertFeedBackQuestions(General.GetNullableInteger(OrderNo)
            , QuestionName
            , RankApplicable
            , Commentsyn
            , Activeyn
            , PhoenixSecurityContext.CurrentSecurityContext.UserCode
             );
    }
    private void UpdateQuestion(int Questionid, string OrderNo, string QuestionName, string RankApplicable, int Commentsyn, int Activeyn)
    {
        PhoenixRegistersSignoffFeedBackQuestions.UpdateFeedBackQuestions(
            Questionid
            , General.GetNullableInteger(OrderNo)
            , QuestionName
            , RankApplicable
            , Commentsyn
            , Activeyn
            , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

    }
}
