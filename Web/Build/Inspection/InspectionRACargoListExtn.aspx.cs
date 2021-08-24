using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRACargoListExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRACargoListExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentCargo')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRACargoListExtn.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRACargoListExtn.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRACargoListExtn.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuCargo.AccessRights = this.ViewState;
        MenuCargo.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["REFID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ucConfirmApprove.Attributes.Add("style", "display:none");
            ucConfirmRevision.Attributes.Add("style", "display:none");

            BindType();
            BindStatus();
            ddlRAType.SelectedValue = "CAR";

            ViewState["COMPANYID"] = "";

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
                    Filter.CurrentCargoRAFilter = null;
                }
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }

            gvRiskAssessmentCargo.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            BindCategory();
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
        ddlStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlStatus.DataBind();
        
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(2, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

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

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDACTIVITYCONDITIONS", "FLDCOMPLETIONDATE", "FLDTASKSTATUS", "FLDREVISIONNO", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Activity / Conditions", "Target Date for completion", "Task completed YN", "Revision No", "Status" };


        NameValueCollection nvc = Filter.CurrentCargoRAFilter;

        if (Filter.CurrentCargoRAFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        DataSet ds = PhoenixInspectionRiskAssessmentCargoExtn.PhoenixInspectionMainFleetRiskAssessmentCargoSearch(
                     nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , sortexpression, sortdirection
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvRiskAssessmentCargo.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlCategory"]) : null
                    );

        General.SetPrintOptions("gvRiskAssessmentCargo", "Cargo", alCaptions, alColumns, ds);

        gvRiskAssessmentCargo.DataSource = ds;
        gvRiskAssessmentCargo.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (Filter.CurrentSelectedCargoRA == null)
            {
                Filter.CurrentSelectedCargoRA = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTCARGOID"].ToString();
                gvRiskAssessmentCargo.SelectedIndexes.Clear();
            }
            SetRowSelection();
        }

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
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDACTIVITYCONDITIONS", "FLDCOMPLETIONDATE", "FLDTASKSTATUS", "FLDREVISIONNO", "FLDSTATUSNAME" };
            string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work","Activity / Conditions", "Target Date for completion", "Task completed YN", "Revision No", "Status" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentCargoRAFilter;

            if (Filter.CurrentCargoRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataSet ds = PhoenixInspectionRiskAssessmentCargoExtn.PhoenixInspectionMainFleetRiskAssessmentCargoSearch(
                      nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , sortexpression, sortdirection
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlCategory"]) : null
                    );

            General.ShowExcel("Cargo", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvRiskAssessmentCargo.SelectedIndexes.Clear();
        gvRiskAssessmentCargo.EditIndexes.Clear();
        gvRiskAssessmentCargo.DataSource = null;
        gvRiskAssessmentCargo.Rebind();
    }
    protected void MenuCargo_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {            
           Response.Redirect("../Inspection/InspectionRACargoExtn.aspx?status=", false);
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
                Response.Redirect("../Inspection/InspectionMainFleetRAGenericListExtn.aspx", false);

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

                ViewState["PAGENUMBER"] = 1;
                Rebind();
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

                Response.Redirect("../Inspection/InspectionMainFleetRAMachineryListExtn.aspx", false);
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
                Response.Redirect("../Inspection/InspectionMainFleetRANavigationListExtn.aspx", false);
            }

            if (ddlRAType.SelectedValue == "ALL")
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

                Response.Redirect("../Inspection/InspectionNonRoutineRiskAssessmentListExtn.aspx", false);
            }
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentCargoRAFilter = null;
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
            Rebind();
        }
    }

    protected void gvRiskAssessmentCargo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentCargo.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRiskAssessmentCargo_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentCargoID");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");

                LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
                LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
                LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
                LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
                RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatus");
               
                if (lblInstallcode != null)
                {
                    if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0) // when RA is approved in vessel.
                    {
                        if (imgApprove != null)
                        {
                            imgApprove.ToolTip = "Emergency Override";
                            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblCargoID.Text + "&TYPE=5','medium'); return true;");
                        }
                    }
                if (lblVesselid.Text == "0")
                {
                    imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblCargoID.Text + "&TYPE=5','medium'); return true;");
                }
                else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                    {
                        if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                        {
                            if (imgApprove != null)
                            {
                                imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalExtn.aspx?RATEMPLATEID=" + lblCargoID.Text + "&TYPE=5','large'); return true;");
                            }
                        }
                    }
            }


            if (imgCopyTemplate != null)
                {
                     imgCopyTemplate.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoExtn.aspx?genericid=" + lblCargoID.Text + "&CopyType=1" + "'); return true;");
                }

                //imgCopy.Visible = false;
                imgrevision.Visible = false;
                imgApprove.Visible = false;
                //imgIssue.Visible = false;
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
                    //imgIssue.Visible = false;
                    imgCopyTemplate.Visible = false;
                }

                else if (lblStatusid.Text == "4")
                {
                    //imgCopy.Visible = false;
                    imgrevision.Visible = false;
                    imgApprove.Visible = true;
                    //imgIssue.Visible = false;
                    //imgCopyTemplate.Visible = false;
                    if (lblVesselid.Text == "0")
                    {
                        //imgIssue.Visible = true;
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
                    //imgIssue.Visible = false;
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
                    //imgIssue.Visible = false;
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
                    if (imgApprove != null) imgApprove.ToolTip = "Approve";
                }
                else
                {
                    if (imgApprove != null) imgApprove.ToolTip = "Approve/Disapprove";
                }

                LinkButton cmdRACargo = (LinkButton)e.Item.FindControl("cmdRACargo");
                if (cmdRACargo != null)
                {
                    cmdRACargo.Visible = SessionUtil.CanAccess(this.ViewState, cmdRACargo.CommandName);
                    cmdRACargo.Attributes.Add("onclick", "openNewWindow('RACargo', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGONEW&genericid=" + lblCargoID.Text + "&showmenu=0&showexcel=NO');return true;");
                }

                if (cmdRevision != null)
                {
                    cmdRevision.Attributes.Add("onclick", "openNewWindow('RACargoRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoRevisionListExtn.aspx?genericid=" + lblCargoID.Text + "');return true;");
                }

               
                if (imgApprove != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName)) imgApprove.Visible = false;
                }
                //if (imgIssue != null)
                //{
                //    if (!SessionUtil.CanAccess(this.ViewState, imgIssue.CommandName)) imgIssue.Visible = false;
                //}
                if (imgrevision != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName)) imgrevision.Visible = false;
                }
                if (cmdRevision != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdRevision.CommandName)) cmdRevision.Visible = false;
                }
                if (cmdRACargo != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdRACargo.CommandName)) cmdRACargo.Visible = false;
                }
            if (imgCopyTemplate != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopyTemplate.CommandName)) imgCopyTemplate.Visible = false;
            }
        }
    }

    protected void gvRiskAssessmentCargo_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            
            RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentCargoID");
            RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
            RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
            RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");

            if (gce.CommandName.ToUpper().Equals("EDITROW"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                Response.Redirect("../Inspection/InspectionRACargoExtn.aspx?genericid=" + lbl.Text + "&status=" + lblstatus.Text, false);
            }

            //if (gce.CommandName.ToUpper().Equals("APPROVE"))
            //{
            //    BindPageURL(gce.Item.ItemIndex);
            //    SetRowSelection();
            //    if (lblInstallcode != null && lblInstallcode.Text == "0")
            //    {                    
            //        if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) == 0)
            //        {
            //            PhoenixInspectionRiskAssessmentCargoExtn.MainFleetApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            //                new Guid(lbl.Text));
            //            ucStatus.Text = "Approved Successfully";
            //        }
            //    }
            //}
            if (gce.CommandName.ToUpper().Equals("ISSUE"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();                		
            }
            if (gce.CommandName.ToUpper().Equals("REVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                ViewState["GENERICID"] = lbl.Text;

                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA?", "ConfirmRevision", 320, 150, null, "ConfirmRevision");
            }

            if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("RAGENERIC"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            Rebind();
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
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentCargoExtn.ApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    ucStatus.Text = "Approved Successfully";
                    Rebind();
                }
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
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentCargoExtn.IssueCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    ucStatus.Text = "Issued Successfully";
                    Rebind();
                }
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
        Rebind();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentCargoID = (RadLabel)gvRiskAssessmentCargo.Items[rowindex].FindControl("lblRiskAssessmentCargoID");
            if (lblRiskAssessmentCargoID != null)
            {
                Filter.CurrentSelectedCargoRA = lblRiskAssessmentCargoID.Text;
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
        gvRiskAssessmentCargo.SelectedIndexes.Clear();
        for (int i = 0; i < gvRiskAssessmentCargo.Items.Count; i++)
        {
            if (gvRiskAssessmentCargo.MasterTableView.Items[i].GetDataKeyValue("FLDRISKASSESSMENTCARGOID").ToString().Equals(Filter.CurrentSelectedCargoRA.ToString()))
            {
                gvRiskAssessmentCargo.MasterTableView.Items[i].Selected = true;
            }            
        }
    }

    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
                if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentCargoExtn.ReviseCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
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
}
