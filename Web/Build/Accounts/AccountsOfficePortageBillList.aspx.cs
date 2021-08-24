using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;
using Telerik.Web.UI.ExportInfrastructure;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using System.IO;

public partial class AccountsOfficePortageBillList : PhoenixBasePage
{
    bool isExport = false;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton(Session["sitepath"] + "/Accounts/AccountsOfficePortageBillList.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            MenuPBExcel.AccessRights = this.ViewState;
            MenuPBExcel.MenuList = toolbargrid.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Portage Bill", "PORTAGEBILL");
            toolbar.AddButton("Leave Report", "LEAVEREPORT");
            toolbar.AddButton("Back", "BACK");
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();
            MenuPB.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                ViewState["OFFSIGNER"] = "0";
                PhoenixAccountsOfficePortageBill.PopulateVesselPortageBillData(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["pbid"]));
            }
            ViewState["EDIT"] = "0";
            //  BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["pno"] != null)
                {
                    if (Request.QueryString["pno"].ToString() != "")
                        Response.Redirect("AccountsOfficePortageBill.aspx?pno=" + Request.QueryString["pno"], true);
                    else
                        Response.Redirect("AccountsOfficePortageBill.aspx", true);
                }
                else
                {
                    Response.Redirect("AccountsOfficePortageBill.aspx", true);
                }
            }
            if (CommandName.ToUpper().Equals("LEAVEREPORT"))
            {
                Response.Redirect("AccountsOfficeLeaveWagesAndPerformanceBonus.aspx?" + Request.QueryString.ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPBExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                gvPB.ExportSettings.FileName = "PortageBill";
                gvPB.MasterTableView.ExportToExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Rebind()
    {
        gvPB.DataSource = null;
        gvPB.Rebind();
    }
    //private void PrepareGridViewForExport(Control gridView)
    //{
    //    for (int i = 0; i < gridView.Controls.Count; i++)
    //    {
    //        //Get the control
    //        Control currentControl = gridView.Controls[i];
    //        if (currentControl is LinkButton)
    //        {
    //            gridView.Controls.Remove(currentControl);
    //            gridView.Controls.AddAt(i, new LiteralControl((currentControl as LinkButton).Text));
    //        }
    //        else if (currentControl is ImageButton)
    //        {
    //            gridView.Controls.Remove(currentControl);
    //            gridView.Controls.AddAt(i, new LiteralControl((currentControl as ImageButton).AlternateText));
    //        }
    //        else if (currentControl is HyperLink)
    //        {
    //            gridView.Controls.Remove(currentControl);
    //            gridView.Controls.AddAt(i, new LiteralControl((currentControl as HyperLink).Text));
    //        }
    //        else if (currentControl is DropDownList)
    //        {
    //            gridView.Controls.Remove(currentControl);
    //            gridView.Controls.AddAt(i, new LiteralControl((currentControl as DropDownList).SelectedItem.Text));
    //        }
    //        else if (currentControl is CheckBox)
    //        {
    //            gridView.Controls.Remove(currentControl);
    //            gridView.Controls.AddAt(i, new LiteralControl((currentControl as CheckBox).Checked ? "True" : "False"));
    //        }
    //        if (currentControl.HasControls())
    //        {
    //            // if there is any child controls, call this method to prepare for export
    //            PrepareGridViewForExport(currentControl);
    //        }
    //    }
    //}
    //public void BindData()
    //{

    //    try
    //    {
    //        ViewState["OFFSIGNER"] = "0";
    //        DataSet ds = PhoenixAccountsOfficePortageBill.ListVesselPortageBillNew(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["pbid"]));
    //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            DataTable dt = ds.Tables[1];
    //            if (ViewState["EDIT"].ToString() != "1")
    //            {
    //                for (int i = 0; i < dt.Rows.Count; i++)
    //                {
    //                    BoundField field = new BoundField();
    //                    field.HeaderText = dt.Rows[i]["FLDGROUPNAME"].ToString();
    //                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
    //                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
    //                    field.FooterText = dt.Rows[i]["FLDAMOUNT"].ToString();
    //                    field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
    //                    field.FooterStyle.Font.Bold = true;
    //                    gvPB.Columns.Insert(gvPB.Columns.Count - 2, field);
    //                }
    //            }
    //            gvPB.DataSource = ds;
    //            gvPB.DataBind();

    //            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
    //            row.Attributes.Add("style", "position:static");
    //            TableCell cell = new TableCell();
    //            cell.ColumnSpan = 7;
    //            row.Cells.Add(cell);

    //            cell = new TableCell();
    //            cell.ColumnSpan = dt.Select("FLDEARNINGDEDUCTION = 1").Length;
    //            cell.HorizontalAlign = HorizontalAlign.Center;
    //            cell.Text = "Earnings";
    //            row.Cells.Add(cell);

    //            cell = new TableCell();
    //            cell.ColumnSpan = dt.Select("FLDEARNINGDEDUCTION = -1").Length;
    //            cell.HorizontalAlign = HorizontalAlign.Center;
    //            cell.Text = "Deductions";
    //            row.Cells.Add(cell);

    //            cell = new TableCell();
    //            cell.ColumnSpan = 5;
    //            cell.HorizontalAlign = HorizontalAlign.Center;
    //            cell.Text = "";
    //            row.Cells.Add(cell);

    //            gvPB.Controls[0].Controls.AddAt(0, row);
    //            // Title1.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"] + " - Period Of " + string.Format("{0:dd/MMM/yyyy}", ds.Tables[0].Rows[0]["FLDSTARTDATE"]) + " to " + string.Format("{0:dd/MMM/yyyy}", ds.Tables[0].Rows[0]["FLDCLOSINGDATE"]);
    //        }
    //        else
    //        {
    //            ShowNoRecordsFound(ds.Tables[0], gvPB);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    public void BindData()
    {

        try
        {
            Guid? uid = Guid.Empty;
            ViewState["OFFSIGNER"] = "0";
            DataSet ds = PhoenixAccountsOfficePortageBill.ListVesselPortageBillNew(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["pbid"]));

            ViewState["UID"] = uid;

            DataTable dt = ds.Tables[1];
            //while (gvPB.Columns.Count > 8)
            //{
            //    gvPB.Columns.RemoveAt(8);
            //}

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GridTemplateColumn field = new GridTemplateColumn();
                field.HeaderText = dt.Rows[i]["FLDGROUPNAME"].ToString();
                field.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                field.ColumnGroupName = dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1" ? "Earnings" : (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1" ? "Deductions" : string.Empty);
                field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                field.FooterText = dt.Rows[i]["FLDAMOUNT"].ToString();
                field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                field.FooterStyle.Font.Bold = true;
                //field.HeaderStyle.Width = Unit.Pixel(field.HeaderText.Length * 5);
                if (field.HeaderText == "B.F")
                {
                    field.HeaderStyle.Width = Unit.Pixel(80);
                }
                else if (field.HeaderText == "ALLOTMENT")
                {
                    field.HeaderStyle.Width = Unit.Pixel(85);
                }
                else
                {
                    field.HeaderStyle.Width = Unit.Pixel(field.HeaderText.Length * 7);
                }
                gvPB.Columns.Insert(gvPB.Columns.Count - 2, field);
            }

            gvPB.MasterTableView.ColumnGroups.FindGroupByName("Earnings").HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            gvPB.MasterTableView.ColumnGroups.FindGroupByName("Deductions").HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

            gvPB.DataSource = ds;
            gvPB.PageSize = ds.Tables[0].Rows.Count;
            gvPB.VirtualItemCount = ds.Tables[0].Rows.Count;
            //txtClosginDate.Text = ds.Tables[0].Rows[0]["FLDCLOSINGDATE"].ToString();
            //txtFromDate.Text = ds.Tables[0].Rows[0]["FLDSTARTDATE"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvPB_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        if (gv.DataSource.GetType().Name != "DataSet") return;
        DataSet ds = (DataSet)gv.DataSource;
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            GridDataItem dataitem = (GridDataItem)e.Item;
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkEmployeename");
            if (isExport)
            {
                if (lbr != null)
                {
                    (e.Item as GridDataItem)["Image"].Visible = false;

                    (e.Item as GridDataItem)["Name"].Text = lbr.Text;
                }
            }

            if (lbr != null)
            {
                if (ViewState["FINALIZEPBYN"].ToString() == "0")
                    lbr.Attributes.Add("onclick", "javascript:openNewWindow('onclick','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsPortageBillPaySlipnew.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&pbyn=0" + "&id=" + drv["FLDUID"].ToString() + "&pbd=" + drv["FLDCLOSINGDATE"].ToString() + "&payd=" + drv["FLDPAYDATE"].ToString() + "&sgid=" + drv["FLDSIGNONOFFID"].ToString() + "'); return false;");
                else
                    lbr.Attributes.Add("onclick", "javascript:openNewWindow('onclick','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsPortageBillPaySlipnew.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&pbyn=1" + "&id=" + drv["FLDPORTAGEBILLID"].ToString() + "&pbd=" + drv["FLDCLOSINGDATE"].ToString() + "&payd=" + drv["FLDPAYDATE"].ToString() + "&sgid=" + drv["FLDSIGNONOFFID"].ToString() + "'); return false;");
            }
            if (drv.Row.Table.Columns.Count > 0)
            {
                string signonoffid = drv["FLDSIGNONOFFID"].ToString();
                DataTable header = ds.Tables[1];
                DataTable data = ds.Tables[2];
                for (int i = 0; i < header.Rows.Count; i++)
                {
                    if (header.Rows[i]["FLDORDER"].ToString() == "1")
                    {
                        DataRow[] dr = data.Select("FLDSIGNONOFFID = " + signonoffid + " AND FLDGROUPNAME = '" + header.Rows[i]["FLDGROUPNAME"].ToString() + "' AND FLDEARNINGDEDUCTION = " + header.Rows[i]["FLDEARNINGDEDUCTION"].ToString());
                        e.Item.Cells[i + 9].Text = (dr.Length > 0 ? dr[0]["FLDAMOUNT"].ToString() : "0.00");
                    }
                    else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1")
                    {
                        e.Item.Cells[i + 9].Text = drv["FLDTOTALEARNINGS"].ToString();
                        e.Item.Cells[i + 9].Attributes.Add("style", "font-weight:bold");
                    }
                    else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1")
                    {
                        e.Item.Cells[i + 9].Text = drv["FLDTOTALDEDUCTION"].ToString();
                        e.Item.Cells[i + 9].Attributes.Add("style", "font-weight:bold");
                    }
                    else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-2")
                    {
                        e.Item.Cells[i + 9].Text = drv["FLDBROUGHTFORWARD"].ToString();
                        e.Item.Cells[i + 9].Attributes.Add("style", "font-weight:bold");
                    }
                    else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-3")
                    {
                        e.Item.Cells[i + 9].Text = drv["FLDFINALBALANCE"].ToString();
                        e.Item.Cells[i + 9].Attributes.Add("style", "font-weight:bold");
                    }
                }
                // if (drv.Row.Table.Columns.Count > 0 && drv["FLDSIGNEDOFFYN"].ToString() == "1" && ViewState["OFFSIGNER"].ToString() == "0")
                if (drv.Row.Table.Columns.Count > 0 && drv["FLDCLOSINGDATE"].ToString() != drv["FLDTODATE"].ToString() && ViewState["OFFSIGNER"].ToString() == "0")
                {
                    ViewState["OFFSIGNER"] = "1";
                    GridDataItem row = new GridDataItem(gvPB.MasterTableView, e.Item.RowIndex, (e.Item as GridDataItem).DataSetIndex);
                    row.Attributes.Add("style", "position:static");
                    GridTableCell cell = new GridTableCell();

                    cell.Visible = true;

                    cell.ColumnSpan = gvPB.Columns.Count + 1;
                    cell.Attributes.Add("style", "font-weight:bold");
                    cell.Text = "Off-Signers";
                    cell.Attributes.Add("Text", "Off-Signers");
                    //row.Controls.Add(cell);
                    row.Cells.Add(cell);
                    gvPB.MasterTableView.Controls[0].Controls.AddAt(e.Item.RowIndex, row);

                }
            }
            System.Web.UI.WebControls.Image nb = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgNegBal");
            if (nb != null)
            {
                if ((drv["FLDFINALBALANCE"].ToString() != string.Empty && decimal.Parse(drv["FLDFINALBALANCE"].ToString()) < 0))
                {
                    nb.Visible = true;
                }
                else
                {
                    nb.Visible = false;
                }
            }
        }
    }
    protected void gvPB_ExportCellFormatting(object sender, ExportCellFormattingEventArgs e)
    {
        GridDataItem item = e.Cell.Parent as GridDataItem;

        gvPB.ExportSettings.Excel.DefaultCellAlignment = HorizontalAlign.Center;
        if (e.FormattedColumn.UniqueName == "Name")
        {
            TableCell cell = item["Name"];
            cell.Text = cell.Text;
        }
    }

    protected void gvPB_InfrastructureExporting(object sender, GridInfrastructureExportingEventArgs e)
    {
        ExportStructure structure = e.ExportStructure;

        structure.Tables[0].Style.Font.Size = 10;

        XlsxRenderer renderer = new XlsxRenderer(structure);
        byte[] xlsxByteArray = renderer.Render();

        XlsxFormatProvider formatProvider = new XlsxFormatProvider();
        Workbook myWorkbook = formatProvider.Import(xlsxByteArray);

        Worksheet worksheet = myWorkbook.ActiveWorksheet;
        worksheet.WorksheetPageSetup.FitToPages = true;

        var colins = worksheet.UsedCellRange.ToIndex.ColumnIndex + 1;
        CellSelection cs = worksheet.Cells[0, 2];
        cs.SetValue("" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "");
        cs.SetIsBold(true);
        CellSelection cs1 = worksheet.Cells[0, 3];
        cs1.SetValue("Period :");
        cs1.SetHorizontalAlignment(RadHorizontalAlignment.Right);
        cs1.SetIsBold(true);
        CellSelection cs2 = worksheet.Cells[0, 4];
        // cs2.SetValue("" + DateTime.Parse(txtFromDate.Text).ToString("dd-MMMM-yyyy") + "");
        CellSelection cs3 = worksheet.Cells[0, 5];
        cs3.SetValue("To :");
        cs3.SetHorizontalAlignment(RadHorizontalAlignment.Right);
        cs3.SetIsBold(true);
        CellSelection cs4 = worksheet.Cells[0, 6];
        //  cs4.SetValue("" + DateTime.Parse(txtClosginDate.Text).ToString("dd-MMMM-yyyy") + "");
        CellSelection cs5 = worksheet.Cells[2, colins];
        cs5.SetValue("Signature");
        cs5.SetIsBold(true);

        byte[] data = formatProvider.Export(myWorkbook);
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.Headers.Remove("Content-Disposition");
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", gvPB.ExportSettings.FileName));
        Response.BinaryWrite(data);
        Response.End();

    }
    //protected void gvPB_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    GridView gv = (GridView)sender;
    //    DataSet ds = (DataSet)gv.DataSource;
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (drv.Row.Table.Columns.Count > 0 && drv["FLDCLOSINGDATE"].ToString() != drv["FLDTODATE"].ToString() && ViewState["OFFSIGNER"].ToString() == "0")
    //        {
    //            ViewState["OFFSIGNER"] = "1";
    //            GridViewRow row = new GridViewRow(e.Row.RowIndex + 1, 0, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
    //            row.Attributes.Add("style", "position:static");
    //            TableCell cell = new TableCell();
    //            cell.Text = "Off-Signers";
    //            cell.ColumnSpan = gvPB.Columns.Count + 1;
    //            cell.Attributes.Add("style", "font-weight:bold");
    //            row.Cells.Add(cell);

    //            gvPB.Controls[0].Controls.AddAt(e.Row.RowIndex + 1, row);

    //        }
    //        if (drv.Row.Table.Columns.Count > 0)
    //        {
    //            string signonoffid = drv["FLDSIGNONOFFID"].ToString();
    //            DataTable header = ds.Tables[1];
    //            DataTable data = ds.Tables[2];
    //            for (int i = 0; i < header.Rows.Count; i++)
    //            {
    //                if (header.Rows[i]["FLDORDER"].ToString() == "1")
    //                {
    //                    DataRow[] dr = data.Select("FLDSIGNONOFFID = " + signonoffid + " AND FLDGROUPNAME = '" + header.Rows[i]["FLDGROUPNAME"].ToString() + "' AND FLDEARNINGDEDUCTION = " + header.Rows[i]["FLDEARNINGDEDUCTION"].ToString());
    //                    e.Row.Cells[i + 7].Text = (dr.Length > 0 ? dr[0]["FLDAMOUNT"].ToString() : "0.00");
    //                }
    //                else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1")
    //                {
    //                    e.Row.Cells[i + 7].Text = drv["FLDTOTALEARNINGS"].ToString();
    //                    e.Row.Cells[i + 7].Attributes.Add("style", "font-weight:bold");
    //                }
    //                else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1")
    //                {
    //                    e.Row.Cells[i + 7].Text = drv["FLDTOTALDEDUCTION"].ToString();
    //                    e.Row.Cells[i + 7].Attributes.Add("style", "font-weight:bold");
    //                }
    //                else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-2")
    //                {
    //                    e.Row.Cells[i + 7].Text = drv["FLDBROUGHTFORWARD"].ToString();
    //                    e.Row.Cells[i + 7].Attributes.Add("style", "font-weight:bold");
    //                }
    //                else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-3")
    //                {
    //                    e.Row.Cells[i + 7].Text = drv["FLDFINALBALANCE"].ToString();
    //                    e.Row.Cells[i + 7].Attributes.Add("style", "font-weight:bold");
    //                }
    //                else if (header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-4")
    //                {
    //                    e.Row.Cells[i + 7].Text = drv["FLDVESSELFINALBALANCE"].ToString();
    //                    e.Row.Cells[i + 7].Attributes.Add("style", "font-weight:bold");
    //                }
    //            }
    //        }
    //        Image nb = (Image)e.Row.FindControl("imgNegBal");
    //        if (nb != null)
    //        {
    //            if ((drv["FLDFINALBALANCE"].ToString() != string.Empty && decimal.Parse(drv["FLDFINALBALANCE"].ToString()) < 0)
    //                )
    //            {
    //                nb.Visible = true;
    //            }
    //            else
    //            {
    //                nb.Visible = false;
    //            }
    //        }
    //        Image fb = (Image)e.Row.FindControl("imgFinalBal");
    //        if (fb != null)
    //        {
    //            if (drv["FLDBALANCEMISMATCH"].ToString() == "1")

    //            {
    //                fb.Visible = true;
    //            }
    //            else
    //            {
    //                fb.Visible = false;
    //            }
    //        }
    //    }
    //}
    //protected void gvPB_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    ViewState["EDIT"] = "1";
    //    BindData();
    //}
    //protected void gvPB_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;
    //    ViewState["EDIT"] = "1";
    //    BindData();
    //    ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtRemarks")).Focus();

    //}
    //protected void gvPB_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        GridViewRow gr = _gridView.Rows[nCurrentRow];
    //        Guid g = new Guid(_gridView.DataKeys[nCurrentRow].Values[0].ToString());
    //        int sid = int.Parse(_gridView.DataKeys[nCurrentRow].Values[1].ToString());
    //        int empid = int.Parse(_gridView.DataKeys[nCurrentRow].Values[2].ToString());

    //        string remarks = ((TextBox)gr.FindControl("txtRemarks")).Text;
    //        if (!IsValidRemarks(remarks))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixAccountsOfficePortageBill.InsertOfficePortageBillRemarks(g, int.Parse(Request.QueryString["vslid"]), empid, remarks, sid);
    //        _gridView.EditIndex = -1;
    //        _gridView.SelectedIndex = -1;
    //        ViewState["EDIT"] = "1";
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private bool IsValidRemarks(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (remarks.Trim().Equals("") || remarks.Trim('.').Equals(""))
            ucError.ErrorMessage = "Office remarks is required.";

        return (!ucError.IsError);
    }

    protected void gvPB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
     
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;
            if (e.CommandName.ToUpper().Equals("SAVE")) 
            {
                Guid lblPortageBill = new Guid(((RadLabel)e.Item.FindControl("lblPortageBill")).Text);
                string lblEmployeeId = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                string lblSignOnOfId = ((RadLabel)e.Item.FindControl("lblSignOnOfId")).Text;
                string remarks = ((TextBox)e.Item.FindControl("txtRemarks")).Text;

                if (!IsValidRemarks(remarks))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOfficePortageBill.InsertOfficePortageBillRemarks(lblPortageBill, int.Parse(Request.QueryString["vslid"]),int.Parse(lblEmployeeId), remarks,int.Parse(lblSignOnOfId));

                ViewState["EDIT"] = "1";
                BindData();
            }

            }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
