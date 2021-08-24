using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCQuestionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

        if (!IsPostBack)
        {
            ViewState["MOCSECTIONID"] = Request.QueryString["MOCSectionId"].ToString();
            ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
            ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();
            ViewState["MOCEVALUATIONID"] = "";

            Binductittle();
            MenuMOC.AccessRights = this.ViewState;
            MenuMOC.MenuList = toolbarmain.Show();
        }
    }
    private void BindGridMOCQuestions()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionMOC.ListMOCQuestion(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , new Guid(ViewState["MOCID"].ToString())
                                         , Int32.Parse((ViewState["MOCSECTIONID"]).ToString()));
        gvMOCQuestion.DataSource = ds;
    }
    protected void gvMOCQuestion_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        BindGridMOCQuestions();
    }

    protected void gvMOCQuestion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindGridMOCQuestions();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MOC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvMOCQuestion.Items)
                {
                    RadCheckBox chkRequiredYNEdit = (((RadCheckBox)gvr.FindControl("chkRequiredYNEdit")));

                    if ((chkRequiredYNEdit.Checked == true) && (chkRequiredYNEdit.Enabled = true))
                    {
                        if ((ViewState["MOCSECTIONID"].Equals("10")))
                        {
                            PhoenixInspectionMOCRequestForChange.MOCSupportRequiredInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , General.GetNullableGuid((ViewState["MOCEVALUATIONID"]).ToString())
                                                                  , new Guid((ViewState["MOCID"]).ToString())
                                                                  , int.Parse(ViewState["Vesselid"].ToString())
                                                                  , Int32.Parse(((RadLabel)gvr.FindControl("lblQuestionid")).Text)
                                                                  , Int32.Parse((((RadCheckBox)gvr.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                                  , null
                                                                  , null
                                                                  );

                            chkRequiredYNEdit.Enabled = false;

                        }
                        if ((ViewState["MOCSECTIONID"].Equals("11")))
                        {
                            PhoenixInspectionMOCRequestForChange.MOCExternalApprovalRequired(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableGuid((ViewState["MOCEVALUATIONID"]).ToString())
                                                                    , new Guid((ViewState["MOCID"]).ToString())
                                                                    , int.Parse(ViewState["Vesselid"].ToString())
                                                                    , Int32.Parse(((RadLabel)gvr.FindControl("lblQuestionid")).Text)
                                                                    , Int32.Parse((((RadCheckBox)gvr.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                                    , null
                                                                    , null);

                            chkRequiredYNEdit.Enabled = false;
                        }

                        if ((ViewState["MOCSECTIONID"].Equals("12")))
                        {
                            PhoenixInspectionMOCRequestForChange.MOCSupportItemRequiredInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableGuid((ViewState["MOCEVALUATIONID"]).ToString())
                                                                    , new Guid((ViewState["MOCID"]).ToString())
                                                                    , int.Parse(ViewState["Vesselid"].ToString())
                                                                    , Int32.Parse(((RadLabel)gvr.FindControl("lblQuestionid")).Text)
                                                                    , Int32.Parse((((RadCheckBox)gvr.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                                    , null
                                                                    , null);

                            chkRequiredYNEdit.Enabled = false;
                        }

                        if ((ViewState["MOCSECTIONID"].Equals("13")))
                        {
                            PhoenixInspectionMOCRequestForChange.MOCShipboardPersonnelAffectedInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableGuid((ViewState["MOCEVALUATIONID"]).ToString())
                                                                    , new Guid((ViewState["MOCID"]).ToString())
                                                                    , int.Parse(ViewState["Vesselid"].ToString())
                                                                    , Int32.Parse(((RadLabel)gvr.FindControl("lblQuestionid")).Text)
                                                                    , Int32.Parse((((RadCheckBox)gvr.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                                    , null
                                                                    , null);
                            chkRequiredYNEdit.Enabled = false;
                        }

                        if ((ViewState["MOCSECTIONID"].Equals("14")))
                        {
                            PhoenixInspectionMOCRequestForChange.MOCShoreBasedPersonnelAffectedInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableGuid((ViewState["MOCEVALUATIONID"]).ToString())
                                                                    , new Guid((ViewState["MOCID"]).ToString())
                                                                    , int.Parse(ViewState["Vesselid"].ToString())
                                                                    , Int32.Parse(((RadLabel)gvr.FindControl("lblQuestionid")).Text)
                                                                    , Int32.Parse((((RadCheckBox)gvr.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                                    , null
                                                                    , null);
                            chkRequiredYNEdit.Enabled = false;
                        }

                        if ((ViewState["MOCSECTIONID"].Equals("15")))
                        {
                            PhoenixInspectionMOCEvaluationProposal.MOCEvaluationProposalInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , General.GetNullableGuid((ViewState["MOCEVALUATIONID"]).ToString())
                                                          , new Guid((ViewState["MOCID"]).ToString())
                                                          , int.Parse(ViewState["Vesselid"].ToString())
                                                          , Int32.Parse(((Label)gvr.FindControl("lblQuestionid")).Text)
                                                          , General.GetNullableInteger(null)
                                                          , null
                                                          , null);
                            chkRequiredYNEdit.Enabled = false;
                        }
                    }
                }
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            else
            {
                ucError.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Binductittle()
    {
        if ((ViewState["MOCSECTIONID"]).ToString() == "10")
        {
            MenuMOC.Title = "Support Required (By Office/ Superintendent, assistance by external parties such as workshop/technician etc) ";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "11")
        {
            MenuMOC.Title = "Identify any external approvals required (Regulatory Authorities/Classification Society/Owner)";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "12")
        {
            MenuMOC.Title = "Identify any Equipment, Stores and Spares, Man Power, Document, Manuals, Drawings/ Plans, Phoenix Modules, etc";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "13")
        {
            MenuMOC.Title = "Shipboard Personnel affected by change";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "14")
        {
            MenuMOC.Title = "Shore Based Personnel affected by change";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "15")
        {
            MenuMOC.Title = "Evaluation of the Proposal Questions";
        }
        if ((ViewState["MOCSECTIONID"]).ToString() == "15")
        {
            MenuMOC.Title = "Evaluation of the Proposal Questions";
        }
        else if ((ViewState["MOCSECTIONID"]).ToString() == "16")
        {
            MenuMOC.Title = "Approval for the Change Questions";
        }
        else if ((ViewState["MOCSECTIONID"]).ToString() == "17")
        {
            MenuMOC.Title = "Implementation Questions";
        }
        else if ((ViewState["MOCSECTIONID"]).ToString() == "18")
        {
            MenuMOC.Title = "Verification on completion of Change Questions";
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //gvMOCQuestion.Rebind();
    }
}
