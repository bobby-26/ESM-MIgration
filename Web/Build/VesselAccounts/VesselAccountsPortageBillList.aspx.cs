using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Globalization;
using Telerik.Web.UI;
using Telerik.Web.UI.ExportInfrastructure;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using System.IO;

public partial class VesselAccountsPortageBillList : PhoenixBasePage
{

    bool isExport = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("UnLock", "UNLOCK", ToolBarDirection.Right);
            toolbar.AddButton("Finalize", "FINALIZEPB", ToolBarDirection.Right);
            toolbar.AddButton("Generate", "GENPB", ToolBarDirection.Right);
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton(Session["sitepath"] + "/VesselAccounts/VesselAccountsPortageBillList.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            //toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPortageBillList.aspx", "Switch Old PB", "61.png", "SWITCH");
            MenuPBExcel.AccessRights = this.ViewState;
            MenuPBExcel.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                confirmunlock.Attributes.Add("style", "display:none");
                //ucConfirm.Attributes.Add("style", "visibility:hidden;");
                ViewState["UID"] = Guid.Empty;
                ViewState["OFFSIGNER"] = "0";
                ViewState["FINALIZEPBYN"] = "0";
                PopulateLockedPortageBill();
                txtClosginDate.Text = LastDayOfMonthFromDateTime(string.IsNullOrEmpty(txtFromDate.Text) ? DateTime.Now : DateTime.Parse(txtFromDate.Text)).ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void PopulateLockedPortageBill()
    {
        DataTable dt = PhoenixVesselAccountsPortageBill.ListPortageBillLocked(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlHistory.DataSource = dt;
        ddlHistory.DataTextFormatString = "{0:" + CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern + "}";
        ddlHistory.DataBind();
        ddlHistory.Items.Insert(0, new DropDownListItem("--Select--", ""));
        if (dt.Rows.Count > 0)
        {
            txtFromDate.Text = General.GetNullableDateTime(dt.Rows[0]["FLDCLOSINGDATE"].ToString()).Value.AddDays(1).ToString();
        }
    }
    protected void ddlHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!General.GetNullableDateTime(ddlHistory.SelectedValue).HasValue)
                txtClosginDate.Text = LastDayOfMonthFromDateTime(string.IsNullOrEmpty(txtFromDate.Text) ? DateTime.Now : DateTime.Parse(txtFromDate.Text).AddMonths(1)).ToString();
            Rebind();

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
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FINALIZEPB"))
            {
                if (!IsValidPBLock(txtClosginDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                string confirmtext;


                confirmtext = "Portage Bill will be Locked till " + txtClosginDate.Text + " and no further changes can be made. Please confirm you want to proceed ?";
                RadWindowManager1.RadConfirm(confirmtext, "confirm", 320, 150, null, "Confirm");
            }
            else if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                if (!IsValidPBUnLock())
                {
                    ucError.Visible = true;
                    return;
                }
                string confirmtextunlock;

                confirmtextunlock = "Are you sure you want unlock ?";
                RadWindowManager2.RadConfirm(confirmtextunlock, "confirmunlock", 320, 150, null, "Confirm");
            }
            else if (CommandName.ToUpper().Equals("GENPB"))
            {
                if (!General.GetNullableDateTime(txtClosginDate.Text).HasValue)
                {
                    txtClosginDate.Text = LastDayOfMonthFromDateTime(string.IsNullOrEmpty(txtFromDate.Text) ? DateTime.Now : DateTime.Parse(txtFromDate.Text)).ToString();
                }
                PhoenixVesselAccountsReimbursement.IncludeReimbursementRecovery(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null);
                Rebind();
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
            else if (CommandName.ToUpper().Equals("SWITCH"))
            {
                Response.Redirect("VesselAccountsPortageBill.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void FinalizePortageBill_Confirm(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsPortageBill.FinalizePortageBill(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                , DateTime.Parse(txtClosginDate.Text)
                                                , new Guid(ViewState["UID"].ToString())
                                                , null);
            ucStatus.Text = "PortageBill Finalized";
            PopulateLockedPortageBill();
            txtClosginDate.Text = LastDayOfMonthFromDateTime(string.IsNullOrEmpty(txtFromDate.Text) ? DateTime.Now : DateTime.Parse(txtFromDate.Text)).ToString();
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void UnLockPortageBill_Confirm(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsPortageBill.UnLockPortageBill(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ddlHistory.SelectedValue));
            ucStatus.Text = "PortageBill UnLocked";
            PopulateLockedPortageBill();
            txtClosginDate.Text = LastDayOfMonthFromDateTime(string.IsNullOrEmpty(txtFromDate.Text) ? DateTime.Now : DateTime.Parse(txtFromDate.Text)).ToString();

            Rebind();
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
            Guid? uid = Guid.Empty;
            ViewState["OFFSIGNER"] = "0";
            DateTime d = DateTime.Now;

            if (General.GetNullableDateTime(ddlHistory.SelectedValue).HasValue)
                d = General.GetNullableDateTime(ddlHistory.SelectedItem.Text).Value;
            else if (General.GetNullableDateTime(txtClosginDate.Text).HasValue)
                d = General.GetNullableDateTime(txtClosginDate.Text).Value;
            DataSet ds;

            if (!IsPostBack)
            {
                gvPB.DataSource = null;
            }
            else
            {
                if (General.GetNullableGuid(ddlHistory.SelectedValue).HasValue)
                {
                    ds = PhoenixVesselAccountsPortageBill.ListPortageBillFinalized(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ddlHistory.SelectedValue));
                    ViewState["FINALIZEPBYN"] = "1";
                }
                else
                {
                    ds = PhoenixVesselAccountsPortageBill.ListPortageBill(PhoenixSecurityContext.CurrentSecurityContext.VesselID, d, ref uid);
                    ViewState["FINALIZEPBYN"] = "0";
                }

                ViewState["UID"] = uid;

                DataTable dt = ds.Tables[1];
                while (gvPB.Columns.Count > 7)
                {
                    gvPB.Columns.RemoveAt(7);
                }

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
                    gvPB.Columns.Insert(gvPB.Columns.Count, field);
                }

                gvPB.MasterTableView.ColumnGroups.FindGroupByName("Earnings").HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                gvPB.MasterTableView.ColumnGroups.FindGroupByName("Deductions").HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                gvPB.DataSource = ds;
                gvPB.PageSize = ds.Tables[0].Rows.Count;
                gvPB.VirtualItemCount = ds.Tables[0].Rows.Count;
                txtClosginDate.Text = ds.Tables[0].Rows[0]["FLDCLOSINGDATE"].ToString();
                txtFromDate.Text = ds.Tables[0].Rows[0]["FLDSTARTDATE"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidPBLock(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Closing Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)// && DateTime.Compare(LastDayOfMonthFromDateTime(resultDate).AddDays(-10), resultDate) > 0)
        {
            ucError.ErrorMessage = "Closing Date should be earlier than current date";// "date or 10 days prior to monthend.";
        }
        if (General.GetNullableDateTime(txtFromDate.Text).HasValue && General.GetNullableDateTime(date).HasValue
            && (General.GetNullableDateTime(txtFromDate.Text).Value.Month != General.GetNullableDateTime(date).Value.Month
            || General.GetNullableDateTime(txtFromDate.Text).Value.Year != General.GetNullableDateTime(date).Value.Year))
        {
            ucError.ErrorMessage = "Portage Bill can only be finalized if the From Date and the To Date are of the same month.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidPBUnLock()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableGuid(ddlHistory.SelectedValue).HasValue)
        {
            ucError.ErrorMessage = "Portage Bill History is required.";
        }
        return (!ucError.IsError);
    }
    private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
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

                if (drv.Row.Table.Columns.Count > 0 && drv["FLDSIGNEDOFFYN"].ToString() == "1" && ViewState["OFFSIGNER"].ToString() == "0")
                {
                    ViewState["OFFSIGNER"] = "1";
                    GridDataItem row = new GridDataItem(gvPB.MasterTableView, e.Item.RowIndex, (e.Item as GridDataItem).DataSetIndex);
                    row.Attributes.Add("style", "position:static");
                    GridTableCell cell = new GridTableCell();
                    cell.Text = "Off-Signers";
                    cell.Visible = true;
                    cell.ColumnSpan = gvPB.Columns.Count + 1;
                    cell.Attributes.Add("style", "font-weight:bold");
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
        if (!string.IsNullOrEmpty(txtFromDate.Text))
        {
            CellSelection cs2 = worksheet.Cells[0, 4];
            cs2.SetValue("" + DateTime.Parse(txtFromDate.Text).ToString("dd-MMMM-yyyy") + "");
        }
        CellSelection cs3 = worksheet.Cells[0, 5];
        cs3.SetValue("To :");
        cs3.SetHorizontalAlignment(RadHorizontalAlignment.Right);
        cs3.SetIsBold(true);
        CellSelection cs4 = worksheet.Cells[0, 6];
        cs4.SetValue("" + DateTime.Parse(txtClosginDate.Text).ToString("dd-MMMM-yyyy") + "");
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
}