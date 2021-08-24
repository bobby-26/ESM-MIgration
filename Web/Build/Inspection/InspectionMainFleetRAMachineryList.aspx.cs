using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionMainFleetRAMachineryList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMainFleetRAMachineryList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentMachinery')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMainFleetRAMachineryList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMainFleetRAMachineryList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMainFleetRAMachineryList.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuMachinery.AccessRights = this.ViewState;
        MenuMachinery.MenuList = toolbar.Show();

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
            ddlRAType.SelectedValue = "MACH";
            ViewState["COMPANYID"] = "";

            ddlStatus.ClearSelection();

            NameValueCollection nvcFilter = Filter.CurrentMachineryRAFilter;
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
                    Filter.CurrentMachineryRAFilter = null;
                }
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
            gvRiskAssessmentMachinery.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            //PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Search", "SEARCH");

            //MenuMachineryMain.AccessRights = this.ViewState;
            //MenuMachineryMain.MenuList = toolbarmain.Show();
            BindCategory();
        }

       // BindData();
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

        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(3, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }


    private void BindData()
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

        NameValueCollection nvc = Filter.CurrentMachineryRAFilter;
        if (Filter.CurrentMachineryRAFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        DataSet ds = PhoenixInspectionRiskAssessmentMachinery.RiskAssessmentMainFleetMachinerySearch(
                     nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , sortexpression, sortdirection
                    , gvRiskAssessmentMachinery.CurrentPageIndex + 1
                    , gvRiskAssessmentMachinery.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : General.GetNullableInteger(ViewState["Status"].ToString())
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlCategory"]) : null
                    );

        General.SetPrintOptions("gvRiskAssessmentMachinery", "Machinery", alCaptions, alColumns, ds);

        gvRiskAssessmentMachinery.DataSource = ds;
        gvRiskAssessmentMachinery.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
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

            NameValueCollection nvc = Filter.CurrentMachineryRAFilter;
            if (Filter.CurrentMachineryRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataSet ds = PhoenixInspectionRiskAssessmentMachinery.RiskAssessmentMainFleetMachinerySearch(
                     nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , sortexpression, sortdirection
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlCategory"]) : null
                    );

            General.ShowExcel("Machinery", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuMachinery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
            {
                Response.Redirect("../Inspection/InspectionRAMachineryDetails.aspx?status=", false);
            }
            else
            {
                Response.Redirect("../Inspection/InspectionRAMachinery.aspx?status=", false);
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
                ViewState["PAGENUMBER"] = 1;
                BindData();
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
                Response.Redirect("../Inspection/InspectionMainFleetRANavigationList.aspx", true);
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

                Response.Redirect("../Inspection/InspectionNonRoutineRiskAssessmentList.aspx", false);
            }
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentMachineryRAFilter = null;
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
        }
        BindData();
        gvRiskAssessmentMachinery.Rebind();
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

    protected void gvRiskAssessmentMachinery_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblMachineryID = (RadLabel)e.Item.FindControl("lblRiskAssessmentMachineryID");
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
                            imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAApprovalRemarks.aspx?RATEMPLATEID=" + lblMachineryID.Text + "&TYPE=3','medium'); return true;");
                        }
                    }
                    else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                    {
                        if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                        {
                            if (imgApprove != null)
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblMachineryID.Text + "&TYPE=3','large'); return true;");
                            }
                        }
                    }
                }
                if (imgCopyTemplate != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachineryDetails.aspx?machineryid=" + lblMachineryID.Text + "&CopyType=1" + "'); return true;");
                    }
                    else
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachinery.aspx?machineryid=" + lblMachineryID.Text + "&CopyType=1" + "'); return true;");
                    }
                }

                if (imgProposeTemplate != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachineryDetails.aspx?machineryid=" + lblMachineryID.Text + "&CopyType=2" + "'); return true;");
                    }
                    else
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachinery.aspx?machineryid=" + lblMachineryID.Text + "&CopyType=2" + "'); return true;");
                    }
                }

                if (imgIssue != null)
                {
                    imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblMachineryID.Text + "&TYPE=3','medium'); return true;");
                }

                if (imgComparison != null)
                {
                    imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachineryComparison.aspx?machneryid=" + lblMachineryID.Text + "'); return true;");
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

                LinkButton cmdRAMachinery = (LinkButton)e.Item.FindControl("cmdRAMachinery");
                if (cmdRAMachinery != null)
                {
                    cmdRAMachinery.Visible = SessionUtil.CanAccess(this.ViewState, cmdRAMachinery.CommandName);
                    //cmdRAMachinery.Visible = false;
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    cmdRAMachinery.Attributes.Add("onclick", "Openpopup('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblMachineryID.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    //{
                    cmdRAMachinery.Attributes.Add("onclick", "javascript:openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblMachineryID.Text + "&showmenu=0&showexcel=NO');return true;");
                    //}
                }

                if (cmdRevision != null)
                {
                    cmdRevision.Attributes.Add("onclick", "openNewWindow('RAMachineryRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryRevisionList.aspx?machineryid=" + lblMachineryID.Text + "');return true;");
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
                if (cmdRAMachinery != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdRAMachinery.CommandName)) cmdRAMachinery.Visible = false;
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

    protected void gvRiskAssessmentMachinery_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = gce.Item.ItemIndex;

                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentMachineryID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");

                if (gce.CommandName.ToUpper().Equals("EDITROW"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    {
                        Response.Redirect("../Inspection/InspectionRAMachineryDetails.aspx?machineryid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                    }
                    else
                    {
                        Response.Redirect("../Inspection/InspectionRAMachinery.aspx?machineryid=" + lbl.Text + "&status=" + lblstatus.Text, false);
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
                        //    ViewState["MACHINERYID"] = lbl.Text;
                        //    ucConfirmApprove.Visible = true;
                        //    ucConfirmApprove.Text = "Once the RA is approved, It cannot be edited. Are you sure to continue?";
                        //    return;
                        //}
                        if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) == 0)
                        {
                            PhoenixInspectionRiskAssessmentMachinery.ApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
                    }
                }
                if (gce.CommandName.ToUpper().Equals("ISSUE"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    //ViewState["MACHINERYID"] = lbl.Text;
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
                    ViewState["MACHINERYID"] = lbl.Text;
                    RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.?", "ConfirmRevision", 320, 150, null, "Revision");
                }

                if (gce.CommandName.ToUpper().Equals("RAMACHINERY"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                }
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
            if (ViewState["MACHINERYID"] != null && ViewState["MACHINERYID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentMachinery.ApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["MACHINERYID"].ToString()));
                ucStatus.Text = "Approved Successfully";
                gvRiskAssessmentMachinery.Rebind();
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
            if (ViewState["MACHINERYID"] != null && ViewState["MACHINERYID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentMachinery.IssueMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["MACHINERYID"].ToString()));
                ucStatus.Text = "Issued Successfully";
                gvRiskAssessmentMachinery.Rebind();
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
            RadLabel lblRiskAssessmentMachineryID = (RadLabel)gvRiskAssessmentMachinery.Items[rowindex].FindControl("lblRiskAssessmentMachineryID");
            if (lblRiskAssessmentMachineryID != null)
            {
                Filter.CurrentSelectedMachineryRA = lblRiskAssessmentMachineryID.Text;
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
        //gvRiskAssessmentMachinery.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessmentMachinery.Rows.Count; i++)
        //{
        //    if (gvRiskAssessmentMachinery.DataKeys[i].Value.ToString().Equals(Filter.CurrentSelectedMachineryRA.ToString()))
        //    {
        //        gvRiskAssessmentMachinery.SelectedIndex = i;
        //    }
        //}
    }

    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["MACHINERYID"] != null && ViewState["MACHINERYID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentMachinery.ReviseMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["MACHINERYID"].ToString()));
                ucStatus.Text = "RA is revised.";
                gvRiskAssessmentMachinery.Rebind();
              //  BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    //protected void MenuMachineryMain_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

    //    if (dce.CommandName.ToUpper().Equals("SEARCH"))
    //    {
    //        Response.Redirect("../Inspection/InspectionMainFleetNonRoutineRAFilter.aspx", false);
    //    }
    //}

    protected void gvRiskAssessmentMachinery_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvRiskAssessmentMachinery_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentMachinery.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
