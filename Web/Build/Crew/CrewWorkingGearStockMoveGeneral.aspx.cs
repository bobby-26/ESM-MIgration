using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOperation;
using System.Text;
using Telerik.Web.UI;

public partial class CrewWorkingGearStockMoveGeneral : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;


                DataSet dsCurrency = PhoenixRegistersCurrency.ListCurrency(1, "INR");
                if (dsCurrency.Tables[0].Rows[0]["FLDCURRENCYID"] != null)
                    ViewState["LOCALCURRENCY"] = dsCurrency.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
                else
                    ViewState["LOCALCURRENCY"] = "";

                cblZone.DataSource = PhoenixRegistersMiscellaneousZoneMaster.ListMiscellaneousZoneMaster();
                cblZone.DataBind();

                ViewState["ORDERID"] = Request.QueryString["ORDERID"];
                ViewState["ACTIVE"] = "1";
                if (ViewState["ORDERID"] != null)
                    EditWorkGearOrder(new Guid(ViewState["ORDERID"].ToString()));
            }
            if (!String.IsNullOrEmpty(ViewState["ZONES"].ToString()))
            {
                divGrid.Visible = true;
                BindData();
            }
            else
            {
                divGrid.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkGearGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder str = new StringBuilder();
                foreach (ListItem item in cblZone.Items)
                {
                    if (item.Selected == true)
                    {
                        str.Append(item.Value.ToString());
                        str.Append(",");
                    }
                }
                if (str.Length > 1)
                {
                    str.Remove(str.Length - 1, 1);
                }

                if (String.IsNullOrEmpty(str.ToString()))
                    ucError.ErrorMessage = "Please select the zone list for moving Stock";

                if (ucError.IsError)
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewWorkingGearStockMove.UpdateStockMovingZoneList(new Guid(ViewState["ORDERID"].ToString()), str.ToString());

                ucStatus.Text = "Selected Zones saved successfully.";
                EditWorkGearOrder(new Guid(ViewState["ORDERID"].ToString()));

                if (!String.IsNullOrEmpty(ViewState["ZONES"].ToString()))
                {
                    divGrid.Visible = true;
                    BindData();
                }
            }
            if (dce.CommandName.ToUpper().Equals("SAVEITEMS"))
            {
                SaveMoveditems(gvWorkGearPendingItem);
            }
            string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1',null,null);";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditWorkGearOrder(Guid gOrderId)
    {
        try
        {
            DataTable dt = PhoenixCrewWorkingGearStockMove.EditOrderForm(gOrderId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
                txtSupplierName.Text = dr["FLDNAME"].ToString();
                txtReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();

                ViewState["ACTIVE"] = String.IsNullOrEmpty(dr["FLDSTOCKMOVEDZONES"].ToString()) ? "1" : "0";
                ViewState["ZONES"] = dr["FLDSTOCKMOVEDZONES"].ToString();
                ViewState["ZONENAMES"] = dr["FLDZONENAMES"].ToString();

                if (!String.IsNullOrEmpty(ViewState["ZONES"].ToString()))
                {
                    string strlist = ViewState["ZONES"].ToString();
                    strlist = "," + strlist + ",";
                    foreach (ListItem item in cblZone.Items)
                    {
                        item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
                    }
                }
                if (ViewState["ACTIVE"] != null && ViewState["ACTIVE"].ToString() == "0")
                {
                    cblZone.Enabled = false;
                }

                MainMenu();
            }
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
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string Zones = ViewState["ZONES"].ToString();

            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDQUANTITY" };
            string[] alCaptions = { "Name", "Quantity" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewWorkingGearStockMove.SearchStockMoveLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
               , Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), 50,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Requisition of Working Gear";

            General.SetPrintOptions("gvWorkGearPendingItem", title, alCaptions, alColumns, ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvWorkGearPendingItem.DataSource = ds;
                gvWorkGearPendingItem.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvWorkGearPendingItem);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuStockMoveItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel(gvWorkGearPendingItem);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        PhoenixToolbar toolbarSub = new PhoenixToolbar();
        if (ViewState["ACTIVE"] == null || ViewState["ACTIVE"].ToString() == "1")
        {
            toolbar.AddButton("Save", "SAVE");
        }
        if (!String.IsNullOrEmpty(ViewState["ZONES"].ToString()))
        {
            toolbar.AddButton("Save Items", "SAVEITEMS");
            toolbarSub.AddImageButton("../Crew/CrewWorkingGearStockMoveGeneral.aspx", "Export to Excel", "icon_xls.png", "Excel");

            MenuStockMoveItem.AccessRights = this.ViewState;
            MenuStockMoveItem.MenuList = toolbarSub.Show();
            MenuStockMoveItem.SetTrigger(pnlWorkGearStockMove);
        }
        
        MenuWorkGearGeneral.AccessRights = this.ViewState;
        MenuWorkGearGeneral.MenuList = toolbar.Show();
       
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
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
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvWorkGearPendingItem.SelectedIndex = -1;
        gvWorkGearPendingItem.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
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
        {
            return true;
        }

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

    protected void ucZone_DataBound(object sender, EventArgs e)
    {
        DropDownList dl = (DropDownList)((UserControlZone)sender).FindControl("ddlZone");

        dl.Items.Insert(0, new ListItem("For All Zone", "0"));
    }

    protected void gvWorkGearPendingItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView grdvw = (GridView)sender;
        int c = e.Row.Cells.Count;
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < c; i++)
            {
                string celltext = e.Row.Cells[i].Text;
                if (celltext.Contains("_"))
                    e.Row.Cells[i].Text = celltext.Remove(celltext.IndexOf('_'));
                else if (celltext.Contains("FLDWORKINGGEARITEMNAME"))
                    e.Row.Cells[i].Text = "Item Name";
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < c; i++)
            {
                string celltext = e.Row.Cells[i].Text;
                if (celltext.Contains("_"))
                {
                    TextBox txtStock = new TextBox();
                    Label lbl = new Label();
                    string zoneid = celltext.Substring(1);
                    e.Row.Cells[i].Text = "";
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    if (lbl != null)
                    {
                        lbl.ID = "lblzoneid_" + zoneid;
                        lbl.Text = zoneid;
                        lbl.Visible = false;
                        e.Row.Cells[i].Controls.Add(lbl);
                    }
                    if (txtStock != null)
                    {
                        txtStock.ID = "txtStock_" + zoneid;
                        txtStock.CssClass = "txtNumber small input";
                        txtStock.Style.Add("width", "75px");
                        txtStock.Attributes["onkeydown"] = "return txtkeypress(event, this, true,null,true);";
                        txtStock.Attributes["onblur"] = "return CalculateTotal('" + e.Row.RowIndex + "');";
                        e.Row.Cells[i].Controls.Add(txtStock);
                    }
                }
                else if (celltext.Contains("TOT"))
                {
                    e.Row.Cells[i].Text = "";
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    TextBox txttot = new TextBox();
                    txttot.ID = "txtTotal";
                    txttot.Text = "0";
                    txttot.Enabled = false;
                    txttot.CssClass = "readonlytextbox txtNumber";
                    txttot.Style.Add("width", "75px");
                    e.Row.Cells[i].Controls.Add(txttot);
                }
            }

        }
    }

    protected void gvWorkGearPendingItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SaveMoveditems(GridView _gridView)
    {
        try
        {
            foreach (GridViewRow r in gvWorkGearPendingItem.Rows)
            {
                int nCurrentRow = r.RowIndex;
                if (nCurrentRow > -1)
                {
                    string zonelist = String.Empty;
                    string valuelist = String.Empty; ;
                    string Orderlineid = _gridView.Rows[nCurrentRow].Cells[4].Text;
                    decimal totalreceived = decimal.Parse(_gridView.Rows[nCurrentRow].Cells[2].Text);
                    string price = _gridView.Rows[nCurrentRow].Cells[3].Text;
                    string strlist = ViewState["ZONES"].ToString();

                    foreach (string s in strlist.Split(','))
                    {
                        Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblzoneid_" + s);
                        TextBox txt = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtStock_" + s);
                        if (lbl != null)
                            zonelist = zonelist + lbl.Text + ",";
                        if (txt != null)
                            valuelist = valuelist + (String.IsNullOrEmpty(txt.Text.Trim()) ? "0" : txt.Text) + ",";
                    }
                    TextBox txttotal = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTotal");
                    decimal totalmoved = decimal.Parse((String.IsNullOrEmpty(txttotal.Text) ? "0" : txttotal.Text));

                    if (totalreceived != totalmoved)
                    {
                        ucError.ErrorMessage = "Moved Total is mismatched with Received on Item:" + _gridView.Rows[nCurrentRow].Cells[0].Text;
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixCrewWorkingGearStockMove.MoveStocktoZones(new Guid(Orderlineid), zonelist, valuelist, General.GetNullableDecimal(price));

                }
            }
            ucStatus.Text = "Stock moved successfully successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void ShowExcel(GridView _gridView)
    {
        try
        {
            int totcell = _gridView.HeaderRow.Cells.Count;
            string[] alCaptions = new string[totcell];

            for (int i = 0; i < totcell; i++)
            {
                alCaptions[i] = _gridView.HeaderRow.Cells[i].Text;
            }
            Response.AddHeader("Content-Disposition", "attachment; filename=WorkingGearSetItems.xls");
            Response.ContentType = "application/vnd.msexcel";

            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td nowrap>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");

            foreach (GridViewRow r in gvWorkGearPendingItem.Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < r.Cells.Count; i++)
                {
                    Response.Write("<td nowrap>");
                    if (!String.IsNullOrEmpty(r.Cells[i].Text))
                    {
                        Response.Write(r.Cells[i].Text);
                        Response.Write("</td>");
                    }
                    else
                    {

                        string strlist = ViewState["ZONES"].ToString();

                        foreach (string s in strlist.Split(','))
                        {
                            Response.Write("<td nowrap>");
                            TextBox txt = (TextBox)r.FindControl("txtStock_" + s);
                            if (txt != null)
                            {
                                Response.Write(txt.Text.Trim());
                            }
                            Response.Write("</td>");
                        }
                        TextBox txttotal = (TextBox)r.FindControl("txtTotal");
                        if (txttotal != null)
                        {
                            Response.Write("<td nowrap>");
                            Response.Write(txttotal.Text.Trim());
                            Response.Write("</td>");
                        }
                    }

                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

}
