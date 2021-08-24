using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewHotelBookingGuests : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                PhoenixToolbar toolbargrid = new PhoenixToolbar();

                toolbarmain.AddButton("Booking", "BOOKING");
                toolbarmain.AddButton("Guests ", "GUESTS");

                MenuMainGuests.AccessRights = this.ViewState;
                MenuMainGuests.MenuList = toolbarmain.Show();

                MenuMainGuests.SelectedMenuIndex = 1;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CITYNAME"] = null;
                ViewState["VESSELID"] = "74";
                ViewState["CITYID"] = "1233";
                ViewState["BOOKINGID"] = null;
                ViewState["ROOMTYPEID"] = null;
                ViewState["EXTRABEDS"] = null;
                ViewState["TRAVELID"]=null;
                if (Request.QueryString["BookingId"] != null)
                {
                    ViewState["BOOKINGID"] = Request.QueryString["BookingId"].ToString();
                }
                toolbargrid.AddImageButton("../Crew/CrewHotelBookingGuest.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvGuest')", "Print Grid", "icon_print.png", "PRINT");
                //toolbarmain.AddImageLink("javascript:Openpopup('Filter','','../Crew/CrewTravelHopList.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "');return false;", "Add Agent", "add.png", "ADD");                
                toolbargrid.AddImageLink("javascript:Openpopup('Filter','','../Crew/CrewHotelBookingTravelHopList.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "&cityid=" + ViewState["CITYID"].ToString() + "&bookingid=" + ViewState["BOOKINGID"].ToString()  + "'); return false;", "Add Guests", "Add.png", "ADD");
              

                MenuHotelGuests.AccessRights = this.ViewState;
                MenuHotelGuests.MenuList = toolbargrid.Show();

                //cblComanyPayableCharges.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 100);
                //cblComanyPayableCharges.DataBind();
                
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
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;

            DataSet ds = new DataSet();

            ds = PhoenixCrewHotelBookingGuests.CrewHotelBookingGuestSearch(ViewState["BOOKINGID"] != null ? General.GetNullableGuid(ViewState["BOOKINGID"].ToString()) : null, sortexpression, sortdirection,
                                (int)ViewState["PAGENUMBER"],
                                10,
                                ref iRowCount,
                                ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvGuest.DataSource = ds;
                gvGuest.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvGuest);
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
    protected void MenuMainGuests_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("BOOKING"))
        {
            Response.Redirect("../Crew/CrewHotelBooking.aspx?bookingid=" + ViewState["BOOKINGID"].ToString());
        }

    }
    protected void gvGuest_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            Label lblGuestId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblGuestId");
           
            if (lblGuestId != null)
            {
                PhoenixCrewHotelBookingGuests.DeleteHotelBookingGuest(General.GetNullableGuid(lblGuestId.Text.ToString()));
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
    protected void MenuHotelGuests_TabStripCommand(object sender, EventArgs e)
    {
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
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
            gvGuest.SelectedIndex = -1;
            gvGuest.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs args)
    {
    }
}
