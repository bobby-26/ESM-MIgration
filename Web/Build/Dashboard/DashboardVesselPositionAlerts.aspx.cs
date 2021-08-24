using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class DashboardVesselPositionAlerts : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{

       
        SessionUtil.PageAccessRights(this.ViewState);
		try
		{
									
			if (!IsPostBack)
			{
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            }
			//BindMeasure();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindMeasure();
    }


    private void BindMeasure()
	{
        gvMeasure.ItemStyle.Height = Unit.Pixel(30);
        gvMeasure.AlternatingItemStyle.Height = Unit.Pixel(25);
        string vessellist = null;
        if (Request.QueryString["vesselid"] != null)
            vessellist = ","+Request.QueryString["vesselid"].ToString()+",";

        DataSet ds = PhoenixDashboardPerformance.DashboardVesselPositionAlert(null, vessellist);
        if (ds.Tables.Count > 0)
		{
            if (ds.Tables[1].Rows.Count > 0)
			{
				DataTable dt = ds.Tables[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GridBoundColumn field = new GridBoundColumn();
                    gvMeasure.MasterTableView.Columns.Add(field);
                    //field.HeaderText = dt.Rows[i]["FLDVESSELNAME"].ToString();
                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    field.HeaderStyle.Wrap = false;
                }
                gvMeasure.DataSource = ds;
            }
			else
			{
				if (ds.Tables[0].Rows.Count > 0)
				{
					gvMeasure.DataSource = ds;
					gvMeasure.DataBind();
				}
			}
		}
	}

    protected void gvMeasure_RowDataBound(Object sender, GridItemEventArgs e)
	{
		RadGrid gv = (RadGrid)sender;
		DataSet ds = (DataSet)gv.DataSource;
		DataRowView drv = (DataRowView)e.Item.DataItem;
		DataTable header = ds.Tables[1];
		DataTable data = ds.Tables[2];

		if (e.Item is GridHeaderItem)
		{
            for (int i = 0; i < header.Rows.Count; i++)
            {
                LinkButton lnk = new LinkButton();
                lnk.ID = "lnk_" + i.ToString();
                lnk.Text = header.Rows[i]["FLDVESSELNAME"].ToString();    //e.Item.Cells[i + 1].Text            
                lnk.CommandName = "VESSEL";
                //if (i != 1)
                //    lnk.Text = "";
                lnk.Attributes.Add("onclick", "javascript:openNewWindow('RangeConfig','','" + Session["sitepath"] + "/Registers/RegistersNoonReportRangeConfig.aspx?vesselid=" + header.Rows[i]["FLDVESSELID"].ToString() + "'); return false;");
                e.Item.Cells[i + 3].Controls.Add(lnk);
            }
            //if (header.Rows.Count < e.Item.Cells.Count - 1)
            //{
            //    for (int i = header.Rows.Count; i < e.Item.Cells.Count - 1; i++)
            //    {
            //        RadLabel lbl = new RadLabel();
            //        lbl.ID = "lbl_" + i.ToString();
            //        lbl.Text = "";
            //        e.Item.Cells[i + 1].Controls.Add(lbl);
            //        gvMeasure.MasterTableView.Columns[i + 1].Visible = false;
            //    }
            //}
        }
        if (e.Item is GridEditableItem)
		{
			string measurename = ((RadLabel)e.Item.FindControl("lblMeasureName")).Text;

            LinkButton ImgCauseExistsYN = (LinkButton)e.Item.FindControl("ImgCauseExistsYN");
            if (ImgCauseExistsYN != null)
            {
                ImgCauseExistsYN.Visible = General.GetNullableInteger( drv["FLDCAUSEEXISTSYN"].ToString()) >0 ? true : false;
                ImgCauseExistsYN.Attributes.Add("onclick", "javascript:openNewWindow('PossibelCause','','" + Session["sitepath"] + "/Registers/RegistersNoonReportRangeConfigPossibelCause.aspx?COLUMNNAME=" + drv["COLUMNNAME"].ToString() + "&DISPLAYTEXT=" + drv["MEASURENAME"].ToString() + "&LAUNCHFROM=1'); return false;");
            }

            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblMeasureName");
            if (lblmeasure != null && drv["MEASURENAME"].ToString() != "Date")
            {
                lblmeasure.Attributes.Add("onclick", "javascript:openNewWindow('PossibelCause','','" + Session["sitepath"] + "/Registers/RegistersNoonReportRangeConfigPossibelCause.aspx?COLUMNNAME=" + drv["COLUMNNAME"].ToString() + "&DISPLAYTEXT=" + drv["MEASURENAME"].ToString() + "&LAUNCHFROM=1'); return false;");
                lblmeasure.Attributes.Add("onmouseover", "this.style.cursor='Hand'");
            }

            if (measurename == "Date")
			{
				for (int i = 0; i < header.Rows.Count; i++)
				{
					DataRow[] dr1 = data.Select("FLDVESSELID = " + header.Rows[i]["FLDVESSELID"].ToString());
					if (dr1.Length > 0)
					{
                        RadLabel lbl = new RadLabel();
						lbl.ID = "lbl_" + i.ToString();
						lbl.Text = General.GetDateTimeToString(dr1[0]["FLDNOONREPORTDATE"].ToString());
						if (dr1[0]["FLDHIGHLIGHTDATE"].ToString() == "1")
							lbl.CssClass = "displayRed";
						else
							lbl.CssClass = "displayBlue";
                        e.Item.Cells[i + 3].Controls.Add(lbl);
					}
                    else
                    {
                        RadLabel lbl = new RadLabel();
                        lbl.ID = "lbl_" + i.ToString();
                        lbl.Text = "-";
                        e.Item.Cells[i + 3].Controls.Add(lbl);
                    }
                }
            }
            else if (measurename == "Vessel Status")
            {
                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] dr = data.Select("MEASURENAME = '" + measurename + "' AND FLDVESSELID = " + header.Rows[i]["FLDVESSELID"].ToString());
                    if (dr.Length > 0)
                    {
                        string[] str = new string[3];
                        RadLabel lbl = new RadLabel();
                        lbl.ID = "lbl_" + i.ToString();
                        lbl.Font.Bold = true;

                        if (dr[0]["FLDVALUE"].ToString() != "" || dr[0]["FLDVALUE"].ToString() != "|")
                        {
                            str = dr[0]["FLDVALUE"].ToString().Split('|');
                            if (str.Length > 1)
                            {
                                lbl.Text = str[0].Trim();
                                lbl.ToolTip= str[1].Trim();
                                e.Item.Cells[i + 3].Controls.Add(lbl);
                            }
                            else
                            {
                                lbl.Text = str[0].Trim();
                                e.Item.Cells[i + 3].Controls.Add(lbl);
                            }
                        }
                        else
                        {
                            lbl.Text = "";
                            e.Item.Cells[i + 3].Controls.Add(lbl);
                        }
                    }
                    else
                    {
                        RadLabel lbl = new RadLabel();
                        lbl.ID = "lbl_" + i.ToString();
                        lbl.Text = "";
                        e.Item.Cells[i + 3].Controls.Add(lbl);
                        e.Item.Visible = false;
                    }
                }

            }
            else
			{
				for (int i = 0; i < header.Rows.Count; i++)
				{

					DataRow[] dr = data.Select("MEASURENAME = '" + measurename + "' AND FLDVESSELID = " + header.Rows[i]["FLDVESSELID"].ToString());
					if (dr.Length > 0)
					{
						RadLabel lbl = new RadLabel();
						lbl.ID = "lbl_" + i.ToString();
                        lbl.Attributes.Add("onclick", "javascript:openNewWindow('FineFilter','','" + Session["sitepath"] + "/Dashboard/DashboardVesselPositionAlertFineFilter.aspx?vesselid=" + dr[0]["FLDVESSELID"].ToString() + "&measurename=" + dr[0]["FLDCOLUMNNAME"].ToString() + "&vesselname=" + dr[0]["FLDVESSELNAME"].ToString() + "&fromdate=" + DateTime.Parse(dr[0]["FLDNOONREPORTDATE"].ToString()).AddMonths(-1).ToString() + "&todate=" + dr[0]["FLDNOONREPORTDATE"].ToString() + "'); return false;");
                        lbl.Attributes.Add("onmouseover", "this.style.cursor='Hand'");

                        if (dr[0]["FLDALERTYN"].ToString() == "1" && dr[0]["FLDVALUE"].ToString() != "" && dr[0]["FLDACTIVEYN"].ToString() == "1")
						{
							lbl.Text = dr[0]["FLDVALUE"].ToString();
							lbl.CssClass = "lossVal";
                            e.Item.Cells[i + 3].Controls.Add(lbl);
                            e.Item.Cells[i + 3].Attributes.Add("style", "white-space: nowrap;");

                        }
						else if (dr[0]["FLDALERTYN"].ToString() == "0" && dr[0]["FLDVALUE"].ToString() != "" && dr[0]["FLDACTIVEYN"].ToString() == "1")
						{
							lbl.Text = dr[0]["FLDVALUE"].ToString();
							lbl.CssClass = "gainVal";
                            e.Item.Cells[i + 3].Controls.Add(lbl);
                            e.Item.Cells[i + 3].Attributes.Add("style", "white-space: nowrap;");
                        }
						else if(dr[0]["FLDACTIVEYN"].ToString() == "1")

                        {
							lbl.Text = dr[0]["FLDVALUE"].ToString() != "" ? dr[0]["FLDVALUE"].ToString() : "-";
							lbl.CssClass = "noVal";
                            e.Item.Cells[i + 3].Controls.Add(lbl);
                            e.Item.Cells[i + 3].Attributes.Add("style", "white-space: nowrap;");
                        }
                        else
                        {
                            lbl.Text = "";
                            e.Item.Cells[i + 3].Controls.Add(lbl);
                        }


					}
                    else
                    {
                        RadLabel lbl = new RadLabel();
                        lbl.ID = "lbl_" + i.ToString();
                        lbl.Text = "";
                        e.Item.Cells[i + 3].Controls.Add(lbl);
                    }

				}
                
			}


		}
	}


    protected void gvMeasure_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindMeasure();
    }
}
