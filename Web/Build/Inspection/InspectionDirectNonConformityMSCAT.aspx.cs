using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionDirectNonConformityMSCAT : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);        

        if (!IsPostBack)
        {
            VesselConfiguration();
            if (Request.QueryString["REVIEWDNC"] != null && Request.QueryString["REVIEWDNC"].ToString() != string.Empty)
            {
                ViewState["REVIEWDNC"] = Request.QueryString["REVIEWDNC"].ToString();
                EditNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));
            }                       
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDirectNonConformityMSCAT.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMSCAT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionDirectNonConformityMSCATAdd.aspx?REVIEWDNC=" + (ViewState["REVIEWDNC"] == null ? null : ViewState["REVIEWDNC"].ToString()) + "')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuMSCAT.AccessRights = this.ViewState;
        MenuMSCAT.MenuList = toolbar.Show();
    }
    protected void gvMSCAT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDITMSCAT"))
            {
                BindPageURL(e.Item.ItemIndex);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMSCAT_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton ce = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ce != null) ce.Visible = SessionUtil.CanAccess(this.ViewState, ce.CommandName);

            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            de.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (de != null) de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);

            string lblMscatid = ((RadLabel)e.Item.FindControl("lblMscatid")).Text;

            if (ce != null)
            {
                if (lblMscatid != null && lblMscatid != string.Empty)
                {
                    ce.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMSCATEdit.aspx?mscatid=" + new Guid(lblMscatid) + "&DEFICIENCYID=" + (ViewState["REVIEWDNC"] == null ? null : ViewState["REVIEWDNC"].ToString()) + "');return true;");
                }
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void BindData()
    {
        string[] alColumns = { "FLDDEFICIENCYDETAILS", "FLDIMMEDIATECAUSE", "FLDICREMARKS", "FLDBASICSUBCAUSE", "FLDBCREMARKS", "FLDSUBCONTROLACTIONNEEDED" };
        string[] alCaptions = { "CAR", "Immediate Cause", "Remarks", "Basic Cause", "Remarks", " Control Action Needs" };

        DataSet ds = new DataSet();
        ds = PhoenixInspectionIncidentMSCAT.MSCATItemList(ViewState["REVIEWDNC"] == null ? null : General.GetNullableGuid((ViewState["REVIEWDNC"].ToString())));

        General.SetPrintOptions("gvMSCAT", "M-SCAT Analysis", alCaptions, alColumns, ds);

        gvMSCAT.DataSource = ds;
    }

    protected void MenuMSCAT_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDDEFICIENCYDETAILS", "FLDIMMEDIATECAUSE", "FLDICREMARKS", "FLDBASICSUBCAUSE", "FLDBCREMARKS", "FLDSUBCONTROLACTIONNEEDED" };
            string[] alCaptions = { "CAR", "Immediate Cause", "Remarks", "Basic Cause", "Remarks", " Control Action Needs" };

            DataSet ds = PhoenixInspectionIncidentMSCAT.MSCATItemList(ViewState["REVIEWDNC"] == null ? null : General.GetNullableGuid((ViewState["REVIEWDNC"].ToString())));

            General.ShowExcel("M-SCAT Analysis", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void EditNonConformity(Guid reviewnonconformity)
    {
        DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNC(reviewnonconformity);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ViewState["vesselid"] = dr["FLDVESSELID"].ToString();
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblMscatid = (RadLabel)gvMSCAT.Items[rowindex].FindControl("lblMscatid");
            if (lblMscatid != null)
            {
                ViewState["MSCATID"] = lblMscatid.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvMSCAT.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvMSCAT.Items)
        {
            if (item.GetDataKeyValue("FLDMSCATID").ToString() == ViewState["MSCATID"].ToString())
            {
                gvMSCAT.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void gvMSCAT_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblMscatid = ((RadLabel)e.Item.FindControl("lblMscatid"));
            PhoenixInspectionIncidentMSCAT.DeleteMSCATItem(new Guid(lblMscatid.Text));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMSCAT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMSCAT.CurrentPageIndex + 1;
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
        gvMSCAT.SelectedIndexes.Clear();
        gvMSCAT.EditIndexes.Clear();
        gvMSCAT.DataSource = null;
        gvMSCAT.Rebind();
    }
}
