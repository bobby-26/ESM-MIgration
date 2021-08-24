using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

public partial class AccountsOfficePortageBillPostingDraft : PhoenixBasePage
{
    string vslid = string.Empty, pbid = string.Empty, pbno = string.Empty;
    //string url, path = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            vslid = Request.QueryString["vslid"];
            pbid = Request.QueryString["pbid"];
            pbno = Request.QueryString["pbno"];

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("With Budget", "WITHBUDGET");
            toolbar.AddButton("Without Budget", "WITHOUTBUDGET");
            toolbar.AddButton("Post Voucher", "POST");
            toolbar.AddButton("Arrears", "ARREARS");
            toolbar.AddButton("Back", "VOUCHER");
            MenuPB1.AccessRights = this.ViewState;
            MenuPB1.MenuList = toolbar.Show();
            MenuPB1.SelectedMenuIndex = 2;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post Voucher", "POST", ToolBarDirection.Right);
            MenuPB2.AccessRights = this.ViewState;
            MenuPB2.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOfficePortageBillPostingDraft.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"], "Export to Excel", "icon_xls.png", "Excel");

            MenuPBExcel.AccessRights = this.ViewState;
            MenuPBExcel.MenuList = toolbargrid.Show();

            MenuLWPBExcel.AccessRights = this.ViewState;
            MenuLWPBExcel.MenuList = toolbargrid.Show();

