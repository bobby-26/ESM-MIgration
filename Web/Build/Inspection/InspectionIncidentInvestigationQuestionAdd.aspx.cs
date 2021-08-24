using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InspectionIncidentInvestigationQuestionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["QUESTIONID"] = "";

                if (Request.QueryString["QUESTIONID"] != null && Request.QueryString["QUESTIONID"].ToString() != string.Empty)
                    ViewState["QUESTIONID"] = Request.QueryString["QUESTIONID"].ToString();

                Bindaccidentcat();
                Bindnearmisscat();
                BindQuestion();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Bindaccidentcat()
    {
        chkaccidentcat.Items.Clear();

        chkaccidentcat.DataSource = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(1);
        chkaccidentcat.DataTextField = "FLDNAME";
        chkaccidentcat.DataValueField = "FLDINCIDENTNEARMISSCATEGORYID";
        chkaccidentcat.DataBind();
    }

    protected void Bindnearmisscat()
    {
        chknearmisscat.Items.Clear();

        chknearmisscat.DataSource = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(2);
        chknearmisscat.DataTextField = "FLDNAME";
        chknearmisscat.DataValueField = "FLDINCIDENTNEARMISSCATEGORYID";
        chknearmisscat.DataBind();
    }

    protected void BindQuestion()
    {
        if (ViewState["QUESTIONID"] != null && ViewState["QUESTIONID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixInspectionInvestigationQuestions.InvestigationQuestionedit(new Guid(ViewState["QUESTIONID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtQuestion.Text = dr["FLDQUESTION"].ToString();
                txtFindingText.Text = dr["FLDFINDINGTEXT"].ToString();
                General.BindCheckBoxList(chkaccidentcat, dr["FLDACCIDENTCATEGORYLIST"].ToString());
                General.BindCheckBoxList(chknearmisscat, dr["FLDNEARMISSCATEGORYLIST"].ToString());
            }
        }
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["QUESTIONID"] != null && ViewState["QUESTIONID"].ToString() != string.Empty)
                    UpdateQuestion();
                else
                    InsertQuestion();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InsertQuestion()
    {
        if (!IsValidQuestion())
        {
            ucError.Visible = true;
            return;
        }
        PhoenixInspectionInvestigationQuestions.InvestigationQuestionInsert(General.GetNullableString(txtQuestion.Text.Trim())
                                                                            , General.GetNullableString(txtFindingText.Text.Trim())
                                                                            , General.GetNullableString(General.ReadCheckBoxList(chkaccidentcat))
                                                                            , General.GetNullableString(General.ReadCheckBoxList(chknearmisscat)));

        ucStatus.Text = "Information updated.";
        BindQuestion();
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdateQuestion()
    {
        if (!IsValidQuestion())
        {
            ucError.Visible = true;
            return;
        }
        PhoenixInspectionInvestigationQuestions.InvestigationQuestionUpdate(new Guid(ViewState["QUESTIONID"].ToString())
                                                                            , General.GetNullableString(txtQuestion.Text.Trim())
                                                                            , General.GetNullableString(txtFindingText.Text.Trim())
                                                                            , General.GetNullableString(General.ReadCheckBoxList(chkaccidentcat))
                                                                            , General.GetNullableString(General.ReadCheckBoxList(chknearmisscat)));

        ucStatus.Text = "Information updated.";
        BindQuestion();
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidQuestion()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtQuestion.Text.Trim()) == null)
            ucError.ErrorMessage = "Question is required.";

        if (General.GetNullableString(txtFindingText.Text) == null)
            ucError.ErrorMessage = "Finding Text is required.";

        return (!ucError.IsError);
    }
}