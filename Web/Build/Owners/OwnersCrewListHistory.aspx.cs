using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.OwnersCrewlist;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.Registers;
using System.Web;
public partial class OwnersCrewListHistory : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCrew.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ViewState["ACCESSTOCREWLISTHISTORY"] = null;
            }
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            checkAccesstoCrewListHistory();
            if (ViewState["ACCESSTOCREWLISTHISTORY"].ToString() == "0")
            {
                MenuReportsFilter.Visible = false;                
            }
            else
            {
                MenuReportsFilter.Visible = true;
                lblNote.Attributes.Add("style", "display:none");
            }
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Owners/OwnersCrewListHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar1.AddImageLink("javascript:CallPrint('gvCrew')", "Print Grid", "icon_print.png", "PRINT");
            toolbar1.AddImageButton("../Owners/OwnersCrewListHistory.aspx", "Clear Filter", "clear-filter.png", "CLEAR");            
            MenuShowExcel.AccessRights = this.ViewState; 
            MenuShowExcel.MenuList = toolbar1.Show();           

            ucDate1.Text = DateTime.Now.ToShortDateString();  
            ShowReport();
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void checkAccesstoCrewListHistory()
    {
        DataSet ds = PhoenixOwnersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        int ownerid = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString()))
                ownerid = int.Parse(ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString());
        }
        if (ownerid != 0)
        {
            DataSet ds1 = PhoenixRegistersOwnerMapping.EditOwnerMapping(ownerid);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (ds1.Tables[0].Rows[0]["FLDVIEWCREWLISTHISTORY"].ToString() == "1")
                    ViewState["ACCESSTOCREWLISTHISTORY"] = 1;
                else
                    ViewState["ACCESSTOCREWLISTHISTORY"] = 0;
            }
            else
                ViewState["ACCESSTOCREWLISTHISTORY"] = 0;
        }
        else
            ViewState["ACCESSTOCREWLISTHISTORY"] = 1;
    }
    
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucDate.Text = null;
                ucVessel.SelectedVessel = "";                
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ShowReport();
                SetPageNavigator();

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
             DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

             if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
             {
                 if (!IsValidFilter(ucVessel.SelectedVessel,ucDate.Text,ucDate1.Text))
                 {
                     ucError.Visible = true;
                     return;
                 }
                 else
                 {
                     ViewState["PAGENUMBER"] = 1;
                     ShowReport();
                     SetPageNavigator();
                 }
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
         int iRowCount = 0;
         int iTotalPageCount = 0;
         string date = DateTime.Now.ToShortDateString();

         DataSet ds = new DataSet();
         string[] alColumns = { "FLDROW", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDDOI", 
                                 "FLDPLACEOFISSUE", "FLDDOE", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSEAPORTNAME" };
         string[] alCaptions = { "Sr.No", "Name", "Emp.No", "Rank", "DOB", "Passport No", "DOI", "POI", "DOE", "Sign On Date", "Relief Due", "Joining Port" };
         string sortexpression;
         int? sortdirection = null;

         sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
         if (ViewState["SORTDIRECTION"] != null)
             sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
         if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
             iRowCount = 10;
         else
             iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
         ds = PhoenixOwnersCrewlist.CrewListSearch(
                        General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                        General.GetNullableDateTime(ucDate.Text),
                        General.GetNullableDateTime(ucDate1.Text),
                        null,null,
                        sortexpression, sortdirection,
                        (int)ViewState["PAGENUMBER"],
                        iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount);

         DataTable dt = ds.Tables[0];
         string fromdate = dt.Rows[0]["FLDFROMPERIOD"].ToString();
         string todate = dt.Rows[0]["FLDTOPERIOD"].ToString();

         Response.AddHeader("Content-Disposition", "attachment; filename=CrewListHistory.xls");
         Response.ContentType = "application/vnd.msexcel";
         Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
         Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"].ToString() + "</center></h5></td></tr>");
         Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew List For Vessel:" + ucVessel.SelectedVesselName.ToString() + "</center></h5></td></tr>");
         Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
         Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
         Response.Write("</tr>");
         Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "' >From: " + fromdate + " To: " + todate + "</td></tr>");
         Response.Write("</TABLE>");
         Response.Write("<br />");
         Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
         Response.Write("<tr>");        
         for (int i = 0; i < alCaptions.Length; i++)
         {
             Response.Write("<td align='left' style='font-family:Arial; font-size:10px;' width='20%'>");
             Response.Write("<b>" + alCaptions[i] + "</b>");
             Response.Write("</td>");
         }
         Response.Write("</tr>");         
         foreach (DataRow dr in ds.Tables[0].Rows)
         {            
             for (int i = 0; i < alColumns.Length; i++)
             {
                 Response.Write("<td align='left' style='font-family:Arial; font-size:10px;'>");
                 Response.Write(dr[alColumns[i]]);
                 Response.Write("</td>");
             }
             Response.Write("</tr>");            
         }
         Response.Write("</TABLE>");
         Response.End();
     }
    private void ShowReport()
     {
         ViewState["SHOWREPORT"] = 1;

         int iRowCount = 0;
         int iTotalPageCount = 0;

         string[] alColumns = { "FLDROW", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDDOI", 
                                 "FLDPLACEOFISSUE", "FLDDOE", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSEAPORTNAME" };
         string[] alCaptions = { "Sr.No", "Name", "Emp.No", "Rank", "DOB", "Passport No", "DOI", "POI", "DOE", "Sign On Date", "Relief Due", "Joining Port" };
         string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
         int? sortdirection = null;
         if (ViewState["SORTDIRECTION"] != null)
             sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

         DataSet ds = PhoenixOwnersCrewlist.CrewListSearch(
                        General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                        General.GetNullableDateTime(ucDate.Text),
                        General.GetNullableDateTime(ucDate1.Text),
                        null, null,
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        General.ShowRecords(null),
                        ref iRowCount,
                        ref iTotalPageCount);

         General.SetPrintOptions("gvCrew", "CrewListHistory", alCaptions, alColumns, ds);

         if (ds.Tables[0].Rows.Count > 0)
         {
             gvCrew.DataSource = ds;
             gvCrew.DataBind();
             ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
         }
         else
         {

             DataTable dt = ds.Tables[0];
             ShowNoRecordsFound(dt, gvCrew);
             ViewState["ROWSINGRIDVIEW"] = 0;
         }
         ViewState["ROWCOUNT"] = iRowCount;
         ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
     }

     protected void cmdGo_Click(object sender, EventArgs e)
     {
         gvCrew.EditIndex = -1;
         gvCrew.SelectedIndex = -1;
         int result;
         if (Int32.TryParse(txtnopage.Text, out result))
         {
             ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

             if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                 ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

             if (0 >= Int32.Parse(txtnopage.Text))
                 ViewState["PAGENUMBER"] = 1;

             if ((int)ViewState["PAGENUMBER"] == 0)
                 ViewState["PAGENUMBER"] = 1;

             txtnopage.Text = ViewState["PAGENUMBER"].ToString();
         }
         ShowReport();
         SetPageNavigator();
     }

     protected void PagerButtonClick(object sender, CommandEventArgs ce)
     {
         gvCrew.SelectedIndex = -1;
         gvCrew.EditIndex = -1;
         if (ce.CommandName == "prev")
             ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
         else
             ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

         ShowReport();
         SetPageNavigator();
     }

     private void SetPageNavigator()
     {
         cmdPrevious.Enabled = IsPreviousEnabled();
         cmdNext.Enabled = IsNextEnabled();
         lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
         lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
         lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
     }

     private Boolean IsPreviousEnabled()
     {
         int iCurrentPageNumber;
         int iTotalPageCount;

         iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
         iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

         if (iTotalPageCount == 0)
             return false;

         if (iCurrentPageNumber > 1)
             return true;

         return false;
     }

     private Boolean IsNextEnabled()
     {
         int iCurrentPageNumber;
         int iTotalPageCount;

         iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
         iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

         if (iCurrentPageNumber < iTotalPageCount)
         {
             return true;
         }
         return false;
     }
     private void ShowNoRecordsFound(DataTable dt, GridView gv)
     {
         dt.Rows.Add(dt.NewRow());
         gv.DataSource = dt;
         gv.DataBind();

         int colcount = gv.Columns.Count;
         gv.Rows[0].Cells.Clear();
         gv.Rows[0].Cells.Add(new TableCell());
         gv.Rows[0].Cells[0].ColumnSpan = colcount;
         gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
         gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
         gv.Rows[0].Cells[0].Font.Bold = true;
         gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
         gv.Rows[0].Attributes["onclick"] = "";
     }
     protected void gvCrew_RowDataBound(object sender, GridViewRowEventArgs e)
     {
         if (e.Row.RowType == DataControlRowType.Header)
         {
             if (ViewState["SORTEXPRESSION"] != null)
             {
                 HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                 if (img != null)
                 {
                     if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                         img.Src = Session["images"] + "/arrowUp.png";
                     else
                         img.Src = Session["images"] + "/arrowDown.png";

                     img.Visible = true;
                 }
             }
         }        
     }
     public bool IsValidFilter(string vessellist,string fromdate,string todate )
     {
         DateTime resultdate;
         ucError.HeaderMessage = "Please provide the following required information";

         if (vessellist.Equals("") || vessellist.Equals("Dummy") || vessellist.Equals(","))
         {
             ucError.ErrorMessage = "Select Vessel";
         }

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
     protected void cmdSort_Click(object sender, EventArgs e)
     {
         ImageButton ib = (ImageButton)sender;
         ShowReport();
         SetPageNavigator();
     }
     protected void gvCrew_Sorting(object sender, GridViewSortEventArgs se)
     {
         gvCrew.EditIndex = -1;
         gvCrew.SelectedIndex = -1;
         ViewState["SORTEXPRESSION"] = se.SortExpression;

         if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
             ViewState["SORTDIRECTION"] = 1;
         else
             ViewState["SORTDIRECTION"] = 0;

         ShowReport();
     }
     protected void gvCrew_RowCommand(object sender, GridViewCommandEventArgs e)
     {
         try
         {
             if (e.CommandName.ToUpper().Equals("SORT"))
                 return;
         }
         catch (Exception ex)
         {
             ucError.ErrorMessage = ex.Message;
             ucError.Visible = true;

         }
     }
}

