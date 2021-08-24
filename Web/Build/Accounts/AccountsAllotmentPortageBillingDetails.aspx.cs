using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsAllotmentPortageBillingDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Request", "REQUEST");
            toolbar.AddButton("LineItem", "LINEITEM");
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Portage Bill", "PORTAGEBILL");
            MenuChecking.AccessRights = this.ViewState;
            MenuChecking.MenuList = toolbarsub.Show();
            MenuChecking.SelectedMenuIndex = 0;

            ViewState["OFFSIGNER"] = "0";
            if (!IsPostBack)
            {
                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();
                ViewState["ALLOTMENTTYPE"]=Request.QueryString["ALLOTMENTTYPE"].ToString();
            }
            BindData();
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
    public void BindData()
    {
        try
        {
            ViewState["OFFSIGNER"] = "0";

            DataSet ds = PhoenixAccountsAllotmentRequestSystemChecking.AllotmentRequestPortageBill(new Guid(ViewState["ALLOTMENTID"].ToString()));
            string[] nonaggcol = { "File No", "Staff Name", "Rank Code", "From", "To", "Days" };
           
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtVesselName.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
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
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //if (hl != null)
                //{
                //    hl.NavigateUrl = "#";
                //    hl.Attributes.Add("onclick", "javascript:Openpopup('PaySlip','','VesselAccountsPortageBillPaySlip.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&pbd=" + drv["To"].ToString() + "&payd=" + drv["FLDPAYDATE"].ToString() + "&sgid=" + drv["FLDSIGNONOFFID"].ToString() + "'); return false;");
                //}
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
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("AccountsAllotmentRequest.aspx?ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString() + "&ALLOTMENTTYPE=" + ViewState["ALLOTMENTTYPE"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("AccountsAllotmentRequestDetails.aspx?ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuChecking_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        }
}
