using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDMRMeteorology : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMRMeteorology.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDMRMeteorology')", "Print Grid","<i class=\"fas fa-print\"></i>", "PRINT");
            MenuDMRMeteorology.AccessRights = this.ViewState;
            MenuDMRMeteorology.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                // BindData();
                gvDMRMeteorology.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDMRMeteorology_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                gvDMRMeteorology.Rebind();

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


    private bool checkvalue(string shortName, string meteorologyName, string sortOrder, string valuetype, string minvalue, string maxvalue)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortName == null) || (shortName == ""))
            ucError.ErrorMessage = "Short name is required.";

        if ((meteorologyName == null) || (meteorologyName == ""))
            ucError.ErrorMessage = "Meteorology name is required.";

        if (General.GetNullableInteger(valuetype) == null)
        {
            ucError.ErrorMessage = "Value Type is Required";
        }
        if ((sortOrder == null) || (sortOrder == ""))
            ucError.ErrorMessage = "Sort order is required.";

        if (General.GetNullableInteger(valuetype) != null)
        {
            if ((General.GetNullableInteger(valuetype) == 2) && ((minvalue != "" || maxvalue != "")))
            {
                ucError.ErrorMessage = "Range Cannot be set for the Value Type Condition";
            }
        }
        if (General.GetNullableInteger(valuetype) != null)
        {
            if ((General.GetNullableInteger(valuetype) == 3) && ((minvalue != "" || maxvalue != "")))
            {
                ucError.ErrorMessage = "Range Cannot be set for the Value Type Direction";
            }
        }

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTNAME", "FLDMETEOROLOGYNAME", "FLDMINVALUE", "FLDMAXVALUE", "FLDSORTORDER", "FLDVALUETYPENAME" };
        string[] alCaptions = { "Short Code", "Description", "Min Value", "Max Value", "Sort Order", "Value Type" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersDMRMeteorology.DMRMeteorologySearch("",
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
           gvDMRMeteorology.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvDMRMeteorology", "Meteorology Data", alCaptions, alColumns, ds);
        gvDMRMeteorology.DataSource = ds;
        gvDMRMeteorology.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }







    //protected void gvDMRMeteorology_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSHORTNAME", "FLDMETEOROLOGYNAME", "FLDMINVALUE", "FLDMAXVALUE", "FLDSORTORDER", "FLDVALUETYPENAME" };
        string[] alCaptions = { "Short Code", "Description", "Min Value", "Max Value", "Sort Order", "Value Type" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersDMRMeteorology.DMRMeteorologySearch("",
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"MeteorologyData.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Meteorology Data</h3></td>");
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

    protected void gvDMRMeteorology_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDMRMeteorology.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDMRMeteorology_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {

                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadComboBox ddlValueTypeEdit = (RadComboBox)e.Item.FindControl("ddlValueTypeEdit");
                if (ddlValueTypeEdit != null)
                    ddlValueTypeEdit.SelectedValue = drv["FLDVALUETYPE"].ToString();
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
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDMRMeteorology_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text),
                    (((RadTextBox)e.Item.FindControl("txtMeteorologyNameAdd")).Text),
                    ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text,
                    ((RadComboBox)e.Item.FindControl("ddlValueTypeAdd")).SelectedValue,
                    ((UserControlMaskNumber)e.Item.FindControl("txtMinValueAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtMaxValueAdd")).Text))
                    return;

                PhoenixRegistersDMRMeteorology.DMRMeteorologyInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ((RadTextBox)e.Item.FindControl("txtMeteorologyNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text,
                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text),
                    ((UserControlMaskNumber)e.Item.FindControl("txtMinValueAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtMaxValueAdd")).Text,
                    int.Parse(((RadComboBox)e.Item.FindControl("ddlValueTypeAdd")).SelectedValue));
                BindData();
                gvDMRMeteorology.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDMRMeteorology.DMRMeteorologyDelete(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblMeteorologyId")).Text));
                BindData();
                gvDMRMeteorology.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
               if (!checkvalue((((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text),
                        (((RadTextBox)e.Item.FindControl("txtMeteorologyNameEdit")).Text),
                        ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text,
                        ((RadComboBox)e.Item.FindControl("ddlValueTypeEdit")).SelectedValue,
                        ((UserControlMaskNumber)e.Item.FindControl("txtMinValueEdit")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("txtMaxValueEdit")).Text))
                        return;

                    PhoenixRegistersDMRMeteorology.DMRMeteorologyUpdate(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(((RadLabel)e.Item.FindControl("lblMeteorologyIdEdit")).Text),
                        ((RadTextBox)e.Item.FindControl("txtMeteorologyNameEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text,
                        int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text),
                         ((UserControlMaskNumber)e.Item.FindControl("txtMinValueEdit")).Text,
                         ((UserControlMaskNumber)e.Item.FindControl("txtMaxValueEdit")).Text,
                        int.Parse(((RadComboBox)e.Item.FindControl("ddlValueTypeEdit")).SelectedValue));

                    BindData();
                    gvDMRMeteorology.Rebind();

                }
                
            }

        
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDMRMeteorology_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
