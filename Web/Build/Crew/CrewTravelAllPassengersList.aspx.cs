using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewTravelAllPassengersList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            toolbar.AddButton("Passengers List", "PASSENGERSLIST");
            toolbar.AddButton("Passenger Details", "PASSENGERSENTRY");
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();
            CrewMenu.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["TRAVELID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["EDITTRAVELREQUEST"] = "1";

                if (Request.QueryString["travelid"].ToString() != "")
                {
                    ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();                   
                }
                if (Request.QueryString["travelrequestedit"] != null)
                {
                    ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();
                }                

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

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


            if (General.GetNullableGuid(ViewState["REQUESTID"] == null ? null : ViewState["REQUESTID"].ToString()) == null)
            {
                Showerror();
                return;
            }
            if (dce.CommandName.ToUpper().Equals("PASSENGERSLIST"))
            {
                Response.Redirect("CrewTravelAllPassengersList.aspx?travelid=" + ViewState["TRAVELID"], false);
            }
            if (dce.CommandName.ToUpper().Equals("PASSENGERSENTRY"))
            {
                Response.Redirect("CrewTravelPassengersDetails.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Showerror()
    {
        ucError.ErrorMessage = "Please select the passenger to proceed further.";
        ucError.Visible = true;
        return;
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewTravelRequest.TravelRequestGeneralSearch(
                General.GetNullableGuid(ViewState["TRAVELID"] != null ? ViewState["TRAVELID"].ToString() : null),
                sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            //General.SetPrintOptions("gvCCT", "Travel Plan", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCCT.DataSource = ds;
                gvCCT.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCCT);
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvCCT.EditIndex = -1;
        gvCCT.SelectedIndex = -1;
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
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCCT.SelectedIndex = -1;
        gvCCT.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCCT_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView gv = (GridView)sender;
        if (e.CommandName.ToUpper() == "SELECT")
        {
            int currentrow = int.Parse(e.CommandArgument.ToString());
            ViewState["REQUESTID"] = ((Label)(gv.Rows[currentrow].FindControl("lblTravelRequestId"))).Text;

            Response.Redirect("CrewTravelPassengersDetails.aspx?travelid=" + ViewState["TRAVELID"] + "&requestid=" + ViewState["REQUESTID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
        }
    }
}