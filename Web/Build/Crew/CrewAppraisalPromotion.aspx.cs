using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewAppraisalPromotion : PhoenixBasePage
{
    string canedit = "1";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 0;

            //Filter.CurrentAppraisalSelection = Request.QueryString["appraisalid"].ToString();
            ViewState["RankId"] = Request.QueryString["rankid"].ToString();

            DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(ViewState["RankId"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                canedit = ds.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
            }
            FillYesNoValues();
            
        }
        //BindData();
       
    }

    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewAppraisalPromotion.AppraisalPromotionSearch( General.GetNullableGuid(Filter.CurrentAppraisalSelection), int.Parse(ViewState["RankId"].ToString()));

            gvCrewPromotionAppraisal.DataSource = ds;

            //gvCrewPromotionAppraisal.DataBind();
            
            //  gvCrewPromotionAppraisal.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void FillYesNoValues()
    {
        if (Filter.CurrentAppraisalSelection != null)
        {
            DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                string YesNovalues = ds.Tables[0].Rows[0]["FLDPROMOTIONYESNO"].ToString();
                if (!String.IsNullOrEmpty(YesNovalues))
                {
                    string[] s = YesNovalues.Split(',');
                    chkTanker.Checked = s[0].ToString().Equals("1");
                    chkRecommendPromotion.Checked = s[1].ToString().Equals("1");
                }
            }
        }
    }

    protected void gvCrewPromotionAppraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!canedit.Equals("1"))
                    db.Visible = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && db.Visible)
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlPromotionEvaluationItem ddlEvaluationEdit = (UserControlPromotionEvaluationItem)e.Item.FindControl("ddlEvaluationEdit");

            if (ddlEvaluationEdit != null)
                ddlEvaluationEdit.SelectedEvaluation = drv["FLDPROMOTIONQUESTIONID"].ToString();

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!canedit.Equals("1"))
                    eb.Visible = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
        }
       
    }
    protected void gvCrewPromotionAppraisal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblAppraisalPromotionId")).Text) != null)
                {
                    PhoenixCrewAppraisalPromotion.DeleteAppraisalPromotion(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                           , new Guid(((RadLabel)e.Item.FindControl("lblAppraisalPromotionId")).Text)
                           );
                }
                BindData();
                gvCrewPromotionAppraisal.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("EDIT"))
                ((RadLabel)e.Item.FindControl("lblRange")).Focus();


            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string a = ((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text;
                if (!IsValidaAppraisalPromotion(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (!String.IsNullOrEmpty(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text.TrimStart('_')))
                    {
                        if (General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblAppraisalPromotionId")).Text) == null)
                        {
                            InsertAppraisalPromotion(
                                int.Parse(((RadLabel)e.Item.FindControl("lblPromotionQuestionId")).Text),
                                int.Parse(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text.TrimStart('_')));
                        }
                        else
                        {
                            PhoenixCrewAppraisalPromotion.UpdateAppraisalPromotion(
                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(((RadLabel)e.Item.FindControl("lblAppraisalPromotionId")).Text)
                                                    , int.Parse(((RadLabel)e.Item.FindControl("lblPromotionQuestionId")).Text)
                                                    , int.Parse(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text.TrimStart('_'))
                                                    );
                        }
                    }
                }
            
                BindData();
                gvCrewPromotionAppraisal.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }       
    
    //private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    //{
    //    int nextRow = 0;
    //    GridView _gridView = (GridView)sender;

    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
    //    {
    //        int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

    //        String script = "var keyValue = SelectSibling(event); ";
    //        script += " if(keyValue == 38) {";  //Up Arrow
    //        nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";

    //        script += " if(keyValue == 40) {";  //Down Arrow
    //        nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";
    //        script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
    //        e.Row.Attributes["onkeydown"] = script;
    //    }    }
   
    private bool IsValidaAppraisalPromotion(string Rating)
    {
        string a = Rating;
        //ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(Rating))
            ucError.ErrorMessage = "Rating is required";
        else if (General.GetNullableInteger(Rating) > 10 || General.GetNullableInteger(Rating) < 0)
            ucError.ErrorMessage = "Rating should be between 0 to 10.";

        return (!ucError.IsError);
    }

    private void InsertAppraisalPromotion(int evaluationitem, int Rating)
    {
        if (Filter.CurrentAppraisalSelection == null)
        {
            ucError.HeaderMessage = "Please provide the Primary Details information";
            ucError.ErrorMessage = "<br/>Primary Details has to be filled and saved before enter rating's";
            ucError.Visible = true;
            return;
        }

        PhoenixCrewAppraisalPromotion.InsertAppraisalPromotion(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(Filter.CurrentAppraisalSelection)
            , evaluationitem
            , Rating
        );
    }

  
    protected void chkbox_CheckedChanged(object sender, EventArgs e)
    {
        string s = string.Empty;
        s = (chkTanker.Checked==true) ? "1" : "0";
        s += (chkRecommendPromotion.Checked==true) ? ",1" : ",0";

        PhoenixCrewAppraisalPromotion.UpdateAppraisalPromotionYesNoQuestions(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , new Guid(Filter.CurrentAppraisalSelection)
                                                                            , s );
        FillYesNoValues();
    }
    
        protected void gvCrewPromotionAppraisal_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewPromotionAppraisal.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}