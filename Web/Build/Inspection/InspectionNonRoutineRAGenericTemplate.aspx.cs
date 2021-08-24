using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionNonRoutineRAGenericTemplate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRAGenericTemplate.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentGeneric')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRAGenericTemplate.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRAGenericTemplate.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRAGenericTemplate.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        }
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
            ddlRAType.SelectedValue = "GEN";

            ViewState["COMPANYID"] = "";
            if (Filter.CurrentGenericRAFilter != null)
            {
                NameValueCollection nvcFilter = Filter.CurrentGenericRAFilter;

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
                    Filter.CurrentGenericRAFilter = null;
                }
            }
            gvRiskAssessmentGeneric.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindData();
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

        NameValueCollection nvc = Filter.CurrentGenericRAFilter;

        if (Filter.CurrentGenericRAFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        DataSet ds = PhoenixInspectionRiskAssessmentGeneric.PhoenixInspectionRiskAssessmentGenericTemplateSearch(
                      General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                    , gvRiskAssessmentGeneric.CurrentPageIndex + 1
                    , gvRiskAssessmentGeneric.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateApprovedFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateApprovedTo"]) : null
                    );

        General.SetPrintOptions("gvRiskAssessmentGeneric", "Generic", alCaptions, alColumns, ds);

        gvRiskAssessmentGeneric.DataSource = ds;
        gvRiskAssessmentGeneric.VirtualItemCount = iRowCount;
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

            NameValueCollection nvc = Filter.CurrentGenericRAFilter;

            if (Filter.CurrentGenericRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataSet ds = PhoenixInspectionRiskAssessmentGeneric.PhoenixInspectionRiskAssessmentGenericTemplateSearch(
                      General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvRiskAssessmentGeneric.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateApprovedFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateApprovedTo"]) : null
                    );

            General.ShowExcel("Generic", ds.Tables[0], alColumns, alCaptions, null, null);

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

                ViewState["PAGENUMBER"] = 1;
                //BindData();
                gvRiskAssessmentGeneric.Rebind();
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
                Response.Redirect("../Inspection/InspectionNonRoutineRATemplate.aspx", true);
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
                Response.Redirect("../Inspection/InspectionNonRoutineRANavigationTemplate.aspx", true);
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
                Response.Redirect("../Inspection/InspectionNonRoutineRACargoTemplate.aspx", true);
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
                Response.Redirect("../Inspection/InspectionStandardTemplateList.aspx", true);
            }
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentGenericRAFilter = null;
            txtRefNo.Text = "";
            ucDateApprovedFrom.Text = "";
            ucDateApprovedTo.Text = "";
            ddlStatus.SelectedValue = "Dummy";
            ViewState["PAGENUMBER"] = 1;
            //BindData();
            gvRiskAssessmentGeneric.Rebind();
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

    protected void gvRiskAssessmentGeneric_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblGenericID = (RadLabel)e.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");

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

    protected void gvRiskAssessmentGeneric_ItemCommand(object sender, GridCommandEventArgs gce)
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
                UserControlMoreLinks ucaction = (UserControlMoreLinks)gce.Item.FindControl("ucaction");
                RadLabel lblVesselid = (RadLabel)gce.Item.FindControl("lblVesselid");

                if (gce.CommandName.ToUpper().Equals("ACTION"))
                {

                    ViewState["GENERICID"] = lbl.Text;
                    ViewState["STATUS"] = lblstatus.Text;
                    ViewState["INSTALLCODE"] = lblInstallcode.Text;
                    BindPageURL(nRow);
                    SetRowSelection();
                     divContextLinks.Attributes.Add("style", "position:absolute;" + txtMouseEvent.Value);
                    PhoenixToolbar MoreLinks = new PhoenixToolbar();
                    MoreLinks.AddLinkButton("../Inspection/InspectionRAGenericTemplateAdd.aspx?genericid=" + ViewState["GENERICID"].ToString() + "&status=" + ViewState["STATUS"].ToString(), "Edit/View", "EDITROW");
                    if (lblstatus.Text == "1")
                    {
                        MoreLinks.AddButton("Request for Approval", "APPROVE");
                    }
                    if (lblstatus.Text == "4")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + ViewState["GENERICID"].ToString() + "&TYPE=1','medium'); return false;", "Approve for Use", "ISSUE");
                    }
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && lblstatus.Text == "5")
                    {
                        MoreLinks.AddButton("Create Revision", "REVISION");
                    }
                    MoreLinks.AddLinkButton("javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGenericRevisionList.aspx?genericid=" + ViewState["GENERICID"].ToString() + "');return false;", "View Revisions", "VIEWREVISION");
                    if (lblstatus.Text == "5")
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGenericTemplateAdd.aspx?genericid=" + ViewState["GENERICID"].ToString() + "&CopyType=1" + "'); return false;", "Copy as Non Routine RA", "COPYTEMPLATE");
                    }
                    if ((PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0) && (lblstatus.Text == "5"))
                    {
                        MoreLinks.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGenericTemplateAdd.aspx?genericid=" + ViewState["GENERICID"].ToString() + "&CopyType=2" + "'); return false;", "Copy Template", "PROPOSETEMPLATE");
                    }
                    MoreLinks.AddLinkButton("javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + ViewState["GENERICID"].ToString() + "&showmenu=0&showexcel=NO');return false;", "Show PDF", "REPORT");
                    MenuMoreLinks.AccessRights = this.ViewState;
                    MenuMoreLinks.MenuList = MoreLinks.Show();
                    MenuMoreLinks.Visible = true;
                }
                else if(gce.CommandName =="Page")
                {
                    ViewState["PAGENUMBER"] = null;
                }
                gvRiskAssessmentGeneric.Rebind();
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
                gvRiskAssessmentGeneric.Rebind();
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
                gvRiskAssessmentGeneric.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnaction_Click(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CONFIRM"))
            {

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
        gvRiskAssessmentGeneric.Rebind();
        MenuMoreLinks.Visible = false;
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentGenericID = (RadLabel)gvRiskAssessmentGeneric.Items[rowindex].FindControl("lblRiskAssessmentGenericID");
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
        //gvRiskAssessmentGeneric.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessmentGeneric.Rows.Count; i++)
        //{
        //    if (gvRiskAssessmentGeneric.DataKeys[i].Value.ToString().Equals(Filter.CurrentSelectedGenericRA.ToString()))
        //    {
        //        gvRiskAssessmentGeneric.SelectedIndex = i;
        //    }
        //}
    }

    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentGeneric.ReviseGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(ViewState["GENERICID"].ToString()));
                ucStatus.Text = "RA is revised.";
                gvRiskAssessmentGeneric.Rebind();
                MenuMoreLinks.Visible = false;
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

        try
        {
            if (dce.CommandName.ToUpper().Equals("CLOSE"))
            {
                MenuMoreLinks.Visible = false;
            }

            if (dce.CommandName.ToUpper().Equals("REVISION"))
            {
                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.?", "ConfirmRevision", 320, 150, null, "Revision");
            }
            if (dce.CommandName.ToUpper().Equals("APPROVE"))
            {
                if (ViewState["INSTALLCODE"] != null && ViewState["INSTALLCODE"].ToString() == "0")
                {
                    PhoenixInspectionRiskAssessmentGeneric.MainFleetApproveGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    ucStatus.Text = "Approved Successfully";
                    gvRiskAssessmentGeneric.Rebind();
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

    protected void gvRiskAssessmentGeneric_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentGeneric.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
