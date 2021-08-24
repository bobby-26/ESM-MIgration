using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;

public partial class CommonPickListCrewOnboard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["VesselId"] != null)
                ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();
        }

        BindData();
    }

    private void BindData()
    {
        try
        {
            DataSet ds;
            int? vesselid = (Request.QueryString["VesselId"] != null) ? General.GetNullableInteger(Request.QueryString["VesselId"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            DateTime? date = (Request.QueryString["date"] != null) ? General.GetNullableDateTime(Request.QueryString["date"].ToString()) : General.GetNullableDateTime("");

            ds = PhoenixCrewManagement.ListCrewOnboard(
                vesselid == null ? 0 : vesselid
                , General.GetNullableInteger(ucRank.SelectedRank)
                , date
                , null);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewList.DataSource = ds;
                gvCrewList.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewList);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        string Script = "";
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = new NameValueCollection();

            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkCrew");
            nvc.Add(lb.ID, lb.Text.ToString());
            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewId");
            nvc.Add(lbl.ID, lbl.Text);
        }
        else
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = Filter.CurrentPickListSelection;
            
            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkCrew");
            nvc.Set(nvc.GetKey(1), lb.Text.ToString());
            Label lblCrewId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewId");
            nvc.Set(nvc.GetKey(2), lblCrewId.Text.ToString());
        }

        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

    }

    protected void gvCrewList_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void gvCrewList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            
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
