using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web;
using System.Text;


public partial class DryDockJobDiff : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			
			if (!IsPostBack)
			{
				ViewState["JOBID"]  = null;
                if (Request.QueryString["JOBID"] != null)
				{
                    ViewState["JOBID"] = Request.QueryString["JOBID"];
				}               											
			}
			BindData();			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void DryDockRepairJobLineItem_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			//ShowExcel();
		}
	}

	private void BindData()
	{       

		string[] alColumns = { "FLDSERIALNO", "FLDDETAIL", "FLDUNIT","FLDINCLUDEYN" };
		string[] alCaptions = { "Serial No", "Job Detail", "Unit","Include" };

		DataTable dt = PhoenixDryDockJob.ListDryDockJobDescriptionChange(
            PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["JOBID"].ToString()));

		//General.SetPrintOptions("gvJobDiff", "Repair Job line Item", alCaptions, alColumns, ds);

        if (dt.Rows.Count > 0)
		{
            gvJobDiff.DataSource = dt;
            gvJobDiff.DataBind();
		}
		else
		{
            ShowNoRecordsFound(dt, gvJobDiff);
		}

	}		
    
	private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
}
