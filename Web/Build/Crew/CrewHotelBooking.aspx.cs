using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewHotelBooking : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            lblBookingId.Attributes.Add("style", "visibility:hidden");


            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbarmain.AddButton("Booking", "BOOKING");
                toolbarmain.AddButton("Guests", "GUESTS");

                MenuHotelBookingMain.AccessRights = this.ViewState;
                MenuHotelBookingMain.MenuList = toolbarmain.Show();

                if (Request.QueryString["bookingid"] != null)
                {
                    ViewState["bookingid"] = Request.QueryString["bookingid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewHotelBookingGeneral.aspx?BookingId=" + Request.QueryString["bookingid"].ToString();
                    lblBookingId.Text = ViewState["bookingid"].ToString();
                }

                toolbargrid.AddImageButton("../Crew/CrewHotelBooking.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvBookingDetails')", "Print Grid", "icon_print.png", "PRINT");
                toolbargrid.AddImageButton("../Crew/CrewHotelBookingFilter.aspx", "Find", "search.png", "FIND");
                toolbargrid.AddImageButton("../Crew/CrewHotelBooking.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

                MenuHotelBooking.AccessRights = this.ViewState;
                MenuHotelBooking.MenuList = toolbargrid.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["bookingid"] = null;
                ViewState["PAGEURL"] = null;

                if (Request.QueryString["bookingid"] != null)
                {
                    ViewState["bookingid"] = Request.QueryString["bookingid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewHotelBookingGeneral.aspx?bookingid=" + Request.QueryString["bookingid"].ToString();
                    lblBookingId.Text = ViewState["bookingid"].ToString();
                }
                MenuHotelBookingMain.SelectedMenuIndex = 0;
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
    protected void gvBookingDetails_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
    }
    protected void MenuHotelBookingMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {

            if (dce.CommandName.ToUpper().Equals("BOOKING"))
            {
                ViewState["PAGEURL"] = "../Crew/CrewHotelBookingGeneral.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("GUESTS"))
            {
                ViewState["PAGEURL"] = "../Crew/CrewHotelBookingGuests.aspx";
                Response.Redirect("../Crew/CrewHotelBookingGuests.aspx?bookingid=" + ViewState["bookingid"].ToString());
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREFERENCENO", "FLDHOTELNAME", "FLDVESSELNAME", "FLDREQUESTEDDATE", "FLDCITYNAME", "FLDSTATUS" };
            string[] alCaptions = { "Number", "Hotel Name", "Vessel Name", "Requested Date", "City", "Status" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            //if (Filter.CurrentHotelBookingFilterCriteria != null)
            // {

            NameValueCollection nvc = Filter.CurrentHotelBookingFilterCriteria;

            ds = PhoenixCrewHotelBooking.HotelBookingSearch(
                    nvc != null ? nvc.Get("txtReferenceNo") : null
                , General.GetNullableInteger(nvc != null ? nvc["ucVessel"] : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc["txtcityid"] : string.Empty)
                , null
                , null
                , null
                , null
                , null
                , null
                , General.GetNullableInteger(nvc != null ? nvc["ucCrewChangeReason"] : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc["ucPurpose"] : string.Empty)
                , null
                , null
                , null
                , sortexpression, sortdirection, 1, iRowCount,
                     ref iRowCount, ref iTotalPageCount);
            //  }        
            Response.AddHeader("Content-Disposition", "attachment; filename= HotelBooking.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Hotel Booking </center></h3></td>");
            //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuHotelBooking_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["bookingid"] = null;
                ViewState["ROWCOUNT"] = null;
                Filter.CurrentHotelBookingFilterCriteria = null;
                BindData();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREFERENCENO", "FLDHOTELNAME", "FLDVESSELNAME", "FLDREQUESTEDDATE", "FLDCITYNAME", "FLDSTATUS" };
            string[] alCaptions = { "Number", "Hotel Name", "Vessel Name", "Requested Date", "City", "Status" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            //if (Filter.CurrentHotelBookingFilterCriteria != null)
            // {

            NameValueCollection nvc = Filter.CurrentHotelBookingFilterCriteria;

            ds = PhoenixCrewHotelBooking.HotelBookingSearch(
                     nvc != null ? nvc.Get("txtReferenceNo") : null
                 , General.GetNullableInteger(nvc != null ? nvc["ucVessel"] : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc["txtcityid"] : string.Empty)
                 , null
                 , null
                 , null
                 , null
                 , null
                 , null
                 , General.GetNullableInteger(nvc != null ? nvc["ucCrewChangeReason"] : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc["ucPurpose"] : string.Empty)
                 , null
                 , null
                 , null
                 , sortexpression, sortdirection, 1, iRowCount,
                      ref iRowCount, ref iTotalPageCount);
            //}


            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                gvBookingDetails.DataSource = ds;
                gvBookingDetails.DataBind();

                if (ViewState["bookingid"] == null)
                {
                    ViewState["bookingid"] = ds.Tables[0].Rows[0]["FLDBOOKINGID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    lblBookingId.Text = ViewState["bookingid"].ToString();
                    gvBookingDetails.SelectedIndex = 0;

                    if (ViewState["bookingid"] != null)
                        ViewState["PAGEURL"] = "../Crew/CrewHotelBookingGeneral.aspx";
                    //MenuOrderFormMain.Visible = true; 
                }

                if (ViewState["PAGEURL"] == null)
                {
                    ViewState["PAGEURL"] = "../Crew/CrewHotelBookingGeneral.aspx";
                }
                if (ViewState["PAGEURL"] != null)
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?bookingid=" + ViewState["bookingid"].ToString(); ;
                }

                SetRowSelection();
                MenuHotelBookingMain.Visible = true;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvBookingDetails);
                ViewState["PAGEURL"] = "CrewHotelBookingNew.aspx";
                ifMoreInfo.Attributes["src"] = "CrewHotelBookingNew.aspx";
                MenuHotelBookingMain.Visible = false;
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            General.SetPrintOptions("gvBookingDetails", "Hotel Booking", alCaptions, alColumns, ds);
            SetTabHighlight();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        gvBookingDetails.SelectedIndex = -1;
        for (int i = 0; i < gvBookingDetails.Rows.Count; i++)
        {
            if (gvBookingDetails.DataKeys[i].Value.ToString().Equals(ViewState["bookingid"].ToString()))
            {
                gvBookingDetails.SelectedIndex = i;
                PhoenixCrewHotelBooking.ReferenceNumber = ((LinkButton)gvBookingDetails.Rows[i].FindControl("lnkReferenceNumberName")).Text;
                ViewState["DTKEY"] = ((Label)gvBookingDetails.Rows[gvBookingDetails.SelectedIndex].FindControl("lbldtkey")).Text;
            }
        }
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvBookingDetails.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvBookingDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvBookingDetails_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    protected void gvBookingDetails_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    protected void gvBookingDetails_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                Label lbtn = (Label)e.Row.FindControl("lblHotelName");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucHotelNameTT");
                //if (lbtn != null)
                //{
                //    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                //    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                //}
            }

            LinkButton lnkReferenceNumberName = (LinkButton)e.Row.FindControl("lnkReferenceNumberName");

            Label referenceno = (Label)e.Row.FindControl("lblReferenceNumber");

        }

        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        {
            LinkButton _doubleClickButton = (LinkButton)e.Row.FindControl("lnkDoubleClick");
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        }
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
            gvBookingDetails.SelectedIndex = -1;
            gvBookingDetails.EditIndex = -1;
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
    protected void gvBookingDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
        //if (Session["New"].ToString() == "Y")
        //{
        //    gvBookingDetails.SelectedIndex = 0;
        //    Session["New"] = "N";
        //}
    }
    protected void gvBookingDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvBookingDetails.SelectedIndex = e.NewSelectedIndex;
        Label lblBookingId = (Label)gvBookingDetails.Rows[e.NewSelectedIndex].FindControl("lblBookingId");

        ViewState["PAGEURL"] = "../Crew/CrewHotelBookingGeneral.aspx";

        if (lblBookingId != null)
        {
            ViewState["bookingid"] = lblBookingId.Text.ToString();

            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?bookingid=" + ViewState["bookingid"].ToString(); ;
        }
    }
    protected void SetTabHighlight()
    {
        //try
        //{
        //    if (ViewState["PAGEURL"].ToString().Trim().Contains("CrewHotelBookingGeneral.aspx"))
        //    {
        //        MenuHotelBookingMain.SelectedMenuIndex = 0;
        //    }
        //    else if (ViewState["PAGEURL"].ToString().Trim().Contains("CrewHotelRoomGuestDetails.aspx"))
        //    {
        //        MenuHotelBookingMain.SelectedMenuIndex = 1;
        //    }
        //    else if (ViewState["PAGEURL"].ToString().Trim().Contains("CrewHotelBookingDetail.aspx"))
        //    {
        //        MenuHotelBookingMain.SelectedMenuIndex = 2;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }
}
