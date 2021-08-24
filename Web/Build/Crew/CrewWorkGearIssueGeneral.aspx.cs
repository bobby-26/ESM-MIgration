using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOperation;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewWorkGearIssueGeneral : PhoenixBasePage
{
    private string empid = string.Empty;
    private string vslid = string.Empty;
    private string crewplanid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"];
        vslid = Request.QueryString["vslid"];
        crewplanid = Request.QueryString["crewplanid"];
        if (!IsPostBack)
        {
            ViewState["MPAGENUMBER"] = 1;
            ViewState["MSORTEXPRESSION"] = null;
            ViewState["MSORTDIRECTION"] = null;

            string iIsuueType = "";
            if (crewplanid != null)
            {
                PhoenixCrewWorkingGearIssuance.GetWorkingGearIssueType(new Guid(crewplanid), ref iIsuueType);
            }
            if (!String.IsNullOrEmpty(iIsuueType))
            {
                if (iIsuueType == "REQUEST")
                {
                    Response.Redirect("CrewWorkingGearIndividualRequest.aspx?empid=" + empid + "&vesslid=" + vslid + "&crewplanid=" + crewplanid, false);
                }
                else if (iIsuueType == "ISSUE")
                {
                    Response.Redirect("CrewWorkingGearIssuance.aspx?empid=" + empid + "&vesslid=" + vslid + "&crewplanid=" + crewplanid, false);
                }
            }

        }

        BindData();
        SetPageNavigator();
    }
    private void RedirectPage()
    {
        if (!String.IsNullOrEmpty(rdoIssueType.SelectedValue))
        {
            if (rdoIssueType.SelectedValue == "0")
            {
                Response.Redirect("CrewWorkingGearIndividualRequest.aspx?empid=" + empid + "&vesslid=" + vslid + "&crewplanid=" + crewplanid, false);

            }
            if (rdoIssueType.SelectedValue == "1")
            {
                Response.Redirect("CrewWorkingGearIssuance.aspx?empid=" + empid + "&vesslid=" + vslid + "&crewplanid=" + crewplanid, false);

            }
        }

    }

    protected void rdoIssueType_SelectedIndexChanged(object sender, EventArgs e)
    {
        RedirectPage();
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDRANKNAME", "FLDORDERDATE", "FLDORDERSTATUSNAME", "FLDSUPPLIER" };
        string[] alCaptions = { "Request Number", "Vessel", "Rank", "Order Date", "Status", "Supplier" };

        string sortexpression = (ViewState["MSORTEXPRESSION"] == null) ? null : (ViewState["MSORTEXPRESSION"].ToString());
        int? sortdirection = 1; //default desc order
        if (ViewState["MSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["MSORTDIRECTION"].ToString());

        try
        {

            DataSet ds = PhoenixCrewWorkingGearOrderForm.SearchWorkGearRequest(General.GetNullableInteger(empid)                                                    
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["MPAGENUMBER"], General.ShowRecords(null)
                                                        , ref iRowCount, ref iTotalPageCount);
            
            General.SetPrintOptions("gvReq", "Work Gear Request", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvReq.DataSource = ds;
                gvReq.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvReq);
            }
            ViewState["MROWCOUNT"] = iRowCount;
            ViewState["MTOTALPAGECOUNT"] = iTotalPageCount;

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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["MPAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["MTOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["MROWCOUNT"].ToString() + " records found)";
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

        iCurrentPageNumber = (int)ViewState["MPAGENUMBER"];
        iTotalPageCount = (int)ViewState["MTOTALPAGECOUNT"];

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

        iCurrentPageNumber = (int)ViewState["MPAGENUMBER"];
        iTotalPageCount = (int)ViewState["MTOTALPAGECOUNT"];

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
                ViewState["MPAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["MTOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["MPAGENUMBER"] = ViewState["MTOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["MPAGENUMBER"] = 1;

                if ((int)ViewState["MPAGENUMBER"] == 0)
                    ViewState["MPAGENUMBER"] = 1;

                txtnopage.Text = ViewState["MPAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
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
            gvReq.SelectedIndex = -1;
            gvReq.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["MPAGENUMBER"] = (int)ViewState["MPAGENUMBER"] - 1;
            else
                ViewState["MPAGENUMBER"] = (int)ViewState["MPAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["MSORTEXPRESSION"] = se.SortExpression;

        if (ViewState["MSORTDIRECTION"] != null && ViewState["MSORTDIRECTION"].ToString() == "0")
            ViewState["MSORTDIRECTION"] = 1;
        else
            ViewState["MSORTDIRECTION"] = 0;

        BindData();
        SetPageNavigator();
    }

    protected void gvReq_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("CANCELREQUEST"))
            {
                string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestId")).Text;
                PhoenixCrewWorkGearIndividualRequest.DeleteIndividualRequest(new Guid(requestid));
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvReq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancelRequest");
        if (cancel != null) 
        {
            cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            cancel.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel the request? '); return false;");
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["MSORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["MSORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["MSORTDIRECTION"] == null || ViewState["MSORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }
}
