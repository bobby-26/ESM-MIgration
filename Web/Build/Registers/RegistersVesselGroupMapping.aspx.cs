using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVesselGroupMapping : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvVessel.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation(r.UniqueID + "$lnkDoubleClick");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselGroupMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVessel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            
            MenuVesselMapping.AccessRights = this.ViewState;
            MenuVesselMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvVessel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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

        string[] alColumns = {  "FLDVESSELNAME", "FLDHARDNAME","FLDLOCATIONNAME" };
        string[] alCaptions = { "Vessel", "Group","Location" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        
        DataSet ds = PhoenixRegistersVesselGroupMapping.VesselGroupMappingSearch(
            General.GetNullableInteger(UcVessel.SelectedVessel),
            sortexpression, sortdirection, 1,
                iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselGroupMapping.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Group Mapping</h3></td>");
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

    protected void MenuVesselMapping_TabStripCommand(object sender, EventArgs e)
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
        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        UcVessel.SelectedVessel = "";
        Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixRegistersVesselGroupMapping.VesselGroupMappingSearch(
               General.GetNullableInteger(UcVessel.SelectedVessel),
               sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvVessel.PageSize, ref iRowCount, ref iTotalPageCount);

            string[] alColumns = { "FLDVESSELNAME", "FLDHARDNAME", "FLDLOCATIONNAME" };
            string[] alCaptions = { "Vessel", "Group", "Location" };

            General.SetPrintOptions("gvVessel", "Vessel Group Mapping", alCaptions, alColumns, ds);


            gvVessel.DataSource = ds;
            gvVessel.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidConfig(string vessel,string group,string Location)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(group) == null)
            ucError.ErrorMessage = "Group is required.";

        if (General.GetNullableInteger(Location) == null)
            ucError.ErrorMessage = "Location is required.";

        return (!ucError.IsError);
    }
    protected void gvVessel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVessel.CurrentPageIndex + 1;
        BindData();
    }
    protected void Rebind()
    {
        gvVessel.SelectedIndexes.Clear();
        gvVessel.EditIndexes.Clear();
        gvVessel.DataSource = null;
        gvVessel.Rebind();
    }

    protected void gvVessel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;


            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");

            if (del != null)
                if (!SessionUtil.CanAccess(this.ViewState, del.CommandName))
                    del.Visible = false;

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");

            if (edit != null)
                if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName))
                    edit.Visible = false;
            
            RadComboBox ddlLocationEdit = (RadComboBox)e.Item.FindControl("ddlLocationEdit");
            if (ddlLocationEdit != null)
            {
                ddlLocationEdit.DataSource = PhoenixRegistersVesselGroupMapping.LocationList();
                ddlLocationEdit.DataBind();
                ddlLocationEdit.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlLocationEdit.SelectedValue = drv["FLDLOCATIONID"].ToString();
            }


            UserControlHard ucGroupEidt = (UserControlHard)e.Item.FindControl("ucGroupEdit");
            if (ucGroupEidt != null)
            {
                ucGroupEidt.HardTypeCode = "246";
                ucGroupEidt.bind();
                ucGroupEidt.SelectedHard = drv["FLDGROUPID"].ToString();
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            UserControlHard ucGroupAdd = (UserControlHard)e.Item.FindControl("ucGroupAdd");
            if (ucGroupAdd != null)
            {
                ucGroupAdd.HardTypeCode = "246";
                ucGroupAdd.bind();
            }
            RadComboBox ddlLocationAdd = (RadComboBox)e.Item.FindControl("ddlLocationAdd");
            if (ddlLocationAdd != null)
            {
                ddlLocationAdd.DataSource = PhoenixRegistersVesselGroupMapping.LocationList();
                ddlLocationAdd.DataBind();
                ddlLocationAdd.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            }
        }
    }

    protected void gvVessel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footerItem = (GridFooterItem)gvVessel.MasterTableView.GetItems(GridItemType.Footer)[0];
                if (!IsValidConfig(
                                    ((UserControlVessel)footerItem.FindControl("ucVesselAdd")).SelectedVessel.ToString()
                                    , ((UserControlHard)footerItem.FindControl("ucGroupAdd")).SelectedHard.ToString()
                                    , ((RadComboBox)footerItem.FindControl("ddlLocationAdd")).SelectedValue.ToString()
                                ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVesselGroupMapping.VesselGroupMappingInsert(
                           General.GetNullableInteger(((UserControlVessel)footerItem.FindControl("ucVesselAdd")).SelectedVessel.ToString())
                                    , General.GetNullableInteger(((UserControlHard)footerItem.FindControl("ucGroupAdd")).SelectedHard.ToString())
                                    , General.GetNullableInteger(((RadComboBox)footerItem.FindControl("ddlLocationAdd")).SelectedValue.ToString())
                    );
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["VesselId"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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


    protected void gvVessel_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidConfig(
                                ((RadLabel)e.Item.FindControl("lblVesselId")).Text
                                , ((UserControlHard)e.Item.FindControl("ucGroupEdit")).SelectedHard
                                , ((RadComboBox)e.Item.FindControl("ddlLocationEdit")).SelectedValue
                            ))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersVesselGroupMapping.VesselGroupMappingInsert(
                        General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblVesselId")).Text)
                                , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucGroupEdit")).SelectedHard)
                                , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlLocationEdit")).SelectedValue
                ));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVessel_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegistersVesselGroupMapping.VesselGroupMappingDelete(
                        General.GetNullableInteger(ViewState["VesselId"].ToString())
                                                );
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }
}
