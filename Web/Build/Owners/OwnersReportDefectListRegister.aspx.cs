using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using SouthNests.Phoenix.Owners;

public partial class OwnersReportDefectListRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarNoonReporttap = new PhoenixToolbar();
            toolbarNoonReporttap.AddImageButton("../Owners/OwnersReportDefectListRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarNoonReporttap.AddImageLink("javascript:CallPrint('gvDefectList')", "Print Grid", "icon_print.png", "PRINT");
            toolbarNoonReporttap.AddImageButton("../Owners/OwnersReportDefectListRegister.aspx", "Find", "search.png", "FIND");
            toolbarNoonReporttap.AddImageButton("../Owners/OwnersReportDefectListRegister.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbarNoonReporttap.AddFontAwesomeButton("javascript:openNewWindow('chart', 'Chart','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectReportChart.aspx?DEFECT=1');return", "Chart", "<i class=\"far fa-chart-bar\"></i>", "CHART");


            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbarNoonReporttap.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                modalPopup.NavigateUrl = Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectListAdd.aspx";
                gvDefectList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDefectList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            string Defectjobid = drv["FLDDEFECTJOBID"].ToString();

            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton verify = (ImageButton)e.Item.FindControl("cmdverify");
            ImageButton workorder = (ImageButton)e.Item.FindControl("cmdWorkorder");
            ImageButton complete = (ImageButton)e.Item.FindControl("cmdComplete");
            ImageButton postpone = (ImageButton)e.Item.FindControl("cmdPostpone");

            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                //edit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectListUpdate.aspx?defectjobId=" + drv["FLDDEFECTJOBID"] + "',false,700,500); ");
            }
            if (verify != null)
            {
                verify.Visible = SessionUtil.CanAccess(this.ViewState, verify.CommandName);
                //verify.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectJobVerification.aspx?defectjobId=" + drv["FLDDEFECTJOBID"] + "&defectno=" + drv["FLDDEFECTNO"] + "',false,500,300); ");
            }
            if (postpone != null)
            {
                postpone.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '" + drv["FLDDEFECTNO"].ToString() + "', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectReschedule.aspx?dueDate=" + drv["FLDDUEDATE"] + "&defectId=" + drv["FLDDEFECTJOBID"] + "',false,500,400); ");
                postpone.Visible = SessionUtil.CanAccess(this.ViewState, postpone.CommandName);
            }
            if (drv["FLDWORKORDERREQUIRED"].ToString() == "0")
            {
                if (verify != null)
                {
                    verify.Visible = false;
                }
                if (complete != null)
                {
                    complete.Visible = true;
                    complete.Visible = SessionUtil.CanAccess(this.ViewState, complete.CommandName);
                    //complete.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectClose.aspx?DefectId=" + drv["FLDDEFECTJOBID"] + "',false,500,300); ");
                }
            }
            if (drv["FLDWORKORDERREQUIRED"].ToString() == "1")
            {
                if (verify != null)
                {
                    verify.Visible = false;
                }
                if (workorder != null)
                {
                    workorder.Visible = SessionUtil.CanAccess(this.ViewState, workorder.CommandName);
                    //workorder.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWODefectJobDetails.aspx?DefectId=" + drv["FLDDEFECTJOBID"] + "&DefectNo=" + drv["FLDDEFECTNO"] + "&ComponentId=" + drv["FLDCOMPONENTID"] + "&Res=" + drv["FLDRESPONSIBILITYID"] + "&Due=" + drv["FLDDUEDATE"] + "',false,800,500); ");
                }
            }

            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                //del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }


            if (!string.IsNullOrEmpty(drv["FLDDONEBY"].ToString()))
            {
                edit.Visible = false;
                del.Visible = false;
                verify.Visible = false;
                postpone.Visible = false;
                complete.Visible = false;

            }
            LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
            if (Communication != null)
            {
                int vesselid = int.Parse(Filter.SelectedOwnersReportVessel);
                Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                Communication.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','" + drv["FLDDEFECTNO"].ToString() + "','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=DEFECT" + "&Referenceid=" + drv["FLDDEFECTJOBID"] + "&Vesselid=" + vesselid + "');");
            }

            ImageButton cmdAtt = (ImageButton)e.Item.FindControl("cmdAtt");
            if (cmdAtt != null)
            {
                cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','" + drv["FLDDEFECTNO"].ToString() + "','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDEFECTJOBID"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&DocSource=DEFECT'); return false;");
                cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);
                if (drv["FLDATTACHMENTCOUNT"].ToString() == "0") cmdAtt.ImageUrl = Session["images"] + "/no-attachment.png";
            }
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDEFECTNO", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDDETAILS", "FLDDUEDATE", "FLDRESPONSIBILITY", "FLDSTATUSNAME" };
            string[] alCaptions = { "Defect No", "Component No", "Component Name.", "Defect Details", "Due", "Responsibility", "Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;

            ds = PhoenixOwnerReportPMS.OwnersReportDefectJobSearch(General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
                                                               ,General.GetNullableString(txtComponentNumber.Text),
                                                               General.GetNullableString(txtComponentName.Text),
                                                               General.GetNullableInteger(ucDisciplineResponsibility.SelectedDiscipline),
                                                               sortexpression,
                                                               sortdirection,
                                                               1, gvDefectList.VirtualItemCount,
                                                               ref iRowCount,
                                                               ref iTotalPageCount
                                                               , General.GetNullableInteger(ddlStatus.SelectedHard)
                                                                    , General.GetNullableString(txtTitle.Text)
                                                                    , General.GetNullableInteger(ddlType.SelectedValue)
                                                                    , General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

            if (ds.Tables.Count > 0)
            {
                General.ShowExcel("Defect Job", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvDefectList.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtComponentNumber.Text = "";
                txtComponentName.Text = "";
                ucDisciplineResponsibility.SelectedDiscipline = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        gvDefectList.SelectedIndexes.Clear();
        gvDefectList.EditIndexes.Clear();
        gvDefectList.DataSource = null;
        gvDefectList.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDEFECTNO", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDDETAILS", "FLDDUEDATE", "FLDRESPONSIBILITY", "FLDSTATUSNAME" };
            string[] alCaptions = { "Defect No", "Component No", "Component Name.", "Defect Details", "Due", "Responsibility", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixOwnerReportPMS.OwnersReportDefectJobSearch(General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
                                                               , General.GetNullableString(txtComponentNumber.Text),
                                                               General.GetNullableString(txtComponentName.Text),
                                                               General.GetNullableInteger(ucDisciplineResponsibility.SelectedDiscipline),
                                                                    sortexpression,
                                                                    sortdirection,
                                                                    gvDefectList.CurrentPageIndex + 1,
                                                                    gvDefectList.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount
                                                                    , General.GetNullableInteger(ddlStatus.SelectedHard)
                                                                    , General.GetNullableString(txtTitle.Text)
                                                                    , General.GetNullableInteger(ddlType.SelectedValue)
                                                                    , General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
            General.SetPrintOptions("gvDefectList", "Defect Job", alCaptions, alColumns, ds);
            gvDefectList.DataSource = ds;
            gvDefectList.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDefectList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            // ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDefectList.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDefectList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["DEFECTJOBID"] = (((RadLabel)e.Item.FindControl("lbldefectjobid")).Text);
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDEFECTJOBID").ToString();
                string script = "$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceDefectListUpdate.aspx?defectjobId=" + id + "';showDialog('Edit');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper().Equals("VERIFY"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDEFECTJOBID").ToString();
                string no = ((RadLabel)item.FindControl("lbldefectno")).Text;
                string script = "$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceDefectJobVerification.aspx?defectjobId=" + id + "&defectno=" + no + "';showDialog('Edit');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper().Equals("COMPLETE"))
            {

                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDEFECTJOBID").ToString();
                string script = "$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceDefectClose.aspx?DefectId=" + id + "';showDialog('Edit');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper().Equals("WORKORDER"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string defectid = (((RadLabel)e.Item.FindControl("lbldefectjobid")).Text);
                string defectno = (((RadLabel)e.Item.FindControl("lbldefectno")).Text);
                Response.Redirect(Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWODefectJobDetails.aspx?DefectId=" + defectid + "&DefectNo=" + defectno);

            }
            else if (e.CommandName.ToUpper() == "REQUISITION")
            {
                GridDataItem item = (GridDataItem)e.Item;
                LinkButton lnkFormNo = (LinkButton)item.FindControl("lnkReqNo");

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVessel", item.GetDataKeyValue("FLDVESSELID").ToString());
                criteria.Add("ddlStockType", "SPARE");
                criteria.Add("txtNumber", lnkFormNo.Text);
                Filter.CurrentOrderFormFilterCriteria = criteria;

                Filter.CurrentPurchaseDashboardCode = null;

                string script = "top.openNewWindow('detail', 'Requisition', '" + Session["sitepath"] + "/Purchase/PurchaseForm.aspx');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDefectList_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvDefectList.Rebind();
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixPlannedMaintenanceDefectJob.DefectJobDelete(new Guid(ViewState["DEFECTJOBID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}