using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;

public partial class CommonPickListRHShipWorkCalendar : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {         

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            BindYear();
        }
        BindData();
    }
    private void BindYear()
    {
        for (int i = 2012; i <= DateTime.Now.Year; i++)
        {
            ddlYear.Items.Add(i.ToString());
        }
        ddlmonth.SelectedValue = DateTime.Now.Month.ToString();
        ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        txtVesselname.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            ds = PhoenixVesselAccountsRH.RestHourShipWorkCalendarSearch( 
                   PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                   int.Parse(ddlYear.SelectedValue),
                   int.Parse(ddlmonth.SelectedValue),
                   (int)ViewState["PAGENUMBER"],
                   General.ShowRecords(null),
                   ref iRowCount,
                   ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {

                gvShipCalendar.DataSource = ds;
                gvShipCalendar.DataBind();
            }
            else
            {

                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvShipCalendar);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void ddlmonth_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }            
    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        {
            return true;
        }

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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
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
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvShipCalendar.SelectedIndex = -1;
            gvShipCalendar.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvShipCalendar_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        string Script = "";
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = new NameValueCollection();

            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkDate");
            nvc.Add(lb.ID, lb.Text.ToString());
            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCalenderid");
            nvc.Add(lbl.ID, lbl.Text);
        }
        else
        {

            nvc = Filter.CurrentPickListSelection;

            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkDate");
            nvc.Set(nvc.GetKey(1), lb.Text.ToString());
            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCalenderid");
            nvc.Set(nvc.GetKey(2), lbl.Text);

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";
        }

        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
