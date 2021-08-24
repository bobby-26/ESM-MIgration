using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class CrewReportsRetention : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsRetention.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsRetention.aspx", "Retention Detail", "<i class=\"fas fa-file-excel\"></i>", "ExcelData");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsRetention.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                Guidlines();
                int year = DateTime.Now.Year;
                txtFromDate.Text = new DateTime(year, 1, 1).ToString();
                txtToDate.Text = DateTime.Now.ToString();
               
            }
            gvCrew.PageSize = 10000;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Guidlines()
    {
        string text = "<table style=\"border-collapse:collapse;\" border=\"1\"> <tr><td align=\"right\">1</td><td>BI</td><td>Begin Inventory</td></tr></tr>";
        text += " <tr><td align=\"right\">2</td><td>EI</td><td>End Inventory</td></tr></tr>";
        text += " <tr><td align=\"right\">3</td><td>AE</td><td>The average number of employees working for the company during the same period as calculated, this should be any period of 12 months. </td></tr></tr>";
        //text += " <tr><td align=\"right\">4</td><td>S</td><td>Total Number of terminations from what ever cause(In effect this means the total number employees that have left the company for what ever reason) </td></tr></tr>";
        text += " <tr><td align=\"right\">4</td><td>S</td><td>Total Number of terminations from what ever cause </td></tr></tr>";
        text += " <tr><td align=\"right\">5</td><td>UT</td><td>Unavoidable Terminations</td></tr></tr>";
        text += " <tr><td align=\"right\">6</td><td>BT</td><td>Beneficial Terminations</td></tr></tr>";
        //text += " <tr><td align=\"right\">5</td><td>UT</td><td>Unavoidable Terminations (i.e. retirements or long term illness)</td></tr></tr>";
        //text += " <tr><td align=\"right\">6</td><td>BT</td><td>Beneficial Terminations (i.e. sometimes those staff that do leave provide benefitto the company by virtue of leaving, for example underperformers </ td></tr></tr>";
        text += " <tr><td align=\"right\">7</td><td>RR</td><td>Officer Retention Rate = 100 - ( ({S- (UT + BT) } / AE) * 100 )</td></tr></tr>";
        //text += " <tr><td align=\"right\">7</td><td>RR</td><td>Officer Retention Rate = 100 - ( (S / AE) * 100 )</td></tr></tr>";
        text += " </table>";
        ucToolTipNW.Text = text;

        imgnotes.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
        imgnotes.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");
        
        ucToolTipNW.Position = ToolTipPosition.TopCenter;
        ucToolTipNW.TargetControlId = imgnotes.ClientID;
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
                //  Response.ClearContent();
                //  Response.ContentType = "application/ms-excel";
                //  Response.AddHeader("content-disposition", "attachment;filename=Retention_Report.xls");
                //  Response.Charset = "";
                //  System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                //  stringwriter.Write("<table><tr><td>From</td>" +
                //     "<td>" + DateTime.Parse(txtFromDate.Text).ToString("dd-MMMM-yyyy") + "</td><td>To </td><td>" + DateTime.Parse(txtToDate.Text).ToString("dd-MMMM-yyyy") + "</td>" +
                //     "<td colspan=\"4\"></td></tr></table><br/><br/>");
                //  
                //  HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                //  gvCrew.RenderControl(htmlwriter);
                //  Response.Write(stringwriter.ToString());
                //  Response.End();
            }
            else if (CommandName.ToUpper().Equals("EXCELDATA"))
            {
                string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDPOOLNAME", "FLDZONE", "FLDRANK", "FLDPRESENTRANK", "FLDLASTSIGNOFF", "FLDINCLUDE", "FLDLASTSIGNON", "FLDBI", "FLDEI", "FLDISINACTIVE", "FLDUT", "FLDBT" };
                string[] alCaptions = { "File No", "Name", "Pool", "Zone", "Rank", "Present Rank", "Last Sign Off", "Is Included", "Last Sign On", "BI", "EI", "S", "UT", "BT" };

                DataSet ds = PhoenixCrewReportRetentionRate.RetentionSearch(General.GetNullableDateTime(txtFromDate.Text).Value
                                                            , General.GetNullableDateTime(txtToDate.Text).Value
                                                            , (byte?)General.GetNullableInteger(chkShowOther.Checked==true ? "1" : "0")
                                                            , (byte?)General.GetNullableInteger(chkGroup.Checked==true ? "1" : "0")
                                                            , lstPool.SelectedPool
                                                            );
                DataTable dt = ds.Tables[1];
                General.ShowExcel("Retention Report Detail", dt, alColumns, alCaptions, null, null);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                int year = DateTime.Now.Year;
                txtFromDate.Text = new DateTime(year, 1, 1).ToString();
                txtToDate.Text = DateTime.Now.ToString();
                
                lstPool.SelectedPool = "";
                ShowReport();
                gvCrew.SelectedIndexes.Clear();
                gvCrew.EditIndexes.Clear();
                gvCrew.DataSource = null;
                gvCrew.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {                
                if(!IsValidFilter(txtFromDate.Text,txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                ShowReport();                
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
              
        string[] alColumns = { "Rank", "BI", "EI", "AE", "S", "UT", "BT", "RR" };
        string[] alCaptions = { "Rank", "BI", "EI", "AE", "S", "UT", "BT", "RR" };

        DataSet ds = PhoenixCrewReportRetentionRate.RetentionSearch(General.GetNullableDateTime(txtFromDate.Text).Value
                                                            , General.GetNullableDateTime(txtToDate.Text).Value
                                                            , (byte?)General.GetNullableInteger(chkShowOther.Checked == true ? "1" : "0")
                                                            , (byte?)General.GetNullableInteger(chkGroup.Checked == true ? "1" : "0")
                                                            , lstPool.SelectedPool     );

        DataTable dt = ds.Tables[0];
        General.ShowExcel("Retention Report", dt, alColumns, alCaptions, null, null);
    }
    private void ShowReport()
    {              
        string[] alColumns = { "Rank", "BI", "EI", "AE", "S", "UT", "BT", "RR" };
        string[] alCaptions = { "Rank", "BI", "EI", "AE", "S", "UT", "BT", "RR" };

        DataSet ds = PhoenixCrewReportRetentionRate.RetentionSearch(General.GetNullableDateTime(txtFromDate.Text).Value
                                                           , General.GetNullableDateTime(txtToDate.Text).Value
                                                           , (byte?)General.GetNullableInteger(chkShowOther.Checked == true ? "1" : "0")
                                                           , (byte?)General.GetNullableInteger(chkGroup.Checked == true ? "1" : "0")
                                                           , lstPool.SelectedPool
                                                           );
        DataTable dt = ds.Tables[0];
        General.SetPrintOptions("gvCrew", "Retention Report", alCaptions, alColumns, ds);
        
        if(dt.Rows.Count > 0)
            gvCrew.DataSource = dt;        
    }
    protected void gvCrew_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            //RadLabel a = (RadLabel)e.Item.FindControl("lblsno");
            //a.Text = (e.Item.ItemIndex + 1).ToString();

            string rank = drv["RANK"].ToString();
            if (drv["FLDGROUPING"].ToString() == "1")
            {
                e.Item.Font.Bold = true;
            }

            // e.Item.Cells[1].Text = (rank.IndexOf("~") >-1 ? "thisis1" : "thisis2");
            string[] d = rank.Split('~');


            RadLabel a = (RadLabel)e.Item.FindControl("lblsno");
            int b = gvCrew.PageSize * (gvCrew.CurrentPageIndex);
            a.Text = (e.Item.ItemIndex + 1 + b).ToString();

            RadLabel c = (RadLabel)e.Item.FindControl("lblrank");
            if (d.Length > 1)
                c.Text = d[1];
            else
                c.Text = d[0];
            //string d = (string)c.Text;
        }
    }
    
    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }

        return (!ucError.IsError);
    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
         //  ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFlagCourseDone.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
    }
}