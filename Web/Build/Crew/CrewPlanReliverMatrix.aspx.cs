using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewPlanReliverMatrix : PhoenixBasePage
{
    string strVessel = string.Empty;
    string strRankId = string.Empty;
  //  string strOffSigner = string.Empty;
    string strRelieverId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string empid = Request.QueryString["empid"];
            string vesselid = Request.QueryString["vesselid"];
            string rankid = Request.QueryString["rankid"];
            if (!IsPostBack)
            {

                
            }
            BindRelieverMatrixData(General.GetNullableInteger(empid), General.GetNullableInteger(rankid), General.GetNullableInteger(vesselid));
          
           
               
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    public void BindRelieverMatrixData(int? iEmployeeId, int? iRankId, int? iVesselId)
    {

        try
        {

            NameValueCollection nvc = Filter.CurrentPlanRelieverFilterSelection;
            DataTable dt = PhoenixCrewPlanning.CrewPlanRelieverMatrixList(iRankId, iVesselId);



            if (dt.Rows.Count > 0)
            {
                gvRelieverMatrix.DataSource = dt;
                gvRelieverMatrix.DataBind();
            }
            else
            {                
                ShowNoRecordsFound(dt, gvRelieverMatrix);
            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    decimal r;
    decimal r2;
    protected void gvRelieverMatrix_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                if (drv["FLDRANKEXP"].ToString() != "")
                {
                    r = r + decimal.Parse(drv["FLDRANKEXP"].ToString());
                }
                if (drv["FLDVESSELTYPEEXP"].ToString() != "")
                {
                    r2 = r2 + decimal.Parse(drv["FLDVESSELTYPEEXP"].ToString());
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[2].Font.Bold = true;
            e.Row.Cells[3].Font.Bold = true;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[3].Text = r.ToString();
            e.Row.Cells[4].Font.Bold = true;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].Text = r2.ToString();
        }
    }
}
