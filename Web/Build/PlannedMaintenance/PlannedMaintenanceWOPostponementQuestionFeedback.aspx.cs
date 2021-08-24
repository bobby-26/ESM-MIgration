using Southnests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class PlannedMaintenance_PlannedMaintenanceWOPostponementQuestionFeedback : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            if (!IsPostBack)
            {
                ViewState["WORKORDERID"] = Request.QueryString["WorkOrderId"];
                ViewState["RESCHEDULEDID"] = Request.QueryString["WORescheduledId"];
                ViewState["VESSELID"] = "0";
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"];
                else
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Refresh", "REFRESH", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuFeedback.AccessRights = this.ViewState;
            MenuFeedback.MenuList = toolbarmain.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPostponementFeedback_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void Rebind()
    {
        gvPostponementFeedback.SelectedIndexes.Clear();
        gvPostponementFeedback.EditIndexes.Clear();
        gvPostponementFeedback.DataSource = null;
        gvPostponementFeedback.Rebind();
    }
    protected void BindData()
    {
        try
        {
            DataSet ds;
            if (Request.QueryString["vesselid"] != null)
            {
                ds = PhoenixPlannedMaintenanceWOPostponementFeedback.GetPostponementQuestionList(
                new Guid(ViewState["WORKORDERID"].ToString())
                , new Guid(ViewState["RESCHEDULEDID"].ToString())
                , int.Parse(ViewState["VESSELID"].ToString())
                );
            }else
            {
                ds = PhoenixPlannedMaintenanceWOPostponementFeedback.GetPostponementQuestionList(
               new Guid(ViewState["WORKORDERID"].ToString())
               , new Guid(ViewState["RESCHEDULEDID"].ToString())
               );
            }
            gvPostponementFeedback.DataSource = ds.Tables[0];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPostponementFeedback_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ViewState["QuestionId"] = ((RadLabel)e.Item.FindControl("lblQuestionId")).Text;
            RadLabel lblCommentsEnable = (RadLabel)e.Item.FindControl("lblCommentsEnable");
            RadTextBox txtOptionComments = (RadTextBox)e.Item.FindControl("txtOptionComments");
            RadTextBox txtQuestionComments = (RadTextBox)e.Item.FindControl("txtQuestionComments");

            //Option Comment TextBox Bind
            RadioButtonList rblOptions = (RadioButtonList)e.Item.FindControl("rblOptions");
            DataSet dsRadio = PhoenixPlannedMaintenanceWOPostponementFeedback.GetOptionQuestionsList(General.GetNullableGuid(ViewState["QuestionId"].ToString()));
            rblOptions.DataSource = dsRadio;
            rblOptions.DataTextField = "FLDOPTIONNAME";
            rblOptions.DataValueField = "FLDOPTIONID";
            if (dsRadio.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsRadio.Tables[0].Rows)
                {
                    HtmlTableRow trOptioncomments = (HtmlTableRow)e.Item.FindControl("trOptioncomments");
                    lblCommentsEnable.Text = dr["FLDISCOMMENT"].ToString().Trim();
                    if (lblCommentsEnable.Text != null && lblCommentsEnable.Text == "1")
                    {
                        trOptioncomments.Visible = true;
                        txtOptionComments.Visible = true;
                    }
                }
            }
            rblOptions.DataBind();

            //Quesiton Comment TextBox Bind
            RadLabel lblCommentsyn = (RadLabel)e.Item.FindControl("lblCommentsyn");
            HtmlTableRow trcomments = (HtmlTableRow)e.Item.FindControl("trcomments");
            if (lblCommentsyn != null && lblCommentsyn.Text == "1")
            {
                trcomments.Visible = true;
                txtQuestionComments.Visible = true;
            }

            if(ViewState["WORKORDERID"] != null && ViewState["RESCHEDULEDID"] != null)
            {
                DataSet ds;
                if (Request.QueryString["vesselid"] != null)
                    ds = PhoenixPlannedMaintenanceWOPostponementFeedback.BindQuestionWithOption
                        (
                            new Guid(ViewState["WORKORDERID"].ToString())
                            , new Guid(ViewState["RESCHEDULEDID"].ToString())
                            , int.Parse(ViewState["VESSELID"].ToString())
                        );
                else
                    ds = PhoenixPlannedMaintenanceWOPostponementFeedback.BindQuestionWithOption
                            (
                                new Guid(ViewState["WORKORDERID"].ToString())
                                , new Guid(ViewState["RESCHEDULEDID"].ToString())
                            );

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (ViewState["QuestionId"].ToString() == dr["FLDQUESTIONID"].ToString().Trim())
                        {
                            rblOptions.SelectedValue = dr["FLDOPTIONID"].ToString().Trim();
                            txtOptionComments.Text = dr["FLDOPTIONCOMMENTS"].ToString().Trim();
                            txtQuestionComments.Text = dr["FLDQUESTIONCOMMENTS"].ToString().Trim();
                        }
                    }
                }
            }
        }
    }

    protected void MenuFeedback_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                InsertPostponementFeedback();
            }
            if (CommandName.ToUpper().Equals("REFRESH"))
            {
                if (Request.QueryString["vesselid"] != null)
                    PhoenixPlannedMaintenanceWOPostponementFeedback.RefreshPostponementFeedback(
                                    new Guid(ViewState["WORKORDERID"].ToString())
                                    , new Guid(ViewState["RESCHEDULEDID"].ToString())
                                    , int.Parse(ViewState["VESSELID"].ToString())
                                    );
                else
                    PhoenixPlannedMaintenanceWOPostponementFeedback.RefreshPostponementFeedback(
                                new Guid(ViewState["WORKORDERID"].ToString())
                                , new Guid(ViewState["RESCHEDULEDID"].ToString())
                                );
                Rebind();
                ucStatus.Text = "New Questions refreshed Sucessfully.";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertPostponementFeedback()
    {
        string sXmlData = "<Return>";
        foreach (GridDataItem gv in gvPostponementFeedback.Items)
        {
            RadLabel lblQuestionId = (RadLabel)gv.FindControl("lblQuestionId");
            RadLabel lblOrder = (RadLabel)gv.FindControl("lblOrder");
            RadioButtonList rblOptions = (RadioButtonList)gv.FindControl("rblOptions");
            RadTextBox txtQuestionComments = (RadTextBox)gv.FindControl("txtQuestionComments");
            RadTextBox txtOptionComments = (RadTextBox)gv.FindControl("txtOptionComments");
            RadLabel lblFeedBackId = (RadLabel)gv.FindControl("lblFeedBackId");

            sXmlData += "<Data ORDERNO=\"" + lblOrder.Text.Trim() + "\"" +
                " QUESTIONID=\"" + lblQuestionId.Text.Trim() + "\"" +
                " OPTIONID=\"" + rblOptions.SelectedValue.Trim() + "\"" +
                " QUESTIONCOMMENTS=\"" + txtQuestionComments.Text.Trim() + "\"" +
                " OPTIONCOMMENTS=\"" + txtOptionComments.Text.Trim() + "\"" +
                " FEEDBACKID=\"" + lblFeedBackId.Text.Trim() + "\" />" +
                Environment.NewLine;
        }
        sXmlData += "</Return>" + Environment.NewLine;
        if (Request.QueryString["vesselid"] != null)
            PhoenixPlannedMaintenanceWOPostponementFeedback.InsertPostponementFeedback
            (
            new Guid(ViewState["WORKORDERID"].ToString())
            , new Guid(ViewState["RESCHEDULEDID"].ToString())
            , sXmlData
            , int.Parse(ViewState["VESSELID"].ToString())
            );
        else
            PhoenixPlannedMaintenanceWOPostponementFeedback.InsertPostponementFeedback
            (
            new Guid(ViewState["WORKORDERID"].ToString())
            , new Guid(ViewState["RESCHEDULEDID"].ToString())
            , sXmlData
            );
        PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleRequestConfirm(int.Parse(ViewState["VESSELID"].ToString())
            , new Guid(ViewState["WORKORDERID"].ToString())
            , General.GetNullableGuid(ViewState["RESCHEDULEDID"].ToString()));
        string script = "closeTelerikWindow('feedback','detail');";
        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
    }

    protected void rblOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridDataItem row in gvPostponementFeedback.Items)
        {
            RadioButtonList rblOptions = (RadioButtonList)row.FindControl("rblOptions");
            DataSet OptionChange = PhoenixPlannedMaintenanceWOPostponementFeedback.GetOptionComment(General.GetNullableInteger(rblOptions.SelectedValue));
            RadTextBox txtOptionComments = (RadTextBox)row.FindControl("txtOptionComments");
            if (OptionChange.Tables[0].Rows.Count > 0)
            {
                txtOptionComments.Text = OptionChange.Tables[0].Rows[0]["FLDCOMMENTCAPTION"].ToString();
            }
        }
    }
}