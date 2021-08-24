using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewHRTravelPassengerGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");

            if (Request.QueryString["travelrequestid"] != null && Request.QueryString["travelrequestid"].ToString() != "")
                ViewState["TRAVELREQUESTID"] = Request.QueryString["travelrequestid"].ToString();
            else
                ViewState["TRAVELREQUESTID"] = "";

            if (Request.QueryString["personalinfosn"] != null && Request.QueryString["personalinfosn"].ToString() != "")
                ViewState["PERSONALINFOSN"] = Request.QueryString["personalinfosn"].ToString();
            else
                ViewState["PERSONALINFOSN"] = "";
        }
        BindBreakUpData();
        BindPassengerData();
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

    private void BindPassengerData()
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void HRTravelPassengerGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("NEW"))
            {

            }
            else if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
       
    }

    protected void gvTravelRequestBreakup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

            GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper() == "ADD")
            {
                Guid travelrequestid = new Guid();
                int personalinfosn = 0;

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

                PhoenixCrewHRTravelRequest.HRTravelRequestInsert(null
                    , General.GetNullableInteger(ddlVessel.SelectedVessel)
                    , int.Parse(departurecityid)
                    , General.GetNullableString(departurecity)
                    , DateTime.Parse(departuredate)
                    , int.Parse(departuretime)
                    , int.Parse(destinationcityid)
                    , General.GetNullableString(destinationcity)
                    , General.GetNullableDateTime(arrivaldate)
                    , General.GetNullableInteger(arrivaltime)
                    , ref travelrequestid
                    , ref personalinfosn);

                //if (travelrequestid != null)
                //{
                //    PhoenixCrewHRTravelRequest.HRTravelBreakupInsert(travelrequestid
                //    , int.Parse(personalinfosn.ToString())
                //    , null
                //    , General.GetNullableInteger(ddlVessel.SelectedVessel)
                //    , int.Parse(departurecityid)
                //    , General.GetNullableString(departurecity)
                //    , DateTime.Parse(departuredate)
                //    , int.Parse(departuretime)
                //    , int.Parse(destinationcityid)
                //    , General.GetNullableString(destinationcity)
                //    , General.GetNullableDateTime(arrivaldate)
                //    , General.GetNullableInteger(arrivaltime));
                //}
                ucstatus.Text = "Travel Breakup is added";
                BindBreakUpData();
                Response.Redirect("../Crew/CrewHRTravelPassengerList.aspx?travelrequestid=" + travelrequestid + "&personalinfosn=" + personalinfosn, false);

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
            if (e.Row.RowIndex == 0)
            {
                if (db != null)
                    db.Visible = false;
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
}
