using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionNonRoutineRiskAssessmentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRiskAssessmentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRiskAssessmentList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRiskAssessmentList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuGeneric.AccessRights = this.ViewState;
            MenuGeneric.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucConfirmApprove.Attributes.Add("style", "display:none");
                ucConfirmIssue.Attributes.Add("style", "display:none");
                ucConfirmRevision.Attributes.Add("style", "display:none");

                ViewState["REFID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TYPE"] = "0";
                ViewState["COMPANYID"] = "";
                ViewState["Status"] = string.Empty;
                BindType();
                BindStatus();

                ddlRAType.SelectedValue = "ALL";

                if(Filter.CurrentStandardTemplateRAFilter != null)
                {
                    NameValueCollection nvcFilter = Filter.CurrentStandardTemplateRAFilter;
                    
                    ViewState["Status"] = nvcFilter["ddlStatus"];
                    ddlStatus.SelectedValue = ViewState["Status"].ToString();
                }

                NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvcCompany.Get("QMS");
                    ucVessel.Company = nvcCompany.Get("QMS");
                    ucVessel.bind();
                }
                if (Request.QueryString["filter"] != null)
                {
                    if (Request.QueryString["filter"].ToString() == "0")
                    {
                        Filter.CurrentStandardTemplateRAFilter = null;
                    }
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                BindCategory();
                gvRiskAssessment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindType()
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivity.ListNonRoutineRiskAssessmentType();
        ddlRAType.DataSource = dt;
        ddlRAType.DataTextField = "FLDNAME";
        ddlRAType.DataValueField = "FLDCODE";
        ddlRAType.DataBind();
        ddlRAType.Items.Insert(0, new RadComboBoxItem("--Select--", "ALL"));
    }

    private void BindStatus()
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivity.ListNonRoutineRiskAssessmentStatus();
        ddlStatus.DataSource = dt;
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(5, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        ddlCategory.Items.Clear();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataBind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string vesselid = "";
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Type", "Activity", "Revision No", "Status", "Approved By" };

            NameValueCollection nvc = Filter.CurrentStandardTemplateRAFilter;

            if (Filter.CurrentStandardTemplateRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            if (nvc != null && General.GetNullableInteger(nvc["ucVessel"]) == null)
            {
                nvc["ucVessel"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            DataSet ds = PhoenixInspectionRiskAssessmentGeneric.PhoenixInspectionNonRoutineRiskAssessmentSearch(
                         nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                        , gvRiskAssessment.CurrentPageIndex + 1
                        , gvRiskAssessment.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , sortexpression, sortdirection
                        , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                        , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                        , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                        , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                        , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                        , nvc != null ? General.GetNullableInteger(nvc["ddlCategory"]) : null
                        );

            General.SetPrintOptions("gvRiskAssessment", "Non Routine RA", alCaptions, alColumns, ds);

            gvRiskAssessment.DataSource = ds;
            gvRiskAssessment.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string vesselid = "";
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Type", "Activity", "Revision No", "Status", "Approved By" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentStandardTemplateRAFilter;

            if (Filter.CurrentStandardTemplateRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            if (nvc != null && General.GetNullableInteger(nvc["ucVessel"]) == null)
            {
                nvc["ucVessel"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            DataSet ds = PhoenixInspectionRiskAssessmentGeneric.PhoenixInspectionNonRoutineRiskAssessmentSearch(
                    nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , sortexpression, sortdirection
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlCategory"]) : null
                    );

            General.ShowExcel("Non Routine RA", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuGeneric_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("SEARCH"))
        {
            if (ddlRAType.SelectedValue == "GEN")
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ddlRAType", ddlRAType.SelectedValue);
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateIntendedWorkFrom", ucDateIntendedWorkFrom.Text);
                criteria.Add("ucDateIntendedWorkTo", ucDateIntendedWorkTo.Text);
                criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
                criteria.Add("ddlCategory", ddlCategory.SelectedValue);
                Filter.CurrentGenericRAFilter = criteria;

                Response.Redirect("../Inspection/InspectionMainFleetRAGenericList.aspx", false);
            }

            if (ddlRAType.SelectedValue == "CAR")
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ddlRAType", ddlRAType.SelectedValue);
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateIntendedWorkFrom", ucDateIntendedWorkFrom.Text);
                criteria.Add("ucDateIntendedWorkTo", ucDateIntendedWorkTo.Text);
                criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
                criteria.Add("ddlCategory", ddlCategory.SelectedValue);
                Filter.CurrentCargoRAFilter = criteria;

                Response.Redirect("../Inspection/InspectionRACargoList.aspx", false);
            }

            if (ddlRAType.SelectedValue == "MACH")
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateIntendedWorkFrom", ucDateIntendedWorkFrom.Text);
                criteria.Add("ucDateIntendedWorkTo", ucDateIntendedWorkTo.Text);
                criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
                criteria.Add("ddlCategory", ddlCategory.SelectedValue);
                Filter.CurrentMachineryRAFilter = criteria;

                Response.Redirect("../Inspection/InspectionMainFleetRAMachineryList.aspx", false);
            }

            if (ddlRAType.SelectedValue == "NAV")
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateIntendedWorkFrom", ucDateIntendedWorkFrom.Text);
                criteria.Add("ucDateIntendedWorkTo", ucDateIntendedWorkTo.Text);
                criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
                criteria.Add("ddlCategory", ddlCategory.SelectedValue);

                Filter.CurrentNavigationRAFilter = criteria;
                Response.Redirect("../Inspection/InspectionMainFleetRANavigationList.aspx", false);
            }
            else
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateIntendedWorkFrom", ucDateIntendedWorkFrom.Text);
                criteria.Add("ucDateIntendedWorkTo", ucDateIntendedWorkTo.Text);
                criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
                criteria.Add("ddlCategory", ddlCategory.SelectedValue);

                Filter.CurrentStandardTemplateRAFilter = criteria;

                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }

        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentStandardTemplateRAFilter = null;
            txtRefNo.Text = "";
            ucTechFleet.SelectedFleet = "";
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() == "0")
            {
                ucVessel.SelectedVessel = "";
            }
            ucDateIntendedWorkFrom.Text = "";
            ucDateIntendedWorkTo.Text = "";
            ddlStatus.SelectedValue = "Dummy";
            ddlCategory.SelectedValue = "Dummy";
            gvRiskAssessment.Rebind();
        }
    }
    protected void gvRiskAssessment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblGenericID = (RadLabel)e.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
                RadLabel lbltypeid = (RadLabel)e.Item.FindControl("lblTypeid");

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
                LinkButton imgIssue = (LinkButton)e.Item.FindControl("imgIssue");
                LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
                LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
                LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
                RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatus");
                LinkButton imgProposeTemplate = (LinkButton)e.Item.FindControl("imgProposeTemplate");
                RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    imgProposeTemplate.Visible = true;
                }
                else
                {
                    imgProposeTemplate.Visible = false;
                }

                if (lblInstallcode != null)
                {
                    if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0) // when RA is approved in vessel.
                    {
                        if (imgApprove != null)
                        {
                            imgApprove.ToolTip = "Emergency Override";

                            if (lbltypeid.Text == "1")
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=1','medium'); return true;");
                            }
                            if (lbltypeid.Text == "2")
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=2','medium'); return true;");
                            }
                            if (lbltypeid.Text == "3")
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=3','medium'); return true;");
                            }
                            if (lbltypeid.Text == "4")
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=5','medium'); return true;");
                            }
                        }
                    }
                    else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                    {
                        if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                        {
                            if (imgApprove != null)
                            {
                                if (lbltypeid.Text == "1")
                                {
                                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=1','large'); return true;");
                                }
                                if (lbltypeid.Text == "2")
                                {
                                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=2','large'); return true;");
                                }
                                if (lbltypeid.Text == "3")
                                {
                                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=3','large'); return true;");
                                }
                                if (lbltypeid.Text == "4")
                                {
                                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=5','large'); return true;");
                                }
                            }
                        }
                    }
                }

                if (imgCopyTemplate != null)
                {
                    if (lbltypeid.Text == "1")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGeneric.aspx?genericid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigation.aspx?navigationid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachinery.aspx?machineryid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRACargo.aspx?genericid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                    }
                }

                if (imgProposeTemplate != null)
                {
                    if (lbltypeid.Text == "1")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGeneric.aspx?genericid=" + lblGenericID.Text + "&CopyType=2&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigation.aspx?navigationid=" + lblGenericID.Text + "&CopyType=2&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachinery.aspx?machineryid=" + lblGenericID.Text + "&CopyType=2&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRACargo.aspx?genericid=" + lblGenericID.Text + "&CopyType=2&showall=1" + "'); return true;");
                    }
                }

                //imgCopy.Visible = false;
                imgrevision.Visible = false;
                imgApprove.Visible = false;
                imgIssue.Visible = false;
                imgCopyTemplate.Visible = false;

                LinkButton imgComparison = (LinkButton)e.Item.FindControl("imgComparison");
                imgComparison.Visible = false;
                Image imgFlag = e.Item.FindControl("imgFlag") as Image;
                if (imgFlag != null && drv["FLDINTENDEDWORKDATEDUEYN"].ToString().Equals("3"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                    imgFlag.ToolTip = "Overdue";
                }
                else if (imgFlag != null && drv["FLDINTENDEDWORKDATEDUEYN"].ToString().Equals("2"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
                    imgFlag.ToolTip = "Due within 2 Weeks";
                }
                else
                {
                    if (imgFlag != null) imgFlag.Visible = false;
                }

                Image imgOfficeFlag = (Image)e.Item.FindControl("imgOfficeFlag");

                if (drv["FLDCOPIEDFROMSTANDARDTEMPLATE"].ToString() == "1")
                {
                    imgOfficeFlag.Visible = true;
                    imgOfficeFlag.ImageUrl = Session["images"] + "/" + "green.png";
                    imgOfficeFlag.ToolTip = "Copied from Standard Template";

                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    {
                        imgComparison.Visible = true;
                    }
                }

                if (imgComparison != null)
                {
                    if (lbltypeid.Text == "1")
                    {
                        imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGenericComparison.aspx?genericid=" + lblGenericID.Text + "'); return true;");
                    }

                    if (lbltypeid.Text == "2")
                    {
                        imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigationComparison.aspx?navigationid=" + lblGenericID.Text + "'); return true;");
                    }

                    if (lbltypeid.Text == "3")
                    {
                        imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachineryComparison.aspx?machneryid=" + lblGenericID.Text + "'); return true;");
                    }

                    if (lbltypeid.Text == "4")
                    {
                        imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRACargoComparison.aspx?cargoid=" + lblGenericID.Text + "'); return true;");
                    }
                }

                cmdRevision.Visible = false;
                if (cmdRevision != null && lblVesselid.Text == "0")
                {
                    cmdRevision.Visible = true;
                }

                if (lblStatusid.Text == "1")
                {
                    //imgCopy.Visible = false;
                    imgrevision.Visible = false;
                    imgApprove.Visible = true;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = false;
                }

                else if (lblStatusid.Text == "4")
                {
                    //imgCopy.Visible = false;
                    imgrevision.Visible = false;
                    imgApprove.Visible = true;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = false;
                    if (lblVesselid.Text == "0")
                    {
                        imgIssue.Visible = true;
                        imgApprove.Visible = false;
                    }
                }
                else if (lblStatusid.Text == "5")
                {
                    //imgCopy.Visible = true;
                    if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                        imgrevision.Visible = true;
                    else
                        imgrevision.Visible = false;

                    imgApprove.Visible = false;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = true;
                }

                else if (lblStatusid.Text == "6")
                {
                    //imgCopy.Visible = true;
                    if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                        imgrevision.Visible = true;
                    else
                        imgrevision.Visible = false;
                    imgApprove.Visible = false;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = true;
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    if (imgCopyTemplate != null) imgCopyTemplate.ToolTip = "Use as RA";
                }
                else
                {
                    if (imgCopyTemplate != null) imgCopyTemplate.ToolTip = "Copy RA";
                }

                if (lblVesselid != null && lblVesselid.Text == "0")
                {
                    if (imgApprove != null) imgApprove.ToolTip = "Request for Approval";
                }
                else
                {
                    if (imgApprove != null) imgApprove.ToolTip = "Approve/Reject";
                }

                if ((imgIssue != null) && (lblVesselid.Text == "0"))
                {
                    if (lbltypeid.Text == "1")
                    {
                        imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=1','medium'); return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=2','medium'); return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=3','medium'); return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=5','medium'); return true;");
                    }
                }

                LinkButton cmdRAGeneric = (LinkButton)e.Item.FindControl("cmdRAGeneric");
                if (cmdRAGeneric != null)
                {
                    cmdRAGeneric.Visible = SessionUtil.CanAccess(this.ViewState, cmdRAGeneric.CommandName);
                    if (lbltypeid.Text == "1")
                    {
                        cmdRAGeneric.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lblGenericID.Text + "&showmenu=0&showexcel=NO');return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        cmdRAGeneric.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblGenericID.Text + "&showmenu=0&showexcel=NO');return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        cmdRAGeneric.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblGenericID.Text + "&showmenu=0&showexcel=NO');return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        cmdRAGeneric.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + lblGenericID.Text + "&showmenu=0&showexcel=NO');return true;");
                    }
                }

                if (cmdRevision != null)
                {
                    if (lbltypeid.Text == "1")
                    {
                        cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGenericRevisionList.aspx?genericid=" + lblGenericID.Text + "');return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationRevisionList.aspx?navigationid=" + lblGenericID.Text + "');return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryRevisionList.aspx?machineryid=" + lblGenericID.Text + "');return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoRevisionList.aspx?genericid=" + lblGenericID.Text + "');return true;");
                    }
                }

                RadLabel lblWorkActivity = (RadLabel)e.Item.FindControl("lblWorkActivity");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucWorkActivity");
                if (uct != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lblWorkActivity.ClientID;
                }

                RadLabel lblApprovedBy = (RadLabel)e.Item.FindControl("lblApprovedBy");
                UserControlToolTip ucApprovedBy = (UserControlToolTip)e.Item.FindControl("ucApprovedBy");
                if (ucApprovedBy != null)
                {
                    ucApprovedBy.Position = ToolTipPosition.TopCenter;
                    ucApprovedBy.TargetControlId = lblApprovedBy.ClientID;
                }

                if (imgApprove != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName)) imgApprove.Visible = false;
                }
                if (imgIssue != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgIssue.CommandName)) imgIssue.Visible = false;
                }
                //if (imgrevision != null)
                //{
                //    if (!SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName)) imgrevision.Visible = false;
                //}
                if (cmdRevision != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdRevision.CommandName)) cmdRevision.Visible = false;
                }
                if (cmdRAGeneric != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdRAGeneric.CommandName)) cmdRAGeneric.Visible = false;
                }
                if (imgCopyTemplate != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgCopyTemplate.CommandName)) imgCopyTemplate.Visible = false;
                }
                if (imgProposeTemplate != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgProposeTemplate.CommandName)) imgProposeTemplate.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvRiskAssessment.SelectedIndexes.Clear();
        gvRiskAssessment.EditIndexes.Clear();
        gvRiskAssessment.DataSource = null;
        gvRiskAssessment.Rebind();
    }

    protected void gvRiskAssessment_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.Item is GridEditableItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = gce.Item.ItemIndex;

                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");
                RadLabel lblvesselid = (RadLabel)gce.Item.FindControl("lblvesselid");

                if (gce.CommandName.ToUpper().Equals("EDITROW"))
                {
                    RadLabel lbltypeid1 = (RadLabel)gce.Item.FindControl("lblTypeid");
                    ViewState["RAID"] = lbl.Text;
                    ViewState["GENERICID"] = lbl.Text;
                    ViewState["STATUS"] = lblstatus.Text;
                    ViewState["INSTALLCODE"] = lblInstallcode.Text;
                    ViewState["TYPEID"] = lbltypeid1.Text;

                    BindPageURL(nRow);
                    //SetRowSelection();
                    if (lbltypeid.Text == "1")
                    {
                        Response.Redirect("../Inspection/InspectionRAGeneric.aspx?showall=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                    }
                    if (lbltypeid.Text == "2")
                    {
                        Response.Redirect("../Inspection/InspectionRANavigation.aspx?showall=1&navigationid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                    }
                    if (lbltypeid.Text == "3")
                    {
                        Response.Redirect("../Inspection/InspectionRAMachinery.aspx?showall=1&machineryid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                    }
                    if (lbltypeid.Text == "4")
                    {
                        Response.Redirect("../Inspection/InspectionRACargo.aspx?showall=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                    }

                }
                if (gce.CommandName.ToUpper().Equals("APPROVE"))
                {
                    BindPageURL(nRow);
                   // SetRowSelection();
                    if (lblInstallcode != null && lblInstallcode.Text == "0" && lblvesselid.Text == "0")
                    {
                        if (lbltypeid.Text == "1")
                        {
                            PhoenixInspectionRiskAssessmentGeneric.MainFleetApproveGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
                        if (lbltypeid.Text == "2")
                        {
                            PhoenixInspectionRiskAssessmentNavigation.MainFleetApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
                        if (lbltypeid.Text == "3")
                        {
                            PhoenixInspectionRiskAssessmentMachinery.MainFleetApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
                        if (lbltypeid.Text == "4")
                        {
                            PhoenixInspectionRiskAssessmentCargo.MainFleetApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
                    }

                }
                //if (gce.CommandName.ToUpper().Equals("ISSUE"))
                //{
                //    BindPageURL(nRow);
                //    SetRowSelection();
                //}
                //if (gce.CommandName.ToUpper().Equals("REVISION"))
                //{
                //    BindPageURL(nRow);
                //    SetRowSelection();
                //    ViewState["GENERICID"] = lbl.Text;
                //    ViewState["TYPE"] = lbltypeid.Text;
                //    RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.?", "ConfirmRevision", 320, 150, null, "Revision");
                //}
                //if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
                //{
                //    BindPageURL(nRow);
                //    SetRowSelection();
                //}
                //if (gce.CommandName.ToUpper().Equals("RAGENERIC"))
                //{
                //    BindPageURL(nRow);
                //    SetRowSelection();
                //}
                else if (gce.CommandName == "Page")
                {
                    ViewState["PAGENUMBER"] = null;
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirmApprove_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
            {
                if (ViewState["TYPE"].ToString() == "1")
                {
                    PhoenixInspectionRiskAssessmentGeneric.ApproveGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "2")
                {
                    PhoenixInspectionRiskAssessmentNavigation.ApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "3")
                {
                    PhoenixInspectionRiskAssessmentMachinery.ApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "4")
                {
                    PhoenixInspectionRiskAssessmentCargo.ApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }

                ucStatus.Text = "Approved Successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirmIssue_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
            {
                if (ViewState["TYPE"].ToString() == "1")
                {
                    PhoenixInspectionRiskAssessmentGeneric.IssueGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "2")
                {
                    PhoenixInspectionRiskAssessmentNavigation.IssueNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "3")
                {
                    PhoenixInspectionRiskAssessmentMachinery.IssueMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "4")
                {
                    PhoenixInspectionRiskAssessmentCargo.IssueCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                ucStatus.Text = "Issued Successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ////gvRiskAssessment.EditIndex = -1;
        ////gvRiskAssessment.SelectedIndex = -1;
        //BindData();
        ////SetPageNavigator();
        gvRiskAssessment.Rebind();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentGenericID = (RadLabel)gvRiskAssessment.Items[rowindex].FindControl("lblRiskAssessmentGenericID");
            if (lblRiskAssessmentGenericID != null)
            {
                Filter.CurrentSelectedGenericRA = lblRiskAssessmentGenericID.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        try
        {
            gvRiskAssessment.SelectedIndexes.Clear();
            for (int i = 0; i < gvRiskAssessment.Items.Count; i++)
            {
                if (gvRiskAssessment.MasterTableView.Items[i].GetDataKeyValue("FLDRISKASSESSMENTID").ToString().Equals(Filter.CurrentSelectedGenericRA.ToString()))
                {
                    gvRiskAssessment.MasterTableView.Items[i].Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
            {
                if (ViewState["TYPE"].ToString() == "1")
                {
                    PhoenixInspectionRiskAssessmentGeneric.ReviseGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "2")
                {
                    PhoenixInspectionRiskAssessmentNavigation.ReviseNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "3")
                {
                    PhoenixInspectionRiskAssessmentMachinery.ReviseMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "4")
                {
                    PhoenixInspectionRiskAssessmentCargo.ReviseCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                ucStatus.Text = "RA is revised.";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    //protected void MenuGenericMain_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

    //    if (dce.CommandName.ToUpper().Equals("SEARCH"))
    //    {
    //        Response.Redirect("../Inspection/InspectionMainFleetNonRoutineRAFilter.aspx", false);
    //    }
    //}

    protected void gvRiskAssessment_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvRiskAssessment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}