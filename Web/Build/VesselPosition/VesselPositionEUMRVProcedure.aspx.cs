using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVProcedure : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUMRVProcedure.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvProcedure')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddImageButton("../VesselPosition/VesselPositionEUMRVProcedure.aspx", "Search", "search.png", "Find");
            //toolbar.AddImageButton("../VesselPosition/VesselPositionEUMRVProcedure.aspx", "Filter", "clear-filter.png", "Clear");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                pnlSatcom.Visible = false;
              //  toolbar.AddImageLink("javascript:Openpopup('codehelp1','','VesselPositionEUMRVProcedureCopyHistory.aspx'); return false;", "History", "showlist.png", "HISTORY");
            }
            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                VesselList();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 || PhoenixSecurityContext.CurrentSecurityContext.InstallCode>0) 
                    pnlSatcom.Visible = false;
                Panel1.Visible = false;

                gvProcedure.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Location_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
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
    protected void ucVessel_Changed(object sender, EventArgs e)
    {

    }

    protected void gvProcedure_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper() == "NAV")
            {
                Guid? ProcedureID = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblProcedureId")).Text);
                string Table = ((RadLabel)e.Item.FindControl("lblProcedureCode")).Text;

                if (Table.ToUpper() == "F.2")
                {
                    Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailsF2.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "A")
                {
                    Response.Redirect("../Registers/RegistersEUMRVRevisionRecordSheet.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "B.2")
                {
                    Response.Redirect("../VesselPosition/VesselPositionEUMRVPrimaryManager.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "B.5" || Table.ToUpper() == "C.2.2" || Table.ToUpper() == "C.2.5" || Table.ToUpper() == "C.2.8" || Table.ToUpper() == "E.1" || Table.ToUpper() == "E.3" || Table.ToUpper() == "E.4" || Table.ToUpper() == "E.5" || Table.ToUpper() == "E.6")
                {
                    Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetail.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "F.1") { Response.Redirect("../Registers/RegistersEUMRVdefinitionandabbreviation.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim()); }
                if (Table.ToUpper() == "B.1") { Response.Redirect("../Registers/RegistersEUMRVShipDetails.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim()); }
                if (Table.ToUpper() == "C.2.3")
                {
                    Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC23.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "C.2.10" || Table.ToUpper() == "C.2.11" || Table.ToUpper() == "C.2.12" || Table.ToUpper() == "C.6.1" || Table.ToUpper() == "E.3" || Table.ToUpper() == "E.4" || Table.ToUpper() == "E.5" || Table.ToUpper() == "E.6"  || Table.ToUpper() == "C.5.2" || Table.ToUpper() == "C.6.2" || Table.ToUpper() == "C.4.2")
                {
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC210.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "C.3" || Table.ToUpper() == "C.4.1")
                {
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC3.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "C.5.1")
                {
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailC5.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "D.1")
                {
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailD1.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());

                }
                if (Table.ToUpper() == "D.2" || Table.ToUpper() == "D.3" || Table.ToUpper() == "D.4")
                {
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailD2.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "E.2")
                {
                        Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailE2.aspx?ProcedureId=" + ProcedureID.ToString() + "&Table=" + Table + "&lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                }
                if (Table.ToUpper() == "C.1")
                    Response.Redirect("../Registers/RegistersEUMRVExemptionArticle.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "C.2.7")
                    Response.Redirect("../Registers/RegistersEUMRVFuelMonitoring.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "C.2.6")
                    Response.Redirect("../Registers/RegistersEUMRVDeterminationDensity.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "C.2.4")
                    Response.Redirect("../Registers/RegistersEUMRVMesurementInstrument.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "C.2.1")
                    Response.Redirect("../Registers/RegistersEUMRVFuelMonitoringConsumption.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "B.3")
                    Response.Redirect("../VesselPosition/VesselPositionEUMRVEmissionSources.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim());
                if (Table.ToUpper() == "C.2.9")
                {
                    Response.Redirect("../Registers/RegistersEUMRVFuelConcumptionFreight.aspx?lanchfrom=" + Request.QueryString["Lanchfrom"].ToString().Trim() + "&ProcedureId=" + ProcedureID + "&Table=" + Table);
                }
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDPROCEDURE" };
        string[] alCaptions = { "Table", "Procedure" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigSearch(
             txtprocedurefilter.Text.Trim(), txtCode.Text.Trim(), sortexpression,
             sortdirection,
             (int)ViewState["PAGENUMBER"],
             gvProcedure.PageSize,
             ref iRowCount,
             ref iTotalPageCount,
             General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=\"CompanyProcedure.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Company Procedure</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
    protected void gvProcedure_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }

        gvProcedure.Rebind();
    }
    private void VesselList()
    {
        DataSet Ds = PhoenixRegistersVessel.ListActiveVessel(null, General.GetNullableString(""), 1);
        chkVesselList.DataSource = Ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();
    }

    protected void cmdCopy_OnClick(object sender, ImageClickEventArgs e)
    {
        try
        {
            string Vessellist = "";
            foreach (ListItem li in chkVesselList.Items)
            {
                if (li.Selected)
                {
                    if (Vessellist == "")
                        Vessellist = ",";
                    Vessellist += li.Value + ",";
                }
            }

            PhoenixVesselPositionEUMRVConfig.EUMRVProcedureCopy(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), Vessellist, PhoenixSecurityContext.CurrentSecurityContext.UserCode);

           // ucStatus.Text = "Copied Successfully";
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcedure_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvProcedure.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDPROCEDURE" };
        string[] alCaptions = { "Table", "Procedure" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigSearch(
            txtprocedurefilter.Text.Trim(), txtCode.Text.Trim(), sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvProcedure.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));

        General.SetPrintOptions("gvProcedure", "Company Procedure", alCaptions, alColumns, ds);

            gvProcedure.DataSource = ds;
        gvProcedure.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
 
}
