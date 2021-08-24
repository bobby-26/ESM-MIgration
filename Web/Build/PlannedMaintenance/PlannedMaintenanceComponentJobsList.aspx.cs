using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Collections.Specialized;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenanceComponentJobsList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobsList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponentJob')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobsFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobsList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");


        MenuDivComponentJob.AccessRights = this.ViewState;
        MenuDivComponentJob.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            ViewState["ISTREENODECLICK"] = false;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["FLDCOMPONENTNUMBER"] = string.Empty;
            ViewState["COMPONENTNAME"] = string.Empty;
            ViewState["FLDJOBCODE"] = string.Empty;
            ViewState["FLDJOBTITLE"] = string.Empty;
            ViewState["FLDPRIORITY"] = string.Empty;
            ViewState["FLDJOBCATEGORY"] = string.Empty;
            ViewState["FLDDISCIPLINENAME"] = string.Empty;

            ViewState["FLDFREQUENCY"] = string.Empty;
            ViewState["FLDFREQUENCYTYPE"] = string.Empty;
            ViewState["FLDMANUALEXIST"] = string.Empty;
            ViewState["FLDISRAREQUIRED"] = string.Empty;
            ViewState["FLDATTACHMENTREQYN"] = string.Empty;
            ViewState["FLDVERIFICATIONLEVEL"] = string.Empty;
            ViewState["FLDISTEMPLATEMAPPED"] = string.Empty;



            gvComponentJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void MenuDivComponentJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentComponentJobFilter = null;
                ViewState["COMPONENTNAME"] = string.Empty;
                ViewState["FLDCOMPONENTNUMBER"] = string.Empty;
                ViewState["FLDJOBCODE"] = string.Empty;
                ViewState["FLDJOBTITLE"] = string.Empty;
                ViewState["FLDPRIORITY"] = string.Empty;
                ViewState["FLDJOBCATEGORY"] = string.Empty;
                ViewState["FLDDISCIPLINENAME"] = string.Empty;

                ViewState["FLDFREQUENCY"] = string.Empty;
                ViewState["FLDFREQUENCYTYPE"] = string.Empty;
                ViewState["FLDMANUALEXIST"] = string.Empty;
                ViewState["FLDISRAREQUIRED"] = string.Empty;
                ViewState["FLDATTACHMENTREQYN"] = string.Empty;
                ViewState["FLDVERIFICATIONLEVEL"] = string.Empty;
                ViewState["FLDISTEMPLATEMAPPED"] = string.Empty;

                BindData();
                gvComponentJob.Rebind();
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
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDREMAININGFREQUENCY", "FLDJOBLASTDONEDATE", "FLDLASTDONEHOURS", "FLDPLANNINGDUEDATE", "FLDDISCIPLINENAME", "FLDPRIORITY", "FLDISRAREQUIRED", "FLDATTACHMENTREQYN", "FLDTEMPLATENAMES", "FLDVERIFICATIONLEVEL", };
            string[] alCaptions = { "Component.No", "Component Name", "Job Code", "Job Title", "Frequency", "Remaining Frequency", "Last Done date", "Last Done Hour", "Due Date", "Resp Discipline", "Priority", "RA Required", "Attachement RequiredYN", "Template", "Verification Level" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvComponentJob.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            SetFilter();



            NameValueCollection nvc = Filter.CurrentComponentJobFilter;

            DataTable dt = PhoenixPlannedMaintenanceComponentJob.ComponentJobsSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , nvc != null ? nvc.Get("txtCompNumber") : null
                        , nvc != null ? nvc.Get("txtCompName") : null
                        , nvc != null ? nvc.Get("txtJobcode") : null
                       , nvc != null ? nvc.Get("txtJobTitle") : null
                       , General.GetNullableInteger(nvc != null ? nvc["ucDiscipline"] : null)
                        , General.GetNullableInteger(nvc != null ? nvc.Get("txtPriority") : string.Empty)
                        , sortexpression, sortdirection,
                        1,
                        iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount,
                        General.GetNullableInteger(nvc != null ? nvc.Get("chkCancelledjob") : string.Empty),
                       nvc != null ? General.GetNullableString(nvc.Get("txtClasscode")) : null,
                         General.GetNullableInteger(nvc != null ? nvc["txtCategory"] : null)
                         , General.GetNullableInteger(ViewState["FLDFREQUENCY"].ToString())
                         , General.GetNullableInteger(ViewState["FLDFREQUENCYTYPE"].ToString())
                         , General.GetNullableByte(ViewState["FLDMANUALEXIST"].ToString())
                         , General.GetNullableByte(ViewState["FLDISRAREQUIRED"].ToString())
                         , General.GetNullableByte(ViewState["FLDATTACHMENTREQYN"].ToString())
                         , General.GetNullableByte(ViewState["FLDISTEMPLATEMAPPED"].ToString())
                         , null
                         , General.GetNullableInteger(ViewState["FLDVERIFICATIONLEVEL"].ToString())

                        );


            General.ShowExcel("Component-Job", dt, alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentJob_PreRender(object sender, EventArgs e)
    {
        //gvComponentJob.MasterTableView.GetColumn("Activeyn").Visible = false;
        //gvComponentJob.Rebind();
    }
    protected void ddlResponsibility_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlDiscipline = sender as RadComboBox;
        ddlDiscipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        ddlDiscipline.DataTextField = "FLDDISCIPLINENAME";
        ddlDiscipline.DataValueField = "FLDDISCIPLINEID";
        ddlDiscipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    private void SetFilter()
    {

        NameValueCollection nvc = new NameValueCollection();

        if (ViewState["FLDCOMPONENTNUMBER"].ToString() != string.Empty) nvc.Add("txtCompNumber", ViewState["FLDCOMPONENTNUMBER"].ToString());
        if (ViewState["COMPONENTNAME"].ToString() != string.Empty) nvc.Add("txtCompName", ViewState["COMPONENTNAME"].ToString());
        if (ViewState["FLDJOBCODE"].ToString() != string.Empty) nvc.Add("txtJobcode", ViewState["FLDJOBCODE"].ToString());
        if (ViewState["FLDJOBTITLE"].ToString() != string.Empty) nvc.Add("txtJobTitle", ViewState["FLDJOBTITLE"].ToString());
        if (ViewState["FLDPRIORITY"].ToString() != string.Empty) nvc.Add("txtPriority", ViewState["FLDPRIORITY"].ToString());
        if (ViewState["FLDJOBCATEGORY"].ToString() != string.Empty) nvc.Add("txtCategory", ViewState["FLDJOBCATEGORY"].ToString());
        if (ViewState["FLDDISCIPLINENAME"].ToString() != string.Empty) nvc.Add("ucDiscipline", ViewState["FLDDISCIPLINENAME"].ToString());


        Filter.CurrentComponentJobFilter = nvc;

    }

    protected void gvComponentJob_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {

            if (e.CommandName == RadGrid.FilterCommandName)
            {

                NameValueCollection nvc = Filter.CurrentComponentJobFilter;
                ViewState["FLDCOMPONENTNUMBER"] = gvComponentJob.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue.ToString();
                if (ViewState["FLDCOMPONENTNUMBER"] != null)
                {
                    nvc.Set("txtCompNumber", ViewState["FLDCOMPONENTNUMBER"].ToString());
                }
                ViewState["COMPONENTNAME"] = gvComponentJob.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue.ToString();
                if (ViewState["COMPONENTNAME"] != null)
                {
                    nvc.Set("txtCompName", ViewState["COMPONENTNAME"].ToString());
                }
                ViewState["FLDJOBCODE"] = gvComponentJob.MasterTableView.GetColumn("FLDJOBCODE").CurrentFilterValue.ToString();
                if (ViewState["FLDJOBCODE"] != null)
                {
                    nvc.Set("txtJobcode", ViewState["FLDJOBCODE"].ToString());
                }
                ViewState["FLDJOBTITLE"] = gvComponentJob.MasterTableView.GetColumn("FLDJOBTITLE").CurrentFilterValue.ToString();
                if (ViewState["FLDJOBTITLE"] != null)
                {
                    nvc.Set("txtJobTitle", ViewState["FLDJOBTITLE"].ToString());
                }
                ViewState["FLDPRIORITY"] = gvComponentJob.MasterTableView.GetColumn("FLDPRIORITY").CurrentFilterValue.ToString();
                if (ViewState["FLDPRIORITY"] != null)
                {
                    nvc.Set("txtPriority", ViewState["FLDPRIORITY"].ToString());
                }
                ViewState["FLDJOBCATEGORY"] = gvComponentJob.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue.ToString();
                if (ViewState["FLDJOBCATEGORY"] != null)
                {
                    nvc.Set("txtCategory", ViewState["FLDJOBCATEGORY"].ToString());
                }
                ViewState["FLDDISCIPLINENAME"] = gvComponentJob.MasterTableView.GetColumn("FLDDISCIPLINENAME").CurrentFilterValue.ToString();
                if (ViewState["FLDDISCIPLINENAME"] != null)
                {
                    nvc.Set("ucDiscipline", ViewState["FLDDISCIPLINENAME"].ToString());
                }

                string freqfilter = gvComponentJob.MasterTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue.ToString();
                if (freqfilter != "")
                {

                 ViewState["FLDFREQUENCY"] = freqfilter.Split('~')[0];
                 ViewState["FLDFREQUENCYTYPE"] = freqfilter.Split('~')[1];
                }
                ViewState["FLDMANUALEXIST"] = gvComponentJob.MasterTableView.GetColumn("FLDMANUALEXIST").CurrentFilterValue;
                ViewState["FLDISRAREQUIRED"] = gvComponentJob.MasterTableView.GetColumn("FLDISRAREQUIRED").CurrentFilterValue;
                ViewState["FLDATTACHMENTREQYN"] = gvComponentJob.MasterTableView.GetColumn("FLDATTACHMENTREQYN").CurrentFilterValue;
                ViewState["FLDVERIFICATIONLEVEL"] = gvComponentJob.MasterTableView.GetColumn("FLDVERIFICATIONLEVEL").CurrentFilterValue;
                ViewState["FLDISTEMPLATEMAPPED"] = gvComponentJob.MasterTableView.GetColumn("FLDISTEMPLATEMAPPED").CurrentFilterValue;
            }


            if (e.CommandName == "UPDATE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.RowIndex;

                RadLabel CompJobId = (RadLabel)e.Item.FindControl("lblComponentJobId");
                UserControlDiscipline ddlDiscipline = (UserControlDiscipline)e.Item.FindControl("ucDiscipline");
                UserControlDate lastDoneDate = ((UserControlDate)e.Item.FindControl("txtLastDonedate"));
                UserControlDecimal lastDoneHour = (UserControlDecimal)e.Item.FindControl("txtLastDoneHour");

                PhoenixPlannedMaintenanceComponentJob.ComponentJobsUpdate(new Guid(CompJobId.Text)
                                                                        , General.GetNullableInteger(ddlDiscipline.SelectedDiscipline.ToString())
                                                                        , General.GetNullableDateTime(lastDoneDate.Text)
                                                                        , General.GetNullableDecimal(lastDoneHour.Text)
                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                        );
                gvComponentJob.Rebind();
            }

            if (e.CommandName == "DELETE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                string componentjobId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOMPONENTJOBID"].ToString();
                PhoenixPlannedMaintenanceComponentJob.DeleteComponentJob(new Guid(componentjobId));
                gvComponentJob.Rebind();
            }

            if (e.CommandName == "Page")
            {
                GridPagerItem item = (GridPagerItem)e.Item;
            }
            else if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                ShowExcel();
            }
            else if (e.CommandName == "RebindGrid")
            {
                Response.Redirect("PlannedMaintenanceComponentJobsFilter.aspx", false);
            }
            //else if (e.CommandName != "Sort" && e.CommandName != "")
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    compJobID = (RadLabel)item.FindControl("lblComponentJobId");
            //    dtKey = (RadLabel)item.FindControl("lbldtkey");
            //    jobID = (RadLabel)item.FindControl("lblJobID");
            //}

            if (e.CommandName == "RowClick" || e.CommandName == "ExpandCollapse")
            {
                bool lastState = e.Item.Expanded;

                if (e.CommandName == "ExpandCollapse")
                {
                    lastState = !lastState;
                }

                CollapseAllRows();
                e.Item.Expanded = !lastState;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void CollapseAllRows()
    {
        foreach (GridItem item in gvComponentJob.MasterTableView.Items)
        {
            item.Expanded = false;
        }
    }

    protected void gvComponentJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        // ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvComponentJob.CurrentPageIndex + 1;
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDREMAININGFREQUENCY", "FLDJOBLASTDONEDATE", "FLDLASTDONEHOURS", "FLDPLANNINGDUEDATE", "FLDDISCIPLINENAME", "FLDPRIORITY", "FLDISRAREQUIRED", "FLDATTACHMENTREQYN", "FLDTEMPLATENAMES", "FLDVERIFICATIONLEVEL", };
        string[] alCaptions = { "Component.No", "Component Name", "Job Code", "Job Title", "Frequency", "Remaining Frequency", "Last Done date", "Last Done Hour", "Due Date", "Resp Discipline", "Priority", "RA Required", "Attachement RequiredYN", "Template", "Verification Level" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        SetFilter();

        NameValueCollection nvc = Filter.CurrentComponentJobFilter;




        DataTable dt = PhoenixPlannedMaintenanceComponentJob.ComponentJobsSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , nvc != null ? nvc.Get("txtCompNumber") : null
                        , nvc != null ? nvc.Get("txtCompName") : null
                        , nvc != null ? nvc.Get("txtJobcode") : null
                       , nvc != null ? nvc.Get("txtJobTitle") : null
                       , General.GetNullableInteger(nvc != null ? nvc["ucDiscipline"] : null)
                        , General.GetNullableInteger(nvc != null ? nvc.Get("txtPriority") : string.Empty)
                        , sortexpression, sortdirection,
                         gvComponentJob.CurrentPageIndex + 1,
                        gvComponentJob.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount,
                        General.GetNullableInteger(nvc != null ? nvc.Get("chkCancelledjob") : string.Empty),
                       nvc != null ? General.GetNullableString(nvc.Get("txtClasscode")) : null,
                         General.GetNullableInteger(nvc != null ? nvc["txtCategory"] : null)
                        , General.GetNullableInteger(ViewState["FLDFREQUENCY"].ToString())
                         , General.GetNullableInteger(ViewState["FLDFREQUENCYTYPE"].ToString())
                         , General.GetNullableByte(ViewState["FLDMANUALEXIST"].ToString())
                         , General.GetNullableByte(ViewState["FLDISRAREQUIRED"].ToString())
                         , General.GetNullableByte(ViewState["FLDATTACHMENTREQYN"].ToString())
                         , General.GetNullableByte(ViewState["FLDISTEMPLATEMAPPED"].ToString())
                         , null
                         , General.GetNullableInteger(ViewState["FLDVERIFICATIONLEVEL"].ToString())
                        );

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvComponentJob", "Component-Job", alCaptions, alColumns, ds);

        gvComponentJob.DataSource = ds;
        gvComponentJob.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void cblJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void gvComponentJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadLabel compJobID = new RadLabel();
        RadLabel dtKey = new RadLabel();
        RadLabel jobID = new RadLabel();

        if (e.Item.IsInEditMode)
        {
            UserControlDiscipline ddlDiscipline = (UserControlDiscipline)e.Item.FindControl("ucDiscipline");
            string disciplineID = ((RadLabel)e.Item.FindControl("lblDiscipline")).Text;

            if (disciplineID != null)
            {
                ddlDiscipline.SelectedDiscipline = disciplineID;
            }
        }

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            compJobID = (RadLabel)item.FindControl("lblComponentJobId");
            dtKey = (RadLabel)item.FindControl("lbldtkey");
            jobID = (RadLabel)item.FindControl("lblJobID");

            RadLabel OverDue = (RadLabel)e.Item.FindControl("lblOverDueYN");
            string OverDueYN = OverDue.Text;
            if (OverDueYN == "1")
            {
                RadBinaryImage imgFlag = (RadBinaryImage)e.Item.FindControl("imgFlag");
                imgFlag.Visible = true;
                imgFlag.ToolTip = "Overdue";
            }
            //RadImageButton db = (RadImageButton)e.Item.FindControl("cmdDelete");
            //if (db != null)
            //{
            //    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            //    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            //}
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton aubtn = (LinkButton)e.Item.FindControl("btnAudit");

            if (aubtn != null)
            {
                aubtn.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobAuditList.aspx?cjobid=" + compJobID.Text + "'); return false;");
            }
            LinkButton mfbtn = (LinkButton)e.Item.FindControl("btnMainForm");
            if (mfbtn != null)
            {
                mfbtn.Attributes.Add("onclick", "javascript:openNewWindow('MaintenanceForm','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobFormMapList.aspx?componentjobid=" + compJobID.Text + "'); return false;");
            }
            LinkButton atbtn = (LinkButton)e.Item.FindControl("btnAttachment");
            if (atbtn != null)
            {
                atbtn.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dtKey.Text + "&mod=PLANNEDMAINTENANCE&u=n'); return false;");
            }
            LinkButton jdbtn = (LinkButton)e.Item.FindControl("btnJobDesc");
            if (jdbtn != null)
            {
                jdbtn.Attributes.Add("onclick", "return openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?framename=ifMoreInfo&JOBID=" + jobID.Text + "', true);");
            }

            LinkButton temp = (LinkButton)e.Item.FindControl("lblReportingTemplate");
            if (temp != null)
            {
                temp.Visible = SessionUtil.CanAccess(this.ViewState, temp.CommandName);
                if (drv["FLDTEMPLATECOUNT"].ToString() == "0")
                {
                    temp.Enabled = false;
                }
                else
                {
                    temp.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobFormMapList.aspx?componentjobid=" + drv["FLDCOMPONENTJOBID"].ToString() + "'); ");
                }
            }

            LinkButton RA = (LinkButton)e.Item.FindControl("lblRARequired");
            if (RA != null)
            {
                RA.Visible = SessionUtil.CanAccess(this.ViewState, temp.CommandName);
                if (drv["FLDISRAREQUIRED"].ToString() == "0")
                {
                    RA.Enabled = false;
                }
                else
                {
                    RA.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRAHistory.aspx?COMPONENTID=" + drv["FLDCOMPONENTID"].ToString() + "'); ");
                }
            }
            LinkButton lnkAttachmentReqd = (LinkButton)e.Item.FindControl("lblAttachmentRequired");
            if (lnkAttachmentReqd != null)
            {
                if (drv["FLDATTACHMENTREQYN"].ToString() == "1")
                {
                    lnkAttachmentReqd.ToolTip = drv["FLDATTINSTRUCTIONS"].ToString();
                    RadToolTipManager1.TargetControls.Add(lnkAttachmentReqd.ClientID, true);
                }
            }

        }

    }

    protected void gvComponentJob_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void cblFrequencyType_DataBinding(object sender, EventArgs e)
    {
        RadComboBox frequency = sender as RadComboBox;
        frequency.DataSource = PhoenixRegistersHard.ListHard(1, 7);
        frequency.DataTextField = "FLDHARDNAME";
        frequency.DataValueField = "FLDHARDCODE";

        frequency.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        frequency.Items.Add(new RadComboBoxItem("Hours", "-1"));
    }
}
