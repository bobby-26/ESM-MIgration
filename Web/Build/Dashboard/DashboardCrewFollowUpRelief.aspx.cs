using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Dashboard_DashboardCrewFollowUpRelief : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SelectedOption();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 1;
                ViewState["MODE"] = "0";

                if (rdBtn.SelectedValue != "0" || rdBtn.SelectedValue != "1" || rdBtn.SelectedValue !="2")
                {
                    rdBtn.SelectedValue = "0";
                }
            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SelectedOption()
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Clear();

        nvc.Add("Url", HttpContext.Current.Request.Url.AbsolutePath);
        nvc.Add("APP", "CREW");
        nvc.Add("Option", "CFR");

        Filter.CurrentDashboardLastSelection = nvc;
        PhoenixDashboardOption.DashboardLastSelected(nvc);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        if (rdBtn.SelectedValue == "0")
        {
            iRowCount = 10;

            ds = PhoenixDashboardCrewFollowUpRelief.CrewFollowUp(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                                                      iRowCount, ref iRowCount, ref iTotalPageCount);
        }

        if (rdBtn.SelectedValue == "1")
        {
            iRowCount = 10;

            ds = PhoenixDashboardCrewFollowUpRelief.NewApplicantFollowUp(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                                                      iRowCount, ref iRowCount, ref iTotalPageCount);
        }

        if (rdBtn.SelectedValue == "2")
        {
            iRowCount = 10;

            ds = PhoenixDashboardCrewFollowUpRelief.NewApplicantReliefDue(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                                                      iRowCount, ref iRowCount, ref iTotalPageCount); 
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAlertsTask.DataSource = ds;
            gvAlertsTask.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvAlertsTask);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        SetPageNavigator();
    }

    protected void gvAlertsTask_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAlertsTask.SelectedIndex = -1;
        gvAlertsTask.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();

    }

   

    protected void gvAlertsTask_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblEmpNo = (Label)e.Row.FindControl("lblEmpId");
            LinkButton lnkName = (LinkButton)e.Row.FindControl("lblEmpName");
            if (lblEmpNo != null && lnkName != null)
            {
                lnkName.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewListForAPeriod','','../Crew/CrewPersonalGeneral.aspx?empid=" + lblEmpNo.Text + "'); return false;");
            }
        }
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

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    protected void btnGo_Click(object sender, EventArgs e)
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();

    }

}
