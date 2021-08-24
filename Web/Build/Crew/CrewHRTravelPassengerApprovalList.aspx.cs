using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewHRTravelPassengerApprovalList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {
            foreach (GridViewRow r in gvTravelPassenger.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation(gvTravelPassenger.UniqueID, "SELECT$" + r.RowIndex.ToString());
                }
            }

            foreach (GridViewRow r in gvTravelRequestBreakup.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation(gvTravelRequestBreakup.UniqueID, "SELECT$" + r.RowIndex.ToString());
                }
            }

            base.Render(writer);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST");
            toolbarmain.AddButton("Details", "PASSENGERS");
            toolbarmain.AddButton("Tickets", "TICKETS");

            MenuTravelPassengerMain.AccessRights = this.ViewState;
            MenuTravelPassengerMain.MenuList = toolbarmain.Show();
            MenuTravelPassengerMain.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Save", "SAVE");

            MenuTravelPassengerSub.AccessRights = this.ViewState;
            MenuTravelPassengerSub.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["TRAVELPASSENGERID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["travelrequestid"] != null && Request.QueryString["travelrequestid"].ToString() != "")
                    ViewState["TRAVELREQUESTID"] = Request.QueryString["travelrequestid"].ToString();
                else
                    ViewState["TRAVELREQUESTID"] = "";

                if (Request.QueryString["personalinfosn"] != null && Request.QueryString["personalinfosn"].ToString() != "")
                    ViewState["PERSONALINFOSN"] = Request.QueryString["personalinfosn"].ToString();
                else
                    ViewState["PERSONALINFOSN"] = "";

                BindVesselData();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewHRTravelPassengerList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvTravelPassenger')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("javascript:Openpopup('Add', '', 'CrewHRTravelPassengerSelectionList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString() + "'); return false;", "Add Passenger", "add.png", "ADD");

            MenuTravelPassenger.AccessRights = this.ViewState;
            MenuTravelPassenger.MenuList = toolbar.Show();

            BindBreakUpData();
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TravelPassengerMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewHRTravelRequestApprovalList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("PASSENGERS"))
            {
                Response.Redirect("../Crew/CrewHRTravelPassengerList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("TICKETS"))
            {
                Response.Redirect("../Crew/CrewHRTravelTicketList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TravelPassengerSub_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewHRTravelRequest.HRTravelRequestUpdate(new Guid(ViewState["TRAVELREQUESTID"].ToString())
                , General.GetNullableInteger(ddlVessel.SelectedVessel));

                ucStatus.Text = "Vessel has been updated.";
                BindVesselData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TravelPassenger_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDDATEOFBIRTH" };
        string[] alCaptions = { "S.No.", "Name", "DOB" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewHRTravelRequest.HRTravelPassengerSearch(new Guid(ViewState["TRAVELREQUESTID"].ToString())
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , General.ShowRecords(null)
                    , ref iRowCount
                    , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=TravelPassengerList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Passenger List</h3></td>");
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

    private void BindBreakUpData()
    {
        try
        {
            DataTable dt = PhoenixCrewHRTravelRequest.HRTravelRequestBreakUpSearch(General.GetNullableGuid(ViewState["TRAVELREQUESTID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                gvTravelRequestBreakup.DataSource = dt;
                gvTravelRequestBreakup.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvTravelRequestBreakup);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDDATEOFBIRTH" };
            string[] alCaptions = { "S.No.", "Name", "DOB" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewHRTravelRequest.HRTravelPassengerSearch(new Guid(ViewState["TRAVELREQUESTID"].ToString())
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , General.ShowRecords(null)
                    , ref iRowCount
                    , ref iTotalPageCount);

            General.SetPrintOptions("gvTravelPassenger", "Travel Passenger List", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTravelPassenger.DataSource = ds;
                gvTravelPassenger.DataBind();
                if (ViewState["TRAVELPASSENGERID"] == null)
                {
                    ViewState["TRAVELPASSENGERID"] = ds.Tables[0].Rows[0]["FLDTRAVELPASSENGERID"].ToString();
                    //ifMoreInfo.Attributes["src"] = "../Crew/CrewHRTravelPassengerGeneral.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&travelpassengerid=" + ViewState["TRAVELPASSENGERID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString();
                    gvTravelPassenger.SelectedIndex = 0;
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvTravelPassenger);
                //ifMoreInfo.Attributes["src"] = "../Crew/CrewHRTravelPassengerGeneral.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&travelpassengerid=&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString();
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

    protected void BindVesselData()
    {
        DataTable dt = PhoenixCrewHRTravelRequest.HRTravelRequestEdit(General.GetNullableGuid(ViewState["TRAVELREQUESTID"].ToString()), null);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ddlVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
        }
    }

    protected void gvTravelPassenger_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            ViewState["TRAVELREQUESTID"] = ((Label)gvTravelPassenger.Rows[nCurrentRow].FindControl("lblTravelRequestId")).Text;
            ViewState["TRAVELPASSENGERID"] = ((Label)gvTravelPassenger.Rows[nCurrentRow].FindControl("lblTravelPassengerId")).Text;
            ViewState["PERSONALINFOSN"] = ((Label)gvTravelPassenger.Rows[nCurrentRow].FindControl("lblpersonalinfosn")).Text;

            if (e.CommandName.ToUpper() == "SELECT")
            {
                //ifMoreInfo.Attributes["src"] = "../Crew/CrewHRTravelPassengerGeneral.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&travelpassengerid=" + ViewState["TRAVELPASSENGERID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString();
                _gridView.SelectedIndex = nCurrentRow;

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixCrewHRTravelRequest.HRTravelPassengerDelete(new Guid(ViewState["TRAVELPASSENGERID"].ToString())
                    , new Guid(ViewState["TRAVELREQUESTID"].ToString()));

                _gridView.EditIndex = -1;
                BindData();
                SetPageNavigator();
                ViewState["TRAVELPASSENGERID"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelPassenger_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton select = (ImageButton)e.Row.FindControl("cmdSelect");
                if (select != null) select.Visible = SessionUtil.CanAccess(this.ViewState, select.CommandName);

                ImageButton delete = (ImageButton)e.Row.FindControl("cmdDelete");
                if (delete != null)
                {
                    delete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    delete.Visible = SessionUtil.CanAccess(this.ViewState, delete.CommandName);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelPassenger_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            gvTravelPassenger.SelectedIndex = e.NewSelectedIndex;

            ViewState["TRAVELREQUESTID"] = ((Label)gvTravelPassenger.Rows[e.NewSelectedIndex].FindControl("lblTravelRequestId")).Text;
            ViewState["TRAVELPASSENGERID"] = ((Label)gvTravelPassenger.Rows[e.NewSelectedIndex].FindControl("lblTravelPassengerId")).Text;
            ViewState["PERSONALINFOSN"] = ((Label)gvTravelPassenger.Rows[e.NewSelectedIndex].FindControl("lblpersonalinfosn")).Text;
            //ifMoreInfo.Attributes["src"] = "../Crew/CrewHRTravelPassengerGeneral.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&travelpassengerid=" + ViewState["TRAVELPASSENGERID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString();
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelPassenger_RowDeleting(object sender, GridViewDeleteEventArgs de)
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

    private void SetRowSelection()
    {
        try
        {
            gvTravelPassenger.SelectedIndex = -1;
            for (int i = 0; i < gvTravelPassenger.Rows.Count; i++)
            {
                if (gvTravelPassenger.DataKeys[i].Value.ToString().Equals(ViewState["TRAVELPASSENGERID"].ToString()))
                {
                    gvTravelPassenger.SelectedIndex = i;
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvTravelPassenger.EditIndex = -1;
        gvTravelPassenger.SelectedIndex = -1;
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
        gvTravelPassenger.SelectedIndex = -1;
        gvTravelPassenger.EditIndex = -1;
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
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTravelBreakUp(string departurecity, string departurecityid, string departuredate,
        string destinationcity, string destinationcityid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(departurecity.Trim()) && General.GetNullableInteger(departurecityid) == null)
            ucError.ErrorMessage = "Departure City is required.";

        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Departure Date is required.";

        if (string.IsNullOrEmpty(destinationcity.Trim()) && General.GetNullableInteger(destinationcityid) == null)
            ucError.ErrorMessage = "Destination City is required.";

        return (!ucError.IsError);
    }

    protected void gvTravelRequestBreakup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper() == "ADD")
            {
                string departurecityid = ((TextBox)_gridView.FooterRow.FindControl("txtDepatureIdBreakupAdd")).Text;
                string departurecity = ((TextBox)_gridView.FooterRow.FindControl("txtDepatureBreakupAdd")).Text;
                string departuredate = ((UserControlDate)_gridView.FooterRow.FindControl("txtDepartureDateAdd")).Text;
                string departuretime = ((DropDownList)_gridView.FooterRow.FindControl("ddlDepartureTimeAdd")).SelectedValue;
                string destinationcityid = ((TextBox)_gridView.FooterRow.FindControl("txtDestinationIdBreakupAdd")).Text;
                string destinationcity = ((TextBox)_gridView.FooterRow.FindControl("txtDestinationBreakupAdd")).Text;
                string arrivaldate = ((UserControlDate)_gridView.FooterRow.FindControl("txtArrivalDateAdd")).Text;
                string arrivaltime = ((DropDownList)_gridView.FooterRow.FindControl("ddlArrivalTimeAdd")).SelectedValue;

                if (!IsValidTravelBreakUp(departurecity, departurecityid, departuredate, destinationcity, destinationcityid))
                {
                    ucError.Visible = true;
                    return;
                }

                //if (General.GetNullableGuid(ViewState["TRAVELREQUESTID"].ToString()) != null)
                //{
                //    PhoenixCrewHRTravelRequest.HRTravelBreakupInsert(new Guid(ViewState["TRAVELREQUESTID"].ToString())
                //    , int.Parse(ViewState["PERSONALINFOSN"].ToString())
                //    , null
                //    , null
                //    , int.Parse(departurecityid)
                //    , General.GetNullableString(departurecity)
                //    , DateTime.Parse(departuredate)
                //    , int.Parse(departuretime)
                //    , int.Parse(destinationcityid)
                //    , General.GetNullableString(destinationcity)
                //    , General.GetNullableDateTime(arrivaldate)
                //    , General.GetNullableInteger(arrivaltime));
                //}
                ucStatus.Text = "Travel Breakup is added.";
                BindBreakUpData();
            }
            else if (e.CommandName.ToUpper() == "SAVE")
            {
                string travelrequestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelRequestId")).Text;
                string travelbreakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelBreakupId")).Text;
                string departurecityid = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDepatureIdBreakupEdit")).Text;
                string departurecity = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDepatureBreakupEdit")).Text;
                string departuredate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDateEdit")).Text;
                string departuretime = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlDepartureTimeEdit")).SelectedValue;
                string destinationcityid = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationIdBreakupEdit")).Text;
                string destinationcity = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationBreakupEdit")).Text;
                string arrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDateEdit")).Text;
                string arrivaltime = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlArrivalTimeEdit")).SelectedValue;
                string ucClassEdit = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucClassEdit")).SelectedHard;

                if (!IsValidTravelBreakUp(departurecity, departurecityid, departuredate, destinationcity, destinationcityid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewHRTravelRequest.HRTravelBreakupUpdate(new Guid(travelrequestid)
                    , new Guid(travelbreakupid)
                    , int.Parse(ViewState["PERSONALINFOSN"].ToString())
                    , null
                    , null
                    , int.Parse(departurecityid)
                    , General.GetNullableString(departurecity)
                    , DateTime.Parse(departuredate)
                    , int.Parse(departuretime)
                    , int.Parse(destinationcityid)
                    , General.GetNullableString(destinationcity)
                    , General.GetNullableDateTime(arrivaldate)
                    , General.GetNullableInteger(arrivaltime)
                    , General.GetNullableInteger(ucClassEdit)
                    );

                ucStatus.Text = "Travel Breakup has been updated.";
                _gridView.EditIndex = -1;
                BindBreakUpData();

            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                string travelrequestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelRequestId")).Text;
                string travelbreakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelBreakupId")).Text;
                string breakuprn = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSerialNo")).Text;

                PhoenixCrewHRTravelRequest.HRTravelBreakupDelete(new Guid(travelrequestid)
                    , new Guid(travelbreakupid)
                    , int.Parse(breakuprn));

                ucStatus.Text = "Travel Breakup is deleted.";
                BindBreakUpData();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTravelRequestBreakup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            DropDownList ddldeparturetime = (DropDownList)e.Row.FindControl("ddlDepartureTimeEdit");
            if (ddldeparturetime != null)
            {
                ddldeparturetime.SelectedValue = drv["FLDDEPATURETIME"].ToString();
            }

            DropDownList ddlarrivaltime = (DropDownList)e.Row.FindControl("ddlArrivalTimeEdit");
            if (ddlarrivaltime != null)
            {
                ddlarrivaltime.SelectedValue = drv["FLDARRIVALTIME"].ToString();
            }

        }
    }
    protected void gvTravelRequestBreakup_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = -1;
        BindBreakUpData();
    }
    protected void gvTravelRequestBreakup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        BindBreakUpData();
    }
    protected void gvTravelRequestBreakup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindBreakUpData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTravelRequestBreakup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindBreakUpData();
    }
    protected void gvTravelRequestBreakup_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.NewSelectedIndex;
    }
}
