using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderRescheduleDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["LOG"] = null;

                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbargrid = new PhoenixToolbar();

                toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWRes')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

                if (Request.QueryString["LOG"] != null)
                {
                    ViewState["LOG"] = Request.QueryString["LOG"].ToString();
                }
                else
                {
                    toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx", "Add", "<i class=\"fas fa-plus\"></i>", "ADD"); 
                }
                MenuDivWorkOrderReschedule.AccessRights = this.ViewState;
                MenuDivWorkOrderReschedule.MenuList = toolbargrid.Show();
                //MenuDivWorkOrderReschedule.SetTrigger(pnlMenuWorkOrderRescheduleDetail);

                ViewState["VESSELID"] = null;
                ViewState["WORKORDERID"] = null;
                ViewState["STATUS"] = "";

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    ViewState["VESSELID"] = Convert.ToInt16(Request.QueryString["vesselid"]);
                }
                else
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                }
                if (Request.QueryString["WORKORDERID"] != null)
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                }
                if (Request.QueryString["LOG"] != null)
                {
                    ViewState["LOG"] = Request.QueryString["WORKORDERID"].ToString();

                }
            }
            //BindData();
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
            string[] alColumns = { "FLDPOSTPONEDATE", "FLDPOSTPONEREASON", "FLDDUEDATE", "FLDAPPROVEDDATE", "FLDRAREFNO", "FLDPOSTPONEREMARKS" };
            string[] alCaptions = { "Postpone Date", "Postpone Reason", "Due Date", "Approved Date", "Linked RA", "Remarks" };

            DataTable dt = PhoenixPlannedMaintenanceWorkOrderReschedule.ListWorkOrderReschedule(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                new Guid(ViewState["WORKORDERID"].ToString()));

            General.SetPrintOptions("gvWRes", "Postpone Approval", alCaptions, alColumns, dt.DataSet);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewState["STATUS"] = dr["FLDSTATUS"].ToString();
            }
            gvWRes.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void SetRADetail(RadImageButton cmdRA, string raid, string ratype)
    {
        cmdRA.Visible = false;
        if (raid != string.Empty)
        {
            cmdRA.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDPOSTPONEDATE", "FLDPOSTPONEREASON", "FLDDUEDATE", "FLDAPPROVEDBY", "FLDAPPROVEDDATE", "FLDRAREFNO", "FLDRISKASSESSMENT", "FLDPOSTPONEREMARKS"};
            string[] alCaptions = { "Postpone Date", "Postpone Reason", "Due Date", "Approved By", "Approved Date", "RA Reference No:", "Risk Assessment", "Remarks"};

            DataTable dt = PhoenixPlannedMaintenanceWorkOrderReschedule.ListWorkOrderReschedule(int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(ViewState["WORKORDERID"].ToString()));

            General.ShowExcel("Work Order Reschedule Details", dt, alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrderReschedule_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "EXCEL")
            {
                ShowExcel();
            }
            if (CommandName.ToUpper() == "ADD")
            {
                if (ViewState["STATUS"].ToString() == "0" || ViewState["STATUS"] == null || ViewState["STATUS"].ToString() == "")
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + ViewState["WORKORDERID"].ToString(), true);
                }
                else
                {
                    ucError.HeaderMessage = "Postponement";
                    ucError.ErrorMessage = "Postpone request pending. Cannot add new Postpone Request.";
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            DataRowView drv = (DataRowView)item.DataItem;

            RadImageButton cmdRA = (RadImageButton)e.Item.FindControl("cmdRA");
            if (cmdRA != null)
            {
                RadLabel RA = (RadLabel)e.Item.FindControl("lblRA");
                RadLabel Type = (RadLabel)e.Item.FindControl("lblType");
                SetRADetail(cmdRA, RA.Text.ToString(), Type.Text.ToString());
            }
            LinkButton Download = (LinkButton)e.Item.FindControl("imgDownload");
            RadLabel lblRAReportId = (RadLabel)e.Item.FindControl("lblRAReportId");
            if (string.IsNullOrEmpty(lblRAReportId.Text))
            {
                Download.Visible = false;
            }
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null && ViewState["LOG"] != null)
            {
                cmdEdit.Visible = false;
            }

            RadImageButton cmdMapRA = (RadImageButton)e.Item.FindControl("cmdMapRA");
            if (cmdMapRA != null)
            {
                if (drv["FLDRAID"].ToString().Equals(""))
                {
                    cmdMapRA.Visible = false;
                }
                else
                {
                    //cmdMapRA.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentRiskAssessmentList.aspx?ComponentId=" + drv["FLDCOMPONENTID"].ToString() + "&WorkorderId=" + drv["FLDWORKORDERID"].ToString() + "&WORescheduleID=" + drv["FLDWORKORDERRESCHEDULEID"].ToString() + "&WORescheduleDTKey=" + drv["FLDRAFLDDTKEY"].ToString() + "&ispostpone=1');");
                    cmdMapRA.Visible = SessionUtil.CanAccess(this.ViewState, cmdMapRA.CommandName);
                }                
            }

            RadImageButton cmdFeedback = (RadImageButton)e.Item.FindControl("cmdFeedback");
            if (cmdFeedback != null)
            {
                if (drv["FLDWORKORDERRESCHEDULEID"].ToString().Equals(string.Empty)) { cmdFeedback.Visible = false; }
                else if (drv["FLDISSMARRA"].ToString().Equals("0"))
                {
                    cmdFeedback.Visible = false;
                }
                else
                {
                    if (Request.QueryString["vesselid"] != null)
                        cmdFeedback.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWOPostponementQuestionFeedback.aspx?WorkOrderId=" + drv["FLDWORKORDERID"].ToString() + "&WORescheduledId=" + drv["FLDWORKORDERRESCHEDULEID"].ToString() + "&vesselid="+ Request.QueryString["vesselid"] + "');");
                    else
                        cmdFeedback.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWOPostponementQuestionFeedback.aspx?WorkOrderId=" + drv["FLDWORKORDERID"].ToString() + "&WORescheduledId=" + drv["FLDWORKORDERRESCHEDULEID"].ToString() + "');");
                    cmdFeedback.Visible = SessionUtil.CanAccess(this.ViewState, cmdFeedback.CommandName);
                }
            }
        }
        
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper()=="DOWNLOAD")
            {
                LinkButton au = (LinkButton)e.Item.FindControl("imgDownload"); 
                if(au != null)
                {
                    string ReportId = ((RadLabel)e.Item.FindControl("lblRAReportId")).Text;
                    string PDate = ((RadLabel)e.Item.FindControl("lblPostponeDate")).Text;
                    string PId = ((RadLabel)e.Item.FindControl("lblRADTKey")).Text;

                    //au.Attributes.Add("onclick", "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Options/OptionsPMS.ashx?methodname=RAEXPORT2XL&exportoption=RA&workorderid=" + ViewState["WORKORDERID"].ToString() + "&Pid=" + PId + "&ReportID=" + ReportId + "&PostponeDate=" + PDate + "&usercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "'); return false;");
                    String scriptpopup = String.Format("javascript:openNewWindow('Filter', '', '" + Session["sitepath"] + "/Options/OptionsPMS.ashx?methodname=RAEXPORT2XL&exportoption=RA&workorderid=" + ViewState["WORKORDERID"].ToString() + "&Pid=" + PId + "&ReportID=" + ReportId + "&PostponeDate=" + PDate + "&usercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                    BindData();
                }
               
            }
            if (e.CommandName.ToUpper() == "RA")
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
                Response.Redirect("PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "&RESCHEDULEID=" + rescheduleid + "&b=detail");
            }
            else if (e.CommandName.ToUpper().Equals("CONFIRM"))
            {
                PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleRequestConfirm(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["WORKORDERID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

}
