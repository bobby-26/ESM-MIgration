using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
public partial class CrewNewApplicantCandidateEvaluationList : PhoenixBasePage
{
    public string strPostedRankId;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantCandidateEvaluationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEvaluationAdd.aspx?newapp=1')", "Add New Proposal", "<i class=\"fas fa-plus\"></i>", "ADDINTSUMMARY");
            else
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewNewApplicantCandidateEvaluationAdd.aspx')", "Add New Proposal", "<i class=\"fas fa-plus\"></i>", "ADDINTSUMMARY");
            MenuIntSummary.AccessRights = this.ViewState;
            MenuIntSummary.MenuList = toolbar.Show();

            PhoenixToolbar ToolbarPromotionCategory = new PhoenixToolbar();
            ToolbarPromotionCategory.AddFontAwesomeButton("../Crew/CrewNewApplicantCandidateEvaluationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            MenuPromotionCategory.AccessRights = this.ViewState;
            MenuPromotionCategory.MenuList = ToolbarPromotionCategory.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ISRATING"] = "";
                SetEmployeePrimaryDetails();
                gvIntSum.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindPromotionCategoryData();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected int? CheckIfOffshoreEmployee()
    {
        int? offshore = null;
        PhoenixCrewOffshoreEmployee.CheckIfOffshoreEmployee(int.Parse(Filter.CurrentNewApplicantSelection), ref offshore);
        return offshore;
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixNewApplicantInterviewSummary.SearchNewApplicantInterviewSummary(Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
                                                                                     , sortexpression, sortdirection
                                                                                     , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                     , gvIntSum.PageSize
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount);

            if (dt.Rows.Count > 0)
            {
                gvIntSum.DataSource = dt;
                gvIntSum.VirtualItemCount = iRowCount;
            }
            else
            {
                gvIntSum.DataSource = "";
            }

            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
            {
                gvIntSum.Columns[1].Visible = false;
            }
            else
            {
                gvIntSum.Columns[1].Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuIntSummary_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { };
        string[] alCaptions = { };

        if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
        {
            alColumns = new string[] { "FLDRANKNAME", "FLDEXPECTEDJOINDATE", "FLDPDSTATUS", "FLDCREATEDDATE", "FLDPROPOSALSTATUS" };
            alCaptions = new string[] { "Rank", "Exp. Join Date", "PD Status", "Created Date", "Proposal Status" };
        }
        else
        {
            alColumns = new string[] { "FLDVESSELNAME", "FLDRANKNAME", "FLDEXPECTEDJOINDATE", "FLDPDSTATUS", "FLDCREATEDDATE", "FLDPROPOSALSTATUS" };
            alCaptions = new string[] { "Vessel", "Rank", "Exp. Join Date", "PD Status", "Created Date", "Proposal Status" };
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixNewApplicantInterviewSummary.SearchNewApplicantInterviewSummary(Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
                                                                                     , sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      iRowCount,
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Assessment.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Assessment</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    //protected void gvIntSum_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            DataRowView drv = (DataRowView)e.Row.DataItem;
    //            Label lblIntStatus = (Label)e.Row.FindControl("lblInterviewStatus");
    //            ImageButton cme = (ImageButton)e.Row.FindControl("cmdEdit");
    //            if (cme != null) cme.Visible = SessionUtil.CanAccess(this.ViewState, cme.CommandName);
    //            Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
    //            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
    //            if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
    //            Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
    //            if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
    //            att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
    //                + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT + (lblIntStatus.Text != string.Empty ? "&u=n" : string.Empty) + "&ratingyn=" + ViewState["ISRATING"] + "&cmdname=PROPOSALUPLOAD'); return false;");

    //            ////////////////////  PD Attachment
    //            Label lblShowPD = (Label)e.Row.FindControl("lblShowPD");
    //            Label lblCrewPlanId = (Label)e.Row.FindControl("lblCrewPlanId");
    //            Label lblPDDTKey = (Label)e.Row.FindControl("lblPDDTKey");
    //            ImageButton cmdPDAtt = (ImageButton)e.Row.FindControl("cmdPDAtt");
    //            Label lblIsPDAtt = (Label)e.Row.FindControl("lblIsPDAtt");
    //            Label lblVesselid = (Label)e.Row.FindControl("lblVesselid");
    //            string vslid = "";

    //            if (!SessionUtil.CanAccess(this.ViewState, cmdPDAtt.CommandName))
    //            {
    //                cmdPDAtt.Visible = false;
    //            }

    //            if (lblIsPDAtt != null && lblIsPDAtt.Text == string.Empty)
    //                cmdPDAtt.ImageUrl = Session["images"] + "/no-attachment.png";

    //            if (lblVesselid != null)
    //                vslid = lblVesselid.Text;

    //            if (lblCrewPlanId != null && lblCrewPlanId.Text == string.Empty)
    //                cmdPDAtt.Visible = false;
    //            else if (lblCrewPlanId != null && lblCrewPlanId.Text != string.Empty)
    //                cmdPDAtt.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Crew/CrewApprovedPDAttachment.aspx?dtkey=" + lblCrewPlanId.Text + "&cmdname=CPDUPLOAD&vslid=" + vslid.ToString() + "'); return false;");
    //            if (lblShowPD.Text == "0")
    //            {
    //                cmdPDAtt.Visible = false;
    //            }
    //            ImageButton cm = (ImageButton)e.Row.FindControl("cmdComment");
    //            if (cm != null)
    //            {
    //                cm.Visible = SessionUtil.CanAccess(this.ViewState, cm.CommandName);
    //                if (Filter.CurrentMenuCodeSelection == "CRW-REC-NAP")
    //                    cm.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewComment','','CrewPendingApprovalRemarks.aspx?dtkey=" + lblDTKey.Text + "&empid=" + Filter.CurrentNewApplicantSelection + "&access=1" + "'); return false;");
    //                else
    //                    cm.Attributes.Add("onclick", "javascript:Openpopup('CrewComment','','CrewPendingApprovalRemarks.aspx?dtkey=" + lblDTKey.Text + "&empid=" + Filter.CurrentNewApplicantSelection + "&access=1" + "'); return false;");
    //            }

    //            Label l = (Label)e.Row.FindControl("lblInterviewId");

    //            LinkButton lb = (LinkButton)e.Row.FindControl("lnkVessel");
    //            //if (CheckIfOffshoreEmployee() == 1)
    //            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
    //            {
    //                lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + l.Text + "&newapp=1');return false;");
    //                cme.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + l.Text + "&newapp=1');return false;");
    //            }
    //            else
    //            {
    //                lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewNewApplicantCandidateEvaluation.aspx?intvid=" + l.Text + "&status=" + drv["FLDPDSTATUS"].ToString() + "');return false;");
    //                cme.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewNewApplicantCandidateEvaluation.aspx?intvid=" + l.Text + "&status=" + drv["FLDPDSTATUS"].ToString() + "');return false;");
    //            }

    //            Label lbtn = (Label)e.Row.FindControl("lblPDStatus");
    //            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress");
    //            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
    //            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

    //            ImageButton dp = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (dp != null)
    //            {
    //                dp.Visible = SessionUtil.CanAccess(this.ViewState, dp.CommandName);
    //                dp.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to deplan the seafarer ?')");
    //            }
    //            ImageButton dpremark = (ImageButton)e.Row.FindControl("cmdDeplanRemark");
    //            if (dpremark != null)
    //            {
    //                dpremark.Visible = SessionUtil.CanAccess(this.ViewState, dp.CommandName);
    //                if (drv["FLDDEPLANREMARKS"].ToString() != string.Empty)
    //                    dpremark.ImageUrl = Session["images"] + "/te_view.png";
    //                dpremark.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Crew/CrewDePlanRemarks.aspx?planid=" + drv["FLDCREWPLANID"].ToString() + "','medium'); return false;");
    //                if (drv["FLDACTIVEYN"].ToString() != "1")
    //                    dpremark.Visible = false;
    //            }
    //            if (drv["FLDPDSTATUS"].ToString().Equals(string.Empty))
    //            {
    //                dp.Visible = false;
    //            }

    //            if (drv["FLDPROPOSALSTATUS"].ToString().ToUpper().Equals("CLOSED"))
    //            {
    //                dp.Visible = false;
    //            }
    //        }
    //    }
    //}
    private bool IsValidReference(string companyname, string phonenumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (companyname.Trim() == string.Empty)
            ucError.ErrorMessage = "Company Name is required";
        if (companyname.Trim() == string.Empty)
            ucError.ErrorMessage = "Phone Number is required";
        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                if (dt.Rows[0]["FLDISOFFICER"].ToString() == "1")
                {

                    ViewState["ISRATING"] = "0";
                }
                else
                {
                    ViewState["ISRATING"] = "1";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvIntSum.Rebind();
    }
    //protected void gvIntSum_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper() == "DEPLAN")
    //        {
    //            string crewPlanId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewPlanId")).Text;
    //            PhoenixCrewPlanning.DeleteCrewPlan(new Guid(crewPlanId), General.GetNullableInteger(Filter.CurrentNewApplicantSelection).Value);
    //            BindData();
    //            gvIntSum.Rebind();
    //            String scriptpopupclose = String.Format("javascript:parent.CloseWindow('codehelp1');");
    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //Promotion Category
    protected void MenuPromotionCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowPromotionCategoryExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowPromotionCategoryExcel()
    {
        string[] alColumns = { "FLDCATEGORY", "FLDEFFECTDATE", "FLDUSER", "FLDCREATEDDATE" };
        string[] alCaptions = { "Category", "With Effect Date", "User", "Date" };

        DataTable dt = PhoenixNewApplicantInterviewSummary.PromotionCategoryList(int.Parse(Filter.CurrentNewApplicantSelection.ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=PromotionCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Promotion Category</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    private void BindPromotionCategoryData()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantInterviewSummary.PromotionCategoryList(int.Parse(Filter.CurrentNewApplicantSelection.ToString()));

            if (dt.Rows.Count > 0)
            {
                gvPromotionCategory.DataSource = dt;
            }
            else
            {
                gvPromotionCategory.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvPromotionCategory_RowCommand(object sender, GridViewCommandEventArgs e)
    //{

    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //    if (e.CommandName.ToString().ToUpper() == "ADD")
    //    {
    //        try
    //        {
    //            string strCategory = ((DropDownList)_gridView.FooterRow.FindControl("ddlCategory")).Text;
    //            string strEffectDate = ((UserControlDate)_gridView.FooterRow.FindControl("txtEffectDate")).Text;

    //            if (!IsValidate(strCategory, strEffectDate))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }



    //            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
    //            if (dt.Rows.Count > 0)
    //            {
    //                strPostedRankId = dt.Rows[0]["FLDRANKPOSTED"].ToString();
    //                if (strPostedRankId == "")
    //                {
    //                    strPostedRankId = dt.Rows[0]["FLDRANK"].ToString();
    //                }
    //            }

    //            PhoenixNewApplicantInterviewSummary.PromotionCategoryInsert(int.Parse(Filter.CurrentNewApplicantSelection.ToString())
    //                                                                        , int.Parse(strPostedRankId)
    //                                                                        , General.GetNullableDateTime(strEffectDate)
    //                                                                        , int.Parse(strCategory)
    //                                                                        , General.GetNullableString(""));

    //            BindPromotionCategoryData();
    //        }
    //        catch (Exception ex)
    //        {
    //            ucError.ErrorMessage = ex.Message;
    //            ucError.Visible = true;

    //        }
    //    }
    //}

    //protected void gvPromotionCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //}


    //protected void gvPromotionCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    //GridView _gridView = (GridView)sender;
    //    //int nCurrentRow = e.RowIndex;
    //    //try
    //    //{
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    ucError.ErrorMessage = ex.Message;
    //    //    ucError.Visible = true;
    //    //}
    //    //_gridView.EditIndex = -1;
    //    //BindData();
    //    //SetPageNavigator();
    //}

    //protected void gvPromotionCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    private bool IsValidate(string strCategory, string strEffectDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(strCategory) == null)
        {
            ucError.ErrorMessage = "Please Select Category";
        }
        if (General.GetNullableDateTime(strEffectDate) == null)
        {
            ucError.ErrorMessage = "Effect Date can not be blank";
        }

        return (!ucError.IsError);
    }

    protected void gvIntSum_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "DEPLAN")
            {
                string crewPlanId = ((RadLabel)e.Item.FindControl("lblCrewPlanId")).Text;
                PhoenixCrewPlanning.DeleteCrewPlan(new Guid(crewPlanId), General.GetNullableInteger(Filter.CurrentNewApplicantSelection).Value);
                BindData();
                gvIntSum.Rebind();
                String scriptpopupclose = String.Format("javascript:parent.CloseWindow('codehelp1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvIntSum_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvIntSum.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvIntSum_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblIntStatus = (RadLabel)e.Item.FindControl("lblInterviewStatus");
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) cme.Visible = SessionUtil.CanAccess(this.ViewState, cme.CommandName);
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                   + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT + (lblIntStatus.Text != string.Empty ? "&u=n" : string.Empty) + "&ratingyn=" + ViewState["ISRATING"] + "&cmdname=PROPOSALUPLOAD'); return false;");
            }
            ////////////////////  PD Attachment
            RadLabel lblShowPD = (RadLabel)e.Item.FindControl("lblShowPD");
            RadLabel lblCrewPlanId = (RadLabel)e.Item.FindControl("lblCrewPlanId");
            RadLabel lblPDDTKey = (RadLabel)e.Item.FindControl("lblPDDTKey");
            LinkButton cmdPDAtt = (LinkButton)e.Item.FindControl("cmdPDAtt");
            RadLabel lblIsPDAtt = (RadLabel)e.Item.FindControl("lblIsPDAtt");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            string vslid = "";
            if (cmdPDAtt != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdPDAtt.CommandName)) cmdPDAtt.Visible = false;

                if (lblIsPDAtt != null && lblIsPDAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    cmdPDAtt.Controls.Add(html);
                }

                if (lblVesselid != null)
                    vslid = lblVesselid.Text;

                if (lblCrewPlanId != null && lblCrewPlanId.Text == string.Empty)
                    cmdPDAtt.Visible = false;

                else if (lblCrewPlanId != null && lblCrewPlanId.Text != string.Empty)
                    cmdPDAtt.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewApprovedPDAttachment.aspx?dtkey=" + lblCrewPlanId.Text + "&cmdname=CPDUPLOAD&vslid=" + vslid.ToString() + "'); return false;");
                if (lblShowPD.Text == "0")
                {
                    cmdPDAtt.Visible = false;
                }

            }
            LinkButton cm = (LinkButton)e.Item.FindControl("cmdComment");
            if (cm != null)
            {
                cm.Visible = SessionUtil.CanAccess(this.ViewState, cm.CommandName);
                if (Filter.CurrentMenuCodeSelection == "CRW-REC-NAP")
                    cm.Attributes.Add("onclick", "javascript:openNewWindow('CrewComment','','" + Session["sitepath"] + "/Crew/CrewPendingApprovalRemarks.aspx?dtkey=" + lblDTKey.Text + "&empid=" + Filter.CurrentNewApplicantSelection + "&access=1" + "'); return false;");
                else
                    cm.Attributes.Add("onclick", "javascript:openNewWindow('CrewComment','','" + Session["sitepath"] + "/Crew/CrewPendingApprovalRemarks.aspx?dtkey=" + lblDTKey.Text + "&empid=" + Filter.CurrentNewApplicantSelection + "&access=1" + "'); return false;");
            }

            RadLabel l = (RadLabel)e.Item.FindControl("lblInterviewId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkVessel");
            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + l.Text + "&newapp=1');return false;");
                cme.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + l.Text + "&newapp=1');return false;");
            }
            else
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewNewApplicantCandidateEvaluation.aspx?intvid=" + l.Text + "&status=" + drv["FLDPDSTATUS"].ToString() + "');return false;");
                cme.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewNewApplicantCandidateEvaluation.aspx?intvid=" + l.Text + "&status=" + drv["FLDPDSTATUS"].ToString() + "');return false;");
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPDStatus");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            LinkButton dp = (LinkButton)e.Item.FindControl("cmdDelete");
            if (dp != null)
            {
                dp.Visible = SessionUtil.CanAccess(this.ViewState, dp.CommandName);
                dp.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to deplan the seafarer ?')");
            }
            LinkButton dpremark = (LinkButton)e.Item.FindControl("cmdDeplanRemark");
            if (dpremark != null)
            {
                dpremark.Visible = SessionUtil.CanAccess(this.ViewState, dp.CommandName);
                if (drv["FLDDEPLANREMARKS"].ToString() != string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();                    
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                    dpremark.Controls.Add(html);
                }
                if (drv["FLDACTIVEYN"].ToString() != "1")
                    dpremark.Visible = false;
                dpremark.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewDePlanRemarks.aspx?planid=" + drv["FLDCREWPLANID"].ToString() + "','medium'); return false;");

            }
            if (drv["FLDPDSTATUS"].ToString().Equals(string.Empty))
            {
                dp.Visible = false;
            }

            if (drv["FLDPROPOSALSTATUS"].ToString().ToUpper().Equals("CLOSED"))
            {
                dp.Visible = false;
            }
        }
    }

