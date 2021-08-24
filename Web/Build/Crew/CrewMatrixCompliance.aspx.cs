using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class CrewMatrixCompliance : PhoenixBasePage
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddFontAwesomeButton("../Crew/CrewMatrixCompliance.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        //toolbar.AddFontAwesomeButton("javascript:CallPrint('GvCrewMatrix')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Crew/CrewMatrixCompliance.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuCrewOilMajorMatrix.AccessRights = this.ViewState;
        MenuCrewOilMajorMatrix.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            
        }
    }


    protected void MenuCrewOilMajorMatrix_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
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

        DataTable dt = PhoenixCrewMatrixCompliance.SearchMatrixCompliance(null,
                                                                         null,
                                                                         General.GetNullableInteger(ddlContract.SelectedHard));

        GvCrewMatrix.DataSource = dt;

    }

    protected void GvCrewMatrix_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
    {
        BindData();

        //RadPivotGrid grid = (RadPivotGrid)sender;
        //DataTable dt = PhoenixCrewMatrixCompliance.SearchMatrixCompliance(null,
        //                                                                  null,
        //                                                                  General.GetNullableInteger(ddlContract.SelectedHard));

        //GvCrewMatrix.DataSource = dt;

    }

    protected void GvCrewMatrix_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
    {

        RadPivotGrid grid = (RadPivotGrid)sender;
        if (e.Cell is PivotGridRowHeaderCell)
        {
            PivotGridRowHeaderCell cell = (PivotGridRowHeaderCell)e.Cell;
            PivotGridRowHeaderItem item = (PivotGridRowHeaderItem)e.Cell.DataItemContainer;
            string row = cell.ParentIndexes.Length > 0 ? cell.ParentIndexes[0].ToString() : string.Empty;
            System.Collections.ArrayList itemarray = (System.Collections.ArrayList)item.DataItem;
            if (itemarray.Count > 1)
            {
                DataTable dt = (DataTable)grid.DataSource;
        
                DataRow[] dr = dt.Select("FLDVESSELID = '" + itemarray[2].ToString() + "'");
              
                if (cell.Text.Trim() == string.Empty)
                {
                     cell.Text = cell.DataItem.ToString();
                }
            }
        }
        if (e.Cell is PivotGridDataCell)
        {
            PivotGridDataCell cell = (PivotGridDataCell)e.Cell;
            if (cell.CellType == PivotGridDataCellType.DataCell)
            {
                DataTable dt = (DataTable)grid.DataSource;
                string code = string.Empty;
                string Vesselid = cell.ParentRowIndexes.Length > 1 ? cell.ParentRowIndexes[2].ToString() : string.Empty;
                string oilmajor = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;

                DataRow[] dr = dt.Select("FLDVESSELID = '" + Vesselid + "' AND FLDOILMAJORNAME='" + oilmajor + "' AND FLDCONTRACTID= '" + ddlContract.SelectedHard + "'");
                foreach (DataRow d in dr)
                {
                    string text = cell.Text;
                    if (d["FLDCOMPLIANCE"].ToString() != "" )
                    {
                        cell.HorizontalAlign = HorizontalAlign.Center;
                        
                        string querystring = "?vesselid=" + d["FLDVESSELID"].ToString() +"&oilmajorid=" + d["FLDOILMAJORID"].ToString() + "&contractid=" + ddlContract.SelectedHard;
                        string link = "Crew/CrewMatrixComplianceOnboard.aspx";

                        string textlink = "<a   href=\"javascript:top.openNewWindow('wo', '" + d["FLDCOMPLIANCE"].ToString() + "', '" + link + querystring + "',false);\" >" + d["FLDCOMPLIANCE"].ToString() + "</a>";

                        cell.Text = textlink;

                        //cell.Text =  d["FLDCOMPLIANCE"].ToString();
                    }
               
                }
            }
        }
    }


}