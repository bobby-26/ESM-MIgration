using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewTravelRequestAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ucstatus.Visible = false;
            if (!IsPostBack)
            {
                Filter.CurrentTravelRequestSelection = null;
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindVesselAccount();
                if (Request.QueryString["travelid"] != null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();
                    SetTravel();
                }
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelRequestAdd.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelRequestAdd.aspx", "Add Crew", "<i class=\"fas fa-users-crew\"></i>", "ADD");            
            MenuTravel.AccessRights = this.ViewState;
            MenuTravel.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "BACK", ToolBarDirection.Right);            
            MenuHeader.AccessRights = this.ViewState;
            MenuHeader.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmenu = new PhoenixToolbar();
            ViewState["TRAVELID"] = null;

            if (Request.QueryString["travelid"] != null)
            {
                ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();
                toolbarmenu.AddButton("Save", "SAVE", ToolBarDirection.Right);

                Menuapprove.AccessRights = this.ViewState;
                Menuapprove.MenuList = toolbarmenu.Show();
                
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetTravel()
    {
        DataSet ds = null;
        if (ViewState["TRAVELID"] != null)
        {
            ds = PhoenixCrewTravelRequest.EditTravel(new Guid(ViewState["TRAVELID"].ToString()));
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            ucvessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ddlAccountDetails.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELACCOUNTID"].ToString();
            txtOrigin.SelectedValue = ds.Tables[0].Rows[0]["FLDORGINID"].ToString();
            txtOrigin.Text = ds.Tables[0].Rows[0]["FLDORIGINNAME"].ToString();
            txtorigindate.Text = ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString();
            ddlampmOriginDate.SelectedValue = ds.Tables[0].Rows[0]["FLDDEPARTUREAMPM"].ToString();
            ddlampmarrival.SelectedValue = ds.Tables[0].Rows[0]["FLDARRIVALAMPM"].ToString();
            txtDestination.SelectedValue = ds.Tables[0].Rows[0]["FLDDESTINATIONID"].ToString();
            txtDestination.Text = ds.Tables[0].Rows[0]["FLDDESTINATIONNAME"].ToString();
            txtDestinationdate.Text = ds.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString();
            ucpurpose.SelectedReason = ds.Tables[0].Rows[0]["FLDTRAVELTYPE"].ToString();
            Payment.SelectedHard = ds.Tables[0].Rows[0]["FLDPAYMENTMODE"].ToString();

            MenuHeader.Title = "Travel Request (" + ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString() + ")";
            setFilter();
        }

    }

    private void setFilter()
    {
        NameValueCollection criteria = Filter.CurrentTravelRequestSelection;

        if (criteria == null)
        {
            string travelid = null;
            travelid = (ViewState["TRAVELID"] == null) ? null : (ViewState["TRAVELID"].ToString());

            criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtorigindate", txtorigindate.Text);
            criteria.Add("txtDestinationdate", txtDestinationdate.Text);
            criteria.Add("ddlampmarrival", ddlampmarrival.SelectedValue);
            criteria.Add("txtOrigin", txtOrigin.SelectedValue);
            criteria.Add("ddlampmOriginDate", ddlampmOriginDate.SelectedValue);
            criteria.Add("txtDestination", txtDestination.SelectedValue);
            criteria.Add("ucvessel", ucvessel.SelectedVessel);
            criteria.Add("Payment", Payment.SelectedHard);
            criteria.Add("ucpurpose", ucpurpose.SelectedReason);
            criteria.Add("ddlAccountDetails", ddlAccountDetails.SelectedValue);
            criteria.Add("travelid", travelid);
            Filter.CurrentTravelRequestSelection = criteria;
        }
    }


    protected void Menuapprove_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTravelRequest())
                {
                    ucError.Visible = true;
                    return;
                }

                NameValueCollection nvc = Filter.CurrentTravelRequestSelection;
                
                nvc = new NameValueCollection();
                nvc.Clear();
                nvc.Add("txtorigindate", txtorigindate.Text);
                nvc.Add("txtDestinationdate", txtDestinationdate.Text);
                nvc.Add("ddlampmarrival", ddlampmarrival.SelectedValue);
                nvc.Add("txtOrigin", txtOrigin.SelectedValue);
                nvc.Add("ddlampmOriginDate", ddlampmOriginDate.SelectedValue);
                nvc.Add("txtDestination", txtDestination.SelectedValue);
                nvc.Add("ucvessel", ucvessel.SelectedVessel);
                nvc.Add("Payment", Payment.SelectedHard);
                nvc.Add("ucpurpose", ucpurpose.SelectedReason);
                nvc.Add("ddlAccountDetails", ddlAccountDetails.SelectedValue);

                if (ViewState["TRAVELID"] != null)
                {
                    nvc.Add("travelid", ViewState["TRAVELID"].ToString());
                }

                Filter.CurrentTravelRequestSelection = nvc;

                if (ViewState["TRAVELID"] != null)
                {
                 PhoenixCrewTravelRequest.UpdateTravelRequest(General.GetNullableGuid(ViewState["TRAVELID"].ToString())
                                                                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtorigindate") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlampmOriginDate") : string.Empty)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDestinationdate") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlampmarrival") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ucvessel") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("Payment") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ucpurpose") : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlAccountDetails") : string.Empty)
                                                                  );

                    ucstatus.Text = "Information Updated";
                    ucstatus.Visible = true;                
                }
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewTravelRequest.aspx");
            }        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidTravelRequest())
                {
                    ucError.Visible = true;
                    return;
                }

                setFilter();

                string sScript = "javascript:openNewWindow('BookMarkScript','','" + Session["sitepath"] + "/Crew/CrewTravelRequestEmployeeList.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKNAME", "FLDPASSPORTNO" };
                string[] alCaptions = { "File No.", "Name", "Rank", "Passport No." };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.CurrentTravelRequestSelection;

                DataSet ds = PhoenixCrewTravelRequest.TravelRequestGeneralSearch(General.GetNullableGuid(nvc != null ? nvc.Get("travelid") : string.Empty),
                                                                                 sortexpression, sortdirection,
                                                                                 (int)ViewState["PAGENUMBER"],
                                                                                 General.ShowRecords(null),
                                                                                 ref iRowCount,
                                                                                 ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Travel Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTravelRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ucvessel.SelectedVessel.ToUpper() == "DUMMY" || General.GetNullableInteger(ucvessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";
        
        DateTime resultDepDate;
        //DateTime resultArrivalDate;

        if (General.GetNullableInteger(ucvessel.SelectedVessel) != null & General.GetNullableInteger(ucvessel.SelectedVessel) > 0)
        {
            if (General.GetNullableInteger(ddlAccountDetails.SelectedValue) == null)
                ucError.ErrorMessage = "Vessel Account is required.";
        }
        if (txtOrigin.SelectedValue.ToUpper() == "DUMMY" || General.GetNullableInteger(txtOrigin.SelectedValue) == null)
            ucError.ErrorMessage = "Departure is required.";

        if (General.GetNullableDateTime(txtorigindate.Text) == null)
            ucError.ErrorMessage = "Departure Date is required.";

        if (General.GetNullableInteger(ddlampmOriginDate.SelectedValue) == null)
            ucError.ErrorMessage = "Departure Time is required.";

        //if (General.GetNullableDateTime(txtorigindate.Text) != null)
        //{
        //    if (DateTime.TryParse(txtorigindate.Text, out resultDepDate) && DateTime.Compare(resultDepDate, DateTime.Now) < 1)
        //    {
        //        ucError.ErrorMessage = "Departure Date should be greater than current date";
        //    }
        //}
        //if (General.GetNullableDateTime(txtDestinationdate.Text) != null)
        //{
        //    if (DateTime.TryParse(txtDestinationdate.Text, out resultArrivalDate) && DateTime.Compare(resultArrivalDate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
        //    {
        //        ucError.ErrorMessage = "Arrival Date should be greater than current date";
        //    }
        //}

        if (General.GetNullableDateTime(txtDestinationdate.Text) != null)
        {
            if (DateTime.TryParse(txtorigindate.Text, out resultDepDate) && DateTime.Compare(resultDepDate, DateTime.Parse(txtDestinationdate.Text)) > 0)
            {
                ucError.ErrorMessage = "Arrival Date should be greater than Departure date";
            }
        }
        
        if (txtDestination.SelectedValue.ToUpper() == "DUMMY" || General.GetNullableInteger(txtDestination.SelectedValue) == null)
            ucError.ErrorMessage = "Destination is required.";

        if (ucpurpose.SelectedReason.ToUpper() == "DUMMY" || General.GetNullableInteger(ucpurpose.SelectedReason) == null)
            ucError.ErrorMessage = "Purpose is required.";

        if (Payment.SelectedHard.ToUpper() == "DUMMY" || General.GetNullableInteger(Payment.SelectedHard) == null)
            ucError.ErrorMessage = "Payment Mode is required.";

            return (!ucError.IsError);
    }

    public void BindVesselAccount()
    {
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(ucvessel.SelectedVessel) == 0 ? null : General.GetNullableInteger(ucvessel.SelectedVessel), 1);
        ddlAccountDetails.DataBind();
    }
    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ucVessel_OnTextChanged(object sender, EventArgs e)
    {
        BindVesselAccount();
    }

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKNAME", "FLDPASSPORTNO" };
        string[] alCaptions = { "File No.", "Name", "Rank", "Passport No." };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentTravelRequestSelection;
        try
        {
            DataSet ds = PhoenixCrewTravelRequest.OfficeTravelRequestGeneralSearch(
                General.GetNullableGuid(nvc != null ? nvc.Get("travelid") : string.Empty),
                sortexpression, sortdirection,
                int.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCrewSearch.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvCrewSearch", "Travel Request", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewSearch.DataSource = ds.Tables[0];
                gvCrewSearch.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCrewSearch.DataSource = "";
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {       
        BindData();
        gvCrewSearch.Rebind();
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

            else if (e.CommandName.ToUpper() == "DELETE")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;

                if (General.GetNullableGuid(requestid) != null)
                {
                    PhoenixCrewTravelRequest.DeleteTravelRequest(new Guid(requestid));
                    BindData();
                    gvCrewSearch.Rebind();
                }
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

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {       
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }
}