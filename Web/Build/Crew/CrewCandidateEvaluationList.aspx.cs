using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewCandidateEvaluationList : PhoenixBasePage
{
    public string strPostedRankId;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            CrewTab.AccessRights = this.ViewState;
            CrewTab.MenuList = toolbar2.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewCandidateEvaluationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
                toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEvaluationAdd.aspx')", "Add New Proposal", "<i class=\"fa fa-plus-circle\"></i>", "ADDINTSUMMARY");
            else
                toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewCandidateEvaluationAdd.aspx')", "Add New Proposal", "<i class=\"fa fa-plus-circle\"></i>", "ADDINTSUMMARY");
            MenuIntSummary.AccessRights = this.ViewState;
            MenuIntSummary.MenuList = toolbar.Show();

            PhoenixToolbar ToolbarPromotionCategory = new PhoenixToolbar();
            ToolbarPromotionCategory.AddFontAwesomeButton("../Crew/CrewCandidateEvaluationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            MenuPromotionCategory.AccessRights = this.ViewState;
            MenuPromotionCategory.MenuList = ToolbarPromotionCategory.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ISRATING"] = "";
                ViewState["OFFERLETTERYN"] = "0";
                string OFFERLETTER = PhoenixCommonRegisters.GetHardCode(1, 266, "OFO");
                if (OFFERLETTER != null && OFFERLETTER != "")
                    ViewState["OFFERLETTERYN"] = "1";
                SetEmployeePrimaryDetails();

                gvIntSum.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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
        PhoenixCrewOffshoreEmployee.CheckIfOffshoreEmployee(int.Parse(Filter.CurrentCrewSelection), ref offshore);
        return offshore;
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

        DataTable dt = PhoenixNewApplicantInterviewSummary.SearchNewApplicantInterviewSummary(Int32.Parse(Filter.CurrentCrewSelection.ToString())
                                                                                     , sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      iRowCount,
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);

        General.ShowExcel("Assessment", dt, alColumns, alCaptions, null, null);

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

            DataTable dt = PhoenixNewApplicantInterviewSummary.SearchNewApplicantInterviewSummary(Int32.Parse(Filter.CurrentCrewSelection.ToString())
                                                                                     , sortexpression, sortdirection
                                                                                     , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                     , gvIntSum.PageSize
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount);

            gvIntSum.DataSource = dt;
            gvIntSum.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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


    protected void gvIntSum_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvIntSum.CurrentPageIndex + 1;

        BindData();
    }


    protected void gvIntSum_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "DEPLAN")
            {
                string crewPlanId = ((RadLabel)e.Item.FindControl("lblCrewPlanId")).Text;
                PhoenixCrewPlanning.DeleteCrewPlan(new Guid(crewPlanId), General.GetNullableInteger(Filter.CurrentCrewSelection).Value);
                BindData();
                gvIntSum.Rebind();

                String scriptpopupclose = String.Format("javascript:parent.CloseWindow('codehelp1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
            }

            if (e.CommandName.ToUpper() == "APPROVALREMARKS")
            {
                string crewPlanId = ((RadLabel)e.Item.FindControl("lblCrewPlanId")).Text;
                PhoenixCrewPlanning.CrewPlanDetailUpdate(new Guid(crewPlanId));

                BindData();
                gvIntSum.Rebind();
            }
            if (e.CommandName.ToUpper() == "OFFERLETTER")
            {
                string crewPlanId = ((RadLabel)e.Item.FindControl("lblCrewPlanId")).Text;

                string Id = "";
                PhoenixCrewOfferLetter.CrewOfferletterInsert(int.Parse(Filter.CurrentCrewSelection), int.Parse(ViewState["RANKID"].ToString()), General.GetNullableGuid(crewPlanId), null, null, null, null, null, null
            , null, null, null, null, null, null, null, null, null, null, null, null, null, null, ref Id);
                Response.Redirect("../Crew/CrewOfferLetter.aspx?Type=p&empid=" + Filter.CurrentCrewSelection + "&id=" + Id);
                BindData();
                gvIntSum.Rebind();
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
    protected void gvIntSum_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblIntStatus = (RadLabel)e.Item.FindControl("lblInterviewStatus");
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT + "&ratingyn=" + ViewState["ISRATING"] + "&cmdname=PROPOSALUPLOAD'); return false;");

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
                    cmdPDAtt.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewApprovedPDAttachment.aspx?dtkey=" + lblCrewPlanId.Text + "&cmdname=PDUPLOAD&vslid=" + vslid.ToString() + "'); return false;");
                if (lblShowPD.Text == "0")
                {
                    cmdPDAtt.Visible = false;
                }

            }

            LinkButton cm = (LinkButton)e.Item.FindControl("cmdComment");
            if (cm != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cm.CommandName)) cm.Visible = false;
                if (Filter.CurrentMenuCodeSelection == "CRW-PER-PER")
                    cm.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewComment','','" + Session["sitepath"] + "/Crew/CrewPendingApprovalRemarks.aspx?dtkey=" + lblDTKey.Text + "&empid=" + Filter.CurrentCrewSelection + "&access=1" + "'); return false;");
                else
                    cm.Attributes.Add("onclick", "javascript:openNewWindow('CrewComment','','" + Session["sitepath"] + "/Crew/CrewPendingApprovalRemarks.aspx?dtkey=" + lblDTKey.Text + "&empid=" + Filter.CurrentCrewSelection + "&access=1" + "'); return false;");

            }

            RadLabel l = (RadLabel)e.Item.FindControl("lblInterviewId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkVessel");
            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
            {
                lb.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + l.Text + "');return false;");
                cme.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + l.Text + "');return false;");
            }
            else
            {
                lb.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewCandidateEvaluation.aspx?intvid=" + l.Text + "&status=" + drv["FLDPDSTATUS"].ToString() + "&proposalstatus=" + drv["FLDPROPOSALSTATUS"].ToString() + "');return false;");
                cme.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewCandidateEvaluation.aspx?intvid=" + l.Text + "&status=" + drv["FLDPDSTATUS"].ToString() + "&proposalstatus=" + drv["FLDPROPOSALSTATUS"].ToString() + "');return false;");
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPDStatus");
            if (lbtn != null)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to deplan the seafarer ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton dpremark = (LinkButton)e.Item.FindControl("cmdDeplanRemark");
            if (dpremark != null)
            {
                dpremark.Visible = SessionUtil.CanAccess(this.ViewState, dpremark.CommandName);

                if (drv["FLDDEPLANREMARKS"].ToString() == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                    dpremark.Controls.Add(html);
                }

                dpremark.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewDePlanRemarks.aspx?planid=" + drv["FLDCREWPLANID"].ToString() + "','medium'); return false;");
                if (drv["FLDACTIVEYN"].ToString() != "1")
                    dpremark.Visible = false;
            }

            if (drv["FLDPDSTATUS"].ToString().Equals(string.Empty))
            {
                db1.Visible = false;
            }

            LinkButton dbofferletter = (LinkButton)e.Item.FindControl("cdmOfferletter");
            if (dbofferletter != null)
            {

                if (ViewState["OFFERLETTERYN"].ToString() == "1")
                {
                    dbofferletter.Visible = SessionUtil.CanAccess(this.ViewState, dbofferletter.CommandName);
                    if (drv["FLDOFFERLETTERYN"].ToString().Equals("0"))
                        dbofferletter.Visible = false;
                }
                else dbofferletter.Visible = false;
            }
            if (drv["FLDPROPOSALSTATUS"].ToString().ToUpper().Equals("CLOSED"))
            {
                db1.Visible = false;
            }

            LinkButton ApproavalRemarks = (LinkButton)e.Item.FindControl("cmdApproavalRemarks");
            if (ApproavalRemarks != null)
            {
                ApproavalRemarks.Visible = SessionUtil.CanAccess(this.ViewState, ApproavalRemarks.CommandName);

                if (drv["FLDREMARKSYN"].ToString() != "1")
                {
                    ApproavalRemarks.Visible = false;
                }
            }

        }


    }

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
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
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


    //Promotion Category
    protected void MenuPromotionCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDCATEGORY", "FLDEFFECTDATE", "FLDUSER", "FLDCREATEDDATE" };
                string[] alCaptions = { "Category", "With Effect Date", "User", "Date" };

                DataTable dt = PhoenixNewApplicantInterviewSummary.PromotionCategoryList(int.Parse(Filter.CurrentCrewSelection.ToString()));

                General.ShowExcel("Promotion Category", dt, alColumns, alCaptions, null, null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPromotionCategoryData()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantInterviewSummary.PromotionCategoryList(int.Parse(Filter.CurrentCrewSelection.ToString()));

            gvPromotionCategory.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPromotionCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPromotionCategoryData();
    }

    protected void gvPromotionCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "ADD")
        {
            try
            {
                string strCategory = ((RadComboBox)e.Item.FindControl("ddlCategory")).SelectedValue;
                string strEffectDate = ((UserControlDate)e.Item.FindControl("txtEffectDate")).Text;
                string remarks = ((RadTextBox)e.Item.FindControl("txtAddRemarks")).Text.Trim();
                if (!IsValidate(strCategory, strEffectDate, remarks))
                {
                    ucError.Visible = true;
                    return;
                }

                DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
                if (dt.Rows.Count > 0)
                {
                    strPostedRankId = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                    if (strPostedRankId == "")
                    {
                        strPostedRankId = dt.Rows[0]["FLDRANK"].ToString();
                    }
                }

                PhoenixNewApplicantInterviewSummary.PromotionCategoryInsert(int.Parse(Filter.CurrentCrewSelection.ToString())
                                                                            , int.Parse(strPostedRankId)
                                                                            , General.GetNullableDateTime(strEffectDate)
                                                                            , int.Parse(strCategory)
                                                                            , General.GetNullableString(remarks));
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

    protected void gvPromotionCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblCategory = (RadLabel)e.Item.FindControl("lblCategory");
            if (lblCategory.Text == "1")
                lblCategory.Text = "Category1";
            if (lblCategory.Text == "2")
                lblCategory.Text = "Category2";
            if (lblCategory.Text == "3")
                lblCategory.Text = "Category3";
            if (lblCategory.Text == "3")
                lblCategory.Text = "Category4";

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


    private bool IsValidate(string strCategory, string strEffectDate, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(strCategory) == null)
        {
            ucError.ErrorMessage = "Category is required";
        }
        if (General.GetNullableDateTime(strEffectDate) == null)
        {
            ucError.ErrorMessage = "With Effect From is Required";
        }
        if (General.GetNullableString(remarks) == null)
        {
            ucError.ErrorMessage = "Remarks is required";
        }

        return (!ucError.IsError);
    }



}
