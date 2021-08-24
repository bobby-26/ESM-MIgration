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
public partial class Crew_CrewReportLongServiceRecord : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "GO",ToolBarDirection.Right);
            MenuLongServiceReport.AccessRights = this.ViewState;
            MenuLongServiceReport.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            
            toolbar.AddFontAwesomeButton("../Crew/CrewReportLongServiceRecord.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            
            toolbar.AddFontAwesomeButton("../Crew/CrewReportLongServiceRecord.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            
            MenuLongServiceRecord.AccessRights = this.ViewState;
            MenuLongServiceRecord.MenuList = toolbar.Show();

            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ucToDate.Text = DateTime.Now.ToShortDateString();
                gvLongServiceRecord.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuLongServiceReport_TabStripCommand(object sender, EventArgs e)
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
            ViewState["PAGENUMBER"] = 1;
            gvLongServiceRecord.CurrentPageIndex = 0;
            BindData();
            gvLongServiceRecord.Rebind();
        }

    }

    protected void BindLongServiceData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = {  "FLDROW","FLDNAME","FLDRANK","FLDFILENO"
								 ,"FLDTRAININGBATCH","FLDDATEOFBIRTH","FLDDDATEOFJOIN","FLDONBOARDDURATION","FLDEMPLOYMENTDURATION"};
            string[] alCaptions = { "SlNo", "Name", "Rank", "File No", "Batch", "Date Of Birth", "Date Of 1st Join", "OnBoard Duration", "Employment Duration" };
            string sortexpression;

            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


            DataSet ds = new DataSet();
            if (ucFromDate.Text == null)
            {
                ds = PhoenixCrewReportLongServiceRecord.CrewReportMISLongServiceRecord(
                                      (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                                      (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                                      (ucRank.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
                                      (ucBatch.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatch.SelectedList.ToString()),
                                      General.GetNullableDateTime(null),
                                      General.GetNullableDateTime(null),
                                      General.GetNullableInteger(ucPrincipal.SelectedAddress),
                                      General.GetNullableInteger(ucManager.SelectedAddress),
                                      General.GetNullableInteger(ddlSelect.SelectedValue.ToString()),
                                      General.GetNullableInteger(ucMonths.Text),
                                      General.GetNullableInteger(ucContracts.Text),
                                      sortexpression, sortdirection,
                                      Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                      gvLongServiceRecord.PageSize,
                                      ref iRowCount,
                                      ref iTotalPageCount
                                    );

            }
            else
            {
                ds = PhoenixCrewReportLongServiceRecord.CrewReportMISLongServiceRecord(
                                      (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                                      (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                                      (ucRank.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
                                      (ucBatch.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatch.SelectedList.ToString()),
                                      General.GetNullableDateTime(ucFromDate.Text),
                                      General.GetNullableDateTime(ucToDate.Text),
                                      General.GetNullableInteger(ucPrincipal.SelectedAddress),
                                      General.GetNullableInteger(ucManager.SelectedAddress),
                                      General.GetNullableInteger(ddlSelect.SelectedValue.ToString()),
                                      General.GetNullableInteger(ucMonths.Text),
                                      General.GetNullableInteger(ucContracts.Text),
                                      sortexpression, sortdirection,
                                      Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                      gvLongServiceRecord.PageSize,
                                      ref iRowCount,
                                      ref iTotalPageCount
                                    );
            } 
            General.SetPrintOptions("gvLongServiceRecord", "Employee Long Service Record", alCaptions, alColumns, ds);

            gvLongServiceRecord.DataSource = ds;
            gvLongServiceRecord.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowStatisticsExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        string[] alColumns = {  "FLDROW","FLDNAME","FLDRANK","FLDFILENO"
								 ,"FLDTRAININGBATCH","FLDDATEOFBIRTH","FLDDDATEOFJOIN","FLDONBOARDDURATION","FLDEMPLOYMENTDURATION"};
        string[] alCaptions = { "SlNo", "Name", "Rank", "File No", "Batch", "Date Of Birth", "Date Of 1st Join", "OnBoard Duration", "Employment Duration" };
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();
        if (ucFromDate.Text == null)
        {
            ds = PhoenixCrewReportLongServiceRecord.CrewReportMISLongServiceRecord(
                                  (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                                  (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                                  (ucRank.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
                                  (ucBatch.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatch.SelectedList.ToString()),
                                  General.GetNullableDateTime(null),
                                  General.GetNullableDateTime(null),
                                  General.GetNullableInteger(ucPrincipal.SelectedAddress),
                                  General.GetNullableInteger(ucManager.SelectedAddress),
                                  General.GetNullableInteger(ddlSelect.SelectedValue.ToString()),
                                  General.GetNullableInteger(ucMonths.Text),
                                  General.GetNullableInteger(ucContracts.Text),
                                  sortexpression, sortdirection,
                                  1,
                                  iRowCount,
                                  ref iRowCount,
                                  ref iTotalPageCount
                                );

        }
        else
        {
            ds = PhoenixCrewReportLongServiceRecord.CrewReportMISLongServiceRecord(
                                  (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                                  (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                                  (ucRank.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
                                  (ucBatch.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatch.SelectedList.ToString()),
                                  General.GetNullableDateTime(ucFromDate.Text),
                                  General.GetNullableDateTime(ucToDate.Text),
                                  General.GetNullableInteger(ucPrincipal.SelectedAddress),
                                  General.GetNullableInteger(ucManager.SelectedAddress),
                                  General.GetNullableInteger(ddlSelect.SelectedValue.ToString()),
                                  General.GetNullableInteger(ucMonths.Text),
                                  General.GetNullableInteger(ucContracts.Text),
                                  sortexpression, sortdirection,
                                  1,
                                  iRowCount,
                                  ref iRowCount,
                                  ref iTotalPageCount
                                );
        }
        Response.AddHeader("Content-Disposition", "attachment; filename=CrewEmployeeGrowthReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Employee Long Service Report</center></h5></td></tr>");
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
        BindLongServiceData();
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
                ucError.ErrorMessage = "Test From Date  should be earlier than current date.";
            }
        }
        if (General.GetNullableDateTime(testtodate) == null)
        {
            ucError.ErrorMessage = "To Date is required.";
        }


        return (!ucError.IsError);
    }

    protected void MenuLongServiceRecord_TabStripCommand(object sender, EventArgs e)
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
                ucManager.SelectedAddress = "";
                ddlSelect.SelectedValue = "";
                ucRank.SelectedRankValue = "";
                ucFromDate.Text = "";
                ucToDate.Text = "";
                ViewState["PAGENUMBER"] = 1;
                gvLongServiceRecord.CurrentPageIndex = 0;
                BindData();
                gvLongServiceRecord.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvLongServiceRecord_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvLongServiceRecord_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        
    }

  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();

    }
    protected void gvLongServiceRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLongServiceRecord.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

