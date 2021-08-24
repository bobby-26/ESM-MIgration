using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;

public partial class Registers_RegisterInstituteAssessmentSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Finalize", "FINAL", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        

        FeedBackTabs.AccessRights = this.ViewState;
        FeedBackTabs.MenuList = toolbar.Show();

        if(!IsPostBack)
        {
            ViewState["addcode"] = "";
            if (Request.QueryString["addresscode"] != null)
            {
                ViewState["addcode"] = Request.QueryString["addresscode"].ToString();
                PhoenixRegistersAddressAssessmentOptions.AddressAssessmentAnswersInsert(General.GetNullableInteger(ViewState["addcode"].ToString())
                                                                                       , null
                                                                                       , null
                                                                                       , null);

                DataTable dt1 = PhoenixRegistersAddressAssessmentOptions.AssessmentStatusList(General.GetNullableInteger(ViewState["addcode"].ToString()), null);
                if(dt1.Rows.Count>0)
                {
                    txtdate.Text = dt1.Rows[0]["FLDINTERIEWDATE"].ToString();
                    txtinterviewername.Text = dt1.Rows[0]["FLDINTERVIEWERNAME"].ToString();
                    txtremark.Text = dt1.Rows[0]["FLDINTERVIEWERCOMMENT"].ToString();
                    rdbstatus.SelectedValue= dt1.Rows[0]["FLDSTATUS"].ToString();

                }
            }


        }
    }

    private void BindData()
    {
        try
        {
                string mnu = Filter.CurrentMenuCodeSelection;
                DataTable dt = PhoenixAddressAssessmentQuestion.InsterviewAssessmentQuestionList(null,1);

                gvFeedBackQst.DataSource = dt;
           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFeedBackQst_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void FeedBackTabs_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper() == "SAVE")
            {
                PhoenixRegistersAddressAssessmentOptions.AssessmentStatusInsert(General.GetNullableInteger(ViewState["addcode"].ToString())
                                                                                   , General.GetNullableInteger(rdbstatus.SelectedValue)
                                                                                   , General.GetNullableDateTime(txtdate.Text)
                                                                                   , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                   , txtinterviewername.Text
                                                                                   , txtremark.Text
                                                                                   , null);
                foreach (GridDataItem gv in gvFeedBackQst.Items)
                {
                    RadLabel lblQuestionId = (RadLabel)gv.FindControl("lblQuestionId");
                    RadioButtonList rblOptions = (RadioButtonList)gv.FindControl("rblOptions");
                    RadTextBox txtComments = (RadTextBox)gv.FindControl("txtComments");
                    string option = "";
                    if (rblOptions != null)
                    {
                        option = rblOptions.SelectedValue.ToString();
                    }


                    PhoenixRegistersAddressAssessmentOptions.AddressAssessmentAnswersInsert(General.GetNullableInteger(ViewState["addcode"].ToString())
                                                                                            , General.GetNullableInteger(lblQuestionId.Text)
                                                                                            , General.GetNullableInteger(option)
                                                                                            , txtComments.Text);

                }
            }
            if (CommandName.ToUpper() == "FINAL")
            {
                if (!IsValidateStatus())
                {
                    ucError.Visible = true;
                    return;
                }
                foreach (GridDataItem gv in gvFeedBackQst.Items)
                {
                    RadLabel lblQuestionId = (RadLabel)gv.FindControl("lblQuestionId");
                    RadioButtonList rblOptions = (RadioButtonList)gv.FindControl("rblOptions");
                    RadTextBox txtComments = (RadTextBox)gv.FindControl("txtComments");
                    if (txtComments != null && txtComments.Visible == true)
                    {
                        if (!IsValidatequestion(txtComments.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                    }
                    if (rblOptions != null)
                    {
                        if (rblOptions.Items.Count > 0)
                        {
                            if (!IsValidateoption(rblOptions.SelectedValue))
                            {
                                ucError.Visible = true;
                                return;
                            }
                        }
                    }


                }
                PhoenixRegistersAddressAssessmentOptions.AssessmentStatusInsert(General.GetNullableInteger(ViewState["addcode"].ToString())
                                                                                 , General.GetNullableInteger(rdbstatus.SelectedValue)
                                                                                 , General.GetNullableDateTime(txtdate.Text)
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                 , txtinterviewername.Text
                                                                                 , txtremark.Text
                                                                                 , 1);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidatequestion(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (remarks == null || remarks == "")
            ucError.ErrorMessage = "Comment is required";

        return (!ucError.IsError);
    }
    private bool IsValidateoption(string option)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (option == null || option == "")
            ucError.ErrorMessage = "Please select the answers for all qustions";


        return (!ucError.IsError);
    }

    private bool IsValidateStatus()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (rdbstatus.SelectedIndex<0)
            ucError.ErrorMessage = "Please select the status";
        if (txtinterviewername.Text =="")
            ucError.ErrorMessage = "Please give interviewer name";
        if (txtremark.Text == "")
            ucError.ErrorMessage = "Please select the interviewer comments";
        if (txtdate.Text == null)
            ucError.ErrorMessage = "Please select the interview date";


        return (!ucError.IsError);
    }
    protected void gvFeedBackQst_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            try
            {
               
                //HtmlTableRow trcomments = (HtmlTableRow)e.Item.FindControl("trcomments");
           
                //trcomments.Visible = true;
               
                RadLabel lblQuestionId = (RadLabel)e.Item.FindControl("lblQuestionId");
                RadioButtonList rblOptions = (RadioButtonList)e.Item.FindControl("rblOptions");
                RadTextBox txtComments = (RadTextBox)e.Item.FindControl("txtComments");
                RadLabel lblRequirRemark = (RadLabel)e.Item.FindControl("lblRequirRemark");
                RadLabel lblcomment = (RadLabel)e.Item.FindControl("lblcomment");

                if (lblRequirRemark.Text.Trim() == "0")
                {
                    lblcomment.Visible = false;
                    txtComments.Visible = false;
                }

                //DataTable dt = PhoenixAddressAssessmentQuestion.InsterviewAssessmentQuestionList(General.GetNullableInteger(lblQuestionId.Text),1);
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (dr["FLDREQUIREREMARK"].ToString().Trim() == "1")
                //        {
                //            lblcomment.Text = "";
                //            lblcomment.Text = "Comments (If " + dr["FLDOPTIONNAME"].ToString() + ")";
                //            //txtComments.CssClass = "input_mandatory";
                //        }

                //    }
                //}

               DataTable dt1 = PhoenixRegistersAddressAssessmentOptions.AssessmentStatusList(General.GetNullableInteger(ViewState["addcode"].ToString()),null );
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr1 in dt1.Rows)
                    {
                        if (lblQuestionId.Text.Trim() == dr1["FLDQUESTIONID"].ToString().Trim())
                        {
                            rblOptions.SelectedValue = dr1["FLDOPTIONID"].ToString().Trim();
                            txtComments.Text = dr1["FLDREMARKS"].ToString().Trim();
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
    }

}