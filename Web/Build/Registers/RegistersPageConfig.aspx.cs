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

public partial class RegistersPageConfig : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersPageConfig.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRank')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersPageConfig.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersPageConfig.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersRank.AccessRights = this.ViewState;
            MenuRegistersRank.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ddlRegister.DataSource = PhoenixRegistersPageConfig.ListRegister(null);
                ddlRegister.DataBind();
                ddlRegister.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlregisterpage.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                gvRank.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlRegister_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ddlregisterpage.Text = "";
            ddlregisterpage.DataSource = PhoenixRegistersPageConfig.ListRegisterPage(General.GetNullableGuid(ddlRegister.SelectedValue));
            ddlregisterpage.DataBind();
            ddlregisterpage.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

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

        string[] alColumns = { "FLDCODE", "FLDNAME" };
        string[] alCaptions = { "Code", "Name" };
        DataSet ds = new DataSet();

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (General.GetNullableGuid(ddlRegister.SelectedValue) != null && ddlRegister.SelectedItem.Text.ToUpper() == "RANK")
        {
            ds = PhoenixRegistersPageConfig.SearchRegisterPageConfigforRank(General.GetNullableGuid(ddlRegister.SelectedValue), General.GetNullableGuid(ddlregisterpage.SelectedValue),
                1, iRowCount, ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixRegistersPageConfig.SearchRegisterPageConfigforVesselType(General.GetNullableGuid(ddlRegister.SelectedValue), General.GetNullableGuid(ddlregisterpage.SelectedValue),
              1, iRowCount, ref iRowCount, ref iTotalPageCount);
        }
        Response.AddHeader("Content-Disposition", "attachment; filename=Rank.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0'>");
        Response.Write("<tr>");
        Response.Write("<td width=\"150px\"><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Rank</h3></td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width=\"150px\">");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td width=\"150px\">");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersRank_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlRegister.Text = "";
                ddlregisterpage.Text = "";
                ddlRegister.DataSource = PhoenixRegistersPageConfig.ListRegister(null);
                ddlRegister.DataBind();
                ddlRegister.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlregisterpage.DataSource = PhoenixRegistersPageConfig.ListRegisterPage(General.GetNullableGuid(ddlRegister.SelectedValue));
                ddlregisterpage.DataBind();
                ddlregisterpage.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        gvRank.SelectedIndexes.Clear();
        gvRank.EditIndexes.Clear();
        gvRank.DataSource = null;
        gvRank.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCODE", "FLDNAME" };
        string[] alCaptions = { "Code", "Name" };
        DataSet ds = new DataSet();

        if (General.GetNullableGuid(ddlRegister.SelectedValue) != null && ddlRegister.SelectedItem.Text.ToUpper() == "RANK")
        {
            ds = PhoenixRegistersPageConfig.SearchRegisterPageConfigforRank(General.GetNullableGuid(ddlRegister.SelectedValue), General.GetNullableGuid(ddlregisterpage.SelectedValue),
                int.Parse(ViewState["PAGENUMBER"].ToString()), gvRank.PageSize, ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixRegistersPageConfig.SearchRegisterPageConfigforVesselType(General.GetNullableGuid(ddlRegister.SelectedValue), General.GetNullableGuid(ddlregisterpage.SelectedValue),
              int.Parse(ViewState["PAGENUMBER"].ToString()), gvRank.PageSize, ref iRowCount, ref iTotalPageCount);
        }


        General.SetPrintOptions("gvRank", "Register Page Config", alCaptions, alColumns, ds);

        gvRank.DataSource = ds;
        gvRank.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvRank_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                if (!IsValidRank())
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    string id = ((RadLabel)e.Item.FindControl("lblID")).Text;
                    PhoenixRegistersPageConfig.RegisterPageconfigInsert(new Guid(ddlRegister.SelectedValue), new Guid(ddlregisterpage.SelectedValue), int.Parse(id));
                    Rebind();
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
    protected void gvRank_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkUserRights");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (cb != null)
                cb.Checked = drv["FLDMAPPEDYN"].ToString().Equals("1") ? true : false;
        }
    }
    private bool IsValidRank()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlRegister.SelectedValue.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Register is required.";

        if (ddlregisterpage.SelectedValue.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Page is required.";


        return (!ucError.IsError);
    }
    protected void gvRank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRank.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



}
