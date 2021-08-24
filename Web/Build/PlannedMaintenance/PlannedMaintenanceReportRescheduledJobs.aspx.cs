using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportRescheduledJobs : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "GO",ToolBarDirection.Right);
            MenuReportRescheduledJobs.AccessRights = this.ViewState;
            MenuReportRescheduledJobs.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ckWotype.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.WORKORDERTYPE), 0,"PDC,PRQ,ROU,CVD");
                ckWotype.DataBind();
                chkClasses.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixQuickTypeCode.JOBCLASS));
                chkClasses.DataBind();

                ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
                ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
                ucMainCause.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCAUSE)).ToString();

                ViewState["ISTREENODECLICK"] = false;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["COMPONENTID"] = null;
                ViewState["WORKORDERID"] = null;
                ViewState["WIEVCOMPONENTID"] = "";
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["COMPONENTJOBID"] = null;
                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["WIEVCOMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                if (Request.QueryString["WORKORDERID"] != null)
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected bool IsValidDateRange(DateTime? FromDate, DateTime? ToDate)
    {
        if (FromDate == null || ToDate == null)
        {
            return true;
        }
        else if (FromDate > ToDate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    protected bool IsValidDurationRange(int? durationFrom, int? durationTo)
    {
        if (durationFrom == null || durationTo == null)
        {
            return true;
        }
        else if (durationFrom > durationTo)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected bool IsValidPriorityRange(int? PriorityFrom, int? PeriodTo)
    {
        if (PriorityFrom == null || PeriodTo == null)
        {
            return true;
        }
        else if (PriorityFrom > PeriodTo)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected bool IsValidRangeFilters()
    {
        if (!IsValidDateRange(General.GetNullableDateTime(txtDateFrom.Text), General.GetNullableDateTime(txtDateTo.Text)))
        {
            ucError.ErrorMessage = "From Date should not be greater than To Date";
            ucError.Visible = true;
            return false;
        }
        if (!IsValidDurationRange(General.GetNullableInteger(txtDurationFrom.Text), General.GetNullableInteger(txtDurationTo.Text)))
        {
            ucError.ErrorMessage = "From Duration should not be greater than To Duration";
            ucError.Visible = true;
            return false;
        }
        if (!IsValidPriorityRange(General.GetNullableInteger(txtPriorityFrom.Text), General.GetNullableInteger(txtPriorityTo.Text)))
        {
            ucError.ErrorMessage = "From Priority should not be greater than To Priority";
            ucError.Visible = true;
            return false;
        }

        return true;
    }

    protected void MenuReportRescheduledJobs_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GO"))
            {
                if (IsValidRangeFilters())
                {
                    string prams = "";
                    string status = "";
                    string worktype = "";
                    string jobclass = "";

                    foreach (ButtonListItem item in chkStatus.Items)
                    {
                        if (item.Selected && item.Value == "1")
                        {
                            DataTable dt = new DataTable();
                            dt = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.WORKORDERSTATUS), 0, "CMT").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    status += dr["FLDHARDCODE"].ToString() + ",";
                                }
                            }
                        }
                        else if (item.Selected && item.Value == "0")
                        {
                            DataTable dt = new DataTable();
                            dt = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.WORKORDERSTATUS), 0, "REQ,CAN,POP,PND,ISS").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    status += dr["FLDHARDCODE"].ToString() + ",";
                                }
                            }
                        }
                    }
                    foreach (ButtonListItem item in ckWotype.Items)
                    {
                        if (item.Selected)
                            worktype = worktype + item.Value + ",";
                    }
                    foreach (ButtonListItem item in chkClasses.Items)
                    {
                        if (item.Selected)
                            jobclass = jobclass + item.Value + ",";
                    }

                    status = status.TrimEnd(new Char[] { ',' });

                    string unexp = "";
                    if (chkUnexpected.Checked == true)
                        unexp = "1";
                    else
                        unexp = "";


                    prams += "&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    prams += "&workorderno=" + General.GetNullableString(txtWorkOrderNumber.Text.Trim());
                    prams += "&workordername=" + General.GetNullableString(txtWorkOrderDescription.Text.Trim());
                    prams += "&compno=" + General.GetNullableString(txtComponentNumber.Text.Trim());
                    prams += "&compname=" + General.GetNullableString(txtComponentName.Text.Trim());
                    prams += "&jobclass=" + General.GetNullableString(jobclass.TrimEnd(new Char[] { ',' }));
                    prams += "&dtfrom=" + General.GetNullableDateTime(txtDateFrom.Text);
                    prams += "&dtto=" + General.GetNullableDateTime(txtDateTo.Text);
                    prams += "&status=" + General.GetNullableString(status.TrimEnd(new Char[] { ',' }));
                    prams += "&wotype=" + General.GetNullableString(worktype.TrimEnd(new Char[] { ',' }));
                    prams += "&maintype=" + General.GetNullableInteger(ucMainType.SelectedQuick);
                    prams += "&mainclass=" + General.GetNullableInteger(ucMaintClass.SelectedQuick);
                    prams += "&maincause=" + General.GetNullableInteger(ucMainCause.SelectedQuick);
                    prams += "&workunexp=" + (byte?)General.GetNullableInteger(unexp);
                    prams += "&priorityfrom=" + (byte?)General.GetNullableInteger(txtPriorityFrom.Text);
                    prams += "&priorityto=" + (byte?)General.GetNullableInteger(txtPriorityTo.Text);
                    prams += "&durationfrom=" + General.GetNullableInteger(txtDurationFrom.Text);
                    prams += "&durationto=" + General.GetNullableInteger(txtDurationTo.Text);
                    prams += "&dtPostponed=" + General.GetNullableDateTime(txtPostponedDate.Text);
                    prams += exceloptions();

                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=RESCHEDULEDJOBS" + prams);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected string exceloptions()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

        string options = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["FLDSHORTNAME"].ToString().Equals("EXL"))
                options += "&showexcel=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("WRD"))
                options += "&showword=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("PDF"))
                options += "&showpdf=" + dr["FLDHARDNAME"].ToString();
        }
        return options;
    }
}
