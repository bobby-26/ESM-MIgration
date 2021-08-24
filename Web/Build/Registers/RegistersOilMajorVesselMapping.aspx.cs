using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersOilMajorVesselMapping : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersOilMajorVesselMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuMapping.AccessRights = this.ViewState;
            MenuMapping.MenuList = toolbar.Show();

            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddButton("Oil Major", "OILMAJOR");
            toolbar2.AddButton("Mapping", "MAPPING");
            Oilmajor.AccessRights = this.ViewState;
            Oilmajor.MenuList = toolbar2.Show();
            Oilmajor.SelectedMenuIndex = 1;
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvMapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Oilmajor_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("OILMAJOR"))
            {
                Response.Redirect("RegistersOilMajorMatrix.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {             
                ViewState["PAGENUMBER"] = 1;
                BindData();               
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDOILMAJOR" };
        string[] alCaptions = { "Vessel", "Oil Major" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersOilMajorVesselMapping.OilMajorSearch(
            null, null,null,
            sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvMapping.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvMapping", "Oil Major Matrix", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMapping.DataSource = ds;
            gvMapping.VirtualItemCount = iRowCount;
        }
        else
        {
            gvMapping.DataSource = "";
        }  
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDOILMAJOR" };
        string[] alCaptions = { "Vessel", "Oil Major" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersOilMajorVesselMapping.OilMajorSearch(
         null, null, null,
         sortexpression,
         sortdirection,
         (int)ViewState["PAGENUMBER"],
         gvMapping.PageSize,
         ref iRowCount,
         ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Oil_Major_Matrix.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Oil Major Matrix</h3></td>");
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
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvMapping.Rebind();
    }

    
    protected void gvMapping_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string vesselid = (((UserControlVessel)e.Item.FindControl("ucVessel")).SelectedVessel);
                string oilmajor = ((UserControlHard)e.Item.FindControl("ddlOilMajorAdd")).SelectedHard;

                PhoenixRegistersOilMajorVesselMapping.InsertOilMajorVessel(
                    General.GetNullableInteger(vesselid)
                    , General.GetNullableInteger(oilmajor)
                    );

                BindData();
                gvMapping.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string mappingid = ((RadLabel)e.Item.FindControl("lblmappingid")).Text;
                PhoenixRegistersOilMajorVesselMapping.DeleteOilMajorVessel(int.Parse(mappingid));
                BindData();
                gvMapping.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string oilmajor = ((UserControlHard)e.Item.FindControl("ddlOilMajoredit")).SelectedHard;
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselIdEdit")).Text;
                string mappingid = ((RadLabel)e.Item.FindControl("lblmappingidedit")).Text;

                PhoenixRegistersOilMajorVesselMapping.UpdateOilMajorVessel(
                    General.GetNullableInteger(vesselid)
                    , General.GetNullableInteger(oilmajor)
                    , int.Parse(mappingid)
                    );
                BindData();
                gvMapping.Rebind();
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

    protected void gvMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMapping.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvMapping_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
            }
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null)
                {
                    del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                }

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                UserControlHard ddlOilMajor = (UserControlHard)e.Item.FindControl("ddlOilMajoredit");
                if (ddlOilMajor != null)
                    ddlOilMajor.SelectedHard = drv["FLDOILMAJORID"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
