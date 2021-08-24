using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class Registers_RegisterProsperMeasure : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperMeasure.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvprospermeasure')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperMeasure.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperMeasure.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            MenuRegistersProsper.AccessRights = this.ViewState;
            MenuRegistersProsper.MenuList = toolbar.Show();
            //  MenuRegistersProsper.SetTrigger(pnlprospermeasure);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["CODE"] = txtmeasurecode.Text;
                ViewState["NAME"] = txtname.Text;
                gvprospermeasure.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        string[] alColumns = { "FLDMEASURECODE", "FLDMEASURENAME" };
        string[] alCaptions = { "Measure Code", "Measure Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegisterProsperMeasure.ProsperMesaureSearch(ViewState["CODE"].ToString(), ViewState["NAME"].ToString(), sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ProsperMeasure.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Prosper Measure</h3></td>");
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

    protected void RegistersProsper_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {

                ViewState["CODE"] = txtmeasurecode.Text;
                ViewState["NAME"] = txtname.Text;

                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvprospermeasure.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtmeasurecode.Text = "";
                txtname.Text = "";
                ViewState["CODE"] = txtmeasurecode.Text;
                ViewState["NAME"] = txtname.Text;
                BindData();
                gvprospermeasure.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDMEASURECODE", "FLDMEASURENAME" };
        string[] alCaptions = { "Measure Code", "Measure Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegisterProsperMeasure.ProsperMesaureSearch(ViewState["CODE"].ToString(), ViewState["NAME"].ToString(), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
           gvprospermeasure.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvprospermeasure", "Measure", alCaptions, alColumns, ds);
        gvprospermeasure.DataSource = ds;
        gvprospermeasure.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void ddlmodulecode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;

        BindData();
    }

    private bool IsValidInstallation(string measurecode, string measurename)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (measurecode.Trim().Equals(""))
            ucError.ErrorMessage = "Measure Code is required.";

        if (measurename.Trim().Equals(""))
            ucError.ErrorMessage = "Measure Name is required.";


        return (!ucError.IsError);
    }

    protected void gvprospermeasure_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvprospermeasure.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvprospermeasure_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {


                string measurecode = ((RadTextBox)e.Item.FindControl("txtmeasureCodeAdd")).Text.Trim();
                string measurename = ((RadTextBox)e.Item.FindControl("txtMeasureNameAdd")).Text.Trim();

                if (!IsValidInstallation(measurecode, measurename))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterProsperMeasure.InsertProsperMeasure(
                     measurecode
                     , measurename
                     );

                BindData();
                gvprospermeasure.Rebind();

            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid? measureid = General.GetNullableGuid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDMEASUREID"].ToString());
                PhoenixRegisterProsperMeasure.DeleteProsperMeasure(measureid);
                BindData();
                gvprospermeasure.Rebind();

            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {

                string code = ((RadLabel)e.Item.FindControl("txtmeasureCode")).Text;
                string name = ((RadTextBox)e.Item.FindControl("txtmeasureName")).Text;

                Guid? measureid = General.GetNullableGuid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDMEASUREID"].ToString());
                if (!IsValidInstallation(code, name))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegisterProsperMeasure.UpdateProsperMeasure(
                          measureid
                         , code
                         , name
                        );


                BindData();
                gvprospermeasure.Rebind();

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

    protected void gvprospermeasure_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
    }

    protected void gvprospermeasure_SortCommand(object sender, GridSortCommandEventArgs e)
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
    }
}
