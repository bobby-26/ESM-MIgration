using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Drawing;

public partial class RegistersDMROilType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMROilType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOilType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersOilType.AccessRights = this.ViewState;
            MenuRegistersOilType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //BindData();
                gvOilType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
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
                gvOilType.Rebind();

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

    protected void gvOilType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtOilTypeNameAdd")).Text)
                    , (((UserControlMaskNumber)e.Item.FindControl("txtOilTypeSortOrderAdd")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtOilShortNameAdd")).Text
                    , ((RadComboBox)e.Item.FindControl("ucUnitAdd")).SelectedValue
                    , ((UserControlMaskNumber)e.Item.FindControl("ucConvFactorm3Add")).Text
                    , ((UserControlMaskNumber)e.Item.FindControl("ucConvFactorBBLAdd")).Text
                    , ((UserControlMaskNumber)e.Item.FindControl("ucConvFactorLTRAdd")).Text
                    , ((RadCheckBox)e.Item.FindControl("chkRequiredAdd")).Checked == true ? "1" : "0"))
                    return;
                string color = System.Drawing.ColorTranslator.ToHtml(((RadColorPicker)e.Item.FindControl("txtColorAdd")).SelectedColor); 
                //if (color != "")
                //    color = "#" + color;
                PhoenixRegistersDMROilType.InsertOilType(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ((RadTextBox)e.Item.FindControl("txtOilTypeNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtOilShortNameAdd")).Text,
                    General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtOilTypeSortOrderAdd")).Text),
                    0, 0, null, null, null, null,
                    int.Parse(((RadComboBox)e.Item.FindControl("ucUnitAdd")).SelectedValue),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucConvFactorm3Add")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucConvFactorBBLAdd")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucConvFactorLTRAdd")).Text),
                    General.GetNullableGuid(((UserControlProductType)e.Item.FindControl("ucProductTypeAdd")).SelectedProductType),
                    General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkRequiredAdd")).Checked == true ? "1" : "0"),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucSpecificGravityAdd")).Text),
                    General.GetNullableString(color));

                BindData();
                gvOilType.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDMROilType.DeleteOilType((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeCode")).Text));
                BindData();
                gvOilType.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtOilTypeNameEdit")).Text)
                    , (((UserControlMaskNumber)e.Item.FindControl("txtOilTypeSortOrderEdit")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtOilShortNameEdit")).Text
                    , ((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit
                    , ((UserControlMaskNumber)e.Item.FindControl("ucConvFactorm3Edit")).Text
                    , ((UserControlMaskNumber)e.Item.FindControl("ucConvFactorBBLEdit")).Text
                    , ((UserControlMaskNumber)e.Item.FindControl("ucConvFactorLTREdit")).Text
                    , ((RadCheckBox)e.Item.FindControl("chkRequiredEdit")).Checked == true ? "1" : "0"))
                    return;
                string color = System.Drawing.ColorTranslator.ToHtml(((RadColorPicker)e.Item.FindControl("txtColorEdit")).SelectedColor);
              
                //if (!color.Contains("#"))
                //{
                //    if (color != "")
                //        color = "#" + color;
                //}
                PhoenixRegistersDMROilType.UpdateOilType(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeCodeEdit")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtOilTypeNameEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtOilShortNameEdit")).Text,
                    General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtOilTypeSortOrderEdit")).Text),
                    0,
                    0, null, null, null, null,
                    int.Parse(((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucConvFactorm3Edit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucConvFactorBBLEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucConvFactorLTREdit")).Text),
                    General.GetNullableGuid(((UserControlProductType)e.Item.FindControl("ucProductTypeEdit")).SelectedProductType),
                    General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkRequiredEdit")).Checked == true ? "1" : "0"),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucSpecificGravityEdit")).Text),
                    General.GetNullableString(color));
                BindData();
                gvOilType.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool checkvalue(string type, string order, string shortname, string itemunit, string covfactorltr, string covfactorm3, string convfactorbbl, string convrequired)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortname == null) || (shortname == ""))
            ucError.ErrorMessage = "Oil Shortname is required.";

        if ((type == null) || (type == ""))
            ucError.ErrorMessage = "Oil Type is required.";

        if ((order == null) || (order == ""))
            ucError.ErrorMessage = "Sort order is required.";

        if (General.GetNullableInteger(itemunit) == null)
            ucError.ErrorMessage = "Unit is required.";

        if (convrequired == "1")
        {
            if (General.GetNullableDecimal(covfactorltr) == null)
                ucError.ErrorMessage = "Conversion Factor for LTR is required.";
            if (General.GetNullableDecimal(covfactorm3) == null)
                ucError.ErrorMessage = "Conversion Factor for M3 is required.";
            if (General.GetNullableDecimal(convfactorbbl) == null)
                ucError.ErrorMessage = "Conversion Factor for BBL is required.";
        }
        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTNAME", "FLDOILTYPENAME", "FLDUNITNAME", "FLDPRODUCTTYPENAME", "FLDSPECIFICGRAVITY", "FLDCONVERSIONREQUIRED", "FLDCONVERSIONFACTORLTR", "FLDCONVERSIONFACTORM3", "FLDCONVERSIONFACTORBBL", "FLDCOLOR", "FLDSORTORDER" };
        string[] alCaptions = { "Short Code", "Description", "Unit", "Product Type", "Specific Gravity", "Conversion Required", "Conv Factor LTR", "Conv Factor M3", "Conv Factor BBL", "Color", "Sort Order" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersDMROilType.OilTypeSearch("", "", 0,
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            gvOilType.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvOilType", "Meteorology Data", alCaptions, alColumns, ds);
        gvOilType.DataSource = ds;
        gvOilType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSHORTNAME", "FLDOILTYPENAME", "FLDUNITNAME", "FLDPRODUCTTYPENAME", "FLDSPECIFICGRAVITY", "FLDCONVERSIONREQUIRED", "FLDCONVERSIONFACTORLTR", "FLDCONVERSIONFACTORM3", "FLDCONVERSIONFACTORBBL", "FLDCOLOR", "FLDSORTORDER" };
        string[] alCaptions = { "Short Code", "Description", "Unit", "Product Type", "Specific Gravity", "Conversion Required", "Conv Factor LTR", "Conv Factor M3", "Conv Factor BBL", "Color", "Sort Order" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersDMROilType.OilTypeSearch("", "", 0,
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Product.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Product</h3></td>");
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

    protected void gvOilType_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            //if (e.Item is GridDataItem)
            //{
            //    DataRowView drv = (DataRowView)e.Item.DataItem;
            //    RadComboBox ddlValueTypeEdit = (RadComboBox)e.Item.FindControl("ddlValueTypeEdit");
            //    if (ddlValueTypeEdit != null)
            //        ddlValueTypeEdit.SelectedValue = drv["FLDVALUETYPE"].ToString();
            //}
            //if (e.Item is GridFooterItem)
            //{
            //    LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            //    if (db != null)
            //    {
            //        if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
            //            db.Visible = false;
            //    }
            //}

            if (e.Item is GridDataItem)
            {

                DataRowView drv = (DataRowView)e.Item.DataItem;
                CheckBox cb = (CheckBox)e.Item.FindControl("chkShowYN");
                if (cb != null)
                    cb.Checked = drv["FLDSHOWYESNO"].ToString().Equals("1") ? true : false;

                UserControlUnit ucunit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
                if (ucunit != null)
                    ucunit.SelectedUnit = drv["FLDUNIT"].ToString();
                UserControlProductType ucProductTypeEdit = (UserControlProductType)e.Item.FindControl("ucProductTypeEdit");
                if (ucProductTypeEdit != null)
                    ucProductTypeEdit.SelectedProductType = drv["FLDPRODUCTTYPEID"].ToString();


            }

            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                RadComboBox ucUnitAdd = (RadComboBox)e.Item.FindControl("ucUnitAdd");
                ucUnitAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvOilType_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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

    //protected void ColorPicker1_Click(object sender, System.EventArgs e)
    //{
    //    gvOilType.lblcolor.Text = ColorTranslator.ToHtml(txtColorEdit_ColorPickerExtender.SelectedColor);
    //}

    //protected void ColorPicker2_Click(object sender, System.EventArgs e)
    //{

    //    txtColorEdit.Text = ColorTranslator.ToHtml(txtColorEdit_ColorPickerExtender.SelectedColor);
    //}
}


