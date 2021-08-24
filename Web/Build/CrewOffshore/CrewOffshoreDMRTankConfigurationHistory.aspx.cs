using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewOffshoreDMRTankConfigurationHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRTankConfigurationHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTankHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuTankHistory.AccessRights = this.ViewState;
            MenuTankHistory.MenuList = toolbar.Show();

            PhoenixToolbar toolbarclean = new PhoenixToolbar();
            toolbarclean.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRTankConfigurationHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarclean.AddFontAwesomeButton("javascript:CallPrint('gvTankCleaned')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuTankCleaned.AccessRights = this.ViewState;
            MenuTankCleaned.MenuList = toolbarclean.Show();

            PhoenixToolbar toolbarpostpone = new PhoenixToolbar();
            toolbarpostpone.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRTankConfigurationHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarpostpone.AddFontAwesomeButton("javascript:CallPrint('gvAlertPostpone')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuAlertPostpone.AccessRights = this.ViewState;
            MenuAlertPostpone.MenuList = toolbarpostpone.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["LocationId"].ToString() != string.Empty)
                    ViewState["LocationId"] = Request.QueryString["LocationId"];

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBERCLN"] = 1;
                ViewState["SORTEXPRESSIONCLN"] = null;
                ViewState["SORTDIRECTIONCLN"] = null;


                ViewState["PAGENUMBERAPS"] = 1;
                ViewState["SORTEXPRESSIONAPS"] = null;
                ViewState["SORTDIRECTIONAPS"] = null;

                if (Request.QueryString["LocationName"].ToString() != string.Empty)
                {
                    ViewState["LocationName"] = Request.QueryString["LocationName"].ToString();
                   // ucTitle.Text = "Tank History   (" + Request.QueryString["LocationName"].ToString() + ")     ";
                }
                //BindData();
                //BindDataClean();
                //BindDataPostpone();
                gvTankHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvTankCleaned.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvAlertPostpone.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuTankHistory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvTankHistory.EditIndex = -1;
                //gvTankHistory.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                //SetPageNavigator();
            }
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

    protected void MenuTankCleaned_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvTankCleaned.EditIndex = -1;
                //gvTankCleaned.SelectedIndex = -1;
                ViewState["PAGENUMBERCLN"] = 1;
                BindDataClean();
                //SetPageNavigatorClean();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelClean();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREPORTDATE", "FLDSEAPORTNAME", "FLDOPERATION", "FLDOILTYPENAME", "FLDUNITNAME","FLDQUANTITY", "FLDROBCUM" };
        string[] alCaptions = { "Date", "Port/Location", "Operation", "Product", "Unit", "Quantity" ,"ROB"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationHistory(new Guid(ViewState["LocationId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression, sortdirection, gvTankHistory.CurrentPageIndex+1,
            gvTankHistory.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvTankHistory", "Cargo (" + ViewState["LocationName"].ToString() + ")", alCaptions, alColumns, ds);
        gvTankHistory.DataSource = ds;
        //gvTankHistory.DataBind();
        gvTankHistory.VirtualItemCount = iRowCount;
       
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        //SetPageNavigator();
    }

    private void BindDataClean()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDATE", "FLDOILTYPENAME" };
        string[] alCaptions = { "Date", "Last Product Loaded " };

        string sortexpression = (ViewState["SORTEXPRESSIONCLN"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTIONCLN"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCLN"].ToString());

        DataSet ds = PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationCleanedHistory(new Guid(ViewState["LocationId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression, sortdirection,gvTankHistory.CurrentPageIndex+1,
            gvTankHistory.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvTankCleaned", "Tank Cleaning (" + ViewState["LocationName"].ToString() + ")", alCaptions, alColumns, ds);
        gvTankCleaned.DataSource = ds;
        // gvTankCleaned.DataBind();
        gvTankCleaned.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNTCLN"] = iRowCount;
        ViewState["TOTALPAGECOUNTCLN"] = iTotalPageCount;
        //SetPageNavigatorClean();
    }

    protected void gvTankHistory_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTankCleaned_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    gvTankHistory.SelectedIndex = -1;
    //    gvTankHistory.EditIndex = -1;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvTankHistory.SelectedIndex = -1;
    //    gvTankHistory.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    gvTankHistory.SelectedIndex = -1;
    //    gvTankHistory.EditIndex = -1;
    //    ViewState["PAGENUMBER"] = 1;
    //    BindData();
    //    SetPageNavigator();
    //}

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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDREPORTDATE", "FLDSEAPORTNAME", "FLDOPERATION", "FLDOILTYPENAME", "FLDUNITNAME","FLDQUANTITY", "FLDROBCUM" };
        string[] alCaptions = { "Date", "Port/Location", "Operation", "Product", "Unit", "Quantity" ,"ROB" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationHistory(new Guid(ViewState["LocationId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Cargo (" + ViewState["LocationName"].ToString() + ").xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Cargo (" + ViewState["LocationName"].ToString() + ")</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");

        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
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

    protected void ShowExcelClean()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDATE", "FLDOILTYPENAME" };
        string[] alCaptions = { "Date", "Last Product Loaded" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONCLN"] == null) ? null : (ViewState["SORTEXPRESSIONCLN"].ToString());
        if (ViewState["SORTDIRECTIONCLN"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCLN"].ToString());
        if (ViewState["ROWCOUNTCLN"] == null || Int32.Parse(ViewState["ROWCOUNTCLN"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTCLN"].ToString());

        ds = PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationCleanedHistory(new Guid(ViewState["LocationId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression, sortdirection, gvTankCleaned.CurrentPageIndex+1,
            iRowCount , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Tank Cleaning (" + ViewState["LocationName"].ToString() + ").xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Tank Cleaning (" + ViewState["LocationName"].ToString() + ")</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");

        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
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

    //protected void cmdGoCln_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    gvTankCleaned.SelectedIndex = -1;
    //    gvTankCleaned.EditIndex = -1;
    //    if (Int32.TryParse(txtnopageCln.Text, out result))
    //    {
    //        ViewState["PAGENUMBERCLN"] = Int32.Parse(txtnopageCln.Text);

    //        if ((int)ViewState["TOTALPAGECOUNTCLN"] < Int32.Parse(txtnopageCln.Text))
    //            ViewState["PAGENUMBERCLN"] = ViewState["TOTALPAGECOUNTCLN"];


    //        if (0 >= Int32.Parse(txtnopageCln.Text))
    //            ViewState["PAGENUMBERCLN"] = 1;

    //        if ((int)ViewState["PAGENUMBERCLN"] == 0)
    //            ViewState["PAGENUMBERCLN"] = 1;

    //        txtnopageCln.Text = ViewState["PAGENUMBERCLN"].ToString();
    //    }
    //    BindDataClean();
    //    SetPageNavigatorClean();
    //}

    //protected void PagerButtonClickCln(object sender, CommandEventArgs ce)
    //{
    //    gvTankCleaned.SelectedIndex = -1;
    //    gvTankCleaned.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBERCLN"] = (int)ViewState["PAGENUMBERCLN"] - 1;
    //    else
    //        ViewState["PAGENUMBERCLN"] = (int)ViewState["PAGENUMBERCLN"] + 1;

    //    BindDataClean();
    //    SetPageNavigatorClean();
    //}

    //private void SetPageNavigatorClean()
    //{
    //    cmdPreviousCln.Enabled = IsPreviousEnabledClean();
    //    cmdNextCln.Enabled = IsNextEnabledClean();
    //    lblPagenumberCln.Text = "Page " + ViewState["PAGENUMBERCLN"].ToString();
    //    lblPagesCln.Text = " of " + ViewState["TOTALPAGECOUNTCLN"].ToString() + " Pages. ";
    //    lblRecordsCln.Text = "(" + ViewState["ROWCOUNTCLN"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabledClean()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERCLN"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCLN"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabledClean()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERCLN"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCLN"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdSearchcln_Click(object sender, EventArgs e)
    //{
    //    gvTankCleaned.SelectedIndex = -1;
    //    gvTankCleaned.EditIndex = -1;
    //    ViewState["PAGENUMBERCLN"] = 1;
    //    BindDataClean();
    //    SetPageNavigatorClean();
    //}


    protected void MenuAlertPostpone_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvAlertPostpone.EditIndex = -1;
                //gvAlertPostpone.SelectedIndex = -1;
                ViewState["PAGENUMBERAPS"] = 1;
                BindDataPostpone();
                gvAlertPostpone.Rebind();
                //SetPageNavigatorClean();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelPostpone();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataPostpone()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

      
        string sortexpression = (ViewState["SORTEXPRESSIONAPS"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTIONAPS"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONAPS"].ToString());

        DataSet ds = PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationAlertPostponeHistory(new Guid(ViewState["LocationId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression, sortdirection, (int)ViewState["PAGENUMBERAPS"],
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        string[] alColumns = { "FLDDATE", "FLDOILTYPENAME", "FLDPOSTPONEREMARKS" };
        string[] alCaptions = { "Date", "Product", "Remarks" };

        General.SetPrintOptions("gvAlertPostpone", "Alert postponing (" + ViewState["LocationName"].ToString() + ")", alCaptions, alColumns, ds);
        gvAlertPostpone.DataSource = ds;
        // gvAlertPostpone.DataBind();
        gvAlertPostpone.VirtualItemCount = iRowCount;
       
        ViewState["ROWCOUNTAPS"] = iRowCount;
        ViewState["TOTALPAGECOUNTAPS"] = iTotalPageCount;
        //SetPageNavigatorPostpone();
    }

    protected void ShowExcelPostpone()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDATE", "FLDOILTYPENAME", "FLDPOSTPONEREMARKS" };
        string[] alCaptions = { "Date", "Product", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONAPS"] == null) ? null : (ViewState["SORTEXPRESSIONAPS"].ToString());
        if (ViewState["SORTDIRECTIONAPS"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONAPS"].ToString());
        if (ViewState["ROWCOUNTCLN"] == null || Int32.Parse(ViewState["ROWCOUNTAPS"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTAPS"].ToString());

        ds = PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationAlertPostponeHistory(new Guid(ViewState["LocationId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression, sortdirection, gvAlertPostpone.CurrentPageIndex+1,
            iRowCount, ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvAlertPostpone", "Alert postponing (" + ViewState["LocationName"].ToString() + ")", alCaptions, alColumns, ds);
        Response.AddHeader("Content-Disposition", "attachment; filename=\"Tank Cleaning (" + ViewState["LocationName"].ToString() + ").xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Tank Cleaning (" + ViewState["LocationName"].ToString() + ")</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");

        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
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


    
    //protected void cmdGoAps_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    gvAlertPostpone.SelectedIndex = -1;
    //    gvAlertPostpone.EditIndex = -1;
    //    if (Int32.TryParse(txtnopageCln.Text, out result))
    //    {
    //        ViewState["PAGENUMBERCLN"] = Int32.Parse(txtnopageAps.Text);

    //        if ((int)ViewState["TOTALPAGECOUNTAPS"] < Int32.Parse(txtnopageAps.Text))
    //            ViewState["PAGENUMBERAPS"] = ViewState["TOTALPAGECOUNTAPS"];


    //        if (0 >= Int32.Parse(txtnopageAps.Text))
    //            ViewState["PAGENUMBERAPS"] = 1;

    //        if ((int)ViewState["PAGENUMBERAPS"] == 0)
    //            ViewState["PAGENUMBERAPS"] = 1;

    //        txtnopageAps.Text = ViewState["PAGENUMBERAPS"].ToString();
    //    }
    //    BindDataPostpone();
    //    SetPageNavigatorPostpone();
    //}

    //protected void PagerButtonClickAps(object sender, CommandEventArgs ce)
    //{
    //    gvAlertPostpone.SelectedIndex = -1;
    //    gvAlertPostpone.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBERAPS"] = (int)ViewState["PAGENUMBERAPS"] - 1;
    //    else
    //        ViewState["PAGENUMBERAPS"] = (int)ViewState["PAGENUMBERAPS"] + 1;

    //    BindDataPostpone();
    //    SetPageNavigatorPostpone();
    //}

    //private void SetPageNavigatorPostpone()
    //{
    //    cmdPreviousAps.Enabled = IsPreviousEnabledPostpone();
    //    cmdNextAps.Enabled = IsNextEnabledPostpone();
    //    lblPagenumberAps.Text = "Page " + ViewState["PAGENUMBERAPS"].ToString();
    //    lblPagesAps.Text = " of " + ViewState["TOTALPAGECOUNTAPS"].ToString() + " Pages. ";
    //    lblRecordsAps.Text = "(" + ViewState["ROWCOUNTAPS"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabledPostpone()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERAPS"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTAPS"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabledPostpone()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERAPS"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTAPS"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdSearchaps_Click(object sender, EventArgs e)
    //{
    //    gvAlertPostpone.SelectedIndex = -1;
    //    gvAlertPostpone.EditIndex = -1;
    //    ViewState["PAGENUMBERAPS"] = 1;
    //    BindDataPostpone();
    //    SetPageNavigatorPostpone();
    //}


    protected void gvTankHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvTankCleaned_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataClean();
    }

    protected void gvAlertPostpone_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
       BindDataPostpone();
    }
}
