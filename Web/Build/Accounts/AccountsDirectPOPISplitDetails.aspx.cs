using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;


public partial class AccountsDirectPOPISplitDetails : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{

    //    Table gridTable = (Table)gvOrderLine.Controls[0];
    //    ViewState["lblhead"] = "";
    //    foreach (GridViewRow gv in gvOrderLine.Rows)
    //    {
    //        Label lblhead = (Label)gv.FindControl("lblhead");
    //        if (lblhead != null)
    //        {
    //            if (ViewState["lblhead"].ToString().Trim().Equals("") || !ViewState["lblhead"].ToString().Trim().Equals(lblhead.Text.Trim()))
    //            {
    //                ViewState["lblhead"] = lblhead.Text.Trim();
    //                int rowIndex = gridTable.Rows.GetRowIndex(gv);
    //                // Add new group header row  

    //                GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

    //                TableCell headerCell = new TableCell();

    //                headerCell.ColumnSpan = gvOrderLine.Columns.Count;

    //                headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", lblhead != null ? lblhead.Text.Trim() : "") + "</b></font>";

    //                headerCell.CssClass = "GroupHeaderRowStyle";

    //                // Add header Cell to header Row, and header Row to gridTable  

    //                headerRow.Cells.Add(headerCell);
    //                headerRow.HorizontalAlign = HorizontalAlign.Left;
    //                gridTable.Controls.AddAt(rowIndex, headerRow);
    //            }
    //        }
    //    }

