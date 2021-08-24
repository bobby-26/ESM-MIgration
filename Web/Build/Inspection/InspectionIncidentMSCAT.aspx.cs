using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionIncidentMSCAT : PhoenixBasePage
{
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {       
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            VesselConfiguration();
            ViewState["Vesselid"] = "";            
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentMSCAT.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMSCAT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentMSCATAdd.aspx", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuMSCAT.AccessRights = this.ViewState;
        MenuMSCAT.MenuList = toolbar.Show();    
    }

    protected void gvMSCAT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDITMSCAT"))
            {
                RadLabel lblMscatid = (RadLabel)e.Item.FindControl("lblMscatid");                
                Response.Redirect("../Inspection/InspectionIncidentMSCATEdit.aspx?mscatid=" + lblMscatid.Text, true);
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

            //if (ce != null)
            //{
            //    if (lblMscatid != null && lblMscatid != string.Empty)
            //    {
            //        ce.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionIncidentMSCATEdit.aspx?mscatid=" + new Guid(lblMscatid) + "');return true;");
            //    }
            //}
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void BindData()
    {
        string[] alColumns = { "FLDFINDINGS", "FLDIMMEDIATECAUSE", "FLDICREMARKS", "FLDBASICSUBCAUSE", "FLDBCREMARKS", "FLDSUBCONTROLACTIONNEEDED" };
        string[] alCaptions = { "Findings", "Immediate Cause", "Remarks", "Basic Cause", "Remarks", " Control Action Needs" };

        DataSet ds = new DataSet();
        ds = PhoenixInspectionIncidentMSCAT.IncidentMSCATItemList(new Guid(Filter.CurrentIncidentID));

        General.SetPrintOptions("gvMSCAT", "RCA List", alCaptions, alColumns, ds);

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
            string[] alColumns = { "FLDFINDINGS", "FLDIMMEDIATECAUSE", "FLDICREMARKS", "FLDBASICSUBCAUSE", "FLDBCREMARKS", "FLDSUBCONTROLACTIONNEEDED" };
            string[] alCaptions = { "Findings", "Immediate Cause", "Remarks", "Basic Cause", "Remarks", " Control Action Needs" };

            DataSet ds = PhoenixInspectionIncidentMSCAT.IncidentMSCATItemList(new Guid(Filter.CurrentIncidentID));

            General.ShowExcel("RCA List", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        for (int i = 0; i < gvMSCAT.Items.Count; i++)
        {
            if (gvMSCAT.MasterTableView.Items[i].GetDataKeyValue("FLDMSCATID").ToString().Equals(ViewState["MSCATID"].ToString()))
            {
                gvMSCAT.MasterTableView.Items[i].Selected = true;
            }
        }
    }
    public class GridDecorator
    {
        public static void MergeRows(RadGrid gridView)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string strCurrentFindings = ((RadLabel)gridView.Items[rowIndex].FindControl("lblFindings")).Text;
                string strPreviousFindings = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblFindings")).Text;

                if (strCurrentFindings == strPreviousFindings)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }                

                string strCurrentIC = ((RadLabel)gridView.Items[rowIndex].FindControl("lblIC")).Text;
                string strPreviousIC = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblIC")).Text;

                if (strCurrentIC == strPreviousIC)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }

                string strCurrentICRemarks = ((RadLabel)gridView.Items[rowIndex].FindControl("lblICRemarks")).Text;
                string strPreviousICRemarks = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblICRemarks")).Text;

                if (strCurrentICRemarks == strPreviousICRemarks)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string strCurrentBC = ((RadLabel)gridView.Items[rowIndex].FindControl("lblBC")).Text;
                string strPreviousBC = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblBC")).Text;

                if (strCurrentBC == strPreviousBC)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                           previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

                string strCurrentBCRemarks = ((RadLabel)gridView.Items[rowIndex].FindControl("lblBCRemarks")).Text;
                string strPreviousBCRemarks = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblBCRemarks")).Text;

                if (strCurrentICRemarks == strPreviousICRemarks)
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                           previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }

                string strCurrentCAN = ((RadLabel)gridView.Items[rowIndex].FindControl("lblCAN")).Text;
                string strPreviousCAN = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblCAN")).Text;

                if (strCurrentCAN == strPreviousCAN)
                {
                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                           previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }
            }
        }
    }
    protected void gvMSCAT_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        RadLabel lblMscatid = ((RadLabel)e.Item.FindControl("lblMscatid"));
        PhoenixInspectionIncidentMSCAT.DeleteMSCATItem(new Guid(lblMscatid.Text));
        Rebind();
    }
    protected void Rebind()
    {
        gvMSCAT.SelectedIndexes.Clear();
        gvMSCAT.EditIndexes.Clear();
        gvMSCAT.DataSource = null;
        gvMSCAT.Rebind();
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

    protected void gvMSCAT_PreRender(object sender, EventArgs e)
    {
        //GridDecorator.MergeRows(gvMSCAT);
    }
}
