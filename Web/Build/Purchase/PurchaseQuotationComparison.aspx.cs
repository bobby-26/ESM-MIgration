using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Purchase_PurchaseQuotationComparison : PhoenixBasePage
{
    string minvalueUniqueName;
    string minvalueItemTotal;
    string minvalueDiscountTotal;
	protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            MenuQuotationCompare.Title = "Quotations Compare ( " + Filter.CurrentPurchaseFormNumberSelection + " )";
            if (Request.QueryString["VIEWONLY"] == null)
            {
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                //toolbargrid.AddFontAwesomeButton("javascript:parent.Openpopup('codehelp2','Quotation','../Purchase/PurchaseQuotationComparison.aspx?orderid=" + Request.QueryString["orderid"] + "&vendors=" + ViewState["selectvendors"].ToString() + "&VIEWONLY=1');return false;", "View", "<i class=\"fas fa-file-excel\"></i>", "VIEW");
                //toolbargrid.AddFontAwesomeButton("javascript:parent.Openpopup('codehelp1','','PurchaseQuotationComparisonBulkSave.aspx?orderid=" + ViewState["orderid"].ToString() + "&vendors=" + ViewState["selectvendors"].ToString() + "')", "Save", "<i class=\"fas fa-database\"></i>", "BULKSAVE");
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationComparison.aspx?orderid=" + Request.QueryString["orderid"] + "&vendors=" + Request.QueryString["vendors"].ToString(), "Split", "<i class=\"fas fa-sitemap\"></i>", "SPLIT");
                toolbargrid.AddButton("Back", "BACK", ToolBarDirection.Right);
                MenuQuotationCompare.MenuList = toolbargrid.Show();
            }
            if (!IsPostBack)
            {
                gvCompare.ExportSettings.FileName = "Quotations Compare ( " + Filter.CurrentPurchaseFormNumberSelection + " )";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    ViewState["selectvendors"] = Request.QueryString["vendors"].ToString();
                }
                else
                {
                    ViewState["orderid"] = "0";
                    ViewState["selectvendors"] = "";
                }
                
                
                BindVendor();
                //CreateGridHeaders();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void CreateGridHeaders()
    //{
    //    DataSet ds;
    //    gvCompare.MasterTableView.ColumnGroups.Clear();
    //    gvCompare.MasterTableView.Columns.Clear();
    //    ds = PhoenixPurchaseQuotation.QuotationsCompare(General.GetNullableGuid(ViewState["orderid"].ToString()), ViewState["selectvendors"].ToString(), int.Parse(Filter.CurrentPurchaseVesselSelection.ToString()));
    //    foreach (DataRow dr in ds.Tables[1].Rows)
    //    {
    //        GridColumnGroup parentHeader = new GridColumnGroup();
    //        gvCompare.MasterTableView.ColumnGroups.Add(parentHeader);
    //        parentHeader.HeaderText = dr["FLDNAME"].ToString();
    //        parentHeader.Name = dr["FLDNAME"].ToString();
    //        parentHeader.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
    //    }

    //    GridClientSelectColumn chk = new GridClientSelectColumn
    //    {
    //        UniqueName = "CHECKBOX",
    //    };
    //    gvCompare.MasterTableView.Columns.Add(chk);
    //    chk.HeaderStyle.Width = Unit.Parse("40px");
    //    chk.ItemStyle.Width = Unit.Parse("40px");

    //    GridBoundColumn serialno = new GridBoundColumn();
    //    gvCompare.MasterTableView.Columns.Add(serialno);
    //    serialno.HeaderText = "S.No";
    //    serialno.UniqueName = "SERIALNO";
    //    serialno.ReadOnly = true;
    //    serialno.DataField = "FLDSERIALNO";
    //    serialno.DataType = typeof(System.Int32);

    //    GridBoundColumn number = new GridBoundColumn();
    //    gvCompare.MasterTableView.Columns.Add(number);
    //    number.HeaderText = "Number";
    //    number.UniqueName = "PARTNUMBER";
    //    number.ReadOnly = true;
    //    number.DataField = "FLDPARTNUMBER";
    //    number.DataType = typeof(System.String);

    //    GridBoundColumn name = new GridBoundColumn();
    //    gvCompare.MasterTableView.Columns.Add(name);
    //    name.HeaderText = "Name";
    //    name.UniqueName = "NAME";
    //    name.ReadOnly = true;
    //    name.DataField = "FLDNAME";
    //    name.DataType = typeof(System.String);


    //    GridBoundColumn unit = new GridBoundColumn();
    //    gvCompare.MasterTableView.Columns.Add(unit);
    //    unit.HeaderText = "Unit";
    //    unit.UniqueName = "UNITNAME";
    //    unit.ReadOnly = true;
    //    unit.DataField = "FLDUNITNAME";
    //    unit.DataType = typeof(System.String);

    //    GridNumericColumn reqQty = new GridNumericColumn();
    //    gvCompare.MasterTableView.Columns.Add(reqQty);
    //    reqQty.HeaderText = "Req Qty";
    //    reqQty.UniqueName = "REQUESTEDQUANTITY";
    //    reqQty.ReadOnly = true;
    //    reqQty.DataField = "FLDREQUESTEDQUANTITY";
    //    reqQty.DataType = typeof(System.Decimal);
    //    reqQty.DataFormatString = "{0:n0}";
    //    reqQty.DecimalDigits = 2;

    //    GridNumericColumn ordQty = new GridNumericColumn();
    //    gvCompare.MasterTableView.Columns.Add(ordQty);
    //    ordQty.HeaderText = "App Qty";
    //    ordQty.UniqueName = "ORDEREDQUANTITY";
    //    //ordQty.ReadOnly = true;
    //    ordQty.DataField = "FLDORDEREDQUANTITY";
    //    ordQty.DataType = typeof(System.Decimal);
    //    ordQty.DataFormatString = "{0:n0}";
    //    ordQty.DecimalDigits = 2;
        

    //    serialno.HeaderStyle.Width = Unit.Parse("40px");
    //    serialno.ItemStyle.Width = Unit.Parse("40px");

    //    number.HeaderStyle.Width = Unit.Parse("100px");
    //    number.ItemStyle.Width = Unit.Parse("100px");

    //    name.HeaderStyle.Width = Unit.Parse("150px");
    //    name.ItemStyle.Width = Unit.Parse("150px");

    //    unit.HeaderStyle.Width = Unit.Parse("60px");
    //    unit.ItemStyle.Width = Unit.Parse("60px");

    //    reqQty.HeaderStyle.Width = Unit.Parse("75px");
    //    reqQty.ItemStyle.Width = Unit.Parse("75px");

    //    ordQty.HeaderStyle.Width = Unit.Parse("75px");
    //    ordQty.ItemStyle.Width = Unit.Parse("75px");


    //    reqQty.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
    //    ordQty.ItemStyle.HorizontalAlign = HorizontalAlign.Right;



    //    foreach (DataRow dr in ds.Tables[1].Rows)
    //    {
    //        //GridColumnGroup parentHeader = new GridColumnGroup();
    //        //gvCompare.MasterTableView.ColumnGroups.Add(parentHeader);
    //        //parentHeader.HeaderText = dr["FLDNAME"].ToString();
    //        //parentHeader.Name = dr["FLDNAME"].ToString();
    //        //parentHeader.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;


    //        GridBoundColumn c = new GridBoundColumn();
    //        gvCompare.MasterTableView.Columns.Add(c);
    //        c.HeaderText = "Unit Price";
    //        c.UniqueName = dr["FLDQUOTATIONID"].ToString() + "_" + "FLDUSDPRICE";
    //        c.ColumnGroupName = dr["FLDNAME"].ToString();
    //        c.ReadOnly = true;
    //        c.DataField = "FLDUSDPRICE";


    //        GridBoundColumn c1 = new GridBoundColumn();
    //        gvCompare.MasterTableView.Columns.Add(c1);
    //        c1.HeaderText = "Item Price";
    //        c1.UniqueName = dr["FLDQUOTATIONID"].ToString() + "_" + "FLDUSDTOTAL";
    //        c1.ColumnGroupName = dr["FLDNAME"].ToString();
    //        c1.FooterText = dr["FLDTOTALAMOUNT"].ToString();
    //        c1.ReadOnly = true;
    //        c1.DataField = "FLDUSDTOTAL";

    //        GridBoundColumn c2 = new GridBoundColumn();
    //        gvCompare.MasterTableView.Columns.Add(c2);
    //        c2.HeaderText = "Disc [" + dr["FLDDISCOUNT"].ToString() + " %]";
    //        c2.UniqueName = dr["FLDQUOTATIONID"].ToString() + "_" + "FLDDISCOUNTTOTAL";
    //        c2.ColumnGroupName = dr["FLDNAME"].ToString();
    //        c2.FooterText = dr["FLDTOTALUSDAMOUNT"].ToString();
    //        c2.ReadOnly = true;
    //        c2.DataField = "FLDDISCOUNTTOTAL";


    //        GridBoundColumn c3 = new GridBoundColumn();
    //        gvCompare.MasterTableView.Columns.Add(c3);
    //        c3.HeaderText = "Del. Time (Days)";
    //        c3.UniqueName = dr["FLDQUOTATIONID"].ToString() + "_" + "FLDDELIVERYTIME";
    //        c3.ColumnGroupName = dr["FLDNAME"].ToString();
    //        c3.ReadOnly = true;
    //        c3.DataField = "FLDDELIVERYTIME";


    //        c.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
    //        c1.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
    //        c1.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
    //        c2.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
    //        c2.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
    //        c3.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

    //        c.HeaderStyle.Width = Unit.Parse("60px");
    //        c.ItemStyle.Width = Unit.Parse("60px");

    //        c1.HeaderStyle.Width = Unit.Parse("60px");
    //        c1.ItemStyle.Width = Unit.Parse("60px");

    //        c2.HeaderStyle.Width = Unit.Parse("80px");
    //        c2.ItemStyle.Width = Unit.Parse("80px");

    //        c3.HeaderStyle.Width = Unit.Parse("80px");
    //        c3.ItemStyle.Width = Unit.Parse("80px");






    //    }
    //}
    private void BindVendor()
    {
        ddlVendor.DataSource = PhoenixPurchaseQuotation.QuotationVendorList(new Guid(ViewState["orderid"].ToString()), ViewState["selectvendors"].ToString());
        ddlVendor.DataBind();
    }

  //  private void BindData()
  //  {
  //      int iRowCount = 0;
  //      int iTotalPageCount = 0;

  //      string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
  //      int? sortdirection = null;
  //      if (ViewState["SORTDIRECTION"] != null)
  //          sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

  //      ds = PhoenixPurchaseQuotation.QuotationComparison(General.GetNullableGuid(ViewState["orderid"].ToString()), ViewState["selectvendors"].ToString(), int.Parse(Filter.CurrentPurchaseVesselSelection.ToString()));
  //      arrayUser.Clear();
  //      arrayFooter.Clear();
  //      arrayDiscount.Clear();
  //      foreach (DataRow dr in ds.Tables[0].Rows)
  //      {
  //          arrayFooter.Add(dr["FLDTOTALAMOUNT"].ToString());
  //          arrayFooter.Add(dr["FLDTOTALUSDAMOUNT"].ToString());
  //          arrayDiscount.Add(dr["FLDDISCOUNT"].ToString());
  //          arrayvendorstatus.Add(dr["FLDQUOTATIONSTATUS"].ToString());
  //      }
  //      if (ds.Tables[1].Rows.Count > 0)
  //      {
		//	gvVendor.DataSource = ds.Tables[1];
  //          gvVendor.DataBind();
		//}
  //      else
  //      {
  //          DataTable dt = ds.Tables[0];
  //          ShowNoRecordsFound(dt, gvVendor);
  //      }
  //      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
  //      {
  //          if (gvVendor.Columns.Count == 10)
  //          {
  //              AddColumnsInGrid(ds);
  //          }
  //          DataTable dt = ds.Tables[0];
  //          GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
  //          row.Attributes.Add("style", "position:static");
  //          TableCell cell = new TableCell();
  //          cell.ColumnSpan = 8;
  //          row.Cells.Add(cell);
  //          for (int i = 0; i < dt.Rows.Count; i++)
  //          {
  //              cell = new TableCell();
  //              cell.HorizontalAlign = HorizontalAlign.Center;
  //              cell.Text = dt.Rows[i]["FLDNAME"].ToString();
  //              cell.ColumnSpan = 4;
  //              row.Cells.Add(cell);

  //              gvVendor.Controls[0].Controls.AddAt(0, row);
  //          }
  //          cell = new TableCell();
  //          cell.HorizontalAlign = HorizontalAlign.Center;
  //          cell.Text = "";
  //          row.Cells.Add(cell);

  //          gvVendor.Controls[0].Controls.AddAt(0, row);
  //      }
  //      AddFooter();
		//CheckLowestPrice();
  //      ViewState["ROWCOUNT"] = iRowCount;
  //      ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
  //  }



    protected void MenuQuotationCompare_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SPLIT"))
            {
                if (ViewState["orderid"] != null)
                {
                    string selectedlineitems = ",";
                    foreach(GridDataItem item in gvCompare.MasterTableView.Items)
                    {
                        if(((CheckBox)item["CHECKBOX"].Controls[0]).Checked)
                        {
                            selectedlineitems = selectedlineitems + item.GetDataKeyValue("FLDORDERLINEID").ToString() + ",";
                        }
                    }

                    if (General.GetNullableGuid(ddlVendor.SelectedValue) == null)
                        ucError.ErrorMessage = "Vendor is required";
                    
                    if (selectedlineitems.Length <= 1)
                        ucError.ErrorMessage = "Line items required.";

                    if (selectedlineitems.Length > 1 && !ucError.IsError)
                    {
                        Response.Redirect("../Purchase/PurchaseFormSplit.aspx?orderid=" + ViewState["orderid"].ToString() + "&orderline=" + selectedlineitems + "&quotationid=" + ddlVendor.SelectedValue);
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
            }else if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?orderid=" + ViewState["orderid"].ToString());
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
        
    }

    //protected void gvVendor_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = int.Parse(e.CommandArgument.ToString());
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SAVE"))
    //        {
    //            UserControlMaskNumber txtOrderQtyEdit = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtOrderQtyEdit"));
    //            Label lblOrderLineIdEdit = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderLineIdEdit"));

    //            if (txtOrderQtyEdit != null)
    //            {
    //                PhoenixPurchaseOrderLine.UpdateOrderLineFromCompareScreen(
    //                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                    new Guid(lblOrderLineIdEdit.Text),
    //                    General.GetNullableGuid(ViewState["orderid"].ToString()),
    //                    Filter.CurrentPurchaseVesselSelection,
    //                    General.GetNullableDecimal(txtOrderQtyEdit.Text));
    //            }
    //            _gridView.EditIndex = -1;
                
    //        }
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    

    protected void gvCompare_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds;
            gvCompare.MasterTableView.ColumnGroups.Clear();
            gvCompare.MasterTableView.Columns.Clear();
            ds = PhoenixPurchaseQuotation.QuotationsCompare(General.GetNullableGuid(ViewState["orderid"].ToString()), ViewState["selectvendors"].ToString(), int.Parse(Filter.CurrentPurchaseVesselSelection.ToString()));
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                GridColumnGroup parentHeader = new GridColumnGroup();
                gvCompare.MasterTableView.ColumnGroups.Add(parentHeader);
                parentHeader.HeaderText = dr["FLDNAME"].ToString();
                parentHeader.Name = dr["FLDQUOTATIONID"].ToString();
                parentHeader.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            }

            GridClientSelectColumn chk = new GridClientSelectColumn
            {
                UniqueName = "CHECKBOX",
            };
            gvCompare.MasterTableView.Columns.Add(chk);
            chk.HeaderStyle.Width = Unit.Parse("40px");
            chk.ItemStyle.Width = Unit.Parse("40px");
            chk.Exportable = false;

            GridBoundColumn serialno = new GridBoundColumn();
            gvCompare.MasterTableView.Columns.Add(serialno);
            serialno.HeaderText = "S.No";
            serialno.UniqueName = "SERIALNO";
            serialno.ReadOnly = true;
            serialno.DataField = "FLDSERIALNO";
            serialno.DataType = typeof(System.Int32);

            GridBoundColumn number = new GridBoundColumn();
            gvCompare.MasterTableView.Columns.Add(number);
            number.HeaderText = "Number";
            number.UniqueName = "PARTNUMBER";
            number.ReadOnly = true;
            number.DataField = "FLDPARTNUMBER";
            number.DataType = typeof(System.String);

            GridBoundColumn name = new GridBoundColumn();
            gvCompare.MasterTableView.Columns.Add(name);
            name.HeaderText = "Name";
            name.UniqueName = "NAME";
            name.ReadOnly = true;
            name.DataField = "FLDNAME";
            name.DataType = typeof(System.String);


            GridBoundColumn unit = new GridBoundColumn();
            gvCompare.MasterTableView.Columns.Add(unit);
            unit.HeaderText = "Unit";
            unit.UniqueName = "UNITNAME";
            unit.ReadOnly = true;
            unit.DataField = "FLDUNITNAME";
            unit.DataType = typeof(System.String);

            GridNumericColumn reqQty = new GridNumericColumn();
            gvCompare.MasterTableView.Columns.Add(reqQty);
            reqQty.HeaderText = "Req Qty";
            reqQty.UniqueName = "REQUESTEDQUANTITY";
            reqQty.ReadOnly = true;
            reqQty.DataField = "FLDREQUESTEDQUANTITY";
            reqQty.DataType = typeof(System.Decimal);
            reqQty.DataFormatString = "{0:n0}";
            reqQty.DecimalDigits = 2;

            GridNumericColumn ordQty = new GridNumericColumn();
            gvCompare.MasterTableView.Columns.Add(ordQty);
            ordQty.HeaderText = "App Qty";
            ordQty.UniqueName = "ORDEREDQUANTITY";
            //ordQty.ReadOnly = true;
            ordQty.DataField = "FLDORDEREDQUANTITY";
            ordQty.DataType = typeof(System.Decimal);
            ordQty.DataFormatString = "{0:n0}";
            ordQty.DecimalDigits = 2;

            if (!Filter.CurrentPurchaseStockType.ToString().ToUpper().Equals("STORE"))
            {
                GridBoundColumn component = new GridBoundColumn();
                gvCompare.MasterTableView.Columns.Add(component);
                component.HeaderText = "Component";
                component.UniqueName = "COMPONENT";
                component.ReadOnly = true;
                component.DataField = "FLDCOMPONENT";
                component.DataType = typeof(System.String);
                component.HeaderStyle.Width = Unit.Parse("100px");
                component.ItemStyle.Width = Unit.Parse("100px");
            }
            


            serialno.HeaderStyle.Width = Unit.Parse("40px");
            serialno.ItemStyle.Width = Unit.Parse("40px");

            number.HeaderStyle.Width = Unit.Parse("100px");
            number.ItemStyle.Width = Unit.Parse("100px");

            name.HeaderStyle.Width = Unit.Parse("150px");
            name.ItemStyle.Width = Unit.Parse("150px");

            unit.HeaderStyle.Width = Unit.Parse("60px");
            unit.ItemStyle.Width = Unit.Parse("60px");

            reqQty.HeaderStyle.Width = Unit.Parse("75px");
            reqQty.ItemStyle.Width = Unit.Parse("75px");

            ordQty.HeaderStyle.Width = Unit.Parse("75px");
            ordQty.ItemStyle.Width = Unit.Parse("75px");


            reqQty.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            ordQty.ItemStyle.HorizontalAlign = HorizontalAlign.Right;



            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                //GridColumnGroup parentHeader = new GridColumnGroup();
                //gvCompare.MasterTableView.ColumnGroups.Add(parentHeader);
                //parentHeader.HeaderText = dr["FLDNAME"].ToString();
                //parentHeader.Name = dr["FLDNAME"].ToString();
                //parentHeader.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;


                GridBoundColumn c = new GridBoundColumn();
                gvCompare.MasterTableView.Columns.Add(c);
                c.HeaderText = "Unit Price";
                c.UniqueName = dr["FLDQUOTATIONID"].ToString() + "_" + "FLDUSDPRICE";
                c.ColumnGroupName = dr["FLDQUOTATIONID"].ToString();
                c.ReadOnly = true;
                c.DataField = "FLDUSDPRICE";


                GridBoundColumn c1 = new GridBoundColumn();
                gvCompare.MasterTableView.Columns.Add(c1);
                c1.HeaderText = "Item Price";
                c1.UniqueName = dr["FLDQUOTATIONID"].ToString() + "_" + "FLDUSDTOTAL";
                c1.ColumnGroupName = dr["FLDQUOTATIONID"].ToString();
                c1.FooterText = dr["FLDTOTALAMOUNT"].ToString();
                c1.ReadOnly = true;
                c1.DataField = "FLDUSDTOTAL";

                GridBoundColumn c2 = new GridBoundColumn();
                gvCompare.MasterTableView.Columns.Add(c2);
                c2.HeaderText = "Discounted Price [" + dr["FLDDISCOUNT"].ToString() + " %]";
                c2.UniqueName = dr["FLDQUOTATIONID"].ToString() + "_" + "FLDDISCOUNTTOTAL";
                c2.ColumnGroupName = dr["FLDQUOTATIONID"].ToString();
                c2.FooterText = dr["FLDTOTALUSDAMOUNT"].ToString();
                c2.ReadOnly = true;
                c2.DataField = "FLDDISCOUNTTOTAL";


                GridBoundColumn c3 = new GridBoundColumn();
                gvCompare.MasterTableView.Columns.Add(c3);
                c3.HeaderText = "Del. Time (Days)";
                c3.UniqueName = dr["FLDQUOTATIONID"].ToString() + "_" + "FLDDELIVERYTIME";
                c3.ColumnGroupName = dr["FLDQUOTATIONID"].ToString();
                c3.ReadOnly = true;
                c3.DataField = "FLDDELIVERYTIME";

                GridBoundColumn c4 = new GridBoundColumn();
                gvCompare.MasterTableView.Columns.Add(c4);
                c4.HeaderText = "Item Type";
                c4.UniqueName = dr["FLDQUOTATIONID"].ToString() + "_" + "FLDITEMTYPE";
                c4.ColumnGroupName = dr["FLDQUOTATIONID"].ToString();
                c4.ReadOnly = true;
                c4.DataField = "FLDITEMTYPE";


                c.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                c1.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                c1.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                c2.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                c2.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                c3.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

                c.HeaderStyle.Width = Unit.Parse("60px");
                c.ItemStyle.Width = Unit.Parse("60px");

                c1.HeaderStyle.Width = Unit.Parse("60px");
                c1.ItemStyle.Width = Unit.Parse("60px");

                c2.HeaderStyle.Width = Unit.Parse("80px");
                c2.ItemStyle.Width = Unit.Parse("80px");

                c3.HeaderStyle.Width = Unit.Parse("80px");
                c3.ItemStyle.Width = Unit.Parse("80px");

                c4.HeaderStyle.Width = Unit.Parse("90px");
                c4.ItemStyle.Width = Unit.Parse("90px");

            }

            gvCompare.DataSource = ds;

            
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        


    }

    protected void gvCompare_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        minvalueUniqueName = null;
        try
        {
            if (e.Item is GridDataItem)
            {
                DataSet ds = (DataSet)grid.DataSource;
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)item.DataItem;

                foreach (GridColumn c in grid.Columns)
                {
                    string[] temp = c.UniqueName.Split('_');
                    if (temp.Length > 1 && temp[1].Contains("FLD"))
                    {
                        DataRow[] dr = ds.Tables[2].Select("FLDORDERLINEID = '" + drv["FLDORDERLINEID"].ToString() + "' AND FLDQUOTATIONID = '" + temp[0] + "'");
                        item[c.UniqueName].Text = dr[0][temp[1]].ToString();
                    }
                    if (temp.Length > 1 && temp[1].Equals("FLDUSDPRICE"))
                    {
                        if (General.GetNullableString(minvalueUniqueName) != null)
                        {
                            if (General.GetNullableDecimal(item[minvalueUniqueName].Text) != null && General.GetNullableDecimal(item[c.UniqueName].Text) != null)
                            {
                                if (decimal.Parse(item[minvalueUniqueName].Text) > decimal.Parse(item[c.UniqueName].Text))
                                    minvalueUniqueName = c.UniqueName;
                            }
                        }
                        else if (General.GetNullableDecimal(item[c.UniqueName].Text) != null)
                            minvalueUniqueName = c.UniqueName;
                    }
                }
                if (General.GetNullableString(minvalueUniqueName) != null && item[minvalueUniqueName] != null)
                    item[minvalueUniqueName].BackColor = System.Drawing.Color.Yellow;

                if (item["NAME"].Text.ToUpper().Equals("DELIVERY/TAX/OTHER CHARGES"))
                {
                    item["CHECKBOX"].Enabled = false;
                    item["ORDEREDQUANTITY"].Enabled = false;
                }
                    

                
            }
            else if (e.Item is GridFooterItem)
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                foreach (GridColumn c in grid.Columns)
                {
                    string[] temp = c.UniqueName.Split('_');
                    if (temp.Length > 1 && temp[1].Equals("FLDUSDTOTAL"))
                    {
                        if (General.GetNullableString(minvalueItemTotal) != null)
                        {
                            if (General.GetNullableDecimal(item[minvalueItemTotal].Text) != null && General.GetNullableDecimal(item[c.UniqueName].Text) != null)
                            {
                                if (decimal.Parse(item[minvalueItemTotal].Text) > decimal.Parse(item[c.UniqueName].Text))
                                    minvalueItemTotal = c.UniqueName;
                            }
                        }
                        else if (General.GetNullableDecimal(item[c.UniqueName].Text) != null)
                            minvalueItemTotal = c.UniqueName;
                    }

                    if (temp.Length > 1 && temp[1].Equals("FLDDISCOUNTTOTAL"))
                    {
                        if (General.GetNullableString(minvalueDiscountTotal) != null)
                        {
                            if (General.GetNullableDecimal(item[minvalueDiscountTotal].Text) != null && General.GetNullableDecimal(item[c.UniqueName].Text) != null)
                            {
                                if (decimal.Parse(item[minvalueDiscountTotal].Text) > decimal.Parse(item[c.UniqueName].Text))
                                    minvalueDiscountTotal = c.UniqueName;
                            }
                        }
                        else if (General.GetNullableDecimal(item[c.UniqueName].Text) != null)
                            minvalueDiscountTotal = c.UniqueName;
                    }

                }
                if (General.GetNullableString(minvalueDiscountTotal) != null && item[minvalueDiscountTotal] != null)
                    item[minvalueDiscountTotal].BackColor = System.Drawing.Color.GreenYellow;

                if (General.GetNullableString(minvalueItemTotal) != null && item[minvalueItemTotal] != null)
                    item[minvalueItemTotal].BackColor = System.Drawing.Color.GreenYellow;
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
        
    }

    protected void gvCompare_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    //protected void gvCompare_EditCommand(object sender, GridCommandEventArgs e)
    //{
    //    if(e.Item is GridEditableItem)
    //    {
    //        GridEditableItem item = (GridEditableItem)e.Item;
    //        gvCompare.EditIndexes.Add(item.ItemIndex);
    //    }
        
    //}

    protected void gvCompare_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        try
        {
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;
                Hashtable oldValues = command.OldValues;
                if (newValues["FLDORDEREDQUANTITY"] != oldValues["FLDORDEREDQUANTITY"]) //You may want to implement stronger difference checks here, or a check for the command name (e.g., when inserting there is little point in looking up old values
                {
                    PhoenixPurchaseOrderLine.UpdateOrderLineFromCompareScreen(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(newValues["FLDORDERLINEID"].ToString()),
                            General.GetNullableGuid(ViewState["orderid"].ToString()),
                            Filter.CurrentPurchaseVesselSelection,
                            General.GetNullableDecimal(newValues["FLDORDEREDQUANTITY"].ToString()) != null ? General.GetNullableDecimal(newValues["FLDORDEREDQUANTITY"].ToString()) : 0
                            );
                }

            }
            gvCompare.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCompare_PreRender(object sender, EventArgs e)
    {
        GridTableView masterTable = ((RadGrid)sender).MasterTableView;
        GridNumericColumnEditor editor1 = masterTable.GetBatchColumnEditor("ORDEREDQUANTITY") as GridNumericColumnEditor;
        RadNumericTextBox numBox1 = editor1.NumericTextBox;
        numBox1.IncrementSettings.InterceptArrowKeys = false;
        numBox1.IncrementSettings.InterceptMouseWheel = false;
    }

    protected void cmdSelect_Click(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableGuid(ddlVendor.SelectedValue) != null)
            {
                DataSet ds = PhoenixPurchaseQuotation.QuotationsValidate(General.GetNullableGuid(ViewState["orderid"].ToString()), General.GetNullableGuid(ddlVendor.SelectedValue));
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["FLDRFQLESSTHAN3VENDOR"].ToString() == "1" || dr["FLDSELECTEDHIGHERQUOTE"].ToString() == "1")
                {

                    String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationReason.aspx?orderid=" + ViewState["orderid"].ToString() + "&quoationid=" + ddlVendor.SelectedValue + "&minvendor=" + dr["FLDRFQLESSTHAN3VENDOR"].ToString() + "&higquote=" + dr["FLDSELECTEDHIGHERQUOTE"].ToString() + "');");
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    UpdateSelectVendorForPo(ddlVendor.SelectedValue);
                    InsertOrderFormHistory();
                }
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateSelectVendorForPo(string quotationid)
    {
        try
        {
            PhoenixPurchaseQuotation.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), new Guid(quotationid), Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "PO")));

            PhoenixPurchaseQuotation.SendApprovalInsert
                    (PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), "yes", 1);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
    }
}
