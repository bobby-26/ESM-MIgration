using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersOilType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersOilType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOilType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersOilType.AccessRights = this.ViewState;
            MenuRegistersOilType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] =null;
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
    protected void Rebind()
    {
        gvOilType.SelectedIndexes.Clear();
        gvOilType.EditIndexes.Clear();
        gvOilType.DataSource = null;
        gvOilType.Rebind();
    }
    protected void gvOilType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtOilTypeNameAdd")).Text), (((RadTextBox)e.Item.FindControl("txtOilTypeSortOrderAdd")).Text), ((RadTextBox)e.Item.FindControl("txtOilShortNameAdd")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersOilType.InsertOilType(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ((RadTextBox)e.Item.FindControl("txtOilTypeNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtOilShortNameAdd")).Text,
                    General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtOilTypeSortOrderAdd")).Text),
                    1,
                    ((RadCheckBox)e.Item.FindControl("chkFuelOilAdd")).Checked.Equals(true)? 1 : 0,
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtReferenceAdd")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtCarbonContentAdd")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtCFAdd")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtNSAdd")).Text),
                    General.GetNullableGuid(((RadDropDownList)e.Item.FindControl("ddlFuelCategoryAdd")).SelectedValue)
                    );

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersOilType.DeleteOilType((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeCode")).Text));
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool checkvalue(string type,string order,string shortname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortname == null) || (shortname == ""))
            ucError.ErrorMessage = "Oil Short Name is required.";

        if ((type == null) || (type == ""))
            ucError.ErrorMessage = "Oil Type is required.";

        if ((order == null) || (order == ""))
            ucError.ErrorMessage = "Sort Order is required.";              

        if(ucError.ErrorMessage!="")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTNAME", "FLDOILTYPENAME", "FLDSORTORDER", "FLDFUELOILYESNO" };
        string[] alCaptions = { "Oil Short Name", "Oil Type", "Sort Order", "Fuel Oil Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersOilType.OilTypeSearch(
            "", "", 1,
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            gvOilType.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvOilType", "Oil Type Register", alCaptions, alColumns, ds);

        gvOilType.DataSource = ds;
        gvOilType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvOilType_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            if (e.Item is GridEditableItem)
            {
                RadLabel lb = (RadLabel)e.Item.FindControl("lblShowYN");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (lb != null)
                    lb.Text = drv["FLDSHOWYESNO"].ToString().Equals("1") ? "Yes" : "No";

                //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                //{
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                //}
                //if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                //{
                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkShowYN");                    
                    if (cb != null)
                        cb.Checked = drv["FLDSHOWYESNO"].ToString().Equals("1") ? true : false;
                //}

                RadDropDownList ddlFuelCategory = (RadDropDownList)e.Item.FindControl("ddlFuelCategory");
                if(ddlFuelCategory!=null)
                {
                    ddlFuelCategory.DataSource = PhoenixRegistersEUMRVFuelCategories.EUMRVFuelCategories();
                    ddlFuelCategory.DataTextField = "FLDCODE";
                    ddlFuelCategory.DataValueField = "FLDID";
                    ddlFuelCategory.DataBind();

                    ddlFuelCategory.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));

                    if (General.GetNullableGuid(drv["FLDFUELCATEGORYID"].ToString()) != null)
                        ddlFuelCategory.SelectedValue = drv["FLDFUELCATEGORYID"].ToString();

                    if (drv["FLDFUELOILYESNO"].ToString().ToUpper().Equals("NO"))
                        ddlFuelCategory.Enabled = false;
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

                RadDropDownList ddlFuelCategoryAdd = (RadDropDownList)e.Item.FindControl("ddlFuelCategoryAdd");
                if (ddlFuelCategoryAdd != null)
                {
                    ddlFuelCategoryAdd.DataSource = PhoenixRegistersEUMRVFuelCategories.EUMRVFuelCategories();
                    ddlFuelCategoryAdd.DataTextField = "FLDCODE";
                    ddlFuelCategoryAdd.DataValueField = "FLDID";
                    ddlFuelCategoryAdd.DataBind();

                    ddlFuelCategoryAdd.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSHORTNAME", "FLDOILTYPENAME", "FLDSORTORDER", "FLDFUELOILYESNO" };
        string[] alCaptions = { "Oil Short Name", "Oil Type", "Sort Order", "Fuel Oil Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersOilType.OilTypeSearch("", "", 1,
            sortexpression, sortdirection, 1,
            iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"OilType.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Oil Type Register</h3></td>");
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

    protected void gvOilType_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOilType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOilType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOilType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (!checkvalue((((RadTextBox)e.Item.FindControl("txtOilTypeNameEdit")).Text), (((RadTextBox)e.Item.FindControl("txtOilTypeSortOrderEdit")).Text), ((RadTextBox)e.Item.FindControl("txtOilShortNameEdit")).Text))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersOilType.UpdateOilType(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeCodeEdit")).Text),
                General.GetNullableString(((RadTextBox)e.Item.FindControl("txtOilTypeNameEdit")).Text),
                ((RadTextBox)e.Item.FindControl("txtOilShortNameEdit")).Text,
                General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtOilTypeSortOrderEdit")).Text),
                1,
                ((RadCheckBox)e.Item.FindControl("chkFuelOilEdit")).Checked == true ? 1 : 0,
                General.GetNullableString(((RadTextBox)e.Item.FindControl("txtReferenceEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtCarbonContentEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtCFEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtNSEdit")).Text),
                General.GetNullableGuid(((RadDropDownList)e.Item.FindControl("ddlFuelCategory")).SelectedValue)
                );
            Rebind();
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
