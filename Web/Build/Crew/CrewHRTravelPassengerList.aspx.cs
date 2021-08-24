using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class CrewHRTravelPassengerList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST");
            toolbarmain.AddButton("Details", "PASSENGERS");
            toolbarmain.AddButton("Quotation", "QUOTATION");
            toolbarmain.AddButton("Tickets", "TICKETS");

            MenuTravelPassengerMain.AccessRights = this.ViewState;
            MenuTravelPassengerMain.MenuList = toolbarmain.Show();
            MenuTravelPassengerMain.SelectedMenuIndex = 1;

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

                if (Request.QueryString["page"] != null && Request.QueryString["page"].ToString() != "")
                    ViewState["PAGE"] = Request.QueryString["page"].ToString();
                else
                    ViewState["PAGE"] = "";

                lblTitle.Text = "Details (" + PhoenixCrewTravelRequest.RequestNo + ")";

                ViewState["ECC"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 227, "ECC");
                ViewState["DEPCITY"] = null;
                gvTravelPassenger.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewHRTravelPassengerList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTravelPassenger')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Add','','" + Session["sitepath"] + "/Crew/CrewHRTravelPassengerFamilySelectionList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString() + "'); return false;", "New Travel Request", "<i class=\"fa fa-plus-circle\"></i>", "ADD");            
            MenuTravelPassenger.AccessRights = this.ViewState;
            MenuTravelPassenger.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.Title = lblTitle.Text;
            MenuTitle.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TravelPassengerMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                if (ViewState["PAGE"].ToString() == "approval")
                {
                    Response.Redirect("../Crew/CrewHRTravelRequestApprovalList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString(), false);
                }
                else
                {
                    Response.Redirect("../Crew/CrewHRTravelRequestList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString(), false);
                }
            }
            else if (CommandName.ToUpper().Equals("PASSENGERS"))
            {
                Response.Redirect("../Crew/CrewHRTravelPassengerList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString() + "&page=" + ViewState["PAGE"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("QUOTATION"))
            {
                Response.Redirect("../Crew/CrewHRTravelQuotationAgentDetail.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString() + "&page=" + ViewState["PAGE"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("TICKETS"))
            {
                Response.Redirect("../Crew/CrewHRTravelTicketList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString() + "&page=" + ViewState["PAGE"].ToString(), false);
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
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
                    , gvTravelPassenger.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Travel Passenger List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    private void BindBreakUpData()
    {
        try
        {
            DataTable dt = PhoenixCrewHRTravelRequest.HRTravelRequestBreakUpSearch(General.GetNullableGuid(ViewState["TRAVELREQUESTID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                gvTravelRequestBreakup.DataSource = dt;               
            }
            else
            {
                gvTravelRequestBreakup.DataSource = "";
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
                    , gvTravelPassenger.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

            General.SetPrintOptions("gvTravelPassenger", "Travel Passenger List", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTravelPassenger.DataSource = ds;
                gvTravelPassenger.VirtualItemCount = iRowCount;
                if (ViewState["TRAVELPASSENGERID"] == null)
                {
                    ViewState["TRAVELPASSENGERID"] = ds.Tables[0].Rows[0]["FLDTRAVELPASSENGERID"].ToString();
                    
                }
                SetRowSelection();
            }
            else
            {
                gvTravelPassenger.DataSource = "";
            }
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
            for (int i = 0; i < gvTravelPassenger.MasterTableView.Items.Count; i++)
            {
                if (gvTravelPassenger.MasterTableView.Items[i].GetDataKeyValue("FLDTRAVELPASSENGERID").ToString().Equals(ViewState["TRAVELPASSENGERID"].ToString()))
                {
                    gvTravelPassenger.MasterTableView.Items[i].Selected = true;
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvTravelPassenger.Rebind();
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTravelBreakUp(string departurecity, string departurecityid, string departuredate,
        string departuretime, string destinationcity, string destinationcityid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(departurecity.Trim()) && General.GetNullableInteger(departurecityid) == null)
            ucError.ErrorMessage = "Origin is required.";

        if (string.IsNullOrEmpty(destinationcity.Trim()) && General.GetNullableInteger(destinationcityid) == null)
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Departure Date is required.";

        if (General.GetNullableInteger(departuretime) == null)
            ucError.ErrorMessage = "Departure Time is required";

        return (!ucError.IsError);
    }

    protected void gvTravelRequestBreakup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                string travelrequestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                string travelbreakupid = ((RadLabel)e.Item.FindControl("lblTravelBreakupId")).Text;
                string departurecity = ((RadTextBox)e.Item.FindControl("txtDepatureBreakupEdit")).Text;
                string departurecityid = ((RadTextBox)e.Item.FindControl("txtDepatureIdBreakupEdit")).Text;
                string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateEdit")).Text;
                string departuretime = ((RadComboBox)e.Item.FindControl("ddlDepartureTimeEdit")).SelectedValue;
                string destinationcity = ((RadTextBox)e.Item.FindControl("txtDestinationBreakupEdit")).Text;
                string destinationcityid = ((RadTextBox)e.Item.FindControl("txtDestinationIdBreakupEdit")).Text;
                string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDateEdit")).Text;
                string arrivaltime = ((RadComboBox)e.Item.FindControl("ddlArrivalTimeEdit")).SelectedValue;
                string ucclassadd = ((UserControlHard)e.Item.FindControl("ucTravelClassEdit")).SelectedHard;

                if (!IsValidTravelBreakUp(departurecity, departurecityid, departuredate, departuretime, destinationcity, destinationcityid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewHRTravelRequest.HRTravelBreakupUpdate(new Guid(travelrequestid)
                    , new Guid(travelbreakupid)
                    //, int.Parse("1")
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
                    , General.GetNullableInteger(ucclassadd));

                ucStatus.Text = "Travel Breakup has been updated.";               
                BindBreakUpData();
                gvTravelRequestBreakup.Rebind();
            }

            if (e.CommandName.ToUpper() == "ADD")
            {
                string departurecityid = ((RadTextBox)e.Item.FindControl("txtDepatureIdBreakupAdd")).Text;
                string departurecity = ((RadTextBox)e.Item.FindControl("txtDepatureBreakupAdd")).Text;
                string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateAdd")).Text;
                string departuretime = ((RadComboBox)e.Item.FindControl("ddlDepartureTimeAdd")).SelectedValue;
                string destinationcityid = ((RadTextBox)e.Item.FindControl("txtDestinationIdBreakupAdd")).Text;
                string destinationcity = ((RadTextBox)e.Item.FindControl("txtDestinationBreakupAdd")).Text;
                string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDateAdd")).Text;
                string arrivaltime = ((RadComboBox)e.Item.FindControl("ddlArrivalTimeAdd")).SelectedValue;
                string ucclassadd = ((UserControlHard)e.Item.FindControl("ucTravelClassAdd")).SelectedHard;

                if (!IsValidTravelBreakUp(departurecity, departurecityid, departuredate, departuretime, destinationcity, destinationcityid))
                {
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableGuid(ViewState["TRAVELREQUESTID"].ToString()) != null)
                {
                    PhoenixCrewHRTravelRequest.HRTravelBreakupInsert(new Guid(ViewState["TRAVELREQUESTID"].ToString())
                                                                     , int.Parse(ViewState["PERSONALINFOSN"].ToString())
                                                                     //, int.Parse("1")
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
                                                                     , General.GetNullableInteger(ucclassadd)
                                                                    );
                }
                ucStatus.Text = "Travel Breakup is added.";
                BindBreakUpData();
                gvTravelRequestBreakup.Rebind();
            }

            else if (e.CommandName.ToUpper() == "DELETE")
            {
                string travelrequestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                string travelbreakupid = ((RadLabel)e.Item.FindControl("lblTravelBreakupId")).Text;
                string breakuprn = ((RadLabel)e.Item.FindControl("lblSerialNo")).Text;

                PhoenixCrewHRTravelRequest.HRTravelBreakupDelete(new Guid(travelrequestid)
                    , new Guid(travelbreakupid)
                    , int.Parse(breakuprn));

                ucStatus.Text = "Travel Breakup is deleted.";
                BindBreakUpData();
                gvTravelRequestBreakup.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelRequestBreakup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadTextBox departurecityid = ((RadTextBox)e.Item.FindControl("txtDepatureBreakupEdit"));
            RadTextBox departurecity = ((RadTextBox)e.Item.FindControl("txtDepatureIdBreakupEdit"));
            UserControlDate departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateEdit"));
            RadComboBox departuretime = ((RadComboBox)e.Item.FindControl("ddlDepartureTimeEdit"));
            RadTextBox newdestinationcityid = ((RadTextBox)e.Item.FindControl("txtDestinationIdBreakupEdit"));
            RadTextBox newdestinationcity = ((RadTextBox)e.Item.FindControl("txtDestinationBreakupEdit"));
            UserControlDate newarrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDateEdit"));
            RadComboBox newarrivaltime = ((RadComboBox)e.Item.FindControl("ddlArrivalTimeEdit"));
            ImageButton btnShowDestinationbreakupAdd = (ImageButton)e.Item.FindControl("btnShowDestinationbreakupEdit");
            ImageButton btnShowDepaturebreakup = (ImageButton)e.Item.FindControl("btnShowDepaturebreakup");
            LinkButton cmdRowSave = (LinkButton)e.Item.FindControl("cmdRowSave");
            UserControlHard ucclassEdit = (UserControlHard)e.Item.FindControl("ucTravelClassEdit");

            if (ucclassEdit != null)
            {
                ucclassEdit.HardList = PhoenixRegistersHard.ListHard(1, 227);
                if (ucclassEdit.SelectedHard == "")
                    ucclassEdit.SelectedHard = drv["FLDTRAVELCLASSID"].ToString();                
            }

            if (departuretime != null)
            {
                if (General.GetNullableInteger(drv["FLDDEPATURETIME"].ToString()) != null)
                    departuretime.SelectedValue = drv["FLDDEPATURETIME"].ToString();
            }

            ViewState["DEPCITY"] = drv["FLDLASTDESTINATIONCITYNAME"].ToString();
            ViewState["DEPCITYID"] = drv["FLDLASTDESTINATIONCITYID"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlHard ucClass = (UserControlHard)e.Item.FindControl("ucTravelClassAdd");
            if (ucClass != null)
            {
                ucClass.SelectedHard = ViewState["ECC"].ToString();
            }
            RadTextBox departurecity = ((RadTextBox)e.Item.FindControl("txtDepatureBreakupAdd"));
            RadTextBox departurecityid = ((RadTextBox)e.Item.FindControl("txtDepatureIdBreakupAdd"));

            if (departurecityid != null)
            {
                departurecityid.Text = ViewState["DEPCITYID"].ToString();
            }

            if (departurecity != null)
            {
                departurecity.Text = ViewState["DEPCITY"].ToString();
            }

        }
    }

    protected void gvTravelRequestBreakup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindBreakUpData();
    }

    protected void gvTravelPassenger_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

            else if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                ViewState["TRAVELPASSENGERID"] = ((RadLabel)e.Item.FindControl("lblTravelPassengerId")).Text;
                ViewState["PERSONALINFOSN"] = ((RadLabel)e.Item.FindControl("lblpersonalinfosn")).Text;
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                string familysn = ((RadLabel)e.Item.FindControl("lblFamilymembersn")).Text;

                PhoenixCrewHRTravelRequest.HRTravelPassengerUpdate(new Guid(ViewState["TRAVELPASSENGERID"].ToString())
                    , new Guid(ViewState["TRAVELREQUESTID"].ToString())
                    , int.Parse(ViewState["PERSONALINFOSN"].ToString())
                    , ((RadLabel)e.Item.FindControl("lblSalutation1")).Text
                    , ((RadTextBox)e.Item.FindControl("txtPassportNoEdit")).Text
                    , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDOBEdit")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtGenderEdit")).Text
                    , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("lblPassportDOEEdit")).Text)
                    , General.GetNullableInteger(familysn)
                    );

                ucStatus.Text = "Passenger detail is added.";
                BindData();
                gvTravelPassenger.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixCrewHRTravelRequest.HRTravelPassengerDelete(new Guid(ViewState["TRAVELPASSENGERID"].ToString())
                    , new Guid(ViewState["TRAVELREQUESTID"].ToString()));
                BindData();
                gvTravelPassenger.Rebind();
                ViewState["TRAVELPASSENGERID"] = null;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelPassenger_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelPassenger.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvTravelPassenger_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton select = (LinkButton)e.Item.FindControl("cmdSelect");
                if (select != null) select.Visible = SessionUtil.CanAccess(this.ViewState, select.CommandName);

                LinkButton delete = (LinkButton)e.Item.FindControl("cmdDelete");
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
}
    