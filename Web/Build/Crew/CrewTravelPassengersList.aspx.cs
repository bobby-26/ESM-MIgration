using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewTravelPassengersList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

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
                gvCCT.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                PhoenixToolbar toolbar1 = new PhoenixToolbar();
                menutab.AccessRights = this.ViewState;
                menutab.Title = PhoenixCrewTravelRequest.RequestNo;
                menutab.MenuList = toolbar1.Show();

            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("New", "GENERAL");
            toolbar.AddButton("Passengers List", "PASSENGERSLIST");
            if (ViewState["TRAVELID"] != null)
            {
                toolbar.AddButton("Passenger Entry", "PASSENGERSENTRY");
            }
           
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();
            CrewMenu.SelectedMenuIndex = 1;
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("CrewTravelNewRequest.aspx?travelid=" + ViewState["TRAVELID"], false);
            }
            if (General.GetNullableGuid(ViewState["TRAVELID"] == null ? null : ViewState["TRAVELID"].ToString()) == null)
            {
                Showerror();
                return;
            }
            if (CommandName.ToUpper().Equals("PASSENGERSLIST"))
            {
                Response.Redirect("CrewTravelPassengersList.aspx?travelid=" + ViewState["TRAVELID"], false);
            }
            if (CommandName.ToUpper().Equals("PASSENGERSENTRY"))
            {
                if (PhoenixCrewTravelRequest.OfficeTravelPassengerFrom.ToString() == "1")
                    Response.Redirect("CrewTravelPassengersSeafarerEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
                else
                    Response.Redirect("CrewTravelPassengersEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
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
        ucError.ErrorMessage = "Create Travel Request before adding passengers.";
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
                gvCCT.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvCCT.DataSource = ds;
            gvCCT.VirtualItemCount = iRowCount;
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
            gvCCT.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvCCT_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (ViewState["EDITTRAVELREQUEST"] != null && !ViewState["EDITTRAVELREQUEST"].ToString().Equals("1"))
                    db.Visible = false;
                else
                    db.Visible = true;
            }
        }
    }

    protected void gvCCT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCT.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCCT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["REQUESTID"] = ((RadLabel)(e.Item.FindControl("lblTravelRequestId"))).Text;
            if (PhoenixCrewTravelRequest.OfficeTravelPassengerFrom.ToString() == "1")
                Response.Redirect("CrewTravelPassengersSeafarerEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&requestid=" + ViewState["REQUESTID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            else
                Response.Redirect("CrewTravelPassengersEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&requestid=" + ViewState["REQUESTID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
        }
        else if (e.CommandName.ToUpper() == "DELETE")
        {
            string requestid = ((RadLabel)(e.Item.FindControl("lblTravelRequestId"))).Text;
            PhoenixCrewTravelRequest.DeleteTravelRequest(new Guid(requestid));
            BindData();
            gvCCT.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}