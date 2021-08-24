using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionPEARSRiskAssessmentDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["RISKASSESSMENTID"] = "";
                ViewState["STATUS"] = "";
                ViewState["CopyType"] = "";
                ViewState["StandardRA"] = "0";
                ViewState["RevYN"] = "0";

                if (Request.QueryString["RAID"] != null && Request.QueryString["RAID"].ToString() != string.Empty)
                    ViewState["RISKASSESSMENTID"] = Request.QueryString["RAID"].ToString().ToUpper();

                if (Request.QueryString["CopyType"] != null && Request.QueryString["CopyType"].ToString() != string.Empty)
                    ViewState["CopyType"] = Request.QueryString["CopyType"].ToString();

                if (Request.QueryString["RevYN"] != null && Request.QueryString["RevYN"].ToString() != string.Empty)
                    ViewState["RevYN"] = Request.QueryString["RevYN"].ToString();

                ViewState["StandardRA"] = string.IsNullOrEmpty(Request.QueryString["StandardRA"]) ? "" : Request.QueryString["StandardRA"];

                ViewState["COMPANYID"] = "";

                NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvcCompany.Get("QMS");
                }

                ucCompany.SelectedCompany = ViewState["COMPANYID"].ToString();
                txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;

                BindGroupAndPPE();
                BindPEARSRA();
                BindMenu();
                gvPEARSRA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }


        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    public void BindMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        PhoenixToolbar toolbaradd = new PhoenixToolbar();

        if (ViewState["CopyType"].ToString().Equals("1"))
        {
            toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
        }
        else
        {
            if (ViewState["RevYN"].ToString() != "1")
            {

                if (ViewState["STATUS"].ToString().Equals("1"))
                    toolbar.AddLinkButton("javascript:openNewWindow('Request','','Inspection/InspectionPEARSRARequestApproval.aspx?RAID=" + ViewState["RISKASSESSMENTID"].ToString() + "')", "Request for Approval", "REQUEST", ToolBarDirection.Right);

                if (ViewState["STATUS"].ToString().Equals("2"))
                    toolbar.AddLinkButton("javascript:openNewWindow('Approve','','Inspection/InspectionPEARSRAApprove.aspx?RAID=" + ViewState["RISKASSESSMENTID"].ToString() + "')", "Approve", "APPROVEREJECT", ToolBarDirection.Right);

                if (ViewState["STATUS"].ToString().Equals("3"))
                    toolbar.AddLinkButton("javascript:openNewWindow('Issue','','Inspection/InspectionPEARSRAIssue.aspx?RAID=" + ViewState["RISKASSESSMENTID"].ToString() + "')", "Issue", "ISSUE", ToolBarDirection.Right);

                if (ViewState["STATUS"].ToString().Equals("5"))
                    toolbar.AddLinkButton("javascript:openNewWindow('Close','','Inspection/InspectionPEARSRAClose.aspx?RAID=" + ViewState["RISKASSESSMENTID"].ToString() + "')", "Close", "CLOSE", ToolBarDirection.Right);

                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("List", "LIST", ToolBarDirection.Right);

                toolbaradd.AddFontAwesomeButton("javascript:openNewWindow('ActivityStep','Add Activity Steps','" + Session["sitepath"] + "/Inspection/InspectionPEARSRiskAssessmentActivityStepsAdd.aspx?RAID=" + ViewState["RISKASSESSMENTID"].ToString() + "')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            }
        }
        MenuGeneral.AccessRights = this.ViewState;
        MenuGeneral.MenuList = toolbar.Show();

        MenuPEARSRA.AccessRights = this.ViewState;
        MenuPEARSRA.MenuList = toolbaradd.Show();
    }
    protected void BindGroupAndPPE()
    {
        ChkgroupMem.DataSource = PhoenixRegistersGroupRank.ListJHAGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ChkgroupMem.DataTextField = "FLDGROUPRANK";
        ChkgroupMem.DataValueField = "FLDGROUPRANKID";
        ChkgroupMem.DataBind();

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentMiscellaneousExtn.RiskAssessmentMiscellaneousSearch(null,
            5,
            null,
            null,
            1,
            500,
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            cbRAPPE.DataSource = ds.Tables[0];
            cbRAPPE.DataBind();
        }

    }
    protected void BindPEARSRA()
    {
        if (ViewState["RISKASSESSMENTID"] != null && ViewState["RISKASSESSMENTID"].ToString() != string.Empty)
        {
            DataSet dss = PhoenixInspectionPEARSRiskAssessment.EditRiskAssessment(new Guid(ViewState["RISKASSESSMENTID"].ToString()));

            ViewState["RISKASSESSMENTID"] = dss.Tables[0].Rows[0]["FLDPEARSRISKASSESSMENTID"].ToString();
            txtVessel.Text = dss.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            txtStatus.Text = dss.Tables[0].Rows[0]["FLDSTATUS"].ToString();
            ViewState["STATUS"] = dss.Tables[0].Rows[0]["FLDSTATUSID"].ToString();
            ucIntendedWorkDate.Text = dss.Tables[0].Rows[0]["FLDINTENDEDWORKDATE"].ToString();
            txtRefNo.Text = dss.Tables[0].Rows[0]["FLDREFERENCENUMBER"].ToString();
            txtCreatedbyName.Text = dss.Tables[0].Rows[0]["FLDCREATEDBYNAME"].ToString();
            txtCreatedbyRank.Text = dss.Tables[0].Rows[0]["FLDCREATEDBYRANK"].ToString();
            txtRewiewedbyName.Text = dss.Tables[0].Rows[0]["FLDREVIEWEDBYNAME"].ToString();
            txtReviewedbyRank.Text = dss.Tables[0].Rows[0]["FLDREVIEWEDBYRANK"].ToString();
            txtIssuedbyName.Text = dss.Tables[0].Rows[0]["FLDISSUEDBYNAME"].ToString();
            txtApprovedbyName.Text = dss.Tables[0].Rows[0]["FLDAPPROVEDBYNAME"].ToString();
            txtApprovedbyRank.Text = dss.Tables[0].Rows[0]["FLDAPPROVEDBYRANK"].ToString();
            txtActivity.Text = dss.Tables[0].Rows[0]["FLDTYPEOFACTIVITY"].ToString();
            txtActivitySite.Text = dss.Tables[0].Rows[0]["FLDACTIVITYWORKSITE"].ToString();
            ucIssuedDate.Text = dss.Tables[0].Rows[0]["FLDISSUEDDATE"].ToString();
            General.BindCheckBoxList(cbRAPPE, dss.Tables[0].Rows[0]["FLDPPELIST"].ToString());
            General.BindCheckBoxList(ChkgroupMem, dss.Tables[0].Rows[0]["FLDGROUPMEMBERS"].ToString());

        }
    }
    private void BindData()
    {
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDVESSELNAME", "FLDCREATEDDATE", "FLDINTENDEDWORKDATE", "FLDTYPEOFACTIVITY", "FLDACTIVITYWORKSITE", "FLDREVISIONNUMBER", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Activity", "Activity Work Site", "Revision No", "Status" };

        NameValueCollection nvc = InspectionFilter.CurrentNonRoutineRAFilter;

        DataSet ds = PhoenixInspectionPEARSRiskAssessment.ListRAActivitySteps(General.GetNullableGuid(ViewState["RISKASSESSMENTID"].ToString()));

        General.SetPrintOptions("gvPEARSRA", "PEARS RA", alCaptions, alColumns, ds);
        gvPEARSRA.DataSource = ds;

        if(ViewState["RevYN"].ToString() == "1")
        {
            gvPEARSRA.Columns[14].Visible = false;
        }
    }
    protected void gvPEARSRA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPEARSRA.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvPEARSRA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel lblRAid = (RadLabel)e.Item.FindControl("lblRAID");
            }
            if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                RadLabel lblRAActivityid = (RadLabel)e.Item.FindControl("lblActivityStepid");

                PhoenixInspectionPEARSRiskAssessment.DeleteRAActivityStep(new Guid(lblRAActivityid.Text));
                gvPEARSRA.Rebind();
                ucStatus.Text = "RA Activity Step Deleted Successfully";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPEARSRA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblRAActivityid = (RadLabel)e.Item.FindControl("lblActivityStepid");
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton imgcancel = (LinkButton)e.Item.FindControl("imgcancel");

            if (cmdEdit != null)
            {
                cmdEdit.Attributes.Add("Onclick", "openNewWindow('ActivityStep','Edit Activity Step', '" + Session["sitepath"] + "/Inspection/InspectionPEARSRiskAssessmentActivityStepsAdd.aspx?RAACTIVITYID=" + lblRAActivityid.Text + "&RAID=" + ViewState["RISKASSESSMENTID"].ToString() + "'); return true;");
            }

            RadLabel lblinitPpl = (RadLabel)e.Item.FindControl("lblinitPpl");
            RadLabel lblinitEnv = (RadLabel)e.Item.FindControl("lblinitEnv");
            RadLabel lblinitAsset = (RadLabel)e.Item.FindControl("lblinitAsset");
            RadLabel lblinitRep = (RadLabel)e.Item.FindControl("lblinitRep");
            RadLabel lblinitSch = (RadLabel)e.Item.FindControl("lblinitSch");

            RadLabel lblResPpl = (RadLabel)e.Item.FindControl("lblResPpl");
            RadLabel lblResEnv = (RadLabel)e.Item.FindControl("lblResEnv");
            RadLabel lblResAsset = (RadLabel)e.Item.FindControl("lblResAsset");
            RadLabel lblResRep = (RadLabel)e.Item.FindControl("lblResRep");
            RadLabel lblResSch = (RadLabel)e.Item.FindControl("lblResSch");

            DataRowView dv = (DataRowView)e.Item.DataItem;

            int minscore = 0, maxscore = 0;

            if (!string.IsNullOrEmpty(dv["FLDMINSCORE"].ToString()))
                minscore = int.Parse(dv["FLDMINSCORE"].ToString());

            if (!string.IsNullOrEmpty(dv["FLDMAXSCORE"].ToString()))
                maxscore = int.Parse(dv["FLDMAXSCORE"].ToString());

            if (lblinitPpl != null)
            {
                if (!string.IsNullOrEmpty(lblinitPpl.Text))
                {
                    if (int.Parse(lblinitPpl.Text) <= minscore)
                        lblinitPpl.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblinitPpl.Text) > minscore && int.Parse(lblinitPpl.Text) <= maxscore)
                        lblinitPpl.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblinitPpl.Text) > maxscore)
                        lblinitPpl.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblinitPpl.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }

            if (lblinitEnv != null)
            {
                if (!string.IsNullOrEmpty(lblinitEnv.Text))
                {
                    if (int.Parse(lblinitEnv.Text) <= minscore)
                        lblinitEnv.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblinitEnv.Text) > minscore && int.Parse(lblinitEnv.Text) <= maxscore)
                        lblinitEnv.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblinitEnv.Text) > maxscore)
                        lblinitEnv.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblinitEnv.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }

            if (lblinitAsset != null)
            {
                if (!string.IsNullOrEmpty(lblinitAsset.Text))
                {
                    if (int.Parse(lblinitAsset.Text) <= minscore)
                        lblinitAsset.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblinitAsset.Text) > minscore && int.Parse(lblinitAsset.Text) <= maxscore)
                        lblinitAsset.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblinitAsset.Text) > maxscore)
                        lblinitAsset.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblinitAsset.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }

            if (lblinitRep != null)
            {
                if (!string.IsNullOrEmpty(lblinitRep.Text))
                {
                    if (int.Parse(lblinitRep.Text) <= minscore)
                        lblinitRep.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblinitRep.Text) > minscore && int.Parse(lblinitRep.Text) <= maxscore)
                        lblinitRep.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblinitRep.Text) > maxscore)
                        lblinitRep.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblinitRep.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }

            if (lblinitSch != null)
            {
                if (!string.IsNullOrEmpty(lblinitSch.Text))
                {
                    if (int.Parse(lblinitSch.Text) <= minscore)
                        lblinitSch.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblinitSch.Text) > minscore && int.Parse(lblinitSch.Text) <= maxscore)
                        lblinitSch.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblinitSch.Text) > maxscore)
                        lblinitSch.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblinitSch.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }

            if (lblResPpl != null)
            {
                if (!string.IsNullOrEmpty(lblResPpl.Text))
                {
                    if (int.Parse(lblinitPpl.Text) <= minscore)
                        lblResPpl.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblResPpl.Text) > minscore && int.Parse(lblResPpl.Text) <= maxscore)
                        lblResPpl.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblResPpl.Text) > maxscore)
                        lblResPpl.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblResPpl.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }

            if (lblResEnv != null)
            {
                if (!string.IsNullOrEmpty(lblResEnv.Text))
                {
                    if (int.Parse(lblResEnv.Text) <= minscore)
                        lblResEnv.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblResEnv.Text) > minscore && int.Parse(lblResEnv.Text) <= maxscore)
                        lblResEnv.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblResEnv.Text) > maxscore)
                        lblResEnv.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblResEnv.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }

            if (lblResAsset != null)
            {
                if (!string.IsNullOrEmpty(lblResAsset.Text))
                {
                    if (int.Parse(lblResAsset.Text) <= minscore)
                        lblResAsset.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblResAsset.Text) > minscore && int.Parse(lblResAsset.Text) <= maxscore)
                        lblResAsset.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblResAsset.Text) > maxscore)
                        lblResAsset.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblResAsset.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }

            if (lblResRep != null)
            {
                if (!string.IsNullOrEmpty(lblResRep.Text))
                {
                    if (int.Parse(lblResRep.Text) <= minscore)
                        lblResRep.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblResRep.Text) > minscore && int.Parse(lblResRep.Text) <= maxscore)
                        lblResRep.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblResRep.Text) > maxscore)
                        lblResRep.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblResRep.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }

            if (lblResSch != null)
            {
                if (!string.IsNullOrEmpty(lblResSch.Text))
                {
                    if (int.Parse(lblResSch.Text) <= minscore)
                        lblResSch.Attributes.Add("style", "background-color:Lime;font-weight:bold;");
                    else if (int.Parse(lblResSch.Text) > minscore && int.Parse(lblResSch.Text) <= maxscore)
                        lblResSch.Attributes.Add("style", "background-color:Yellow;font-weight:bold;");
                    else if (int.Parse(lblResSch.Text) > maxscore)
                        lblResSch.Attributes.Add("style", "background-color:Red;font-weight:bold;");
                }
                else
                    lblResSch.Attributes.Add("style", "background-color:White;font-weight:bold;");
            }
        }
    }

    protected void MenuPEARSRA_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                if (ViewState["StandardRA"].ToString() == "1")
                {
                    Response.Redirect("../Inspection/InspectionPEARSRAStandard.aspx", false);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionPEARSRiskAssessmentList.aspx", false);
                }

            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                InserUpdatetRA();
                ViewState["PAGENUMBER"] = 1;
                gvPEARSRA.Rebind();
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                CopyRiskAssessment();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void InserUpdatetRA()
    {
        try
        {
            if (!IsValidRA())
            {
                ucError.Visible = true;
                return;
            }

            Guid? RAID = General.GetNullableGuid(ViewState["RISKASSESSMENTID"].ToString());

            PhoenixInspectionPEARSRiskAssessment.InsertRiskAssessment(ref RAID
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                , General.GetNullableDateTime(ucIntendedWorkDate.Text)
                , General.GetNullableString(txtActivity.Text)
                , General.GetNullableString(txtActivitySite.Text)
                , General.ReadCheckBoxList(cbRAPPE));

            ViewState["RISKASSESSMENTID"] = RAID.ToString();
            ucStatus.Text = "RA updated Successfully.";
            BindPEARSRA();
            BindMenu();

        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void CopyRiskAssessment()
    {
        try
        {
            PhoenixInspectionPEARSRiskAssessment.CopyRiskAssessment(new Guid(ViewState["RISKASSESSMENTID"].ToString())
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

            ucStatus.Text = "RA Copied Successfully.";

            String script = String.Format("javascript:fnReloadList('Copy','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindPEARSRA();
        BindMenu();
        gvPEARSRA.Rebind();

    }
    private bool IsValidRA()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(ucIntendedWorkDate.Text) == null)
            ucError.ErrorMessage = "Intended Work Date is required.";

        if (General.GetNullableString(txtActivity.Text) == null)
            ucError.ErrorMessage = "Type of Activity is required.";

        if (General.GetNullableString(txtActivitySite.Text) == null)
            ucError.ErrorMessage = "Activity Work Site is required.";

        if (General.GetNullableString(General.ReadCheckBoxList(cbRAPPE)) == null)
            ucError.ErrorMessage = "PPE is required.";       

        return (!ucError.IsError);

    }
}