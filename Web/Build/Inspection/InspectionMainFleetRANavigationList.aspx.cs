using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionMainFleetRANavigationList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMainFleetRANavigationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentNavigation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMainFleetRANavigationList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMainFleetRANavigationList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMainFleetRANavigationList.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuNavigation.AccessRights = this.ViewState;
        MenuNavigation.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ucConfirmApprove.Attributes.Add("style", "display:none");
            ucConfirmIssue.Attributes.Add("style", "display:none");
            ucConfirmRevision.Attributes.Add("style", "display:none");

            ViewState["REFID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["Status"] = null;

            BindType();
            BindStatus();
            ddlRAType.SelectedValue = "NAV";

            ViewState["COMPANYID"] = "";

            ddlStatus.ClearSelection();

            NameValueCollection nvcFilter = Filter.CurrentNavigationRAFilter;
            ViewState["Status"] = nvcFilter["ddlStatus"];
            ddlStatus.SelectedValue = ViewState["Status"].ToString();

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
                    Filter.CurrentNavigationRAFilter = null;
                }
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
            gvRiskAssessmentNavigation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            BindCategory();
        }
        //  BindData();
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

    protected void BindCategory()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(1, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string vesselid = "";
        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDINTENDEDWORKDATE", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
        string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Type", "Activity / Conditions", "Intended Work", "Revision No", "Status", "Approved By" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentNavigationRAFilter;
        if (Filter.CurrentNavigationRAFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        DataSet ds = PhoenixInspectionRiskAssessmentNavigation.RiskAssessmentMainFleetNavigationSearch(
                     nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , sortexpression, sortdirection
                    , gvRiskAssessmentNavigation.CurrentPageIndex + 1
                    , gvRiskAssessmentNavigation.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlCategory"]) : null
                    );

        General.SetPrintOptions("gvRiskAssessmentNavigation", "Navigation", alCaptions, alColumns, ds);

        gvRiskAssessmentNavigation.DataSource = ds;
        gvRiskAssessmentNavigation.VirtualItemCount = iRowCount;
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
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDINTENDEDWORKDATE", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Type", "Activity / Conditions", "Intended Work", "Revision No", "Status", "Approved By" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentNavigationRAFilter;
            if (Filter.CurrentNavigationRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataSet ds = PhoenixInspectionRiskAssessmentNavigation.RiskAssessmentMainFleetNavigationSearch(
                         nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                        , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                        , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                        , sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvRiskAssessmentNavigation.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                        , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                        , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                        , nvc != null ? General.GetNullableInteger(nvc["ddlCategory"]) : null
                        );

            General.ShowExcel("Navigation", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuNavigation_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
            {
                Response.Redirect("../Inspection/InspectionRANavigationDetails.aspx?status=", false);
            }
            else
            {
                Response.Redirect("../Inspection/InspectionRANavigation.aspx?status=", false);
            }
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
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
                Response.Redirect("../Inspection/InspectionMainFleetRAGenericList.aspx", true);

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

                Response.Redirect("../Inspection/InspectionRACargoList.aspx", true);
            }

            if (ddlRAType.SelectedValue == "MACH")
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
                Filter.CurrentMachineryRAFilter = criteria;

                Response.Redirect("../Inspection/InspectionMainFleetRAMachineryList.aspx", true);
            }

            if (ddlRAType.SelectedValue == "NAV")
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

                Filter.CurrentNavigationRAFilter = criteria;
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (ddlRAType.SelectedValue == "ALL")
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

                Filter.CurrentStandardTemplateRAFilter = criteria;

                Response.Redirect("../Inspection/InspectionNonRoutineRiskAssessmentList.aspx", true);
            }
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentNavigationRAFilter = null;
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
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
        BindData();
        gvRiskAssessmentNavigation.Rebind();
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void gvRiskAssessmentNavigation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblNavigationID = (RadLabel)e.Item.FindControl("lblRiskAssessmentNavigationID");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");
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

                Image imgOfficeFlag = e.Item.FindControl("imgOfficeFlag") as Image;
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


                //ImageButton imgCopy = (ImageButton)e.Row.FindControl("imgCopy");

                //imgCopy.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'InspectionRATemplateMapping.aspx?RISKASSESSMENTID=" + lblGenericID.Text + "&TYPE=1');return false;");


                //if (imgCopy != null) imgCopy.Visible = SessionUtil.CanAccess(this.ViewState, imgCopy.CommandName);


                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
                LinkButton imgIssue = (LinkButton)e.Item.FindControl("imgIssue");
                LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
                LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
                LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
                RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatus");
                LinkButton imgProposeTemplate = (LinkButton)e.Item.FindControl("imgProposeTemplate");
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
                            imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAApprovalRemarks.aspx?RATEMPLATEID=" + lblNavigationID.Text + "&TYPE=2','medium'); return true;");
                        }
                    }
                    else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                    {
                        if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                        {
                            if (imgApprove != null)
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblNavigationID.Text + "&TYPE=2','large'); return true;");
                            }
                        }
                    }
                }

                if (imgCopyTemplate != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigationDetails.aspx?navigationid=" + lblNavigationID.Text + "&CopyType=1" + "'); return true;");
                    }
                    else
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigation.aspx?navigationid=" + lblNavigationID.Text + "&CopyType=1" + "'); return true;");
                    }
                }

                if (imgProposeTemplate != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigationDetails.aspx?navigationid=" + lblNavigationID.Text + "&CopyType=2" + "'); return true;");
                    }
                    else
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigation.aspx?navigationid=" + lblNavigationID.Text + "&CopyType=2" + "'); return true;");
                    }
                }

                if (imgIssue != null)
                {
                    imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblNavigationID.Text + "&TYPE=2','medium'); return true;");
                }
                //imgCopy.Visible = false;
                imgrevision.Visible = false;
                imgApprove.Visible = false;
                imgIssue.Visible = false;
                imgCopyTemplate.Visible = false;

                if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text == "0")
                {
                    if (cmdRevision != null) cmdRevision.Visible = true;
                }
                else
                {
                    if (cmdRevision != null) cmdRevision.Visible = false;
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

                else if ((lblStatusid.Text == "4") && (lblVesselid.Text == "0"))
                {
                    imgIssue.Visible = true;
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
                    if (imgCopyTemplate != null) imgCopyTemplate.ToolTip = "Use RA";
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
                    if (imgApprove != null) imgApprove.ToolTip = "Approve/Disapprove";
                }

                LinkButton cmdRANavigation = (LinkButton)e.Item.FindControl("cmdRANavigation");
                if (cmdRANavigation != null)
                {
                    cmdRANavigation.Visible = SessionUtil.CanAccess(this.ViewState, cmdRANavigation.CommandName);
                    //cmdRANavigation.Visible = false;

                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    cmdRANavigation.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblNavigationID.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    //{
                    cmdRANavigation.Attributes.Add("onclick", "javascript:openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblNavigationID.Text + "&showmenu=0&showexcel=NO');return true;");
                    //}
                }

                if (cmdRevision != null)
                {
                    cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('RANavigationRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationRevisionList.aspx?navigationid=" + lblNavigationID.Text + "');return true;");
                }

                if (imgComparison != null)
                {
                    imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigationComparison.aspx?navigationid=" + lblNavigationID.Text + "'); return true;");
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
                if (imgrevision != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName)) imgrevision.Visible = false;
                }
                if (cmdRevision != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdRevision.CommandName)) cmdRevision.Visible = false;
                }
                if (cmdRANavigation != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdRANavigation.CommandName)) cmdRANavigation.Visible = false;
                }
                if (imgCopyTemplate != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgCopyTemplate.CommandName)) imgCopyTemplate.Visible = false;
                }
                if (imgProposeTemplate != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgProposeTemplate.CommandName)) imgProposeTemplate.Visible = false;
                }

                if (imgComparison != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgComparison.CommandName)) imgComparison.Visible = false;
                }
            }
        }
    }

    protected void gvRiskAssessmentNavigation_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = gce.Item.ItemIndex;

                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentNavigationID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");

                if (gce.CommandName.ToUpper().Equals("EDITROW"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    {
                        Response.Redirect("../Inspection/InspectionRANavigationDetails.aspx?navigationid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                    }
                    else
                    {
                        Response.Redirect("../Inspection/InspectionRANavigation.aspx?navigationid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                    }
                }
                if (gce.CommandName.ToUpper().Equals("APPROVE"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    if (lblInstallcode != null && lblInstallcode.Text == "0")
                    {
                        //if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) > 0)
                        //{
                        //    ViewState["NAVIGATIONID"] = lbl.Text;
                        //    ucConfirmApprove.Visible = true;
                        //    ucConfirmApprove.Text = "Once the RA is approved, It cannot be edited. Are you sure to continue?";
                        //    return;
                        //}
                        if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) == 0)
                        {
                            PhoenixInspectionRiskAssessmentNavigation.MainFleetApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
                    }
                }
                if (gce.CommandName.ToUpper().Equals("ISSUE"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    //ViewState["NAVIGATIONID"] = lbl.Text;
                    //ucConfirmIssue.Visible = true;
                    //ucConfirmIssue.Text = "Once the RA is issued, It cannot be edited. Are you sure to continue?";
                    //return;	                
                }
                if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                }
                if (gce.CommandName.ToUpper().Equals("REVISION"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    ViewState["NAVIGATIONID"] = lbl.Text;
                    RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.?", "ConfirmRevision", 320, 150, null, "cfIssue");
                }
                if (gce.CommandName.ToUpper().Equals("RANAVIGATION"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                }
                BindData();
                gvRiskAssessmentNavigation.Rebind();
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
            if (ViewState["NAVIGATIONID"] != null && ViewState["NAVIGATIONID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentNavigation.ApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["NAVIGATIONID"].ToString()));
                ucStatus.Text = "Approved Successfully";
                gvRiskAssessmentNavigation.Rebind();
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
            if (ViewState["NAVIGATIONID"] != null && ViewState["NAVIGATIONID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentNavigation.IssueNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["NAVIGATIONID"].ToString()));
                ucStatus.Text = "Issued Successfully";
                gvRiskAssessmentNavigation.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentNavigationID = (RadLabel)gvRiskAssessmentNavigation.Items[rowindex].FindControl("lblRiskAssessmentNavigationID");
            if (lblRiskAssessmentNavigationID != null)
            {
                Filter.CurrentSelectedNavigationRA = lblRiskAssessmentNavigationID.Text;
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
        //gvRiskAssessmentNavigation.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessmentNavigation.Rows.Count; i++)
        //{
        //    if (gvRiskAssessmentNavigation.DataKeys[i].Value.ToString().Equals(Filter.CurrentSelectedNavigationRA.ToString()))
        //    {
        //        gvRiskAssessmentNavigation.SelectedIndex = i;
        //    }
        //}
    }

    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["NAVIGATIONID"] != null && ViewState["NAVIGATIONID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentNavigation.ReviseNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["NAVIGATIONID"].ToString()));
                ucStatus.Text = "RA is revised.";
                gvRiskAssessmentNavigation.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    ////protected void MenuNavigationMain_TabStripCommand(object sender, EventArgs e)
    ////{
    ////    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

    ////    if (dce.CommandName.ToUpper().Equals("SEARCH"))
    ////    {
    ////        Response.Redirect("../Inspection/InspectionMainFleetNonRoutineRAFilter.aspx", false);
    ////    }
    ////}


    protected void gvRiskAssessmentNavigation_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvRiskAssessmentNavigation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentNavigation.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
