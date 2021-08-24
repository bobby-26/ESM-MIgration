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

public partial class VesselPositionIMODCSConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvProcedure')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            
            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

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
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
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
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("PROCEDURELIST"))
        {
            Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureList.aspx");
        }
    }

    private bool IsValidProcedure(string Procedure, string ProcedureCode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((ProcedureCode.Trim() == null) || (ProcedureCode.Trim() == ""))
            ucError.ErrorMessage = "code is required.";

        if ((Procedure.Trim() == null) || (Procedure.Trim() == ""))
            ucError.ErrorMessage = "Decription is required.";



        return (!ucError.IsError);
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

                if (Table.ToUpper() == "SHP") { Response.Redirect("../Registers/RegistersIMODCSShipParticulars.aspx"); }
                if (Table.ToUpper() == "MFC") { Response.Redirect("../VesselPosition/VesselPositionIMOMeasureFuelOilConsumption.aspx"); }
                if (Table.ToUpper() == "RFC") { Response.Redirect("../VesselPosition/VesselpositionIMODCSRevisedConsumption.aspx"); }
                if (Table.ToUpper() == "SFEC") { Response.Redirect("../VesselPosition/VesselPositionIMODCSEmissionSources.aspx"); }


                if (Table.ToUpper() == "MDT") { Response.Redirect("../VesselPosition/VesselPositionIMODCSDMSMeasure.aspx?code=MDT"); }
                if (Table.ToUpper() == "MHU") { Response.Redirect("../VesselPosition/VesselPositionIMODCSDMSMeasure.aspx?code=MHU"); }
                if (Table.ToUpper() == "RDA") { Response.Redirect("../VesselPosition/VesselPositionIMODCSDMSMeasure.aspx?code=RDA"); }
                if (Table.ToUpper() == "DQMS") { Response.Redirect("../VesselPosition/VesselPositionIMODCSDQualityData.aspx"); }
                if (Table.ToUpper() == "EMF") { Response.Redirect("../VesselPosition/VesselPositionIMODCSEmisionFactor.aspx"); }
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

    protected void gvProcedure_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvProcedure_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigUpdate(
                new Guid(((RadLabel)e.Item.FindControl("lblProcedureId")).Text),
                ((RadCheckBox)e.Item.FindControl("chkApplicableyn")).Checked == true ? 1 : 0
                );

            Rebind();

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

        string[] alColumns = { "FLDSORTORDER", "FLDNAME" };
        string[] alCaptions = { "No.", "Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixVesselPositionIMODCSConfiguration.IMODCSConfigurationSearch(
             null, null, General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), sortexpression,
             sortdirection,
             (int)ViewState["PAGENUMBER"],
             gvProcedure.PageSize,
             ref iRowCount,
             ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"IMODCSProcedure.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>IMO DCS Procedure</h3></td>");
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

        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");

        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        Rebind();
    }


    protected void cmdCopy_OnClick(object sender, ImageClickEventArgs e)
    {
        try
        {
            //string Vessellist = "";
            //foreach (ListItem li in chkVesselList.Items)
            //{
            //    if (li.Selected)
            //    {
            //        if (Vessellist == "")
            //            Vessellist = ",";
            //        Vessellist += li.Value + ",";
            //    }
            //}

            //PhoenixVesselPositionEUMRVConfig.EUMRVProcedureCopy(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), Vessellist, PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            //ucStatus.Text = "Copied Successfully";
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcedure_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvProcedure.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSORTORDER", "FLDNAME" };
        string[] alCaptions = { "No.", "Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionIMODCSConfiguration.IMODCSConfigurationSearch(
            null, null, General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
            , sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvProcedure.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvProcedure", "IMO DCS Procedure", alCaptions, alColumns, ds);

        gvProcedure.DataSource = ds;
        gvProcedure.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvProcedure.SelectedIndexes.Clear();
        gvProcedure.EditIndexes.Clear();
        gvProcedure.DataSource = null;
        Rebind();
    }
}
