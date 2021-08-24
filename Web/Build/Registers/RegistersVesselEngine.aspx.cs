using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class RegistersVesselEngine : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselEngine.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselEngine.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuVesselEngine.AccessRights = this.ViewState;
            MenuVesselEngine.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselEngine_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                RadGrid1.CurrentPageIndex = 0;
                RadGrid1.Rebind();
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDENGINENAME" };
        string[] alCaptions = { "Engine Type" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersVesselEngine.VesselEngineSearch(txtSearch.Text, null, sortexpression, sortdirection,
            RadGrid1.CurrentPageIndex + 1,
            RadGrid1.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselEngine.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Engine</h3></td>");
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

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];

                if (!IsValidVesselEngine(((RadTextBox)item.FindControl("txtEngineNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertVesselEngine(((RadTextBox)item.FindControl("txtEngineNameAdd")).Text);

                ucStatus.Text = "Information Added";
                RadGrid1.Rebind();
            }
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            if (e.CommandName == "InitInsert")
            {
                RadGrid1.EditIndexes.Clear();
            }
            if (e.CommandName == RadGrid.EditCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string EngineId = ((RadLabel)e.Item.FindControl("lblEngineIDEdit")).Text;
                if (EngineId == string.Empty || EngineId == "")
                {
                    //if (!IsValidVesselEngine(((RadTextBox)eeditedItem.FindControl("txtEngineNameEdit")).Text))
                    //{
                    //    ucError.Visible = true;
                    //    return;
                    //}
                    //InsertVesselEngine(((RadTextBox)eeditedItem.FindControl("txtEngineNameEdit")).Text);

                    //ucStatus.Text = "Information Added";
                    //RadGrid1.Rebind();
                }
                else
                {
                    string engineid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDENGINEID"].ToString();

                    if (!IsValidVesselEngine(((RadTextBox)eeditedItem.FindControl("txtEngineNameEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateVesselEngine(
                        Int32.Parse(engineid),
                        ((RadTextBox)eeditedItem.FindControl("txtEngineNameEdit")).Text);

                    ucStatus.Text = "Vessel Engine information updated";
                    RadGrid1.Rebind();
                }
            }
            if(e.CommandName == "Page")
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
    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDENGINENAME" };
            string[] alCaptions = { "Engine Type" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixRegistersVesselEngine.VesselEngineSearch(txtSearch.Text, null, sortexpression, sortdirection,
                RadGrid1.CurrentPageIndex + 1,
                RadGrid1.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("RadGrid1", "Vessel Engine", alCaptions, alColumns, ds);

            RadGrid1.DataSource = ds;
            RadGrid1.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            ViewState["ENGINEID"] = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDENGINEID"].ToString();
            RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
            return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

        LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
    }
    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {

    }
    protected void RadGrid1_SortCommand(object sender, GridSortCommandEventArgs e)
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
    }
    private void InsertVesselEngine(string Enginename)
    {
        PhoenixRegistersVesselEngine.InsertVesselEngine(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Enginename);
    }

    private void UpdateVesselEngine(int Engineid, string Enginename)
    {
        PhoenixRegistersVesselEngine.UpdateVesselEngine(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Engineid, Enginename);
    }
    private bool IsValidVesselEngine(string Enginename)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Enginename.Trim().Equals(""))
            ucError.ErrorMessage = "Engine Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteVesselEngine(int VesselEnginecode)
    {
        PhoenixRegistersVesselEngine.DeleteVesselEngine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselEnginecode);
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteVesselEngine(Int32.Parse(ViewState["ENGINEID"].ToString()));
            RadGrid1.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
