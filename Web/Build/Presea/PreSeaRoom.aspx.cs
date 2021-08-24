using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSea_PreSeaRoom : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["BLOCKID"] = Request.QueryString["blockname"];
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../PreSea/PreSeaRoom.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvPreSeaRoom')", "Print Grid", "icon_print.png", "PRINT");
                toolbar.AddImageButton("../PreSea/PreSeaRoom.aspx", "Find", "search.png", "FIND");
                MenuPreSeaRoom.AccessRights = this.ViewState;
                MenuPreSeaRoom.MenuList = toolbar.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlPreSeaBlock.SelectedBlock = ViewState["BLOCKID"].ToString();
                ddlPreSeaBlock.Enabled = false;
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDBLOCKNAME", "FLDROOMNAME", "FLDFLOOR", "FLDCAPACITY", "FLDISPROJECTORYN" };
        string[] alCaptions = { "Block Name", "Room Name", "Floor", "Capacity", "Projector Available" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPreSeaRoom.PreSeaRoomSearch(General.GetNullableInteger(ViewState["BLOCKID"].ToString())
            , txtRoomName.Text
            , null
            , General.GetNullableInteger(ddlIsProjectorAvailable.SelectedValue)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=DesignationinvoiceStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Infrastructure(Block) Room Details</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDBLOCKNAME", "FLDROOMNAME", "FLDFLOOR", "FLDCAPACITY", "FLDISPROJECTORYN" };
        string[] alCaptions = { "Block Name", "Room Name", "Floor", "Capacity", "Projector Available" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixPreSeaRoom.PreSeaRoomSearch(General.GetNullableInteger(ViewState["BLOCKID"].ToString())
            , txtRoomName.Text
            , txtFloor.Text
            , General.GetNullableInteger(ddlIsProjectorAvailable.SelectedValue)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        General.SetPrintOptions("gvPreSeaRoom", "Infrastructure(Block) Room Details", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaRoom.DataSource = ds;
            gvPreSeaRoom.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaRoom);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void PreSeaRoom_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPreSeaRoom.EditIndex = -1;
                gvPreSeaRoom.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
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

    private void InsertPreSeaRoom(int blockid, string roomname, int capacity, int? isprojectyn, string shortname)
    {
        PhoenixPreSeaRoom.InsertPreSeaRoom(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            blockid,
            roomname,
            capacity,
            isprojectyn,
            shortname
            );
    }

    private void UpdatePreSeaRoom(int roomid, int blockid, string roomname, int capacity, int? isprojectyn, string shortname)
    {
        PhoenixPreSeaRoom.UpdatePreSeaRoom(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            roomid,
            blockid,
            roomname,
            capacity,
            isprojectyn,
            shortname
            );
    }

    private void DeletePreSeaRoom(int roomid)
    {
        PhoenixPreSeaRoom.DeletePreSeaRoom(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            roomid);
    }

    private bool IsValidPreSeaRoom(string blockid, string roomname, string capacity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(blockid) == null)
        {
            ucError.ErrorMessage = "Block Name is required.";
        }
        if (roomname.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Room Name is required.";
        }
        if (General.GetNullableInteger(capacity) == null)
        {
            ucError.ErrorMessage = "Capacity is required.";
        }

        return (!ucError.IsError);
    }

    protected void gvPreSeaRoom_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }
    protected void gvPreSeaRoom_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
            String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaRoom(
                     ViewState["BLOCKID"].ToString(),
                     ((TextBox)_gridView.FooterRow.FindControl("txtRoomNameAdd")).Text,
                     ((TextBox)_gridView.FooterRow.FindControl("txtCapacityAdd")).Text
                     )
                    )
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaRoom(
                     int.Parse(ViewState["BLOCKID"].ToString()),
                     ((TextBox)_gridView.FooterRow.FindControl("txtRoomNameAdd")).Text,
                     int.Parse(((TextBox)_gridView.FooterRow.FindControl("txtCapacityAdd")).Text),
                     General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlIsProjectorRoomAdd")).SelectedValue),
                     ((TextBox)_gridView.FooterRow.FindControl("txtShortNameAdd")).Text
                 );
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPreSeaRoom(
                    ViewState["BLOCKID"].ToString(),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRoomNameEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCapacityEdit")).Text
                    )
                   )
                {
                    ucError.Visible = true;
                    return;
                }
                UpdatePreSeaRoom(
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblRoomIdEdit")).Text),
                    int.Parse(ViewState["BLOCKID"].ToString()),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRoomNameEdit")).Text,
                    int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCapacityEdit")).Text),
                    General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlIsProjectorRoomEdit")).SelectedValue),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortNameEdit")).Text
                 );
                _gridView.EditIndex = -1;
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletePreSeaRoom(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblRoomId")).Text));
            }
            BindData();
            SetPageNavigator();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreSeaRoom_RowDataBound(object sender, GridViewRowEventArgs e)
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
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            }

            UserControlPreSeaBlock ucBlock = (UserControlPreSeaBlock)e.Row.FindControl("ddlPreSeaBlockEdit");
            DataRowView drvBlock = (DataRowView)e.Row.DataItem;
            if (ucBlock != null)
            {
                ucBlock.SelectedBlock = drvBlock["FLDBLOCKID"].ToString();
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void gvPreSeaRoom_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvPreSeaRoom_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }
    protected void gvPreSeaRoom_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreSeaRoom.EditIndex = -1;
        gvPreSeaRoom.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvPreSeaRoom.EditIndex = -1;
        gvPreSeaRoom.SelectedIndex = -1;
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
        gvPreSeaRoom.SelectedIndex = -1;
        gvPreSeaRoom.EditIndex = -1;
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
}

