using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewTravelRequestAdmin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelRequestAdmin.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelRequestAdmin.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuTravel.AccessRights = this.ViewState;
            MenuTravel.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {

        try
        {
            DataTable dt = new DataTable();
            dt = PhoenixCrewTravelAdmin.SearchCrewTravelRequestAdmin(General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableDateTime(txtDateofCrewChange.Text)
                , General.GetNullableString(txtRequisitiono.Text)
                , General.GetNullableDateTime(txtFromDate.Text)
                , General.GetNullableDateTime(txtToDate.Text));

            gvRequestSearch.DataSource = dt;

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
            gvRequestSearch.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
    }
    protected void MenuTravel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvRequestSearch.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucVessel.SelectedVessel = "";
                txtRequisitiono.Text = "";
                txtDateofCrewChange.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRequestSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                Guid? requestid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text);
                Guid? travelid = General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtTravelIdEdit")).Text);
                string ReqNo = ((RadTextBox)e.Item.FindControl("txtRequestNoEdit")).Text;
                string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateEdit")).Text;
                string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDateEdit")).Text;

                if (General.GetNullableString(ReqNo) == null)
                {
                    ucError.ErrorMessage = "Request No. Required";
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewTravelAdmin.UpdateCrewTravelRequestAdmin(requestid
                    , General.GetNullableString(ReqNo)
                    , General.GetNullableDateTime(departuredate)
                    , General.GetNullableDateTime(arrivaldate)
                    , travelid);

                BindData();
                gvRequestSearch.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRequestSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvRequestSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
        }
    }
}
