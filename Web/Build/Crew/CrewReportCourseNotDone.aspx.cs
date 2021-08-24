using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReportCourseNotDone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "GO", ToolBarDirection.Right);
            MenuCourseNotDoneReport.AccessRights = this.ViewState;
            MenuCourseNotDoneReport.MenuList = toolbar.Show();

            PhoenixToolbar toolbar2 = new PhoenixToolbar();

            toolbar2.AddFontAwesomeButton("../Crew/CrewReportCourseNotDone.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar2.AddFontAwesomeButton("javascript:CallPrint('gvCrewCourseNotDone')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar2.AddFontAwesomeButton("../Crew/CrewReportCourseNotDone.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar2.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DOCTYPE"] = "";
                ViewState["RANK"] = "";
                ViewState["VESSELTYPE"] = "";
                gvCrewCourseNotDone.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindCourse();
            }
            //BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCourse()
    {
        ddlCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(ViewState["DOCTYPE"].ToString()));
        ddlCourse.DataTextField = "FLDCOURSE";
        ddlCourse.DataValueField = "FLDDOCUMENTID";
        ddlCourse.DataBind();
    }

    protected void DocumentTypeSelection(object sender, EventArgs e)
    {
        ViewState["DOCTYPE"] = ucDocumentType.SelectedHard;
        BindCourse();
    }

    protected void ucPrincipal_TextChangedEvent(object sender, EventArgs e)
    {
        StringBuilder strVesselType = new StringBuilder();
        ucPrincipal.SelectedAddress = General.GetNullableInteger(ucPrincipal.SelectedAddress) == null ? "" : ucPrincipal.SelectedAddress;
        ucVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, General.GetNullableString(ucPrincipal.SelectedAddress), 0, lstVesselType.SelectedVesseltype);
    }

    private bool IsValidCourseFilter(string course)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(course) && txtFileNo.Text == "" && ucSignonFromDate.Text == null && ucSignonToDate.Text == null)
            ucError.ErrorMessage = "Course is required.";

        return (!ucError.IsError);
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROW", "FLDFILENO", "FLDBATCH","FLDEMPLOYEENAME", "FLDRANKNAME","FLDFIRSTJOINDATE", "FLDZONE",  "FLDPRESENTVESSEL",
                                     "FLDSIGNONDATE","FLDLASTVESSEL", "FLDLASTSIGNOFFDATE" };
            string[] alCaptions = { "Sl No.", "File No", "Batch", "Name", "Rank", "1st join dt", "Zone", "Onboard", "S/on Date", "Last Vessel", "S/off Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string coursetype = "";
            if (General.GetNullableInteger(ddlCourse.SelectedValue) != null)
            {
                DataSet ds1 = PhoenixRegistersDocumentCourse.EditDocumentCourse(int.Parse(ddlCourse.SelectedValue));

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    coursetype = ds1.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
                }
            }

            DataSet ds = new DataSet();

            if (coursetype == PhoenixCommonRegisters.GetHardCode(1, 103, "3")) //Presea courses
            {

                ds = PhoenixCrewReports.GeneratePreseaCourseNotDone(General.GetNullableInteger(ddlCourse.SelectedValue)
                                   , General.GetNullableString(ucRank.selectedlist)
                                   , General.GetNullableString(ucVessel.SelectedVessel)
                                   , General.GetNullableString(lstVesselType.SelectedVesseltype)
                                   , General.GetNullableInteger(ucPrincipal.SelectedAddress)
                                   , General.GetNullableString(ucBatch.SelectedList)
                                   , General.GetNullableInteger(ddlStatus.SelectedValue)
                                   , sortexpression, sortdirection
                                   , (int)ViewState["PAGENUMBER"]
                                   , gvCrewCourseNotDone.PageSize
                                   , ref iRowCount
                                   , ref iTotalPageCount
                                   , General.GetNullableString(txtFileNo.Text)
                                   , General.GetNullableDateTime(ucSignonFromDate.Text)
                                   , General.GetNullableDateTime(ucSignonToDate.Text)
                                   , General.GetNullableDateTime(ucSignoffFromDate.Text)
                                   , General.GetNullableDateTime(ucSignoffToDate.Text)
                                   , General.GetNullableString(ucPool.SelectedPool)
                                   , General.GetNullableInteger(ucManager.SelectedAddress)
                                   , General.GetNullableString(ucZone.selectedlist)
                                   , chkIncludeNewApp.Checked == true ? 1 : 0
                                   );
            }
            else
            {
                ds = PhoenixCrewReports.GenerateCourseNotDone(General.GetNullableInteger(ddlCourse.SelectedValue)
                                   , General.GetNullableString(ucRank.selectedlist)
                                   , General.GetNullableString(ucVessel.SelectedVessel)
                                   , General.GetNullableString(lstVesselType.SelectedVesseltype)
                                   , General.GetNullableInteger(ucPrincipal.SelectedAddress)
                                   , General.GetNullableString(ucBatch.SelectedList)
                                   , General.GetNullableInteger(ddlStatus.SelectedValue)
                                   , sortexpression, sortdirection
                                   , (int)ViewState["PAGENUMBER"]
                                   , gvCrewCourseNotDone.PageSize
                                   , ref iRowCount
                                   , ref iTotalPageCount
                                   , General.GetNullableString(txtFileNo.Text)
                                   , General.GetNullableDateTime(ucSignonFromDate.Text)
                                   , General.GetNullableDateTime(ucSignonToDate.Text)
                                   , General.GetNullableDateTime(ucSignoffFromDate.Text)
                                   , General.GetNullableDateTime(ucSignoffToDate.Text)
                                   , General.GetNullableString(ucPool.SelectedPool)
                                   , General.GetNullableInteger(ucManager.SelectedAddress)
                                   , General.GetNullableString(ucZone.selectedlist)
                                   , chkIncludeNewApp.Checked == true ? 1 : 0
                                   );
            }

            General.SetPrintOptions("gvCrewCourseNotDone", "Crew Course Not Done", alCaptions, alColumns, ds);
            
            gvCrewCourseNotDone.DataSource = ds.Tables[0];
            gvCrewCourseNotDone.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            

            if (ddlStatus.SelectedValue == "1")
            {
                gvCrewCourseNotDone.Columns[9].Visible = false;
                gvCrewCourseNotDone.Columns[10].Visible = false;
            }

            if (ddlStatus.SelectedValue == "0")
            {
                gvCrewCourseNotDone.Columns[7].Visible = false;
                gvCrewCourseNotDone.Columns[8].Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROW", "FLDFILENO", "FLDBATCH","FLDEMPLOYEENAME", "FLDRANKNAME","FLDFIRSTJOINDATE", "FLDZONE",  "FLDPRESENTVESSEL",
                                     "FLDSIGNONDATE","FLDLASTVESSEL", "FLDLASTSIGNOFFDATE" };
            string[] alCaptions = { "Sl No.", "File No", "Batch", "Name", "Rank", "1st join dt", "Zone", "Onboard", "S/on Date", "Last Vessel", "S/off Date" };

            string[] FilterColumns = { "FLDCOURSENAME", "FLDSELECTEDFILENO", "FLDSTATUS", "FLDPRINCIPAL", "FLDVESSELTYPES", "FLDVESSELNAMES", "FLDRANKNAMES", "FLDBATCH" };
            string[] FilterCaptions = { "Course", "File No", "Status", "Principal", "Vessel Type", "Vessel", "Rank", "Batch" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string coursetype = "";
            if (General.GetNullableInteger(ddlCourse.SelectedValue) != null)
            {
                DataSet ds1 = PhoenixRegistersDocumentCourse.EditDocumentCourse(int.Parse(ddlCourse.SelectedValue));

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    coursetype = ds1.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
                }
            }

            DataSet ds = new DataSet();

            if (coursetype == PhoenixCommonRegisters.GetHardCode(1, 103, "3")) //Presea courses
            {
                ds = PhoenixCrewReports.GeneratePreseaCourseNotDone(General.GetNullableInteger(ddlCourse.SelectedValue)
                                  , General.GetNullableString(ucRank.selectedlist)
                                  , General.GetNullableString(ucVessel.SelectedVessel)
                                  , General.GetNullableString(lstVesselType.SelectedVesseltype)
                                  , General.GetNullableInteger(ucPrincipal.SelectedAddress)
                                  , General.GetNullableString(ucBatch.SelectedList)
                                  , General.GetNullableInteger(ddlStatus.SelectedValue)
                                  , sortexpression, sortdirection
                                  , 1
                                  , iRowCount
                                  , ref iRowCount
                                  , ref iTotalPageCount
                                  , General.GetNullableString(txtFileNo.Text)
                                   , General.GetNullableDateTime(ucSignonFromDate.Text)
                                   , General.GetNullableDateTime(ucSignonToDate.Text)
                                   , General.GetNullableDateTime(ucSignoffFromDate.Text)
                                   , General.GetNullableDateTime(ucSignoffToDate.Text)
                                   , General.GetNullableString(ucPool.SelectedPool)
                                   , General.GetNullableInteger(ucManager.SelectedAddress)
                                   , General.GetNullableString(ucZone.selectedlist)
                                   , chkIncludeNewApp.Checked == true ? 1 : 0
                                  );
            }
            else
            {
                ds = PhoenixCrewReports.GenerateCourseNotDone(General.GetNullableInteger(ddlCourse.SelectedValue)
                                  , General.GetNullableString(ucRank.selectedlist)
                                  , General.GetNullableString(ucVessel.SelectedVessel)
                                  , General.GetNullableString(lstVesselType.SelectedVesseltype)
                                  , General.GetNullableInteger(ucPrincipal.SelectedAddress)
                                  , General.GetNullableString(ucBatch.SelectedList)
                                  , General.GetNullableInteger(ddlStatus.SelectedValue)
                                  , sortexpression, sortdirection
                                  , 1
                                  , iRowCount
                                  , ref iRowCount
                                  , ref iTotalPageCount
                                  , General.GetNullableString(txtFileNo.Text)
                                   , General.GetNullableDateTime(ucSignonFromDate.Text)
                                   , General.GetNullableDateTime(ucSignonToDate.Text)
                                   , General.GetNullableDateTime(ucSignoffFromDate.Text)
                                   , General.GetNullableDateTime(ucSignoffToDate.Text)
                                   , General.GetNullableString(ucPool.SelectedPool)
                                   , General.GetNullableInteger(ucManager.SelectedAddress)
                                   , General.GetNullableString(ucZone.selectedlist)
                                   , chkIncludeNewApp.Checked == true ? 1 : 0
                                  );
            }


            Response.AddHeader("Content-Disposition", "attachment; filename=CrewCourseNotDoneReport.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Course Not Done Report</center></h5></td></tr>");
            //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");

            General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);

            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true; throw;
        }
    }

    protected void MenuCourseNotDoneReport_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            if (!IsValidCourseFilter(General.GetNullableString(ddlCourse.SelectedValue)))
            {
                ucError.Visible = true;
                return;
            }
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvCrewCourseNotDone.Rebind();
        }

    }

    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                if (!IsValidCourseFilter(General.GetNullableString(ddlCourse.SelectedValue)))
                {
                    ucError.Visible = true;
                    return;
                }
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                string empty = General.GetNullableString("");
                ucRank.selectedlist = "";
                ucVessel.SelectedVessel = "";
                lstVesselType.SelectedVesseltype = "";
                ucPrincipal.SelectedAddress = "";
                ucBatch.SelectedList = "";
                ddlStatus.SelectedIndex = 0;
                txtFileNo.Text = "";
                ucDocumentType.SelectedHard = "";
                ucPool.SelectedPool = "";
                ucManager.SelectedAddress = "";
                ViewState["DOCTYPE"] = ucDocumentType.SelectedHard;
                BindCourse();
                ddlCourse.SelectedIndex = 0;
                BindData();
                gvCrewCourseNotDone.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlCourse_DataBound(object sender, EventArgs e)
    {
        ddlCourse.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void gvCrewCourseNotDone_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCourseNotDone_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblCrewId = (RadLabel)e.Item.FindControl("lblCrewId");
            RadLabel lb = (RadLabel)e.Item.FindControl("lnkCrew");
            lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblCrewId.Text + "'); return false;");
        }
    }

    protected void gvCrewCourseNotDone_SelectedIndexChanging(object sender, GridSelectCommandEventArgs e)
    {
        BindData();
        // SetPageNavigator();
    }

    protected void gvCrewCourseNotDone_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvCrewCourseNotDone_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCourseNotDone.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}