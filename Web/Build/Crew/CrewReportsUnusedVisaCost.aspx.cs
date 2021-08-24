using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportsUnusedVisaCost : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;

                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                BindYear();
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewReportsUnusedVisaCost.aspx?e=3", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewReportsUnusedVisaCost.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewReportsUnusedVisaCost.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
            MenuVisaCostRecord.AccessRights = this.ViewState;
            MenuVisaCostRecord.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuVisaCost.AccessRights = this.ViewState;
            MenuVisaCost.MenuList = toolbar.Show();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuVisaCost_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            BindData();
            gvCrew.Rebind();
        }
    }
     
    protected void BindLongServiceData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = {  "FLDROW","FLDVESSELNAME","FLDNAME","FLDRANK"
								 ,"FLDVISACOUNTRY","FLDAMOUNT","FLDREMARKS"};
            string[] alCaptions = { "S.No.", "Vessel", "Name", "Rank", "Visa", "Cost", "Remarks"};
            string sortexpression;

            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


            DataSet ds = new DataSet();
            if (ddlMonth.SelectedItem== null)
            {
                
                ds = PhoenixCrewReportUnusedVisaCost.CrewReportUnusedVisaCost(
                                      General.GetNullableInteger(null),
                                      General.GetNullableInteger(null),
                                      sortexpression, sortdirection,
                                      Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                      General.ShowRecords(null),
                                      ref iRowCount,
                                      ref iTotalPageCount
                                    );

            }
            else
            {
                ds = PhoenixCrewReportUnusedVisaCost.CrewReportUnusedVisaCost(
                                  
                                      General.GetNullableInteger(ddlMonth.SelectedValue),
                                      General.GetNullableInteger(ddlYear.SelectedValue),
                                      sortexpression, sortdirection,
                                      Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                      gvCrew.PageSize,
                                      ref iRowCount,
                                      ref iTotalPageCount
                                    );
            }
            General.SetPrintOptions("gvCrew", "Unused Visa Cost", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrew.DataSource = ds;
                gvCrew.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCrew.DataSource = "";
            }
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
        string[] alColumns = {  "FLDROW","FLDVESSELNAME","FLDNAME","FLDRANK" ,"FLDVISACOUNTRY","FLDAMOUNT","FLDREMARKS"};
        string[] alCaptions = { "S.No.", "Vessel", "Name", "Rank", "Visa", "Cost", "Remarks" };
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = new DataSet();


        ds = PhoenixCrewReportUnusedVisaCost.CrewReportUnusedVisaCost(
                              General.GetNullableInteger("1"),
                              General.GetNullableInteger("2012"),
                              sortexpression, sortdirection,
                              Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                              gvCrew.PageSize,
                              ref iRowCount,
                              ref iTotalPageCount
                            );
        Response.AddHeader("Content-Disposition", "attachment; filename=Unused_Visa_Cost.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Unused Visa Cost</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From: " + 1+ " To: " +1+  " </center></h5></td></tr>");
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
    protected void MenuVisaCostRecord_TabStripCommand(object sender, EventArgs e)
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
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
                BindData();
                gvCrew.Rebind();
            }
            else if (CommandName.ToUpper().Equals("GO"))
            {
                BindData();
                gvCrew.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrew.Rebind();
    }
    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            RadComboBoxItem li = new RadComboBoxItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }
    
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName == "Page")
                ViewState["PAGENUMBER"] = null;          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
        BindData();
    }
}
