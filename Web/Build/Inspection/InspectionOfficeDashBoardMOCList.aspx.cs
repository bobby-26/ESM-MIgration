using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Inspection_InspectionOfficeDashBoardMOCList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionOfficeDashBoardMOCList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMOC')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            if (!IsPostBack)
            {
                InspectionFilter.CurrentMOCDashboardFilter = null;

                ucConfirmRevision.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["STATUS"] = "";
                ViewState["OFFICEYN"] = "";
                ViewState["VESSELID"] = "";

                if (Request.QueryString["OFFICEYN"].ToString() != null)
                    ViewState["OFFICEYN"] = Request.QueryString["OFFICEYN"].ToString();
                if (Request.QueryString["STATUS"].ToString() != null)
                    ViewState["STATUS"] = Request.QueryString["STATUS"].ToString();

                if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["vslid"].ToString();

                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvMOC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            if (ViewState["STATUS"].ToString() == "PSD" || ViewState["STATUS"].ToString() == "EAP" || ViewState["STATUS"].ToString() == "PAL" || ViewState["STATUS"].ToString() == "APD" || ViewState["STATUS"].ToString() == "IMD" || ViewState["STATUS"].ToString() == "IMP")
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionOfficeDashBoardMOCListFilter.aspx?OFFICEYN="+ ViewState["OFFICEYN"].ToString() + "')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
                toolbar.AddFontAwesomeButton("../Inspection/InspectionOfficeDashBoardMOCList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            }
            MenuMOC.MenuList = toolbar.Show();
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

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDVESSELNAME", "FLDMOCTITLE", "FLDMOCDATE", "FLDMOCREFERENCENO", "FLDCATEGORY", "FLDSUBCATEGORY", "FLDNATUREOFCHANGE", "FLDIMPLEMENTATIONDATE", "FLDSTATUS", "FLDREVISIONNUMBER", "FLDNEXTINTERIMDUEDATE", "FLDCOMPLETIONDATE" };
            string[] alCaptions = { "Office/Ship", "Title", "Proposed", "Ref.No", "Category", "Sub-Category", "Nature of change", "Target date", "Status", "Rev.No", "Next Int.Date", "Completion" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            NameValueCollection Dashboardnvc = InspectionFilter.CurrentMOCDashboardFilter;
            DataSet ds = new DataSet();

            if (ViewState["STATUS"].ToString() == "RES" || ViewState["STATUS"].ToString() == "OVD")
            {
                ds = PhoenixInspectionOfficeDashboard.DashBoardMOCRequestSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null :(Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                      , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                      , General.GetNullableString(ViewState["STATUS"].ToString())
                      , General.GetNullableInteger(ViewState["OFFICEYN"].ToString())
                      , sortexpression
                      , sortdirection
                      , (int)ViewState["PAGENUMBER"]
                      , iRowCount
                      , ref iRowCount
                      , ref iTotalPageCount
                      , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            }
            else if (ViewState["STATUS"].ToString() == "ERPA")
            {
                ds = PhoenixInspectionOfficeDashboard.DashBoardMOCExtentionSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                      , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                      , General.GetNullableInteger(ViewState["OFFICEYN"].ToString())
                      , sortexpression
                      , sortdirection
                      , (int)ViewState["PAGENUMBER"]
                      , iRowCount
                      , ref iRowCount
                      , ref iTotalPageCount
                      ,PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            }
            else if (ViewState["STATUS"].ToString() == "IVO")
            {
                ds = PhoenixInspectionOfficeDashboard.DashBoardMOCIntVerificationSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                     , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                     , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                     , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                     , General.GetNullableInteger(ViewState["OFFICEYN"].ToString())
                     , sortexpression
                     , sortdirection
                     , (int)ViewState["PAGENUMBER"]
                     , iRowCount
                     , ref iRowCount
                     , ref iTotalPageCount
                     , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            }
            else
            {
                ds = PhoenixInspectionOfficeDashboard.DashBoardMOCSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                       , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                       , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                       , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                       , General.GetNullableString(ViewState["STATUS"].ToString())
                       , General.GetNullableInteger(ViewState["OFFICEYN"].ToString())
                       , sortexpression
                       , sortdirection
                       , (int)ViewState["PAGENUMBER"]
                       , iRowCount
                       , ref iRowCount
                       , ref iTotalPageCount
                       , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ucreferenceno"] : null)
                       , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["uccategory"] : null)
                       , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucsubcategory"] : null)
                       , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucnatureofchange"] : null)
                       , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucFrom"] : null)
                       , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucTo"] : null)
                       , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            }

            General.ShowExcel("Management Of Change", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOC.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDVESSELNAME", "FLDMOCTITLE", "FLDMOCDATE", "FLDMOCREFERENCENO", "FLDCATEGORY", "FLDSUBCATEGORY", "FLDNATUREOFCHANGE", "FLDIMPLEMENTATIONDATE", "FLDSTATUS", "FLDREVISIONNUMBER", "FLDNEXTINTERIMDUEDATE", "FLDCOMPLETIONDATE" };
            string[] alCaptions = { "Office/Ship", "Title", "Proposed", "Ref.No", "Category", "Sub-Category", "Nature of change", "Target date", "Status", "Rev.No", "Next Int.Date", "Completion" };

            NameValueCollection Dashboardnvc = InspectionFilter.CurrentMOCDashboardFilter;

            if (ViewState["STATUS"].ToString() == "RES" || ViewState["STATUS"].ToString() == "OVD")
            {
                ds = PhoenixInspectionOfficeDashboard.DashBoardMOCRequestSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                      , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                      , General.GetNullableString(ViewState["STATUS"].ToString())
                      , General.GetNullableInteger(ViewState["OFFICEYN"].ToString())
                      , sortexpression
                      , sortdirection
                      , (int)ViewState["PAGENUMBER"]
                      , gvMOC.PageSize
                      , ref iRowCount
                      , ref iTotalPageCount
                      , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            }
            else if (ViewState["STATUS"].ToString() == "ERPA")
            {
                ds = PhoenixInspectionOfficeDashboard.DashBoardMOCExtentionSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                      , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                      , General.GetNullableInteger(ViewState["OFFICEYN"].ToString())
                      , sortexpression
                      , sortdirection
                      , (int)ViewState["PAGENUMBER"]
                      , gvMOC.PageSize
                      , ref iRowCount
                      , ref iTotalPageCount
                      , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            }
            else if (ViewState["STATUS"].ToString() == "IVO")
            {
                ds = PhoenixInspectionOfficeDashboard.DashBoardMOCIntVerificationSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                     , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                     , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                     , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                     , General.GetNullableInteger(ViewState["OFFICEYN"].ToString())
                     , sortexpression
                     , sortdirection
                     , (int)ViewState["PAGENUMBER"]
                     , gvMOC.PageSize
                     , ref iRowCount
                     , ref iTotalPageCount
                     , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            }
            else
            {
                ds = PhoenixInspectionOfficeDashboard.DashBoardMOCSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                  , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                  , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                  , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                  , General.GetNullableString(ViewState["STATUS"].ToString())
                  , General.GetNullableInteger(ViewState["OFFICEYN"].ToString())
                  , sortexpression
                  , sortdirection
                  , (int)ViewState["PAGENUMBER"]
                  , gvMOC.PageSize
                  , ref iRowCount
                  , ref iTotalPageCount
                  , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ucreferenceno"] : null)
                  , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["uccategory"] : null)
                  , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucsubcategory"] : null)
                  , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucnatureofchange"] : null)
                  , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucFrom"] : null)
                  , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucTo"] : null)
                  , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            }

            General.SetPrintOptions("gvMOC", "Management Of Change", alCaptions, alColumns, ds);
            gvMOC.DataSource = ds;

            gvMOC.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOC_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {

    }

    protected void gvMOC_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvMOC_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDITREQUESTEXTENSION"))
            {
                Response.Redirect(Session["sitepath"] + "/Inspection/InspectionMOCRequestAdd.aspx?MOCID=" + ((RadLabel)e.Item.FindControl("lblmocid")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text, false);
            }
            else if (e.CommandName.ToUpper().Equals("REVISION"))
            {
                ViewState["MOCID"] = ((RadLabel)e.Item.FindControl("lblmocid")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to revise this MOC?", "ConfirmRevision", 320, 150, null, "Revision");
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCTemplate.MOCDelete(new Guid(((RadLabel)e.Item.FindControl("lblmocid")).Text));
                BindData();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOC_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                RadLabel lblmocintermediateverificationid = ((RadLabel)e.Item.FindControl("lblmocintermediateverificationid"));
                LinkButton cmdintverificationEdit = (LinkButton)e.Item.FindControl("cmdintverificationEdit");

                RadLabel lblTitle = (RadLabel)e.Item.FindControl("lblTitle");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ChangeCategory");
                if (uct != null)
                {
                    lblTitle.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lblTitle.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
                RadLabel lblChangeCategory = (RadLabel)e.Item.FindControl("lblChangeCategory");
                UserControlToolTip uct1 = (UserControlToolTip)e.Item.FindControl("Category");
                if (uct1 != null)
                {
                    lblChangeCategory.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct1.ToolTip + "', 'visible');");
                    lblChangeCategory.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct1.ToolTip + "', 'hidden');");
                }

                RadLabel lblResponsePerson = (RadLabel)e.Item.FindControl("lblResponsePerson");
                UserControlToolTip uctResponsePerson = (UserControlToolTip)e.Item.FindControl("ResponsePerson");
                if (uctResponsePerson != null)
                {
                    lblResponsePerson.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctResponsePerson.ToolTip + "', 'visible');");
                    lblResponsePerson.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctResponsePerson.ToolTip + "', 'hidden');");
                }

                RadLabel lblProposerName = (RadLabel)e.Item.FindControl("lblProposerName");
                UserControlToolTip uctProposerName = (UserControlToolTip)e.Item.FindControl("ProposerName");
                if (uctProposerName != null)
                {
                    lblProposerName.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctProposerName.ToolTip + "', 'visible');");
                    lblProposerName.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctProposerName.ToolTip + "', 'hidden');");
                }

                RadLabel lblmoctitle = (RadLabel)e.Item.FindControl("lblmoctitle");
                UserControlToolTip uctlblmoctitle = (UserControlToolTip)e.Item.FindControl("moctitle");
                if (uctlblmoctitle != null)
                {
                    lblmoctitle.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctlblmoctitle.ToolTip + "', 'visible');");
                    lblmoctitle.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctlblmoctitle.ToolTip + "', 'hidden');");
                }

                LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
                RadLabel MOCid = (RadLabel)e.Item.FindControl("lblmocid");
                RadLabel Vesselid = (RadLabel)e.Item.FindControl("lblvesselid");

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString().Equals("0"))
                    imgCopyTemplate.Visible = true;
                else
                    imgCopyTemplate.Visible = false;

                if (imgCopyTemplate != null && MOCid != null && Vesselid != null)
                    imgCopyTemplate.Attributes.Add("onclick", "openNewWindow('MoreInfo', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCCopyTemplate.aspx?MOCID=" + MOCid.Text + "&VESSELID=" + Vesselid.Text + "'); return true;");

                RadLabel lblStatusId = (RadLabel)e.Item.FindControl("lblStatusId");
                LinkButton approve = (LinkButton)e.Item.FindControl("imgApproval");
                LinkButton Revision = (LinkButton)e.Item.FindControl("imgrevision");
                LinkButton ViewRevision = (LinkButton)e.Item.FindControl("cmdViewRevisions");
                RadLabel lblMocid = (RadLabel)e.Item.FindControl("lblmocid");

                if (lblStatusId.Text == "3")
                    approve.Visible = true;

                if (lblStatusId.Text == "4" || lblStatusId.Text == "6" || lblStatusId.Text == "7" || lblStatusId.Text == "9")
                {
                    Revision.Visible = true;
                }
                if (ViewRevision != null)
                    ViewRevision.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCRevisionHistory.aspx?MOCID=" + lblMocid.Text + "'); return true;");

                if (lblmocintermediateverificationid.Text == "" || lblmocintermediateverificationid.Text == null)
                {
                    cmdintverificationEdit.Visible = false;
                }
                else
                {
                    cmdintverificationEdit.Visible = true;
                }

                LinkButton lnkRefNumber = (LinkButton)e.Item.FindControl("lnkRefNumber");
                if (lnkRefNumber != null)
                {
                    lnkRefNumber.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCRequestAdd.aspx?MOCID=" + lblMocid.Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text + "'); return true;");
                }

                LinkButton cmdActionPlanEdit = (LinkButton)e.Item.FindControl("cmdActionPlanEdit");
                if (cmdActionPlanEdit != null)
                {
                    cmdActionPlanEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCRequestActionPlan.aspx?MOCID=" + lblMocid.Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text + "'); return true;");
                }

                LinkButton imgApproval = (LinkButton)e.Item.FindControl("imgApproval");
                if (imgApproval != null)
                {
                    imgApproval.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCRequestEvalutionApproval.aspx?MOCID=" + lblMocid.Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text + "'); return true;");
                }

                LinkButton cmdintVerificationEdit = (LinkButton)e.Item.FindControl("cmdintVerificationEdit");
                if (cmdintVerificationEdit != null)
                {
                    cmdintVerificationEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCIntermediateVerificationAdd.aspx?MOCID=" + lblMocid.Text + "&MOCRequestid=" + ((RadLabel)e.Item.FindControl("lblmocintermediateverificationid")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text + "'); return true;");
                }

                LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkVessel");
                RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                if (lnkvessel != null)
                {
                    lnkvessel.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblvesselid.Text + "');");
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOC_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvMOC.Rebind();
    }

    protected void MenuMOC_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if(CommandName.ToUpper().Equals("CLEAR"))
        {
            InspectionFilter.CurrentMOCDashboardFilter = null;
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }
    protected void Rebind()
    {
        gvMOC.SelectedIndexes.Clear();
        gvMOC.EditIndexes.Clear();
        gvMOC.DataSource = null;
        gvMOC.Rebind();
    }
    protected void ucConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["MOCID"] != null && ViewState["MOCID"].ToString() != "")
            {
                PhoenixInspectionMOCTemplate.MOCRevisionInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["MOCID"].ToString()));
                ucStatus.Text = "MOC is revised.";
                gvMOC.Rebind();
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