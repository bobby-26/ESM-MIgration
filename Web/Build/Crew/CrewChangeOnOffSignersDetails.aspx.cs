using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
public partial class CrewChangeOnOffSignersDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddImageButton("../Crew/CrewChangeOnOffSignersDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrewChangeChange')", "Print Grid", "icon_print.png", "PRINT");
            MenuCrewChange.AccessRights = this.ViewState;
            MenuCrewChange.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";
                ViewState["PORTID"] = "";
                ViewState["DATEOFCREWCHANGE"] = "";

                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                if (Request.QueryString["portid"] != null)
                    ViewState["PORTID"] = Request.QueryString["portid"].ToString();

                if (Request.QueryString["dateofcrewchange"] != null)
                    ViewState["DATEOFCREWCHANGE"] = Request.QueryString["dateofcrewchange"].ToString();

            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewChange_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDONSIGNERFILENO", "FLDONSIGNERNAME", "FLDONSIGNERRANK", "FLDDATEOFCREWCHANGE", "FLDSIGNONREASON", "FLDSIGNONREMARKS", "FLDOFFSIGNERFILENO", "FLDOFFSIGNERNAME", "FLDOFFSIGNERRANK", "FLDDATEOFCREWCHANGE", "FLDSIGNOFFREASON", "FLDSIGNOFFREMARKS" };
        string[] alCaptions = { "File No", "Onsigner Name", "Onsigner Rank", "SignOnDate", "Signon Reason", "Signon Remarks", "Offsigner Fileno", "Offsigner Name", "Offsigner Rank", "SignOff Date", "Signoff Reason", "Signoff Remarks" };

        ds = PhoenixCrewReportMonthlyCrewChange.CrewChangeOnOffSignersDetail(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , General.GetNullableDateTime(ViewState["DATEOFCREWCHANGE"].ToString())
                , General.GetNullableInteger(ViewState["PORTID"].ToString()));

        General.SetPrintOptions("gvCrewChangeChange", "Crew Change Details", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            txtPort.Text = ds.Tables[0].Rows[0]["FLDCREWCHANGEPORT"].ToString();

            gvCrewChange.DataSource = ds;
            gvCrewChange.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCrewChange);

        }
    }

    protected void ShowExcel()
    {
        try
        {
            DataSet ds = new DataSet();
            string[] alColumns = { "FLDONSIGNERFILENO", "FLDONSIGNERNAME", "FLDONSIGNERRANK", "FLDDATEOFCREWCHANGE", "FLDSIGNONREASON", "FLDSIGNONREMARKS", "FLDOFFSIGNERNAME", "FLDOFFSIGNERFILENO", "FLDOFFSIGNERRANK", "FLDDATEOFCREWCHANGE", "FLDSIGNOFFREASON", "FLDSIGNOFFREMARKS" };
            string[] alCaptions = { "File No", "OnSigner Name", "Rank", "SignOnDate", "Signon Reason", "Signon Remarks", "File No", "OffSigner Name", "Rank", "SignOff Date", "SignOff Reason", "SignOff Remarks" };

            ds = PhoenixCrewReportMonthlyCrewChange.CrewChangeOnOffSignersDetail(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                    , General.GetNullableDateTime(ViewState["DATEOFCREWCHANGE"].ToString())
                    , General.GetNullableInteger(ViewState["PORTID"].ToString()));

            Response.AddHeader("Content-Disposition", "attachment; filename= CrewChangeDetails.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Crew Change Details </center></h3></td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
}
