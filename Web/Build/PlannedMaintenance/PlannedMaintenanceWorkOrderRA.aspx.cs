using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderRA : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvRa')", "Print Grid", "icon_print.png", "PRINT");
            //MenuRA.AccessRights = this.ViewState;
            //MenuRA.MenuList = toolbar.Show();

            //PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            ////toolbarmain.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
            //toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            //MenuWorkOrderRA.AccessRights = this.ViewState;
            //MenuWorkOrderRA.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["DUEDATE"] = "";
                ViewState["PRIORITY"] = "";
                ViewState["WORKORDERNO"] = "";
                ViewState["VESSELID"] = "0";
                ViewState["MAXPOSTPONEDATE"] = "";
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
                ViewState["OWNERVESSELID"] = "0";
                if (Request.QueryString["vesselid"] != null)
                    ViewState["OWNERVESSELID"] = Request.QueryString["vesselid"];
                else
                    ViewState["OWNERVESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                if (Request.QueryString["WORKORDERRESCHEDULEID"] != null)
                    ViewState["WORKORDERRESCHEDULEID"] = Request.QueryString["WORKORDERRESCHEDULEID"];
                else
                    ViewState["WORKORDERRESCHEDULEID"] = "";
                ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["PAGENUMBER"]) ? "1" : Request.QueryString["PAGENUMBER"].ToString();
                if (Request.QueryString["vesselid"] != null)
                    cmdShowReason.Attributes.Add("onclick", "return showPickList('spnPickReason', 'codehelp1', '', '../PlannedMaintenance/PlannedMaintenanceRemarksPopup.aspx?framename=ifMoreInfo&WORKORDERID=" + Request.QueryString["WORKORDERID"] + "&vesselid=" + ViewState["OWNERVESSELID"] + "', true);");
                else
                    cmdShowReason.Attributes.Add("onclick", "return showPickList('spnPickReason', 'codehelp1', '', '../PlannedMaintenance/PlannedMaintenanceRemarksPopup.aspx?framename=ifMoreInfo&WORKORDERID=" + Request.QueryString["WORKORDERID"] + "', true);");
                RadWindow_NavigateUrl.NavigateUrl = "../Common/CommonPickListMachineryRA.aspx?catid=3&vesselid=" + ViewState["OWNERVESSELID"] + "&status=4,5";
                imgShowRA.Attributes.Add("onclick", "showDialog('RA');");
                //cmdShowHistory.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=" + ViewState["WORKORDERID"] + "');return true;");
                if (Request.QueryString["vesselid"] != null)
                    cmdShowHistory.Attributes.Add("onclick", "javascript: top.openNewWindow('psphistory','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=" + ViewState["WORKORDERID"] + "&vesselid=" + ViewState["OWNERVESSELID"] + "'); return false;");
                else
                    cmdShowHistory.Attributes.Add("onclick", "javascript: top.openNewWindow('psphistory','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=" + ViewState["WORKORDERID"] + "'); return false;");

                BindFields();
            }
            BindMenu();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkOrderRA_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPostponement(txtPostponeDate.SelectedDate, ddlRescheduleReason.SelectedQuick, txtReason.Text, txtRAId.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleRequest(int.Parse(ViewState["OWNERVESSELID"].ToString()),
                    new Guid(ViewState["WORKORDERID"].ToString())
                    , General.GetNullableInteger(ddlRescheduleReason.SelectedQuick)
                    , txtPostponeDate.SelectedDate.Value
                    , txtReason.Text
                    , byte.Parse(chkExpectionalCircumstance.Checked.HasValue && chkExpectionalCircumstance.Checked.Value ? "1" : "0")
                    , General.GetNullableGuid(ViewState["WORKORDERRESCHEDULEID"].ToString()));
                ucStatus.Text = "Saved";
                Response.Redirect("PlannedMaintenanceWorkOrderPostponedApproval.aspx?Dashboard=Y&code=TECH-PMS-OPJPA");               

            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleRequestConfirm(int.Parse(ViewState["OWNERVESSELID"].ToString()), new Guid(ViewState["WORKORDERID"].ToString()), General.GetNullableGuid(ViewState["WORKORDERRESCHEDULEID"].ToString()));
                Response.Redirect("PlannedMaintenanceWorkOrderPostponedApproval.aspx?Dashboard=Y&code=TECH-PMS-OPJPA");
            }
            if(CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["b"] != null)
                {
                    string url = Request.QueryString["b"];
                    if (url != null & url.ToUpper().Equals("APPROVAL"))
                    {
                        Response.Redirect("PlannedMaintenanceWorkOrderPostponedApproval.aspx?Dashboard=Y&code=TECH-PMS-OPJPA");
                        return;
                    }

                    Response.Redirect("PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=" + ViewState["WORKORDERID"].ToString());
                }
                else
                {
                    Response.Redirect("PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=" + ViewState["WORKORDERID"].ToString());
                }
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindFields()
    {
        try
        {
            if (ViewState["WORKORDERID"] != null && ViewState["WORKORDERID"].ToString() != "")
            {
                DataSet ds = PhoenixPlannedMaintenanceWorkOrder.EditPostponeWorkOrder(new Guid(ViewState["WORKORDERID"].ToString()), int.Parse(ViewState["OWNERVESSELID"].ToString()), General.GetNullableGuid(ViewState["WORKORDERRESCHEDULEID"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];

                txtPostponeDate.SelectedDate = General.GetNullableDateTime(dr["FLDPOSTPONEDATE"].ToString());// != null ? dr["FLDPOSTPONEDATE"].ToString() : "";

                if (General.GetNullableInteger(dr["FLDPOSTPONEREASONID"].ToString()) != null)
                    ddlRescheduleReason.SelectedValue = dr["FLDPOSTPONEREASONID"].ToString();
                //txtReportedBy.Text = dr[""].ToString();
                txtReason.Text = dr["FLDPOSTPONEREMARKS"].ToString();
                txtDueDate.Text =  dr["FLDPLANNINGDUEDATE"].ToString();
                chkExpectionalCircumstance.Checked = dr["FLDISEXCCIRC"].ToString().Equals("1");
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                ViewState["DUEDATE"] = dr["FLDPLANNINGDUEDATE"].ToString();
                ViewState["PRIORITY"] = dr["FLDPLANINGPRIORITY"].ToString();
                ViewState["WORKORDERNO"] = dr["FLDWORKORDERNUMBER"].ToString();
                ViewState["ReportID"] = dr["FLDRAREPORTID"].ToString();
                ViewState["RAFLDDTKEY"] = dr["FLDRAFLDDTKEY"].ToString();
                ViewState["POSTPONEDATE"] = dr["FLDPOSTPONEDATE"].ToString();
                ViewState["WORKORDERRESCHEDULEID"] = dr["FLDWORKORDERRESCHEDULEID"].ToString();
                hdnComp.Value= dr["FLDCOMPONENTID"].ToString();
                lblCurrentRunningHrValue.Text = dr["FLDCURRENTRUNHR"].ToString();
                lblEstRunHrValue.Text = dr["FLDPOSPONERUNHR"].ToString();
                lblEstDueDateValue.Text = General.GetDateTimeToString(dr["FLDESTDUEDATE"]);
                lblPosponeDateValue.Text = General.GetDateTimeToString(dr["FLDPOSTPONEDUEDATE"].ToString());
                ViewState["MAXPOSTPONEDATE"] = dr["FLDPOSTPONEDUEDATE"].ToString();
                txtPostponeDate_SelectedDateChanged(null, null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private bool IsValidPostponement(DateTime? duedate, string reason, string remarks, string raid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!duedate.HasValue)
            ucError.ErrorMessage = "Postponement Date is required.";
        else if (DateTime.Compare(duedate.Value, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "Postponement Date should be later than current date";
        }
        else if (ViewState["DUEDATE"].ToString() != "" && DateTime.Compare(duedate.Value, DateTime.Parse(ViewState["DUEDATE"].ToString())) < 0)
        {
            ucError.ErrorMessage = "Postponement Date should be later than Due Date";
        }


        if (!General.GetNullableInteger(reason).HasValue)
        {
            ucError.ErrorMessage = "Postponement Reason is required.";
        }

        if (remarks.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Postponement Remarks is required.";
        }
        //DateTime? m = General.GetNullableDateTime(ViewState["MAXPOSTPONEDATE"].ToString());
        //chkExpectionalCircumstance.Enabled = false;
        //if (duedate.HasValue && m.HasValue && DateTime.d(duedate.Value, m.Value) > 0)
        //{
        //    ucError.ErrorMessage = "Exceptional Circumstances tobe checked.";
        //}
        return (!ucError.IsError);
    }

    protected void BindMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkOrderRA.AccessRights = this.ViewState;
        MenuWorkOrderRA.MenuList = toolbarmain.Show();

        if(ViewState["WORKORDERRESCHEDULEID"].ToString().Equals("")) return;

        DataSet ds = PhoenixPlannedMaintenanceWorkOrder.EditPostponeWorkOrder(new Guid(ViewState["WORKORDERID"].ToString()), int.Parse(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(ViewState["WORKORDERRESCHEDULEID"].ToString()));
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbarmain.AddButton("Submit", "CONFIRM", ToolBarDirection.Right);

            if (General.GetNullableInteger(dr["FLDISEXCCIRC"].ToString()) == 1)
            {
                RadWindow_NavigateUrl.NavigateUrl = Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentRiskAssessmentList.aspx?ComponentId=" + dr["FLDCOMPONENTID"].ToString() + "&WorkorderId=" + dr["FLDWORKORDERID"].ToString() + "&WORescheduleID=" + dr["FLDWORKORDERRESCHEDULEID"].ToString() + "&WORescheduleDTKey=" + dr["FLDRAFLDDTKEY"].ToString() + "&ispostpone=1&inlp=1";
                //toolbarmain.AddButton("Create RA", "RA", ToolBarDirection.Right);
                toolbarmain.AddFontAwesomeButton("javascript: showDialog('Map RA');", "Map RA", "<i class=\"fas fa-registered\"></i>", "RA");
                divRA.Visible = true;
                txtRANumber.Text = dr["FLDRAREFNO"].ToString();
                txtRA.Text = General.GetNullableString(dr["FLDRISKASSESSMENT"].ToString()) != null ? dr["FLDRISKASSESSMENT"].ToString() : dr["FLDNAME"].ToString();
                txtRaType.Text = dr["FLDTYPE"].ToString();
                txtRAId.Text = dr["FLDRAID"].ToString();

            }
            else
            {
                if (Request.QueryString["vesselid"] != null)
                    toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('feedback', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWOPostponementQuestionFeedback.aspx?WorkOrderId=" + dr["FLDWORKORDERID"].ToString() + "&WORescheduledId=" + dr["FLDWORKORDERRESCHEDULEID"].ToString() + "&vessselid=" + ViewState["OWNERVESSELID"].ToString() + "');", "Postpone Feedback", "<i class=\"fas fa-postcomment\"></i>", "FEEDBACK");
                else
                    toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('feedback', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWOPostponementQuestionFeedback.aspx?WorkOrderId=" + dr["FLDWORKORDERID"].ToString() + "&WORescheduledId=" + dr["FLDWORKORDERRESCHEDULEID"].ToString() + "');", "Postpone Feedback", "<i class=\"fas fa-postcomment\"></i>", "FEEDBACK");
            }

            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuWorkOrderRA.AccessRights = this.ViewState;
            MenuWorkOrderRA.MenuList = toolbarmain.Show();
        }       
    }
    
    protected void lnkCreateRA_OnClick(object sender, EventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
        {
            RadScriptManager.RegisterStartupScript(this.Page, typeof(Page), "redirect", "parent.location.href='" + Session["sitepath"] + "/Inspection/InspectionRAMachinery.aspx?status=&IsPostpone=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString() + "&WORKORDERNO=" + ViewState["WORKORDERNO"].ToString() + "';", true);
        }
        else
        {
            RadScriptManager.RegisterStartupScript(this.Page, typeof(Page), "redirect", "parent.location.href='" + Session["sitepath"] + "/Inspection/InspectionRAMachineryDetails.aspx?status=&IsPostpone=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString() + "&WORKORDERNO=" + ViewState["WORKORDERNO"].ToString() + "';", true);
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void lnkDownloadRA_Click(object sender, EventArgs e)
    {
        String scriptpopup;
        if (Request.QueryString["vesselid"] != null)
            scriptpopup = String.Format("javascript:parent.openNewWindow('City','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWOPostponementQuestionFeedback.aspx?WorkOrderId=" + ViewState["WORKORDERID"].ToString() + "&WORescheduledId=" + ViewState["WORKORDERRESCHEDULEID"].ToString() + "&vessselid="+ViewState["OWNERVESSELID"].ToString()+"');");
        else
            scriptpopup = String.Format("javascript:parent.openNewWindow('City','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWOPostponementQuestionFeedback.aspx?WorkOrderId=" + ViewState["WORKORDERID"].ToString() + "&WORescheduledId=" + ViewState["WORKORDERRESCHEDULEID"].ToString() + "');");
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    }

    protected void cmdRA_Click(object sender, ImageClickEventArgs e)
    {
        BindFields();
        if (General.GetNullableGuid(txtRAId.Text) != null)
        {
            string link = "";
            link = "RAMachinery', '', '" + Session["sitepath"] + "/SSRSReports/SsrsReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERYNEW&machineryid=" + txtRAId.Text + "&showmenu=0&showexcel=NO";
            String scriptpopup = String.Format("javascript: top.openNewWindow('" + link + "');");
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindFields();
    }    

    protected void txtPostponeDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        DateTime? d = txtPostponeDate.SelectedDate;
        DateTime? m = General.GetNullableDateTime(ViewState["MAXPOSTPONEDATE"].ToString());
        chkExpectionalCircumstance.Enabled = false;
        if (d.HasValue && m.HasValue && d > m)
        {
            chkExpectionalCircumstance.Enabled = true;
        }
    }
    protected void gvPostponematrix_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            DataTable dt = PhoenixRegistersPostponementMatrix.List();
            gvPostponematrix.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
