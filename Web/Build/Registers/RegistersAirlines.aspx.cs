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
public partial class RegistersAirlines : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersAirlines.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAirlines')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersAirlines.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersAirlines.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersAirlines.AccessRights = this.ViewState;
            MenuRegistersAirlines.MenuList = toolbar.Show();
           
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvAirlines.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDAIRLINESCODE", "FLDAIRLINESNAME", "FLDCOUNTRYNAME", "FLDACTIVE" };
        string[] alCaptions = { "Code","Name", "Country","Active Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersAirlines.AirlinesSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,txtSearch.Text,txtAirlinesCode.Text, null , sortexpression, sortdirection,
        1,
        iRowCount,
        ref iRowCount,
        ref iTotalPageCount,
        General.GetNullableInteger(ucCountry.SelectedCountry));


        Response.AddHeader("Content-Disposition", "attachment; filename=Airlines.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Airline</h3></td>");
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

    protected void RegistersAirlines_TabStripCommand(object sender, EventArgs e)
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
                txtAirlinesCode.Text = "";
                txtSearch.Text = "";
                ucCountry.SelectedCountry = "";
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDAIRLINESCODE", "FLDAIRLINESNAME", "FLDCOUNTRYNAME", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Country", "Active Y/N" };
       
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersAirlines.AirlinesSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , txtSearch.Text
            , txtAirlinesCode.Text
            , null
            , sortexpression
            , sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvAirlines.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(ucCountry.SelectedCountry));

        General.SetPrintOptions("gvAirlines", "Airline", alCaptions, alColumns, ds);

        gvAirlines.DataSource = ds;
        gvAirlines.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvAirlines_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string airlinescode = ((RadTextBox)e.Item.FindControl("txtAirlinescodeAdd")).Text;
                string airlinesname = ((RadTextBox)e.Item.FindControl("txtAirlinesNameAdd")).Text;
                string countryid = ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).SelectedCountry;
                int isactive = (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0;

                if (!IsValidAirlines(airlinescode, airlinesname, countryid))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersAirlines.InsertAirlines(PhoenixSecurityContext.CurrentSecurityContext.UserCode, airlinescode, airlinesname, Convert.ToInt32(countryid), isactive);
                ((RadTextBox)e.Item.FindControl("txtAirlinesNameAdd")).Focus();
                ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).DataBind();
                Rebind();
                ucStatus.Text = "Airlines information Added";
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                int airlinesid = Int16.Parse(((RadLabel)e.Item.FindControl("lblAirlinesidEdit")).Text);
                string airlinescode = ((RadTextBox)e.Item.FindControl("txtAirlinescodeedit")).Text;
                string airlinesname = ((RadTextBox)e.Item.FindControl("txtAirlinesNameedit")).Text;
                string countryid = ((UserControlCountry)e.Item.FindControl("ucCountryEdit")).SelectedCountry;
                int isactive = (((RadCheckBox)e.Item.FindControl("chkActiveYNedit")).Checked.Equals(true)) ? 1 : 0;

                if (!IsValidAirlines(airlinescode, airlinesname, countryid))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersAirlines.UpdateAirlines(PhoenixSecurityContext.CurrentSecurityContext.UserCode, airlinesid, airlinescode, airlinesname, Convert.ToInt32(countryid), isactive);
                Rebind();
                ucStatus.Text = "Airlines information updated";
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["AIRLINESCODE"] = ((RadLabel)e.Item.FindControl("lblAirlinesid")).Text;
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

    protected void Rebind()
    {
        gvAirlines.SelectedIndexes.Clear();
        gvAirlines.EditIndexes.Clear();
        gvAirlines.DataSource = null;
        gvAirlines.Rebind();
    }
    protected void gvAirlines_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)  
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                //db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }

        if(e.Item.IsInEditMode)
        {
            string AirlineCode = ((RadTextBox)e.Item.FindControl("txtAirlinescodeEdit")).Text;
            string AirlineNames = ((RadTextBox)e.Item.FindControl("txtAirlinesNameEdit")).Text;
            UserControlCountry ddlCountry = (UserControlCountry)e.Item.FindControl("ucCountryEdit");
            string CountryId = ((RadLabel)e.Item.FindControl("lblCountryId")).Text;

            if (CountryId != null)
            {
                ddlCountry.SelectedCountry = CountryId;
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
        }
    }
    private bool IsValidAirlines(string airlinescode,string airlinesname,string countryid)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;

        if (airlinescode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (airlinesname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (countryid == null || !Int16.TryParse(countryid, out result))
            ucError.ErrorMessage = "Country  is required";

        return (!ucError.IsError);
    }
    protected void gvAirlines_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAirlines.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAirlines_SortCommand(object source, GridSortCommandEventArgs e)
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
        PhoenixRegistersAirlines.DeleteAirlines(PhoenixSecurityContext.CurrentSecurityContext.UserCode,int.Parse(ViewState["AIRLINESCODE"].ToString()));
        Rebind();
    }
}
