using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAGenericRevisionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);        
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAGenericRevisionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentGenericRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuGeneric.AccessRights = this.ViewState;
        MenuGeneric.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["GENERICID"] = "";            
            if (Request.QueryString["genericid"] != null && Request.QueryString["genericid"].ToString() != "")
                ViewState["GENERICID"] = Request.QueryString["genericid"].ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME" };
        string[] alCaptions = { "Ref Number", "Vessel", "Date", "Type", "Activity / Conditions", "Revision No", "Status" };

        DataSet ds = PhoenixInspectionRiskAssessmentGeneric.InspectionRiskAssessmentGenericRevisionSearch(
                    new Guid(ViewState["GENERICID"].ToString()),
                    gvRiskAssessmentGenericRevision.CurrentPageIndex+1,
                    gvRiskAssessmentGenericRevision.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentGenericRevision", "Generic Revisions", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //gvRiskAssessmentGenericRevision.DataSource = ds;
            //gvRiskAssessmentGenericRevision.DataBind();

            //if (ViewState["SELECTEDGENERICID"] == null)
            //{
            //    ViewState["SELECTEDGENERICID"] = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTGENERICID"].ToString();
            //    gvRiskAssessmentGenericRevision.SelectedIndex = 0;
            //}
            //SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }
        gvRiskAssessmentGenericRevision.DataSource = ds;
        gvRiskAssessmentGenericRevision.VirtualItemCount = iRowCount;
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME" };
            string[] alCaptions = { "Ref Number", "Vessel", "Date", "Type", "Activity / Conditions", "Revision No", "Status" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentGeneric.InspectionRiskAssessmentGenericRevisionSearch(
                    new Guid(ViewState["GENERICID"].ToString()),
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("Generic Revisions", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuGeneric_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    protected void gvRiskAssessmentGenericRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblCargoID = (RadLabel)e.Item.FindControl("lblRiskAssessmentGenericID");
            LinkButton lnkRefNo = (LinkButton)e.Item.FindControl("lnkRefNo");
            if (lnkRefNo != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
                {
                    lnkRefNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGeneric.aspx?genericid=" + lblCargoID.Text + "&RevYN=1" + "'); return true;");
                }
            }
           // if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                RadLabel lblWorkActivity = (RadLabel)e.Item.FindControl("lblWorkActivity");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucWorkActivity");
                if (uct != null)
                {
                    lblWorkActivity.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lblWorkActivity.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
            }
        }
    }

    protected void gvRiskAssessmentGenericRevision_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if(gce.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = gce.Item.ItemIndex;

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentGenericID = (RadLabel)gvRiskAssessmentGenericRevision.Items[rowindex].FindControl("lblRiskAssessmentGenericID");
            if (lblRiskAssessmentGenericID != null)
            {
                ViewState["SELECTEDGENERICID"] = lblRiskAssessmentGenericID.Text;
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
        //gvRiskAssessmentGenericRevision.SelectedIndex = -1;
        //for (int i = 0; i < gvRiskAssessmentGenericRevision.Rows.Count; i++)
        //{
        //    if (gvRiskAssessmentGenericRevision.DataKeys[i].Value.ToString().Equals(ViewState["SELECTEDGENERICID"].ToString()))
        //    {
        //        gvRiskAssessmentGenericRevision.SelectedIndex = i;
        //    }
        //}
    }
}
