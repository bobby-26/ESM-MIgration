using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewOffshore_CrewOffshoreReportCrewListFormatOSVISPetronasMatrix : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        if (!IsPostBack)
        {
            if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"] != string.Empty)
            {
                ViewState["VesselName"] = Request.QueryString["Vesselname"];
                ViewState["VESSELID"] = Request.QueryString["VESSELID"];
              

            }
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreReportCrewListFormatOSVISPetronasMatrix.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        MenuShellMatrixWeekly.AccessRights = this.ViewState;
        MenuShellMatrixWeekly.MenuList = toolbar.Show();

        ViewState["EDIT"] = "0";
        ViewState["EDITENGINE"] = "0";
        BindVesselDetails();
        // BindData();
        GetTable();
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
    protected void MenuOSVISClewList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

    }
    private void BindData()
    {
        //int year, month;
        DataSet ds = null;
        string vesselid = null;

        try
        {
            if (Request.QueryString["VESSELID"] != null)
                vesselid = Request.QueryString["VESSELID"].ToString();

            ds = PhoenixCrewOffshoreCrewList.SearchVesselPetronasCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 2);

            if (ds.Tables.Count > 0)
            {


                DataTable dt = ds.Tables[2];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //adding columns dynamically
                    if (ViewState["EDIT"].ToString() != "1")
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            GridBoundColumn field = new GridBoundColumn();

                            field.HeaderText = General.GetNullableString(dt.Rows[i]["FLDRANKNAMES"].ToString());
                            field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                            

                            gvshellmatrixView.MasterTableView.Columns.Add(field);

                        }
                    }
                    //DataSet dsrows = new DataSet();
                    //dsrows.(ds.Tables[1]);
                    gvshellmatrixView.DataSource = ds;
                    // gvshellmatrixView.DataBind();
                    ViewState["EDIT"] = "1";
                }
                else
                {
                    gvShellNoRecordFound.DataSource = ds;
                    gvShellNoRecordFound.Visible = true;
                }
                dt = ds.Tables[3];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //adding columns dynamically
                    if (ViewState["EDITENGINE"].ToString() != "1")
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            GridBoundColumn field = new GridBoundColumn();
                            field.HeaderText = General.GetNullableString(dt.Rows[i]["FLDRANKNAMES"].ToString());
                            field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;

                            gvshellmatrixViewEngine.Columns.Insert(gvshellmatrixViewEngine.Columns.Count, field);
                        }
                    }
                    //DataSet dsrows = new DataSet();
                    //dsrows.(ds.Tables[1]);
                    gvshellmatrixViewEngine.DataSource = ds;
                   
                    ViewState["EDITENGINE"] = "1";
                }
                else
                {
                    gvShellNoRecordFound.DataSource = ds;
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
    protected void ShowExcel()
    {
        DataTable _datatable = (DataTable)ViewState["dttable"];
        DataTable _datatableengine = (DataTable)ViewState["dttableengine"];


        Response.AddHeader("Content-Disposition", "attachment; filename=PetronasMatrix.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        //Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/Logo3.png" + "' /></td>");
        Response.Write("<td colspan='" + (_datatable.Columns.Count - 2).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp <br/>&nbsp&nbsp&nbsp&nbsp VESSELS CREW - PETRONAS MATRIX</h3></td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + 2 + "'><h5>  Vessel Name / Vessel Type :  </h5></td>");
        Response.Write("<td colspan='" + 3 + "'><h5> " + txtVessel.Text + " </h5></td>");
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

        Response.Write("<tr>");
        Response.Write("<td colspan='" + 2 + "'><h5> Deck Department  </h5></td>");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        //Response.Write("<tr>");
        //for (int i = 0; i < _datatable.Columns.Count - 1; i++)
        //{
        //    Response.Write("<td width='20%'>");
        //    Response.Write("<b>" + _datatable.Columns[i] + "</b>");
        //    Response.Write("</td>");
        //}
        //Response.Write("</tr>");

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
        Response.Write("<br />");

        Response.Write("<tr>");
        Response.Write("<td colspan='" + 2 + "'><h5> Engine Department  </h5></td>");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        //Response.Write("<tr>");
        //for (int i = 0; i < _datatableengine.Columns.Count - 1; i++)
        //{
        //    Response.Write("<td width='20%'>");
        //    Response.Write("<b>" + _datatableengine.Columns[i] + "</b>");
        //    Response.Write("</td>");
        //}
        //Response.Write("</tr>");
        foreach (DataRow dr in _datatableengine.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < _datatableengine.Columns.Count - 1; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[_datatableengine.Columns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }

        Response.End();

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

   
    public void GetTable()
    {
        //DECK DEPARTPMENT
        DataTable _datatable = new DataTable();
        DataSet ds = new DataSet();



        ds = PhoenixCrewOffshoreCrewList.SearchVesselPetronasCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 2);
        _datatable.Columns.Add("RANK");
        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
        {
            _datatable.Columns.Add(ds.Tables[2].Rows[i]["FLDRANKNAME"].ToString());
        }
        _datatable.Columns.Add("FLDEMPLOYEEID");

        if (ds.Tables.Count > 0)
        {

            DataTable header = ds.Tables[2];
            DataTable dtemployee = ds.Tables[0];
            DataTable dtvalues = ds.Tables[1];

            DataRow dr1 = _datatable.NewRow();
            for (int i = 0; i < 1; i++)
            {
                dr1 = _datatable.NewRow();
                dr1["RANK"] = "Rank";
                for (int j = 0; j < header.Rows.Count; j++)
                {
                    dr1[header.Rows[j]["FLDRANKNAME"].ToString()] = header.Rows[j]["FLDRANKNAMES"].ToString();
                }
                _datatable.Rows.Add(dr1.ItemArray);
            }
            for (int i = 0; i < dtemployee.Rows.Count; i++)
            {
                dr1 = _datatable.NewRow();

                dr1["RANK"] = dtemployee.Rows[i]["FLDDISPLAYNAME"].ToString();
                //dr1["File No."] = dtemployee.Rows[i]["FLDFILENO"].ToString();
                //_datatable.Rows.Add(dr1.ItemArray);
                string heading = dtemployee.Rows[i]["FLDDISPLAYNAME"].ToString();

                for (int j = 0; j < header.Rows.Count; j++)
                {

                    DataRow[] drdetails = dtvalues.Select("FLDEMPLOYEEID = " + header.Rows[j]["FLDEMPLOYEEID"].ToString() +
                                               " AND  FLDDOCHEADING = '" + heading + "'");



                    dr1[header.Rows[j]["FLDRANKNAME"].ToString()] = drdetails[0]["FLDHAVING"].ToString();

                }
                _datatable.Rows.Add(dr1.ItemArray);
            }

        }

        ViewState["dttable"] = _datatable;

        //ENGINE DEPARTPMENT
        DataTable _datatableengine = new DataTable();
        DataSet dsengine = new DataSet();

        //for (int i = 0; i < gvshellmatrixViewEngine.Columns.Count; i++)
        //{
        //    _datatableengine.Columns.Add(gvshellmatrixViewEngine.Columns[i].ToString());
        //}
        //_datatableengine.Columns.Add("FLDEMPLOYEEID");

        dsengine = PhoenixCrewOffshoreCrewList.SearchVesselPetronasCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString()), 2);
        _datatableengine.Columns.Add("RANK");
        for (int i = 0; i < dsengine.Tables[3].Rows.Count; i++)
        {
            _datatableengine.Columns.Add(dsengine.Tables[3].Rows[i]["FLDRANKNAME"].ToString());
        }
        _datatableengine.Columns.Add("FLDEMPLOYEEID");
        if (dsengine.Tables.Count > 0)
        {

            DataTable header = dsengine.Tables[3];
            DataTable dtemployee = dsengine.Tables[0];
            DataTable dtvalues = dsengine.Tables[1];

            DataRow dr1 = _datatableengine.NewRow();
            for (int i = 0; i < 1; i++)
            {
                dr1 = _datatableengine.NewRow();
                dr1["RANK"] = "Rank";
                for (int j = 0; j < header.Rows.Count; j++)
                {
                    dr1[header.Rows[j]["FLDRANKNAME"].ToString()] = header.Rows[j]["FLDRANKNAMES"].ToString();
                }
                _datatableengine.Rows.Add(dr1.ItemArray);
            }
            for (int i = 0; i < dtemployee.Rows.Count; i++)
            {
                dr1 = _datatableengine.NewRow();
                dr1["RANK"] = dtemployee.Rows[i]["FLDDISPLAYNAME"].ToString();
                //dr1["File No."] = dtemployee.Rows[i]["FLDFILENO"].ToString();
                //_datatable.Rows.Add(dr1.ItemArray);
                string heading = dtemployee.Rows[i]["FLDDISPLAYNAME"].ToString();

                for (int j = 0; j < header.Rows.Count; j++)
                {

                    DataRow[] drdetails = dtvalues.Select("FLDEMPLOYEEID = " + header.Rows[j]["FLDEMPLOYEEID"].ToString() +
                                               " AND  FLDDOCHEADING = '" + heading + "'");


                    dr1[header.Rows[j]["FLDRANKNAME"].ToString()] = drdetails[0]["FLDHAVING"].ToString();

                }
                _datatableengine.Rows.Add(dr1.ItemArray);
            }

        }

        ViewState["dttableengine"] = _datatableengine;
        //ShowExcel(_datatable);
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
                ViewState["EDITENGINE"] = "1";
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

    protected void gvshellmatrixView_ItemCreated(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        DataSet ds = (DataSet)gv.DataSource;
        if (e.Item is GridDataItem)
        {
            if (drv.Row.Table.Columns.Count > 0)
            {
                string tooltip = string.Empty;
                string heading = drv["FLDDISPLAYNAME"].ToString();

                DataTable header = ds.Tables[2];
                DataTable dtemployee = ds.Tables[0];
                DataTable dtvalues = ds.Tables[1];

                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] drdetails = dtvalues.Select("FLDDOCHEADING = '" + heading +
                                                "' AND  FLDEMPLOYEEID = '" + header.Rows[i]["FLDEMPLOYEEID"].ToString() + "'");

                    RadLabel lbldetails = new RadLabel();
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
                    GridBoundColumn boundColumn = new GridBoundColumn();
                    boundColumn.DataField = "FLDHAVING";
                    //boundColumn.DataField = ds2.Tables[0].Rows[0]["PresentAmps"].ToString();
                    //e.Item. Cells[i + 2].Controls.Add(lbldetails);

                }
            }
        }
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
                string heading = drv["FLDDISPLAYNAME"].ToString();

                DataTable header = ds.Tables[2];
                DataTable dtemployee = ds.Tables[0];
                DataTable dtvalues = ds.Tables[1];

                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] drdetails = dtvalues.Select("FLDDOCHEADING = '" + heading +
                                                "' AND  FLDEMPLOYEEID = '" + header.Rows[i]["FLDEMPLOYEEID"].ToString() + "'");

                    RadLabel lbldetails = new RadLabel();
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
                    //GridBoundColumn boundColumn = new GridBoundColumn();
                    //boundColumn.DataField = "FLDHAVING";
                    //boundColumn.DataField = ds2.Tables[0].Rows[0]["PresentAmps"].ToString();
                    e.Item.Cells[i + 3].Controls.Add(lbldetails);

                }
            }
        }
    }

    protected void gvshellmatrixViewEngine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        DataSet ds = (DataSet)gv.DataSource;
        if(e.Item is GridDataItem)
        {
            if (drv.Row.Table.Columns.Count > 0)
            {
                string tooltip = string.Empty;
                string heading = drv["FLDDISPLAYNAME"].ToString();

                DataTable header = ds.Tables[3];
                DataTable dtemployee = ds.Tables[0];
                DataTable dtvalues = ds.Tables[1];

                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] drdetails = dtvalues.Select("FLDDOCHEADING = '" + heading +
                                                "' AND  FLDEMPLOYEEID = '" + header.Rows[i]["FLDEMPLOYEEID"].ToString() + "'");

                    RadLabel lbldetails = new RadLabel();
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

                    e.Item.Cells[i + 3].Controls.Add(lbldetails);

                }
            }
        }
    }
}
