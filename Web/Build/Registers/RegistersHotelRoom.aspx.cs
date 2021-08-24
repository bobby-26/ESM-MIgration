using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersHotelRoom : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersHotelRoom.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvHotelRoom')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersHotelRoom.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersHotelRoom.AccessRights = this.ViewState;
            MenuRegistersHotelRoom.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvHotelRoom.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDCODE", "FLDROOMTYPENAME", "FLDNOOFBEDS" };
        string[] alCaptions = { "Code", "Hotel Room Type Name", "Number Of Beds" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersHotelRoom.HotelRoomSearch(txtRoomTypeName.Text,
                    (int)ViewState["PAGENUMBER"],
                    gvHotelRoom.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=HotelRoom.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Hotel Room</h3></td>");
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
    protected void RegistersHotelRoom_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvHotelRoom.Rebind();
            }
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDROOMTYPENAME", "FLDNOOFBEDS" };
        string[] alCaptions = { "Code","Hotel Room Type Name", "Number Of Beds" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersHotelRoom.HotelRoomSearch(txtRoomTypeName.Text,
                     (int)ViewState["PAGENUMBER"],
                     gvHotelRoom.PageSize,
                     ref iRowCount,
                     ref iTotalPageCount);

        General.SetPrintOptions("gvHotelRoom", "Hotel Room", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvHotelRoom.DataSource = ds;
            gvHotelRoom.VirtualItemCount = iRowCount;
        }
        else
        {
            gvHotelRoom.DataSource = "";
        }
    }
    

    private void InsertHotelRoom(string roomtype, int noofbeds,string code)
    {
        PhoenixRegistersHotelRoom.InsertHotelRoom(PhoenixSecurityContext.CurrentSecurityContext.UserCode,roomtype,noofbeds,code);
    }
    private void UpdateHotelRoom(string roomtypeid,string roomtype, int noofbeds,string code)
    {
        PhoenixRegistersHotelRoom.UpdateHotelRoom(PhoenixSecurityContext.CurrentSecurityContext.UserCode,new Guid(roomtypeid.ToString()),roomtype, noofbeds,code);
    }
    private void DeleteHotelRoom(string roomtypeid)
    {
        PhoenixRegistersHotelRoom.DeleteHotelRoom(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(roomtypeid));
    }
   
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvHotelRoom.Rebind();
    }
  
    private bool IsValidHotelRoom(string RoomTypeName, string NoOfBeds,string code)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (RoomTypeName.Trim().Equals(""))
            ucError.ErrorMessage = "Room Type is required.";

        if (NoOfBeds.Trim().Equals(""))
            ucError.ErrorMessage = "No Of Beds Required";

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Short code is required.";

        return (!ucError.IsError);
    }
  

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvHotelRoom_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidHotelRoom(((RadTextBox)e.Item.FindControl("txtRoomTypeNameAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("ucNoOfBedsAdd")).Text,
                    (((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text)
                ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertHotelRoom(
                    ((RadTextBox)e.Item.FindControl("txtRoomTypeNameAdd")).Text,
                    int.Parse(((UserControlNumber)e.Item.FindControl("ucNoOfBedsAdd")).Text),
                    (((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text)
                );
                BindData();
                gvHotelRoom.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidHotelRoom(((RadTextBox)e.Item.FindControl("txtRoomTypeNameEdit")).Text,
                    ((UserControlNumber)e.Item.FindControl("ucNoOfBedsEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text
                ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateHotelRoom(
                     ((RadLabel)e.Item.FindControl("lblRoomTypeIdEdit")).Text.ToString(),
                     ((RadTextBox)e.Item.FindControl("txtRoomTypeNameEdit")).Text.ToString(),
                    int.Parse(((UserControlNumber)e.Item.FindControl("ucNoOfBedsEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text.ToString()
                );
                BindData();
                gvHotelRoom.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteHotelRoom(((RadLabel)e.Item.FindControl("lblRoomTypeId")).Text.ToString());
                BindData();
                gvHotelRoom.Rebind();
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

    protected void gvHotelRoom_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHotelRoom.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvHotelRoom_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvHotelRoom_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
