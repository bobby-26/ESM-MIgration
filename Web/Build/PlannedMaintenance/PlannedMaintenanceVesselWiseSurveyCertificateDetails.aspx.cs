using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceVesselWiseSurveyCertificateDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselWiseSurveyCertificateDetails.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSurvey')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuSurveySchedule.AccessRights = this.ViewState;
            MenuSurveySchedule.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                gvSurvey.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["VESSELID"] = "";
                ViewState["SheduleId"] = "";
                ViewState["CertificateId"] = string.IsNullOrEmpty(Request.QueryString["CertificateId"]) ? "" : Request.QueryString["CertificateId"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 0;
                ViewState["CURRENTINDEX"] = 0;
                ViewState["SELECTEDINDEX"] = 0;
                ViewState["CATEGORY"] = "";
                if (!string.IsNullOrEmpty(Request.QueryString["category"]))
                {
                    ViewState["CATEGORY"] = Request.QueryString["category"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["VESSELID"]))
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"];
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FlDVESSELNAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDNEXTDUEDATE", "FLDWINDOWPERIODBEFORE", "FLDWINDOWPERIODAFTER", "FLDSURVEYTYPENAME", "FLDPLANDATE", "FLDSEAPORTNAME", "FLDANNIVERSARYDATE", "FLDLASTSURVEYDATE", "FLDLASTSURVEYTYPENAME", "FLDCERTIFICATEREMARKS" };
        string[] alCaptions = { "Vessel", "Issue Date", "Expiry Date", "Issued By", "Due Date for Survey / Follow up", "Window(Before)", "Window(After)", "Type of Next Survey", "Planned Date of Survey", "Port", "Date of Initial Audit / Anniversary Date", "Date of Last Survey", "Type of Last Audit / Survey", "Remarks" };
        string Sortexpression = "EXCEL";
        string date = DateTime.Now.ToShortDateString();

        int Sortdirection;

        if (ViewState["SORTDIRECTION"] != null)
            Sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            Sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds;
        if (Request.QueryString["FMSYN"] == null)
        {
            ds = PhoenixPlannedMaintenanceSurveySchedule.VesselWiseSurveyScheduleSearch(General.GetNullableInteger(ViewState["CertificateId"].ToString())
               , General.GetNullableString(Sortexpression)
               , Sortdirection
               , 1
               , iRowCount
               , ref iRowCount
               , ref iTotalPageCount
               );
        }
        else
        {
            ds = PhoenixPlannedMaintenanceSurveySchedule.VesselWiseSurveyScheduleSearch(General.GetNullableInteger(ViewState["CertificateId"].ToString())
                 , General.GetNullableString(Sortexpression)
                 , Sortdirection
                 , gvSurvey.CurrentPageIndex + 1
                 , gvSurvey.PageSize
                 , ref iRowCount
                 , ref iTotalPageCount
                 , int.Parse(ViewState["VESSELID"].ToString())
                 );

        }
        General.ShowExcel("Vessel Wise Certificate Details", ds.Tables[0], alColumns, alCaptions, null, "");
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FlDVESSELNAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDNEXTDUEDATE", "FLDSURVEYTYPENAME", "FLDPLANDATE", "FLDSEAPORTNAME", "FLDANNIVERSARYDATE", "FLDLASTSURVEYDATE", "FLDLASTSURVEYTYPENAME", "FLDCERTIFICATEREMARKS" };
            string[] alCaptions = { "Vessel", "IssueD", "Expiry", "Issued By", "Due Date for Survey / Follow up", "Type of Next Survey", "Planned Date of Survey", "Port", "Date of Initial Audit / Anniversary Date", "Date of Last Survey", "Type of Last Audit / Survey", "Remarks" };

            int Sortdirection;
            string Sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                Sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            else
                Sortdirection = 0;
            DataSet ds;
            if (Request.QueryString["FMSYN"] == null)
            {
                ds = PhoenixPlannedMaintenanceSurveySchedule.VesselWiseSurveyScheduleSearch(General.GetNullableInteger(ViewState["CertificateId"].ToString())

               , General.GetNullableString(Sortexpression)
               , Sortdirection
               , gvSurvey.CurrentPageIndex + 1
               , gvSurvey.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );
            }
            else
            {
                ds = PhoenixPlannedMaintenanceSurveySchedule.VesselWiseSurveyScheduleSearch(General.GetNullableInteger(ViewState["CertificateId"].ToString())
                 , General.GetNullableString(Sortexpression)
                 , Sortdirection
                 , gvSurvey.CurrentPageIndex + 1
                 , gvSurvey.PageSize
                 , ref iRowCount
                 , ref iTotalPageCount
                 , int.Parse(ViewState["VESSELID"].ToString())
             );
            }
            General.SetPrintOptions("gvSurvey", "Certificates", alCaptions, alColumns, ds);

            gvSurvey.DataSource = ds;
            gvSurvey.VirtualItemCount = iRowCount;


            if (ds.Tables[0].Rows.Count > 0)
            {
                txtCertificateName.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATENAME"].ToString();
                txtCertificateCategory.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNAME"].ToString();
            }

            ViewState["ROWCOUNT"] = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSurveySchedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurvey_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvSurvey_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarks");

                UserControlToolTip ucRemarks = item["Data"].FindControl("ucRemarks") as UserControlToolTip;
                if (lblRemarks != null)
                {
                    lblRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucRemarks.ToolTip + "', 'visible');");
                    lblRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucRemarks.ToolTip + "', 'hidden');");
                }

                ImageButton cmdSvyAtt = item["Data"].FindControl("cmdSvyAtt") as ImageButton;
                if (cmdSvyAtt != null)
                {
                    if (drv["FLDDTKEY"].ToString() == string.Empty) cmdSvyAtt.Visible = false;
                    if (drv["FLDATTACHMENTYN"].ToString() == "0")
                    {
                        cmdSvyAtt.ImageUrl = Session["images"] + "/no-attachment.png";
                    }
                    if (ViewState["CATEGORY"].ToString() == "SC")
                        cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=STATUTORYCERTIFICATE&u=n'); return false;");
                    else
                        cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCERTIFICATE&u=n'); return false;");
                }

                if (drv["FLDSURVEYDUE"].ToString() == "1")
                {
                    item["WinRange"].Attributes.Add("style", "background-color:#6fb76f;font-weight:bold;");
                }
                else if (drv["FLDSURVEYDUE"].ToString() == "2")
                {
                    item["WinRange"].Attributes.Add("style", "background-color:#FFFFAA");
                }
                else if (drv["FLDSURVEYDUE"].ToString() == "3")
                {
                    item["WinRange"].Attributes.Add("style", "background-color:#FF8000");
                }
                else if (drv["FLDSURVEYDUE"].ToString() == "4")
                {
                    item["WinRange"].Attributes.Add("style", "background-color:#ff4d4d");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
