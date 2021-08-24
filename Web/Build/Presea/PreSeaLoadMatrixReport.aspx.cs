using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaLoadMatrixReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar Maintoolbar = new PhoenixToolbar();
                Maintoolbar.AddButton("Show Report", "REPORT");
                MenuPreSeaScoreCradSummary.AccessRights = this.ViewState;
                MenuPreSeaScoreCradSummary.MenuList = Maintoolbar.Show();

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddImageButton("../PreSea/PreSeaLoadMatrixReport.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
                MenuPreSeaGrid.AccessRights = this.ViewState;
                MenuPreSeaGrid.MenuList = toolbarsub.Show();

                BindYear();
                BindWeek(null, null);
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year) + 1; i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
    }

    protected void BindWeek(int? year, byte? month)
    {
        year = year.HasValue ? year : General.GetNullableInteger(DateTime.Today.Year.ToString());
        month = month.HasValue ? month : General.GetNullableByte(DateTime.Today.Month.ToString());

        DataTable dt = PhoenixPreSeaCommon.GetWeeksinaMonth(year, month);
        ddlWeek.Items.Clear();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlWeek.Items.Add(li);
        ddlWeek.DataSource = dt;
        ddlWeek.DataBind();
    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("REPORT"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
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

    protected void MenuPreSeaGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=PreSea.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table><tr><td colspan=\"" + (gvPreSea.Columns.Count) + "\"></td></tr></table>");
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                gvPreSea.RenderControl(htmlwriter);
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    public void BindData()
    {
        try
        {
            DataSet ds = new DataSet();
            string start = "";
            string end = "";
            if (General.GetNullableString(ddlWeek.SelectedValue) != null)
            {
                string[] s = ddlWeek.SelectedValue.Split('-');
                start = s[0];
                end = s[1];
            }

            if (ddlType.SelectedValue.Equals("0"))
                ds = PhoenixPreSeaReports.LoadMatrixActualReport(General.GetNullableInteger(ucBatch.SelectedBatch), General.GetNullableDateTime(start), General.GetNullableDateTime(end));
            else
                ds = PhoenixPreSeaReports.LoadMatrixProposedReport(General.GetNullableInteger(ucBatch.SelectedBatch), General.GetNullableDateTime(start), General.GetNullableDateTime(end));

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BoundField field1 = new BoundField();
                    field1.HeaderText = dt.Rows[i]["FLDSECTION"].ToString() + "<br/>" + dt.Rows[i]["FLDLECTURE"].ToString();
                    field1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    field1.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field1.ItemStyle.Wrap = false;
                    field1.HtmlEncode = false;
                    gvPreSea.Columns.Add(field1);

                    BoundField field2 = new BoundField();
                    field2.HeaderText = dt.Rows[i]["FLDSECTION"].ToString() + "<br/>" + dt.Rows[i]["FLDPRACTICAL"].ToString();
                    field2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    field2.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field2.ItemStyle.Wrap = false;
                    field2.HtmlEncode = false;
                    gvPreSea.Columns.Add(field2);
                }
                gvPreSea.DataSource = ds;
                gvPreSea.DataBind();
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvPreSea);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView gv = (GridView)sender;

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow && gv.DataSource.GetType().Equals(typeof(DataSet)))
            {
                DataSet ds = (DataSet)gv.DataSource;
                string facultyid = drv["FLDFACULTY"].ToString();

                DataTable header = ds.Tables[1];
                DataTable data = ds.Tables[2];
                int variant = 2;
                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] drl = data.Select("FLDFACULTY = " + facultyid + " AND FLDCLASSTYPE = '" + header.Rows[i]["FLDLECTURE"].ToString() + "' AND FLDSECTIONID = " + header.Rows[i]["FLDSECTIONID"].ToString());
                    if (drl.Length > 0)
                        e.Row.Cells[i + variant].Text = (String.IsNullOrEmpty(drl[0]["FLDLOADHOURS"].ToString()) ? "-" : drl[0]["FLDLOADHOURS"].ToString());
                    else
                        e.Row.Cells[i + variant].Text = "-";
                    variant++;
                    DataRow[] drp = data.Select("FLDFACULTY = " + facultyid + " AND FLDCLASSTYPE = '" + header.Rows[i]["FLDPRACTICAL"].ToString() + "' AND FLDSECTIONID = " + header.Rows[i]["FLDSECTIONID"].ToString());
                    if (drp.Length > 0)
                        e.Row.Cells[i + variant].Text = (String.IsNullOrEmpty(drp[0]["FLDLOADHOURS"].ToString()) ? "-" : drp[0]["FLDLOADHOURS"].ToString());
                    else
                        e.Row.Cells[i + variant].Text = "-";
                }
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

            gv.HeaderRow.Cells[0].Text = "&nbsp;";

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

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ddlType.SelectedValue) == null)
            ucError.ErrorMessage = "Report type is required.";

        if (General.GetNullableString(ddlYear.SelectedValue) == null)
            ucError.ErrorMessage = "Year is required.";

        if (General.GetNullableInteger(ddlMonth.SelectedValue) == null)
            ucError.ErrorMessage = "Month is required.";

        if (General.GetNullableString(ddlWeek.SelectedValue) == null)
            ucError.ErrorMessage = "Week is required.";

        return (!ucError.IsError);
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindWeek(General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableByte(ddlMonth.SelectedValue));
        BindData();
    }
}
