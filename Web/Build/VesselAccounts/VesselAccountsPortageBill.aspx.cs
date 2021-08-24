using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class VesselAccountsPortageBill :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            ViewState["BNDSUBSIDY"] = "0";
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Generate", "GENPB");
            toolbar.AddButton("Finalize", "FINALIZEPB");
            toolbar.AddButton("UnLock", "UNLOCK");
            DataTable dt = PhoenixVesselAccountsBondSubsidy.FetchVesselBondSubsidy(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0 && General.GetNullableDecimal(dt.Rows[0]["FLDBONDSUBSIDYAMOUNT"].ToString()).HasValue
                && General.GetNullableDecimal(dt.Rows[0]["FLDBONDSUBSIDYAMOUNT"].ToString()).Value > 0)
            {
                toolbar.AddButton("Bond Subsidy", "BONDSUBSIDY");
                ViewState["BNDSUBSIDY"] = "1";
            }
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPortageBill.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenuPBExcel.AccessRights = this.ViewState;
            MenuPBExcel.MenuList = toolbargrid.Show();

            ViewState["OFFSIGNER"] = "0";
            if (!IsPostBack)
            {
                ListPortageBill();
                txtClosginDate.Text = LastDayOfMonthFromDateTime(string.IsNullOrEmpty(txtFromDate.Text) ? DateTime.Now : DateTime.Parse(txtFromDate.Text)).ToString();
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                //if (!(General.GetNullableDateTime(ddlHistory.SelectedValue).HasValue))
                //{
                //    ucError.ErrorMessage = "Portage Bill Date is required.";
                //    ucError.Visible = true;
                //    return;
                //}
                BindData();
                Table tbl = ((Table)gvPB.Controls[0]);
                TableRow row = tbl.Rows[0];
                TableCell cell = new TableCell();
                cell.Text = "";
                row.Cells.Add(cell);
                row = tbl.Rows[1];
                cell.Text = "<b> Signature </b>";
                row.Cells.Add(cell);
                for (int i = 2; i <= gvPB.Rows.Count + 2; i++)
                {
                    row = tbl.Rows[i];
                    cell = new TableCell();
                    cell.Text = "";
                    row.Cells.Add(cell);
                }
                PrepareGridViewForExport(gvPB);
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=PortageBill.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table><tr><td colspan=\"" + (gvPB.Columns.Count + 1) + "\"></td></tr></table>");
                stringwriter.Write("<table><tr><td colspan=\"3\"><b>" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "<b/></td>" +
                    "<td>Period : </td><td>" + DateTime.Parse(txtFromDate.Text).ToString("dd-MMMM-yyyy") + "</td><td>To : </td><td>" + DateTime.Parse(txtClosginDate.Text).ToString("dd-MMMM-yyyy") + "</td>" +
                    "<td colspan=\"" + (gvPB.Columns.Count - 6) + "\"></td></tr></table>");
                stringwriter.Write("<table><tr><td colspan=\"" + (gvPB.Columns.Count + 1) + "\"></td></tr></table>");
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                gvPB.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    /// <summary>
    /// This event is used to verify the form control is rendered
    /// It is used to remove the error occuring while exporting to export
    /// The Error is : Control 'XXX' of type 'GridView' must be placed inside a form tag with runat=server.
    /// </summary>
    /// <param name="control"></param>
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    /// <summary>
    /// Replace any container controls with literals
    /// like Hyperlink, ImageButton, LinkButton, DropDown, ListBox to literals
    /// </summary>
    /// <param name="gridView">GridView</param>
    private void PrepareGridViewForExport(Control gridView)
    {
        for (int i = 0; i < gridView.Controls.Count; i++)
        {
            //Get the control
            Control currentControl = gridView.Controls[i];
            if (currentControl is LinkButton)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as LinkButton).Text));
            }
            else if (currentControl is ImageButton)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as ImageButton).AlternateText));
            }
            else if (currentControl is HyperLink)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as HyperLink).Text));
            }
            else if (currentControl is DropDownList)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as DropDownList).SelectedItem.Text));
            }
            else if (currentControl is CheckBox)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as CheckBox).Checked ? "True" : "False"));
            }
            if (currentControl.HasControls())
            {
                // if there is any child controls, call this method to prepare for export
                PrepareGridViewForExport(currentControl);
            }
        }
    }
    private void ListPortageBill()
    {
        DataTable dt = PhoenixVesselAccountsPortageBill.ListVesselPortageBillHistory(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlHistory.DataSource = dt;
        ddlHistory.DataBind();
        ddlHistory.Items.Insert(0, new ListItem("--Select--", ""));
        if (txtClosginDate.Text != null && txtClosginDate.Text != string.Empty)
        {
            foreach (ListItem li in ddlHistory.Items)
            {
                if (li.Value == string.Empty) continue;
                if (DateTime.Parse(txtClosginDate.Text) == DateTime.Parse(li.Value))
                {
                    li.Selected = true;
                    break;
                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            txtFromDate.Text = General.GetNullableDateTime(dt.Rows[0]["FLDPBCLOSGINDATE"].ToString()).Value.AddDays(1).ToString();
        }
    }
    public void BindData()
    {

        try
        {
            PhoenixVesselAccountsReimbursement.ReverseReimbursementRecovery(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            ViewState["OFFSIGNER"] = "0";
            DateTime d = DateTime.Now;
            DataSet ds = new DataSet();
            if (General.GetNullableDateTime(ddlHistory.SelectedValue).HasValue)
                d = General.GetNullableDateTime(ddlHistory.SelectedValue).Value;
            else if (General.GetNullableDateTime(txtClosginDate.Text).HasValue)
                d = General.GetNullableDateTime(txtClosginDate.Text).Value;

            ds = PhoenixVesselAccountsPortageBill.ListVesselPortageBill(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                  , d);
            txtClosginDate.Text = d.ToString();
            string[] nonaggcol = { "File No", "Staff Name", "Rank Code", "From", "To", "Days" };
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < nonaggcol.Length; i++)
                {
                    if (nonaggcol[i] == "Staff Name")
                    {
                        //string[] str = new string[2];
                        //str[0] = "FLDPORTAGEBILLID";
                        //str[1] = "FLDEMPLOYEEID";
                        HyperLinkField lnk = new HyperLinkField();
                        lnk.HeaderText = nonaggcol[i];
                        lnk.DataTextField = nonaggcol[i];
                        //lnk.DataTextFormatString = "{0:g,1:g}";
                        //lnk.DataNavigateUrlFields = str;
                        //lnk.DataNavigateUrlFormatString = "javascript:Openpopup('PaySlip','','VesselAccountsPortageBillPaySlip.aspx?pbid={0}&empid={1}'); return false;";
                        ////lnk.NavigateUrl = "javascript:Openpopup('PaySlip','','VesselAccountsPortageBillPaySlip.aspx?pbid={0}&empid={1}');";
                        gvPB.Columns.Add(lnk);
                    }
                    else
                    {
                        BoundField field = new BoundField();
                        field.DataField = nonaggcol[i];
                        field.HeaderText = nonaggcol[i];
                        if (i == 3 || i == 4)
                            field.DataFormatString = "{0:dd/MM/yyyy}";
                        gvPB.Columns.Add(field);
                    }
                }
                DataTable dt = ds.Tables[1];
                int ecnt = 0, dcnt = 0;
                for (int i = 0; i < dt.Rows.Count - 2; i++)
                {
                    if (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1") ecnt++; else if (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1") dcnt++;
                    BoundField field = new BoundField();
                    if (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1" && (i == 0 || dt.Rows[i - 1]["FLDEARNINGDEDUCTION"].ToString() == "1"))
                    {
                        field = new BoundField();
                        field.DataField = "FLDTOTALEAR";
                        field.HeaderText = "Total Earnings";
                        field.FooterText = dt.Rows[dt.Rows.Count - 2]["FLDAMOUNT"].ToString();
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.ItemStyle.Font.Bold = true;
                        field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.FooterStyle.Font.Bold = true;
                        gvPB.Columns.Add(field);
                    }

                    if (i == dt.Rows.Count - 3)
                    {
                        field = new BoundField();
                        field.DataField = "FLDTOTALDED";
                        field.HeaderText = "Total Deduction";
                        field.FooterText = dt.Rows[dt.Rows.Count - 3]["FLDAMOUNT"].ToString();
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.ItemStyle.Font.Bold = true;
                        field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.FooterStyle.Font.Bold = true;
                        gvPB.Columns.Add(field);

                        field = new BoundField();
                        field.DataField = "FLDBFAMOUNT";
                        field.HeaderText = "B.F";
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.ItemStyle.Font.Bold = true;
                        gvPB.Columns.Add(field);

                        field = new BoundField();
                        field.DataField = "FLDGRANDTOTAL";
                        field.HeaderText = "Final Balance";
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.ItemStyle.Font.Bold = true;
                        field.FooterText = dt.Rows[dt.Rows.Count - 1]["FLDAMOUNT"].ToString();
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.FooterStyle.Font.Bold = true;
                        gvPB.Columns.Add(field);
                    }
                    else
                    {
                        field = new BoundField();
                        field.DataField = (i + 1).ToString();
                        field.HeaderText = dt.Rows[i]["FLDCOMPONENTNAME"].ToString();
                        field.FooterText = dt.Rows[i]["FLDAMOUNT"].ToString();
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.FooterStyle.Font.Bold = true;
                        gvPB.Columns.Add(field);
                    }
                }
                gvPB.DataSource = ds;
                gvPB.DataBind();
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                row.Attributes.Add("style", "position:static");
                TableCell cell = new TableCell();
                cell.ColumnSpan = 6;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.ColumnSpan = ecnt + 1;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = "Earnings";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.ColumnSpan = dcnt + 1;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = "Deductions";
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.ColumnSpan = 2;
                row.Cells.Add(cell);
                gvPB.Controls[0].Controls.AddAt(0, row);

                txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
            }
            else
            {
                BoundField field = new BoundField();
                field.HeaderText = "";
                gvPB.Columns.Add(field);
                DataTable dt = new DataTable();
                ShowNoRecordsFound(dt, gvPB);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (drv.Row.Table.Columns.Count > 0 && drv["FLDSIGNOFFDATE"].ToString() != string.Empty && ViewState["OFFSIGNER"].ToString() == "0")
            {
                ViewState["OFFSIGNER"] = "1";
                GridViewRow row = new GridViewRow(e.Row.RowIndex + 1, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
                row.Attributes.Add("style", "position:static");
                TableCell cell = new TableCell();
                cell.Text = "Off-Signers";
                cell.ColumnSpan = gvPB.Columns.Count;
                cell.Attributes.Add("style", "font-weight:bold");
                row.Cells.Add(cell);

                gvPB.Controls[0].Controls.AddAt(e.Row.RowIndex + 1, row);
            }
            if (e.Row.Cells.Count > 1)
            {
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                if (hl != null)
                {
                    hl.NavigateUrl = "#";
                    hl.Attributes.Add("onclick", "javascript:Openpopup('PaySlip','','VesselAccountsPortageBillPaySlip.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&pbd=" + drv["To"].ToString() + "&payd=" + drv["FLDPAYDATE"].ToString() + "&sgid=" + drv["FLDSIGNONOFFID"].ToString() + "'); return false;");
                }
            }
        }
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
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FINALIZEPB"))
            {
                if (!IsValidPBLock(txtClosginDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                ucConfirm.Visible = true;
                ucConfirm.Text = (LastDayOfMonthFromDateTime(DateTime.Parse(txtClosginDate.Text)).ToString("dd/MM/yyyy") == DateTime.Parse(txtClosginDate.Text).ToString("dd/MM/yyyy") ? string.Empty : "MID MONTH CLOSING!! ")
                    + "Portage Bill will be Locked till " + txtClosginDate.Text + " and no further changes can be made. Please confirm you want to proceed ?";
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("UNLOCK"))
            {
                if (!IsValidPBUnLock(ddlHistory.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                ucUnlock.Visible = true;
                ucUnlock.Text = "Are you sure you want unlock ?";
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("BONDSUBSIDY"))
            {
                PhoenixVesselAccountsBondSubsidy.UpdateBondSubsidy(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ucStatus.Text = "Bond Subsidy Updated.";
            }
            else if (dce.CommandName.ToUpper().Equals("GENPB"))
            {
                PhoenixVesselAccountsReimbursement.IncludeReimbursementRecovery(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null);
                if (ViewState["BNDSUBSIDY"].ToString() == "1")
                {
                    PhoenixVesselAccountsBondSubsidy.UpdateBondSubsidy(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                }

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!General.GetNullableDateTime(ddlHistory.SelectedValue).HasValue)
                txtClosginDate.Text = LastDayOfMonthFromDateTime(string.IsNullOrEmpty(txtFromDate.Text) ? DateTime.Now : DateTime.Parse(txtFromDate.Text).AddMonths(1)).ToString();
            BindData();
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
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                PhoenixVesselAccountsPortageBill.InsertVesselPortageBill(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtClosginDate.Text), 1);
                ucStatus.Text = "PortageBill Finalized";
                ListPortageBill();
            }
            BindData();
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
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                PhoenixVesselAccountsPortageBill.UnLockVesselPortageBill(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(ddlHistory.SelectedValue));
                ucStatus.Text = "PortageBill UnLocked";
                ListPortageBill();
            }
            BindData();
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
            ucError.ErrorMessage = "Closing Date should be earlier than current";// "date or 10 days prior to monthend.";
        }
        if (General.GetNullableDateTime(txtFromDate.Text).HasValue && General.GetNullableDateTime(date).HasValue
            && (General.GetNullableDateTime(txtFromDate.Text).Value.Month != General.GetNullableDateTime(date).Value.Month
            || General.GetNullableDateTime(txtFromDate.Text).Value.Year != General.GetNullableDateTime(date).Value.Year))
        {
            ucError.ErrorMessage = "Portage Bill can only be finalized if the From Date and the To Date are of the same month.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidPBUnLock(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Closing Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)// && DateTime.Compare(LastDayOfMonthFromDateTime(resultDate).AddDays(-10), resultDate) > 0)
        {
            ucError.ErrorMessage = "Closing Date should be earlier than current";// "date or 10 days prior to monthend.";
        }
        return (!ucError.IsError);
    }
    private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
    }
}
