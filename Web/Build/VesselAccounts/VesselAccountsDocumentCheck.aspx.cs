using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsDocumentCheck :  PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);            
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsDocumentCheck.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsDocumentCheck.aspx", "Find", "search.png", "SEARCH");
            MenuCrewSuitabilityList.AccessRights = this.ViewState;
            MenuCrewSuitabilityList.MenuList = toolbar.Show();
          
            BindData();			
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            string rank = string.Empty, flag = string.Empty, type = string.Empty;
            DataTable dt = PhoenixVesselAccountsEmployee.ListEmployeeDocumentCheck(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , General.GetNullableInteger(ddlEmployee.SelectedEmployee), ref rank, ref flag, ref type);
            txtRank.Text = rank;
            txtVesselType.Text = type;
            txtFlag.Text = flag;
            if (dt.Rows.Count > 0)
            {
                gvSuitability.DataSource = dt;
                gvSuitability.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvSuitability);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	protected void CrewSuitabilityList_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {           
            BindData();
        }
	}
	protected void ShowExcel()
	{
		DataSet ds = new DataSet();
		string[] alColumns = { "FLDCATEGORY", "FLDDOCUMENTNAME", "FLDDATEOFEXPIRY", "FLDNATIONALITY", "FLDMEETINGREQYN"};
		string[] alCaptions = { "Category", "Document", "Expiry Date", "Nationality", "Meeting Requirement" };

        string rank = string.Empty, flag = string.Empty, type = string.Empty;
        DataTable dt = PhoenixVesselAccountsEmployee.ListEmployeeDocumentCheck(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            , General.GetNullableInteger(ddlEmployee.SelectedEmployee), ref rank, ref flag, ref type);
        txtRank.Text = rank;
        txtVesselType.Text = type;
        txtFlag.Text = flag;

        General.ShowExcel("Suitability Check List", dt, alColumns, alCaptions, null, null);

	}
    protected void gvSuitability_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
			DataRowView drv = (DataRowView)e.Row.DataItem;
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                Label lblMissingYN = (Label)e.Row.FindControl("lblMissingYN");
                Label lblExpiredYN = (Label)e.Row.FindControl("lblExpiredYN");
                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                Label lblExpiryDate = (Label)e.Row.FindControl("lblExpiryDate");
                Label lblNationality = (Label)e.Row.FindControl("lblNationality");
                //Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");

                if (lblMissingYN.Text.Trim() == "1")
                {
                    lblDocumentName.ForeColor = System.Drawing.Color.Blue;
                    lblExpiryDate.ForeColor = System.Drawing.Color.Blue;
                    lblNationality.ForeColor = System.Drawing.Color.Blue;
                }
                else if (lblExpiredYN.Text.Trim() == "1")
                {
                    lblDocumentName.ForeColor = System.Drawing.Color.Red;
                    lblExpiryDate.ForeColor = System.Drawing.Color.Red;
                    lblNationality.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
	protected void gvSuitability_PreRender(object sender, EventArgs e)
	{
		GridDecorator.MergeRows(gvSuitability);
	}

	public class GridDecorator
	{
		public static void MergeRows(GridView gridView)
		{
			for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
			{
				GridViewRow row = gridView.Rows[rowIndex];
				GridViewRow previousRow = gridView.Rows[rowIndex + 1];

				string currentCategoryName = ((Label)gridView.Rows[rowIndex].FindControl("lblCategoryName")).Text;
				string previousCategoryName = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblCategoryName")).Text;
  
				if (currentCategoryName == previousCategoryName)
				{
					row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
										previousRow.Cells[1].RowSpan + 1;
					previousRow.Cells[1].Visible = false;
				}

			}
			
		}
	}
    protected void ddlEmployee_TextChangedEvent(object sender, EventArgs e)
    {        
        BindData();
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
