using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselpositionIMODCSRevisedConsumption : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarMAIN = new PhoenixToolbar();
            toolbarMAIN.AddButton("Back", "BACK",ToolBarDirection.Right);
            MenuFuelTypeMain.AccessRights = this.ViewState;
            MenuFuelTypeMain.MenuList = toolbarMAIN.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselpositionIMODCSRevisedConsumption.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOilType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersOilType.AccessRights = this.ViewState;
            MenuRegistersOilType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvOilType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersOilType_TabStripCommand(object sender, EventArgs e)
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

    protected void gvOilType_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtoilconsumersAdd")).Text.ToString()), (((UserControlDate)e.Item.FindControl("txtDateofrevisionAdd")).Text)))
                    return;

                PhoenixVesselPositionIMODCSFuelConsumption.InsertIMODCSFuelOilrevisonconsumption(
                    int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                    ((RadTextBox)e.Item.FindControl("txtoilconsumersAdd")).Text,
                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDateofrevisionAdd")).Text));

                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.DeleteIMODCSFuelOilrevisonconsumption((General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbloilconsumerid")).Text)));
                Rebind();
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

    private bool checkvalue(string provision, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((date == null) || (date == ""))
            ucError.ErrorMessage = "Date of revision is required.";

        if ((provision == null) || (provision == ""))
            ucError.ErrorMessage = "Revised Provision is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }
    protected void gvOilType_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);



                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvOilType_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!checkvalue((((RadTextBox)e.Item.FindControl("txtoilconsumersEdit")).Text), (((UserControlDate)e.Item.FindControl("txtDateofrevisionEdit")).Text)))
                return;

            PhoenixVesselPositionIMODCSFuelConsumption.UpdateIMODCSFuelOilrevisonconsumption(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbloilconsumeridedit")).Text),
                General.GetNullableString(((RadTextBox)e.Item.FindControl("txtoilconsumersEdit")).Text),
                General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDateofrevisionEdit")).Text));

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
        DataSet ds = new DataSet();


        string[] alColumns = { "FLDREVISIONDATE", "FLDREVISIONPROVISION" };
        string[] alCaptions = { "Date of revision", "Revised Provision" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFuelOilrevisonconsumptionSearch(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            gvOilType.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"RevisionofFuelOilConsumption.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Record of revision of Fuel Oil Consumption Data Collection Plan</h3></td>");
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

    protected void MenuFuelTypeMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../VesselPosition/VesselPositionIMODCSConfiguration.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOilType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOilType.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREVISIONDATE", "FLDREVISIONPROVISION" };
        string[] alCaptions = { "Date of revision", "Revised Provision" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFuelOilrevisonconsumptionSearch(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            gvOilType.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvOilType", "Record of revision of Fuel Oil Consumption Data Collection Plan", alCaptions, alColumns, ds);

        gvOilType.DataSource = ds;
        gvOilType.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvOilType.SelectedIndexes.Clear();
        gvOilType.EditIndexes.Clear();
        gvOilType.DataSource = null;
        gvOilType.Rebind();
    }
}
