using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewIncidentsEmployeeList : PhoenixBasePage
{
    string strIncidentId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuCrewList.MenuList = toolbar.Show();

            strIncidentId = Request.QueryString["id"];

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["CREWLIST"] = string.Empty;
                SetIncidentDetail();
                gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (gvCrewList.Items.Count > 0 && gvCrewList.Items[0].Cells.Count == 1) return;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string str = string.Empty;
                foreach (GridDataItem item in gvCrewList.Items)
                {
                    //GridViewRow row = gvCrewList.Items[i];
                    bool isChecked = ((CheckBox)item.FindControl("chkSelect")).Checked;

                    if (isChecked)
                    {
                        str += ((RadLabel)item.FindControl("lblEmployeeId")).Text + ",";
                    }
                }
                str = str.TrimEnd(',');
                PhoenixCrewIncidents.UpdateIncidentsCrweList(new Guid(strIncidentId), str);
                ucStatus.Text = "Information Updated";
                SetIncidentDetail();
                gvCrewList.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetIncidentDetail()
    {
        DataTable dt = PhoenixCrewIncidents.ListIncidents(new Guid(strIncidentId), null);
        if (dt.Rows.Count > 0)
        {
            ViewState["DATE"] = dt.Rows[0]["FLDINCIDENTDATE"].ToString();
            ViewState["VESSEL"] = dt.Rows[0]["FLDVESSELID"].ToString();
            ViewState["CREWLIST"] = dt.Rows[0]["FLDCREWLIST"].ToString();
            txtIncidentDate.Text = dt.Rows[0]["FLDINCIDENTDATE"].ToString();
            txtVesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewManagement.ListCrewOnboard(General.GetNullableInteger(ViewState["VESSEL"].ToString()), null
            , General.GetNullableDateTime(ViewState["DATE"].ToString()), null);


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewList.DataSource = ds;

            iRowCount = ds.Tables[0].Rows.Count;
            iTotalPageCount = 1;
            MenuCrewList.Visible = true;
        }
        else
        {
            iRowCount = 0;
            iTotalPageCount = 0;
            gvCrewList.DataSource = ds;
            MenuCrewList.Visible = false;
        }
        gvCrewList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
    }


    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            RadLabel lblId = (RadLabel)e.Item.FindControl("lblEmployeeId");
            CheckBox cl = (CheckBox)e.Item.FindControl("chkSelect");
            if (("," + ViewState["CREWLIST"].ToString() + ",").Contains("," + lblId.Text + ",")) cl.Checked = true;

        }
    }
}
