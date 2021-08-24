using AjaxControlToolkit;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class CrewOffshore_CrewOffshoreShellMatrixReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["VESSELID"] = Request.QueryString["VESSELID"];
            if (ViewState["dttable"] == null)
            {
                ViewState["dttable"] = null;
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreShellMatrixReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

            MenuShellMatrixWeekly.AccessRights = this.ViewState;
            MenuShellMatrixWeekly.MenuList = toolbar.Show();

            //ddlYear.SelectedYear = DateTime.Now.Year.ToString();
            //ddlMonth.SelectedMonthNumber = DateTime.Now.Month.ToString();
            DateTime today = DateTime.Today;
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            ViewState["EDIT"] = "0";
            BindVesselDetails();
            //BindData();
           GetTable();
        }
    }
    private void BindVesselDetails()
    {
        DataSet ds = null;
        string vesselid = null;

        if (Request.QueryString["VEESELID"] != null)
            vesselid = Request.QueryString["VEESELID"].ToString();
        ds = PhoenixCrewOffshoreCrewList.SearchVesselShellWeeklyVesselDetailsList(General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        if (ds.Tables.Count > 0)
        {
            DataTable dtdetails = ds.Tables[0];

            txtVessel.Text = General.GetNullableString(dtdetails.Rows[0]["FLDVESSELNAMETYPE"].ToString());
            txtDate.Text = General.GetNullableString(dtdetails.Rows[0]["FLDDATE"].ToString());
            txtLocation.Text = General.GetNullableString(dtdetails.Rows[0]["FLDLOCATION"].ToString());
            txtOwner.Text = General.GetNullableString(dtdetails.Rows[0]["FLDOWNER"].ToString());
        }
    }
    private void BindData()
    {
        //int year, month;
        DataSet ds = null;
        string vesselid = null;

        try
        {
            if (Request.QueryString["VEESELID"] != null)
                vesselid = Request.QueryString["VEESELID"].ToString();

            ds = PhoenixCrewOffshoreCrewList.SearchVesselShellWeeklyCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString()),1);

            if (ds.Tables.Count > 0)
            {
                

                DataTable dt = ds.Tables[1];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //adding columns dynamically
                    if (ViewState["EDIT"].ToString() != "1")
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            GridBoundColumn field = new GridBoundColumn();
                            field.HeaderText = General.GetNullableString(dt.Rows[i]["FLDDISPLAYNAME"].ToString());
                            field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                            //if (dt.Rows[i]["FLDDISPLAYNAME"].ToString() == "Name")
                            //{
                            //    field.HeaderStyle.Width = Unit.Percentage(15);
                            //}
                            //else
                            //    field.HeaderStyle.Width = Unit.Percentage(0.1);
                            gvshellmatrixView.Columns.Insert(gvshellmatrixView.Columns.Count, field);
                        }
                    }
                    gvshellmatrixView.DataSource = ds;
                  
                    ViewState["EDIT"] = "1";
                }
                else
                {
                    ShowNoRecordsFound(ds.Tables[0], gvShellNoRecordFound);
                    gvShellNoRecordFound.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            if (ds == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("FLDDISPLAYNAME");
                DataSet dsresult = new DataSet();
                dsresult.Tables.Add(dt);
                ShowNoRecordsFound(dsresult.Tables[0], gvShellNoRecordFound);
                gvShellNoRecordFound.Visible = true;
            }

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuShellMatrixWeekly_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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

   public void GetTable()
    {
        DataTable _datatable = new DataTable();
        DataSet ds = new DataSet();
        ds = PhoenixCrewOffshoreCrewList.SearchVesselShellWeeklyCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 1);
        _datatable.Columns.Add("FLDFILENO");
        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
        {
            _datatable.Columns.Add(ds.Tables[1].Rows[i]["FLDDISPLAYNAME"].ToString());
        }
        _datatable.Columns.Add("FLDEMPLOYEEID");

        

        if (ds.Tables.Count > 0)
        {

            DataTable dtvalues = ds.Tables[2];
            DataTable dtemployee = ds.Tables[0];
            DataTable header = ds.Tables[1];

            for (int i = 0; i < dtemployee.Rows.Count; i++)
            {
                DataRow dr1 = _datatable.NewRow();
                //dr1["FLDEMPLOYEEID"] = dtemployee.Rows[i]["FLDEMPLOYEEID"].ToString();
                //dr1["File No."] = dtemployee.Rows[i]["FLDFILENO"].ToString();
                //_datatable.Rows.Add(dr1.ItemArray);

                for (int j = 0; j < header.Rows.Count; j++)
                {
                    DataRow[] drdetails = dtvalues.Select("FLDEMPLOYEEID = " + dtemployee.Rows[i]["FLDEMPLOYEEID"].ToString() +
                                               " AND  FLDDOCHEADING = '" + header.Rows[j]["FLDDISPLAYNAME"].ToString() + "'");

                    dr1[header.Rows[j]["FLDDISPLAYNAME"].ToString()] = drdetails[0]["FLDHAVING"].ToString();

                }
                _datatable.Rows.Add(dr1.ItemArray);
            }

        }

        ViewState["dttable"] = _datatable;
        //ShowExcel(_datatable);
    }

    protected void ShowExcel()
    {
        DataTable _datatable = (DataTable)ViewState["dttable"];

        Response.AddHeader("Content-Disposition", "attachment; filename=ShellWeekly.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/Logo3.png" + "' /></td>");
        Response.Write("<td colspan='" + (_datatable.Columns.Count - 2).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp MARINE DEPARTMENT SSB/ SSPC <br/>&nbsp&nbsp&nbsp&nbsp VESSELS CREW - CHECKLIST FOR ENDORSEMENTS IN SAFETY & HEALTH PERSONAL PASSPORT</h3></td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + 2 + "'><h5>  Vessel Name / Vessel Type :  </h5></td>");
        Response.Write("<td colspan='" + 3 + "'><h5> " + txtVessel.Text +" </h5></td>");
        Response.Write("<td colspan='" + 2 + "'><h5>  Location :  </h5></td>");
        Response.Write("<td colspan='" + 1 + "'><h5> " + txtLocation.Text + " </h5></td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + 2 + "'><h5>  Owner :  </h5></td>");
        Response.Write("<td colspan='" + 3 + "'><h5> " + txtOwner.Text + " </h5></td>");
        Response.Write("<td colspan='" + 2 + "'><h5>  Date :  </h5></td>");
        Response.Write("<td colspan='" + 1 + "'><h5> " + txtDate.Text + " </h5></td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < _datatable.Columns.Count - 1; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + _datatable.Columns[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");

        foreach (DataRow dr in _datatable.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < _datatable.Columns.Count - 1; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[_datatable.Columns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Menushellmatrix_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOW"))
            {

                BindData();
                ViewState["EDIT"] = "1";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvshellmatrixView_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvshellmatrixView_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        DataSet ds = (DataSet)gv.DataSource;
        if (e.Item is GridDataItem)
        {
            if (drv.Row.Table.Columns.Count > 0)
            {
                string tooltip = string.Empty;
                string employeeid = drv["FLDEMPLOYEEID"].ToString();

                DataTable header = ds.Tables[1];
                DataTable dtemployee = ds.Tables[0];
                DataTable dtvalues = ds.Tables[2];

                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] drdetails = dtvalues.Select("FLDEMPLOYEEID = " + employeeid +
                                                " AND  FLDDOCHEADING = '" + header.Rows[i]["FLDDISPLAYNAME"].ToString() + "'");

                    Label lbldetails = new Label();
                    lbldetails.Visible = true;
                    lbldetails.Text = drdetails[0]["FLDHAVING"].ToString();

                    if (drdetails.Length > 0)
                    {
                        lbldetails.Text = drdetails[0]["FLDHAVING"].ToString();
                        //if(header.Rows[i]["FLDDISPLAYNAME"].ToString() == "Name")
                        //{
                        //    lbldetails.Width = Unit.Percentage(180);
                        //}

                    }

                    e.Item.Cells[i+3].Controls.Add(lbldetails);

                }
            }
        }
    }
}