            MenuSLExcel.AccessRights = this.ViewState;
            MenuSLExcel.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                PhoenixAccountsOfficePortageBill.PopulateVesselPortageBillData(int.Parse(vslid), new Guid(pbid));
                gvPB.ExportSettings.ExportOnlyData = true;
                gvSL.ExportSettings.ExportOnlyData = true;
                gvLWPB.ExportSettings.ExportOnlyData = true;

            }
            ViewState["PBRECORDCOUNT"] = "0";
            ViewState["SLRECORDCOUNT"] = "0";
            ViewState["LWPBRECORDCOUNT"] = "0";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuPB1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("WITHBUDGET"))
            {
                Response.Redirect("../Accounts/AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"] + "&withbudget=1");
            }
            if (CommandName.ToUpper().Equals("WITHOUTBUDGET"))
            {
                Response.Redirect("../Accounts/AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"] + "&withbudget=0");
            }
            if (CommandName.ToUpper().Equals("POST"))
            {
                Response.Redirect("../Accounts/AccountsOfficePortageBillPostingDraft.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"]);
            }
            if (CommandName.ToUpper().Equals("ARREARS"))
            {
                Response.Redirect("AccountsOfficeWagesAdjustmentPosting.aspx?pbid=" + pbid + "&vslid=" + vslid + "&pno=" + Request.QueryString["pno"], true);
            }
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("AccountsOfficePortageBillPosting.aspx?pno=" + Request.QueryString["pno"], true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPB2_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("POST"))
            {
                if (!IsValidPosting())
                {
                    ucError.Visible = true;
                    return;
                }

                Guid VoucherFileDtKey = Guid.Empty;
                bool msg = false;
                PhoenixAccountsOfficePortageBill.OfficePortageBillVoucherPosting(int.Parse(vslid), new Guid(pbid), ref VoucherFileDtKey);
                if (VoucherFileDtKey != Guid.Empty)
                {
                    AddAttachment(VoucherFileDtKey, 1);
                    msg = true;
                }
                Guid VoucherFileDtKey1 = Guid.Empty;
                PhoenixAccountsOfficePortageBill.OfficeSideLetterVoucherPosting(int.Parse(vslid), new Guid(pbid), ref VoucherFileDtKey1);
                if (VoucherFileDtKey1 != Guid.Empty)
                {
                    AddAttachment(VoucherFileDtKey1, 2);
                    msg = true;
                }
                Guid VoucherFileDtKey2 = Guid.Empty;
                PhoenixAccountsOfficePortageBill.OfficeLeaveWagesVoucherPosting(int.Parse(vslid), new Guid(pbid), ref VoucherFileDtKey2);
                if (VoucherFileDtKey2 != Guid.Empty)
                {
                    AddAttachment(VoucherFileDtKey2, 3);
                    msg = true;
                }

                RadWindowManager1.RadAlert("Voucher" + (msg ? "" : " Already") + " Posted.", 320, 150, null, "");
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
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=PortageBill.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table><tr><td colspan=\"" + (gvPB.Columns.Count) + "\">PORTAGE BILL FOR THE PERIOD :" + ViewState["DATEFORMAT"].ToString() + "</td></tr><tr><td colspan=\"" + (gvPB.Columns.Count) + "\">" + ViewState["VESSELNAME"].ToString() + "<td></tr><tr><td colspan=\"" + (gvPB.Columns.Count) + "\"></td></tr></table>");
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
    protected void MenuLWPBExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=OnboardAccruals.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table><tr><td colspan=\"" + (gvLWPB.Columns.Count) + "\">ONBOARD ACCRUALS FOR THE PERIOD :" + ViewState["DATEFORMAT"].ToString() + "</td></tr><tr><td colspan=\"" + (gvLWPB.Columns.Count) + "\">" + ViewState["VESSELNAME"].ToString() + "<td></tr><tr><td colspan=\"" + (gvLWPB.Columns.Count) + "\"></td></tr></table>");
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                gvLWPB.RenderControl(htmlwriter);
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
    protected void MenuSLExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=OfficeAccruals.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table><tr><td colspan=\"" + (gvSL.Columns.Count) + "\">OFFICE ACCRUAL FOR THE PERIOD :" + ViewState["DATEFORMAT"].ToString() + "</td></tr><tr><td colspan=\"" + (gvSL.Columns.Count) + "\">" + ViewState["VESSELNAME"].ToString() + "<td></tr><tr><td colspan=\"" + (gvSL.Columns.Count) + "\"></td></tr></table>");
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                gvSL.RenderControl(htmlwriter);
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
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {

    }
    public void BindData()
    {
        try
        {
            ViewState["OFFSIGNER1"] = "0";
            DataSet ds = PhoenixAccountsOfficePortageBill.OfficePortageBillPostingDraftView(int.Parse(vslid), new Guid(pbid), 1);
            gvPB.Columns.Clear();

            GridBoundColumn col = new GridBoundColumn();
            col.HeaderText = "File No.";
            col.DataField = "FLDFILENO";
            col.ColumnGroupName = "Empty";
            col.HeaderStyle.Width = Unit.Pixel(70);
            gvPB.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "Employee";
            col.DataField = "FLDEMPLOYEENAME";
            col.ColumnGroupName = "Empty";
            col.HeaderStyle.Width = Unit.Pixel(240);
            gvPB.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "Rank";
            col.DataField = "FLDRANKCODE";
            col.ColumnGroupName = "Empty";
            col.HeaderStyle.Width = Unit.Pixel(70);
            gvPB.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "From";
            col.DataField = "FLDFROMDATE";
            col.DataFormatString = "{0:dd/MM/yyyy}";
            col.HeaderStyle.Width = Unit.Pixel(70);
            col.ColumnGroupName = "Empty";
            gvPB.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "To";
            col.DataField = "FLDTODATE";
            col.DataFormatString = "{0:dd/MM/yyyy}";
            col.HeaderStyle.Width = Unit.Pixel(70);
            col.ColumnGroupName = "Empty";
            col.FooterText = "Total";
            gvPB.MasterTableView.Columns.Add(col);

            gvPB.DataSource = ds;
            gvPB.PageSize = ds.Tables[0].Rows.Count;
            gvPB.VirtualItemCount = ds.Tables[0].Rows.Count;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["PBRECORDCOUNT"] = "1";
                DataTable dt = ds.Tables[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GridTemplateColumn field = new GridTemplateColumn();
                    field.HeaderText = dt.Rows[i]["FLDSUBACCOUNT"].ToString();
                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    field.ColumnGroupName = (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1" && dt.Rows[i]["FLDLOGTYPE"].ToString() == "1") ? "Earnings" : ((dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1" && dt.Rows[i]["FLDLOGTYPE"].ToString() == "1") ? "Deductions" : (dt.Rows[i]["FLDLOGTYPE"].ToString() == "6" ? "BF" : (dt.Rows[i]["FLDLOGTYPE"].ToString() == "10" ? "FB" : string.Empty)));
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterText = dt.Rows[i]["FLDAMOUNT"].ToString();
                    field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.Font.Bold = true;
                    field.UniqueName = dt.Rows[i]["FLDSUBACCOUNT"].ToString();
                    if (dt.Rows[i]["FLDLOGTYPE"].ToString() == "6")
                        field.HeaderStyle.Width = Unit.Pixel(110);
                    else
                        field.HeaderStyle.Width = Unit.Pixel(95);
                    gvPB.Columns.Insert(gvPB.Columns.Count, field);
                }

                ViewState["VESSELNAME"] = "Vessel - " + ds.Tables[0].Rows[0]["FLDVESSELNAME"];
                ViewState["DATEFORMAT"] = string.Format("{0:dd/MMM/yyyy}", ds.Tables[0].Rows[0]["FLDSTARTDATE"]) + " to " + string.Format("{0:dd/MMM/yyyy}", ds.Tables[0].Rows[0]["FLDENDDATE"]);
                txtvessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"] + " - Period Of " + string.Format("{0:dd/MMM/yyyy}", ds.Tables[0].Rows[0]["FLDSTARTDATE"]) + " to " + string.Format("{0:dd/MMM/yyyy}", ds.Tables[0].Rows[0]["FLDENDDATE"]);
            }
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

        if (e.Item is GridDataItem)
        {
            GridDataItem dataitem = (GridDataItem)e.Item;
            if (ViewState["PBRECORDCOUNT"].ToString() != "0")
            {
                DataSet ds = (DataSet)gv.DataSource;
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string a = drv["FLDSIGNEDOFFYN"].ToString();
                string a1 = ViewState["OFFSIGNER1"].ToString();
                if (drv.Row.Table.Columns.Count > 0 && drv["FLDSIGNEDOFFYN"].ToString() == "1" && ViewState["OFFSIGNER1"].ToString() == "0")
                {
                    ViewState["OFFSIGNER1"] = "1";
                    GridDataItem row = new GridDataItem(gvPB.MasterTableView, e.Item.RowIndex, (e.Item as GridDataItem).DataSetIndex);
                    row.Attributes.Add("style", "position:static");
                    GridTableCell cell = new GridTableCell();
                    cell.Text = "Off-Signers";
                    cell.Visible = true;
                    cell.ColumnSpan = gvPB.Columns.Count + 1;
                    cell.Attributes.Add("style", "font-weight:bold");
                    row.Cells.Add(cell);
                    gvPB.MasterTableView.Controls[0].Controls.AddAt(e.Item.RowIndex, row);
                }
                string sgid = drv["FLDSIGNONOFFID"].ToString();
                DataTable header = ds.Tables[1];
                DataTable data = ds.Tables[2];
                for (int i = 0; i < header.Rows.Count; i++)
                {
                    string budgetcode = gv.Columns[i].HeaderText;
                    string logtype = header.Rows[i]["FLDLOGTYPE"].ToString();
                    DataRow[] dr = data.Select("FLDSIGNONOFFID = " + sgid + " AND FLDSUBACCOUNT = '" + header.Rows[i]["FLDSUBACCOUNT"].ToString() + "' AND FLDEARNINGDEDUCTION = " + header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() + " AND FLDLOGTYPE = " + logtype);
                    e.Item.Cells[i + 7].Text = (dr.Length > 0 ? dr[0]["FLDAMOUNT"].ToString() : "0.00");
                }
            }
        }
    }
    private bool IsValidPosting()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(txtTotal.Text).HasValue && General.GetNullableDecimal(txtTotal.Text).Value != 0)
            ucError.ErrorMessage = "Earnings - Deductions + B.F - Final Balance should be equal to Zero.";

        return (!ucError.IsError);
    }
    protected void gvLWPB_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        if (gv.DataSource.GetType().Name != "DataSet") return;

        if (e.Item is GridDataItem)
        {
            GridDataItem dataitem = (GridDataItem)e.Item;
            if (ViewState["LWPBRECORDCOUNT"].ToString() != "0")
            {
                DataSet ds = (DataSet)gv.DataSource;
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (drv.Row.Table.Columns.Count > 0 && drv["FLDSIGNEDOFFYN"].ToString() == "1" && ViewState["OFFSIGNER2"].ToString() == "0")
                {
                    ViewState["OFFSIGNER2"] = "1";
                    GridDataItem row = new GridDataItem(gvPB.MasterTableView, e.Item.RowIndex, (e.Item as GridDataItem).DataSetIndex);
                    row.Attributes.Add("style", "position:static");
                    GridTableCell cell = new GridTableCell();
                    cell.Text = "Off-Signers";
                    cell.Visible = true;
                    cell.ColumnSpan = gvPB.Columns.Count + 1;
                    cell.Attributes.Add("style", "font-weight:bold");
                    row.Cells.Add(cell);
                    gvLWPB.MasterTableView.Controls[0].Controls.AddAt(e.Item.RowIndex, row);
                }
                string sgid = drv["FLDSIGNONOFFID"].ToString();
                DataTable header = ds.Tables[1];
                DataTable data = ds.Tables[2];
                for (int i = 0; i < header.Rows.Count; i++)
                {
                    string budgetcode = gv.Columns[i].HeaderText;
                    string logtype = header.Rows[i]["FLDLOGTYPE"].ToString();
                    DataRow[] dr = data.Select("FLDSIGNONOFFID = " + sgid + " AND FLDSUBACCOUNT = '" + header.Rows[i]["FLDSUBACCOUNT"].ToString() + "' AND FLDEARNINGDEDUCTION = " + header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() + " AND FLDLOGTYPE = " + logtype);
                    e.Item.Cells[i + 7].Text = (dr.Length > 0 ? dr[0]["FLDAMOUNT"].ToString() : "0.00");
                }
            }
        }
    }
    protected void gvLWPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindLWPBData();
    }
    public void BindLWPBData()
    {
        try
        {
            ViewState["OFFSIGNER2"] = "0";
            DataSet ds = PhoenixAccountsOfficePortageBill.OfficePortageBillPostingDraftView(int.Parse(vslid), new Guid(pbid), 3);
            gvLWPB.Columns.Clear();

            GridBoundColumn col = new GridBoundColumn();
            col.HeaderText = "File No.";
            col.DataField = "FLDFILENO";
            col.ColumnGroupName = "Empty";
            col.HeaderStyle.Width = Unit.Pixel(70);
            gvLWPB.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "Employee";
            col.DataField = "FLDEMPLOYEENAME";
            col.ColumnGroupName = "Empty";
            col.HeaderStyle.Width = Unit.Pixel(240);
            gvLWPB.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "Rank";
            col.DataField = "FLDRANKCODE";
            col.ColumnGroupName = "Empty";
            col.HeaderStyle.Width = Unit.Pixel(70);
            gvLWPB.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "From";
            col.DataField = "FLDFROMDATE";
            col.DataFormatString = "{0:dd/MM/yyyy}";
            col.HeaderStyle.Width = Unit.Pixel(70);
            col.ColumnGroupName = "Empty";
            gvLWPB.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "To";
            col.DataField = "FLDTODATE";
            col.DataFormatString = "{0:dd/MM/yyyy}";
            col.HeaderStyle.Width = Unit.Pixel(70);
            col.ColumnGroupName = "Empty";
            col.FooterText = "Total";
            gvLWPB.MasterTableView.Columns.Add(col);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["LWPBRECORDCOUNT"] = "1";
                DataTable dt = ds.Tables[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GridTemplateColumn field = new GridTemplateColumn();
                    field.HeaderText = dt.Rows[i]["FLDSUBACCOUNT"].ToString();
                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    field.ColumnGroupName = (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1" && dt.Rows[i]["FLDLOGTYPE"].ToString() == "1") ? "Earnings" : ((dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1" && dt.Rows[i]["FLDLOGTYPE"].ToString() == "1") ? "Deductions" : (dt.Rows[i]["FLDLOGTYPE"].ToString() == "6" ? "BF" : (dt.Rows[i]["FLDLOGTYPE"].ToString() == "10" ? "FB" : string.Empty)));
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterText = dt.Rows[i]["FLDAMOUNT"].ToString();
                    field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.Font.Bold = true;
                    field.UniqueName = dt.Rows[i]["FLDSUBACCOUNT"].ToString();
                    if (dt.Rows[i]["FLDLOGTYPE"].ToString() == "6")
                        field.HeaderStyle.Width = Unit.Pixel(110);
                    else
                        field.HeaderStyle.Width = Unit.Pixel(95);
                    gvLWPB.Columns.Insert(gvLWPB.Columns.Count, field);
                }

                gvLWPB.DataSource = ds;



            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindSLData()
    {
        try
        {
            ViewState["OFFSIGNER3"] = "0";
            DataSet ds = PhoenixAccountsOfficePortageBill.OfficePortageBillPostingDraftView(int.Parse(vslid), new Guid(pbid), 2);
            gvSL.Columns.Clear();

            GridBoundColumn col = new GridBoundColumn();
            col.HeaderText = "File No.";
            col.DataField = "FLDFILENO";
            col.ColumnGroupName = "Empty";
            col.HeaderStyle.Width = Unit.Pixel(70);
            gvSL.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "Employee";
            col.DataField = "FLDEMPLOYEENAME";
            col.ColumnGroupName = "Empty";
            col.HeaderStyle.Width = Unit.Pixel(240);
            gvSL.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "Rank";
            col.DataField = "FLDRANKCODE";
            col.ColumnGroupName = "Empty";
            col.HeaderStyle.Width = Unit.Pixel(70);
            gvSL.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "From";
            col.DataField = "FLDFROMDATE";
            col.DataFormatString = "{0:dd/MM/yyyy}";
            col.HeaderStyle.Width = Unit.Pixel(70);
            col.ColumnGroupName = "Empty";
            gvSL.MasterTableView.Columns.Add(col);

            col = new GridBoundColumn();
            col.HeaderText = "To";
            col.DataField = "FLDTODATE";
            col.DataFormatString = "{0:dd/MM/yyyy}";
            col.HeaderStyle.Width = Unit.Pixel(70);
            col.ColumnGroupName = "Empty";
            col.FooterText = "Total";
            gvSL.MasterTableView.Columns.Add(col);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["SLRECORDCOUNT"] = "1";
                DataTable dt = ds.Tables[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GridTemplateColumn field = new GridTemplateColumn();
                    field.HeaderText = dt.Rows[i]["FLDSUBACCOUNT"].ToString();
                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    field.ColumnGroupName = (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1" && dt.Rows[i]["FLDLOGTYPE"].ToString() == "1") ? "Earnings" : ((dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1" && dt.Rows[i]["FLDLOGTYPE"].ToString() == "1") ? "Deductions" : (dt.Rows[i]["FLDLOGTYPE"].ToString() == "6" ? "BF" : (dt.Rows[i]["FLDLOGTYPE"].ToString() == "10" ? "FB" : string.Empty)));
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterText = dt.Rows[i]["FLDAMOUNT"].ToString();
                    field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.Font.Bold = true;
                    field.UniqueName = dt.Rows[i]["FLDSUBACCOUNT"].ToString();
                    if (dt.Rows[i]["FLDLOGTYPE"].ToString() == "6")
                        field.HeaderStyle.Width = Unit.Pixel(110);
                    else
                        field.HeaderStyle.Width = Unit.Pixel(95);
                    gvSL.Columns.Insert(gvSL.Columns.Count, field);
                }
                gvSL.DataSource = ds;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSL_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        if (gv.DataSource.GetType().Name != "DataSet") return;

        if (e.Item is GridDataItem)
        {
            GridDataItem dataitem = (GridDataItem)e.Item;
            if (ViewState["SLRECORDCOUNT"].ToString() != "0")
            {

                {
                    DataSet ds = (DataSet)gv.DataSource;
                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    string a = drv["FLDSIGNEDOFFYN"].ToString();
                    if (drv.Row.Table.Columns.Count > 0 && drv["FLDSIGNEDOFFYN"].ToString() == "1" && ViewState["OFFSIGNER3"].ToString() == "0")
                    {
                        ViewState["OFFSIGNER3"] = "1";
                        GridDataItem row = new GridDataItem(gvPB.MasterTableView, e.Item.RowIndex, (e.Item as GridDataItem).DataSetIndex);
                        row.Attributes.Add("style", "position:static");
                        GridTableCell cell = new GridTableCell();
                        cell.Text = "Off-Signers";
                        cell.Visible = true;
                        cell.ColumnSpan = gvPB.Columns.Count + 1;
                        cell.Attributes.Add("style", "font-weight:bold");
                        row.Cells.Add(cell);

                        gvSL.MasterTableView.Controls[0].Controls.AddAt(e.Item.RowIndex, row);
                    }
                    string sgid = drv["FLDSIGNONOFFID"].ToString();
                    DataTable header = ds.Tables[1];
                    DataTable data = ds.Tables[2];
                    for (int i = 0; i < header.Rows.Count; i++)
                    {
                        string budgetcode = gv.Columns[i].HeaderText;
                        string logtype = header.Rows[i]["FLDLOGTYPE"].ToString();
                        DataRow[] dr = data.Select("FLDSIGNONOFFID = " + sgid + " AND FLDSUBACCOUNT = '" + header.Rows[i]["FLDSUBACCOUNT"].ToString() + "' AND FLDEARNINGDEDUCTION = " + header.Rows[i]["FLDEARNINGDEDUCTION"].ToString() + " AND FLDLOGTYPE = " + logtype);
                        e.Item.Cells[i + 7].Text = (dr.Length > 0 ? dr[0]["FLDAMOUNT"].ToString() : "0.00");
                    }
                }
            }
        }
    }
    protected void gvSL_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindSLData();
    }
    public void AddAttachment(Guid VoucherFileDtKey, int VoucherType)
    {
        //server folder path which is stored your PDF documents
        //string path = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/");
        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/ACCOUNTS/";
        string filefullpath = path + VoucherFileDtKey + ".pdf";

        string Title = ViewState["DATEFORMAT"].ToString();
        if (VoucherType == 1)
        {
            //gvPB.Rebind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);

            gvPB.RenderControl(hw);

            string html = StrHtml(sb.ToString());
            converttopdf(html, filefullpath, "PORTAGE BILL FOR THE PERIOD :" + Title);
        }
        else if (VoucherType == 2)
        {
            //gvSL.Rebind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);

            gvSL.RenderControl(hw);

            string html = StrHtml(sb.ToString());
            converttopdf(html, filefullpath, "ONBOARD ACCRUALS FOR THE PERIOD :" + Title);
        }
        else if (VoucherType == 3)
        {
            //gvLWPB.Rebind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);

            gvLWPB.RenderControl(hw);

            string html = StrHtml(sb.ToString());
            converttopdf(html, filefullpath, "OFFICE ACCRUAL FOR THE PERIOD :" + Title);

        }
    }
    private string StrHtml(string gvHtmlTable)
    {
        string strHtml = gvHtmlTable;
        strHtml = strHtml.Replace("&nbsp;", "");

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.LoadXml(strHtml);

        XmlNodeList tablelist = XmlDoc.SelectNodes("//table");
        XmlNode temp = null;
        foreach (XmlNode tbl in tablelist)
        {
            if (tbl.Attributes["class"] != null)
                tbl.Attributes.Remove(tbl.Attributes["class"]);
            if (tbl.Attributes["style"] != null)
                tbl.Attributes.Remove(tbl.Attributes["style"]);
            if (tbl.Attributes["border"] != null)
                tbl.Attributes.Remove(tbl.Attributes["border"]);
            XmlAttribute att = XmlDoc.CreateAttribute("style");
            att.InnerText = "font-size: 11px;width:100%;border-collapse:collapse;";
            tbl.Attributes.Append(att);

            att = XmlDoc.CreateAttribute("border");
            att.InnerText = "1";
            tbl.Attributes.Append(att);

            XmlNodeList list = tbl.SelectNodes("//tr");
            for (int i = 0; i < list.Count; i++)
            {
                XmlNode node = list[i];
                node.Attributes.RemoveAll();
                XmlAttribute att1 = XmlDoc.CreateAttribute("style");
                if (i == 0 || i == 1)
                {
                    att1.InnerText = "height:10px;font-family:times new roman;font-size:6px;font-weight:bold";
                }
                else
                {
                    att1.InnerText = "height:10px;font-family:times new roman;font-size:6px;";
                }
                node.Attributes.Append(att1);

                foreach (XmlNode xn in node.ChildNodes)
                {
                    if (xn.Attributes["class"] != null)
                        xn.Attributes.Remove(xn.Attributes["class"]);

                    xn.InnerText = xn.InnerText.Trim();
                }
            }
            if (tbl.Attributes["id"].Value.ToString().ToLower().Contains("_header"))
            {
                temp = tbl;
            }
            if (!tbl.Attributes["id"].Value.ToString().ToLower().Contains("_header")
                && !tbl.Attributes["id"].Value.ToString().ToLower().Contains("_footer")
                && !tbl.Attributes["id"].Value.ToString().ToLower().Contains("pager"))
            {
                XmlNode node = tbl.SelectSingleNode("//table[@id='" + tbl.Attributes["id"].Value + "']/tbody");
                temp.ChildNodes[2].InnerXml = node.InnerXml;
            }
            if (tbl.Attributes["id"].Value.ToString().ToLower().Contains("_footer"))
            {
                XmlNode node = tbl.SelectSingleNode("//table[@id='" + tbl.Attributes["id"].Value + "']/tbody/tr");
                temp.ChildNodes[2].AppendChild(node);
            }
        }
        return temp.OuterXml;
    }
    //HTMLString = Pass your Html , fileLocation = File Store Location
    public void converttopdf(string HTMLString, string fileLocation, string Title)
    {

        string urlpath = Session["sitepath"].ToString();
        string imagepath = Session["images"].ToString().Replace(Request.ApplicationPath, "");
        
        Document document = new Document(new Rectangle(842f, 595f), 35, 50, 80, 40);
        PdfPageEvents e = new PdfPageEvents(Title, ViewState["VESSELNAME"].ToString(), urlpath + imagepath + "/esmlogo4_small.png");
        PdfWriter pw = PdfWriter.GetInstance(document, new FileStream(fileLocation, FileMode.Create));
        pw.PageEvent = e;
        document.Open();
        ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), GridViewStyle());

        for (int k = 0; k < htmlarraylist.Count; k++)
        {
            document.Add((IElement)htmlarraylist[k]);
        }

        document.Close();
    }

    public StyleSheet GridViewStyle()
    {
        StyleSheet css = new StyleSheet();
        //css.LoadStyle("DataGrid-HeaderStyle", "background-color", "#5588bb");
        //css.LoadStyle("DataGrid-HeaderStyle", "color", "#000000");
        //css.LoadStyle("DataGrid-HeaderStyle", "font-weight", "bold");
        ////css.LoadStyle("DataGrid-HeaderStyle", "position", "relative");
        ////css.LoadStyle("DataGrid-HeaderStyle", "cursor", "default");
        ////css.LoadStyle("DataGrid-HeaderStyle", "z-index", "10");
        //css.LoadStyle("datagrid_alternatingstyle", "background-color", "#f9f9fa");
        //css.LoadStyle("datagrid_selectedstyle", "background-color", "#bbddff");
        //css.LoadStyle("datagrid_selectedstyle", "font-weight", "bold");
        //css.LoadStyle("datagrid_footerstyle", "background-color", "#dfdfdf");
        //css.LoadStyle("datagrid_footerstyle", "color", "#000000");
        return css;
    }

    public void BindHeaderData()
    {
        try
        {
            DataSet ds = PhoenixAccountsOfficePortageBill.OfficePortageBillDraftViewSum(int.Parse(vslid), new Guid(pbid));
            gvHeader.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHeader_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            if (drv["FLDEARNINGS"].ToString() != drv["FLDEARNINGBUDGET"].ToString())
            {
                e.Item.Cells[3].ForeColor = System.Drawing.Color.Red;
            }
            if (drv["FLDDEDUCTION"].ToString() != drv["FLDDEDUCTIONBUDGET"].ToString())
            {
                e.Item.Cells[5].ForeColor = System.Drawing.Color.Red;
            }
            if (drv["FLDNAME"].ToString() == "")
            {
                e.Item.Cells[1].Text = "Total";
                e.Item.CssClass = "rgMasterTable rgClipCells rgClipCells";
                e.Item.Font.Bold = true;
            }
            if (drv["FLDBF"].ToString() == "" && drv["FLDNAME"].ToString() == "" && ((drv["FLDEARNINGBUDGET"].ToString() == "" && drv["FLDEARNINGS"].ToString() == "") || drv["FLDEARNINGBUDGET"].ToString() == ""))
            {
                e.Item.Cells[0].Text = "";
                e.Item.Cells.Remove(e.Item.Cells[4]);
                e.Item.Cells.Remove(e.Item.Cells[4]);
                e.Item.Cells[5].ColumnSpan = 3;
                e.Item.Cells[5].Text = "(Earnings(with Budget Code) - Deductions(with Budget Code) + B.F - Final Balance) = 	";
                txtTotal.Text = drv["FLDFINALBALANCE"].ToString();
            }
        }

    }
    protected void gvHeader_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindHeaderData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    class PdfPageEvents : PdfPageEventHelper
    {
        private string _title = string.Empty;
        private string _logo = string.Empty;
        private string _vessel = string.Empty;
        public PdfPageEvents(string Title, string vessel, string strLogoUrl)
        {
            _title = Title;
            _vessel = vessel;
            _logo = strLogoUrl;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            float[] widths = new float[] { 5f, table.TotalWidth - 5f };
            table.SetWidths(widths);

            //logo
            PdfPCell cell2 = new PdfPCell(iTextSharp.text.Image.GetInstance(new Uri(_logo)));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.Border = Rectangle.NO_BORDER;
            table.AddCell(cell2);

            //title
            cell2 = new PdfPCell(new Phrase("\n               " + _title + "\n               " + _vessel, new Font(Font.HELVETICA, 11, Font.BOLD)));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.Border = Rectangle.NO_BORDER;
            table.AddCell(cell2);
            table.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 36, writer.DirectContent);
        }
    }

}
