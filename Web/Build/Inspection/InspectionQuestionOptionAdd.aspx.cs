using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionQuestionOptionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuQuestions.AccessRights = this.ViewState;
            MenuQuestions.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["QUESTIONID"] = "";
                ViewState["OPTIONID"] = "";

                if (Request.QueryString["QUESTIONID"] != null && Request.QueryString["QUESTIONID"].ToString() != string.Empty)
                    ViewState["QUESTIONID"] = Request.QueryString["QUESTIONID"].ToString();

                if (Request.QueryString["OPTIONID"] != null && Request.QueryString["OPTIONID"].ToString() != string.Empty)
                {
                    ViewState["OPTIONID"] = Request.QueryString["OPTIONID"].ToString();

                    cbActive.Enabled = true;
                    BindOption();
                }
                else
                {
                    cbActive.Checked = true;
                    cbActive.Enabled = false;
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindOption()
    {
        try
        {
            if (ViewState["OPTIONID"] != null && ViewState["OPTIONID"].ToString() != string.Empty)
            {
                DataSet ds = new DataSet();
                ds = PhoenixInspectionQuestion.InspectionQuestionOptionEdit(new Guid(ViewState["OPTIONID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtOption.Text = dr["FLDDMSOPTION"].ToString();
                    txtSortorder.Text = dr["FLDSORTORDER"].ToString();

                    if (dr["FLDCORRECTANSWERYN"].ToString() == "1")
                        cb.Checked = true;
                    else
                        cb.Checked = false;

                    if (dr["FLDACTIVEYN"].ToString() == "1")
                        cbActive.Checked = true;
                    else
                        cbActive.Checked = false;

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuQuestions_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (Request.QueryString["OPTIONID"] != null && Request.QueryString["OPTIONID"].ToString() != string.Empty)
                {
                    UpdateQuestionOption();
                }
                else
                {
                    InsertQuestionOption();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void InsertQuestionOption()
    {
        try
        {
            if (!IsValidQuestion(txtOption.Text, txtSortorder.Text))
            {
                ucError.Visible = true;
                return;
            }
            int Correct = cb.Checked == true ? 1 : 0;
            PhoenixInspectionQuestion.InspectionQuestionOptionInsert(General.GetNullableGuid(ViewState["QUESTIONID"].ToString())
                                                                    , General.GetNullableString(txtOption.Text)
                                                                    , Correct
                                                                    , General.GetNullableInteger(txtSortorder.Text));
            ucStatus.Text = "Option Added Successfully";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('OptionEdit', 'Option');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void UpdateQuestionOption()
    {
        try
        {
            if (!IsValidQuestion(txtOption.Text, txtSortorder.Text))
            {
                ucError.Visible = true;
                return;
            }
            int Correct = cb.Checked == true ? 1 : 0;
            int Active = cbActive.Checked == true ? 1 : 0;
            PhoenixInspectionQuestion.InspectionQuestionOptionUpdate(General.GetNullableGuid(ViewState["OPTIONID"].ToString())
                                                                    , General.GetNullableString(txtOption.Text)
                                                                    , Correct
                                                                    , General.GetNullableInteger(txtSortorder.Text)
                                                                    , Active);

            ucStatus.Text = "Option Updated Successfully";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('OptionEdit', 'Option');", true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidQuestion(string Option, string SortOrder)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Option.Trim().Equals(""))
            ucError.ErrorMessage = "Option is required.";
        if (SortOrder.Trim().Equals(""))
            ucError.ErrorMessage = "Sort order is required.";

        return (!ucError.IsError);
    }
}