    protected void gvPromotionCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPromotionCategoryData();
    }

    protected void gvPromotionCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            int rowCounter;
            RadLabel lblRowNo = e.Item.FindControl("lblRowNo") as RadLabel;
            rowCounter = gvPromotionCategory.MasterTableView.PageSize * gvPromotionCategory.MasterTableView.CurrentPageIndex;
            if (lblRowNo != null)
                lblRowNo.Text = (e.Item.ItemIndex + 1 + rowCounter).ToString();

            RadLabel lblCategory = (RadLabel)e.Item.FindControl("lblCategory");
            if (lblCategory != null)
            {
                if (lblCategory.Text == "1")
                    lblCategory.Text = "Category1";
                if (lblCategory.Text == "2")
                    lblCategory.Text = "Category2";
                if (lblCategory.Text == "3")
                    lblCategory.Text = "Category3";
                if (lblCategory.Text == "4")
                    lblCategory.Text = "Category4";
            }
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            if (lbtn != null)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipRemarks");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvPromotionCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {        
        if (e.CommandName.ToString().ToUpper() == "ADD")
        {
            try
            {
                string strCategory = ((RadComboBox)e.Item.FindControl("ddlCategory")).SelectedValue;
                string strEffectDate = ((UserControlDate)e.Item.FindControl("txtEffectDate")).Text;

                if (!IsValidate(strCategory, strEffectDate))
                {
                    ucError.Visible = true;
                    return;
                }

                DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
                if (dt.Rows.Count > 0)
                {
                    strPostedRankId = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                    if (strPostedRankId == "")
                    {
                        strPostedRankId = dt.Rows[0]["FLDRANK"].ToString();
                    }
                }

                PhoenixNewApplicantInterviewSummary.PromotionCategoryInsert(int.Parse(Filter.CurrentNewApplicantSelection.ToString())
                                                                            , int.Parse(strPostedRankId)
                                                                            , General.GetNullableDateTime(strEffectDate)
                                                                            , int.Parse(strCategory)
                                                                            , General.GetNullableString(""));

                BindPromotionCategoryData();
                gvPromotionCategory.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }
}
