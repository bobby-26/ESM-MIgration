using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersNOXList : PhoenixBasePage
{
      protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersNOXList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvNOx')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersNOXList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersNOXList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersNOX.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOMPANY");

            MenuNOx.AccessRights = this.ViewState;
            MenuNOx.MenuList = toolbar.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";

                UcVessel.bind();
                UcVessel.DataBind();
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                    {
                        ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
                            UcVessel.Enabled = false;
                    }
                }
                else
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
                    UcVessel.Enabled = false;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvNOx.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDNOX" };
        string[] alCaptions = { "Vessel", "NOx" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersNOX.NOXSearch(
            General.GetNullableInteger(UcVessel.SelectedVessel),
            sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvNOx.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ESIParameters.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ESI Parameters</h3></td>");
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

    protected void NOx_TabStripCommand(object sender, EventArgs e)
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
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            UcVessel.SelectedVessel = "";
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }


    protected void gvNOx_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersNOX.DeleteNOX(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid((((RadLabel)e.Item.FindControl("lblNOxId")).Text)));
                Rebind();
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

    protected void gvNOx_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            gvNOx.Columns[1].Visible = false;
        }

        DataRowView dr = (DataRowView)e.Item.DataItem;

        if (e.Item is GridEditableItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                if (del != null)
                    del.Visible = true;
            }
            else
            {
                if (del != null)
                    del.Visible = false;
            }

            if (!SessionUtil.CanAccess(this.ViewState, del.CommandName)) del.Visible = false;

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName)) edit.Visible = false;


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }

            LinkButton lb = (LinkButton)e.Item.FindControl("lblVesselName");
            if (lb != null)
                lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersNOX.aspx?NOxId=" + dr["FLDNOXID"].ToString() + "&VesselId=" + dr["FLDVESSELID"].ToString() + "');return false;");

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
                db1.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Registers/RegistersNOX.aspx?NOxId=" + dr["FLDNOXID"].ToString() + "&VesselId=" + dr["FLDVESSELID"].ToString() + "');return false;");
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvNOx_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvNOx.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersNOX.NOXSearch(
            General.GetNullableInteger(UcVessel.SelectedVessel),
            sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvNOx.PageSize, ref iRowCount, ref iTotalPageCount);

        string[] alColumns = { "FLDVESSELNAME", "FLDNOX" };
        string[] alCaptions = { "Vessel", "NOx" };

        General.SetPrintOptions("gvNOx", "ESI Parameters", alCaptions, alColumns, ds);

        gvNOx.DataSource = ds;
        gvNOx.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvNOx.SelectedIndexes.Clear();
        gvNOx.EditIndexes.Clear();
        gvNOx.DataSource = null;
        gvNOx.Rebind();
    }
}
