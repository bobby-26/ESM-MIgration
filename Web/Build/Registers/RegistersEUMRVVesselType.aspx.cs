using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersEUMRVVesselType : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVVesselType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvEUVesselType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVVesselType.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVVesselType.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersEUVesselType.AccessRights = this.ViewState;
            MenuRegistersEUVesselType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvEUVesselType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersEUVesselType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvEUVesselType.CurrentPageIndex = 0;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtCode.Text = string.Empty;
                txtVesselType.Text = string.Empty;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool checkvalue(string name, string shortname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortname == null) || (shortname == ""))
            ucError.ErrorMessage = "Code is required.";

        if ((name == null) || (name == ""))
            ucError.ErrorMessage = "Vessel Type is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    protected void gvEUVesselType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            GridFooterItem item = (GridFooterItem)gvEUVesselType.MasterTableView.GetItems(GridItemType.Footer)[0];
            string VesselType, code;
            VesselType = (((RadTextBox)item.FindControl("txtEUVesselTypeAdd")).Text);
            code = (((RadTextBox)item.FindControl("txtEUVesselTypeCodeAdd")).Text);
            if ((checkvalue(VesselType.Trim(), code.Trim())))
            {
                PhoenixRegistersEUMRVVesselType.InsertEUVesselType((PhoenixSecurityContext.CurrentSecurityContext.UserCode),
                                                        General.GetNullableString(code.Trim()),
                                                         General.GetNullableString(VesselType.Trim())
                                                      );
                ucStatus.Text = "Information Added";
            }
            Rebind();
        }
        else
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            {
                if (checkvalue((((RadTextBox)eeditedItem.FindControl("txtEUVesselTypeEdit")).Text.Trim()), (((RadTextBox)eeditedItem.FindControl("txtEUVesselTypeCodeEdit")).Text.Trim())))
                {
                    PhoenixRegistersEUMRVVesselType.UpdateEUVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            General.GetNullableGuid(((RadLabel)eeditedItem.FindControl("lblEUVesselTypeidEdit")).Text),
                                                            General.GetNullableString(((RadTextBox)eeditedItem.FindControl("txtEUVesselTypeCodeEdit")).Text.Trim()),
                                                            General.GetNullableString(((RadTextBox)eeditedItem.FindControl("txtEUVesselTypeEdit")).Text.Trim())
                                                          );
                }
                Rebind();
            }
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            ViewState["EUVesselTypeid"] = ((RadLabel)e.Item.FindControl("lblEUVesselTypeid")).Text;
            RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
            return;
        }
        if(e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvEUVesselType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
    }

    protected void gvEUVesselType_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvEUVesselType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEUVesselType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvEUVesselType.SelectedIndexes.Clear();
        gvEUVesselType.EditIndexes.Clear();
        gvEUVesselType.DataSource = null;
        gvEUVesselType.Rebind();
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCODE", "FLDVESSELTYPE" };
        string[] alCaptions = { "Code", "Vessel Type" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixRegistersEUMRVVesselType.EUVesselTypeSearch(General.GetNullableString(txtCode.Text.Trim()), General.GetNullableString(txtVesselType.Text.Trim()),
                                                 sortexpression,
                                                 sortdirection,
                                                 int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                 gvEUVesselType.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount
                                               );

        General.SetPrintOptions("gvEUVesselType", "EU Vessel Type", alCaptions, alColumns, ds);

        gvEUVesselType.DataSource = ds;
        gvEUVesselType.VirtualItemCount = iRowCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCODE", "FLDVESSELTYPE" };
        string[] alCaptions = { "Code", "Vessel Type" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersEUMRVVesselType.EUVesselTypeSearch(General.GetNullableString(txtCode.Text.Trim()), General.GetNullableString(txtVesselType.Text.Trim()),
                                                  sortexpression,
                                                  sortdirection,
                                                  1,
                                                  iRowCount,
                                                  ref iRowCount,
                                                  ref iTotalPageCount
                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"EUVesselType.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>EU Vessel Type</h3></td>");
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

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegistersEUMRVVesselType.DeleteEUVesselType((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableGuid(ViewState["EUVesselTypeid"].ToString()));
            Rebind();

            ucStatus.Text = "Information Deleted Successfully";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
