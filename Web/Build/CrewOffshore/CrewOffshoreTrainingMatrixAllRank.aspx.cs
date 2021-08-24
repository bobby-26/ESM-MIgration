using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Southnests.Phoenix;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Data.SqlClient;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;
using System.Text;

public partial class CrewOffshore_CrewOffshoreTrainingMatrixAllRank : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ucCharterer.AddressType = ((int)PhoenixAddressType.CHARTERER).ToString();
            ucManager.AddressType = ((int)PhoenixAddressType.MANAGER).ToString();
            //  ucConfirm.Visible = false;
            confirm.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingMatrixAllRank.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingMatrixAllRank.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CrewTrainingMenu.AccessRights = this.ViewState;
            CrewTrainingMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvmatrixView_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvmatrixView_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        DataSet ds = (DataSet)gv.DataSource;
        if (e.Item is GridDataItem)
        {
            if (ds.Tables[2].Columns.Count > 0)
            {
                string tooltip = string.Empty;
                string docid = ((Label)e.Item.FindControl("lbldocid")).Text;// drv["FLDDOCUMENTID"].ToString();
                string doctype = ((Label)e.Item.FindControl("lbldoctype")).Text;// drv["FLDDOCUMENTID"].ToString();
                DataTable header = ds.Tables[1];
                DataTable dtdocs = ds.Tables[0];
                DataTable dtvalues = ds.Tables[2];

                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] drdetails = dtvalues.Select("FLDDOCUMENTID = " + docid +
                                                " AND FLDDOCMENTTYPE = " + doctype +
                                                " AND  FLDRANKHEADING = '" + header.Rows[i]["FLDHEADINGNAME"].ToString() + "'");

                    RadCheckBox chkdetails = new RadCheckBox();
                    Label lblTQMatrix = new Label();
                    Label lblrankid = new Label();
                    chkdetails.Visible = true;
                    if (Convert.ToInt16(drdetails[0]["FLDHAVING"].ToString()) > 0)
                        chkdetails.Checked = true;//.Text = drdetails[0]["FLDHAVING"].ToString();                    
                    else
                        chkdetails.Checked = false;

                    chkdetails.AutoPostBack = true;
                    chkdetails.CommandName = "SELECT";
                    chkdetails.CheckedChanged += new EventHandler(ck_CheckedChanged);
                    //chkdetails.Attributes.Add("onchange", "submitit(this);");

                    lblTQMatrix.Text = drdetails[0]["FLDTQID"].ToString();
                    lblrankid.Text = drdetails[0]["FLDRANKID"].ToString();



                    e.Item.Cells[i + 4].Attributes.Add("lblTQMatrix", lblTQMatrix.Text);
                    e.Item.Cells[i + 4].Controls.Add(chkdetails);
                    e.Item.Cells[i + 4].Attributes.Add("lblrankid", lblrankid.Text);

                }
            }
        }
    }
    private void BindData()
    {
        //int year, month;
        DataSet ds = null;


        try
        {


            ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixAllRankList(General.GetNullableInteger(ucVesselType.SelectedVesseltype)
                    , General.GetNullableInteger(ucCharterer.SelectedAddress));
            if (gvmatrixView.Columns.Count < ds.Tables[1].Rows.Count)
            {
                if (ds.Tables.Count > 0)
                {


                    DataTable dt = ds.Tables[1];
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        //adding columns dynamically

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            GridBoundColumn field = new GridBoundColumn();
                            field.HeaderStyle.Width = 65;
                            field.HeaderText = General.GetNullableString(dt.Rows[i]["FLDHEADINGNAME"].ToString());
                            field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;

                            gvmatrixView.Columns.Insert(gvmatrixView.Columns.Count, field);
                        }
                    }
                    gvmatrixView.DataSource = ds;

                    ViewState["EDIT"] = "1";
                }
                else
                {
                    gvmatrixView.DataSource = ds;
                }

                
            }
            else
            {
                gvmatrixView.DataSource = ds;
               
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindDataExp()
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixAllRankExpList(General.GetNullableInteger(ucVesselType.SelectedVesseltype)
                    , General.GetNullableInteger(ucCharterer.SelectedAddress));
            gvmatrixexp.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void setFilter(object sender, EventArgs e)
    {
        ViewState["vsltype"] = ucVesselType.SelectedVesseltype;
        //ViewState["rank"] = ucRank.SelectedRank;
        ViewState["charterer"] = ucCharterer.SelectedAddress;
        gvmatrixView.Rebind();
    }

    protected void CrewTrainingMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("ADDMATRIX"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingMatrixEdit.aspx");
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvmatrixView.Rebind();
                gvmatrixexp.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                ucVesselType.SelectedVesseltype = "";
                //ucRank.SelectedRank = "";
                ucCharterer.SelectedAddress = "";

                gvmatrixView.Rebind();
                gvmatrixexp.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            //UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            //if (ucCM.confirmboxvalue == 1)
            //{
            if (ViewState["MATRIXID"] != null && ViewState["MATRIXID"].ToString() != "")
            {
                PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixCopy(int.Parse(ViewState["MATRIXID"].ToString()));
                ucStatus.Text = "Copied Successfully";
                gvmatrixView.Rebind();

            }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ck_CheckedChanged(object sender, EventArgs e)
    {

        string cbID = ((RadCheckBox)sender).ClientID;
        ViewState["CHECKED"] = ((RadCheckBox)sender).Checked;

    }

    protected void gvmatrixView_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            if (e.CommandName.ToUpper() == "SELECT")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string idcode = ((Label)e.Item.FindControl("lblcode")).Text;
                string lbldocid = ((Label)e.Item.FindControl("lbldocid")).Text;
                string rankid = ((GridTableCell)(((RadCheckBox)e.CommandSource).Parent)).Attributes["lblrankid"];
                string tqid = ((GridTableCell)(((RadCheckBox)e.CommandSource).Parent)).Attributes["lblTQMatrix"];
                DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(General.GetNullableInteger(ucVesselType.SelectedVesseltype)
                                                                                , General.GetNullableInteger(rankid)
                                                                                , General.GetNullableInteger(ucCharterer.SelectedAddress));
                if (dt.Rows.Count > 0)
                {
                    string strListcoc;
                    string strListstcw;
                    string strListotherdoc;
                    string strListMedical;


                    strListcoc = dt.Rows[0]["FLDCOCLIST"].ToString();
                    strListstcw = dt.Rows[0]["FLDSTCWLIST"].ToString();
                    strListotherdoc = dt.Rows[0]["FLDOTHERDOCLIST"].ToString();
                    strListMedical = dt.Rows[0]["FLDMEDICALDOCSLIST"].ToString();



                    if (((bool)ViewState["CHECKED"]) == true)
                    {
                        if (idcode == "COC")
                            strListcoc = strListcoc + "," + lbldocid;
                        if (idcode == "STCW")
                            strListstcw = strListstcw + "," + lbldocid;
                        if (idcode == "OTHER")
                            strListotherdoc = strListotherdoc + "," + lbldocid;
                        if (idcode == "MED")
                            strListMedical = strListMedical + "," + lbldocid;
                    }

                    if (((bool)ViewState["CHECKED"]) == false)
                    {
                        string oldtext = "," + lbldocid + ",";

                        if (idcode == "COC")
                            strListcoc = strListcoc.Replace(oldtext, ",");
                        if (idcode == "STCW")
                            strListstcw = strListstcw.Replace(oldtext, ",");
                        if (idcode == "OTHER")
                            strListotherdoc = strListotherdoc.Replace(oldtext, ",");
                        if (idcode == "MED")
                            strListMedical = strListMedical.Replace(oldtext, ",");
                    }

                    PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixUpdate(int.Parse(dt.Rows[0]["FLDMATRIXID"].ToString())
                     , int.Parse(ucVesselType.SelectedVesseltype)
                     , int.Parse(ucCharterer.SelectedAddress)
                     , int.Parse(rankid)
                     , int.Parse(dt.Rows[0]["FLDMANAGEMENTCOMPANYID"].ToString())
                     , General.GetNullableString(strListcoc.ToString().Replace(",,", ","))
                     , General.GetNullableString(strListstcw.ToString().Replace(",,", ","))
                     , General.GetNullableString(strListotherdoc.ToString().Replace(",,", ","))
                     , null
                     , dt.Rows[0]["FLDEXPERIENCEINRANKLIST"].ToString()
                     , General.GetNullableInteger(dt.Rows[0]["FLDRANKEXPERIENCE"].ToString())
                     , dt.Rows[0]["FLDEXPERIENCEVESSELTYPELIST"].ToString()
                     , General.GetNullableInteger(dt.Rows[0]["FLDVESSELTYPEEXPERIENCE"].ToString())
                     , null
                     , null
                     , null
                     , null
                     , 0
                     , 0
                     , General.GetNullableString(strListMedical.ToString().Replace(",,", ","))
                     );
                }
                else
                {
                    string strListcoc;
                    string strListstcw;
                    string strListotherdoc;
                    string strListMedical;

                    int? matrixid = null;

                    strListcoc = "";
                    strListstcw = "";
                    strListotherdoc = "";
                    strListMedical = "";

                    if (idcode == "COC")
                        strListcoc = "," + lbldocid + ",";
                    if (idcode == "STCW")
                        strListstcw = "," + lbldocid + ",";
                    if (idcode == "OTHER")
                        strListotherdoc = "," + lbldocid + ",";
                    if (idcode == "MED")
                        strListMedical = "," + lbldocid + ",";

                    if (!IsValidMatrix())
                    {
                        ucError.Visible = true;
                        return;
                    }


                    PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixInsert(int.Parse(ucVesselType.SelectedVesseltype)
                   , int.Parse(ucCharterer.SelectedAddress)
                   , int.Parse(rankid)
                   , int.Parse(ucManager.SelectedAddress)
                   , General.GetNullableString(strListcoc.ToString().Replace(",,", ","))
                   , General.GetNullableString(strListstcw.ToString().Replace(",,", ","))
                   , General.GetNullableString(strListotherdoc.ToString().Replace(",,", ","))
                   , null
                   , null
                   , null
                   , null
                   , null
                   , null
                   , null
                   , null
                   , null
                   , ref matrixid
                   , 0
                   , 0
                   , General.GetNullableString(strListMedical.ToString().Replace(",,", ","))
                   );

                }
                BindData();
                gvmatrixView.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMatrix()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVesselType.SelectedVesseltype) == null)
            ucError.ErrorMessage = "Vessel Type is required.";


        if (General.GetNullableInteger(ucCharterer.SelectedAddress) == null)
            ucError.ErrorMessage = "Charterer is required.";

        if (General.GetNullableInteger(ucManager.SelectedAddress) == null)
            ucError.ErrorMessage = "Management company is required.";

        return (!ucError.IsError);
    }

    protected void gvmatrixexp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataExp();
    }

    protected void gvmatrixexp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "ADD")
        {
            RadComboBox ddlRank = (RadComboBox)e.Item.FindControl("ddlRank");
            RadComboBox ddlvesseltypeselect = (RadComboBox)e.Item.FindControl("ddlvesseltypeselect");
            RadComboBox ddlRankselect = (RadComboBox)e.Item.FindControl("ddlRankselect");

            RadTextBox txtrankexp = (RadTextBox)e.Item.FindControl("txtrankexp");
            RadTextBox txtvesseltypeexp = (RadTextBox)e.Item.FindControl("txtvesseltypeexp");

            DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(General.GetNullableInteger(ucVesselType.SelectedVesseltype)
                                                                              , General.GetNullableInteger(ddlRank.SelectedValue.ToString())
                                                                              , General.GetNullableInteger(ucCharterer.SelectedAddress));

            if (dt.Rows.Count > 0)
            {
                PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixUpdate(int.Parse(dt.Rows[0]["FLDMATRIXID"].ToString())
               , int.Parse(ucVesselType.SelectedVesseltype)
               , int.Parse(ucCharterer.SelectedAddress)
               , int.Parse(dt.Rows[0]["FLDRANKID"].ToString())
               , int.Parse(dt.Rows[0]["FLDMANAGEMENTCOMPANYID"].ToString())
               , General.GetNullableString(dt.Rows[0]["FLDCOCLIST"].ToString())
               , General.GetNullableString(dt.Rows[0]["FLDSTCWLIST"].ToString())
               , General.GetNullableString(dt.Rows[0]["FLDOTHERDOCLIST"].ToString())
               , null
               , General.GetNullableString(GetCsvValue(ddlRankselect))
               , General.GetNullableInteger(txtrankexp.Text)
               , General.GetNullableString(GetCsvValue(ddlvesseltypeselect))
               , General.GetNullableInteger(txtvesseltypeexp.Text)
               , null
               , null
               , null
               , null
               , 0
               , 0
               , General.GetNullableString(dt.Rows[0]["FLDMEDICALDOCSLIST"].ToString())
               );
                gvmatrixexp.Rebind();
            }
            else
            {
                ucError.ErrorMessage = "Select the documents for the rank";
                ucError.Visible = false;
                return;
            }

        }
        if (e.CommandName.ToUpper() == "SAVE")
        {
            Label lblrankidedit = (Label)e.Item.FindControl("lblrankidedit");
            RadComboBox ddlvesseltypeselect = (RadComboBox)e.Item.FindControl("ddlvesseltypeselectedit");
            RadComboBox ddlRankselect = (RadComboBox)e.Item.FindControl("ddlRankselectedit");

            RadTextBox txtrankexp = (RadTextBox)e.Item.FindControl("txtrankexpedit");
            RadTextBox txtvesseltypeexp = (RadTextBox)e.Item.FindControl("txtvesseltypeexpedit");
            DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(General.GetNullableInteger(ucVesselType.SelectedVesseltype)
                                                                            , General.GetNullableInteger(lblrankidedit.Text)
                                                                            , General.GetNullableInteger(ucCharterer.SelectedAddress));

            if (dt.Rows.Count > 0)
            {

                PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixUpdate(int.Parse(dt.Rows[0]["FLDMATRIXID"].ToString())
               , int.Parse(ucVesselType.SelectedVesseltype)
               , int.Parse(ucCharterer.SelectedAddress)
               , int.Parse(dt.Rows[0]["FLDRANKID"].ToString())
               , int.Parse(dt.Rows[0]["FLDMANAGEMENTCOMPANYID"].ToString())
               , General.GetNullableString(dt.Rows[0]["FLDCOCLIST"].ToString())
               , General.GetNullableString(dt.Rows[0]["FLDSTCWLIST"].ToString())
               , General.GetNullableString(dt.Rows[0]["FLDOTHERDOCLIST"].ToString())
               , null
               , General.GetNullableString(GetCsvValue(ddlRankselect))
               , General.GetNullableInteger(txtrankexp.Text)
               , General.GetNullableString(GetCsvValue(ddlvesseltypeselect))
               , General.GetNullableInteger(txtvesseltypeexp.Text)
               , null
               , null
               , null
               , null
               , 0
               , 0
               , General.GetNullableString(dt.Rows[0]["FLDMEDICALDOCSLIST"].ToString())
               );
            }
            gvmatrixexp.Rebind();
        }

    }

    protected void gvmatrixexp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlRankselect = (RadComboBox)e.Item.FindControl("ddlRankselect");
            DataSet ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixRankList(0, null);
            ddlRankselect.DataSource = ds;
            ddlRankselect.DataBind();

            RadComboBox ddlRank = (RadComboBox)e.Item.FindControl("ddlRank");
            ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixRankList(0, null);
            ddlRank.DataSource = ds;
            ddlRank.DataBind();

            RadComboBox ddlvesseltypeselect = (RadComboBox)e.Item.FindControl("ddlvesseltypeselect");
            DataSet ds1 = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixVesselTypeList(0, null);
            ddlvesseltypeselect.DataSource = ds1;
            ddlvesseltypeselect.DataBind();



        }
        if (e.Item is GridDataItem)
        {

            RadComboBox ddlRankselectedit = (RadComboBox)e.Item.FindControl("ddlRankselectedit");
            if (ddlRankselectedit != null)
            {
                DataSet ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixRankList(0, null);
                ddlRankselectedit.DataSource = ds;
                ddlRankselectedit.DataBind();

                RadLabel lblranklistedit = (RadLabel)e.Item.FindControl("lblranklistedit");
                SetCsvValue(ddlRankselectedit, lblranklistedit.Text);
            }
            RadComboBox ddlvesseltypeselectedit = (RadComboBox)e.Item.FindControl("ddlvesseltypeselectedit");
            if (ddlvesseltypeselectedit != null)
            {
                DataSet ds = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixVesselTypeList(0, null);
                ddlvesseltypeselectedit.DataSource = ds;
                ddlvesseltypeselectedit.DataBind();

                RadLabel lbltypelistedit = (RadLabel)e.Item.FindControl("lbltypelistedit");
                SetCsvValue(ddlvesseltypeselectedit, lbltypelistedit.Text);
            }


        }
    }

    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }



}