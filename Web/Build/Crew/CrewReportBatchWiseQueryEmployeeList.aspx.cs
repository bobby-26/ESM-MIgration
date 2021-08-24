using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportBatchWiseQueryEmployeeList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportBatchWiseQueryEmployeeList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvBatch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["BatchId"] = string.IsNullOrEmpty(Request.QueryString["BatchId"]) ? "" : Request.QueryString["BatchId"];
                ViewState["CourseId"] = string.IsNullOrEmpty(Request.QueryString["CourseId"]) ? "" : Request.QueryString["CourseId"];
                gvBatch.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //ShowReport();
         
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
            string date = DateTime.Now.ToShortDateString();


            string[] alColumns = { "FLDROWNUMBER", "FLDBATCH", "FLDFILENO", "FLDNAME", "FLDOALLRANK", "FLDCERTDATE", "FLDRANKNAME", "FLDSTATUS", "FLDDATEOFJOINING", "FLDLASTVESSELNAME", "FLDSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDSIGNONDATE" };
            string[] alCaptions = { "Sr.No", "Batch", "Emp Code", "Emp Name", "O'all Rank", "Cert Issue Date", "Present Rank", "Status", "First Join Date", "Last Vessel", "Sign Off Date", "Present Vessel", "Sign On Date" };
            
            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentCrewBatchWiseQueryReportFilters;
            DataSet ds = new DataSet();
            ds = PhoenixCrewBatchWiseQuery.CrewBatchWiseQueryEmployeeList(General.GetNullableInteger(ViewState["BatchId"].ToString())
                    , General.GetNullableInteger(ViewState["CourseId"].ToString())
                    , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("ucPool")) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("ucZone")) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("lstStatus")) : null)
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=Participants.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Participants</center></h5></td></tr>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
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
                    Response.Write("<td align='left'>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.Write(string.IsNullOrEmpty(ConfigurationManager.AppSettings["softwarename"]) ? "" : ConfigurationManager.AppSettings["softwarename"]);
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDBATCH", "FLDFILENO", "FLDNAME","FLDOALLRANK", "FLDCERTDATE", "FLDRANKNAME", "FLDSTATUS", "FLDDATEOFJOINING", "FLDLASTVESSELNAME", "FLDSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDSIGNONDATE"};
            string[] alCaptions = { "Sr.No", "Batch", "Emp Code", "Emp Name","O'all Rank", "Cert Issue Date", "Present Rank", "Status", "First Join Date", "Last Vessel", "Sign Off Date", "Present Vessel", "Sign On Date"};
            
            NameValueCollection nvc = Filter.CurrentCrewBatchWiseQueryReportFilters;
            DataSet ds = new DataSet();

            ds = PhoenixCrewBatchWiseQuery.CrewBatchWiseQueryEmployeeList(General.GetNullableInteger(ViewState["BatchId"].ToString())
                 , General.GetNullableInteger(ViewState["CourseId"].ToString())
                 , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                 , (nvc != null ? General.GetNullableString(nvc.Get("ucPool")) : null)
                 , (nvc != null ? General.GetNullableString(nvc.Get("ucZone")) : null)
                 , (nvc != null ? General.GetNullableString(nvc.Get("lstStatus")) : null)
                 , gvBatch.CurrentPageIndex+1
                 , gvBatch.PageSize
                 , ref iRowCount
                 , ref iTotalPageCount);

            General.SetPrintOptions("gvBatch", "Participants", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtCourse.Text = ds.Tables[0].Rows[0]["FLDCOURSE"].ToString();

                gvBatch.DataSource = ds;
                gvBatch.VirtualItemCount = iRowCount;
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                    ShowExcel();
                }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvBatch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            {
                if (e.CommandName.ToUpper().Equals("PAGE"))
                    ViewState["PAGENUMBER"] = null;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBatch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBatch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBatch.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}