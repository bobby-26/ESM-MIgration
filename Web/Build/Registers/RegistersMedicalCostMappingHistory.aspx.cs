using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersMedicalCostMappingHistory : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			
			BindMedicalData();
			BindMedicalTestData();
			BindMedicalVaccination();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}


	private void BindMedicalData()
	{
		try
		{

			DataSet ds = PhoenixRegistersMedicalCostMapping.MedicalCostMappingHistoryList(
						General.GetNullableInteger(Request.QueryString["clinicid"]), 1);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvMedical.DataSource = ds;
				gvMedical.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvMedical);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void BindMedicalTestData()
	{
		try
		{

			DataSet ds = PhoenixRegistersMedicalCostMapping.MedicalCostMappingHistoryList(
						General.GetNullableInteger(Request.QueryString["clinicid"]), 2);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvMedicalTest.DataSource = ds;
				gvMedicalTest.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvMedicalTest);
			}
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
			gv.Rows[0].Attributes["onclick"] = "";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void BindMedicalVaccination()
	{
		try
		{

			DataSet ds = PhoenixRegistersMedicalCostMapping.MedicalCostMappingHistoryList(
						General.GetNullableInteger(Request.QueryString["clinicid"]), 3);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvVaccination.DataSource = ds;
				gvVaccination.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvVaccination);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
}
