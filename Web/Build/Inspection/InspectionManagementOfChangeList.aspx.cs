using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web.UI;

public partial class InspectionManagementOfChangeList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionManagementOfChangeList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMOC')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCAdd.aspx", "Add New", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            //toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionMOCFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionManagementOfChangeList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuMOC.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucConfirmRevision.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["txtRefNo"] = string.Empty;
                ViewState["VesselID"] = string.Empty;
                ViewState["ddlStatus"] = string.Empty;
                ViewState["txtTitle"] = string.Empty;
                ViewState["ddlCategory"] = string.Empty;
                ViewState["ddlNChange"] = string.Empty;
                ViewState["ddlSubCategory"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;

                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvMOC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                if (InspectionFilter.MOCFilterCriteria == null)
                {
                    SetFilter();
                }

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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string Vesselid = "";
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

            NameValueCollection nvc = InspectionFilter.MOCFilterCriteria;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                Vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            if (nvc != null && General.GetNullableInteger(nvc["ucVessel"]) == null)
            {
                nvc["ucVessel"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            DataSet ds = PhoenixInspectionMOCTemplate.MOCSearch(
                   nvc.Get("ddlStatus") != null ? General.GetNullableInteger(nvc["ddlStatus"]) : General.GetNullableInteger(ViewState["ddlStatus"].ToString())
                   , nvc.Get("ucVessel") != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(ViewState["VesselID"].ToString())
                   , sortexpression
                   , sortdirection
                   , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                   , General.ShowRecords(null)
                   , ref iRowCount
                   , ref iTotalPageCount
                   , nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                   , nvc.Get("txtTitle") != null ? General.GetNullableString(nvc["txtTitle"].ToString()) : General.GetNullableString(ViewState["txtTitle"].ToString())
                   , nvc.Get("ddlCategory") != null ? General.GetNullableGuid(nvc["ddlCategory"].ToString()) : General.GetNullableGuid(ViewState["ddlCategory"].ToString())
                   , nvc.Get("ddlSubCategory") != null ? General.GetNullableGuid(nvc["ddlSubCategory"].ToString()) : General.GetNullableGuid(ViewState["ddlSubCategory"].ToString())
                   , nvc.Get("ddlNChange") != null ? General.GetNullableInteger(nvc["ddlNChange"]) : General.GetNullableInteger(ViewState["ddlNChange"].ToString())
                   , nvc.Get("FDATE") != null ? General.GetNullableDateTime(nvc["FDATE"]) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
                   , nvc.Get("TDATE") != null ? General.GetNullableDateTime(nvc["TDATE"]) : General.GetNullableDateTime(ViewState["TDATE"].ToString()));

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
            SetFilter();
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

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDVESSELNAME", "FLDMOCTITLE", "FLDMOCDATE", "FLDMOCREFERENCENO", "FLDCATEGORY", "FLDSUBCATEGORY", "FLDNATUREOFCHANGE", "FLDIMPLEMENTATIONDATE", "FLDSTATUS", "FLDREVISIONNUMBER", "FLDNEXTINTERIMDUEDATE", "FLDCOMPLETIONDATE" };
            string[] alCaptions = { "Office/Ship", "Title", "Proposed", "Ref.No", "Category", "Sub-Category", "Nature of change", "Target date", "Status", "Rev.No", "Next Int.Date", "Completion" };

            NameValueCollection nvc = InspectionFilter.MOCFilterCriteria;

            DataSet ds = PhoenixInspectionMOCTemplate.MOCSearch(
                   nvc.Get("ddlStatus") != null ? General.GetNullableInteger(nvc["ddlStatus"]) : General.GetNullableInteger(ViewState["ddlStatus"].ToString())
                   , nvc.Get("ucVessel") != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(ViewState["VesselID"].ToString())
                   , sortexpression
                   , sortdirection
                   , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                   , General.ShowRecords(null)
                   , ref iRowCount
                   , ref iTotalPageCount
                   , nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                   , nvc.Get("txtTitle") != null ? General.GetNullableString(nvc["txtTitle"].ToString()) : General.GetNullableString(ViewState["txtTitle"].ToString())
                   , nvc.Get("ddlCategory") != null ? General.GetNullableGuid(nvc["ddlCategory"].ToString()) : General.GetNullableGuid(ViewState["ddlCategory"].ToString())
                   , nvc.Get("ddlSubCategory") != null ? General.GetNullableGuid(nvc["ddlSubCategory"].ToString()) : General.GetNullableGuid(ViewState["ddlSubCategory"].ToString())
                   , nvc.Get("ddlNChange") != null ? General.GetNullableInteger(nvc["ddlNChange"]) : General.GetNullableInteger(ViewState["ddlNChange"].ToString())
                   , nvc.Get("FDATE") != null ? General.GetNullableDateTime(nvc["FDATE"]) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
                   , nvc.Get("TDATE") != null ? General.GetNullableDateTime(nvc["TDATE"]) : General.GetNullableDateTime(ViewState["TDATE"].ToString()));

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
            if (e.CommandName.ToUpper().Equals("EDITACTIONPLAN"))
            {
                Response.Redirect(Session["sitepath"] + "/Inspection/InspectionMOCRequestActionPlan.aspx?MOCID=" + ((RadLabel)e.Item.FindControl("lblmocid")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text, false);
            }
            else if (e.CommandName.ToUpper().Equals("EDITAPPROVALOFMOC"))
            {
                LinkButton imgApproval = (LinkButton)e.Item.FindControl("imgApproval");
                if (imgApproval != null)
                {
                    Response.Redirect(Session["sitepath"] + "/Inspection/InspectionMOCRequestEvalutionApproval.aspx?MOCID=" + ((RadLabel)e.Item.FindControl("lblmocid")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text, false);
                }
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Response.Redirect(Session["sitepath"] + "/Inspection/InspectionMOCRequestAdd.aspx?MOCID=" + ((RadLabel)e.Item.FindControl("lblmocid")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text, false);
            }
            else if (e.CommandName.ToUpper().Equals("EDITREQUESTEXTENSION"))
            {
                Response.Redirect(Session["sitepath"] + "/Inspection/InspectionMOCRequestAdd.aspx?MOCID=" + ((RadLabel)e.Item.FindControl("lblmocid")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text, false);
            }
            else if (e.CommandName.ToUpper().Equals("EDITINTVERIFICATION"))
            {
                Response.Redirect(Session["sitepath"] + "/Inspection/InspectionMOCIntermediateVerificationAdd.aspx?MOCID=" + ((RadLabel)e.Item.FindControl("lblmocid")).Text + "&MOCRequestid=" + ((RadLabel)e.Item.FindControl("lblmocintermediateverificationid")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text, false);
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
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)e.CommandArgument;
                ViewState["PAGENUMBER"] = 1;

                string daterange = gvMOC.MasterTableView.GetColumn("FLDIMPLEMENTATIONDATE").CurrentFilterValue.ToString();
                if (daterange != string.Empty)
                {
                    ViewState["FDATE"] = daterange.Split('~')[0];
                    ViewState["TDATE"] = daterange.Split('~')[1];
                }

                ViewState["txtRefNo"] = gvMOC.MasterTableView.GetColumn("FLDMOCREFERENCENO").CurrentFilterValue;
                ViewState["txtTitle"] = gvMOC.MasterTableView.GetColumn("FLDMOCTITLE").CurrentFilterValue;
                SetFilter();
                gvMOC.Rebind();
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
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["txtRefNo"] = string.Empty;
            ViewState["VesselID"] = string.Empty;
            ViewState["ddlStatus"] = string.Empty;
            ViewState["txtTitle"] = string.Empty;
            ViewState["ddlCategory"] = string.Empty;
            ViewState["ddlNChange"] = string.Empty;
            ViewState["ddlSubCategory"] = string.Empty;
            ViewState["FDATE"] = string.Empty;
            ViewState["TDATE"] = string.Empty;

            ViewState["PAGENUMBER"] = 1;

            InspectionFilter.MOCFilterCriteria = null;
            SetFilter();
            BindData();
            gvMOC.Rebind();
        }
    }

    private void SetFilter()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        NameValueCollection criteria = new NameValueCollection();

        criteria.Add("txtRefNo", ViewState["txtRefNo"].ToString());
        criteria.Add("ucVessel", ViewState["VesselID"].ToString());
        criteria.Add("ddlStatus", ViewState["ddlStatus"].ToString());
        criteria.Add("txtTitle", ViewState["txtTitle"].ToString());
        criteria.Add("ddlCategory", ViewState["ddlCategory"].ToString());
        criteria.Add("ddlNChange", ViewState["ddlNChange"].ToString());
        criteria.Add("ddlSubCategory", ViewState["ddlSubCategory"].ToString());
        criteria.Add("FDATE", ViewState["FDATE"].ToString());
        criteria.Add("TDATE", ViewState["TDATE"].ToString());

        InspectionFilter.MOCFilterCriteria = criteria;
    }

    protected void ucVessel_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDVESSELID").CurrentFilterValue = e.Value;
        ViewState["VesselID"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        gvMOC.Rebind();
    }

    protected void ucVessel_DataBinding(object sender, EventArgs e)
    {
        DataSet dt = new DataSet();
        dt = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(null), General.GetNullableInteger(ViewState["COMPANYID"].ToString()), General.GetNullableInteger(null), 0);
        RadComboBox ucVessel = sender as RadComboBox;
        ucVessel.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ucVessel.DataSource = dt;

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Tables[0].Columns["FLDVESSELID"];
        dt.Tables[0].PrimaryKey = keyColumns;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ucVessel.Enabled = false;
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        if (ViewState["VesselID"] != null && ViewState["VesselID"].ToString() != "")
        {
            if (dt.Tables[0].Rows.Contains(ViewState["VesselID"].ToString()))
            {
                ucVessel.SelectedValue = ViewState["VesselID"].ToString();
            }
        }
    }

    protected void ddlSubCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlSubCategory = sender as RadComboBox;

        DataSet ds = PhoenixInspectionMOCCategory.MOCSubCategoryList(General.GetNullableGuid(ViewState["ddlCategory"].ToString()));

        ddlSubCategory.DataSource = ds.Tables[0];
        ddlSubCategory.DataTextField = "FLDMOCSUBCATEGORYNAME";
        ddlSubCategory.DataValueField = "FLDMOCSUBCATEGORYID";
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("All", "Dummy"));
    }

    protected void ddlSubCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDMOCSUBCATEGORYID").CurrentFilterValue = e.Value;
        ViewState["ddlSubCategory"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        gvMOC.Rebind();
    }

    protected void ddlCategory_DataBinding(object sender, EventArgs e)
    {
        DataSet ds = PhoenixInspectionMOCCategory.MOCCategoryList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        RadComboBox ddlCategory = sender as RadComboBox;
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "FLDMOCCATEGORYNAME";
        ddlCategory.DataValueField = "FLDMOCCATEGORYID";
        ddlCategory.Items.Insert(0, new RadComboBoxItem("All", "Dummy"));
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDMOCCATEGORYID").CurrentFilterValue = e.Value;
        ViewState["ddlCategory"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        gvMOC.Rebind();
    }

    protected void ddlNatureofChange_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDNATUREOFCHANGE").CurrentFilterValue = e.Value;
        ViewState["ddlNChange"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        gvMOC.Rebind();
    }

    protected void ddlstatus_DataBinding(object sender, EventArgs e)
    {
        DataSet ds = PhoenixInspectionMOCRequestForChange.MOCStatusList();

        RadComboBox ddlstatus = sender as RadComboBox;
        ddlstatus.DataSource = ds.Tables[0];
        ddlstatus.DataTextField = "FLDSTATUSNAME";
        ddlstatus.DataValueField = "FLDMOCSTATUSID";
        ddlstatus.Items.Insert(0, new RadComboBoxItem("All", "Dummy"));
    }

    protected void ddlstatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDMOCSTATUSID").CurrentFilterValue = e.Value;
        ViewState["ddlStatus"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
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

