using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVProcedureVesseltypeMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUMRVProcedureVesseltypeMapping.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvEmissionSource')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Location_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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
   

   

    protected void gvEmissionSource_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                RadCheckBoxList chl = (RadCheckBoxList)e.Item.FindControl("chkVesselTypeEdit");
                if (chl != null)
                {
                    DataSet Ds = PhoenixRegistersEUMRVVesselTypeMapping.ListEUVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode); // For only fueloil
                    chl.DataSource = Ds;
                    chl.DataBindings.DataTextField = "FLDVESSELTYPE";
                    chl.DataBindings.DataValueField = "FLDEUMRVVESSELTYPEID";
                    chl.DataBind();

                    foreach (ButtonListItem li in chl.Items)
                    {
                        string[] slist = drv["FLDVESSELTYPE"].ToString().Split(',');
                        foreach (string s in slist)
                        {
                            if (li.Value.Equals(s))
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvEmissionSource_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkVesselTypeEdit");
            string FuelId = "";
            foreach (ButtonListItem li in chk.Items)
            {
                if (li.Selected)
                {
                    FuelId += li.Value + ",";
                }
            }
            string lblCode = (((RadLabel)e.Item.FindControl("lblCode")).Text);
            string ProcedureId = (((RadLabel)e.Item.FindControl("lblProcedureId")).Text);
            String dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
            PhoenixVesselPositionEUMRVProcedureVesselTypeMapping.InsertEUVesselTypeMapping(lblCode, new Guid(ProcedureId)
                                                                                            , General.GetNullableString(FuelId)
                                                                                            , General.GetNullableGuid(dtkey)
                                                                                           );
            gvEmissionSource.SelectedIndexes.Clear();
            gvEmissionSource.EditIndexes.Clear();
            gvEmissionSource.DataSource = null;
            gvEmissionSource.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDCODE", "FLDPROCEDURE", "FLDVESSELTYPENAME" };
        string[] alCaptions = { "Table", "Procedure", "Vessel Type" };

        DataSet ds = PhoenixVesselPositionEUMRVProcedureVesselTypeMapping.ListEUVesselType();

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ProcedureVesselTypeMapping.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>EUMRV Config</h3></td>");
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

    protected void gvEmissionSource_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        string[] alColumns = { "FLDCODE", "FLDPROCEDURE", "FLDVESSELTYPENAME" };
        string[] alCaptions = { "Table", "Procedure", "Vessel Type" };
        DataSet ds = PhoenixVesselPositionEUMRVProcedureVesselTypeMapping.ListEUVesselType();

        General.SetPrintOptions("gvEmissionSource", "EUMRV Config", alCaptions, alColumns, ds);

        gvEmissionSource.DataSource = ds;
   
    }
}
