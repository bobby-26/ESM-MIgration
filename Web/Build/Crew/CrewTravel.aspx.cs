using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewTravel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravel.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravel.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
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

            dt = PhoenixCrewTravelAdmin.SearchCrewTravelAdmin(General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableInteger(ucport.SelectedValue)
                , General.GetNullableDateTime(txtDateOfCrewChange.Text)
                , General.GetNullableString(txtRequisitiono.Text)
                , General.GetNullableDateTime(txtFromDate.Text)
                , General.GetNullableDateTime(txtToDate.Text));

            if (dt.Rows.Count > 0)
            {
                gvTravelSearch.DataSource = dt;                
            }
            else
            {
                gvTravelSearch.DataSource = "";
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
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvTravelSearch.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucVessel.SelectedVessel = "";
                ucport.SelectedValue    = "";
                ucport.Text = "";
                txtRequisitiono.Text    = "";
                txtDateOfCrewChange.Text = "";
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
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvTravelSearch.Rebind();
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


    protected void gvTravelSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {       
        try
        {
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                Guid? id = General.GetNullableGuid((e.Item as GridEditableItem).GetDataKeyValue("FLDTRAVELID").ToString());
                string ReqNo = ((RadTextBox)e.Item.FindControl("txtRequestNoEdit")).Text;
                UserControlSeaport ucPort = (UserControlSeaport)e.Item.FindControl("ucPortEdit");
                string port = ucPort.SelectedSeaport;
                string crewchangedate = ((UserControlDate)e.Item.FindControl("txtCrewChangeDateAdd")).Text;
                UserControlHard travelstatus = (UserControlHard)e.Item.FindControl("uctravelstatus");
                string status = travelstatus.SelectedHard;

                if (General.GetNullableString(ReqNo) == null)
                {
                    ucError.ErrorMessage = "Request No. Required";
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewTravelAdmin.UpdateCrewTravelAdmin(id
                    , General.GetNullableString(ReqNo)
                    , General.GetNullableInteger(port)
                    , General.GetNullableDateTime(crewchangedate)
                    , General.GetNullableInteger(status));
                BindData();
                gvTravelSearch.Rebind();
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {   
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {        
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }

            UserControlSeaport ddlSeaPort = (UserControlSeaport)e.Item.FindControl("ucPortEdit");
            UserControlHard ddlstatus = (UserControlHard)e.Item.FindControl("uctravelstatus");

            if (ddlSeaPort != null) ddlSeaPort.SelectedSeaport = drv["FLDPORTOFCREWCHANGE"].ToString();
            if (ddlstatus != null) ddlstatus.SelectedHard = drv["FLDTRAVELSTATUS"].ToString();
        }
    }
}
