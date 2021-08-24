using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewReports;
using Telerik.Web.UI;
public partial class Crew_CrewReportReasonwiseIllnessAndInjury : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "GO", ToolBarDirection.Right);
            MenuReasonwiseReport.AccessRights = this.ViewState;
            MenuReasonwiseReport.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Crew/CrewReportReasonwiseIllnessAndInjury.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("../Crew/CrewReportReasonwiseIllnessAndInjury.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuReasonwiseRecord.AccessRights = this.ViewState;
            MenuReasonwiseRecord.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ucToDate.Text = DateTime.Now.ToShortDateString();
                gvReasonwiseRecord.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //  BindData();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuReasonwiseReport_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            if (!IsValidTestFilter(ucFromDate.Text, ucToDate.Text))
            {
                ucError.Visible = true;
                return;
            }
            ViewState["PAGENUMBER"]=1;
            gvReasonwiseRecord.CurrentPageIndex =0;
            BindData();
            gvReasonwiseRecord.Rebind();
        }

    }
    protected void ucTypeofcase_Changed(object sender, EventArgs e)
    {
        try
        {
            UserControlHard ucCaseType = (UserControlHard)sender;

            if (ucCaseType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 174, "INJ"))
            {
                ucTypeofInjury.Enabled = true;
                ucTypeofInjury.SelectedQuick = string.Empty;
            }
            else
            {
                ucTypeofInjury.Enabled = false;
                ucTypeofInjury.SelectedQuick = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void ShowStatisticsExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        string[] alColumns = { "FLDTYPESOFINJURY", "FLDINJURYCOUNT" };
        string[] alCaptions = { "Reason", "No Of Case" };
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 20;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;

        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        ucPort.SelectedPort = ucPort.SelectedPort.ToString().Contains("Dummy,") ? ucPort.SelectedPort.ToString().Replace("Dummy,", "") : ucPort.SelectedPort;
        ucPrincipal.SelectedList = ucPrincipal.SelectedList.ToString().Contains("Dummy,") ? ucPrincipal.SelectedList.ToString().Replace("Dummy,", "") : ucPrincipal.SelectedList;

        DataSet ds = new DataSet();
        if (ucFromDate.Text == null)
        {
            ds = PhoenixCrewReportReasonwiseIllnessAndInjury.CrewReportReasonwiseIllnessAndInjury(
                                  (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                                  (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                                  (ucPort.SelectedPort) == "," ? null : General.GetNullableString(ucPort.SelectedPort.ToString()),
                                  General.GetNullableDateTime(null),
                                  General.GetNullableDateTime(null),
                                  sortexpression, sortdirection,
                                  1,
                                  iRowCount,
                                  ref iRowCount,
                                  ref iTotalPageCount
                                  , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                  , (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList)
                                  , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                  , General.GetNullableInteger(ucTypeofInjury.SelectedQuick)
                                  , General.GetNullableInteger(ucTypeofcase.SelectedHard)
                                );

        }
        else
        {
            ds = PhoenixCrewReportReasonwiseIllnessAndInjury.CrewReportReasonwiseIllnessAndInjury(
                                 (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                                 (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                                 (ucPort.SelectedPort) == "," ? null : General.GetNullableString(ucPort.SelectedPort.ToString()),
                                 General.GetNullableDateTime(ucFromDate.Text),
                                 General.GetNullableDateTime(ucToDate.Text),
                                 sortexpression, sortdirection,
                                 1,
                                 iRowCount,
                                 ref iRowCount,
                                 ref iTotalPageCount
                                  , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                  , (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList)
                                  , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                  , General.GetNullableInteger(ucTypeofInjury.SelectedQuick)
                                  , General.GetNullableInteger(ucTypeofcase.SelectedHard)
                               );

        }
        Response.AddHeader("Content-Disposition", "attachment; filename=CrewEmployeeGrowthReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Reasonwise Illness/Injury Report</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From: " + ucFromDate.Text + " To: " + ucToDate.Text + " </center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>As of Date:" + DateTime.Now.ToShortDateString() + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");


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
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDTYPESOFINJURY", "FLDINJURYCOUNT" };
            string[] alCaptions = { "Reason", "No Of Case" };
            string sortexpression;

            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
            ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
            ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;

            ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
            ucPort.SelectedPort = ucPort.SelectedPort.ToString().Contains("Dummy,") ? ucPort.SelectedPort.ToString().Replace("Dummy,", "") : ucPort.SelectedPort;
            ucPrincipal.SelectedList = ucPrincipal.SelectedList.ToString().Contains("Dummy,") ? ucPrincipal.SelectedList.ToString().Replace("Dummy,", "") : ucPrincipal.SelectedList;

            DataSet ds = new DataSet();
            if (ucFromDate.Text == null)
            {
                ds = PhoenixCrewReportReasonwiseIllnessAndInjury.CrewReportReasonwiseIllnessAndInjury(
                                      (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                                      (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                                      (ucPort.SelectedPort) == "," ? null : General.GetNullableString(ucPort.SelectedPort.ToString()),
                                      General.GetNullableDateTime(null),
                                      General.GetNullableDateTime(null),
                                      sortexpression, sortdirection,
                                      Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                      gvReasonwiseRecord.PageSize,
                                      ref iRowCount,
                                      ref iTotalPageCount
                                      , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                      , (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList)
                                      , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                      , General.GetNullableInteger(ucTypeofInjury.SelectedQuick)
                                      , General.GetNullableInteger(ucTypeofcase.SelectedHard)
                                    );

            }
            else
            {
                ds = PhoenixCrewReportReasonwiseIllnessAndInjury.CrewReportReasonwiseIllnessAndInjury(
                                     (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                                     (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                                     (ucPort.SelectedPort) == "," ? null : General.GetNullableString(ucPort.SelectedPort.ToString()),
                                     General.GetNullableDateTime(ucFromDate.Text),
                                      General.GetNullableDateTime(ucToDate.Text),
                                     sortexpression, sortdirection,
                                     Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                     gvReasonwiseRecord.PageSize,
                                     ref iRowCount,
                                     ref iTotalPageCount
                                      , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                      , (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList)
                                      , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                      , General.GetNullableInteger(ucTypeofInjury.SelectedQuick)
                                      , General.GetNullableInteger(ucTypeofcase.SelectedHard)
                                   );

            }
            General.SetPrintOptions("gvReasonwiseRecord", "Reason Wise Illness And Injury Record", alCaptions, alColumns, ds);

            gvReasonwiseRecord.DataSource = ds;
            gvReasonwiseRecord.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindListBox(ListBox lstBox, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (lstBox.Items.FindByValue(item) != null)
                    lstBox.Items.FindByValue(item).Selected = true;
            }
        }
    }
    private bool IsValidTestFilter(string testfromdate, string testtodate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if (General.GetNullableDateTime(testfromdate) == null)
        {
            ucError.ErrorMessage = "From Date is required.";
        }

        else if (!string.IsNullOrEmpty(testfromdate))
        {
            if (DateTime.TryParse(testfromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "From Date  should be earlier than current date.";
            }
        }
        if (General.GetNullableDateTime(testtodate) == null)
        {
            ucError.ErrorMessage = "To Date is required.";
        }


        return (!ucError.IsError);
    }
    protected void MenuReasonwiseRecord_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowStatisticsExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {

                ucPool.SelectedPool = "";
                ucZone.selectedlist = "";
                ucPort.SelectedPort = "";

                ucFromDate.Text = "";
                ucToDate.Text = "";
                ucTypeofcase.SelectedHard = "";
                ucTypeofInjury.SelectedQuick = "";
                gvReasonwiseRecord.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvReasonwiseRecord.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvReasonwiseRecord_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvReasonwiseRecord_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkNoofcase");
            RadLabel reasoncode = (RadLabel)e.Item.FindControl("lblinjurytypecode");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewReportReasonwiseIllnessAndInjuryDetails.aspx?Reason=" + General.GetNullableString(reasoncode.Text) + "&port=" + General.GetNullableString(ucPort.SelectedPort) + "&pool=" + General.GetNullableString(ucPool.SelectedPool) + "&zone=" + General.GetNullableString(ucZone.selectedlist) + "&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&RankList=" + General.GetNullableString(ucRank.selectedlist) + "&Principal=" + General.GetNullableString(ucPrincipal.SelectedList) + "&VesselType=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&InjuryType=" + General.GetNullableString(ucTypeofInjury.SelectedQuick) + "&MedicalCase=" + General.GetNullableString(ucTypeofcase.SelectedHard) + "'); return false;");
        }
    }    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();     
    }
    protected void gvReasonwiseRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReasonwiseRecord.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}