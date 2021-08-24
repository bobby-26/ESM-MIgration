using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
public partial class Registers_RegistersTestQuestionsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuRegistersTestQstAdd.AccessRights = this.ViewState;
        MenuRegistersTestQstAdd.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            chkCourse.DataSource = PhoenixRegistersTestQuestions.ListCourse();
            chkCourse.DataTextField = "FLDCOURSE";
            chkCourse.DataValueField = "FLDCOURSEID";
            chkCourse.DataBind();
            PopulateDetails(Request.QueryString["type"], Request.QueryString["QuestionId"]);
            txtQuestion.Focus();
        }
    }
    private void PopulateDetails(string type, string QuestionId)
    {
        if (type.ToUpper() == "EDIT" && General.GetNullableInteger(QuestionId) != null)
        {
            DataSet ds = PhoenixRegistersTestQuestions.DocumentQuestionEdit(General.GetNullableInteger(QuestionId));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtQuestion.Text = ds.Tables[0].Rows[0]["FLDQUESTION"].ToString();
                string strlist = "," + ds.Tables[0].Rows[0]["FLDCOURSEIDLIST"].ToString() + ",";
                foreach (ListItem item in chkCourse.Items)
                {
                    item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
                }
                //ucLevel.SelectedQuick = ds.Tables[0].Rows[0]["FLDLEVEL"].ToString();
                chkActiveYN.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
            }
        }
        else
        {
            txtQuestion.Text = string.Empty;
            chkCourse.SelectedValue = string.Empty;
            //ucLevel.SelectedQuick = "";
            chkActiveYN.Checked = false;
        }
    }
    private bool IsValidDocumentQuestion(string Question, string level, string course)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Question.Trim().Equals(""))
            ucError.ErrorMessage = "Question is required.";

        if (course.Trim().Equals("") || course.Trim().Equals(","))
            ucError.ErrorMessage = "Course is required.";

        //if (General.GetNullableInteger(level) == null)
        //    ucError.ErrorMessage = "Level is required.";

        return (!ucError.IsError);
    }
    protected void MenuRegistersTestQstAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strlist = new StringBuilder();
                strlist.Append(",");
                foreach (ListItem item in chkCourse.Items)
                {
                    if (item.Selected == true)
                    {
                        strlist.Append(item.Value.ToString());
                        strlist.Append(",");
                    }

                }
                if (!IsValidDocumentQuestion(txtQuestion.Text.Trim(), null, strlist.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                string ActiveYN = chkActiveYN.Checked ? "1" : "0";
                if (Request.QueryString["type"].ToUpper() == "ADD")
                {
                    PhoenixRegistersTestQuestions.InsertDocumentTestQuestion(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , txtQuestion.Text.Trim(), null, strlist.ToString(), General.GetNullableInteger(ActiveYN));
                    String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
                }
                else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToUpper() == "EDIT"
                    && Request.QueryString["QuestionId"] != null && Request.QueryString["QuestionId"] != string.Empty)
                {
                    PhoenixRegistersTestQuestions.UpdateDocumentTestQuestion(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , int.Parse(Request.QueryString["QuestionId"])
                                        , txtQuestion.Text.Trim(), null, strlist.ToString()
                                        , General.GetNullableInteger(ActiveYN));
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
