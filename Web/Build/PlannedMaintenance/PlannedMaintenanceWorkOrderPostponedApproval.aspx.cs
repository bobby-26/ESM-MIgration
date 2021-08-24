using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Collections.Specialized;

public partial class PlannedMaintenanceWorkOrderPostponedApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderPostponedApproval.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPostponedApproval')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderPostponedFilter.aspx?" + Request.QueryString.ToString(), "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderPostponedApproval.aspx?" + Request.QueryString.ToString(), "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuPostponedApproval.AccessRights = this.ViewState;
            MenuPostponedApproval.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["DASHBOARDYN"] = Request.QueryString["DASHBOARD"] != null ? Request.QueryString["DASHBOARD"].ToString() : "";

                ucVessel.bind();
                ucVessel.DataBind();


                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() != "0")
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                    //ucVessel.Visible = false;
                    //tblFilter.Visible = false;
                    gvPostponedApproval.MasterTableView.GetColumn("FLDVESSELNAME").Visible = false;
                }

                if (Request.QueryString["workordernumber"] != null && Request.QueryString["workordernumber"] != "")
                {
                    //txtnumber.Text = Request.QueryString["workordernumber"].ToString();
                }
                gvPostponedApproval.PageSize = General.ShowRecords(gvPostponedApproval.PageSize);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPostponedApproval_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "FIND")
            {
                gvPostponedApproval.CurrentPageIndex = 0;
                gvPostponedApproval.Rebind();
            }
            if (CommandName.ToUpper() == "EXCEL")
            {
                ShowExcel();
            }
            if (CommandName.ToUpper() == "CLEAR")
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = "";
                }
                //txtnumber.Text = "";
                Filter.CurrentComponentFilterCriteria = null;
                gvPostponedApproval.CurrentPageIndex = 0;
                gvPostponedApproval.Rebind();
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
            string[] alColumns = { "FLDVESSELNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDPLANNINGDUEDATE", "FLDLASTDONEDATE" };
            string[] alCaptions = { "Vessel Name", "Number", "Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Responsibility", "Due Date", "Last Done Date" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            int? vessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            if(vessel == 0)
            {
                vessel = General.GetNullableInteger(ucVessel.SelectedVessel);
            }
            DataSet ds = new DataSet();
            if (Filter.CurrentComponentFilterCriteria != null)
            {
                NameValueCollection nvc = Filter.CurrentComponentFilterCriteria;

                ds = PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderPostponedApprovalSearch(vessel,
                        General.GetNullableString(nvc != null ? nvc.Get("txtnumber").ToString() : null),
                        sortexpression, sortdirection,
                        1,
                        iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount,
                        General.GetNullableGuid(nvc != null ? nvc.Get("txtComponent").ToString() : null),
                        General.GetNullableInteger(nvc != null ? nvc.Get("ucstatus").ToString() : null),
                         General.GetNullableInteger(nvc != null ? nvc.Get("rblpostpone").ToString() : null),
                         General.GetNullableInteger(nvc != null ? nvc.Get("responsibility").ToString() : null),
                           General.GetNullableInteger(nvc != null ? nvc.Get("ucComponentCategory").ToString() : null),
                           General.GetNullableString(nvc != null ? nvc.Get("txtclasscode").ToString() : null),
                           General.GetNullableString(ViewState["DASHBOARDYN"].ToString())


                        );
            }
            else
            {
                ds = PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderPostponedApprovalSearch(vessel, null,
                      sortexpression, sortdirection,
                      1,
                      iRowCount,
                      ref iRowCount,
                      ref iTotalPageCount,
                     null,
                     null,
                     null,
                     null,
                     null,
                     null,
                     General.GetNullableString(ViewState["DASHBOARDYN"].ToString())

                      );
            }

            General.ShowExcel("Postponement Requests", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvPostponedApproval.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPostponedApproval_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                ShowExcel();
            }
            if (e.CommandName.ToUpper() == "APPROVERESCHEDULE")
            {
                string workorderid = ((RadLabel)e.Item.FindControl("lblWorkorderid")).Text;
                int vesselid = Convert.ToInt16(((RadLabel)e.Item.FindControl("lblVesselid")).Text);
                string rescheduleid = ((RadLabel)e.Item.FindControl("lblRescheduleID")).Text;
                PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleRequestApprove(vesselid, new Guid(workorderid), General.GetNullableGuid(rescheduleid));
                gvPostponedApproval.Rebind();
            }
            else if (e.CommandName.ToUpper() == "RA")
            {
                string lblRA = ((RadLabel)e.Item.FindControl("lblRA")).Text;
                string link = "";
                if (lblRA != string.Empty)
                {
                    byte IsnewRA = 0;
                    PhoenixPlannedMaintenanceWorkOrderReschedule.RescheduleRiskAssessmentIdentify(new Guid(lblRA), ref IsnewRA);
                    if (IsnewRA == 1)
                    {
                        link = "javascript:openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryExtn.aspx?machineryid=" + lblRA + "');";
                    }
                    else
                    {
                        link = "javascript:openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/SSRSReports/SsrsReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblRA + "&showmenu=0&showexcel=NO');";
                    }
                    //String scriptpopup = String.Format("javascript:openNewWindow(" + link + ");");
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", link, true);
                }
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string rescheduleid = ((RadLabel)e.Item.FindControl("lblRescheduleID")).Text;
                string workorderid = ((RadLabel)e.Item.FindControl("lblWorkorderid")).Text;
                Response.Redirect("PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + workorderid + "&RESCHEDULEID=" + rescheduleid + "&b=approval");
            }
            if (e.CommandName.ToUpper().Equals("CANCELREQ"))
            {
                int vesselid = Convert.ToInt16(((RadLabel)e.Item.FindControl("lblVesselid")).Text);
                string workorderid = ((RadLabel)e.Item.FindControl("lblWorkorderid")).Text;
                string rescheduleid = ((RadLabel)e.Item.FindControl("lblRescheduleID")).Text;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {                   
                    PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleRequestCancel(vesselid, new Guid(workorderid), General.GetNullableGuid(rescheduleid), null);
                    gvPostponedApproval.Rebind();
                }
                else
                {
                    string title = ((LinkButton)e.Item.FindControl("lnkJobDetaiil")).Text;
                    string script = "$modalWindow.modalWindowUrl='PlannedMaintenanceRemarksPopup.aspx?vslid=" + vesselid + "&woid="+ workorderid + "&woreschid=" + rescheduleid + "';showDialog('Cancel Remark - "+ title + "');";
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
                }
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPostponedApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDPLANNINGDUEDATE", "FLDLASTDONEDATE" };
        string[] alCaptions = { "Vessel Name", "Number", "Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Responsibility", "Due Date", "Last Done Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        int? vessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (vessel == 0)
        {
            vessel = General.GetNullableInteger(ucVessel.SelectedVessel);
        }


        NameValueCollection nvc = Filter.CurrentComponentFilterCriteria;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        DataSet ds = PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderPostponedApprovalSearch(vessel,
                    General.GetNullableString(nvc.Get("txtnumber") ?? null),
                    sortexpression, sortdirection,
                  gvPostponedApproval.CurrentPageIndex + 1,
                    gvPostponedApproval.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableGuid(nvc.Get("txtComponent") ?? null),
                    General.GetNullableInteger(nvc.Get("ucstatus") ?? null),
                     General.GetNullableInteger(nvc.Get("rblpostpone") ?? null),
                     General.GetNullableInteger(nvc.Get("responsibility") ?? null),
                      General.GetNullableInteger(nvc.Get("ucComponentCategory") ?? null),
                       General.GetNullableString(nvc.Get("txtclasscode") ?? null),
                       General.GetNullableString(ViewState["DASHBOARDYN"].ToString())

                    );

        General.SetPrintOptions("gvPostponedApproval", "Postponement Requests", alCaptions, alColumns, ds);

        gvPostponedApproval.DataSource = ds;
        gvPostponedApproval.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvPostponedApproval_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
           
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)item.DataItem;

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null)
                {
                    cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
                    cancel.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure want to cancel this request?'); return false;");
                }                                
                ImageButton cmdApproveReschedule = (ImageButton)e.Item.FindControl("cmdApproveReschedule");
                if (cmdApproveReschedule != null)
                {
                    cmdApproveReschedule.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Are you sure want to approve this request?'); return false;");
                    if (drv["FLDAPPROVEDYN"].ToString() == "0" && drv["FLDRESCHEDULESTATUS"].ToString() == string.Empty)
                        cmdApproveReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdApproveReschedule.CommandName);
                    else if (drv["FLDRESCHEDULESTATUS"].ToString() == "2")
                        cmdApproveReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdApproveReschedule.CommandName);
                    else
                        cmdApproveReschedule.Visible = false;
                }

                ImageButton au = (ImageButton)e.Item.FindControl("cmdView");
                if (au != null)
                {
                    au.Visible = SessionUtil.CanAccess(this.ViewState, au.CommandName);
                    string workorderid = ((RadLabel)e.Item.FindControl("lblWorkorderid")).Text;
                    int vesselid = Convert.ToInt16(((RadLabel)e.Item.FindControl("lblVesselid")).Text);

                    au.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=" + workorderid + "&vesselid=" + vesselid + "'); return false;");
                    if (drv["FLDCOUNT"].ToString().Equals("") || drv["FLDCOUNT"].ToString().Equals("0"))
                        au.Visible = false;
                }
               

                ImageButton cmdMapRA = (ImageButton)e.Item.FindControl("cmdRA");
                if (cmdMapRA != null)
                {
                    if (drv["FLDRAID"].ToString().Equals(""))
                    {
                        cmdMapRA.Visible = false;
                    }
                    else
                    {
                        //cmdMapRA.Attributes.Add("onclick", "javascript:openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/SSRSReports/SsrsReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERYNEW&machineryid=" + drv["FLDRAID"].ToString() + "&showmenu=0&showexcel=NO'); return false;");
                        cmdMapRA.Visible = SessionUtil.CanAccess(this.ViewState, cmdMapRA.CommandName);
                    }
                }
                
                ImageButton cmdFeedback = (ImageButton)e.Item.FindControl("cmdFeedback");
                if (cmdFeedback != null)
                {
                    
                    if (drv["FLDISSMARRA"].ToString().Equals("0"))
                    {
                        cmdFeedback.Visible = false;
                    }
                    else
                    {
                        cmdFeedback.Attributes.Add("onclick", "javascript:openNewWindow('feedback', '" + drv["FLDWORKORDERNUMBER"].ToString() + " " + drv["FLDWORKORDERNAME"].ToString().Replace("'", "&lsquo;") + "', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWOPostponementQuestionFeedback.aspx?WorkOrderId=" + drv["FLDWORKORDERID"].ToString() + "&WORescheduledId=" + drv["FLDWORKORDERRESCHEDULEID"].ToString() + "&ViewOnly=Y');");
                        cmdFeedback.Visible = SessionUtil.CanAccess(this.ViewState, cmdFeedback.CommandName);
                    }
                }
                LinkButton xlwp11 = (LinkButton)e.Item.FindControl("cmdXLWP11");
                if (xlwp11 != null)
                {
                    if (drv["FLDRAREPORTID"].ToString() != string.Empty)
                    {
                        cmdFeedback.Visible = false;
                        xlwp11.Visible = SessionUtil.CanAccess(this.ViewState, xlwp11.CommandName);
                        xlwp11.Attributes.Add("onclick", "javascript: openNewWindow('Filter', '', '" + Session["sitepath"] + "/Options/OptionsPMS.ashx?methodname=RAEXPORT2XL&exportoption=RA&workorderid=" + drv["FLDWORKORDERID"].ToString() + "&vesselId=" + drv["FLDVESSELID"] + "&Pid=" + drv["FLDRAFLDDTKEY"].ToString() + "&ReportID=" + drv["FLDRAREPORTID"].ToString() + "&PostponeDate=" + drv["FLDPOSTPONEDATE"].ToString() + "&usercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "'); ");
                    }
                    else
                    {
                        xlwp11.Visible = false;
                    }
                }
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                }
                LinkButton ljd = (LinkButton)e.Item.FindControl("lnkJobDetaiil");
                if (ljd != null)
                {
                    ljd.Visible = SessionUtil.CanAccess(this.ViewState, ljd.CommandName);
                    //ljd.Attributes.Add("onclick", "javascript: top.openNewWindow('jobdetail', 'Job Detail', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobDetailForWorkOrder.aspx?JOBID=" + drv["FLDJOBID"] + "'); ");

                    string cjid = drv["FLDCOMPONENTJOBID"].ToString();
                    if (General.GetNullableGuid(cjid).HasValue && General.GetNullableGuid(cjid).Value != Guid.Empty)
                        ljd.Attributes.Add("onclick", "javascript:openNewWindow('jobdetail', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0'); ");
                    else
                        ljd.Attributes.Add("onclick", "javascript:openNewWindow('jobdetail','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "','','1200','600');return false");

                }
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        gvPostponedApproval.CurrentPageIndex = 0;
        gvPostponedApproval.Rebind();
    }
}