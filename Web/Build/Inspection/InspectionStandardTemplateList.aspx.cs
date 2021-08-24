using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionStandardTemplateList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionStandardTemplateList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionStandardTemplateList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionStandardTemplateList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

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
            ViewState["Status"] = string.Empty;
            BindType();
            BindStatus();

            ddlRAType.SelectedValue = "ALL";

            ViewState["COMPANYID"] = "";

            if (Filter.CurrentStandardTemplateRAFilter != null)
            {
                NameValueCollection nvcFilter = Filter.CurrentStandardTemplateRAFilter;

                ViewState["Status"] = nvcFilter["ddlStatus"];
                ddlStatus.SelectedValue = ViewState["Status"].ToString();
            }

            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
            }
            if (Request.QueryString["filter"] != null)
            {
                if (Request.QueryString["filter"].ToString() == "0")
                {
                    Filter.CurrentStandardTemplateRAFilter = null;
                }
            }

            gvRiskAssessment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        dt = PhoenixInspectionRiskAssessmentActivity.ListStandardTemplateStatus();
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

        string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDDATE", "FLDAPPROVEDBY" };
        string[] alCaptions = { "Ref Number", "Prepared", "Type", "Activity", "Revision No", "Status", "Approved", "Approved By" };

        NameValueCollection nvc = Filter.CurrentStandardTemplateRAFilter;

        if (Filter.CurrentStandardTemplateRAFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        DataSet ds = PhoenixInspectionRiskAssessmentGeneric.PhoenixInspectionRiskAssessmentStandardTemplateSearch(
                     nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , gvRiskAssessment.CurrentPageIndex + 1
                    , gvRiskAssessment.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateApprovedFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateApprovedTo"]) : null
                    );

        General.SetPrintOptions("gvRiskAssessment", "Standard Template", alCaptions, alColumns, ds);

        gvRiskAssessment.DataSource = ds;
        gvRiskAssessment.VirtualItemCount = iRowCount;

    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string vesselid = "";
            string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDDATE", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Ref Number", "Prepared", "Type", "Activity", "Revision No", "Status", "Approved", "Approved By" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentStandardTemplateRAFilter;

            if (Filter.CurrentStandardTemplateRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataSet ds = PhoenixInspectionRiskAssessmentGeneric.PhoenixInspectionRiskAssessmentStandardTemplateSearch(
                    nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvRiskAssessment.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateApprovedFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateApprovedTo"]) : null
                    );

            General.ShowExcel("Standard Template", ds.Tables[0], alColumns, alCaptions, null, null);

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

        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
            {
                Response.Redirect("../Inspection/InspectionRAGenericTemplateDetails.aspx?status=", false);
            }
            else
            {
                Response.Redirect("../Inspection/InspectionRAGenericTemplateAdd.aspx?status=", false);
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
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateApprovedFrom", ucDateApprovedFrom.Text);
                criteria.Add("ucDateApprovedTo", ucDateApprovedTo.Text);
                Filter.CurrentGenericRAFilter = criteria;

                Response.Redirect("../Inspection/InspectionNonRoutineRAGenericTemplate.aspx", false);
            }
            if (ddlRAType.SelectedValue == "MACH")
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateApprovedFrom", ucDateApprovedFrom.Text);
                criteria.Add("ucDateApprovedTo", ucDateApprovedTo.Text);
                Filter.CurrentMachineryRAFilter = criteria;
                Response.Redirect("../Inspection/InspectionNonRoutineRATemplate.aspx", false);
            }

            if (ddlRAType.SelectedValue == "NAV")
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateApprovedFrom", ucDateApprovedFrom.Text);
                criteria.Add("ucDateApprovedTo", ucDateApprovedTo.Text);

                Filter.CurrentNavigationRAFilter = criteria;
                Response.Redirect("../Inspection/InspectionNonRoutineRANavigationTemplate.aspx", false);
            }

            if (ddlRAType.SelectedValue == "CAR")
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ddlRAType", ddlRAType.SelectedValue);
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateApprovedFrom", ucDateApprovedFrom.Text);
                criteria.Add("ucDateApprovedTo", ucDateApprovedTo.Text);
                Filter.CurrentCargoRAFilter = criteria;
                Response.Redirect("../Inspection/InspectionNonRoutineRACargoTemplate.aspx", false);
            }

            if (ddlRAType.SelectedValue == "ALL")
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ddlRAType", ddlRAType.SelectedValue);
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDateApprovedFrom", ucDateApprovedFrom.Text);
                criteria.Add("ucDateApprovedTo", ucDateApprovedTo.Text);
                Filter.CurrentStandardTemplateRAFilter = criteria;
                ViewState["PAGENUMBER"] = 1;
                //BindData();
                gvRiskAssessment.Rebind();
            }
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentStandardTemplateRAFilter = null;
            txtRefNo.Text = "";
            ucDateApprovedFrom.Text = "";
            ucDateApprovedTo.Text = "";
            ddlStatus.SelectedValue = "Dummy";
            ViewState["PAGENUMBER"] = 1;
            //BindData();
            gvRiskAssessment.Rebind();
        }

        MenuMoreLinks.Visible = false;
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

    protected void gvRiskAssessment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblGenericID = (RadLabel)e.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
                RadLabel lbltypeid = (RadLabel)e.Item.FindControl("lblTypeid");

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

            }
        }
    }

    protected void gvRiskAssessment_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = gce.Item.ItemIndex;

                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");
                RadLabel lblvesselid = (RadLabel)gce.Item.FindControl("lblvesselid");

                if (gce.CommandName.ToUpper().Equals("ACTION"))
                {
                    ViewState["RAID"] = lbl.Text;
                    ViewState["GENERICID"] = lbl.Text;
                    ViewState["STATUS"] = lblstatus.Text;
                    ViewState["INSTALLCODE"] = lblInstallcode.Text;
                    ViewState["TYPEID"] = lbltypeid.Text;
                    BindPageURL(nRow);
                    SetRowSelection();
                      divContextLinks.Attributes.Add("style", "position:absolute;" + txtMouseEvent.Value);
                    PhoenixToolbar MoreLinks = new PhoenixToolbar();
                    if (lbltypeid.Text == "1")
                    {
                        MoreLinks.AddLinkButton("../Inspection/InspectionRAGenericTemplateAdd.aspx?showall=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text, "Edit/View", "EDITROW");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        MoreLinks.AddLinkButton("../Inspection/InspectionRANavigationTemplate.aspx?showall=1&navigationid=" + lbl.Text + "&status=" + lblstatus.Text, "Edit/View", "EDITROW");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        MoreLinks.AddLinkButton("../Inspection/InspectionNonRountineRATemplateAdd.aspx?showall=1&machineryid=" + lbl.Text + "&status=" + lblstatus.Text, "Edit/View", "EDITROW");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        MoreLinks.AddLinkButton("../Inspection/InspectionRACargoTemplateAdd.aspx?showall=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text, "Edit/View", "EDITROW");
                    }
                    if (lblstatus.Text == "1")
                    {
                        MoreLinks.AddButton("Request for Approval", "APPROVE");
                    }
                    if (lblstatus.Text == "4")
                    {
                        if (lbltypeid.Text == "1")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + ViewState["RAID"].ToString() + "&TYPE=1','medium'); return false;", "Approve for Use", "ISSUE");
                        }
                        if (lbltypeid.Text == "2")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + ViewState["RAID"].ToString() + "&TYPE=2','medium'); return false;", "Approve for Use", "ISSUE");
                        }
                        if (lbltypeid.Text == "3")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + ViewState["RAID"].ToString() + "&TYPE=3','medium'); return false;", "Approve for Use", "ISSUE");
                        }
                        if (lbltypeid.Text == "4")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + ViewState["RAID"].ToString() + "&TYPE=5','medium'); return false;", "Approve for Use", "ISSUE");
                        }
                    }
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && lblstatus.Text == "5")
                    {
                        MoreLinks.AddButton("Create Revision", "REVISION");
                    }
                    if (lbltypeid.Text == "1")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGenericRevisionList.aspx?genericid=" + ViewState["RAID"].ToString() + "');return false;", "View Revisions", "VIEWREVISION");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationRevisionList.aspx?navigationid=" + ViewState["RAID"].ToString() + "');return false;", "View Revisions", "VIEWREVISION");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryRevisionList.aspx?machineryid=" + ViewState["RAID"].ToString() + "');return false;", "View Revisions", "VIEWREVISION");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoRevisionList.aspx?genericid=" + ViewState["RAID"].ToString() + "');return false;", "View Revisions", "VIEWREVISION");
                    }
                    if (lblstatus.Text == "5")
                    {
                        if (lbltypeid.Text == "1")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGenericTemplateAdd.aspx?genericid=" + ViewState["RAID"].ToString() + "&CopyType=1&showall=1" + "'); return false;", "Copy as Non Routine RA", "COPYTEMPLATE");
                        }
                        if (lbltypeid.Text == "2")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigationTemplate.aspx?navigationid=" + ViewState["RAID"].ToString() + "&CopyType=1&showall=1" + "'); return false;", "Copy as Non Routine RA", "COPYTEMPLATE");
                        }
                        if (lbltypeid.Text == "3")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionNonRountineRATemplateAdd.aspx?machineryid=" + ViewState["RAID"].ToString() + "&CopyType=1&showall=1" + "'); return false;", "Copy as Non Routine RA", "COPYTEMPLATE");
                        }
                        if (lbltypeid.Text == "4")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRACargoTemplateAdd.aspx?genericid=" + ViewState["RAID"].ToString() + "&CopyType=1&showall=1" + "'); return false;", "Copy as Non Routine RA", "COPYTEMPLATE");
                        }
                    }
                    if ((PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0) && (lblstatus.Text == "5"))
                    {
                        if (lbltypeid.Text == "1")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGenericTemplateAdd.aspx?genericid=" + ViewState["RAID"].ToString() + "&CopyType=2&showall=1" + "'); return false;", "Copy Template", "PROPOSETEMPLATE");
                        }
                        if (lbltypeid.Text == "2")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigationTemplate.aspx?navigationid=" + ViewState["RAID"].ToString() + "&CopyType=2&showall=1" + "'); return false;", "Copy Template", "PROPOSETEMPLATE");
                        }
                        if (lbltypeid.Text == "3")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionNonRountineRATemplateAdd.aspx?machineryid=" + ViewState["RAID"].ToString() + "&CopyType=2&showall=1" + "'); return false;", "Copy Template", "PROPOSETEMPLATE");
                        }
                        if (lbltypeid.Text == "4")
                        {
                            MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRACargoTemplateAdd.aspx?genericid=" + ViewState["RAID"].ToString() + "&CopyType=2&showall=1" + "'); return false;", "Copy Template", "PROPOSETEMPLATE");
                        }
                    }
                    if (lbltypeid.Text == "1")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + ViewState["RAID"].ToString() + "&showmenu=0&showexcel=NO');return false;", "Show PDF", "REPORT");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + ViewState["RAID"].ToString() + "&showmenu=0&showexcel=NO');return false;", "Show PDF", "REPORT");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + ViewState["RAID"].ToString() + "&showmenu=0&showexcel=NO');return false;", "Show PDF", "REPORT");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + ViewState["RAID"].ToString() + "&showmenu=0&showexcel=NO');return false;", "Show PDF", "REPORT");
                    }
                    MenuMoreLinks.AccessRights = this.ViewState;
                    MenuMoreLinks.MenuList = MoreLinks.Show();
                    MenuMoreLinks.Visible = true;
                }


                if (gce.CommandName.ToUpper().Equals("APPROVE"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    if (lblInstallcode != null && lblInstallcode.Text == "0" && lblvesselid.Text == "0")
                    {
                        //if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) > 0)
                        //{
                        //    ViewState["GENERICID"] = lbl.Text;
                        //    ucConfirmApprove.Visible = true;
                        //    ucConfirmApprove.Text = "Once the RA is approved, It cannot be edited. Are you sure to continue?";
                        //    return;
                        //}
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
                if (gce.CommandName.ToUpper().Equals("ISSUE"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    //ViewState["GENERICID"] = lbl.Text;
                    //ucConfirmIssue.Text = "Once the RA is issued, It cannot be edited. Are you sure to continue?";
                    // RadWindowManager1.RadConfirm("Once the RA is issued, It cannot be edited. Are you sure to continue?", "ConfirmIssue", 320, 150, null, "Revision");
		
                }
                if (gce.CommandName.ToUpper().Equals("REVISION"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                    ViewState["GENERICID"] = lbl.Text;
                    ViewState["TYPE"] = lbltypeid.Text;
                    RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.?", "ConfirmRevision", 320, 150, null, "Revision");
                }
                if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                }
                if (gce.CommandName.ToUpper().Equals("RAGENERIC"))
                {
                    BindPageURL(nRow);
                    SetRowSelection();
                }
                else if (gce.CommandName == "Page")
                {
                    ViewState["PAGENUMBER"] = null;
                }
                //BindData();
                gvRiskAssessment.Rebind();
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
                PhoenixInspectionRiskAssessmentGeneric.ApproveGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                ucStatus.Text = "Approved Successfully";
                gvRiskAssessment.Rebind();
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
                PhoenixInspectionRiskAssessmentGeneric.IssueGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                ucStatus.Text = "Issued Successfully";
                gvRiskAssessment.Rebind();
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
        //gvRiskAssessment.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessment.Rows.Count; i++)
        //{
        //    if (gvRiskAssessment.DataKeys[i].Value.ToString().Equals(Filter.CurrentSelectedGenericRA.ToString()))
        //    {
        //        gvRiskAssessment.SelectedIndex = i;
        //    }
        //}
    }

    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
            {
                if (ViewState["TYPEID"].ToString() == "1")
                {
                    PhoenixInspectionRiskAssessmentGeneric.ReviseGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPEID"].ToString() == "2")
                {
                    PhoenixInspectionRiskAssessmentNavigation.ReviseNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPEID"].ToString() == "3")
                {
                    PhoenixInspectionRiskAssessmentMachinery.ReviseMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPEID"].ToString() == "4")
                {
                    PhoenixInspectionRiskAssessmentCargo.ReviseCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                ucStatus.Text = "RA is revised.";
                gvRiskAssessment.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuMoreLinks_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        string CommandName = dce.CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                MenuMoreLinks.Visible = false;
            }

            if (CommandName.ToUpper().Equals("REVISION"))
            {
                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.?", "ConfirmRevision", 320, 150, null, "Revision");
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (ViewState["INSTALLCODE"] != null && ViewState["INSTALLCODE"].ToString() == "0")
                {
                    if (ViewState["TYPEID"].ToString() == "1")
                    {
                        PhoenixInspectionRiskAssessmentGeneric.MainFleetApproveGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       new Guid(ViewState["RAID"].ToString()));
                        ucStatus.Text = "Approved Successfully";
                        gvRiskAssessment.Rebind();
                    }
                    if (ViewState["TYPEID"].ToString() == "2")
                    {
                        PhoenixInspectionRiskAssessmentNavigation.MainFleetApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       new Guid(ViewState["RAID"].ToString()));
                        ucStatus.Text = "Approved Successfully";
                        gvRiskAssessment.Rebind();
                    }
                    if (ViewState["TYPEID"].ToString() == "3")
                    {
                        PhoenixInspectionRiskAssessmentMachinery.MainFleetApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       new Guid(ViewState["RAID"].ToString()));
                        ucStatus.Text = "Approved Successfully";
                        gvRiskAssessment.Rebind();
                    }
                    if (ViewState["TYPEID"].ToString() == "4")
                    {
                        PhoenixInspectionRiskAssessmentCargo.MainFleetApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       new Guid(ViewState["RAID"].ToString()));
                        ucStatus.Text = "Approved Successfully";
                        gvRiskAssessment.Rebind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
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
