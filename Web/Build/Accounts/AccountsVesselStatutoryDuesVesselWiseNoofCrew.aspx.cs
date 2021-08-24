using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsVesselStatutoryDuesVesselWiseNoofCrew : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Head Count", "HEADCOUNT");
            toolbarmain.AddButton("Monthly", "NOOFDAYS");
            toolbarmain.AddButton("Sign Off", "SIGNONOFF");
            toolbarmain.AddButton("Summary For Month", "GROUPBYMONTH");
            toolbarmain.AddButton("Consolidated", "CBANOOFDAYS");
            MenuStatoryDuesMain.AccessRights = this.ViewState;
            MenuStatoryDuesMain.MenuList = toolbarmain.Show();
            MenuStatoryDuesMain.SelectedMenuIndex = 4;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsVesselStatutoryDuesVesselWiseNoofCrew.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageButton("../Accounts/AccountsVesselStatutoryDuesVesselWiseNoofCrew.aspx", "Find", "search.png", "FIND");
            MenuStock.AccessRights = this.ViewState;
            MenuStock.MenuList = toolbar.Show();

            if (!IsPostBack)
            {                                
                ddlComponent.Items.Clear();
                ddlComponent.Items.Insert(0, new RadComboBoxItem("--SELECT--", ""));
            }
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStatoryDuesMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("HEADCOUNT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesHeadCount.aspx";
            }
            if (CommandName.ToUpper().Equals("NOOFDAYS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesNoofDays.aspx";
            }
            if (CommandName.ToUpper().Equals("CBANOOFDAYS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesVesselWiseNoofCrew.aspx";
            }
            if (CommandName.ToUpper().Equals("GROUPBYMONTH"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesGroupbyMonth.aspx";
            }
            if (CommandName.ToUpper().Equals("SIGNONOFF"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesSignOffFormat1.aspx";
            }
            Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString(), false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStatoryDues_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("POST"))
            {
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidCrew(txtFromDate.Text, txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
               
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                if (!IsValidCrew(txtFromDate.Text, txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
                DataSet ds = PhoenixAccountsStatutoryDuesReports.StatutoryDuesConsolidatedReportsSearch(General.GetNullableGuid(ddlComponent.SelectedValue), General.GetNullableInteger(ddlUnion.SelectedAddress)
                                                                                        , General.GetNullableDateTime(txtFromDate.Text)
                                                                                        , General.GetNullableDateTime(txtToDate.Text));
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=StatutoryDues.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table></tr>");
                stringwriter.Write("<tr>");
                stringwriter.Write("<td colspan='2' rowspan='2'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                stringwriter.Write("</tr><tr><td><h3>" + ddlComponent.SelectedItem.Text.Substring(0, (ddlComponent.SelectedItem.Text.Length - 25)) + "</h3></td></tr>");
                stringwriter.Write("</table>");
               if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int i = ds.Tables[0].Rows.Count + 1;
                    stringwriter.Write("<table  border=\"1\"><tr  align='Center'>");
                    stringwriter.Write("<td colspan=2></td><td colspan='" + i.ToString() + "'>No. of Crew </td>");
                    stringwriter.Write("<td colspan=2>Period</td><td></td></tr></table>");
                }
                System.IO.StringWriter stringwriter1 = new System.IO.StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(stringwriter1);
                gvStock.RenderBeginTag(htw);
                gvStock.HeaderRow.RenderControl(htw);
                foreach (GridViewRow row in gvStock.Rows)
                {
                    row.RenderControl(htw);
                }
                gvStock.FooterRow.RenderControl(htw);
                gvStock.RenderEndTag(htw);
                Response.Write(stringwriter + stringwriter1.ToString().Replace("table", "table border =\"1\"").Replace("td", "td valign=\"top\""));
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidCrew(string From, string To)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(From).HasValue)
        {
            ucError.ErrorMessage = "From Date is required.";
        }
        if (!General.GetNullableDateTime(To).HasValue)
        {
            ucError.ErrorMessage = "To Date is required.";
        }
        if (DateTime.TryParse(From, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(To)) > 0)
        {
            ucError.ErrorMessage = "From date later than To date.";
        }

        return (!ucError.IsError);
    }

    protected void ddlUnion_Changed(object sender, EventArgs e)
    {
        try
        {
            PopulateComponent();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void PopulateComponent()
    {
        try
        {
            
            ddlComponent.Items.Clear();
            DataTable dt = PhoenixAccountsStatutoryDuesReports.ListStatutoryDuesComponentForPeriod(General.GetNullableInteger(ddlUnion.SelectedAddress)
                                                            , General.GetNullableDateTime(txtFromDate.Text)
                                                            , General.GetNullableDateTime(txtToDate.Text)
                                                           );
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    RadComboBoxItem item = new RadComboBoxItem(dr["FLDCOMPONENTNAME"].ToString(), dr["FLDCOMPONENTID"].ToString());
                    item.Attributes["OptionGroup"] = dr["FLDUNION"].ToString();
                    ddlComponent.Items.Add(item);
                }
                ddlComponent.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            }
            else
            {
                ddlComponent.Items.Insert(0, new RadComboBoxItem("-- No Component Found --", ""));
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
            DataSet ds = PhoenixAccountsStatutoryDuesReports.StatutoryDuesConsolidatedReportsSearch(General.GetNullableGuid(ddlComponent.SelectedValue), General.GetNullableInteger(ddlUnion.SelectedAddress)
                                                                                          , General.GetNullableDateTime(txtFromDate.Text)
                                                                                          , General.GetNullableDateTime(txtToDate.Text));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["RECORDCOUNT"] = "1";
                DataTable dt = GetTable(ds);
                gvStock.DataSource = dt;
                gvStock.DataBind();

                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell;

                HeaderCell = new TableCell();
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "No. of Crew";
                HeaderCell.ColumnSpan = ds.Tables[0].Rows.Count + 1;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);
                HeaderCell = new TableCell();
                HeaderCell.Text = "Period";
                HeaderCell.ColumnSpan = 2;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);
                HeaderCell = new TableCell();
                // HeaderCell.Text = "Period";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);

                gvStock.Controls[0].Controls.AddAt(0, HeaderGridRow);
                GridViewRow row1 = ((GridViewRow)gvStock.Controls[0].Controls[0]);
                row1.Attributes.Add("style", "position:static");
            }
            else
            {
                ViewState["RECORDCOUNT"] = "0";
                ShowNoRecordsFound(ds.Tables[0], gvStock);
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
   

    private DataTable GetTable(DataSet ds)
    {
        DataTable table = new DataTable();
        table.Columns.Add("Sr.No", typeof(string)); ;
        table.Columns.Add("Vessel Name", typeof(string)); ;
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {

            table.Columns.Add(DateTime.Parse("01/" + ds.Tables[0].Rows[i]["FLDMONTH"].ToString() + "/" + ds.Tables[0].Rows[i]["FLDYEAR"].ToString()).ToString("MMM") + "-" + ds.Tables[0].Rows[i]["FLDYEAR"].ToString(), typeof(string));
        }
        table.Columns.Add("Total", typeof(string));
        table.Columns.Add("From", typeof(string));
        table.Columns.Add("To", typeof(string));
        table.Columns.Add("Amount", typeof(string));
        DataTable dtUniqRecords = new DataTable();
        dtUniqRecords = ds.Tables[1].DefaultView.ToTable(true, "FLDVESSELNAME");
        int rowno = 0;
        foreach (DataRow dr in dtUniqRecords.Rows)
        {
            rowno += 1;
            DataRow[] result = ds.Tables[1].Select("FLDVESSELNAME ='" + dr["FLDVESSELNAME"] + "'");
            DataRow rows = table.NewRow();
            rows["Sr.No"] = rowno;
            rows["Vessel Name"] = dr["FLDVESSELNAME"];
            int TotalCrew = 0;
            Decimal totalamount = 0;
            foreach (DataRow row in result)
            {
                rows[DateTime.Parse("01/" + row["FLDMONTH"].ToString() + "/" + row["FLDYEAR"].ToString()).ToString("MMM") + "-" + row["FLDYEAR"].ToString()] = row["FLDCOUNT"];
                TotalCrew += int.Parse(row["FLDCOUNT"].ToString());
                totalamount += decimal.Parse(row["FLDAMOUNT"].ToString());
            }
            rows["Total"] = TotalCrew.ToString();
            rows["From"] = txtFromDate.Text;
            rows["To"] = txtToDate.Text; rows["Amount"] = totalamount.ToString();
            table.Rows.Add(rows);
        }
        return table;
    }

    protected void gvStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

}