    //    base.Render(writer);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            MenuDirectPO.Title = "Medical P&I";
            MenuDirectPO.AccessRights = this.ViewState;
            MenuDirectPO.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                BindEdit();

            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOrderLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    private void BindEdit()
    {

        try
        {
            DataSet ds = PhoenixAccountsPNIIntergeration.Pnimedicaldpolineitemedit(new Guid(Request.QueryString["directpoid"].ToString()), new Guid(Request.QueryString["medicalid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtCurrency.Text = ds.Tables[0].Rows[0]["FLDCURRENCYNAME"].ToString();
                txtEmployee.Text = ds.Tables[0].Rows[0]["FLDINJURENAME"].ToString();
                txtMedical.Text = ds.Tables[0].Rows[0]["FLDMEDICALREFNO"].ToString();
                txtPono.Text = ds.Tables[0].Rows[0]["FLDFORMNO"].ToString();
                //txtType.Text = ds.Tables[0].Rows[0]["FLDHARDNAME"].ToString();
                ViewState["CURRENCY"] = ds.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
                ViewState["CURRENCYCODE"] = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
                ViewState["ORDERID"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void gvOrderLine_PreRender(object sender, EventArgs e)
    //{
    //   GridDecorator.MergeRows(gvOrderLine);

    //}

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string currentCategoryName = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblCategoryName")).Text;
                string previousCategoryName = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblCategoryName")).Text;

                if (currentCategoryName == previousCategoryName)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                        previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }

            }

        }
    }
    private void BindData()
    {

        try
        {
            DataSet ds = PhoenixAccountsPNIIntergeration.PniSplitLineitemlist(int.Parse(Request.QueryString["type"].ToString()), new Guid(Request.QueryString["directpoid"].ToString()), new Guid(Request.QueryString["medicalid"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOrderLine.DataSource = ds;
                // gvOrderLine.DataBind();
                // ViewState["gvdetails"] = ds.Tables[0];
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void RegistersStockItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOrderLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)e.Item;
            int iRowno = e.Item.ItemIndex;
            if (e.CommandName.ToUpper().Equals("ADD"))
            {


            }

            if (e.CommandName == "INFO1")
            {

            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {


                string dtkey = (((RadLabel)e.Item.FindControl("lbldtkey")).Text);
                ViewState["ORDERID"] = Request.QueryString["directpoid"].ToString();

                PhoenixAccountsPNIIntergeration.PnidpoLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(dtkey), new Guid(ViewState["ORDERID"].ToString()));
                PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
                                                                    new Guid(((RadLabel)e.Item.FindControl("lblsubtype")).Text),
                                                                    new Guid(Request.QueryString["medicalid"].ToString()),
                                                                    0,
                                                                    0,
                                                                    0,
                                                                    new Guid(ViewState["ORDERID"].ToString())
                                                                    );
                qtyTotal = 0;
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {


                string Amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;
                string subtype = (((RadLabel)e.Item.FindControl("lblsubtype")).Text);
                string dtkey = (((RadLabel)e.Item.FindControl("lbldtkey")).Text);
                string updatedtkey = (((RadLabel)e.Item.FindControl("lblupdatedtkay")).Text);
                if (!IsValidMedical(Amount))
                {
                    ucError.Visible = true;
                    return;
                }
                if (dtkey == null || dtkey == string.Empty)
                {
                    PhoenixAccountsPNIIntergeration.PnidpoLineItemInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["directpoid"].ToString()), decimal.Parse(Amount), new Guid(subtype), new Guid(Request.QueryString["medicalid"].ToString()), General.GetNullableInteger(ViewState["CURRENCY"].ToString()));
                    PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
                                                                      new Guid(((RadLabel)e.Item.FindControl("lblsubtype")).Text),
                                                                      new Guid(Request.QueryString["medicalid"].ToString()),
                                                                      0,
                                                                      decimal.Parse(Amount),
                                                                      0,
                                                                      new Guid(updatedtkey)
                                                                      );
                }
                else
                {
                    PhoenixAccountsPNIIntergeration.PnidpoLineItemUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["directpoid"].ToString()), decimal.Parse(Amount), new Guid(subtype), new Guid(dtkey), General.GetNullableInteger(ViewState["CURRENCY"].ToString()));
                    PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
                                                                 new Guid(subtype),
                                                                 new Guid(Request.QueryString["medicalid"].ToString()),
                                                                 0,
                                                                 decimal.Parse(Amount),
                                                                 0,
                                                                 new Guid(updatedtkey)
                                                                 );
                }

                qtyTotal = 0;
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    decimal qtyTotal = 0;
    //protected void gvOrderLine_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = int.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {


    //        }

    //        if (e.CommandName == "INFO1")
    //        {

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvOrderLine_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = de.RowIndex;
    //        string dtkey = (((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text);
    //        ViewState["ORDERID"] = Request.QueryString["directpoid"].ToString();
    //        _gridView.EditIndex = -1;
    //        PhoenixAccountsPNIIntergeration.PnidpoLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(dtkey), new Guid(ViewState["ORDERID"].ToString()));
    //        PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
    //                                                            new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblsubtype")).Text),
    //                                                            new Guid(Request.QueryString["medicalid"].ToString()),
    //                                                            0,
    //                                                            0,
    //                                                            0,
    //                                                            new Guid(ViewState["ORDERID"].ToString())
    //                                                            );
    //        qtyTotal = 0;
    //        BindData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvOrderLine_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    qtyTotal = 0;
    //    BindData();
    //}
    //protected void gvOrderLine_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = de.NewEditIndex;
    //    qtyTotal = 0;
    //    BindData();
    //}
    //protected void gvOrderLine_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        string Amount = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text;
    //        string subtype = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblsubtype")).Text);
    //        string dtkey = (((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text);
    //        string updatedtkey = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblupdatedtkay")).Text);
    //        if (!IsValidMedical(Amount))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        if (dtkey == null || dtkey == string.Empty)
    //        {
    //            PhoenixAccountsPNIIntergeration.PnidpoLineItemInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["directpoid"].ToString()), decimal.Parse(Amount), new Guid(subtype), new Guid(Request.QueryString["medicalid"].ToString()),General.GetNullableInteger(ViewState["CURRENCY"].ToString()));
    //            PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
    //                                                              new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblsubtype")).Text),
    //                                                              new Guid(Request.QueryString["medicalid"].ToString()),
    //                                                              0,
    //                                                              decimal.Parse(Amount),
    //                                                              0,
    //                                                              new Guid(updatedtkey)
    //                                                              );
    //        }
    //        else
    //        {
    //            PhoenixAccountsPNIIntergeration.PnidpoLineItemUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["directpoid"].ToString()), decimal.Parse(Amount), new Guid(subtype), new Guid(dtkey), General.GetNullableInteger(ViewState["CURRENCY"].ToString()));
    //            PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
    //                                                         new Guid(subtype),
    //                                                         new Guid(Request.QueryString["medicalid"].ToString()),
    //                                                         0,
    //                                                         decimal.Parse(Amount),
    //                                                         0,
    //                                                         new Guid(updatedtkey)
    //                                                         );
    //        }
    //        _gridView.EditIndex = -1;
    //        qtyTotal = 0;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}  decimal qtyTotal = 0;
    private bool IsValidMedical(string Amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (Amount == string.Empty)
            ucError.ErrorMessage = "Amount is required.";
        return (!ucError.IsError);
    }
    protected void gvOrderLine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            RadLabel lblHeader = (RadLabel)e.Item.FindControl("lblHeader");
            lblHeader.Text = "Amount (" + ViewState["CURRENCYCODE"].ToString() + ")";
        }
        if (e.Item is GridDataItem)
        {
            ImageButton imgnotes = (ImageButton)e.Item.FindControl("imgnotes");
            UserControlToolTip ucToolTipNW = (UserControlToolTip)e.Item.FindControl("ucToolTipNW");
            RadLabel lblCategoryName = (RadLabel)e.Item.FindControl("lblCategoryName");
            RadLabel lblsigner = (RadLabel)e.Item.FindControl("lblsigner");
            if (imgnotes != null && ucToolTipNW != null)
            {
                string[] alColumns = { "FLDPARTNAME", "FLDTOTALAMOUNT" };
                string[] alCaptions = { "Item Name", "Amount" };
                DataTable dt1 = PhoenixAccountsPNIIntergeration.OrderLineItemforPNIinfo(new Guid(Request.QueryString["medicalid"].ToString())
                                                                                         , new Guid(Request.QueryString["directpoid"].ToString())
                                                                                         , lblCategoryName.Text.ToUpper().Trim()
                                                                                         , General.GetNullableInteger(lblsigner.Text));
                if (dt1.Rows.Count > 0)
                {
                    string html = "<table>";
                    //add header row
                    html += "<tr>";
                    for (int i = 0; i < 2; i++)
                        html += "<td>" + dt1.Columns[i].ColumnName + "</td>";
                    html += "</tr>";
                    //add rows
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        html += "<tr>";
                        for (int j = 0; j < 2; j++)
                            html += "<td>" + dt1.Rows[i][j].ToString() + "</td>";
                        html += "</tr>";
                    }
                    html += "</table>";
                    ucToolTipNW.Text = html;//dt1.Rows[0][0].ToString();//General.ShowGrid(dt1, alColumns, alCaptions);
                }
                else
                    ucToolTipNW.Text = "no records";//General.ShowGrid(dt1, alColumns, alCaptions);
                //imgnotes.ToolTip = dt1.ToString();// General.ShowGrid(dt1, alColumns, alCaptions);
                imgnotes.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
                imgnotes.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");
            }

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdDelete");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                if (DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString() == null || DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString() == string.Empty)
                    eb.Visible = false;
            }
            decimal tmpTotal = decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDAMOUNT").ToString() == "" ? "0" : DataBinder.Eval(e.Item.DataItem, "FLDAMOUNT").ToString());
            qtyTotal += tmpTotal;

            RadGrid gvOrderlineitem = (RadGrid)e.Item.FindControl("gvOrderlineitem");

            //Label lblCategoryName = (Label)e.Row.FindControl("lblCategoryName");



        }
        if (e.Item is GridFooterItem)
        {
            e.Item.Cells[4].Text = qtyTotal.ToString();
        }

    }
    //protected void gvOrderLine_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if(e.Row.RowType == DataControlRowType.Header)
    //    {
    //        Label lblHeader = (Label)e.Row.FindControl("lblHeader");
    //        lblHeader.Text = "Amount (" + ViewState["CURRENCYCODE"].ToString() + ")";
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit )
    //    {
    //        ImageButton imgnotes = (ImageButton)e.Row.FindControl("imgnotes");
    //        UserControlToolTip ucToolTipNW = (UserControlToolTip)e.Row.FindControl("ucToolTipNW");
    //        Label lblCategoryName = (Label)e.Row.FindControl("lblCategoryName");
    //        Label lblsigner = (Label)e.Row.FindControl("lblsigner"); 
    //        if (imgnotes != null && ucToolTipNW != null)
    //        {
    //            string[] alColumns = { "FLDPARTNAME", "FLDTOTALAMOUNT" };
    //            string[] alCaptions = { "Item Name", "Amount" };
    //            DataTable dt1 = PhoenixAccountsPNIIntergeration.OrderLineItemforPNIinfo(new Guid(Request.QueryString["medicalid"].ToString())
    //                                                                                     , new Guid(Request.QueryString["directpoid"].ToString())
    //                                                                                     , lblCategoryName.Text.ToUpper().Trim()
    //                                                                                     , General.GetNullableInteger(lblsigner.Text));
    //            if (dt1.Rows.Count > 0)
    //            {
    //                string html = "<table>";
    //                //add header row
    //                html += "<tr>";
    //                for (int i = 0; i < 2; i++)
    //                    html += "<td>" + dt1.Columns[i].ColumnName + "</td>";
    //                html += "</tr>";
    //                //add rows
    //                for (int i = 0; i < dt1.Rows.Count; i++)
    //                {
    //                    html += "<tr>";
    //                    for (int j = 0; j < 2; j++)
    //                        html += "<td>" + dt1.Rows[i][j].ToString() + "</td>";
    //                    html += "</tr>";
    //                }
    //                html += "</table>";
    //                ucToolTipNW.Text = html;//dt1.Rows[0][0].ToString();//General.ShowGrid(dt1, alColumns, alCaptions);
    //            }
    //            else
    //                ucToolTipNW.Text = "no records";//General.ShowGrid(dt1, alColumns, alCaptions);
    //            //imgnotes.ToolTip = dt1.ToString();// General.ShowGrid(dt1, alColumns, alCaptions);
    //            imgnotes.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
    //            imgnotes.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");
    //        }

    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (eb != null)
    //        {
    //            eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
    //            if (DataBinder.Eval(e.Row.DataItem, "FLDDTKEY").ToString() == null || DataBinder.Eval(e.Row.DataItem, "FLDDTKEY").ToString() == string.Empty)
    //                eb.Visible = false;
    //        }
    //        decimal tmpTotal = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "FLDAMOUNT").ToString() == "" ? "0" : DataBinder.Eval(e.Row.DataItem, "FLDAMOUNT").ToString());
    //        qtyTotal += tmpTotal;

    //        GridView gvOrderlineitem = (GridView)e.Row.FindControl("gvOrderlineitem");

    //        //Label lblCategoryName = (Label)e.Row.FindControl("lblCategoryName");



    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        e.Row.Cells[2].Text = qtyTotal.ToString();
    //    }
    //}

}

